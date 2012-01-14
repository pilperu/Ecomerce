using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MerchantTribe.Web.Data;
using MerchantTribe.Web.Logging;

namespace MerchantTribe.Commerce.Catalog
{
    public class ProductTypePropertyAssociationRepository: ConvertingRepositoryBase<Data.EF.bvc_ProductTypeXProductProperty, ProductTypePropertyAssociation>
    {
        private RequestContext context = null;

        public static ProductTypePropertyAssociationRepository InstantiateForMemory(RequestContext c)
        {
            return new ProductTypePropertyAssociationRepository(c, new MemoryStrategy<Data.EF.bvc_ProductTypeXProductProperty>(PrimaryKeyType.Long), new NullLogger());
        }
        public static ProductTypePropertyAssociationRepository InstantiateForDatabase(RequestContext c)
        {
            ProductTypePropertyAssociationRepository result = null;
            result = new ProductTypePropertyAssociationRepository(c,
                new EntityFrameworkRepository<Data.EF.bvc_ProductTypeXProductProperty>(
                    new Data.EF.EntityFrameworkDevConnectionString(c.ConnectionStringForEntityFramework)),
                    new SupressLogger());
            return result;
        }
        public ProductTypePropertyAssociationRepository(RequestContext c, IRepositoryStrategy<Data.EF.bvc_ProductTypeXProductProperty> r, ILogger log)
        {
            context = c;
            repository = r;
            this.logger = log;
            repository.Logger = this.logger;
        }

        protected override void CopyDataToModel(Data.EF.bvc_ProductTypeXProductProperty data, ProductTypePropertyAssociation model)
        {
            model.Id = data.Id;
            model.StoreId = data.StoreId;
            model.ProductTypeBvin = data.ProductTypeBvin;
            model.PropertyId = data.PropertyId;
            model.SortOrder = data.SortOrder;            
        }
        protected override void CopyModelToData(Data.EF.bvc_ProductTypeXProductProperty data, ProductTypePropertyAssociation model)
        {
            data.Id = model.Id;
            data.StoreId = model.StoreId;
            data.ProductTypeBvin = model.ProductTypeBvin;
            data.PropertyId = model.PropertyId;
            data.SortOrder = model.SortOrder;            
        }

