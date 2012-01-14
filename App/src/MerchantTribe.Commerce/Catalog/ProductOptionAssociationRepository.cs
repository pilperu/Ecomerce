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
    public class ProductOptionAssociationRepository : ConvertingRepositoryBase<Data.EF.bvc_ProductXOption, ProductOptionAssociation>
    {
        private RequestContext context = null;

        public static ProductOptionAssociationRepository InstantiateForMemory(RequestContext c)
        {
            return new ProductOptionAssociationRepository(c, new MemoryStrategy<Data.EF.bvc_ProductXOption>(PrimaryKeyType.Long), new TextLogger());
        }
        public static ProductOptionAssociationRepository InstantiateForDatabase(RequestContext c)
        {
            return new ProductOptionAssociationRepository(c,
                new EntityFrameworkRepository<Data.EF.bvc_ProductXOption>(
                    new Data.EF.EntityFrameworkDevConnectionString(c.ConnectionStringForEntityFramework)),
                    new SupressLogger()
                    );
        }
        public ProductOptionAssociationRepository(RequestContext c, IRepositoryStrategy<Data.EF.bvc_ProductXOption> r, ILogger log)
        {
            context = c;
            repository = r;
            this.logger = log;
            repository.Logger = this.logger;
        }

        protected override void CopyDataToModel(Data.EF.bvc_ProductXOption data, ProductOptionAssociation model)
        {
            model.Id = data.Id;
            model.OptionBvin = data.OptionBvin;
            model.ProductBvin = data.ProductBvin;
            model.SortOrder = data.SortOrder;
            model.StoreId = data.StoreId;            
        }
        protected override void CopyModelToData(Data.EF.bvc_ProductXOption data, ProductOptionAssociation model)
        {
            data.Id = model.Id;
            data.OptionBvin = model.OptionBvin;
            data.ProductBvin = model.ProductBvin;
            data.SortOrder = model.SortOrder;
            data.StoreId = model.StoreId;            
        }

        public ProductOptionAssociation Find(long id)
        {
            Data.EF.bvc_ProductXOption data = repository.FindByPrimaryKey(new PrimaryKey(id));
            if (data == null) return null;

            ProductOptionAssociation result = new ProductOptionAssociation();
            CopyDataToModel(data, result);
            return result;
        }
        public ProductOptionAssociation FindByProductAndOption(string productId, string optionId)
        {
            return FindByProductAndOption(productId, optionId, context.CurrentStore.Id);
        }
        public ProductOptionAssociation FindByProductAndOption(string productId, string optionId, long storeId)
        {
            IQueryable<Data.EF.bvc_ProductXOption> data = repository.Find().Where(y => y.ProductBvin == productId)
                                                                             .Where(y => y.OptionBvin == optionId)
                                                                             .Where(y => y.StoreId == storeId).OrderBy(y => y.SortOrder);
            if (data.Count() > 0) return SinglePoco(data);
            return null;
        }

        public override bool Create(ProductOptionAssociation item)
        {
            item.StoreId = context.CurrentStore.Id;
            item.SortOrder = FindMaxSort(item.ProductBvin) + 1;
            return base.Create(item);
        }
        private int FindMaxSort(string productId)
        {
            int maxSort = 0;
            long storeId = context.CurrentStore.Id;
            List<ProductOptionAssociation> result = new List<ProductOptionAssociation>();
            IQueryable<Data.EF.bvc_ProductXOption> items = repository.Find().Where(y => y.ProductBvin == productId)
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
        public bool Update(ProductOptionAssociation c)
        {
            if (c.StoreId != context.CurrentStore.Id)
            {
                return false;
            }
            return this.Update(c, new PrimaryKey(c.Id));
        }

        public List<ProductOptionAssociation> FindAll()
        {
            long storeId = context.CurrentStore.Id;
            IQueryable<Data.EF.bvc_ProductXOption> result = repository.Find().Where(y => y.StoreId == storeId).OrderBy(y => y.SortOrder);
            return ListPoco(result);
        }
        public List<ProductOptionAssociation> FindAllForAllStores()
        {
            return this.FindAllPagedForAllStores(1, int.MaxValue);
        }
        public new List<ProductOptionAssociation> FindAllPaged(int pageNumber, int pageSize)
        {
            List<ProductOptionAssociation> result = new List<ProductOptionAssociation>();

            if (pageNumber < 1) pageNumber = 1;

            int take = pageSize;
            int skip = (pageNumber - 1) * pageSize;
            long storeId = context.CurrentStore.Id;

            IQueryable<Data.EF.bvc_ProductXOption> items = repository.Find().Where(y => y.StoreId == storeId).OrderBy(y => y.SortOrder).Skip(skip).Take(take);
            if (items != null)
            {
                result = ListPoco(items);
            }

            return result;
        }
        public List<ProductOptionAssociation> FindAllPagedForAllStores(int pageNumber, int pageSize)
        {
            List<ProductOptionAssociation> result = new List<ProductOptionAssociation>();

            if (pageNumber < 1) pageNumber = 1;

            int take = pageSize;
            int skip = (pageNumber - 1) * pageSize;

            IQueryable<Data.EF.bvc_ProductXOption> items = repository.Find().OrderBy(y => y.SortOrder).Skip(skip).Take(take);
            if (items != null)
            {
                result = ListPoco(items);
            }

            return result;
        }

        public List<ProductOptionAssociation> FindForProduct(string productId, int pageNumber, int pageSize)
        {
            List<ProductOptionAssociation> result = new List<ProductOptionAssociation>();

            if (pageNumber < 1) pageNumber = 1;

            int take = pageSize;
            int skip = (pageNumber - 1) * pageSize;
            long storeId = context.CurrentStore.Id;

            IQueryable<Data.EF.bvc_ProductXOption> items = repository.Find().Where(y => y.ProductBvin == productId)
                                                                              .Where(y => y.StoreId == storeId)
                                                                              .OrderBy(y => y.SortOrder).Skip(skip).Take(take);
            if (items != null)
            {
                result = ListPoco(items);
            }

            return result;
        }
        public List<ProductOptionAssociation> FindForOption(string optionId, int pageNumber, int pageSize)
        {
            List<ProductOptionAssociation> result = new List<ProductOptionAssociation>();

            if (pageNumber < 1) pageNumber = 1;

            int take = pageSize;
            int skip = (pageNumber - 1) * pageSize;
            long storeId = context.CurrentStore.Id;

            IQueryable<Data.EF.bvc_ProductXOption> items = repository.Find().Where(y => y.OptionBvin == optionId)
                                                                              .Where(y => y.StoreId == storeId)
                                                                              .OrderBy(y => y.SortOrder).Skip(skip).Take(take);
            if (items != null)
            {
                result = ListPoco(items);
            }

            return result;
        }

        public bool DeleteAllForProduct(string productId)
        {
            long storeId = context.CurrentStore.Id;

            IQueryable<Data.EF.bvc_ProductXOption> items = repository.Find().Where(y => y.ProductBvin == productId)
                                                                              .Where(y => y.StoreId == storeId)
                                                                              .OrderBy(y => y.SortOrder);
            
            List<PrimaryKey> keysToDelete = new List<PrimaryKey>();

            if (items != null)
            {
                foreach (Data.EF.bvc_ProductXOption item in items)
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
        public bool DeleteAllForOption(string optionId)
        {
            long storeId = context.CurrentStore.Id;

            IQueryable<Data.EF.bvc_ProductXOption> items = repository.Find().Where(y => y.OptionBvin == optionId)
                                                                              .Where(y => y.StoreId == storeId)
                                                                              .OrderBy(y => y.SortOrder);

            
            if (items != null)
            {
                // Pull out to list to close transaction of read before
                // possible transaction of delete in EF
                List<Data.EF.bvc_ProductXOption> itemList = items.ToList();
                repository.SubmitChanges();

                foreach (Data.EF.bvc_ProductXOption item in itemList)
                {
                    repository.Delete(new PrimaryKey(item.Id));
                }
                repository.SubmitChanges();
            }

            return true;
        }

        public bool AddOptionToProduct(string productId, string optionId)
        {                        
            long storeId = context.CurrentStore.Id;

            Data.EF.bvc_ProductXOption exists = repository.Find().Where(y => y.ProductBvin == productId)
                                                                               .Where(y => y.OptionBvin == optionId)
                                                                              .Where(y => y.StoreId == storeId)
                                                                              .SingleOrDefault();                                                                              
            if (exists == null)
            {
                ProductOptionAssociation x = new ProductOptionAssociation();
                x.ProductBvin = productId;
                x.OptionBvin = optionId;
                return Create(x);
            }

            return true;
        }
        public bool RemoveOptionFromProduct(string productId, string optionId)
        {
            long storeId = context.CurrentStore.Id;

            Data.EF.bvc_ProductXOption exists = repository.Find().Where(y => y.ProductBvin == productId)
                                                                               .Where(y => y.OptionBvin == optionId)
                                                                              .Where(y => y.StoreId == storeId)
                                                                              .SingleOrDefault();            

            if (exists != null)
            {
                return Delete(new PrimaryKey(exists.Id));
            }

            return true;
        }

        private bool UpdateSortOrderForOption(string productId, string optionId, int newSortOrder)
        {
            ProductOptionAssociation c1 = FindByProductAndOption(productId, optionId);
            if (c1 == null) return false;                        
            c1.SortOrder = newSortOrder;
            return Update(c1);                                    
        }
        public bool ResortOptionsForProduct(string productId, List<string> sortedIds)
        {
            if (sortedIds != null)
            {
                for (int i = 1; i <= sortedIds.Count; i++)
                {
                    UpdateSortOrderForOption(productId, sortedIds[i - 1], i);
                }
            }
            return true;
        }

    }
}
