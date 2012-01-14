using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MerchantTribe.Web.Data;
using MerchantTribe.Web.Logging;

namespace MerchantTribe.Commerce.Scheduling
{
    public class QueuedTaskRepository : ConvertingRepositoryBase<Data.EF.QueuedTask, QueuedTask>
    {

        public static QueuedTaskRepository InstantiateForMemory(RequestContext c)
        {
            QueuedTaskRepository result = null;
            result = new QueuedTaskRepository(c, new MemoryStrategy<Data.EF.QueuedTask>(PrimaryKeyType.Long),
                                           new TextLogger());
            return result;
        }
        public static QueuedTaskRepository InstantiateForDatabase(RequestContext c)
        {
            QueuedTaskRepository result = null;
            result = new QueuedTaskRepository(c,
                new EntityFrameworkRepository<Data.EF.QueuedTask>(
                    new Data.EF.EntityFrameworkDevConnectionString(c.ConnectionStringForEntityFramework)),
                    new SupressLogger()
                    );
            return result;
        }
        private RequestContext context = null;
        private QueuedTaskRepository(RequestContext c, IRepositoryStrategy<Data.EF.QueuedTask> r,
                                    ILogger log)
        {
            context = c;
            repository = r;
            this.logger = log;
            repository.Logger = this.logger;
        }
        protected override void CopyDataToModel(Data.EF.QueuedTask data, QueuedTask model)
        {
            model.Id = data.Id;
            model.StoreId = data.StoreId;
            model.FriendlyName = data.FriendlyName;
            model.Payload = data.Payload;
            model.StartAtUtc = data.StartAtUtc;
            model.Status = (QueuedTaskStatus)data.Status;
            model.StatusNotes = data.StatusNotes;
            model.TaskProcessorId = data.TaskProcessorId;
            model.TaskProcessorName = data.TaskProcessorName;                        
        }
        protected override void CopyModelToData(Data.EF.QueuedTask data, QueuedTask model)
        {
            data.Id = model.Id;
            data.StoreId = model.StoreId;
            data.FriendlyName = model.FriendlyName;
            data.Payload = model.Payload;
            data.StartAtUtc = model.StartAtUtc;
            data.Status = (int)model.Status;
            data.StatusNotes = model.StatusNotes;
            data.TaskProcessorId = model.TaskProcessorId;
            data.TaskProcessorName = model.TaskProcessorName;                        
        }

        public QueuedTask Find(long id)
        {
            QueuedTask result = FindForAllStores(id);
            if (result != null)
            {
                if (result.StoreId == context.CurrentStore.Id)
                {
                    return result;
                }
            }
            return null;
        }
        public QueuedTask FindNextQueuedByProcessorId(Guid processorId)
        {
            long storeId = context.CurrentStore.Id;
            var task = repository.Find().Where(y => y.StoreId == storeId)
                                        .Where(y => y.Status == (int)QueuedTaskStatus.Pending)
                                        .Where(y => y.TaskProcessorId == processorId)
                                        .OrderBy(y => y.StartAtUtc);            
            QueuedTask result = FirstPoco(task);            
            if (result == null) return null;            
            return result;
        }
        public QueuedTask FindForAllStores(long id)
        {
            return this.Find(new PrimaryKey(id));
        }

        public override bool Create(QueuedTask item)
        {
            if (item.StoreId < 1)
            {
                item.StoreId = context.CurrentStore.Id;
            }
            return base.Create(item);
        }
        public bool Update(QueuedTask c)
        {
            if (c.StoreId != context.CurrentStore.Id)
            {
                return false;
            }
            return this.Update(c, new PrimaryKey(c.Id));
        }
        public bool Delete(long id)
        {
            return Delete(new PrimaryKey(id));
        }
      
        public List<QueuedTask> FindAll()
        {
            return FindAllPaged(1, 100);
        }
        public new List<QueuedTask> FindAllPaged(int pageNumber, int pageSize)
        {
            if (pageNumber < 1) pageNumber = 1;

            int take = pageSize;
            int skip = (pageNumber - 1) * pageSize;
            long storeId = context.CurrentStore.Id;
            IQueryable<Data.EF.QueuedTask> result = repository.Find().Where(y => y.StoreId == storeId).OrderByDescending(y => y.Id).Skip(skip).Take(take);

            return ListPoco(result);                        
        }

        // Finds all stores that have a pending task ready to run at this time.
        public List<long> ListStoresWithTasksToRun()
        {
            var storeIds = repository.Find().Where(y => y.Status == (int)QueuedTaskStatus.Pending)
                                        .Where(y => y.StartAtUtc <= DateTime.UtcNow)
                                        .Select(y => y.StoreId).Distinct().ToList();
            if (storeIds == null) return new List<long>();
            return storeIds;
            
        }

        public QueuedTask PopATaskForRun(long storeId)
        {
            var task = repository.Find().Where(y => y.StoreId == storeId)
                                        .Where(y => y.Status == (int)QueuedTaskStatus.Pending)
                                        .OrderBy(y => y.Id);                                                    
            
            QueuedTask result = FirstPoco(task);
            if (result == null) return null;

            // Lock task in running mode
            result.Status = QueuedTaskStatus.Running;
            result.StatusNotes = "Started Running at " + DateTime.UtcNow.ToString();
            Update(result);
            return result;            
        }
    }
}
