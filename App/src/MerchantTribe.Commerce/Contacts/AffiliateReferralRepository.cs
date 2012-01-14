using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MerchantTribe.Web.Data;
using MerchantTribe.Web.Logging;

namespace MerchantTribe.Commerce.Contacts
{
    public class AffiliateReferralRepository: ConvertingRepositoryBase<Data.EF.bvc_AffiliateReferral, AffiliateReferral>
    {
        private RequestContext context = null;

        public static AffiliateReferralRepository InstantiateForMemory(RequestContext c)
        {
            AffiliateReferralRepository result = null;
            result = new AffiliateReferralRepository(c, new MemoryStrategy<Data.EF.bvc_AffiliateReferral>(PrimaryKeyType.Long), new TextLogger());
            return result;
        }
        public static AffiliateReferralRepository InstantiateForDatabase(RequestContext c)
        {
            AffiliateReferralRepository result = null;
            result = new AffiliateReferralRepository(c,
                new EntityFrameworkRepository<Data.EF.bvc_AffiliateReferral>(new Data.EF.EntityFrameworkDevConnectionString(c.ConnectionStringForEntityFramework)),
                new SupressLogger());
            return result;
        }
        public AffiliateReferralRepository(RequestContext c, IRepositoryStrategy<Data.EF.bvc_AffiliateReferral> r, ILogger log)
        {
            context = c;
            repository = r;
            this.logger = log;
            repository.Logger = this.logger;
        }

        protected override void CopyDataToModel(Data.EF.bvc_AffiliateReferral data, AffiliateReferral model)
        {
            model.AffiliateId = data.AffiliateId;
            model.Id = data.Id;
            model.TimeOfReferralUtc = data.TimeOfReferralUtc;
            model.ReferrerUrl = data.referrerurl;
            model.StoreId = data.StoreId;
        }
        protected override void CopyModelToData(Data.EF.bvc_AffiliateReferral data, AffiliateReferral model)
        {
            data.AffiliateId = model.AffiliateId;
            data.Id = model.Id;
            data.TimeOfReferralUtc = model.TimeOfReferralUtc;
            data.referrerurl = model.ReferrerUrl;
            data.StoreId = model.StoreId;
        }

        public AffiliateReferral Find(long id)
        {
            AffiliateReferral result = FindForAllStores(id);
            if (result != null)
            {
                if (result.StoreId == context.CurrentStore.Id)
                {
                    return result;
                }
            }
            return null;
        }
        public AffiliateReferral FindForAllStores(long id)
        {
            return this.Find(new PrimaryKey(id));
        }
        public override bool Create(AffiliateReferral item)
        {
            return Create2(item, false);
        }
        public bool Create2(AffiliateReferral item, bool timeStampIsSet)
        {
            if (!timeStampIsSet)
            {
                item.TimeOfReferralUtc = DateTime.UtcNow;
            }
            item.StoreId = context.CurrentStore.Id;
            return base.Create(item);
        }
        public bool Update(AffiliateReferral c)
        {
            if (c.StoreId != context.CurrentStore.Id)
            {
                return false;
            }            
            return this.Update(c, new PrimaryKey(c.Id));
        }
        public bool Delete(long id)
        {
            AffiliateReferral existing = Find(id);
            if (existing != null)
            {
                if (existing.StoreId == context.CurrentStore.Id)
                {
                    return Delete(new PrimaryKey(id));
                }
            }
            return false;
        }

        public List<AffiliateReferral> FindByCriteria(AffiliateReferralSearchCriteria criteria, int pageNumber, int pageSize, ref int rowCount)
        {
            IQueryable<Data.EF.bvc_AffiliateReferral> items = repository.Find().Where(y => y.StoreId == context.CurrentStore.Id);

            if (criteria.AffiliateId > 0)
            {
                items = items.Where(y => y.AffiliateId == criteria.AffiliateId);
            }
            if (criteria.StartDateUtc.HasValue && criteria.EndDateUtc.HasValue)
            {
                items = items.Where(y => y.TimeOfReferralUtc >= criteria.StartDateUtc.Value && y.TimeOfReferralUtc <= criteria.EndDateUtc.Value);
            }

            if (items != null)
            {
                rowCount = items.Count();
                var x2 = items.OrderBy(y => y.Id).Skip((pageNumber - 1) * pageSize).Take(pageSize);
                return ListPoco(x2);
            }
            return new List<AffiliateReferral>();
        }

        public bool DeleteAllForAffiliate(long affiliateId)
        {
            int totalCount = 0;
            List<AffiliateReferral> items = FindByCriteria(new AffiliateReferralSearchCriteria() { AffiliateId = affiliateId }, 
                                                           1, 
                                                           int.MaxValue, 
                                                           ref totalCount);
            if (items != null)
            {
                foreach (AffiliateReferral r in items)
                {
                    Delete(r.Id);
                }
            }
            return true;
        }
       
    }
}

