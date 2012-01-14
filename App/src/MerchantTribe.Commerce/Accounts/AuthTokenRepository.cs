using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MerchantTribe.Web.Data;
using MerchantTribe.Web.Logging;

namespace MerchantTribe.Commerce.Accounts
{
    public class AuthTokenRepository: ConvertingRepositoryBase<Data.EF.ecommrc_AuthTokens, AuthToken>
    {
        private RequestContext context = null;

        public static AuthTokenRepository InstantiateForMemory(RequestContext c)
        {
            return new AuthTokenRepository(c, new MemoryStrategy<Data.EF.ecommrc_AuthTokens>(PrimaryKeyType.Guid), new TextLogger());
        }
        public static AuthTokenRepository InstantiateForDatabase(RequestContext c)
        {
            return new AuthTokenRepository(c, new EntityFrameworkRepository<Data.EF.ecommrc_AuthTokens>(
                new Data.EF.EntityFrameworkDevConnectionString(c.ConnectionStringForEntityFramework)),
                new SupressLogger());
        }
        public AuthTokenRepository(RequestContext c, IRepositoryStrategy<Data.EF.ecommrc_AuthTokens> r, ILogger log)
        {
            context = c;
            repository = r;
            this.logger = log;
            repository.Logger = this.logger;
        }

        protected override void CopyDataToModel(Data.EF.ecommrc_AuthTokens data, AuthToken model)
        {
            model.Id = data.Id;
            model.Expires = data.Expires;
            model.TokenId = data.TokenId;
            model.UserId = data.UserId;            
        }
        protected override void CopyModelToData(Data.EF.ecommrc_AuthTokens data, AuthToken model)
        {
            data.Id = model.Id;
            data.Expires = model.Expires;
            data.TokenId = model.TokenId;
            data.UserId = model.UserId;            
        }

        public bool Update(AuthToken t)
        {
            return this.Update(t, new PrimaryKey(t.Id));
        }
        public bool Delete(long id)
        {
            return this.Delete(new PrimaryKey(id));
        }        

        public AuthToken FindByTokenId(Guid tokenId)
        {
            Data.EF.ecommrc_AuthTokens data = repository.Find().Where(y => y.TokenId == tokenId).SingleOrDefault();
            if (data == null) return null;      

            AuthToken result = new AuthToken();
            CopyDataToModel(data, result);
            return result;
        }              
        public List<AuthToken> FindByUserId(long userId)
        {
            IQueryable<Data.EF.ecommrc_AuthTokens> data = repository.Find().Where(y => y.UserId == userId);
            return ListPoco(data);
        }
      
    }
}
