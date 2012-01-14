using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MerchantTribe.Web.Data;
using MerchantTribe.Web.Logging;

namespace MerchantTribe.Commerce.Shipping
{
    public class ZoneRepository: ConvertingRepositoryBase<Data.EF.ecommrc_ShippingZones, Zone>, IZoneRepository
    {
        private RequestContext context = null;

        public static ZoneRepository InstantiateForMemory(RequestContext c)
        {
            ZoneRepository result = null;
            result = new ZoneRepository(c,
                        new MemoryStrategy<Data.EF.ecommrc_ShippingZones>(PrimaryKeyType.Long), new TextLogger());
            return result;
        }
        public static ZoneRepository InstantiateForDatabase(RequestContext c)
        {
            ZoneRepository result = null;
            result = new ZoneRepository(c,
                     new EntityFrameworkRepository<Data.EF.ecommrc_ShippingZones>(new Data.EF.EntityFrameworkDevConnectionString(c.ConnectionStringForEntityFramework)),
                     new SupressLogger());            
            return result;
        }
        public ZoneRepository(RequestContext c, IRepositoryStrategy<Data.EF.ecommrc_ShippingZones> r,ILogger log)
        {
            context = c;
            repository = r;
            this.logger = log;
            repository.Logger = this.logger;
        }

        protected override void CopyDataToModel(Data.EF.ecommrc_ShippingZones data, Zone model)
        {
            model.Id = data.Id;
            model.StoreId = data.StoreId;
            model.Name = data.Name;
            model.Areas = MerchantTribe.Web.Json.ObjectFromJson<List<ZoneArea>>(data.Areas);      
        }
        protected override void CopyModelToData(Data.EF.ecommrc_ShippingZones data, Zone model)
        {
            data.Id = model.Id;
            data.StoreId = model.StoreId;
            data.Name = model.Name;
            data.Areas = MerchantTribe.Web.Json.ObjectToJson(model.Areas);
        }

        public override bool Create(Zone item)
        {
            item.StoreId = context.CurrentStore.Id;
            return base.Create(item);
        }
        public bool Update(Zone c)
        {
            return this.Update(c, new PrimaryKey(c.Id));
        }
        public bool Delete(long id)
        {
            long storeId = context.CurrentStore.Id;
            Zone item = Find(id);
            if (item == null) return false;
            if (item.StoreId != storeId) return false;
            return Delete(new PrimaryKey(id));
        }

        public Zone Find(long id)
        {
            Zone result = FindForAllStores(id);
            if (result != null)
            {
                if (result.StoreId == context.CurrentStore.Id)
                {
                    return result;
                }
            }
            return null;
        }
        public Zone FindForAllStores(long id)
        {
            Data.EF.ecommrc_ShippingZones data = repository.FindByPrimaryKey(new PrimaryKey(id));
            if (data == null) return null;
            Zone result = new Zone();
            CopyDataToModel(data, result);
            return result;
        }

        public bool NameExists(string name, long currentId, long storeId)
        {
            bool result = false;

            List<Zone> zones = FindForStore(storeId);
            if (zones != null)
            {
                foreach (Zone z in zones)
                {
                    if (z.Name.Trim().ToLowerInvariant() == name.Trim().ToLowerInvariant())
                    {
                        if (z.Id != currentId)
                        {
                            return true;
                        }
                    }
                }
            }

            return result;
        }
        public List<Zone> FindForStore(long storeId)
        {
            List<Zone> result = new List<Zone>();
            result.Add(Zone.UnitedStatesAll());
            result.Add(Zone.UnitedStates48Contiguous());
            result.Add(Zone.UnitedStatesAlaskaAndHawaii());
            result.Add(Zone.International("USA"));
            
            try
            {
                var x = repository.Find().Where(y => y.StoreId == storeId).OrderBy(y => y.Name);
                if (x != null)
                {
                    foreach (Data.EF.ecommrc_ShippingZones n in x)
                    {
                        Zone a = new Zone();
                        CopyDataToModel(n, a);
                        result.Add(a);
                    }
                }
            }
            catch (Exception ex)
            {
                EventLog.LogEvent(ex);
            }
            return result;
        }
        public List<Zone> FindAllZonesForAddress(MerchantTribe.Web.Geography.IAddress address, long storeId)
        {
            List<Zone> all = FindForStore(storeId);

            List<Zone> result = new List<Zone>();

            foreach (Zone z in all)
            {
                if (z.AddressIsInZone(address))
                {
                    result.Add(z);
                }
            }
            return result;
        }
        public List<Zone> GetZones(long storeId)
        {
            return FindForStore(storeId);
        }

    }
}
