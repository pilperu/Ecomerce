using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MerchantTribe.Web;
using MerchantTribe.Web.Data;
using MerchantTribe.Web.Logging;

namespace MerchantTribe.Commerce.Catalog
{
    public class ProductRelationshipRepository : ConvertingRepositoryBase<Data.EF.bvc_ProductRelationship, ProductRelationship>
    {
        private RequestContext context = null;

        public static ProductRelationshipRepository InstantiateForMemory(RequestContext c)
        {
            return new ProductRelationshipRepository(c, new MemoryStrategy<Data.EF.bvc_ProductRelationship>(PrimaryKeyType.Long), new TextLogger());
        }
        public static ProductRelationshipRepository InstantiateForDatabase(RequestContext c)
        {
            return new ProductRelationshipRepository(c,
                new EntityFrameworkRepository<Data.EF.bvc_ProductRelationship>(
                    new Data.EF.EntityFrameworkDevConnectionString(c.ConnectionStringForEntityFramework)),
                    new SupressLogger()
                    );
        }        
        public ProductRelationshipRepository(RequestContext c, IRepositoryStrategy<Data.EF.bvc_ProductRelationship> r, ILogger log)
        {
            context = c;
            repository = r;
            this.logger = log;
            repository.Logger = this.logger;
        }

        protected override void CopyDataToModel(Data.EF.bvc_ProductRelationship data, ProductRelationship model)
        {

            model.Id = data.Id;
            model.StoreId = data.StoreId;
            model.IsSubstitute = data.IsSubstitute;
            model.MarketingDescription = data.MarketingDescription;
            model.ProductId = data.ProductId;
            model.RelatedProductId = data.RelatedProductId;
            model.SortOrder = data.SortOrder;            
            
        }
        protected override void CopyModelToData(Data.EF.bvc_ProductRelationship data, ProductRelationship model)
        {
            data.Id = model.Id;
            data.StoreId = model.StoreId;
            data.IsSubstitute = model.IsSubstitute;
            data.MarketingDescription = model.MarketingDescription;
            data.ProductId = model.ProductId;
            data.RelatedProductId = model.RelatedProductId;
            data.SortOrder = model.SortOrder;            
        }
     
