using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MerchantTribe.Web.Data;
using MerchantTribe.Web.Logging;

namespace MerchantTribe.Commerce.Catalog
{
    public class ProductReviewRepository : ConvertingRepositoryBase<Data.EF.bvc_ProductReview, ProductReview>
    {
        private RequestContext context = null;

        public static ProductReviewRepository InstantiateForMemory(RequestContext c)
        {
            return new ProductReviewRepository(c, new MemoryStrategy<Data.EF.bvc_ProductReview>(PrimaryKeyType.Bvin), new NullLogger());
        }
        public static ProductReviewRepository InstantiateForDatabase(RequestContext c)
        {
            ProductReviewRepository result = null;
            result = new ProductReviewRepository(c,
                new EntityFrameworkRepository<Data.EF.bvc_ProductReview>(
                    new Data.EF.EntityFrameworkDevConnectionString(c.ConnectionStringForEntityFramework)),
                    new SupressLogger());
            return result;
        }
        public ProductReviewRepository(RequestContext c, IRepositoryStrategy<Data.EF.bvc_ProductReview> r, ILogger log)
        {
            context = c;
            repository = r;
            this.logger = log;
            repository.Logger = this.logger;
        }

        protected override void CopyDataToModel(Data.EF.bvc_ProductReview data, ProductReview model)
        {
            model.StoreId = data.StoreId;
            model.Approved = data.Approved == 1 ? true : false;
            model.Bvin = data.bvin;
            model.Description = data.Description;
            model.Karma = data.Karma;
            model.LastUpdated = data.lastUpdated;
            model.ProductBvin = data.ProductBvin;
            //model.ProductName = data.ProductName;
            model.Rating = (ProductReviewRating)data.Rating;
            model.ReviewDateUtc = data.ReviewDate;
            model.UserID = data.UserID;
        }
        protected override void CopyModelToData(Data.EF.bvc_ProductReview data, ProductReview model)
        {
            data.StoreId = model.StoreId;
            data.Approved = model.Approved == true ? 1 : 0;
            data.bvin = model.Bvin;
            data.Description = model.Description;
            data.Karma = model.Karma;
            data.lastUpdated = model.LastUpdated;
            data.ProductBvin = model.ProductBvin;
            //data.ProductName = model.ProductName;
            data.Rating = (int)model.Rating;
            data.ReviewDate = model.ReviewDateUtc;
            data.UserID = model.UserID;
        }

        public ProductReview Find(string bvin)
        {
            ProductReview result = FindForAllStores(bvin);
            if (result != null)
            {
                if (result.StoreId == context.CurrentStore.Id)
                {
                    return result;
                }
            }
            return null;
        }
        public ProductReview FindForAllStores(string bvin)
        {
            Data.EF.bvc_ProductReview data = repository.FindByPrimaryKey(new PrimaryKey(bvin));
            if (data == null) return null;

            ProductReview result = new ProductReview();
            CopyDataToModel(data, result);
            return result;
        }

        public override bool Create(ProductReview item)
        {
            item.LastUpdated = DateTime.UtcNow;            
            if (item.Bvin == string.Empty)
            {
                item.Bvin = System.Guid.NewGuid().ToString();
            }
            item.StoreId = context.CurrentStore.Id;
 	        return base.Create(item);
        }
        public bool Update(ProductReview c)
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
            ProductReview img = Find(bvin);
            if (img == null) return false;
            return Delete(new PrimaryKey(bvin));            
        }
        public bool DeleteForProductId(string productId)
        {
            List<ProductReview> toDelete = FindByProductId(productId);
            foreach (ProductReview r in toDelete)
            {
                Delete(r.Bvin);
            }
            return true;
        }
        public List<ProductReview> FindByProductId(string productId)
        {
            return FindByProductIdPaged(productId, 1, int.MaxValue);
        }
        public List<ProductReview> FindByProductIdPaged(string productId, int pageNumber, int pageSize)
        {
            List<ProductReview> result = new List<ProductReview>();

            if (pageNumber < 1) pageNumber = 1;

            int take = pageSize;
            int skip = (pageNumber - 1) * pageSize;
            long storeId = context.CurrentStore.Id;

            IQueryable<Data.EF.bvc_ProductReview> items = repository.Find().Where(y => y.ProductBvin == productId)
                                                                          .Where(y => y.StoreId == storeId)
                                                                          .OrderByDescending(y => y.Karma)
                                                                          .ThenByDescending(y => y.ReviewDate)
                                                                          .Skip(skip)
                                                                          .Take(take);
            if (items != null)
            {
                result = ListPoco(items);
            }

            return result;
        }
        public void MergeList(string productBvin, List<ProductReview> subitems)
        {
            long storeId = context.CurrentStore.Id;
            // Set Base Key Field
            foreach (ProductReview item in subitems)
            {
                item.ProductBvin = productBvin;
                item.StoreId = storeId;
            }

            // Create or Update
            foreach (ProductReview itemnew in subitems)
            {
                if (itemnew.Bvin == string.Empty)
                {
                    itemnew.LastUpdated = DateTime.UtcNow;
                    Create(itemnew);
                }
                else
                {
                    Update(itemnew);
                }
            }

            // Delete missing
            List<ProductReview> existing = FindByProductId(productBvin);
            foreach (ProductReview ex in existing)
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

        public List<ProductReview> FindNotApproved(int pageNumber, int pageSize)
        {
            List<ProductReview> result = new List<ProductReview>();

            if (pageNumber < 1) pageNumber = 1;

            int take = pageSize;
            int skip = (pageNumber - 1) * pageSize;
            long storeId = context.CurrentStore.Id;

            IQueryable<Data.EF.bvc_ProductReview> items = repository.Find().Where(y => y.Approved == 0)
                                                                          .Where(y => y.StoreId == storeId)
                                                                          .OrderByDescending(y => y.ReviewDate)
                                                                          .Skip(skip)
                                                                          .Take(take);
            if (items != null)
            {
                result = ListPoco(items);
            }

            return result;
        }
        
        public bool UpdateKarma(string reviewBvin, int karmaModifier)
        {
            ProductReview item = Find(reviewBvin);
            if (item == null) return false;
            item.Karma += karmaModifier;            
            return Update(item);
        }
    }
}
