using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MerchantTribe.Web.Data;
using MerchantTribe.Web.Logging;

namespace MerchantTribe.Commerce.Catalog
{
    public class ProductImageRepository: ConvertingRepositoryBase<Data.EF.bvc_ProductImage, ProductImage>
    {
        private RequestContext context = null;

        public static ProductImageRepository InstantiateForMemory(RequestContext c)
        {
            return new ProductImageRepository(c, new MemoryStrategy<Data.EF.bvc_ProductImage>(PrimaryKeyType.Bvin), new NullLogger());
        }
        public static ProductImageRepository InstantiateForDatabase(RequestContext c)
        {
            ProductImageRepository result = null;
            result = new ProductImageRepository(c,
                new EntityFrameworkRepository<Data.EF.bvc_ProductImage>(
                    new Data.EF.EntityFrameworkDevConnectionString(c.ConnectionStringForEntityFramework)),
                    new SupressLogger());
            return result;
        }
        public ProductImageRepository(RequestContext c, IRepositoryStrategy<Data.EF.bvc_ProductImage> r, ILogger log)
        {
            context = c;
            repository = r;
            this.logger = log;
            repository.Logger = this.logger;
        }

        protected override void CopyDataToModel(Data.EF.bvc_ProductImage data, ProductImage model)
        {
            model.AlternateText = data.AlternateText;
            model.Bvin = data.bvin;
            model.Caption = data.Caption;
            model.FileName = data.FileName;
            model.LastUpdatedUtc = data.LastUpdated;
            model.ProductId = data.ProductID;
            model.SortOrder = data.SortOrder;
            model.StoreId = data.StoreId;            
        }
        protected override void CopyModelToData(Data.EF.bvc_ProductImage data, ProductImage model)
        {
            data.AlternateText = model.AlternateText;
            data.bvin = model.Bvin;
            data.Caption = model.Caption;
            data.FileName = model.FileName;
            data.LastUpdated = model.LastUpdatedUtc;
            data.ProductID = model.ProductId;
            data.SortOrder = model.SortOrder;
            data.StoreId = model.StoreId;
        }

     
        public ProductImage Find(string bvin)
        {
            ProductImage result = FindForAllStores(bvin);
            if (result != null)
            {
                if (result.StoreId == context.CurrentStore.Id)
                {
                    return result;
                }
            }
            return null;
        }
        public ProductImage FindForAllStores(string bvin)
        {
            Data.EF.bvc_ProductImage data = repository.FindByPrimaryKey(new PrimaryKey(bvin));
            if (data == null) return null;

            ProductImage result = new ProductImage();
            CopyDataToModel(data, result);
            return result;
        }
        
        public override bool  Create(ProductImage item)
        {
            item.LastUpdatedUtc = DateTime.UtcNow;            
            if (item.Bvin == string.Empty)
            {
                item.Bvin = System.Guid.NewGuid().ToString();
            }
            item.StoreId = context.CurrentStore.Id;

            item.SortOrder = FindMaxSort(item.ProductId) + 1;

 	        return base.Create(item);
        }
        private int FindMaxSort(string productId)
        {
            int maxSort = 0;
            long storeId = context.CurrentStore.Id;
            List<ProductImage> result = new List<ProductImage>();
            IQueryable<Data.EF.bvc_ProductImage> items = repository.Find().Where(y => y.ProductID == productId)
                                                                      .Where(y => y.StoreId == storeId).OrderByDescending(y => y.SortOrder)
                                                                      .Take(1);
            if (items != null)
            {
                var i = items.ToList();
                if (i.Count > 0)
                {
                    maxSort = i[0].SortOrder;
                }
            }
            return maxSort;
        }

        public bool Update(ProductImage c)
        {
            if (c.StoreId != context.CurrentStore.Id)
            {
                return false;
            }

            c.LastUpdatedUtc = DateTime.UtcNow;
            return this.Update(c, new PrimaryKey(c.Bvin));            
        }

        public bool Delete(string bvin)
        {
            long storeId = context.CurrentStore.Id;
            ProductImage img = Find(bvin);
            if (img == null) return false;

            Storage.DiskStorage.DeleteAdditionalProductImage(storeId, img.ProductId, img.Bvin);            
            return Delete(new PrimaryKey(bvin));            
        }
        public bool DeleteForProductId(string productId)
        {
            List<ProductImage> toDelete = FindByProductId(productId);
            foreach (ProductImage item in toDelete)
            {
                Delete(item.Bvin);
            }
            return true;
        }
        public List<ProductImage> FindByProductId(string productId)
        {
            return FindByProductIdPaged(productId, 1, int.MaxValue);
        }      
        public List<ProductImage> FindByProductIdPaged(string productId, int pageNumber, int pageSize)
        {
            List<ProductImage> result = new List<ProductImage>();

            if (pageNumber < 1) pageNumber = 1;

            int take = pageSize;
            int skip = (pageNumber - 1) * pageSize;
            long storeId = context.CurrentStore.Id;

            IQueryable<Data.EF.bvc_ProductImage> items = repository.Find().Where(y => y.ProductID == productId)
                                                                          .Where(y => y.StoreId == storeId)
                                                                          .OrderBy(y => y.SortOrder)
                                                                          .Skip(skip)
                                                                          .Take(take);
            if (items != null)
            {
                result = ListPoco(items);
            }

            return result;
        }
        public void MergeList(string productBvin, List<ProductImage> subitems)
        {
            long storeId = context.CurrentStore.Id;
            // Set Base Key Field
            foreach (ProductImage item in subitems)
            {
                item.ProductId = productBvin;
                item.StoreId = storeId;
            }

            // Create or Update
            foreach (ProductImage itemnew in subitems)
            {
                if (itemnew.Bvin == string.Empty)
                {
                    itemnew.LastUpdatedUtc = DateTime.UtcNow;
                    Create(itemnew);
                }
                else
                {
                    Update(itemnew);
                }
            }

            // Delete missing
            List<ProductImage> existing = FindByProductId(productBvin);
            foreach (ProductImage ex in existing)
            {
                var count = (from sub in subitems
                             where sub.Bvin == ex.Bvin
                             select sub).Count();
                if (count < 1)
                {
                    Delete(ex.Bvin);
                }
            }
        }

        public bool Resort(string productBvin, List<string> sortedIds)
        {
            if (sortedIds != null)
            {
                for (int i = 1; i <= sortedIds.Count; i++)
                {
                    UpdateSortOrderForImage(productBvin, sortedIds[i - 1], i);
                }
            }
            return true;
        }
        private bool UpdateSortOrderForImage(string productBvin, string imageBvin, int newSortOrder)
        {
            ProductImage item = Find(imageBvin);
            if (item == null) return false;
            if (item.ProductId != productBvin) return false;
            item.SortOrder = newSortOrder;
            return Update(item);
        }
    }
}