        public ProductRelationship Find(long id)
        {
            Data.EF.bvc_ProductRelationship data = repository.FindByPrimaryKey(new PrimaryKey(id));
            if (data == null) return null;

            ProductRelationship result = new ProductRelationship();
            CopyDataToModel(data, result);
            return result;
        }
        public ProductRelationship FindByProductAndRelated(string productId, string relatedId)
        {
            Data.EF.bvc_ProductRelationship data = repository.Find().Where(y => y.ProductId == productId)
                                                                    .Where(y => y.RelatedProductId == relatedId)
                                                                    .SingleOrDefault();
            if (data == null) return null;

            ProductRelationship result = new ProductRelationship();
            CopyDataToModel(data, result);
            return result;
        }        
        public override bool Create(ProductRelationship item)
        {
            item.StoreId = context.CurrentStore.Id;
            item.SortOrder = FindMaxSort(item.ProductId) + 1;
            return base.Create(item);
        }
        private int FindMaxSort(string productId)
        {
            int maxSort = 0;
            long storeId = context.CurrentStore.Id;
            List<ProductRelationship> result = new List<ProductRelationship>();
            IQueryable<Data.EF.bvc_ProductRelationship> items = repository.Find().Where(y => y.ProductId == productId)
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
        public bool Update(ProductRelationship c)
        {
            if (c.StoreId != context.CurrentStore.Id)
            {
                return false;
            }
            return this.Update(c, new PrimaryKey(c.Id));
        }

        public List<ProductRelationship> FindForProduct(string productId)
        {
            return FindForProduct(productId, context.CurrentStore.Id);
        }
        public List<ProductRelationship> FindForProduct(string productId, long storeId)
        {
            List<ProductRelationship> result = new List<ProductRelationship>();

            IQueryable<Data.EF.bvc_ProductRelationship> data = repository.Find().Where(y => y.ProductId == productId)
                                                                             .Where(y => y.StoreId == storeId).OrderBy(y => y.SortOrder);
            if (data != null)
            {
                result = ListPoco(data);
            }
            
            return result;
        }
        public List<ProductRelationship> FindForRelatedProduct(string relatedProductId)
        {
            return FindForRelatedProduct(relatedProductId, context.CurrentStore.Id);
        }
        public List<ProductRelationship> FindForRelatedProduct(string relatedProductId, long storeId)
        {
            List<ProductRelationship> result = new List<ProductRelationship>();

            IQueryable<Data.EF.bvc_ProductRelationship> data = repository.Find().Where(y => y.RelatedProductId == relatedProductId)
                                                                             .Where(y => y.StoreId == storeId).OrderBy(y => y.SortOrder);
            if (data != null)
            {
                result = ListPoco(data);
            }

            return result;
        }     

        public bool DeleteAllForProduct(string productId)
        {
            long storeId = context.CurrentStore.Id;

            IQueryable<Data.EF.bvc_ProductRelationship> items = repository.Find().Where(y => y.ProductId == productId)
                                                                              .Where(y => y.StoreId == storeId)            
                                                                              .OrderBy(y => y.SortOrder);

            if (items == null) return true;
            List<Data.EF.bvc_ProductRelationship> itemList = items.ToList();
            repository.SubmitChanges();

            if (itemList != null)
            {
                foreach (Data.EF.bvc_ProductRelationship item in itemList)
                {
                    repository.Delete(new PrimaryKey(item.Id));
                }
                repository.SubmitChanges();                
            }

            return true;
        }
        public bool DeleteAllForRelatedProduct(string relatedProductId)
        {
            long storeId = context.CurrentStore.Id;

            IQueryable<Data.EF.bvc_ProductRelationship> items = repository.Find().Where(y => y.RelatedProductId == relatedProductId)
                                                                              .Where(y => y.StoreId == storeId)
                                                                              .OrderBy(y => y.SortOrder);

            if (items == null) return true;
            List<Data.EF.bvc_ProductRelationship> itemList = items.ToList();
            repository.SubmitChanges();

            if (itemList != null)
            {
                foreach (Data.EF.bvc_ProductRelationship item in itemList)
                {
                    repository.Delete(new PrimaryKey(item.Id));
                }
                repository.SubmitChanges();
            }

            return true;
        }

        public bool RelateProducts(string productId, string relatedProductId, bool isSubstitute)
        {                        
            long storeId = context.CurrentStore.Id;

            Data.EF.bvc_ProductRelationship exists = repository.Find().Where(y => y.RelatedProductId == relatedProductId)
                                                                               .Where(y => y.ProductId == productId)
                                                                              .Where(y => y.StoreId == storeId)
                                                                              .SingleOrDefault();                                                                              
            if (exists == null)
            {
                ProductRelationship x = new ProductRelationship();
                x.ProductId = productId;
                x.RelatedProductId = relatedProductId;
                x.IsSubstitute = isSubstitute;                
                return Create(x);
            }

            return true;
        }
        public bool UnrelateProducts(string productId, string relatedProductId)
        {
            long storeId = context.CurrentStore.Id;

            Data.EF.bvc_ProductRelationship exists = repository.Find().Where(y => y.RelatedProductId == relatedProductId)
                                                                               .Where(y => y.ProductId == productId)
                                                                              .Where(y => y.StoreId == storeId)
                                                                              .SingleOrDefault();
            repository.SubmitChanges();
            if (exists != null)
            {
                return Delete(new PrimaryKey(exists.Id));
            }

            return true;
        }

        private bool UpdateSortOrderForProduct(string productId, string relatedId, int newSortOrder)
        {
            Catalog.ProductRelationship item = FindByProductAndRelated(productId, relatedId);
            if (item == null) return false;                        
            item.SortOrder = newSortOrder;
            return Update(item);                                    
        }

        public bool ResortRelationships(string productId, List<string> sortedIds)
        {
            if (sortedIds != null)
            {
                for (int i = 1; i <= sortedIds.Count; i++)
                {
                    UpdateSortOrderForProduct(productId, sortedIds[i - 1], i);
                }
            }
            return true;
        }

    }
}
