using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MerchantTribe.Web.Data;
using MerchantTribe.Web.Logging;

namespace MerchantTribe.Commerce.Shipping
{
    public class ShippingMethodRepository: ConvertingRepositoryBase<Data.EF.bvc_ShippingMethod, ShippingMethod>
    {
        private RequestContext context = null;

        public static ShippingMethodRepository InstantiateForMemory(RequestContext c)
        {
            ShippingMethodRepository result = null;
            result = new ShippingMethodRepository(c,
                        new MemoryStrategy<Data.EF.bvc_ShippingMethod>(PrimaryKeyType.Bvin), new TextLogger());
            return result;
        }
        public static ShippingMethodRepository InstantiateForDatabase(RequestContext c)
        {
            ShippingMethodRepository result = null;
            result = new ShippingMethodRepository(c,
                     new EntityFrameworkRepository<Data.EF.bvc_ShippingMethod>(new Data.EF.EntityFrameworkDevConnectionString(c.ConnectionStringForEntityFramework)),
                     new SupressLogger());            
            return result;
        }
        public ShippingMethodRepository(RequestContext c, IRepositoryStrategy<Data.EF.bvc_ShippingMethod> r, ILogger log)
        {
            context = c;
            repository = r;
            this.logger = log;
            repository.Logger = this.logger;
        }

        protected override void CopyDataToModel(Data.EF.bvc_ShippingMethod data, ShippingMethod model)
        {
            model.Adjustment = data.Adjustment;
            model.AdjustmentType = (ShippingMethodAdjustmentType)data.AdjustmentType;
            model.Bvin = data.bvin;
            model.LastUpdated = data.LastUpdated;
            model.Name = data.Name;
            model.Settings = MerchantTribe.Web.Json.ObjectFromJson<MerchantTribe.Shipping.ServiceSettings>(data.Settings);      
            model.ShippingProviderId = data.ShippingProviderId;
            model.StoreId = data.StoreId;
            model.ZoneId = data.ZoneId;            
        }
        protected override void CopyModelToData(Data.EF.bvc_ShippingMethod data, ShippingMethod model)
        {
            data.Adjustment = model.Adjustment;
            data.AdjustmentType = (int)model.AdjustmentType;
            data.bvin = model.Bvin;
            data.LastUpdated = model.LastUpdated;
            data.Name = model.Name;
            data.Settings = MerchantTribe.Web.Json.ObjectToJson(model.Settings);
            data.ShippingProviderId = model.ShippingProviderId;
            data.StoreId = model.StoreId;
            data.ZoneId = model.ZoneId; 
        }

        public override bool Create(ShippingMethod item)
        {
            if (item.Bvin == string.Empty)
            {
                item.Bvin = System.Guid.NewGuid().ToString();
            }
            item.LastUpdated = DateTime.UtcNow;
            item.StoreId = context.CurrentStore.Id;
            return base.Create(item);
        }
        public bool Update(ShippingMethod item)
        {
            item.LastUpdated = DateTime.UtcNow;
            return this.Update(item, new PrimaryKey(item.Bvin));
        }
        public bool Delete(string bvin)
        {
            long storeId = context.CurrentStore.Id;
            ShippingMethod item = Find(bvin);
            if (item == null) return false;
            if (item.StoreId != storeId) return false;
            return Delete(new PrimaryKey(bvin));
        }

        public ShippingMethod Find(string bvin)
        {

            Data.EF.bvc_ShippingMethod data = repository.FindByPrimaryKey(new PrimaryKey(bvin));
            if (data == null) return null;

            ShippingMethod result = new ShippingMethod();
            CopyDataToModel(data, result);                                    
            if (result.StoreId == context.CurrentStore.Id)
            {
                return result;
            }            
            return null;
        }
              
        public List<ShippingMethod> FindAll(long storeId)
        {
            List<ShippingMethod> result = new List<ShippingMethod>();

            try
            {
                var x = repository.Find().Where(y => y.StoreId == storeId).OrderBy(y => y.Name);
                if (x != null)
                {
                    foreach (Data.EF.bvc_ShippingMethod item in x)
                    {
                        ShippingMethod m = new ShippingMethod();
                        CopyDataToModel(item, m);
                        result.Add(m);
                    }
                }
            }
            catch (Exception ex)
            {
                EventLog.LogEvent(ex);
            }
            return result;
        }

        public List<ShippingMethod> FindForZones(List<Zone> zones)
        {
            long storeId = context.CurrentStore.Id;
            List<ShippingMethod> all = FindAll(storeId);

            List<ShippingMethod> result = new List<ShippingMethod>();

            foreach (ShippingMethod m in all)
            {
                foreach (Zone z in zones)
                {
                    if (m.ZoneId == z.Id)
                    {
                        result.Add(m);
                    }
                }
            }
            return result;
        }
    }
}
