using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MerchantTribe.Web.Data;
using MerchantTribe.Web.Logging;

namespace MerchantTribe.Commerce.Security
{
    public class FraudRuleRepository: ConvertingRepositoryBase<Data.EF.bvc_Fraud, FraudRule>
    {
        private RequestContext context = null;
        
        public FraudRuleRepository(RequestContext c)
        {
            context = c;
            repository = new EntityFrameworkRepository<Data.EF.bvc_Fraud>(new Data.EF.EntityFrameworkDevConnectionString(c.ConnectionStringForEntityFramework));
            this.logger = new SupressLogger();
            repository.Logger = this.logger;
        }
        public FraudRuleRepository(RequestContext c, IRepositoryStrategy<Data.EF.bvc_Fraud> r)
        {
            context = c;
            repository = r;
            this.logger = new SupressLogger();
            repository.Logger = this.logger;
        }

        protected override void CopyDataToModel(Data.EF.bvc_Fraud data, FraudRule model)
        {
            model.Bvin = data.bvin;
            model.LastUpdated = data.LastUpdated;
            model.RuleType = (FraudRuleType)data.RuleType;
            model.RuleValue = data.RuleValue;
            model.StoreId = data.StoreId;            
        }
        protected override void CopyModelToData(Data.EF.bvc_Fraud data, FraudRule model)
        {
            data.bvin = model.Bvin;
            data.LastUpdated = model.LastUpdated;
            data.RuleType = (int)model.RuleType;
            data.RuleValue = model.RuleValue;
            data.StoreId = model.StoreId;            
        }

        public FraudRule Find(string bvin)
        {
            FraudRule result = FindForAllStores(bvin);
            if (result != null)
            {
                if (result.StoreId == context.CurrentStore.Id)
                {
                    return result;
                }
            }
            return null;
        }
        public FraudRule FindForAllStores(string bvin)
        {
            Data.EF.bvc_Fraud data = repository.FindByPrimaryKey(new PrimaryKey(bvin));
            if (data == null) return null;

            FraudRule result = new FraudRule();
            CopyDataToModel(data, result);
            return result;
        }

        public override bool Create(FraudRule item)
        {
            item.LastUpdated = DateTime.UtcNow;            
            if (item.Bvin == string.Empty)
            {
                item.Bvin = System.Guid.NewGuid().ToString();
            }
            item.StoreId = context.CurrentStore.Id;

 	        return base.Create(item);
        }

        public bool Update(FraudRule c)
        {
            if (c.StoreId != context.CurrentStore.Id)
            {
                return false;
            }
            c.LastUpdated = DateTime.UtcNow;
            return this.Update(c, new PrimaryKey(c.Bvin));            
        }

        public bool Delete(string bvin)
        {
            long storeId = context.CurrentStore.Id;
            FraudRule img = Find(bvin);
            if (img == null) return false;
            return Delete(new PrimaryKey(bvin));            
        }

        public List<FraudRule> FindForStore(long storeId)
        {
            List<FraudRule> result = new List<FraudRule>();

            IQueryable<Data.EF.bvc_Fraud> items = repository.Find().Where(y => y.StoreId == storeId)
                                                                          .OrderBy(y => y.RuleValue);
            if (items != null)
            {
                result = ListPoco(items);
            }

            return result;
        }
    
    }
}
