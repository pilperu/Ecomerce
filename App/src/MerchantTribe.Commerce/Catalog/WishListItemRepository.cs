using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MerchantTribe.Web.Data;
using MerchantTribe.Web.Logging;

namespace MerchantTribe.Commerce.Catalog
{
    public class WishListItemRepository: ConvertingRepositoryBase<Data.EF.WishListItem, WishListItem>
    {
        private RequestContext context = null;

        public static WishListItemRepository InstantiateForMemory(RequestContext c)
        {
            return new WishListItemRepository(c, new MemoryStrategy<Data.EF.WishListItem>(PrimaryKeyType.Long), new NullLogger());
        }
        public static WishListItemRepository InstantiateForDatabase(RequestContext c)
        {
            WishListItemRepository result = null;
            result = new WishListItemRepository(c,
                new EntityFrameworkRepository<Data.EF.WishListItem>(
                    new Data.EF.EntityFrameworkDevConnectionString(c.ConnectionStringForEntityFramework)),
                    new SupressLogger());
            return result;
        }
        public WishListItemRepository(RequestContext c, IRepositoryStrategy<Data.EF.WishListItem> r, ILogger log)
        {
            context = c;
            repository = r;
            this.logger = log;
            repository.Logger = this.logger;
        }

        protected override void CopyDataToModel(Data.EF.WishListItem data, WishListItem model)
        {
            model.Id = data.Id;
            model.CustomerId = data.CustomerId;
            model.LastUpdatedUtc = data.LastUpdated;
            model.ProductId = data.ProductId;
            model.Quantity = data.Quantity;
            model.SelectionData.DeserializeFromXml(data.SelectionData);            
            model.StoreId = data.StoreId;                        
        }
        protected override void CopyModelToData(Data.EF.WishListItem data, WishListItem model)
        {
            data.Id = model.Id;
            data.CustomerId = model.CustomerId;
            data.LastUpdated = model.LastUpdatedUtc;
            data.ProductId = model.ProductId;
            data.Quantity = model.Quantity;
            data.SelectionData = model.SelectionData.SerializeToXml();
            data.StoreId = model.StoreId;
        }

        public WishListItem Find(long id)
        {
            WishListItem result = FindForAllStores(id);
            if (result != null)
            {
                if (result.StoreId == context.CurrentStore.Id)
                {
                    return result;
                }
            }
            return null;
        }
        public WishListItem FindForAllStores(long id)
        {
            Data.EF.WishListItem data = repository.FindByPrimaryKey(new PrimaryKey(id));
            if (data == null) return null;

            WishListItem result = new WishListItem();
            CopyDataToModel(data, result);
            return result;
        }
        public override bool Create(WishListItem item)
        {
            item.StoreId = context.CurrentStore.Id;
 	        return base.Create(item);
        }
        public bool Update(WishListItem c)
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
            WishListItem item = Find(id);
            if (item == null) return false;
            return Delete(new PrimaryKey(id));            
        }

        public List<WishListItem> FindByProductId(string productId)
        {
            return FindByProductIdPaged(productId, 1, int.MaxValue);
        }
        public List<WishListItem> FindByProductIdPaged(string productId, int pageNumber, int pageSize)
        {
            List<WishListItem> result = new List<WishListItem>();

            if (pageNumber < 1) pageNumber = 1;

            int take = pageSize;
            int skip = (pageNumber - 1) * pageSize;
            long storeId = context.CurrentStore.Id;

            IQueryable<Data.EF.WishListItem> items = repository.Find().Where(y => y.ProductId == productId)
                                                                          .Where(y => y.StoreId == storeId)       
                                                                          .OrderBy(y => y.ProductId)
                                                                          .Skip(skip)
                                                                          .Take(take);
            if (items != null)
            {
                result = ListPoco(items);
            }

            return result;
        }
        public List<WishListItem> FindByCustomerIdPaged(string customerId, int pageNumber, int pageSize)
        {
            List<WishListItem> result = new List<WishListItem>();

            if (pageNumber < 1) pageNumber = 1;

            int take = pageSize;
            int skip = (pageNumber - 1) * pageSize;
            long storeId = context.CurrentStore.Id;

            IQueryable<Data.EF.WishListItem> items = repository.Find().Where(y => y.CustomerId == customerId)
                                                                          .Where(y => y.StoreId == storeId)
                                                                          .OrderByDescending(y => y.LastUpdated)
                                                                          .Skip(skip)
                                                                          .Take(take);
            if (items != null)
            {
                result = ListPoco(items);
            }

            return result;
        }
        public bool DeleteForProductId(string productBvin)
        {
            List<WishListItem> items = FindByProductId(productBvin);
            if (items != null)
            {
                foreach (WishListItem v in items)
                {
                    Delete(v.Id);
                }
            }
            return true;
        }
        public bool DeleteForCustomerId(string customerId)
        {
            List<WishListItem> items = FindByCustomerIdPaged(customerId, 1, int.MaxValue);
            if (items != null)
            {
                foreach (WishListItem v in items)
                {
                    Delete(v.Id);
                }
            }
            return true;
        }

    }
}

