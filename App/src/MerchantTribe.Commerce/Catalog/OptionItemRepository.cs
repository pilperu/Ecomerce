using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MerchantTribe.Web.Data;
using MerchantTribe.Commerce.Content;
using MerchantTribe.Web.Logging;

namespace MerchantTribe.Commerce.Catalog
{
    class OptionItemRepository: ConvertingRepositoryBase<Data.EF.bvc_ProductOptionsItems, OptionItem>
    {
        RequestContext context = null;

        public static OptionItemRepository InstantiateForMemory(RequestContext c)
        {
            OptionItemRepository result = null;            
            ILogger logger = new TextLogger();
            result = new OptionItemRepository(c, new MemoryStrategy<Data.EF.bvc_ProductOptionsItems>(PrimaryKeyType.Bvin), logger);
            return result;
        }
        public static OptionItemRepository InstantiateForDatabase(RequestContext c)
        {            
            OptionItemRepository result = null;
            ILogger logger = new SupressLogger();
            result = new OptionItemRepository(c, new EntityFrameworkRepository<Data.EF.bvc_ProductOptionsItems>(
                    new Data.EF.EntityFrameworkDevConnectionString(c.ConnectionStringForEntityFramework)), logger);
            return result;
        }
        public OptionItemRepository(RequestContext c, IRepositoryStrategy<Data.EF.bvc_ProductOptionsItems> strategy, ILogger log)
        {
            this.context = c;
            repository = strategy;
            this.logger = log;
            repository.Logger = this.logger;
        }

        protected override void CopyModelToData(Data.EF.bvc_ProductOptionsItems data, OptionItem model)
        {
            data.bvin = model.Bvin;
            data.IsLabel = model.IsLabel;
            data.Name = model.Name;
            data.OptionBvin = model.OptionBvin;
            data.PriceAdjustment = model.PriceAdjustment;
            data.SortOrder = model.SortOrder;
            data.StoreId = model.StoreId;
            data.WeightAdjustment = model.WeightAdjustment;
        }
        protected override void CopyDataToModel(Data.EF.bvc_ProductOptionsItems data, OptionItem model)
        {
            model.Bvin = data.bvin;
            model.IsLabel = data.IsLabel;
            model.Name = data.Name;
            model.OptionBvin = data.OptionBvin;
            model.PriceAdjustment = data.PriceAdjustment;
            model.SortOrder = data.SortOrder;
            model.StoreId = data.StoreId;
            model.WeightAdjustment = data.WeightAdjustment;
        }

        public OptionItem Find(string bvin)
        {
            Data.EF.bvc_ProductOptionsItems data = repository.FindByPrimaryKey(new PrimaryKey(bvin));
            if (data == null) return null;
            OptionItem result = new OptionItem();
            CopyDataToModel(data, result);
            return result;            
        }
        public override bool Create(OptionItem item)
        {
            if (item.Bvin == string.Empty)
            {
                item.Bvin = System.Guid.NewGuid().ToString();
            }
            item.StoreId = context.CurrentStore.Id;

            return base.Create(item);
        }
        public bool Update(OptionItem item)
        {            
            return base.Update(item, new PrimaryKey(item.Bvin));
        }
        public bool Delete(string bvin)
        {
            return Delete(new PrimaryKey(bvin));
        }
        public List<OptionItem> FindForOption(string optionId)
        {
            var items = repository.Find().Where(y => y.OptionBvin == optionId)
                                        .OrderBy(y => y.SortOrder);
            return ListPoco(items);
        }
        public void DeleteForOption(string optionBvin)
        {
            List<OptionItem> existing = FindForOption(optionBvin);
            foreach (OptionItem sub in existing)
            {
                Delete(sub.Bvin);
            }
        }

        private int FindMaxSort(List<OptionItem> items)
        {
            int maxSort = 0;
            if (items == null) return 0;
            if (items.Count < 1) return 0;
            maxSort = items.Max(y => y.SortOrder);
            return maxSort;
        }
        public bool Resort(string optionBvin, List<string> sortedIds)
        {
            if (sortedIds != null)
            {
                for (int i = 1; i <= sortedIds.Count; i++)
                {
                    UpdateSortOrderForOptionItem(optionBvin, sortedIds[i - 1], i);
                }
            }
            return true;
        }
        private bool UpdateSortOrderForOptionItem(string optionBvin, string optionItemBvin, int newSortOrder)
        {
            OptionItem item = Find(new PrimaryKey(optionItemBvin));
            if (item == null) return false;
            if (item.OptionBvin != optionBvin) return false;
            item.SortOrder = newSortOrder;
            return Update(item);
        }
        public void MergeList(string optionBvin, List<OptionItem> subitems)
        {
            // Set Base Key Field
            foreach (OptionItem item in subitems)
            {
                item.OptionBvin = optionBvin;
                item.StoreId = context.CurrentStore.Id;
            }

            int maxSort = FindMaxSort(subitems);

            // Create or Update
            foreach (OptionItem itemnew in subitems)
            {                             
                {
                    if (itemnew.Bvin == string.Empty)
                    {
                        maxSort++;
                        itemnew.SortOrder = maxSort;
                        Create(itemnew);
                    }
                    else
                    {
                        OptionItem existingNew = Find(itemnew.Bvin);
                        if (existingNew == null)
                        {
                            Create(itemnew);
                        }                        
                        Update(itemnew);
                    }
                }
            }    
        
            // Delete missing
            List<OptionItem> existing = FindForOption(optionBvin);
            foreach (OptionItem ex in existing)
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
