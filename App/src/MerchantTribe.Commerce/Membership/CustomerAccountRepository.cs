using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MerchantTribe.Web.Data;
using MerchantTribe.Web.Logging;

namespace MerchantTribe.Commerce.Membership
{
    public class CustomerAccountRepository: ConvertingRepositoryBase<Data.EF.bvc_User, CustomerAccount>
    {
        private RequestContext context = null;

        public static CustomerAccountRepository InstantiateForMemory(RequestContext c)
        {
            return new CustomerAccountRepository(c, new MemoryStrategy<Data.EF.bvc_User>(PrimaryKeyType.Bvin),                                           
                                           new TextLogger());
        }
        public static CustomerAccountRepository InstantiateForDatabase(RequestContext c)
        {
            CustomerAccountRepository result = null;
            result = new CustomerAccountRepository(c,
                new EntityFrameworkRepository<Data.EF.bvc_User>(
                    new Data.EF.EntityFrameworkDevConnectionString(c.ConnectionStringForEntityFramework)),
                    new SupressLogger()
                    );
            return result;
        }
        public CustomerAccountRepository(RequestContext c, IRepositoryStrategy<Data.EF.bvc_User> r,                                    
                                    ILogger log)
        {
            context = c;
            repository = r;
            this.logger = log;
            repository.Logger = this.logger;
        }

        protected override void CopyDataToModel(Data.EF.bvc_User data, CustomerAccount model)
        {
            model.Bvin = data.bvin;            
            model.StoreId = data.StoreId;
            model.LastUpdatedUtc = data.LastUpdated;
            model.Addresses = Contacts.AddressList.FromJson(data.AddressBook);
            model.CreationDateUtc = data.CreationDate;
            model.Email = data.Email;
            model.FailedLoginCount = data.FailedLoginCount;
            model.FirstName = data.FirstName;
            model.LastLoginDateUtc = data.LastLoginDate;
            model.LastName = data.LastName;
            model.Locked = data.Locked == 1;
            model.LockedUntilUtc = data.LockedUntil;
            model.Notes = data.Comment;
            model.Password = data.Password;
            model.Phones = Contacts.PhoneNumberList.FromJson(data.Phones);
            model.PricingGroupId = data.PricingGroup;
            model.Salt = data.Salt;
            model.TaxExempt = data.TaxExempt == 1;
            
            Contacts.Address shipAddr = MerchantTribe.Web.Json.ObjectFromJson<Contacts.Address>(data.ShippingAddress);
            model.ShippingAddress = shipAddr ?? new Contacts.Address();

            Contacts.Address billAddr = MerchantTribe.Web.Json.ObjectFromJson<Contacts.Address>(data.BillingAddress);
            model.BillingAddress = billAddr ?? new Contacts.Address();
        }
        protected override void CopyModelToData(Data.EF.bvc_User data, CustomerAccount model)
        {
            data.bvin = model.Bvin;
            data.StoreId = model.StoreId;
            data.LastUpdated = model.LastUpdatedUtc;
            data.AddressBook = model.Addresses.ToJson();
            data.CreationDate = model.CreationDateUtc;
            data.Email = model.Email;
            data.FailedLoginCount = model.FailedLoginCount;
            data.FirstName = model.FirstName;
            data.LastLoginDate = model.LastLoginDateUtc;
            data.LastName = model.LastName;
            data.Locked = model.Locked ? 1 : 0;
            data.LockedUntil = model.LockedUntilUtc;
            data.Comment = model.Notes;
            data.Password = model.Password;
            data.Phones = model.Phones.ToJson();
            data.PricingGroup = model.PricingGroupId;
            data.Salt = model.Salt;
            data.TaxExempt = model.TaxExempt ? 1 : 0;
            data.CustomQuestionAnswers = string.Empty; // To Be Remove Soon
            data.ShippingAddress = MerchantTribe.Web.Json.ObjectToJson(model.ShippingAddress);
            data.BillingAddress = MerchantTribe.Web.Json.ObjectToJson(model.BillingAddress);
        }
      
