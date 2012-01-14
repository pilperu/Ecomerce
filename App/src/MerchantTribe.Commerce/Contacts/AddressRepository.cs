using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MerchantTribe.Web;
using MerchantTribe.Web.Data;
using MerchantTribe.Web.Logging;

namespace MerchantTribe.Commerce.Contacts
{
    public class AddressRepository: ConvertingRepositoryBase<Data.EF.bvc_Address, Address>
    {
        private RequestContext context = null;

        public static AddressRepository InstantiateForMemory(RequestContext c)
        {
            return new AddressRepository(c, new MemoryStrategy<Data.EF.bvc_Address>(PrimaryKeyType.Bvin), new TextLogger());
        }
        public static AddressRepository InstantiateForDatabase(RequestContext c)
        {
            return new AddressRepository(c,
                new EntityFrameworkRepository<Data.EF.bvc_Address>(
                    new Data.EF.EntityFrameworkDevConnectionString(c.ConnectionStringForEntityFramework)),
                    new SupressLogger()
                    );
        }
        public AddressRepository(RequestContext c, IRepositoryStrategy<Data.EF.bvc_Address> r, ILogger log)
        {
            context = c;
            repository = r;
            this.logger = log;
            repository.Logger = this.logger;
        }

        protected override void CopyDataToModel(Data.EF.bvc_Address data, Address model)
        {
            //model.Id = data.Id;
            model.StoreId = data.StoreId;            
            model.Bvin = data.bvin;
            model.AddressType = (AddressTypes)data.AddressType;
            model.City = data.City;
            model.Company = data.Company;
            model.CountryBvin = data.CountryBvin;
            model.CountryName = data.CountryName;
            model.CountyBvin = data.CountyBvin;
            model.CountyName = data.CountyName;
            model.Fax = data.Fax;
            model.FirstName = data.FirstName;
            model.LastName = data.LastName;
            model.LastUpdatedUtc = data.LastUpdated;
            model.Line1 = data.Line1;
            model.Line2 = data.Line2;
            model.Line3 = data.Line3;
            model.MiddleInitial = data.MiddleInitial;
            model.NickName = data.NickName;
            model.Phone = data.Phone;
            model.PostalCode = data.PostalCode;
            model.RegionBvin = data.RegionBvin;
            model.RegionName = data.RegionName;
            model.UserBvin = data.UserBvin;
            model.WebSiteUrl = data.WebSiteUrl;

            //model.Residential;            
                            
        }
        protected override void CopyModelToData(Data.EF.bvc_Address data, Address model)
        {
            //data.Id = model.Id;
            data.StoreId = model.StoreId;
            data.bvin = model.Bvin;
            data.AddressType = (int)model.AddressType;
            data.City = model.City;
            data.Company = model.Company;
            data.CountryBvin = model.CountryBvin;
            data.CountryName = model.CountryName;
            data.CountyBvin = model.CountyBvin;
            data.CountyName = model.CountyName;
            data.Fax = model.Fax;
            data.FirstName = model.FirstName;
            data.LastName = model.LastName;
            data.LastUpdated = model.LastUpdatedUtc;
            data.Line1 = model.Line1;
            data.Line2 = model.Line2;
            data.Line3 = model.Line3;
            data.MiddleInitial = model.MiddleInitial;
            data.NickName = model.NickName;
            data.Phone = model.Phone;
            data.PostalCode = model.PostalCode;
            data.RegionBvin = model.RegionBvin;
            data.RegionName = model.RegionName;
            data.UserBvin = model.UserBvin;
            data.WebSiteUrl = model.WebSiteUrl;
        }

      
        public Address Find(string bvin)
        {
            Data.EF.bvc_Address data = repository.FindByPrimaryKey(new PrimaryKey(bvin));
            if (data == null) return null;

            Address result = new Address();
            CopyDataToModel(data, result);
            return result;
        }                        
        public override bool Create(Address item)
        {
            item.StoreId = context.CurrentStore.Id;
            item.LastUpdatedUtc = DateTime.UtcNow;
            item.Bvin = System.Guid.NewGuid().ToString();
            return base.Create(item);
        }       
        public bool Update(Address item)
        {
            if (item.StoreId != context.CurrentStore.Id)
            {
                return false;
            }
            item.LastUpdatedUtc = DateTime.UtcNow;
            return this.Update(item, new PrimaryKey(item.Bvin));
        }

        public List<Address> FindAll()
        {
            long storeId = context.CurrentStore.Id;
            IQueryable<Data.EF.bvc_Address> result = repository.Find().Where(y => y.StoreId == storeId).OrderBy(y => y.Id);
            return ListPoco(result);
        }
        public List<Address> FindByType(AddressTypes t)
        {
            long storeId = context.CurrentStore.Id;
            IQueryable<Data.EF.bvc_Address> result = repository.Find().Where(y => y.StoreId == storeId)
                                                                        .Where(y => y.AddressType == (int)t)
                                                                        .OrderBy(y => y.Id);
            return ListPoco(result);
        }
        public List<Address> FindByUserBvin(string userBvin)
        {
            long storeId = context.CurrentStore.Id;
            IQueryable<Data.EF.bvc_Address> result = repository.Find().Where(y => y.StoreId == storeId)
                                                                        .Where(y => y.UserBvin == userBvin)
                                                                        .OrderBy(y => y.Id);
            return ListPoco(result);
        }

        public Address FindStoreContactAddress()
        {
            Address result = new Address();
            result.CountryBvin = MerchantTribe.Web.Geography.Country.UnitedStatesCountryBvin;
            result.CountryName = "United States";
            result.RegionBvin = "VA";
            result.RegionName = "Virginia";
            result.AddressType = AddressTypes.StoreContact;
            List<Contacts.Address> possible = FindByType(AddressTypes.StoreContact);

            if (possible == null)
            {
                Create(result);
                return result;
            }
            if (possible.Count < 1)
            {
                Create(result);
                return result;
            }

            result = possible[0];
            return result;
        }
       
    }
}
