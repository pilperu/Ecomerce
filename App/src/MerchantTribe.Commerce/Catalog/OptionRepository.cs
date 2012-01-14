using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MerchantTribe.Web.Data;
using MerchantTribe.Web.Logging;

namespace MerchantTribe.Commerce.Catalog
{
    public class OptionRepository: ConvertingRepositoryBase<Data.EF.bvc_ProductOptions, Option>
    {
        private RequestContext context = null;
        private OptionItemRepository itemRepository = null;
        private ProductOptionAssociationRepository optionCrosses = null;

        public static OptionRepository InstantiateForMemory(RequestContext c)
        {
            return new OptionRepository(c, new MemoryStrategy<Data.EF.bvc_ProductOptions>(PrimaryKeyType.Bvin),
                                           new MemoryStrategy<Data.EF.bvc_ProductOptionsItems>(PrimaryKeyType.Bvin),
                                           new MemoryStrategy<Data.EF.bvc_ProductXOption>(PrimaryKeyType.Long),
                                           new TextLogger());
        }
        public static OptionRepository InstantiateForDatabase(RequestContext c)
        {
            OptionRepository result = null;
            result = new OptionRepository(c, 
                new EntityFrameworkRepository<Data.EF.bvc_ProductOptions>(
                    new Data.EF.EntityFrameworkDevConnectionString(c.ConnectionStringForEntityFramework)),
                new EntityFrameworkRepository<Data.EF.bvc_ProductOptionsItems>(
                    new Data.EF.EntityFrameworkDevConnectionString(c.ConnectionStringForEntityFramework)),
                new EntityFrameworkRepository<Data.EF.bvc_ProductXOption>(
                    new Data.EF.EntityFrameworkDevConnectionString(c.ConnectionStringForEntityFramework)),
                    new SupressLogger()
                    );
            return result;
        }               
        public OptionRepository(RequestContext c, IRepositoryStrategy<Data.EF.bvc_ProductOptions> r,
                                    IRepositoryStrategy<Data.EF.bvc_ProductOptionsItems> subr, 
                                    IRepositoryStrategy<Data.EF.bvc_ProductXOption> crosses, ILogger log)
        {
            context = c;
            repository = r;
            this.logger = log;
            repository.Logger = this.logger;
            itemRepository = new OptionItemRepository(c, subr, this.logger);
            this.optionCrosses = new ProductOptionAssociationRepository(c, crosses, this.logger);
        }

        protected override void CopyDataToModel(Data.EF.bvc_ProductOptions data, Option model)
        {            
            model.Bvin = data.bvin;
            model.IsShared = data.IsShared;
            model.IsVariant = data.IsVariant;
            model.SetProcessor((OptionTypes)data.OptionType);
            model.Name = data.Name;
            model.NameIsHidden = data.NameIsHidden;
            model.Settings = OptionSettings.FromJson(data.Settings);
            model.StoreId = data.StoreId;            
        }
        protected override void CopyModelToData(Data.EF.bvc_ProductOptions data, Option model)
        {
            data.bvin = model.Bvin;
            data.IsShared = model.IsShared;
            data.IsVariant = model.IsVariant;
            data.OptionType = (int)model.OptionType;
            data.Name = model.Name;
            data.NameIsHidden = model.NameIsHidden;
            data.Settings = model.Settings.ToJson();
            data.StoreId = model.StoreId;
        }

        protected override void DeleteAllSubItems(Option model)
        {
            itemRepository.DeleteForOption(model.Bvin);
        }
        protected override void GetSubItems(Option model)
        {
            model.Items = itemRepository.FindForOption(model.Bvin);
        }
        protected override void MergeSubItems(Option model)
        {
            itemRepository.MergeList(model.Bvin, model.Items);
        }
            
        public Option Find(string bvin)
        {
            Option result = FindForAllStores(bvin);
            if (result != null)
            {
                if (result.StoreId == context.CurrentStore.Id)
                {
                    return result;
                }
            }
            return null;
        }
        public Option FindForAllStores(string bvin)
        {
            return this.Find(new PrimaryKey(bvin));
        }
        public override bool Create(Option item)
        {
            if (item.Bvin == string.Empty)
            {
                item.Bvin = System.Guid.NewGuid().ToString();
            }
            item.StoreId = context.CurrentStore.Id;            
 	        return base.Create(item);
        }       
        public bool Update(Option c)
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
            Option item = Find(bvin);
            if (item == null) return false;

