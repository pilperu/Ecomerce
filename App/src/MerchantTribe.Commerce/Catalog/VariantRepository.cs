using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MerchantTribe.Web.Data;
using MerchantTribe.Web.Logging;

namespace MerchantTribe.Commerce.Catalog
{
    public class VariantRepository : ConvertingRepositoryBase<Data.EF.bvc_Variants, Variant>
    {
        private RequestContext context = null;

        public static VariantRepository InstantiateForMemory(RequestContext c)
        {
            return new VariantRepository(c, new MemoryStrategy<Data.EF.bvc_Variants>(PrimaryKeyType.Bvin), new NullLogger());
        }
        public static VariantRepository InstantiateForDatabase(RequestContext c)
        {
            VariantRepository result = null;
            result = new VariantRepository(c,
                new EntityFrameworkRepository<Data.EF.bvc_Variants>(
                    new Data.EF.EntityFrameworkDevConnectionString(c.ConnectionStringForEntityFramework)),
                    new SupressLogger());
            return result;
        }
        public VariantRepository(RequestContext c, IRepositoryStrategy<Data.EF.bvc_Variants> r, ILogger log)
        {
            context = c;
            repository = r;
            this.logger = log;
            repository.Logger = this.logger;
        }

        protected override void CopyDataToModel(Data.EF.bvc_Variants data, Variant model)
        {
            model.Bvin = data.bvin;
            model.Price = data.Price;
            model.ProductId = data.ProductId;
            List<Catalog.OptionSelection> selectionData = MerchantTribe.Web.Json.ObjectFromJson<List<Catalog.OptionSelection>>(data.SelectionData);
            if (selectionData != null)
            {
               model.Selections.Clear();
               model.Selections.AddRange(selectionData);
            }
            model.Sku = data.Sku ?? string.Empty;
            model.StoreId = data.StoreId;            
        }
        protected override void CopyModelToData(Data.EF.bvc_Variants data, Variant model)
        {
            data.bvin = model.Bvin;
            data.Price = model.Price;
            data.ProductId = model.ProductId;
            data.SelectionData = MerchantTribe.Web.Json.ObjectToJson(model.Selections);
            data.Sku = model.Sku;
            data.StoreId = model.StoreId;
        }

     
        public Variant Find(string bvin)
        {
            Variant result = FindForAllStores(bvin);
            if (result != null)
            {
                if (result.StoreId == context.CurrentStore.Id)
                {
                    return result;
                }
            }
            return null;
        }
        public Variant FindForAllStores(string bvin)
        {
            Data.EF.bvc_Variants data = repository.FindByPrimaryKey(new PrimaryKey(bvin));
            if (data == null) return null;

            Variant result = new Variant();
            CopyDataToModel(data, result);
            return result;
        }
        public override bool Create(Variant item)
        {
            if (item.Bvin == string.Empty)
            {
                item.Bvin = System.Guid.NewGuid().ToString();
            }
            item.StoreId = context.CurrentStore.Id;
 	        return base.Create(item);
        }
        public bool Update(Variant c)
        {
            if (c.StoreId != context.CurrentStore.Id)
            {
                return false;
            }
            return this.Update(c, new PrimaryKey(c.Bvin));            
        }
        public bool Delete(string bvin)
        {
            long storeId = context.CurrentStore.Id;
            Variant item = Find(bvin);
            if (item == null) return false;
            return Delete(new PrimaryKey(bvin));            
        }

        public List<Variant> FindByProductId(string productId)
        {
            return FindByProductIdPaged(productId, 1, int.MaxValue);
        }
        public List<Variant> FindByProductIdPaged(string productId, int pageNumber, int pageSize)
        {
            List<Variant> result = new List<Variant>();

            if (pageNumber < 1) pageNumber = 1;

            int take = pageSize;
            int skip = (pageNumber - 1) * pageSize;
            long storeId = context.CurrentStore.Id;

            IQueryable<Data.EF.bvc_Variants> items = repository.Find().Where(y => y.ProductId == productId)
                                                                          .Where(y => y.StoreId == storeId)       
                                                                          .OrderBy(y => y.ProductId)
                                                                          .Skip(skip)
                                                                          .Take(take);
            if (items != null)
            {
                result = ListPoco(items);
            }

            return result;
        }
        public bool DeleteForProductId(string productBvin)
        {
            List<Variant> items = FindByProductId(productBvin);
            if (items != null)
            {
                foreach (Variant v in items)
                {
                    Delete(v.Bvin);
                }
            }
            return true;
        }
        public void MergeList(string productBvin, List<Variant> subitems)
        {
            long storeId = context.CurrentStore.Id;
            // Set Base Key Field
            foreach (Variant item in subitems)
            {
                item.ProductId = productBvin;
                item.StoreId = storeId;
            }

            // Create or Update
            foreach (Variant itemnew in subitems)
            {
                if (itemnew.Bvin == string.Empty)
                {                    
                    Create(itemnew);
                }
                else
                {
                    Update(itemnew);
                }
            }

            // Delete missing
            List<Variant> existing = FindByProductId(productBvin);
            foreach (Variant ex in existing)
            {
                var count = (from sub in subitems
                             where sub.Bvin == ex.Bvin
                             select sub).Count();
                if (count < 1)
                {
                    Delete(ex.Bvin);
                }
            }
        }
    }
}
