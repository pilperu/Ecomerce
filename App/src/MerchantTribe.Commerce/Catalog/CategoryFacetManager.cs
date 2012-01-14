using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Objects;
using MerchantTribe.Web.Data;
using MerchantTribe.Web.Logging;

namespace MerchantTribe.Commerce.Catalog
{
    public class CategoryFacetManager: ConvertingRepositoryBase<Data.EF.bvc_CategoryXProperty, CategoryFacet>
    {
        private RequestContext context = null;
        private ProductPropertyValueRepository productValueRepository = null;

        public static CategoryFacetManager InstantiateForMemory(RequestContext c)
        {
            CategoryFacetManager result = null;
            result = new CategoryFacetManager(c,
                        new MemoryStrategy<Data.EF.bvc_CategoryXProperty>(PrimaryKeyType.Long),
                     new MemoryStrategy<Data.EF.bvc_ProductPropertyValue>(PrimaryKeyType.Long));
            return result;
        }
        public static CategoryFacetManager InstantiateForDatabase(RequestContext c)
        {
            CategoryFacetManager result = null;
            result = new CategoryFacetManager(c,
                     new EntityFrameworkRepository<Data.EF.bvc_CategoryXProperty>(new Data.EF.EntityFrameworkDevConnectionString(c.ConnectionStringForEntityFramework)),
                     new EntityFrameworkRepository<Data.EF.bvc_ProductPropertyValue>(new Data.EF.EntityFrameworkDevConnectionString(c.ConnectionStringForEntityFramework)));            
            return result;
        }
        public CategoryFacetManager(RequestContext c, IRepositoryStrategy<Data.EF.bvc_CategoryXProperty> r,
                                    IRepositoryStrategy<Data.EF.bvc_ProductPropertyValue> valueRepo)
        {
            context = c;
            repository = r;
            this.logger = new SupressLogger();
            repository.Logger = this.logger;
            this.productValueRepository = new ProductPropertyValueRepository(c, valueRepo, this.logger);
        }

        protected override void CopyDataToModel(Data.EF.bvc_CategoryXProperty data, CategoryFacet model)
        {
            model.CategoryId = data.CategoryId;
            model.DisplayMode = (CategoryFacetDisplayMode)data.DisplayMode;
            model.FilterName = data.FilterName;
            model.Id = data.Id;
            model.ParentPropertyId = data.ParentPropertyId;
            model.PropertyId = data.PropertyId;
            model.SortOrder = data.SortOrder;
            model.StoreId = data.StoreId;      
        }
        protected override void CopyModelToData(Data.EF.bvc_CategoryXProperty data, CategoryFacet model)
        {
            data.CategoryId = model.CategoryId;
            data.DisplayMode = (int)model.DisplayMode;
            data.FilterName = model.FilterName;
            data.Id = model.Id;
            data.ParentPropertyId = model.ParentPropertyId;
            data.PropertyId = model.PropertyId;
            data.SortOrder = model.SortOrder;
            data.StoreId = model.StoreId;
        }

        public override bool Create(CategoryFacet item)
        {
            item.StoreId = context.CurrentStore.Id;
            return base.Create(item);
        }
        public bool Update(CategoryFacet c)
        {
            return this.Update(c, new PrimaryKey(c.Id));
        }
        public bool Delete(long id)
        {
            long storeId = context.CurrentStore.Id;
            CategoryFacet item = Find(id);
            if (item == null) return false;
            if (item.StoreId != storeId) return false;
            return Delete(new PrimaryKey(id));
        }
      
        public CategoryFacet Find(long id)
        {

            CategoryFacet result = FindForAllStores(id);
            if (result != null)
            {
                if (result.StoreId == context.CurrentStore.Id)
                {
                    return result;
                }
            }
            return null;            
        }
        public CategoryFacet FindForAllStores(long id)
        {
            Data.EF.bvc_CategoryXProperty data = repository.FindByPrimaryKey(new PrimaryKey(id));
            if (data == null) return null;
            CategoryFacet result = new CategoryFacet();
            CopyDataToModel(data, result);
            return result;
        }

        public List<CategoryFacet> FindByParentInList(List<CategoryFacet> all, long parentId)
        {
            List<CategoryFacet> result = new List<CategoryFacet>();

            var x = (from n in all
                     where n.ParentPropertyId == parentId
                     orderby n.SortOrder
                     select n).ToList();
            if (x != null)
            {
                result = (List<CategoryFacet>)x;
            }

            return result;
        }

