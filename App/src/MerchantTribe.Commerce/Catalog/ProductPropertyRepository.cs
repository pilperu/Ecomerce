using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MerchantTribe.Web.Data;
using MerchantTribe.Web.Logging;

namespace MerchantTribe.Commerce.Catalog
{
    public class ProductPropertyRepository: ConvertingRepositoryBase<Data.EF.bvc_ProductProperty, ProductProperty>
    {
        private RequestContext context = null;
        private ProductPropertyChoiceRepository choiceRepository = null;

        public static ProductPropertyRepository InstantiateForMemory(RequestContext c)
        {
            return new ProductPropertyRepository(c, new MemoryStrategy<Data.EF.bvc_ProductProperty>(PrimaryKeyType.Long),
                                           new MemoryStrategy<Data.EF.bvc_ProductPropertyChoice>(PrimaryKeyType.Long),
                                           new TextLogger());
        }
        public static ProductPropertyRepository InstantiateForDatabase(RequestContext c)
        {
            ProductPropertyRepository result = null;
            result = new ProductPropertyRepository(c,
                new EntityFrameworkRepository<Data.EF.bvc_ProductProperty>(
                    new Data.EF.EntityFrameworkDevConnectionString(c.ConnectionStringForEntityFramework)),
                new EntityFrameworkRepository<Data.EF.bvc_ProductPropertyChoice>(
                    new Data.EF.EntityFrameworkDevConnectionString(c.ConnectionStringForEntityFramework)),
                    new SupressLogger()
                    );
            return result;
        }
        public ProductPropertyRepository(RequestContext c, IRepositoryStrategy<Data.EF.bvc_ProductProperty> r,
                                    IRepositoryStrategy<Data.EF.bvc_ProductPropertyChoice> subr, 
                                    ILogger log)
        {
            context = c;
            repository = r;
            this.logger = log;
            repository.Logger = this.logger;
            this.choiceRepository = new ProductPropertyChoiceRepository(c, subr, this.logger);
        }

        protected override void CopyDataToModel(Data.EF.bvc_ProductProperty data, ProductProperty model)
        {
            model.CultureCode = data.CultureCode;
            model.DefaultValue = data.DefaultValue;
            model.DisplayName = data.DisplayName;
            model.DisplayOnSite = data.DisplayOnSite == 1;
            model.DisplayToDropShipper = data.DisplayToDropShipper == 1;
            model.Id = data.Id;
            model.PropertyName = data.PropertyName;
            model.StoreId = data.StoreId;
            model.TypeCode = (ProductPropertyType)data.TypeCode;
            model.LastUpdatedUtc = data.LastUpdated;
        }
        protected override void CopyModelToData(Data.EF.bvc_ProductProperty data, ProductProperty model)
        {
            data.CultureCode = model.CultureCode;
            data.DefaultValue = model.DefaultValue;
            data.DisplayName = model.DisplayName;
            data.DisplayOnSite = model.DisplayOnSite == true ? 1 : 0;
            data.DisplayToDropShipper = model.DisplayToDropShipper == true ? 1 : 0;
            data.Id = model.Id;
            data.PropertyName = model.PropertyName;
            data.StoreId = model.StoreId;
            data.TypeCode = (int)model.TypeCode;
            data.LastUpdated = model.LastUpdatedUtc;       
        }

        protected override void DeleteAllSubItems(ProductProperty model)
        {
            choiceRepository.DeleteForProperty(model.Id);
        }
        protected override void GetSubItems(ProductProperty model)
        {
            model.Choices = choiceRepository.FindForProperty(model.Id);
        }
        protected override void MergeSubItems(ProductProperty model)
        {
            choiceRepository.MergeList(model.Id, model.Choices);
        }

        public ProductProperty Find(long id)
        {
            ProductProperty result = FindForAllStores(id);
            if (result != null)
            {
                if (result.StoreId == context.CurrentStore.Id)
                {
                    return result;
                }
            }
            return null;
        }
        public ProductProperty FindForAllStores(long id)
        {
            return this.Find(new PrimaryKey(id));
        }
        public ProductProperty FindByName(string name)
        {
            long storeId = context.CurrentStore.Id;
            IQueryable<Data.EF.bvc_ProductProperty> items = repository.Find()
                                                                          .Where(y => y.StoreId == storeId)
                                                                          .Where(y => y.PropertyName == name);

            return FirstPoco(items);
        }
        public override bool Create(ProductProperty item)
        {            
            item.StoreId = context.CurrentStore.Id;            
 	        return base.Create(item);
        }
        public bool Update(ProductProperty c)
        {
            if (c.StoreId != context.CurrentStore.Id)
            {
                return false;
            }
            return this.Update(c, new PrimaryKey(c.Id));            
        }
        public bool Delete(long id)
        {
            long storeId = context.CurrentStore.Id;
            ProductProperty item = Find(id);
            if (item == null) return false;

           return Delete(new PrimaryKey(id));            
        }
        public List<ProductProperty> FindAll()
        {
            List<ProductProperty> result = new List<ProductProperty>();

            long storeId = context.CurrentStore.Id;

            IQueryable<Data.EF.bvc_ProductProperty> items = repository.Find()
                                                                          .Where(y => y.StoreId == storeId)
                                                                          .OrderBy(y => y.PropertyName);
            if (items != null)
            {
                result = ListPoco(items);
            }

            return result;
        }