        public CustomerAccount Find(string bvin)
        {
            CustomerAccount result = FindForAllStores(bvin);
            if (result != null)
            {
                if (result.StoreId == context.CurrentStore.Id)
                {
                    return result;
                }
            }
            return null;
        }
        public CustomerAccount FindForAllStores(string bvin)
        {
            return this.Find(new PrimaryKey(bvin));
        }
        public override bool Create(CustomerAccount item)
        {
            if (item.Bvin == string.Empty)
            {
                item.Bvin = System.Guid.NewGuid().ToString();
            }
            item.StoreId = context.CurrentStore.Id;
            item.LastUpdatedUtc = DateTime.UtcNow;
 	        bool result = base.Create(item);            
            if (result) context.IntegrationEvents.CustomerAccountCreated(item);
            return result;
        }
        public bool Update(CustomerAccount c)
        {
            if (c.StoreId != context.CurrentStore.Id)
            {
                return false;
            }
            c.LastUpdatedUtc = DateTime.UtcNow;
            bool result = this.Update(c, new PrimaryKey(c.Bvin));
            if (result) context.IntegrationEvents.CustomerAccountUpdated(c);
            return result;
        }
        public bool Delete(string bvin)
        {
            long storeId = context.CurrentStore.Id;
            CustomerAccount item = Find(bvin);
            if (item == null) return false;            
           bool result = Delete(new PrimaryKey(bvin));
           if (result) context.IntegrationEvents.CustomerAccountDeleted(item);
           return result;
        }
        public List<CustomerAccount> FindAll()
        {
            int totalCount = 0;
            return FindAllPaged(1, int.MaxValue, ref totalCount);
        }
        public List<CustomerAccount> FindAllPaged(int pageNumber, int pageSize, ref int totalCount)
        {
            List<CustomerAccount> result = new List<CustomerAccount>();
            long storeId = context.CurrentStore.Id;
            IQueryable<Data.EF.bvc_User> data = repository.Find().Where(y => y.StoreId == storeId)
                                      .OrderBy(y => y.Email);

            var countData = data;
            totalCount = countData.Count();

            var paged = PageItems(pageNumber, pageSize, data);
            result = ListPoco(paged);
            if (result == null) result = new List<CustomerAccount>();
                      
            return result;
        }
        public CustomerAccount FindByEmail(string email)
        {
            long storeId = context.CurrentStore.Id;
            var query = repository.Find().Where(y => y.StoreId == storeId)
                                        .Where(y => y.Email == email);
            return FirstPoco(query);                
        }
        public List<CustomerAccount> FindMany(List<string> ids)
        {       
            List<CustomerAccount> result = new List<CustomerAccount>();
            long storeId = context.CurrentStore.Id;
            IQueryable<Data.EF.bvc_User> data = repository.Find().Where(y => y.StoreId == storeId)
                        .Where(y => ids.Contains(y.bvin))
                                      .OrderBy(y => y.Email);

            var countData = data;
            result = ListPoco(data);
            if (result == null) result = new List<CustomerAccount>();

            return result;
        
        }
        public List<CustomerAccount> FindByFilter(string filter, int pageNumber, int pageSize, ref int totalCount)
        {
            List<CustomerAccount> result = new List<CustomerAccount>();
            long storeId = context.CurrentStore.Id;
            IQueryable<Data.EF.bvc_User> data = repository.Find().Where(y => y.StoreId == storeId)
                        .Where(y => y.Email.Contains(filter) ||
                                    y.FirstName.Contains(filter) ||
                                    y.LastName.Contains(filter) ||
                                    y.Phones.Contains(filter) ||
                                    y.AddressBook.Contains(filter))                        
                                      .OrderBy(y => y.Email);

            var countData = data;
            totalCount = countData.Count();

            var paged = PageItems(pageNumber, pageSize, data);
            result = ListPoco(paged);
            if (result == null) result = new List<CustomerAccount>();

            return result;
        }


       
    }
}
