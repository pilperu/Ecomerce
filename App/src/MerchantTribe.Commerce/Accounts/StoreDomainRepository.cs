using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MerchantTribe.Web.Data;
using MerchantTribe.Web.Logging;

namespace MerchantTribe.Commerce.Accounts
{
    public class StoreDomainRepository: ConvertingRepositoryBase<Data.EF.ecommrc_StoreDomains, StoreDomain>
    {
        private RequestContext context = null;

        public StoreDomainRepository(RequestContext c)
        {
            context = c;
            repository = new EntityFrameworkRepository<Data.EF.ecommrc_StoreDomains>(new Data.EF.EntityFrameworkDevConnectionString(c.ConnectionStringForEntityFramework));
            this.logger = new SupressLogger();
            repository.Logger = this.logger;
        }
        public StoreDomainRepository(RequestContext c, IRepositoryStrategy<Data.EF.ecommrc_StoreDomains> r)
        {
            context = c;
            repository = r;
            this.logger = new SupressLogger();
            repository.Logger = this.logger;
        }

        protected override void CopyModelToData(Data.EF.ecommrc_StoreDomains data, StoreDomain model)
        {
            data.Id = model.Id;
            data.StoreId = model.StoreId;
            data.DomainName = model.DomainName.Trim().ToLowerInvariant();
        }
        protected override void CopyDataToModel(Data.EF.ecommrc_StoreDomains data, StoreDomain model)
        {
            model.Id = data.Id;
            model.StoreId = data.StoreId;
            model.DomainName = data.DomainName;
        }

        public StoreDomain Find(long id)
        {
            Data.EF.ecommrc_StoreDomains data = repository.FindByPrimaryKey(new PrimaryKey(id));
            if (data == null) return null;

            StoreDomain result = new StoreDomain();
            CopyDataToModel(data, result);
            return result;
        }
        public StoreDomain FindForAnyStoreByDomain(string domain)
        {
            string domainMatch = domain.Trim().ToLowerInvariant();
            IQueryable<Data.EF.ecommrc_StoreDomains> result
                = repository.Find().Where(y => y.DomainName == domainMatch);
            return SinglePoco(result);            
        }       
        public override bool Create(StoreDomain item)
        {
            item.StoreId = context.CurrentStore.Id;
            return base.Create(item);
        }
        public bool Update(StoreDomain item)
        {
            if (item.StoreId != context.CurrentStore.Id)
            {
                return false;
            }
            return this.Update(item, new PrimaryKey(item.Id));
        }

        public bool Delete(long id)
        {
            return Delete(new PrimaryKey(id));
        }

        public List<StoreDomain> FindForStore(long storeId)
        {
            IQueryable<Data.EF.ecommrc_StoreDomains> result 
                = repository.Find()
                            .Where(y => y.StoreId == storeId)
                            .OrderBy(y => y.DomainName);
            return ListPoco(result);
        }
        public List<StoreDomain> FindForCurrentStore()
        {
            long storeId = context.CurrentStore.Id;
            return FindForStore(storeId);
        }
    }
}
