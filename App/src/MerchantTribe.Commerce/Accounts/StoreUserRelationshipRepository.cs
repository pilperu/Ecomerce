using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MerchantTribe.Web.Data;
using MerchantTribe.Web.Logging;

namespace MerchantTribe.Commerce.Accounts
{
    public class StoreUserRelationshipRepository : ConvertingRepositoryBase<Data.EF.ecommrc_StoresXUsers, StoreUserRelationship>
    {
        private RequestContext context = null;

        public static StoreUserRelationshipRepository InstantiateForMemory(RequestContext c)
        {
            return new StoreUserRelationshipRepository(c, new MemoryStrategy<Data.EF.ecommrc_StoresXUsers>(PrimaryKeyType.Long), new TextLogger());
        }
        public static StoreUserRelationshipRepository InstantiateForDatabase(RequestContext c)
        {
            return new StoreUserRelationshipRepository(c, new EntityFrameworkRepository<Data.EF.ecommrc_StoresXUsers>(
                new Data.EF.EntityFrameworkDevConnectionString(c.ConnectionStringForEntityFramework)),
                new SupressLogger());
        }        
        public StoreUserRelationshipRepository(RequestContext c, IRepositoryStrategy<Data.EF.ecommrc_StoresXUsers> r, ILogger log)
        {
            context = c;
            repository = r;
            this.logger = log;
            repository.Logger = this.logger;
        }

        protected override void CopyDataToModel(Data.EF.ecommrc_StoresXUsers data, StoreUserRelationship model)
        {
            model.Id = data.Id;
            model.StoreId = data.StoreId;
            model.UserId = data.UserId;
            model.AccessMode = (StoreAccessMode)data.AccessMode;
        }
        protected override void CopyModelToData(Data.EF.ecommrc_StoresXUsers data, StoreUserRelationship model)
        {
            data.Id = model.Id;
            data.StoreId = model.StoreId;
            data.UserId = model.UserId;
            data.AccessMode = (int)model.AccessMode;                        
        }

        public bool Update(StoreUserRelationship r)
        {
            return this.Update(r, new PrimaryKey(r.Id));
        }
        public bool Delete(long id)
        {
            return this.Delete(new PrimaryKey(id));
        }                      
        public bool Delete(long storeId, long userId)
        {
            var u = repository.Find().Where(y => y.StoreId == storeId).Where(y => y.UserId == userId).SingleOrDefault();
            if (u == null) return true;
            return Delete(u.Id);                                  
        }

        public List<StoreUserRelationship> FindByUserId(long id)
        {
            IQueryable<Data.EF.ecommrc_StoresXUsers> data =
                repository.Find().Where(y => y.UserId == id);
            return ListPoco(data);            
        }
        public List<StoreUserRelationship> FindByStoreId(long id)
        {
            IQueryable<Data.EF.ecommrc_StoresXUsers> data =
                repository.Find().Where(y => y.StoreId == id);
            return ListPoco(data);            
        }

        public StoreUserRelationship FindByUserIdAndStoreId(long userId, long storeId)
        {
            IQueryable<Data.EF.ecommrc_StoresXUsers> data =
                repository.Find().Where(y => y.StoreId == storeId).Where(y => y.UserId == userId);
            return FirstPoco(data);
        }
                    
    }
}
