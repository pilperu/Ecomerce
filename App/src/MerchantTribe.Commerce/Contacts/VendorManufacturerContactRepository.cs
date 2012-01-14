using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MerchantTribe.Web.Data;
using MerchantTribe.Web.Logging;

namespace MerchantTribe.Commerce.Contacts
{
    public class VendorManufacturerContactRepository: ConvertingRepositoryBase<Data.EF.bvc_UserXContact, VendorManufacturerContact>
    {
        private RequestContext context = null;

        public static VendorManufacturerContactRepository InstantiateForMemory(RequestContext c)
        {
            VendorManufacturerContactRepository result = null;
            result = new VendorManufacturerContactRepository(c, new MemoryStrategy<Data.EF.bvc_UserXContact>(PrimaryKeyType.Long), new TextLogger());
            return result;
        }
        public static VendorManufacturerContactRepository InstantiateForDatabase(RequestContext c)
        {
            VendorManufacturerContactRepository result = null;
            result = new VendorManufacturerContactRepository(c, 
                new EntityFrameworkRepository<Data.EF.bvc_UserXContact>(
                    new Data.EF.EntityFrameworkDevConnectionString(c.ConnectionStringForEntityFramework)),
                    new SupressLogger());
            return result;
        }
        public VendorManufacturerContactRepository(RequestContext c, IRepositoryStrategy<Data.EF.bvc_UserXContact> r, ILogger log)
        {
            context = c;
            repository = r;            
            this.logger = log;
            repository.Logger = this.logger;
        }

        protected override void CopyDataToModel(Data.EF.bvc_UserXContact data, VendorManufacturerContact model)
        {
            model.Id = data.Id;
            model.VendorManufacturerId = data.ContactId;
            model.StoreId = data.StoreId;
            model.UserId = data.UserId;
        }
        protected override void CopyModelToData(Data.EF.bvc_UserXContact data, VendorManufacturerContact model)
        {
            data.Id = model.Id;
            data.ContactId = model.VendorManufacturerId;
            data.StoreId = model.StoreId;
            data.UserId = model.UserId;
        }

        public VendorManufacturerContact Find(long id)
        {
            VendorManufacturerContact result = FindForAllStores(id);
            if (result != null)
            {
                if (result.StoreId == context.CurrentStore.Id)
                {
                    return result;
                }
            }
            return null;
        }
        public VendorManufacturerContact FindForAllStores(long id)
        {
            return this.Find(new PrimaryKey(id));
        }
        public override bool Create(VendorManufacturerContact item)
        {
            item.StoreId = context.CurrentStore.Id;
            return base.Create(item);
        }
        public bool Update(VendorManufacturerContact c)
        {
            if (c.StoreId != context.CurrentStore.Id)
            {
                return false;
            }
            return this.Update(c, new PrimaryKey(c.Id));
        }
        public bool Delete(long id)
        {
            return Delete(new PrimaryKey(id));
        }

        public List<VendorManufacturerContact> FindForVendorManufacturer(string bvin)
        {
            var items = repository.Find().Where(y => y.ContactId == bvin);                                       
            return ListPoco(items);
        }
        public void DeleteForVendorManufacturer(string bvin)
        {
            List<VendorManufacturerContact> existing = FindForVendorManufacturer(bvin);
            foreach (VendorManufacturerContact sub in existing)
            {
                Delete(sub.Id);
            }
        }
        public void MergeList(string bvin, List<VendorManufacturerContact> subitems)
        {
            // Set Base Key Field
            foreach (VendorManufacturerContact item in subitems)
            {
                item.VendorManufacturerId = bvin;
            }

            // Create or Update
            foreach (VendorManufacturerContact itemnew in subitems)
            {
                if (itemnew.Id < 1)
                {
                    Create(itemnew);
                }
                else
                {
                    Update(itemnew);
                }
            }

            // Delete missing
            List<VendorManufacturerContact> existing = FindForVendorManufacturer(bvin);
            foreach (VendorManufacturerContact ex in existing)
            {
                var count = (from sub in subitems
                             where sub.Id == ex.Id
                             select sub).Count();
                if (count < 1)
                {
                    Delete(ex.Id);
                }
            }
        }
    }
}
