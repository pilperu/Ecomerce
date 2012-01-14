using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MerchantTribe.Web;
using MerchantTribe.Web.Data;
using System.Data.Entity;
using MerchantTribe.Web.Logging;

namespace MerchantTribe.Commerce.Catalog
{
    public class CategoryProductAssociationRepository : ConvertingRepositoryBase<Data.EF.bvc_ProductXCategory, CategoryProductAssociation>
    {
        private RequestContext context = null;

        public static CategoryProductAssociationRepository InstantiateForMemory(RequestContext c)
        {
            return new CategoryProductAssociationRepository(c, new MemoryStrategy<Data.EF.bvc_ProductXCategory>(PrimaryKeyType.Long), new TextLogger());
        }
        public static CategoryProductAssociationRepository InstantiateForDatabase(RequestContext c)
        {
            return new CategoryProductAssociationRepository(c,
                new EntityFrameworkRepository<Data.EF.bvc_ProductXCategory>(
                    new Data.EF.EntityFrameworkDevConnectionString(c.ConnectionStringForEntityFramework)),
                    new SupressLogger()
                    );
        }
        public CategoryProductAssociationRepository(RequestContext c, IRepositoryStrategy<Data.EF.bvc_ProductXCategory> r, ILogger log)
        {
            context = c;
            repository = r;
            this.logger = log;
            repository.Logger = this.logger;
        }

        protected override void CopyDataToModel(Data.EF.bvc_ProductXCategory data, CategoryProductAssociation model)
        {
            model.Id = data.Id;
            model.CategoryId = data.CategoryId;
            model.ProductId = data.ProductId;
            model.SortOrder = data.SortOrder;
            model.StoreId = data.StoreId;
        }
        protected override void CopyModelToData(Data.EF.bvc_ProductXCategory data, CategoryProductAssociation model)
        {
            data.Id = model.Id;
            data.CategoryId = model.CategoryId;
            data.ProductId = model.ProductId;
            data.SortOrder = model.SortOrder;
            data.StoreId = model.StoreId;
        }

        public CategoryProductAssociation Find(long id)
        {
            Data.EF.bvc_ProductXCategory data = repository.FindByPrimaryKey(new PrimaryKey(id));
            if (data == null) return null;

            CategoryProductAssociation result = new CategoryProductAssociation();
            CopyDataToModel(data, result);
            return result;
        }
        public CategoryProductAssociation FindByCategoryAndProduct(string categoryId, string productId)
        {
            return FindByCategoryAndProduct(categoryId, productId, context.CurrentStore.Id);
        }
        public CategoryProductAssociation FindByCategoryAndProduct(string categoryId, string productId, long storeId)
        {
            IQueryable<Data.EF.bvc_ProductXCategory> data = repository.Find().Where(y => y.CategoryId == categoryId)
                                                                             .Where(y => y.ProductId == productId)
                                                                             .Where(y => y.StoreId == storeId).OrderBy(y => y.SortOrder);
            if (data.Count() > 0) return SinglePoco(data);
            return null;
        }
    
