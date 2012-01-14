using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MerchantTribe.Web.Data;
using MerchantTribe.Web.Logging;

namespace MerchantTribe.Commerce.Catalog
{
    public class ProductPropertyValueRepository: ConvertingRepositoryBase<Data.EF.bvc_ProductPropertyValue, ProductPropertyValue>
    {
        private RequestContext context = null;

        public static ProductPropertyValueRepository InstantiateForMemory(RequestContext c)
        {
            return new ProductPropertyValueRepository(c, new MemoryStrategy<Data.EF.bvc_ProductPropertyValue>(PrimaryKeyType.Long),
                                           new TextLogger());
        }
        public static ProductPropertyValueRepository InstantiateForDatabase(RequestContext c)
        {
            ProductPropertyValueRepository result = null;
            result = new ProductPropertyValueRepository(c,
                new EntityFrameworkRepository<Data.EF.bvc_ProductPropertyValue>(
                    new Data.EF.EntityFrameworkDevConnectionString(c.ConnectionStringForEntityFramework)),
                    new SupressLogger()
                    );
            return result;
        }
        public ProductPropertyValueRepository(RequestContext c, IRepositoryStrategy<Data.EF.bvc_ProductPropertyValue> r,                                    
                                    ILogger log)
        {
            context = c;
            repository = r;
            this.logger = log;
            repository.Logger = this.logger;
        }

        protected override void CopyDataToModel(Data.EF.bvc_ProductPropertyValue data, ProductPropertyValue model)
        {            
            model.Id = data.Id;
            model.ProductID = data.ProductBvin;
            model.PropertyID = data.PropertyId;
            //model.PropertyChoiceId = data.PropertyChoiceId;
            model.StoreId = data.StoreId;
            model.StringValue = data.PropertyValue;
        }
        protected override void CopyModelToData(Data.EF.bvc_ProductPropertyValue data, ProductPropertyValue model)
        {
            data.Id = model.Id;
            data.ProductBvin = model.ProductID;
            data.PropertyId = model.PropertyID;
            //data.PropertyChoiceId = model.PropertyChoiceId;
            data.StoreId = model.StoreId;
            data.PropertyValue = model.StringValue;
        }