        public ProductTypePropertyAssociation Find(long id)
        {
            ProductTypePropertyAssociation result = FindForAllStores(id);
            if (result != null)
            {
                if (result.StoreId == context.CurrentStore.Id)
                {
                    return result;
                }
            }
            return null;
        }
        public ProductTypePropertyAssociation FindForAllStores(long id)
        {
            Data.EF.bvc_ProductTypeXProductProperty data = repository.FindByPrimaryKey(new PrimaryKey(id));
            if (data == null) return null;

            ProductTypePropertyAssociation result = new ProductTypePropertyAssociation();
            CopyDataToModel(data, result);
            return result;
        }
        public ProductTypePropertyAssociation FindForTypeAndProperty(string typeBvin, long propertyId)
        {
            long storeId = context.CurrentStore.Id;
            IQueryable<Data.EF.bvc_ProductTypeXProductProperty> items = repository.Find()
                                                                          .Where(y => y.StoreId == storeId)
                                                                          .Where(y => y.PropertyId == propertyId)
                                                                          .Where(y => y.ProductTypeBvin == typeBvin);
            return FirstPoco(items);                                                                                                               
        }
        public override bool Create(ProductTypePropertyAssociation item)
        {
            item.StoreId = context.CurrentStore.Id;
            item.SortOrder = FindMaxSort(item.ProductTypeBvin);
 	        return base.Create(item);
        }
        private int FindMaxSort(string typeBvin)
        {
            int maxSort = 0;
            long storeId = context.CurrentStore.Id;
            List<ProductTypePropertyAssociation> result = new List<ProductTypePropertyAssociation>();
            IQueryable<Data.EF.bvc_ProductTypeXProductProperty> items = repository.Find().Where(y => y.StoreId == storeId)
                                                                      .Where(y => y.ProductTypeBvin == typeBvin)
                                                                      .OrderByDescending(y => y.SortOrder)
                                                                      .Take(1);
            if (items != null)
            {
                var i = items.ToList();
                if (i.Count > 0)
                {
                    maxSort = i[0].SortOrder;
                }
            }
            return maxSort;
        }
        public bool Update(ProductTypePropertyAssociation c)
        {
            if (c.StoreId != context.CurrentStore.Id)
            {
                return false;
            }
            return this.Update(c, new PrimaryKey(c.Id));            
        }
        public bool UpdateSortOrder(string typeBvin, long propertyId, int sortOrder)
        {
            ProductTypePropertyAssociation item = FindForTypeAndProperty(typeBvin, propertyId);
            if (item != null)
            {
                item.SortOrder = sortOrder;
                return Update(item);
            }
            return false;
        }
        public bool UpdateSortOrder(long id, int sortOrder)
        {
            ProductTypePropertyAssociation item = Find(id);
            if (item != null)
            {
                item.SortOrder = sortOrder;
                return Update(item);
            }
            return false;
        }
        public bool Delete(long id)
        {
            long storeId = context.CurrentStore.Id;
            ProductTypePropertyAssociation item = Find(id);
            if (item == null) return false;
            return Delete(new PrimaryKey(id));            
        }
        public bool DeleteForProperty(long propertyId)
        {
            List<ProductTypePropertyAssociation> toDelete = FindByProperty(propertyId);
            foreach (ProductTypePropertyAssociation item in toDelete)
            {
                Delete(item.Id);
            }
            return true;
        }
        public bool DeleteForProductType(string productTypeBvin)
        {
            List<ProductTypePropertyAssociation> toDelete = FindByProductType(productTypeBvin);
            foreach (ProductTypePropertyAssociation item in toDelete)
            {
                Delete(item.Id);
            }
            return true;
        }
        public bool DeleteForTypeAndProperty(string typeBvin, long propertyId)
        {
            ProductTypePropertyAssociation item = FindForTypeAndProperty(typeBvin, propertyId);
            if (item == null) return false;
            return Delete(new PrimaryKey(item.Id)); 
        }

        public List<ProductTypePropertyAssociation> FindByProperty(long propertyId)
        {
            List<ProductTypePropertyAssociation> result = new List<ProductTypePropertyAssociation>();

            long storeId = context.CurrentStore.Id;
            IQueryable<Data.EF.bvc_ProductTypeXProductProperty> items = repository.Find().Where(y => y.PropertyId == propertyId)
                                                                          .Where(y => y.StoreId == storeId)
                                                                          .OrderBy(y => y.SortOrder);
            if (items != null)
            {
                result = ListPoco(items);
            }

            return result;
        }
        public List<ProductTypePropertyAssociation> FindByProductType(string productTypeBvin)
        {
            List<ProductTypePropertyAssociation> result = new List<ProductTypePropertyAssociation>();

            long storeId = context.CurrentStore.Id;
            IQueryable<Data.EF.bvc_ProductTypeXProductProperty> items = repository.Find()
                                                                          .Where(y => y.StoreId == storeId)
                                                                          .Where(y => y.ProductTypeBvin == productTypeBvin)
                                                                          .OrderBy(y => y.SortOrder);
            if (items != null)
            {
                result = ListPoco(items);
            }

            return result;
        }
        public int CountByProductType(string productTypeBvin)
        {
            int result = 0;
            long storeId = context.CurrentStore.Id;
            int temp = repository.Find().Where(y => y.StoreId == storeId)
                                        .Where(y => y.ProductTypeBvin == productTypeBvin).Count();
            if (temp >= 0)
            {
                result = temp;
            }
            return result;
        }
    }
}