        //public List<ProductProperty> FindAllShared(int pageNumber, int pageSize)
        //{
        //    List<ProductProperty> result = new List<ProductProperty>();

        //    if (pageNumber < 1) pageNumber = 1;

        //    int take = pageSize;
        //    int skip = (pageNumber - 1) * pageSize;
        //    long storeId = context.CurrentStore.Id;

        //    IQueryable<Data.EF.bvc_ProductProperty> items = repository.Find().Where(y => y.IsShared == true)
        //                                                                  .Where(y => y.StoreId == storeId)
        //                                                                  .OrderBy(y => y.Name)
        //                                                                  .Skip(skip)
        //                                                                  .Take(take);
        //    if (items != null)
        //    {
        //        result = ListPoco(items);
        //    }

        //    return result;
        //}
        public List<ProductProperty> FindMany(List<long> ids)
        {
            List<ProductProperty> result = new List<ProductProperty>();

            long storeId = context.CurrentStore.Id;

            IQueryable<Data.EF.bvc_ProductProperty> items = repository.Find().Where(y => ids.Contains(y.Id))
                                                                          .Where(y => y.StoreId == storeId)
                                                                          .OrderBy(y => y.PropertyName);
            if (items != null)
            {
                result = ListPoco(items);
            }

            return result;
        }

        //public List<ProductProperty> FindByProductId(string productBvin)
        //{
        //    List<ProductOptionAssociation> crosses = optionCrosses.FindForProduct(productBvin, 1, 1000);
        //    List<string> ids = new List<string>();
        //    foreach (ProductOptionAssociation cross in crosses)
        //    {
        //        ids.Add(cross.OptionBvin);
        //    }

        //    // FindMany sorts by BVIN so we
        //    // need to resort based on option order
        //    // in ProductXOption table
        //    List<ProductProperty> unsorted = FindMany(ids);
        //    List<ProductProperty> result = new List<ProductProperty>();
        //    foreach (ProductOptionAssociation cross in crosses)
        //    {
        //        ProductProperty found = unsorted.Where(y => y.Bvin == cross.OptionBvin).FirstOrDefault();
        //        if (found != null) result.Add(found);
        //    }

        //    return result;
        //}
        public bool DeleteChoice(long id)
        {
            return this.choiceRepository.Delete(id);
        }
        public bool ResortChoices(long propertyId, List<long> sortedItemIds)
        {
            return this.choiceRepository.Resort(propertyId, sortedItemIds);
        }
        public ProductPropertyChoice ChoiceFind(long id)
        {
            return this.choiceRepository.Find(id);
        }
        public bool ChoiceUpdate(ProductPropertyChoice item)
        {
            return this.choiceRepository.Update(item);
        }
        public bool MoveChoiceUp(long propertyID, long choiceID)
        {
            bool ret = false;

            List<ProductPropertyChoice> peers;
            peers = choiceRepository.FindForProperty(propertyID);

            if (peers != null)
            {
                int currentSort = 0;
                int targetSort = 0;
                long targetID = 0;
                bool foundTarget = false;

                for (int i = 0; i <= peers.Count - 1; i++)
                {
                    // process last request

                    if (peers[i].Id == choiceID)
                    {
                        currentSort = peers[i].SortOrder;
                        // process last request
                        foundTarget = true;
                    }
                    else
                    {
                        if (foundTarget == false)
                        {
                            targetSort = peers[i].SortOrder;
                            targetID = peers[i].Id;
                        }
                    }

                }

                if (foundTarget == true)
                {
                    ret = true;
                    if (peers.Count > 1)
                    {
                        for (int k = 0; k <= peers.Count - 1; k++)
                        {
                            if (peers[k].Id == choiceID)
                            {
                                peers[k].SortOrder = targetSort;
                                choiceRepository.Update(peers[k]);
                            }
                            if (peers[k].Id == targetID)
                            {
                                peers[k].SortOrder = currentSort;
                                choiceRepository.Update(peers[k]);
                            }
                        }
                    }
                }

            }

            peers = null;

            return ret;
        }
        public bool MoveChoiceDown(long propertyID, long choiceID)
        {
            bool ret = false;

            List<ProductPropertyChoice> peers;
            peers = choiceRepository.FindForProperty(propertyID);

            if (peers != null)
            {
                int currentSort = 0;
                int targetSort = 0;
                long targetID = 0;
                bool foundCurrent = false;
                bool foundTarget = false;

                for (int i = 0; i <= peers.Count - 1; i++)
                {
                    if (foundCurrent)
                    {
                        targetID = peers[i].Id;
                        targetSort = peers[i].SortOrder;
                        foundCurrent = false;
                        foundTarget = true;
                    }
                    if (peers[i].Id == choiceID)
                    {
                        currentSort = peers[i].SortOrder;
                        foundCurrent = true;
                    }

                }

                if (foundTarget == true)
                {
                    ret = true;
                    if (peers.Count > 1)
                    {
                        for (int k = 0; k <= peers.Count - 1; k++)
                        {
                            if (peers[k].Id == choiceID)
                            {
                                peers[k].SortOrder = targetSort;
                                choiceRepository.Update(peers[k]);
                            }
                            if (peers[k].Id == targetID)
                            {
                                peers[k].SortOrder = currentSort;
                                choiceRepository.Update(peers[k]);
                            }
                        }
                    }
                }

            }

            peers = null;

            return ret;
        }
    }
}