        public ProductPropertyValue Find(long id)
        {
            ProductPropertyValue result = FindForAllStores(id);
            if (result != null)
            {
                if (result.StoreId == context.CurrentStore.Id)
                {
                    return result;
                }
            }
            return null;
        }
        public ProductPropertyValue FindForAllStores(long id)
        {
            return this.Find(new PrimaryKey(id));
        }
        public override bool Create(ProductPropertyValue item)
        {
            item.StoreId = context.CurrentStore.Id;            
 	        return base.Create(item);
        }
        public bool Update(ProductPropertyValue c)
        {
            if (c.StoreId != context.CurrentStore.Id)
            {
                return false;
            }
            return this.Update(c, new PrimaryKey(c.Id));            
        }
        public bool Delete(long id)
        {
            long storeId = context.CurrentStore.Id;
            ProductPropertyValue item = Find(id);
            if (item == null) return false;

           return Delete(new PrimaryKey(id));            
        }
        public List<ProductPropertyValue> FindByProductId(string productId)
        {
            long storeId = context.CurrentStore.Id;
            IQueryable<Data.EF.bvc_ProductPropertyValue> data = repository.Find().Where(y => y.StoreId == storeId)
                                                       .Where(y => y.ProductBvin == productId);
            return ListPoco(data);
        }
        public ProductPropertyValue FindByProductIdAndPropertyId(string productId, long propertyId)
        {
            long storeId = context.CurrentStore.Id;
            IQueryable<Data.EF.bvc_ProductPropertyValue> data = repository.Find().Where(y => y.StoreId == storeId)
                                                       .Where(y => y.ProductBvin == productId)
                                                       .Where(y => y.PropertyId == propertyId);
            return FirstPoco(data);
        }
        private class CountResult
        {
            public string Bvin { get; set; }
            public int C { get; set; }
        }
        public List<string> FindProductsContainingAllChoiceIds(List<long> choiceIds)
        {
            return FindProductsContainingAllChoiceIds(choiceIds, 1, int.MaxValue);
        }
        public List<string> FindProductsContainingAllChoiceIds(List<long> choiceIds, int pageNumber, int pageSize)
        {   
            // Find all product bvins that have at least a single value for all 
            List<string> result = new List<string>();


            // Convert longs to strings for matching
            List<string> choiceIdStrings = new List<string>();
            foreach(long l in choiceIds)
            {
                if (l > 0)
                {
                    choiceIdStrings.Add(l.ToString());
                }
            }
            int choiceIdCount = choiceIdStrings.Count;

            long storeId = context.CurrentStore.Id;
            var matchingItems = repository.Find().Where(y => y.StoreId == storeId);
            if (choiceIdStrings.Count > 0)
            {
                matchingItems = matchingItems.Where(y => choiceIdStrings.Contains(y.PropertyValue));
            }            
            
            var match2 = matchingItems.GroupBy(y => y.ProductBvin)
                              .Select(grouping => grouping.Select(y => new CountResult { Bvin = y.ProductBvin, C = grouping.Count() })
                                                          .FirstOrDefault()).Where(y => y.C >= choiceIdCount)
                                                          .OrderBy(y => y.Bvin);

            if (pageNumber < 1) pageNumber = 1;

            int take = pageSize;
            int skip = (pageNumber - 1) * pageSize;
            var paged = match2.Skip(skip).Take(take);            
            if (paged == null) return result;                                            
            foreach (var p in paged)
            {                
                result.Add(p.Bvin);                
            }                                                                                                                                                                                                                          
            return result;
        }
        public int FindCountOfProductsContainingAllChoiceIds(List<long> choiceIds)
        {
            // Find all product bvins that have at least a single value for all 
            int result = 0;

            // Convert longs to strings for matching
            List<string> choiceIdStrings = new List<string>();
            foreach (long l in choiceIds)
            {
                if (l > 0)
                {
                    choiceIdStrings.Add(l.ToString());
                }
            }
            int choiceIdCount = choiceIdStrings.Count;

            long storeId = context.CurrentStore.Id;
            var matchingItems = repository.Find().Where(y => y.StoreId == storeId);
            if (choiceIdStrings.Count > 0)
            {
                matchingItems = matchingItems.Where(y => choiceIdStrings.Contains(y.PropertyValue));
            }

            var match2 = matchingItems.GroupBy(y => y.ProductBvin)
                              .Select(grouping => grouping.Select(y => new CountResult { Bvin = y.ProductBvin, C = grouping.Count() })
                                                          .FirstOrDefault()).Where(y => y.C >= choiceIdCount)
                                                          .OrderBy(y => y.Bvin);

            result = match2.Count();

            return result;
        }
        public List<string> FindProductIdsMatchingKey(string key, int pageNumber, int pageSize)
        {
            List<long> choiceIds = CategoryFacetKeyHelper.ParseKeyToList(key);
            return FindProductsContainingAllChoiceIds(choiceIds, pageNumber, pageSize);
        }
        public int FindCountProductIdsMatchingKey(string key)
        {
            List<long> choiceIds = CategoryFacetKeyHelper.ParseKeyToList(key);
            return FindCountOfProductsContainingAllChoiceIds(choiceIds);
        }
        public bool DeleteByProductId(string productId)
        {
            List<ProductPropertyValue> items = FindByProductId(productId);
            foreach (ProductPropertyValue v in items)
            {
                Delete(v.Id);
            }
            return true;
        }
        public bool SetPropertyValue(string productId, long propertyId, string stringValue)
        {
            ProductPropertyValue v = new ProductPropertyValue();
            v.ProductID = productId;
            v.PropertyID = propertyId;
            v.StringValue = stringValue;
            return Create(v);                       
        }
        public string GetPropertyValue(string productID, long propertyId)
        {
            string result = string.Empty;
            ProductPropertyValue item = FindByProductIdAndPropertyId(productID, propertyId);
            if (item != null)
            {
                result = item.StringValue;
            }
            return result;
        }
     
    }
}
