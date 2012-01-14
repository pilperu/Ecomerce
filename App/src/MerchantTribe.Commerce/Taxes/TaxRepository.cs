using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MerchantTribe.Web.Data;
using MerchantTribe.Web.Logging;

namespace MerchantTribe.Commerce.Taxes
{
    public class TaxRepository: ConvertingRepositoryBase<Data.EF.ecommrc_Taxes, Tax>, ITaxRateRepository
    {
        private RequestContext context = null;

        public static TaxRepository InstantiateForMemory(RequestContext c)
        {
            TaxRepository result = null;
            result = new TaxRepository(c, 
                        new MemoryStrategy<Data.EF.ecommrc_Taxes>(PrimaryKeyType.Long), new TextLogger());
            return result;
        }
        public static TaxRepository InstantiateForDatabase(RequestContext c)
        {
            TaxRepository result = null;
            result = new TaxRepository(c, 
                     new EntityFrameworkRepository<Data.EF.ecommrc_Taxes>(new Data.EF.EntityFrameworkDevConnectionString(c.ConnectionStringForEntityFramework)),
                     new SupressLogger());            
            return result;
        }       
        public TaxRepository(RequestContext c, IRepositoryStrategy<Data.EF.ecommrc_Taxes> r, ILogger log)
        {
            context = c;
            repository = r;
            this.logger = log;
            repository.Logger = this.logger;
        }

        protected override void CopyDataToModel(Data.EF.ecommrc_Taxes data, Tax model)
        {
            model.ApplyToShipping = data.ApplyToShipping;
            model.CountryName = data.CountryName;
            model.Id = data.Id;
            model.PostalCode = data.PostalCode;
            model.Rate = data.Rate;
            model.RegionAbbreviation = data.RegionName;
            model.StoreId = data.StoreId;
            model.TaxScheduleId = data.TaxScheduleId;            
        }
        protected override void CopyModelToData(Data.EF.ecommrc_Taxes data, Tax model)
        {
            data.ApplyToShipping = model.ApplyToShipping;
            data.CountryName = model.CountryName;
            data.Id = model.Id;
            data.PostalCode = model.PostalCode;
            data.Rate = model.Rate;
            data.RegionName = model.RegionAbbreviation;
            data.StoreId = model.StoreId;
            data.TaxScheduleId = model.TaxScheduleId;
        }

        public bool ExactMatchExists(Tax item)
        {
            bool result = false;

            List<Tax> all = FindByTaxSchedule(context.CurrentStore.Id, item.TaxScheduleId);
            if (all == null) return false;
            var t = all.Where(y => y.ApplyToShipping == item.ApplyToShipping)
                .Where(y => y.CountryName == item.CountryName)
                .Where(y => y.PostalCode == item.PostalCode)
                .Where(y => y.Rate == item.Rate)
                .Where(y => y.RegionAbbreviation == item.RegionAbbreviation)
                .Where(y => y.StoreId == item.StoreId)
                .Where(y => y.TaxScheduleId == item.TaxScheduleId).FirstOrDefault();
            if (t != null) return true;

            return result;
        }
        public override bool Create(Tax item)
        {            
            item.StoreId = context.CurrentStore.Id;
            return base.Create(item);
        }
        public bool Update(Tax c)
        {            
            return this.Update(c, new PrimaryKey(c.Id));
        }
        public bool Delete(long id)
        {
            long storeId = context.CurrentStore.Id;
            Tax item = Find(id);
            if (item == null) return false;
            if (item.StoreId != storeId) return false;
            return Delete(new PrimaryKey(id));
        }

        public Tax Find(long id)
        {
            Tax result = FindForAllStores(id);
            if (result != null)
            {
                if (result.StoreId == context.CurrentStore.Id)
                {
                    return result;
                }
            }
            return null;
        }
        public Tax FindForAllStores(long id)
        {
            Data.EF.ecommrc_Taxes data = repository.FindByPrimaryKey(new PrimaryKey(id));
            if (data == null) return null;
            Tax result = new Tax();
            CopyDataToModel(data, result);
            return result;
        }
        public List<Tax> FindByStoreId(long storeId)
        {
            List<Tax> result = new List<Tax>();

            IQueryable<Data.EF.ecommrc_Taxes> items = repository.Find().Where(y => y.StoreId == storeId)
                                                                          .OrderBy(y => y.CountryName)
                                                                          .ThenBy(y => y.RegionName)
                                                                          .ThenBy(y =>y.PostalCode);                                                                          
            if (items != null)
            {
                result = ListPoco(items);
            }

            return result;
        }
        public List<Tax> FindByTaxSchedule(long storeId, long scheduleId)
        {
            List<Tax> result = new List<Tax>();
            
            IQueryable<Data.EF.ecommrc_Taxes> items = repository.Find().Where(y => y.StoreId == storeId)
                                                                        .Where(y => y.TaxScheduleId == scheduleId)
                                                                         .OrderBy(y => y.CountryName)
                                                                         .ThenBy(y => y.RegionName)
                                                                         .ThenBy(y => y.PostalCode);
            if (items != null)
            {
                result = ListPoco(items);
            }
            
            return result;
        }

        public List<ITaxRate> GetRates(long storeId, long scheduleId)
        {
            List<ITaxRate> rates = new List<ITaxRate>();
            foreach (ITaxRate r in FindByTaxSchedule(storeId, scheduleId))
            {
                rates.Add(r);
            }
            return rates;
        }
    }
}
