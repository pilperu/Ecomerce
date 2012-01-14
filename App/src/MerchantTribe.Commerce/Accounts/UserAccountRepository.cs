using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MerchantTribe.Web.Data;
using MerchantTribe.Web.Logging;

namespace MerchantTribe.Commerce.Accounts
{
    public class UserAccountRepository: ConvertingRepositoryBase<Data.EF.ecommrc_UserAccounts, UserAccount>
    {
        private RequestContext context = null;

        public static UserAccountRepository InstantiateForMemory(RequestContext c)
        {
            return new UserAccountRepository(c, new MemoryStrategy<Data.EF.ecommrc_UserAccounts>(PrimaryKeyType.Long), new TextLogger());
        }
        public static UserAccountRepository InstantiateForDatabase(RequestContext c)
        {
            return new UserAccountRepository(c, new EntityFrameworkRepository<Data.EF.ecommrc_UserAccounts>(
                new Data.EF.EntityFrameworkDevConnectionString(c.ConnectionStringForEntityFramework)),
                new SupressLogger());
        }
        public UserAccountRepository(RequestContext c, IRepositoryStrategy<Data.EF.ecommrc_UserAccounts> r, ILogger log)
        {
            context = c;
            repository = r;
            this.logger = log;
            repository.Logger = this.logger;
        }

        protected override void CopyDataToModel(Data.EF.ecommrc_UserAccounts data, UserAccount model)
        {
            model.Id = data.Id;
            model.DateCreated = data.DateCreated;
            model.Email = data.email;
            model.HashedPassword = data.password;
            model.Salt = data.Salt;
            model.Status = (UserAccountStatus)data.statuscode;
            model.ResetKey = data.ResetKey;                   
        }
        protected override void CopyModelToData(Data.EF.ecommrc_UserAccounts data, UserAccount model)
        {
            data.Id = model.Id;
            data.DateCreated = model.DateCreated;
            data.email = model.Email;
            data.password = model.HashedPassword;
            data.Salt = model.Salt;
            data.statuscode = (int)model.Status;
            data.ResetKey = model.ResetKey;
        }

        public override bool Create(UserAccount item)
        {
            item.DateCreated = DateTime.UtcNow;
            item.HashPasswordIfNeeded();            
            return base.Create(item);
        }
        public bool Update(UserAccount c)
        {
            c.HashPasswordIfNeeded();
            return this.Update(c, new PrimaryKey(c.Id));
        }
        public bool Delete(long id)
        {
            return this.Delete(new PrimaryKey(id));
        }        

        public UserAccount FindById(long id)
        {
            Data.EF.ecommrc_UserAccounts data = repository.FindByPrimaryKey(new PrimaryKey(id));
            if (data == null) return null;      

            UserAccount result = new UserAccount();
            CopyDataToModel(data, result);
            return result;
        }
        public UserAccount FindByEmail(string email)
        {
            Data.EF.ecommrc_UserAccounts data = repository.Find().Where(y => y.email == email)                                                                    
                                                                    .SingleOrDefault();
            if (data == null) return null;

            UserAccount result = new UserAccount();
            CopyDataToModel(data, result);
            return result;
        }
                
    }
}
