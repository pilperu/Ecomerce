using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MerchantTribe.Web.Data;
using MerchantTribe.Web.Logging;

namespace MerchantTribe.Commerce.Accounts
{
    public class StoreRepository: ConvertingRepositoryBase<Data.EF.ecommrc_Stores, Store>
    {
        private RequestContext context = null;

        public static StoreRepository InstantiateForMemory(RequestContext c)
        {
            StoreRepository result = null;
            result = new StoreRepository(c, new MemoryStrategy<Data.EF.ecommrc_Stores>(PrimaryKeyType.Bvin),
                                           new MemoryStrategy<Data.EF.ecommrc_StoreSettings>(PrimaryKeyType.Long),
                                           new TextLogger());
            return result;
        }
        public static StoreRepository InstantiateForDatabase(RequestContext c)
        {
            StoreRepository result = null;
            result = new StoreRepository(c,
                new EntityFrameworkRepository<Data.EF.ecommrc_Stores>(
                    new Data.EF.EntityFrameworkDevConnectionString(c.ConnectionStringForEntityFramework)),
                new EntityFrameworkRepository<Data.EF.ecommrc_StoreSettings>(
                    new Data.EF.EntityFrameworkDevConnectionString(c.ConnectionStringForEntityFramework)),
                    new SupressLogger()
                    );
            return result;
        }

        private StoreSettingsRepository settingsRepository = null;

        public StoreRepository(RequestContext c, IRepositoryStrategy<Data.EF.ecommrc_Stores> r,
                                    IRepositoryStrategy<Data.EF.ecommrc_StoreSettings> subr, ILogger log)
        {
            context = c;
            repository = r;
            this.logger = log;
            repository.Logger = this.logger;
            settingsRepository = new StoreSettingsRepository(c, subr, this.logger);                        
        }

        protected override void CopyDataToModel(Data.EF.ecommrc_Stores data, Store model)
        {
            model.Id = data.Id;
            model.CurrentPlanDayOfMonth = data.CurrentPlanDayOfMonth;
            model.CurrentPlanPercent = data.CurrentPlanPercent;
            model.CurrentPlanRate = data.CurrentPlanRate;
            model.CustomUrl = data.CustomUrl;
            model.DateCancelled = data.DateCancelled;
            model.DateCreated = data.DateCreated;
            model.PlanId = data.PlanId;
            model.Status = (StoreStatus)data.Status;
            model.StoreName = data.StoreName;
            //model.SubscriptionId = data.SubscriptionId;            
        }
        protected override void CopyModelToData(Data.EF.ecommrc_Stores data, Store model)
        {
            data.Id = model.Id;
            data.CurrentPlanDayOfMonth = model.CurrentPlanDayOfMonth;
            data.CurrentPlanPercent = model.CurrentPlanPercent;
            data.CurrentPlanRate = model.CurrentPlanRate;
            data.CustomUrl = model.CustomUrl;
            data.DateCancelled = model.DateCancelled;
            data.DateCreated = model.DateCreated;
            data.PlanId = model.PlanId;
            data.Status = (int)model.Status;
            data.StoreName = model.StoreName;
            data.SubscriptionId = -1; // model.SubscriptionId;            
        }

        protected override void DeleteAllSubItems(Store model)
        {
            settingsRepository.DeleteForStore(model.Id);
        }
        protected override void GetSubItems(Store model)
        {
            model.Settings.AllSettings = settingsRepository.FindForStore(model.Id);
        }
        protected override void MergeSubItems(Store model)
        {
            settingsRepository.MergeList(model.Id, model.Settings.AllSettings);
        }

        public Store FindById(long id)
        {
            IQueryable<Data.EF.ecommrc_Stores> s = repository.Find().Where(y => y.Id == id);
            return FirstPoco(s);
        }
        public Store FindByStoreName(string storeName)
        {
            IQueryable<Data.EF.ecommrc_Stores> s = repository.Find().Where(y => y.StoreName == storeName);
            return FirstPoco(s);
        }
        public long FindStoreIdByCustomUrl(string hostName)
        {
            long result = -1;

            var s = repository.Find().Where(y => y.CustomUrl == hostName).SingleOrDefault();
            if (s != null)
            {
                result = s.Id;
            }
            
            return result;
        }

        public bool Update(Store s)
        {
            return this.Update(s, new PrimaryKey(s.Id));
        }
        public bool Delete(long id)
        {
            return this.Delete(new PrimaryKey(id));
        }


        public List<Store> ListAllForSuper()
        {
            IQueryable<Data.EF.ecommrc_Stores> data = repository.Find().OrderByDescending(y => y.DateCreated);
            return ListPoco(data);            
        }
        public List<Store> FindStoresCreatedAfterDateForSuper(DateTime cutOffDateUtc)
        {
            IQueryable<Data.EF.ecommrc_Stores> data = repository.Find().Where(y => y.DateCreated >= cutOffDateUtc).OrderByDescending(y => y.DateCreated);
            return ListPoco(data);            
        }
        public List<Store> FindBillableStoresForDay(int dayOfMonth)
        {
            IQueryable<Data.EF.ecommrc_Stores> data = repository.Find()
                                                        .Where(y => y.CurrentPlanDayOfMonth == dayOfMonth)
                                                        .Where(y => y.DateCancelled.HasValue == false)
                                                        .Where(y => y.Status == 1)
                                                        .Where(y => y.PlanId > 0)
                                                        .OrderByDescending(y => y.Id);
            return ListPoco(data);            
        }

        public int CountOfAll()
        {
            return repository.CountOfAll();
        }
        public int CountOfActive()
        {
            return repository.Find().Where(y => y.Status == 1).Count();
        }
        public int CountOfFree()
        {
            return repository.Find().Where(y => y.Status == 1).Where(y=>y.PlanId==0).Count();
        }
        public int CountOfPaid()
        {
            return repository.Find().Where(y => y.Status == 1).Where(y => y.PlanId > 0).Count();
        }

        public List<StoreDomainSnapshot> FindDomainSnapshotsByIds(List<long> ids)
        {
            List<StoreDomainSnapshot> result = new List<StoreDomainSnapshot>();

            IQueryable<Data.EF.ecommrc_Stores> items = repository.Find().Where(y => ids.Contains(y.Id))
                                                                        .OrderBy(y => y.Id);
            if (items != null)
            {
                foreach(Data.EF.ecommrc_Stores store in items)
                {
                    StoreDomainSnapshot snap = new StoreDomainSnapshot();
                    snap.CustomUrl = store.CustomUrl;
                    snap.Id = store.Id;
                    snap.StoreName = store.StoreName;
                    result.Add(snap);
                }                
            }
            return result;
        }
                        
    }
}
