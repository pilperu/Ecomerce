using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MerchantTribe.Web.Data;
using MerchantTribe.Commerce.Content;
using MerchantTribe.Web.Logging;

namespace MerchantTribe.Commerce.Catalog
{
    class ProductPropertyChoiceRepository: ConvertingRepositoryBase<Data.EF.bvc_ProductPropertyChoice, ProductPropertyChoice>
    {
        RequestContext context = null;

        public static ProductPropertyChoiceRepository InstantiateForMemory(RequestContext c)
        {
            ProductPropertyChoiceRepository result = null;            
            ILogger logger = new TextLogger();
            result = new ProductPropertyChoiceRepository(c, new MemoryStrategy<Data.EF.bvc_ProductPropertyChoice>(PrimaryKeyType.Integer), logger);
            return result;
        }
        public static ProductPropertyChoiceRepository InstantiateForDatabase(RequestContext c)
        {            
            ProductPropertyChoiceRepository result = null;
            ILogger logger = new SupressLogger();
            result = new ProductPropertyChoiceRepository(c, new EntityFrameworkRepository<Data.EF.bvc_ProductPropertyChoice>(
                    new Data.EF.EntityFrameworkDevConnectionString(c.ConnectionStringForEntityFramework)), logger);
            return result;
        }
        public ProductPropertyChoiceRepository(RequestContext c, IRepositoryStrategy<Data.EF.bvc_ProductPropertyChoice> strategy, ILogger log)
        {
            this.context = c;
            repository = strategy;
            this.logger = log;
            repository.Logger = this.logger;
        }

        protected override void CopyModelToData(Data.EF.bvc_ProductPropertyChoice data, ProductPropertyChoice model)
        {
            data.ChoiceName = model.ChoiceName;
            data.Id = model.Id;
            data.LastUpdated = model.LastUpdated;
            data.PropertyId = model.PropertyId;
            data.SortOrder = model.SortOrder;
            data.StoreId = model.StoreId;            
        }
        protected override void CopyDataToModel(Data.EF.bvc_ProductPropertyChoice data, ProductPropertyChoice model)
        {
            model.ChoiceName = data.ChoiceName;
            model.Id = data.Id;
            model.LastUpdated = data.LastUpdated;
            model.PropertyId = data.PropertyId;
            model.SortOrder = data.SortOrder;
            model.StoreId = data.StoreId;         
        }

        public ProductPropertyChoice Find(long id)
        {
            Data.EF.bvc_ProductPropertyChoice data = repository.FindByPrimaryKey(new PrimaryKey(id));
            if (data == null) return null;
            ProductPropertyChoice result = new ProductPropertyChoice();
            CopyDataToModel(data, result);
            return result;            
        }
        public override bool Create(ProductPropertyChoice item)
        {
            item.LastUpdated = DateTime.UtcNow;
            //item.SortOrder = FindMaxSort(FindForProperty(item.PropertyId));
            item.StoreId = context.CurrentStore.Id;
            return base.Create(item);
        }
        public bool Update(ProductPropertyChoice item)
        {
            item.LastUpdated = DateTime.UtcNow;
            return base.Update(item, new PrimaryKey(item.Id));
        }
        public bool Delete(long id)
        {
            return Delete(new PrimaryKey(id));
        }
        public List<ProductPropertyChoice> FindForProperty(long propertyId)
        {
            var items = repository.Find().Where(y => y.PropertyId == propertyId)
                                        .OrderBy(y => y.SortOrder);
            return ListPoco(items);
        }
        public void DeleteForProperty(long propertyId)
        {
            List<ProductPropertyChoice> existing = FindForProperty(propertyId);
            foreach (ProductPropertyChoice sub in existing)
            {
                Delete(sub.Id);
            }
        }

        private int FindMaxSort(List<ProductPropertyChoice> items)
        {
            int maxSort = 0;
            if (items == null) return 0;
            if (items.Count < 1) return 0;
            maxSort = items.Max(y => y.SortOrder);
            return maxSort;
        }
        public bool Resort(long propertyId, List<long> sortedIds)
        {
            if (sortedIds != null)
            {
                for (int i = 1; i <= sortedIds.Count; i++)
                {
                    UpdateSortOrderForChoice(propertyId, sortedIds[i - 1], i);
                }
            }
            return true;
        }
        private bool UpdateSortOrderForChoice(long propertyId, long choiceId, int newSortOrder)
        {
            ProductPropertyChoice item = Find(choiceId);
            if (item == null) return false;
            if (item.PropertyId != propertyId) return false;
            item.SortOrder = newSortOrder;
            return Update(item);
        }
        public void MergeList(long propertyId, List<ProductPropertyChoice> subitems)
        {
            // Set Base Key Field
            foreach (ProductPropertyChoice item in subitems)
            {
                item.PropertyId = propertyId;
            }

            int maxSort = FindMaxSort(subitems);

            // Create or Update
            foreach (ProductPropertyChoice itemnew in subitems)
            {
                if (itemnew.Id < 1)
                {
                    maxSort++;
                    itemnew.SortOrder = maxSort;
                    Create(itemnew);
                }
                else
                {
                    Update(itemnew);
                }
            }    
        
            // Delete missing
            List<ProductPropertyChoice> existing = FindForProperty(propertyId);
            foreach (ProductPropertyChoice ex in existing)
            {
                var count = (from sub in subitems
                             where sub.Id == ex.Id
                             select sub).Count();
                if (count < 1)                
                {
                    Delete(ex.Id);
                }
            }
        }

    }
}
