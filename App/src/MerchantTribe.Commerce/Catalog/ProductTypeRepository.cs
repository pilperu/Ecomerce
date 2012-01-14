using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MerchantTribe.Web.Data;
using MerchantTribe.Web.Logging;

namespace MerchantTribe.Commerce.Catalog
{
    public class ProductTypeRepository: ConvertingRepositoryBase<Data.EF.bvc_ProductType, ProductType>
    {
        private RequestContext context = null;

        public static ProductTypeRepository InstantiateForMemory(RequestContext c)
        {
            return new ProductTypeRepository(c, new MemoryStrategy<Data.EF.bvc_ProductType>(PrimaryKeyType.Bvin), new NullLogger());
        }
        public static ProductTypeRepository InstantiateForDatabase(RequestContext c)
        {
            ProductTypeRepository result = null;
            result = new ProductTypeRepository(c,
                new EntityFrameworkRepository<Data.EF.bvc_ProductType>(
                    new Data.EF.EntityFrameworkDevConnectionString(c.ConnectionStringForEntityFramework)),
                    new SupressLogger());
            return result;
        }
        public ProductTypeRepository(RequestContext c, IRepositoryStrategy<Data.EF.bvc_ProductType> r, ILogger log)
        {
            context = c;
            repository = r;
            this.logger = log;
            repository.Logger = this.logger;
        }

        protected override void CopyDataToModel(Data.EF.bvc_ProductType data, ProductType model)
        {
            model.Bvin = data.bvin;
            model.IsPermanent = data.IsPermanent;
            model.LastUpdated = data.LastUpdated;
            model.ProductTypeName = data.ProductTypeName;
            model.StoreId = data.StoreId;
        }
        protected override void CopyModelToData(Data.EF.bvc_ProductType data, ProductType model)
        {
            data.bvin = model.Bvin;
            data.IsPermanent = model.IsPermanent;
            data.LastUpdated = model.LastUpdated;
            data.ProductTypeName = model.ProductTypeName;
            data.StoreId = model.StoreId;
        }

        public ProductType Find(string bvin)
        {
            ProductType result = FindForAllStores(bvin);
            if (result != null)
            {
                if (result.StoreId == context.CurrentStore.Id)
                {
                    return result;
                }
            }
            return null;
        }
        public ProductType FindForAllStores(string bvin)
        {
            Data.EF.bvc_ProductType data = repository.FindByPrimaryKey(new PrimaryKey(bvin));
            if (data == null) return null;

            ProductType result = new ProductType();
            CopyDataToModel(data, result);
            return result;
        }
        public string FindNameForType(string bvin)
        {            
            ProductType item = Find(bvin);
            if (item != null)
            {
                return item.ProductTypeName;
            }
            return string.Empty;
        }
        public bool CreateAsNew(ProductType item)
        {
            if (item != null)
            {
                item.Bvin = string.Empty;
            }
            return Create(item);
        }
        public override bool Create(ProductType item)
        {
            item.LastUpdated = DateTime.UtcNow;            
            if (item.Bvin == string.Empty)
            {
                item.Bvin = System.Guid.NewGuid().ToString();
            }
            item.LastUpdated = DateTime.UtcNow;
            item.StoreId = context.CurrentStore.Id;
 	        return base.Create(item);
        }
        public bool Update(ProductType c)
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
            long storeId = context.CurrentStore.Id;
            ProductType img = Find(bvin);
            if (img == null) return false;
            return Delete(new PrimaryKey(bvin));            
        }

        public List<ProductType> FindAll()
        {
            List<ProductType> result = new List<ProductType>();
            long storeId = context.CurrentStore.Id;
            IQueryable<Data.EF.bvc_ProductType> items = repository.Find().Where(y => y.StoreId == storeId)
                                                                         .OrderBy(y => y.ProductTypeName);                                                                         
            if (items != null)
            {
                result = ListPoco(items);
            }
            return result;
        }
        public List<ProductType> FindByName(string name)
        {
            List<ProductType> result = new List<ProductType>();
            long storeId = context.CurrentStore.Id;
            IQueryable<Data.EF.bvc_ProductType> items = repository.Find().Where(y => y.StoreId == storeId)
                                                                         .Where(y => y.ProductTypeName == name);
            if (items != null)
            {
                result = ListPoco(items);
            }
            return result;
        }
              
    }
}
