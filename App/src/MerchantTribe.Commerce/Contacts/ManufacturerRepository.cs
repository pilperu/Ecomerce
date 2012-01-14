using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MerchantTribe.Web.Data;
using MerchantTribe.Web.Logging;

namespace MerchantTribe.Commerce.Contacts
{
    public class ManufacturerRepository: ConvertingRepositoryBase<Data.EF.bvc_Manufacturer, VendorManufacturer>
    {
        private RequestContext context = null;
        private VendorManufacturerContactRepository contactRepository = null;

        public static ManufacturerRepository InstantiateForMemory(RequestContext c)
        {
            ManufacturerRepository result = null;
            result = new ManufacturerRepository(c, new MemoryStrategy<Data.EF.bvc_Manufacturer>(PrimaryKeyType.Guid),
                                        new MemoryStrategy<Data.EF.bvc_UserXContact>(PrimaryKeyType.Long),
                                        new TextLogger());
            return result;
        }
        public static ManufacturerRepository InstantiateForDatabase(RequestContext c)
        {
            ManufacturerRepository result = null;
            result = new ManufacturerRepository(c,
                new EntityFrameworkRepository<Data.EF.bvc_Manufacturer>(new Data.EF.EntityFrameworkDevConnectionString(c.ConnectionStringForEntityFramework)),
                new EntityFrameworkRepository<Data.EF.bvc_UserXContact>(new Data.EF.EntityFrameworkDevConnectionString(c.ConnectionStringForEntityFramework)),
                new SupressLogger());
            return result;
        }
        public ManufacturerRepository(RequestContext c, IRepositoryStrategy<Data.EF.bvc_Manufacturer> r,
                                IRepositoryStrategy<Data.EF.bvc_UserXContact> sub, ILogger log)
        {
            context = c;
            repository = r;
            this.logger = log;
            contactRepository = new VendorManufacturerContactRepository(c, sub, this.logger);
            repository.Logger = this.logger;
        }

        protected override void CopyDataToModel(Data.EF.bvc_Manufacturer data, VendorManufacturer model)
        {
            model.Address.FromXmlString(data.Address);
            model.Bvin = data.bvin;
            model.DisplayName = data.DisplayName;
            model.DropShipEmailTemplateId = data.DropShipEmailTemplateId;
            model.EmailAddress = data.EmailAddress;
            model.LastUpdated = data.LastUpdated;
            model.StoreId = data.StoreId;
            model.ContactType = VendorManufacturerType.Manufacturer;
        }
        protected override void CopyModelToData(Data.EF.bvc_Manufacturer data, VendorManufacturer model)
        {
            data.Address = model.Address.ToXml(true);
            data.bvin = model.Bvin;
            data.DisplayName = model.DisplayName;
            data.DropShipEmailTemplateId = model.DropShipEmailTemplateId;
            data.EmailAddress = model.EmailAddress;
            data.LastUpdated = model.LastUpdated;
            data.StoreId = model.StoreId;
        }

        protected override void DeleteAllSubItems(VendorManufacturer model)
        {
            contactRepository.DeleteForVendorManufacturer(model.Bvin);
        }
        protected override void GetSubItems(VendorManufacturer model)
        {
            model.Contacts = contactRepository.FindForVendorManufacturer(model.Bvin);
        }
        protected override void MergeSubItems(VendorManufacturer model)
        {
            contactRepository.MergeList(model.Bvin, model.Contacts);
        }

        public VendorManufacturer Find(string bvin)
        {
            VendorManufacturer result = FindForAllStores(bvin);
            if (result != null)
            {
                if (result.StoreId == context.CurrentStore.Id)
                {
                    return result;
                }
            }
            return null;
        }
        public VendorManufacturer FindForAllStores(string bvin)
        {
            return this.Find(new PrimaryKey(bvin));
        }
        public override bool Create(VendorManufacturer item)
        {
            if (item.Bvin == string.Empty) item.Bvin = System.Guid.NewGuid().ToString();
            item.LastUpdated = DateTime.UtcNow;
            item.StoreId = context.CurrentStore.Id;
            return base.Create(item);
        }
        public bool Update(VendorManufacturer c)
        {
            if (c.StoreId != context.CurrentStore.Id)
            {
                return false;
            }

            c.LastUpdated = DateTime.UtcNow;
            return this.Update(c, new PrimaryKey(c.Bvin));
        }
        public bool Delete(string bvin)
        {
            return Delete(new PrimaryKey(bvin));
        }

        public List<VendorManufacturer> FindAll()
        {
            return ListPoco(repository.Find().Where(y => y.StoreId == context.CurrentStore.Id));
        }
        public List<VendorManufacturer> FindAllWithFilter(string filter, int pageNumber, int pageSize, ref int rowCount)
        {
            var x = repository.Find().Where(y => y.StoreId == context.CurrentStore.Id)
                                     .Where(y => y.DisplayName.Contains(filter) || y.EmailAddress.Contains(filter));                                     
            if (x != null)
            {
                rowCount = x.Count();
                var x2 = x.OrderBy(y => y.StoreId).Skip((pageNumber - 1) * pageSize).Take(pageSize);
                return ListPoco(x2);
            }

            return new List<VendorManufacturer>();
        }
        public List<VendorManufacturer> FindByUserId(string userId)
        {
            var x = repository.Find().Where(y => y.StoreId == context.CurrentStore.Id);

            List<VendorManufacturer> vendors = ListPoco(x);

            List<VendorManufacturer> output = vendors.Where(y => y.ContactExists(userId) == true).ToList();
            if (output != null)
            {
                return output;
            }

            return new List<VendorManufacturer>();
        }
    }
}
