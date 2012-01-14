using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MerchantTribe.Web.Data;
using MerchantTribe.Web.Logging;

namespace MerchantTribe.Commerce.Accounts
{
    public class ToDoItemRepository: ConvertingRepositoryBase<Data.EF.ToDoItem, ToDoItem>
    {

        private RequestContext context = null;
        
        public ToDoItemRepository(RequestContext c)
        {
            context = c;
            repository = new EntityFrameworkRepository<Data.EF.ToDoItem>(new Data.EF.EntityFrameworkDevConnectionString(c.ConnectionStringForEntityFramework));
            this.logger = new SupressLogger();
            repository.Logger = this.logger;
        }
        public ToDoItemRepository(RequestContext c, IRepositoryStrategy<Data.EF.ToDoItem> r)
        {
            context = c;
            repository = r;
            this.logger = new SupressLogger();
            repository.Logger = this.logger;
        }

        protected override void CopyModelToData(Data.EF.ToDoItem data, ToDoItem model)
        {
            data.AccountId = model.AccountId;
            data.Details = model.Details;
            data.Id = model.Id;
            data.IsComplete = model.IsComplete;
            data.SortOrder = model.SortOrder;
            data.Title = model.Title;
        }
        protected override void CopyDataToModel(Data.EF.ToDoItem data, ToDoItem model)
        {
            model.AccountId = data.AccountId;
            model.Details = data.Details;
            model.Id = data.Id;
            model.IsComplete = data.IsComplete;
            model.SortOrder = data.SortOrder;
            model.Title = data.Title;
        }

        public ToDoItem Find(long id)
        {
            return this.Find(new PrimaryKey(id));
        }
        public override bool Create(ToDoItem item)
        {
            if (item.AccountId == 0) return false;

            item.SortOrder = FindMaxSort(item.AccountId) + 1;

            return base.Create(item);
        }
        private int FindMaxSort(long accountId)
        {
            int maxSort = 0;
            long storeId = context.CurrentStore.Id;
            List<ToDoItem> result = new List<ToDoItem>();
            IQueryable<Data.EF.ToDoItem> items = repository.Find().Where(y => y.AccountId == accountId)
                                                                      .OrderByDescending(y => y.SortOrder)
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
        public bool Update(ToDoItem c)
        {
            return this.Update(c, new PrimaryKey(c.Id));
        }
        public bool Delete(long id)
        {
            return this.Delete(new PrimaryKey(id));
        }

        public List<ToDoItem> FindByAccountId(long accountId)
        {
            List<ToDoItem> result = new List<ToDoItem>();

            IQueryable<Data.EF.ToDoItem> items = repository.Find().Where(y => y.AccountId == accountId)
                                                                          .OrderBy(y => y.SortOrder);
            if (items != null)
            {
                result = ListPoco(items);
            }
            return result;
        }

        public bool Resort(long accountId, List<long> sortedIds)
        {
            if (sortedIds != null)
            {
                for (int i = 1; i <= sortedIds.Count; i++)
                {
                    UpdateSortOrder(accountId, sortedIds[i - 1], i);
                }
            }
            return true;
        }
        private bool UpdateSortOrder(long accountId, long itemId, int newSortOrder)
        {
            ToDoItem item = Find(itemId);
            if (item == null) return false;
            if (item.AccountId != accountId) return false;
            item.SortOrder = newSortOrder;
            return Update(item);
        }
    }
}
