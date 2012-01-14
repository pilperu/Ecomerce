using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MerchantTribe.Web.Data;
using MerchantTribe.Web.Logging;

namespace MerchantTribe.Commerce.Taxes
{
    public class TaxScheduleRepository: ConvertingRepositoryBase<Data.EF.ecommrc_TaxSchedules, TaxSchedule>
    {
        private RequestContext context = null;

        public static TaxScheduleRepository InstantiateForMemory(RequestContext c)
        {
            TaxScheduleRepository result = null;
            result = new TaxScheduleRepository(c, 
                        new MemoryStrategy<Data.EF.ecommrc_TaxSchedules>(PrimaryKeyType.Long),
                        new TextLogger());
            return result;
        }
        public static TaxScheduleRepository InstantiateForDatabase(RequestContext c)
        {
            TaxScheduleRepository result = null;
            result = new TaxScheduleRepository(c, 
                     new EntityFrameworkRepository<Data.EF.ecommrc_TaxSchedules>(new Data.EF.EntityFrameworkDevConnectionString(c.ConnectionStringForEntityFramework)),
                     new SupressLogger());            
            return result;
        }
        public TaxScheduleRepository(RequestContext c, IRepositoryStrategy<Data.EF.ecommrc_TaxSchedules> r, ILogger log)
        {
            context = c;
            repository = r;
            this.logger = log;
            repository.Logger = this.logger;
        }

        protected override void CopyDataToModel(Data.EF.ecommrc_TaxSchedules data, TaxSchedule model)
        {
            model.Id = data.Id;
            model.Name = data.Name;
            model.StoreId = data.StoreId;            
        }
        protected override void CopyModelToData(Data.EF.ecommrc_TaxSchedules data, TaxSchedule model)
        {
            data.Id = model.Id;
            data.Name = model.Name;
            data.StoreId = model.StoreId;
        }

        public override bool Create(TaxSchedule item)
        {
            item.StoreId = context.CurrentStore.Id;
            return base.Create(item);
        }
        public bool Update(TaxSchedule c)
        {
            return this.Update(c, new PrimaryKey(c.Id));
        }
        public bool Delete(long id)
        {
            long storeId = context.CurrentStore.Id;
            TaxSchedule item = FindForThisStore(id);
            if (item == null) return false;
            if (item.StoreId != storeId) return false;
            return Delete(new PrimaryKey(id));
        }

        public TaxSchedule FindForThisStore(long id)
        {
            TaxSchedule result = FindForAllStores(id);
            if (result != null)
            {
                if (result.StoreId == context.CurrentStore.Id)
                {
                    return result;
                }
            }
            return null;
        }
        public TaxSchedule FindForAllStores(long id)
        {
            Data.EF.ecommrc_TaxSchedules data = repository.FindByPrimaryKey(new PrimaryKey(id));
            if (data == null) return null;
            TaxSchedule result = new TaxSchedule();
            CopyDataToModel(data, result);
            return result;
        }
        public List<TaxSchedule> FindAll(long storeId)
        {
            List<TaxSchedule> result = new List<TaxSchedule>();

            IQueryable<Data.EF.ecommrc_TaxSchedules> items = repository.Find().Where(y => y.StoreId == storeId)
                                                                          .OrderBy(y => y.Name);
            if (items != null)
            {
                result = ListPoco(items);
            }

            return result;
        }

        public TaxSchedule Find(long id)
        {
            long storeId = context.CurrentStore.Id;
            List<TaxSchedule> all = FindAllAndCreateDefault(storeId);
            if (all == null) return null;
            var existing = all.Where(y => y.Id == id).FirstOrDefault();
            if (existing == null) return null;
            return existing;
        }
        public TaxSchedule FindByName(string name)
        {
            long storeId = context.CurrentStore.Id;
            List<TaxSchedule> all = FindAllAndCreateDefault(storeId);
            if (all == null) return null;
            var existing = all.Where(y => y.Name.Trim().ToLowerInvariant() == name.Trim().ToLowerInvariant()).FirstOrDefault();
            if (existing == null) return null;
            return existing;
        }
        public List<TaxSchedule> FindAllAndCreateDefault(long storeId)
        {
            List<TaxSchedule> result = new List<TaxSchedule>();

            result = FindAll(storeId);
            if (result.Count < 1)
            {
                TaxSchedule ts = new TaxSchedule();
                ts.Name = "Default";
                ts.StoreId = storeId;
                Create(ts);
                result = FindAll(storeId);
            }

            return result;
        }
        public List<ITaxSchedule> FindAllAndCreateDefaultAsInterface(long storeId)
        {
            List<ITaxSchedule> result = new List<ITaxSchedule>();
            foreach (ITaxSchedule ts in FindAllAndCreateDefault(storeId))
            {
                result.Add(ts);
            }
            return result;
        }
        public List<ITaxSchedule> FindAllAsInterface(long storeId)
        {
            List<ITaxSchedule> result = new List<ITaxSchedule>();
            foreach (ITaxSchedule ts in FindAll(storeId))
            {
                result.Add(ts);
            }
            return result;
        }
        public bool NameExists(string name, long currentId, long storeId)
        {
            bool result = false;

            List<TaxSchedule> schedules = FindAll(storeId);
            if (schedules != null)
            {
                foreach (TaxSchedule ts in schedules)
                {
                    if (ts.Name.Trim().ToLowerInvariant() == name.Trim().ToLowerInvariant())
                    {
                        if (ts.Id != currentId)
                        {
                            return true;
                        }
                    }
                }
            }

            return result;
        }

    }
}