        public override bool Create(CategoryProductAssociation item)
        {
            item.StoreId = context.CurrentStore.Id;
            item.SortOrder = FindMaxSort(item.CategoryId) + 1;
            return base.Create(item);
        }
        private int FindMaxSort(string categoryId)
        {
            int maxSort = 0;
            long storeId = context.CurrentStore.Id;
            List<CategoryProductAssociation> result = new List<CategoryProductAssociation>();
            IQueryable<Data.EF.bvc_ProductXCategory> items = repository.Find().Where(y => y.CategoryId == categoryId)
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
        public bool Update(CategoryProductAssociation c)
        {
            if (c.StoreId != context.CurrentStore.Id)
            {
                return false;
            }
            return this.Update(c, new PrimaryKey(c.Id));
        }

        public List<CategoryProductAssociation> FindAll()
        {
            long storeId = context.CurrentStore.Id;
            IQueryable<Data.EF.bvc_ProductXCategory> result = repository.Find().Where(y => y.StoreId == storeId).OrderBy(y => y.SortOrder);
            return ListPoco(result);
        }
        public List<CategoryProductAssociation> FindAllForAllStores()
        {
            return this.FindAllPagedForAllStores(1, int.MaxValue);
        }
        public new List<CategoryProductAssociation> FindAllPaged(int pageNumber, int pageSize)
        {
            List<CategoryProductAssociation> result = new List<CategoryProductAssociation>();

            if (pageNumber < 1) pageNumber = 1;

            int take = pageSize;
            int skip = (pageNumber - 1) * pageSize;
            long storeId = context.CurrentStore.Id;

            IQueryable<Data.EF.bvc_ProductXCategory> items = repository.Find().Where(y => y.StoreId == storeId).OrderBy(y => y.SortOrder).Skip(skip).Take(take);
            if (items != null)
            {
                result = ListPoco(items);
            }

            return result;
        }
        public List<CategoryProductAssociation> FindAllPagedForAllStores(int pageNumber, int pageSize)
        {
            List<CategoryProductAssociation> result = new List<CategoryProductAssociation>();

            if (pageNumber < 1) pageNumber = 1;

            int take = pageSize;
            int skip = (pageNumber - 1) * pageSize;

            IQueryable<Data.EF.bvc_ProductXCategory> items = repository.Find().OrderBy(y => y.SortOrder).Skip(skip).Take(take);
            if (items != null)
            {
                result = ListPoco(items);
            }

            return result;
        }

        public List<CategoryProductAssociation> FindForCategory(string categoryId, int pageNumber, int pageSize)
        {
            List<CategoryProductAssociation> result = new List<CategoryProductAssociation>();

            if (pageNumber < 1) pageNumber = 1;

            int take = pageSize;
            int skip = (pageNumber - 1) * pageSize;
            long storeId = context.CurrentStore.Id;

            IQueryable<Data.EF.bvc_ProductXCategory> items = repository.Find().Where(y => y.CategoryId == categoryId)
                                                                              .Where(y => y.StoreId == storeId)
                                                                              .OrderBy(y => y.SortOrder).Skip(skip).Take(take);
            if (items != null)
            {
                result = ListPoco(items);
            }

            return result;
        }
        public List<CategoryProductAssociation> FindForProduct(string productId, int pageNumber, int pageSize)
        {
            List<CategoryProductAssociation> result = new List<CategoryProductAssociation>();

            if (pageNumber < 1) pageNumber = 1;

            int take = pageSize;
            int skip = (pageNumber - 1) * pageSize;
            long storeId = context.CurrentStore.Id;

            IQueryable<Data.EF.bvc_ProductXCategory> items = repository.Find().Where(y => y.ProductId == productId)
                                                                              .Where(y => y.StoreId == storeId)
                                                                              .OrderBy(y => y.SortOrder).Skip(skip).Take(take);
            if (items != null)
            {
                result = ListPoco(items);
            }

            return result;
        }

        public bool DeleteAllForCategory(string categoryId)
        {
            long storeId = context.CurrentStore.Id;

            IQueryable<Data.EF.bvc_ProductXCategory> items = repository.Find().Where(y => y.CategoryId == categoryId)
                                                                              .Where(y => y.StoreId == storeId)
                                                                              .OrderBy(y => y.SortOrder);
            
            List<PrimaryKey> keysToDelete = new List<PrimaryKey>();

            if (items != null)
            {
                foreach (Data.EF.bvc_ProductXCategory item in items)
                {
                    keysToDelete.Add(new PrimaryKey(item.Id));
                }
                
            }
            repository.SubmitChanges();

            foreach (PrimaryKey key in keysToDelete)
            {
                repository.Delete(key);
            }
            repository.SubmitChanges();

            return true;
        }

        public bool DeleteAllForProduct(string productId)
        {
            long storeId = context.CurrentStore.Id;

            IQueryable<Data.EF.bvc_ProductXCategory> items = repository.Find().Where(y => y.ProductId == productId)
                                                                              .Where(y => y.StoreId == storeId)
                                                                              .OrderBy(y => y.SortOrder);

            
            if (items != null)
            {
                // Pull out to list to close transaction of read before
                // possible transaction of delete in EF
                List<Data.EF.bvc_ProductXCategory> itemList = items.ToList();
                repository.SubmitChanges();

                foreach (Data.EF.bvc_ProductXCategory item in itemList)
                {
                    repository.Delete(new PrimaryKey(item.Id));
                }
                repository.SubmitChanges();
            }

            return true;
        }

        public bool AddProductToCategory(string productId, string categoryId)
        {                        
            long storeId = context.CurrentStore.Id;

            Data.EF.bvc_ProductXCategory exists = repository.Find().Where(y => y.CategoryId == categoryId)
                                                                               .Where(y => y.ProductId == productId)
                                                                              .Where(y => y.StoreId == storeId)
                                                                              .SingleOrDefault();                                                                              
            if (exists == null)
            {
                CategoryProductAssociation x = new CategoryProductAssociation();
                x.ProductId = productId;
                x.CategoryId = categoryId;
                return Create(x);
            }

            return true;
        }

        public bool RemoveProductFromCategory(string productId, string categoryId)
        {
            long storeId = context.CurrentStore.Id;

            Data.EF.bvc_ProductXCategory exists = repository.Find().Where(y => y.CategoryId == categoryId)
                                                                               .Where(y => y.ProductId == productId)
                                                                              .Where(y => y.StoreId == storeId)
                                                                              .SingleOrDefault();            

            if (exists != null)
            {
                return Delete(new PrimaryKey(exists.Id));
            }

            return true;
        }



        private bool UpdateSortOrderForProduct(string categoryId, string productId, int newSortOrder)
        {
            Catalog.CategoryProductAssociation c1 = FindByCategoryAndProduct(categoryId, productId);
            if (c1 == null) return false;                        
            c1.SortOrder = newSortOrder;
            return Update(c1);                                    
        }

        //private bool SwapOrder(string categoryId, string currentId, int currentSort, string targetId, int targetSort)
        //{
        //    bool result = false;

        //    // Update Target
        //    result = UpdateSortOrderForProduct(categoryId, targetId, currentSort);

        //    // Update Current
        //    result = UpdateSortOrderForProduct(categoryId, currentId, targetSort);

        //    return result;
        //}

        //public bool MoveProductUpInCategory(string categoryId, string productId)
        //{
        //    bool result = false;

        //    Collection<Catalog.CategoryProductAssociation> peers = new Collection<Catalog.CategoryProductAssociation>();
        //    peers = Datalayer.CategoryProductAssociationMapper.FindByCategory(categoryId);

        //    if (peers != null)
        //    {

        //        int currentSort = 0;
        //        string currentId = productId;
        //        int targetSort = 0;
        //        string targetId = string.Empty;
        //        bool foundTarget = false;

        //        // Find current and Target Information
        //        for (int i = 0; i <= peers.Count - 1; i++)
        //        {
        //            if (peers[i].ProductId == productId)
        //            {
        //                foundTarget = true;
        //                currentSort = peers[i].SortOrder;
        //            }
        //            else
        //            {
        //                if (foundTarget == false)
        //                {
        //                    targetSort = peers[i].SortOrder;
        //                    targetId = peers[i].ProductId;
        //                }
        //            }
        //        }

        //        // Swap Sort Order
        //        if (foundTarget == true)
        //        {
        //            SwapOrder(categoryId, currentId, currentSort, targetId, targetSort);
        //        }

        //    }

        //    peers = null;

        //    return result;
        //}
        //public bool MoveProductDownInCategory(string categoryId, string productId)
        //{
        //    bool result = false;

        //    Collection<Catalog.CategoryProductAssociation> peers = new Collection<Catalog.CategoryProductAssociation>();
        //    peers = Datalayer.CategoryProductAssociationMapper.FindByCategory(categoryId);

        //    if (peers != null)
        //    {

        //        int currentSort = 0;
        //        string currentId = productId;
        //        int targetSort = 0;
        //        string targetId = string.Empty;
        //        bool foundCurrent = false;
        //        bool foundTarget = false;

        //        // Find current and Target Information
        //        for (int i = 0; i <= peers.Count - 1; i++)
        //        {
        //            if (foundCurrent == true)
        //            {
        //                targetId = peers[i].ProductId;
        //                targetSort = peers[i].SortOrder;
        //                foundCurrent = false;
        //                foundTarget = true;
        //            }
        //            if (peers[i].ProductId == productId)
        //            {
        //                foundCurrent = true;
        //                currentSort = peers[i].SortOrder;
        //            }
        //        }

        //        // Swap Sort Order
        //        if (foundTarget == true)
        //        {
        //            SwapOrder(categoryId, currentId, currentSort, targetId, targetSort);
        //        }

        //    }

        //    peers = null;

        //    return result;
        //}

        public bool ResortProducts(string categoryId, List<string> sortedIds)
        {
            if (sortedIds != null)
            {
                for (int i = 1; i <= sortedIds.Count; i++)
                {
                    UpdateSortOrderForProduct(categoryId, sortedIds[i - 1], i);
                }
            }
            return true;
        }

    }
}
