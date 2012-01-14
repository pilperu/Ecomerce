using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MerchantTribe.Web.Data;
using MerchantTribe.Web.Logging;

namespace MerchantTribe.Commerce.Accounts
{
    public class ApiKeyRepository : ConvertingRepositoryBase<Data.EF.ApiKey, ApiKey>
    {
        private RequestContext context = null;

        public static ApiKeyRepository InstantiateForMemory(RequestContext c)
        {
            return new ApiKeyRepository(c, new MemoryStrategy<Data.EF.ApiKey>(PrimaryKeyType.Long), new TextLogger());
        }
        public static ApiKeyRepository InstantiateForDatabase(RequestContext c)
        {
            return new ApiKeyRepository(c, new EntityFrameworkRepository<Data.EF.ApiKey>(
                new Data.EF.EntityFrameworkDevConnectionString(c.ConnectionStringForEntityFramework)),
                new SupressLogger());
        }
        public ApiKeyRepository(RequestContext c, IRepositoryStrategy<Data.EF.ApiKey> r, ILogger log)
        {
            context = c;
            repository = r;
            this.logger = log;
            repository.Logger = this.logger;
        }

        protected override void CopyDataToModel(Data.EF.ApiKey data, ApiKey model)
        {
            model.Id = data.Id;
            model.StoreId = data.StoreId;
            model.Key = data.ApiKey1;
        }
        protected override void CopyModelToData(Data.EF.ApiKey data, ApiKey model)
        {
            data.Id = model.Id;
            data.StoreId = model.StoreId;
            data.ApiKey1 = model.Key;
        }
        
        public bool Delete(long id)
        {
            return this.Delete(new PrimaryKey(id));
        }        

        public ApiKey FindByKey(string key)
        {
            string[] parts = key.Split('-');
            long storeId = -1;
            if (parts.Count() > 0)
            {
                string tempId = parts[0];
                long.TryParse(tempId, out storeId);                
            }
            if (storeId < 0) return null;

            Data.EF.ApiKey data = repository.Find().Where(y => y.StoreId == storeId).Where(y => y.ApiKey1 == key).SingleOrDefault();
            if (data == null) return null;

            ApiKey result = new ApiKey();
            CopyDataToModel(data, result);
            return result;
        }              
        public List<ApiKey> FindByStoreId(long storeId)
        {
            IQueryable<Data.EF.ApiKey> data = repository.Find().Where(y => y.StoreId == storeId);
            return ListPoco(data);
        }
      
    }
}