           return Delete(new PrimaryKey(bvin));            
        }

        public List<Option> FindAllShared(int pageNumber, int pageSize)
        {
            List<Option> result = new List<Option>();

            if (pageNumber < 1) pageNumber = 1;

            int take = pageSize;
            int skip = (pageNumber - 1) * pageSize;
            long storeId = context.CurrentStore.Id;

            IQueryable<Data.EF.bvc_ProductOptions> items = repository.Find().Where(y => y.IsShared == true)
                                                                          .Where(y => y.StoreId == storeId)
                                                                          .OrderBy(y => y.Name)
                                                                          .Skip(skip)
                                                                          .Take(take);
            if (items != null)
            {
                result = ListPoco(items);
            }

            return result;
        }
        public List<Option> FindMany(List<string> ids)
        {
            List<Option> result = new List<Option>();

            long storeId = context.CurrentStore.Id;

            IQueryable<Data.EF.bvc_ProductOptions> items = repository.Find().Where(y => ids.Contains(y.bvin))
                                                                          .Where(y => y.StoreId == storeId)
                                                                          .OrderBy(y => y.bvin);
            if (items != null)
            {
                result = ListPoco(items);
            }

            return result;
        }
      
        public List<Option> FindByProductId(string productBvin)
        {
            List<ProductOptionAssociation> crosses = optionCrosses.FindForProduct(productBvin, 1, 1000);
            List<string> ids = new List<string>();
            foreach (ProductOptionAssociation cross in crosses)
            {
                ids.Add(cross.OptionBvin);
            }

            // FindMany sorts by BVIN so we
            // need to resort based on option order
            // in ProductXOption table
            List<Option> unsorted = FindMany(ids);
            List<Option> result = new List<Option>();
            foreach (ProductOptionAssociation cross in crosses)
            {
                Option found = unsorted.Where(y => y.Bvin == cross.OptionBvin).FirstOrDefault();
                if (found != null) result.Add(found);
            }

            return result;
        }
        public bool DeleteForProductId(string productId)
        {
            List<Option> opts = FindByProductId(productId);
            optionCrosses.DeleteAllForProduct(productId);            
            foreach (Option o in opts)
            {                
                if (o.IsShared == false)
                {
                    Delete(o.Bvin);
                }
            }
            return true;
        }
        public void MergeList(string productBvin, OptionList subitems)
        {
            long storeId = context.CurrentStore.Id;
            // Set Base Key Field
            foreach (Option item in subitems)
            {
                item.StoreId = storeId;
            }

            // Create or Update
            foreach (Option itemnew in subitems)
            {
                if (itemnew.Bvin == string.Empty)
                {                    
                    Create(itemnew);
                    optionCrosses.AddOptionToProduct(productBvin, itemnew.Bvin);
                }
                else
                {
                    Update(itemnew);
                }
            }

            // Delete missing
            List<Option> existing = FindByProductId(productBvin);
            foreach (Option ex in existing)
            {                
                var count = (from sub in subitems
                             where sub.Bvin == ex.Bvin
                             select sub).Count();
                if (count < 1)
                {
                    optionCrosses.RemoveOptionFromProduct(productBvin, ex.Bvin);
                    if (ex.IsShared == false)
                    {
                        Delete(ex.Bvin);
                    }
                }
            }
        }

        public bool DeleteOptionItem(string optionItemBvin)
        {
            return this.itemRepository.Delete(optionItemBvin);
        }
        public bool ResortOptionItems(string optionId, List<string> sortedItemIds)
        {
            return this.itemRepository.Resort(optionId, sortedItemIds);
        }
        public OptionItem OptionItemFind(string bvin)
        {
            return this.itemRepository.Find(bvin);
        }
        public bool OptionItemUpdate(OptionItem item)
        {
            return this.itemRepository.Update(item);
        }
    }
}
