using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MerchantTribe.Web.Data;
using MerchantTribe.Web.Logging;

namespace MerchantTribe.Commerce.Catalog
{
    public class ProductInventoryRepository: ConvertingRepositoryBase<Data.EF.bvc_ProductInventory, ProductInventory>
    {
        private RequestContext context = null;

        public static ProductInventoryRepository InstantiateForMemory(RequestContext c)
        {
            return new ProductInventoryRepository(c, new MemoryStrategy<Data.EF.bvc_ProductInventory>(PrimaryKeyType.Bvin),
                                           new TextLogger());
        }
        public static ProductInventoryRepository InstantiateForDatabase(RequestContext c)
        {
            ProductInventoryRepository result = null;
            result = new ProductInventoryRepository(c,
                new EntityFrameworkRepository<Data.EF.bvc_ProductInventory>(
                    new Data.EF.EntityFrameworkDevConnectionString(c.ConnectionStringForEntityFramework)),
                    new SupressLogger()
                    );
            return result;
        }
        public ProductInventoryRepository(RequestContext c, IRepositoryStrategy<Data.EF.bvc_ProductInventory> r,                                    
                                    ILogger log)
        {
            context = c;
            repository = r;
            this.logger = log;
            repository.Logger = this.logger;
        }

        protected override void CopyDataToModel(Data.EF.bvc_ProductInventory data, ProductInventory model)
        {
            model.Bvin = data.bvin;
            model.LastUpdated = data.LastUpdated;
            model.LowStockPoint = data.LowStockPoint;
            model.ProductBvin = data.ProductBvin;
            // Calculated Field
            //model.QuantityAvailableForSale = data.QuantityAvailableForSale;
            model.QuantityOnHand = data.QuantityOnHand;
            model.QuantityReserved = data.QuantityReserved;
            model.StoreId = data.StoreId;
            model.VariantId = data.VariantId;
        }
        protected override void CopyModelToData(Data.EF.bvc_ProductInventory data, ProductInventory model)
        {
            data.bvin = model.Bvin;
            data.LastUpdated = model.LastUpdated;
            data.LowStockPoint = model.LowStockPoint;
            data.ProductBvin = model.ProductBvin;
            data.QuantityAvailableForSale = model.QuantityAvailableForSale;
            data.QuantityOnHand = model.QuantityOnHand;
            data.QuantityReserved = model.QuantityReserved;
            data.StoreId = model.StoreId;
            data.VariantId = model.VariantId;        
        }

        public ProductInventory Find(string bvin)
        {
            ProductInventory result = FindForAllStores(bvin);
            if (result != null)
            {
                if (result.StoreId == context.CurrentStore.Id)
                {
                    return result;
                }
            }
            return null;
        }
        public ProductInventory FindForAllStores(string bvin)
        {
            return this.Find(new PrimaryKey(bvin));
        }
        public bool InventoryExists(ProductInventory item)
        {
            var inventory = FindByProductIdAndVariantId(item.ProductBvin, item.VariantId);
            if (inventory != null) return true;
            return false;
        }
        public override bool Create(ProductInventory item)
        {
            if (item.Bvin == string.Empty)
            {
                item.Bvin = System.Guid.NewGuid().ToString();
            }
            item.StoreId = context.CurrentStore.Id;
            item.LastUpdated = DateTime.UtcNow;

            // See if we need to update instead of create
            if (InventoryExists(item)) return Update(item);
                        
 	        return base.Create(item);
        }
        public bool Update(ProductInventory c)
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
            ProductInventory item = Find(bvin);
            if (item == null) return false;

           return Delete(new PrimaryKey(bvin));            
        }

        public List<ProductInventory> FindByProductId(string productId)
        {
            long storeId = context.CurrentStore.Id;
            IQueryable<Data.EF.bvc_ProductInventory> data = repository.Find().Where(y => y.StoreId == storeId)
                                                       .Where(y => y.ProductBvin == productId);
            return ListPoco(data);
        }
        public ProductInventory FindByProductIdAndVariantId(string productId, string variantId)
        {
            long storeId = context.CurrentStore.Id;
            IQueryable<Data.EF.bvc_ProductInventory> data = repository.Find().Where(y => y.StoreId == storeId)
                                                       .Where(y => y.ProductBvin == productId)
                                                       .Where(y => y.VariantId == variantId);
            return FirstPoco(data);
        }        
        public bool DeleteByProductId(string productId)
        {
            List<ProductInventory> items = FindByProductId(productId);
            foreach (ProductInventory v in items)
            {
                Delete(v.Bvin);
            }
            return true;
        }
        public List<ProductInventory> FindAllLowStock()
        {
            long storeId = context.CurrentStore.Id;
            IQueryable<Data.EF.bvc_ProductInventory> data = repository.Find().Where(y => y.StoreId == storeId)
                                                       .Where(y => y.QuantityAvailableForSale <= y.LowStockPoint);
            return ListPoco(data);
        }
      

     
    }
}
