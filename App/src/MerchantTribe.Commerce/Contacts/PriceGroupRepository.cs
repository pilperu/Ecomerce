using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MerchantTribe.Web.Data;
using MerchantTribe.Web.Logging;

namespace MerchantTribe.Commerce.Contacts
{
    public class PriceGroupRepository: ConvertingRepositoryBase<Data.EF.bvc_PriceGroup, PriceGroup>
    {
        private RequestContext context = null;

        public static PriceGroupRepository InstantiateForMemory(RequestContext c)
        {
            return new PriceGroupRepository(c, new MemoryStrategy<Data.EF.bvc_PriceGroup>(PrimaryKeyType.Long), new TextLogger());
        }
        public static PriceGroupRepository InstantiateForDatabase(RequestContext c)
        {
            return new PriceGroupRepository(c,
                new EntityFrameworkRepository<Data.EF.bvc_PriceGroup>(
                    new Data.EF.EntityFrameworkDevConnectionString(c.ConnectionStringForEntityFramework)),
                    new SupressLogger()
                    );
        }
        public PriceGroupRepository(RequestContext c, IRepositoryStrategy<Data.EF.bvc_PriceGroup> r, ILogger log)
        {
            context = c;
            repository = r;
            this.logger = log;
            repository.Logger = this.logger;
        }

        protected override void CopyModelToData(Data.EF.bvc_PriceGroup data, PriceGroup model)
        {
            data.AdjustmentAmount = model.AdjustmentAmount;
            data.bvin = model.Bvin;
            data.LastUpdated = model.LastUpdated;
            data.Name = model.Name;
            data.PricingType = (int)model.PricingType;
            data.StoreId = model.StoreId;            
        }
        protected override void CopyDataToModel(Data.EF.bvc_PriceGroup data, PriceGroup model)
        {
            model.AdjustmentAmount = data.AdjustmentAmount;
            model.Bvin = data.bvin;
            model.LastUpdated = data.LastUpdated;
            model.Name = data.Name;
            model.PricingType = (PricingTypes)data.PricingType;
            model.StoreId = data.StoreId;
        }

     
        public PriceGroup Find(string bvin)
        {
            Data.EF.bvc_PriceGroup data = repository.FindByPrimaryKey(new PrimaryKey(bvin));
            if (data == null) return null;

            PriceGroup result = new PriceGroup();
            CopyDataToModel(data, result);
            return result;
        }
        public override bool Create(PriceGroup item)
        {
            if (item.Bvin == string.Empty)
            {
                item.Bvin = System.Guid.NewGuid().ToString();
            }
            item.StoreId = context.CurrentStore.Id;
            item.LastUpdated = DateTime.UtcNow;
            return base.Create(item);
        }
        public bool Update(PriceGroup item)
        {
            if (item.StoreId != context.CurrentStore.Id)
            {
                return false;
            }
            item.LastUpdated = DateTime.UtcNow;
            return this.Update(item, new PrimaryKey(item.Bvin));
        }
        public bool Delete(string bvin)
        {
            return this.Delete(new PrimaryKey(bvin));
        }
        public List<PriceGroup> FindAll()
        {            
            long storeId = context.CurrentStore.Id;
            IQueryable<Data.EF.bvc_PriceGroup> result = repository.Find().Where(y => y.StoreId == storeId).OrderBy(y => y.Name);
            return ListPoco(result);
        }
        
    }
}
