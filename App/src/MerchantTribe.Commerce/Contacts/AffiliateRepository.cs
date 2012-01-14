using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MerchantTribe.Web.Data;
using MerchantTribe.Web.Logging;

namespace MerchantTribe.Commerce.Contacts
{
    public class AffiliateRepository: ConvertingRepositoryBase<Data.EF.bvc_Affiliate, Affiliate>
    {
        private RequestContext context = null;
        private AffiliateContactRepository contactRepository = null;

        public static AffiliateRepository InstantiateForMemory(RequestContext c)
        {
            AffiliateRepository result = null;
            result = new AffiliateRepository(c, new MemoryStrategy<Data.EF.bvc_Affiliate>(PrimaryKeyType.Long),
                                        new MemoryStrategy<Data.EF.bvc_UserXContact>(PrimaryKeyType.Long),
                                        new TextLogger());
            return result;
        }
        public static AffiliateRepository InstantiateForDatabase(RequestContext c)
        {
            AffiliateRepository result = null;
            result = new AffiliateRepository(c,
                new EntityFrameworkRepository<Data.EF.bvc_Affiliate>(new Data.EF.EntityFrameworkDevConnectionString(c.ConnectionStringForEntityFramework)),
                new EntityFrameworkRepository<Data.EF.bvc_UserXContact>(new Data.EF.EntityFrameworkDevConnectionString(c.ConnectionStringForEntityFramework)),
                new SupressLogger());
            return result;
        }
        public AffiliateRepository(RequestContext c, IRepositoryStrategy<Data.EF.bvc_Affiliate> r,
                                IRepositoryStrategy<Data.EF.bvc_UserXContact> sub, ILogger log)
        {
            context = c;
            repository = r;
            this.logger = log;
            contactRepository = new AffiliateContactRepository(c, sub, this.logger);
            repository.Logger = this.logger;
        }

        protected override void CopyDataToModel(Data.EF.bvc_Affiliate data, Affiliate model)
        {            
            model.Address.FromXmlString(data.Address);
            model.CommissionAmount = data.CommissionAmount;
            model.CommissionType = (AffiliateCommissionType)data.CommissionType;
            model.CustomThemeName = data.StyleSheet;
            model.DisplayName = data.DisplayName;
            model.DriversLicenseNumber = data.DriversLicenseNumber;
            model.Enabled = data.Enabled;
            model.Id = data.Id;
            model.LastUpdatedUtc = data.LastUpdated;
            model.Notes = data.Notes;
            model.ReferralDays = data.ReferralDays;
            model.ReferralId = data.ReferralID;
            model.StoreId = data.StoreId;
            model.TaxId = data.TaxID;
            model.WebSiteUrl = data.WebSiteURL;
        }
        protected override void CopyModelToData(Data.EF.bvc_Affiliate data, Affiliate model)
        {
            data.Address = model.Address.ToXml(true);
            data.CommissionAmount = model.CommissionAmount;
            data.CommissionType = (int)model.CommissionType;
            data.StyleSheet = model.CustomThemeName;
            data.DisplayName = model.DisplayName;
            data.DriversLicenseNumber = model.DriversLicenseNumber;
            data.Enabled = model.Enabled;
            data.Id = model.Id;
            data.LastUpdated = model.LastUpdatedUtc;
            data.Notes = model.Notes;
            data.ReferralDays = model.ReferralDays;
            data.ReferralID = model.ReferralId;
            data.StoreId = model.StoreId;
            data.TaxID = model.TaxId;
            data.WebSiteURL = model.WebSiteUrl;
        }

        protected override void DeleteAllSubItems(Affiliate model)
        {
            contactRepository.DeleteForAffiliate(model.Id);
        }
        protected override void GetSubItems(Affiliate model)
        {
            model.Contacts = contactRepository.FindForAffiliate(model.Id);
        }
        protected override void MergeSubItems(Affiliate model)
        {
            contactRepository.MergeList(model.Id, model.Contacts);
        }

        public Affiliate Find(long id)
        {
            Affiliate result = FindForAllStores(id);
            if (result != null)
            {
                if (result.StoreId == context.CurrentStore.Id)
                {
                    return result;
                }
            }
            return null;
        }
        public Affiliate FindForAllStores(long id)
        {
            return this.Find(new PrimaryKey(id));
        }
        public override bool Create(Affiliate item)
        {
            item.LastUpdatedUtc = DateTime.UtcNow;
            item.StoreId = context.CurrentStore.Id;
            return base.Create(item);
        }
        public bool Update(Affiliate c)
        {
            if (c.StoreId != context.CurrentStore.Id)
            {
                return false;
            }
            c.LastUpdatedUtc = DateTime.UtcNow;
            return this.Update(c, new PrimaryKey(c.Id));
        }
        public bool Delete(long id)
        {
            Affiliate existing = Find(id);
            if (existing != null)
            {
                if (existing.StoreId == context.CurrentStore.Id)
                {
                    return Delete(new PrimaryKey(id));
                }
            }
            return false;
        }

        public Affiliate FindByReferralId(string referralId)
        {
            var x = repository.Find().Where(y => y.StoreId == context.CurrentStore.Id).Where(y => y.ReferralID == referralId);
            return SinglePoco(x);
        }
        public List<Affiliate> FindAllWithFilter(string filter, int pageNumber, int pageSize, ref int rowCount)
        {
            IQueryable<Data.EF.bvc_Affiliate> items = repository.Find().Where(y => y.StoreId == context.CurrentStore.Id);
            if (filter.Trim().Length > 0)
            {
                items = items.Where(y => y.DisplayName.Contains(filter) 
                                || y.Address.Contains(filter)
                                || y.ReferralID.Contains(filter));
            }
            if (items != null)
            {
                rowCount = items.Count();
                var x2 = items.OrderBy(y => y.DisplayName).Skip((pageNumber - 1) * pageSize).Take(pageSize);
                return ListPoco(x2);
            }
            return new List<Affiliate>();
        }

        public List<AffiliateContact> FindAffiliateContactsForCustomer(string customerId)
        {
            return this.contactRepository.FindForCustomerId(customerId);
        }
        public bool DeleteAffiliateContactsForCustomer(string customerId)
        {
            return this.contactRepository.DeleteForCustomerId(customerId);
        }
    }
}

