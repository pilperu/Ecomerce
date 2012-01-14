using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MerchantTribe.Web.Data;
using MerchantTribe.Web.Logging;

namespace MerchantTribe.Commerce.Contacts
{
    public class AffiliateContactRepository: ConvertingRepositoryBase<Data.EF.bvc_UserXContact, AffiliateContact>
    {
        private RequestContext context = null;

        public static AffiliateContactRepository InstantiateForMemory(RequestContext c)
        {
            AffiliateContactRepository result = null;
            result = new AffiliateContactRepository(c, new MemoryStrategy<Data.EF.bvc_UserXContact>(PrimaryKeyType.Long), new TextLogger());
            return result;
        }
        public static AffiliateContactRepository InstantiateForDatabase(RequestContext c)
        {
            AffiliateContactRepository result = null;
            result = new AffiliateContactRepository(c, 
                new EntityFrameworkRepository<Data.EF.bvc_UserXContact>(
                    new Data.EF.EntityFrameworkDevConnectionString(c.ConnectionStringForEntityFramework)),
                    new SupressLogger());
            return result;
        }
        public AffiliateContactRepository(RequestContext c, IRepositoryStrategy<Data.EF.bvc_UserXContact> r, ILogger log)
        {
            context = c;
            repository = r;            
            this.logger = log;
            repository.Logger = this.logger;
        }

        protected override void CopyDataToModel(Data.EF.bvc_UserXContact data, AffiliateContact model)
        {
            model.Id = data.Id;
            model.AffiliateId = data.ContactId;
            model.StoreId = data.StoreId;
            model.UserId = data.UserId;
        }
        protected override void CopyModelToData(Data.EF.bvc_UserXContact data, AffiliateContact model)
        {
            data.Id = model.Id;
            data.ContactId = model.AffiliateId;
            data.StoreId = model.StoreId;
            data.UserId = model.UserId;
        }

        public AffiliateContact Find(long id)
        {
            AffiliateContact result = FindForAllStores(id);
            if (result != null)
            {
                if (result.StoreId == context.CurrentStore.Id)
                {
                    return result;
                }
            }
            return null;
        }
        public AffiliateContact FindForAllStores(long id)
        {
            return this.Find(new PrimaryKey(id));
        }
        public override bool Create(AffiliateContact item)
        {
            item.StoreId = context.CurrentStore.Id;
            return base.Create(item);
        }
        public bool Update(AffiliateContact c)
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

        public List<AffiliateContact> FindForAffiliate(long affiliateId)
        {
            string affId = affiliateId.ToString();
            var items = repository.Find().Where(y => y.ContactId == affId);                                       
            return ListPoco(items);
        }
        public List<AffiliateContact> FindForCustomerId(string customerId)
        {
            var items = repository.Find().Where(y => y.UserId == customerId);
            return ListPoco(items);
        }
        public void DeleteForAffiliate(long affiliateId)
        {
            List<AffiliateContact> existing = FindForAffiliate(affiliateId);
            foreach (AffiliateContact sub in existing)
            {
                Delete(sub.Id);
            }
        }
        public bool DeleteForCustomerId(string customerId)
        {
            List<AffiliateContact> existing = FindForCustomerId(customerId);
            foreach (AffiliateContact sub in existing)
            {
                Delete(sub.Id);
            }
            return true;
        }
        public void MergeList(long affiliateId, List<AffiliateContact> subitems)
        {
            // Set Base Key Field
            foreach (AffiliateContact item in subitems)
            {
                item.AffiliateId = affiliateId.ToString();
            }

            // Create or Update
            foreach (AffiliateContact itemnew in subitems)
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
            List<AffiliateContact> existing = FindForAffiliate(affiliateId);
            foreach (AffiliateContact ex in existing)
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