        public List<CategoryFacet> FindByParent(long parentId)
        {
            List<CategoryFacet> result = new List<CategoryFacet>();

            IQueryable<Data.EF.bvc_CategoryXProperty> items = repository.Find().Where(y => y.StoreId == context.CurrentStore.Id)
                                                                            .Where(y => y.ParentPropertyId == parentId)
                                                                          .OrderBy(y => y.SortOrder);
            if (items != null)
            {
                result = ListPoco(items);
            }

            return result;           
        }

        public List<CategoryFacet> FindByCategory(string categoryBvin)
        {
            List<CategoryFacet> result = new List<CategoryFacet>();

            IQueryable<Data.EF.bvc_CategoryXProperty> items = repository.Find().Where(y => y.StoreId == context.CurrentStore.Id)
                                                                            .Where(y => y.CategoryId == categoryBvin)
                                                                          .OrderBy(y => y.SortOrder);
            if (items != null)
            {
                result = ListPoco(items);
            }

            return result;    
        }

        public int FindMaxSortForCategoryParent(string categoryBvin, long parentPropertyId)
        {
            int result = 0;

            try
            {

                result = repository.Find().Where(y => y.StoreId == context.CurrentStore.Id)
                                          .Where(y => y.CategoryId == categoryBvin)
                                          .Where(y => y.ParentPropertyId == parentPropertyId)
                                          .Max(y => y.SortOrder);
                if (result < 0) result = 0;
            }
            catch (Exception ex)
            {
                EventLog.LogEvent(ex);
            }

            return result;
        }

        public List<ProductFacetCount> FindCountsOfVisibleFacets(string key, 
                                                                List<CategoryFacet> allFacets, 
                                                                List<ProductProperty> properties)
        {
            List<long> visibleFacets = FindVisibleFacetsIdsForKey(key, allFacets);
            List<string> sqlKeys = new List<string>();
            for (int i = 0; i < allFacets.Count; i++)
            {
                if (visibleFacets.Contains(allFacets[i].Id))
                {
                    if (!IsFacetSelectedInKey(key, allFacets, allFacets[i].Id))
                    {
                        // It's a visible facet, not selected 
                        // so generate all possible SQL keys for choices
                        
                        var p = (from pr in properties
                                 where pr.Id == allFacets[i].PropertyId
                                 select pr).SingleOrDefault();
                        if (p != null)
                        {
                            foreach (ProductPropertyChoice c in p.Choices)
                            {
                                string updatedKey = CategoryFacetKeyHelper.ReplaceKeyValue(key, i, c.Id);
                                sqlKeys.Add(CategoryFacetKeyHelper.ParseKeyToSqlList(updatedKey));
                            }
                        }
                    }
                }                
            }

            return FindProductCountsForKeys(sqlKeys);            
        }
        public List<ProductFacetCount> FindProductCountsForKeys(List<string> sqlKeys)
        {
            List<ProductFacetCount> result = new List<ProductFacetCount>();
            foreach (string key in sqlKeys)
            {
                ProductFacetCount f = new ProductFacetCount();
                f.Key = key;
                f.ProductCount = productValueRepository.FindCountProductIdsMatchingKey(key);
                result.Add(f);
            }
            return result;
        }

        public List<long> FindVisibleFacetsIdsForKey(string key, List<CategoryFacet> allFacets)
        {
            List<long> result = new List<long>();

            result = FindVisibleChildren(key, allFacets, 0);

            return result;
        }                
        private List<long> FindVisibleChildren(string key, List<CategoryFacet> allFacets, long parentId)
        {
            List<long> result = new List<long>();

            bool parentIsSelected = IsFacetSelectedInKey(key, allFacets, parentId);

            if (parentIsSelected || parentId == 0)
            {
                foreach (CategoryFacet f in FindByParentInList(allFacets, parentId))
                {
                    result.Add(f.Id);
                    List<long> visibleChildren = FindVisibleChildren(key, allFacets, f.Id);
                    if (visibleChildren != null)
                    {
                        if (visibleChildren.Count > 0)
                        {
                            result.AddRange(visibleChildren);
                        }
                    }                    
                }
            }

            return result;
        }
        public bool IsFacetSelectedInKey(string key, List<CategoryFacet> allFacets, long facetId)
        {
            if (key == string.Empty) return false;

            bool result = false;

            List<long> keyparts = CategoryFacetKeyHelper.ParseKeyToList(key);
            for (int i = 0; i < allFacets.Count; i++)
            {
                if (allFacets[i].Id == facetId)
                {
                    if (keyparts[i] > 0)
                    {
                        return true;
                    }
                }
            }

            return result;
        }
    }
}
