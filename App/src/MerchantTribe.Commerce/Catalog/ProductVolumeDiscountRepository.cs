using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MerchantTribe.Web.Data;
using MerchantTribe.Web.Logging;

namespace MerchantTribe.Commerce.Catalog
{
    public class ProductVolumeDiscountRepository: ConvertingRepositoryBase<Data.EF.bvc_ProductVolumeDiscounts, ProductVolumeDiscount>
    {
        private RequestContext context = null;

        public static ProductVolumeDiscountRepository InstantiateForMemory(RequestContext c)
        {
            return new ProductVolumeDiscountRepository(c, new MemoryStrategy<Data.EF.bvc_ProductVolumeDiscounts>(PrimaryKeyType.Bvin),
                                           new TextLogger());
        }
        public static ProductVolumeDiscountRepository InstantiateForDatabase(RequestContext c)
        {
            ProductVolumeDiscountRepository result = null;
            result = new ProductVolumeDiscountRepository(c,
                new EntityFrameworkRepository<Data.EF.bvc_ProductVolumeDiscounts>(
                    new Data.EF.EntityFrameworkDevConnectionString(c.ConnectionStringForEntityFramework)),
                    new SupressLogger()
                    );
            return result;
        }
        public ProductVolumeDiscountRepository(RequestContext c, IRepositoryStrategy<Data.EF.bvc_ProductVolumeDiscounts> r,                                    
                                    ILogger log)
        {
            context = c;
            repository = r;
            this.logger = log;
            repository.Logger = this.logger;
        }

        protected override void CopyDataToModel(Data.EF.bvc_ProductVolumeDiscounts data, ProductVolumeDiscount model)
        {
            model.Amount = data.Amount;
            model.Bvin = data.bvin;
            model.DiscountType = (ProductVolumeDiscountType)model.DiscountType;
            model.LastUpdated = data.LastUpdated;
            model.ProductId = data.ProductID;
            model.Qty = data.Qty;
            model.StoreId = data.StoreId;            
        }
        protected override void CopyModelToData(Data.EF.bvc_ProductVolumeDiscounts data, ProductVolumeDiscount model)
        {
            data.Amount = model.Amount;
            data.bvin = model.Bvin;
            data.DiscountType = (int)model.DiscountType;
            data.LastUpdated = model.LastUpdated;
            data.ProductID = model.ProductId;
            data.Qty = model.Qty;
            data.StoreId = model.StoreId;            
        }

        public ProductVolumeDiscount Find(string bvin)
        {
            ProductVolumeDiscount result = FindForAllStores(bvin);
            if (result != null)
            {
                if (result.StoreId == context.CurrentStore.Id)
                {
                    return result;
                }
            }
            return null;
        }
        public ProductVolumeDiscount FindForAllStores(string bvin)
        {
            return this.Find(new PrimaryKey(bvin));
        }
        public override bool Create(ProductVolumeDiscount item)
        {
            if (item.Bvin == string.Empty)
            {
                item.Bvin = System.Guid.NewGuid().ToString();
            }
            item.StoreId = context.CurrentStore.Id;
            item.LastUpdated = DateTime.UtcNow;
 	        return base.Create(item);
        }
        public bool Update(ProductVolumeDiscount c)
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
            ProductVolumeDiscount item = Find(bvin);
            if (item == null) return false;

           return Delete(new PrimaryKey(bvin));            
        }

        public List<ProductVolumeDiscount> FindByProductId(string productId)
        {
            long storeId = context.CurrentStore.Id;
            IQueryable<Data.EF.bvc_ProductVolumeDiscounts> data = repository.Find().Where(y => y.StoreId == storeId)
                                                       .Where(y => y.ProductID == productId);
            return ListPoco(data);
        }

        public ProductVolumeDiscount FindByQtyAndProductId(string productId, int qty)
        {
            long storeId = context.CurrentStore.Id;
            IQueryable<Data.EF.bvc_ProductVolumeDiscounts> data = repository.Find().Where(y => y.StoreId == storeId)
                                                       .Where(y => y.ProductID == productId)
                                                       .Where(y => y.Qty <= qty)
                                                       .OrderByDescending(y => y.Qty);
            return FirstPoco(data);
        }

        public bool DeleteByProductId(string productId)
        {
            List<ProductVolumeDiscount> items = FindByProductId(productId);
            foreach (ProductVolumeDiscount v in items)
            {
                Delete(v.Bvin);
            }
            return true;
        }
     
    }
}
