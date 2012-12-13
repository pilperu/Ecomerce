using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MerchantTribe.Web.Data;
using MerchantTribe.Web.Logging;
using System.Transactions;

namespace MerchantTribe.Commerce.Accounts
{
    public class StoreSettingsRepository: ConvertingRepositoryBase<Data.EF.ecommrc_StoreSettings, StoreSetting>
    {
        private RequestContext context = null;

        protected override void CopyModelToData(Data.EF.ecommrc_StoreSettings data, StoreSetting model)
        {
            data.Id = model.Id;
            data.SettingName = model.SettingName;
            data.SettingValue = model.SettingValue;
            data.StoreId = model.StoreId;            
        }
        protected override void CopyDataToModel(Data.EF.ecommrc_StoreSettings data, StoreSetting model)
        {
            model.Id = data.Id;
            model.SettingValue = data.SettingValue;
            model.SettingName = data.SettingName;
            model.StoreId = data.StoreId;
        }

        public static StoreSettingsRepository InstantiateForMemory(RequestContext c)
        {
            return new StoreSettingsRepository(c, new MemoryStrategy<Data.EF.ecommrc_StoreSettings>(PrimaryKeyType.Long), new TextLogger());
        }
        public static StoreSettingsRepository InstantiateForDatabase(RequestContext c)
        {
            return new StoreSettingsRepository(c, new EntityFrameworkRepository<Data.EF.ecommrc_StoreSettings>(
                new Data.EF.EntityFrameworkDevConnectionString(c.ConnectionStringForEntityFramework)),
                new SupressLogger());
        }        

        public StoreSettingsRepository(RequestContext c, IRepositoryStrategy<Data.EF.ecommrc_StoreSettings> strategy, ILogger log)
        {
            this.context = c;
            repository = strategy;
            this.logger = log;
            repository.Logger = this.logger;            
        }

        public bool Update(StoreSetting item)
        {
            if (item.Id < 1)
            {
                return base.Create(item);
            }
            else
            {
                return base.Update(item, new PrimaryKey(item.Id));
            }
        }

        public bool Delete(long id)
        {
            return Delete(new PrimaryKey(id));
        }

        public StoreSetting FindSingleSetting(long storeId, string settingName)
        {
            var item = repository.Find().Where(y => y.StoreId == storeId).Where(y => y.SettingName == settingName);            
            return SinglePoco(item);
        }
        public List<StoreSetting> FindForStore(long storeId)
        {
            var items = repository.Find().Where(y => y.StoreId == storeId);                                        
            return ListPoco(items);
        }

        public void DeleteForStore(long storeId)
        {
            List<StoreSetting> existing = FindForStore(storeId);
            foreach (StoreSetting sub in existing)
            {
                Delete(sub.Id);
            }
        }

        public void MergeList(long storeId, List<StoreSetting> subitems)
        {
            // Set Base Key Field
            foreach (StoreSetting item in subitems)
            {
                item.StoreId = storeId;
            }

            // Create or Update
            foreach (StoreSetting itemnew in subitems)
            {
                if (itemnew.Id < 1)
                {
                    Create(itemnew);
                }
                else
                {
                    Update(itemnew);
                }
            }    
        
            // Delete missing
            List<StoreSetting> existing = FindForStore(storeId);
            foreach (StoreSetting ex in existing)
            {
                var count = (from sub in subitems
                             where sub.Id == ex.Id
                             select sub).Count();
                if (count < 1)                
                {
                    Delete(ex.Id);
                }
            }
        }

    }
}
