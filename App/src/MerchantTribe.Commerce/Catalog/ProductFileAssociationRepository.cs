using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MerchantTribe.Web.Data;
using MerchantTribe.Web.Logging;

namespace MerchantTribe.Commerce.Catalog
{
    public class ProductFileAssociationRepository: ConvertingRepositoryBase<Data.EF.bvc_ProductFileXProduct, ProductFileAssociation>
    {
        private RequestContext context = null;

        public static ProductFileAssociationRepository InstantiateForMemory(RequestContext c)
        {
            return new ProductFileAssociationRepository(c, new MemoryStrategy<Data.EF.bvc_ProductFileXProduct>(PrimaryKeyType.Bvin), new NullLogger());
        }
        public static ProductFileAssociationRepository InstantiateForDatabase(RequestContext c)
        {
            ProductFileAssociationRepository result = null;
            result = new ProductFileAssociationRepository(c,
                new EntityFrameworkRepository<Data.EF.bvc_ProductFileXProduct>(
                    new Data.EF.EntityFrameworkDevConnectionString(c.ConnectionStringForEntityFramework)),
                    new SupressLogger());
            return result;
        }
        public ProductFileAssociationRepository(RequestContext c, IRepositoryStrategy<Data.EF.bvc_ProductFileXProduct> r, ILogger log)
        {
            context = c;
            repository = r;
            this.logger = log;
            repository.Logger = this.logger;
        }

        protected override void CopyDataToModel(Data.EF.bvc_ProductFileXProduct data, ProductFileAssociation model)
        {
            model.Id = data.Id;
            model.StoreId = data.StoreId;
            model.AvailableMinutes = data.AvailableMinutes;
            model.FileId = data.ProductFileId;
            model.LastUpdatedUtc = data.LastUpdated;
            model.MaxDownloads = data.MaxDownloads;
            model.ProductId = data.ProductId;            
        }
        protected override void CopyModelToData(Data.EF.bvc_ProductFileXProduct data, ProductFileAssociation model)
        {
            data.Id = model.Id;
            data.StoreId = model.StoreId;
            data.AvailableMinutes = model.AvailableMinutes;
            data.ProductFileId = model.FileId;
            data.LastUpdated = model.LastUpdatedUtc;
            data.MaxDownloads = model.MaxDownloads;
            data.ProductId = model.ProductId;            
        }

        public ProductFileAssociation Find(long id)
        {
            ProductFileAssociation result = FindForAllStores(id);
            if (result != null)
            {
                if (result.StoreId == context.CurrentStore.Id)
                {
                    return result;
                }
            }
            return null;
        }
        public ProductFileAssociation FindForAllStores(long id)
        {
            Data.EF.bvc_ProductFileXProduct data = repository.FindByPrimaryKey(new PrimaryKey(id));
            if (data == null) return null;

            ProductFileAssociation result = new ProductFileAssociation();
            CopyDataToModel(data, result);
            return result;
        }
        public override bool Create(ProductFileAssociation item)
        {
            item.LastUpdatedUtc = DateTime.UtcNow;            
            item.StoreId = context.CurrentStore.Id;
 	        return base.Create(item);
        }
        public bool Update(ProductFileAssociation c)
        {
            if (c.StoreId != context.CurrentStore.Id)
            {
                return false;
            }
            c.LastUpdatedUtc = DateTime.UtcNow;
            return this.Update(c, new PrimaryKey(c.Id));            
        }
        public bool Delete(long id)
        {
            long storeId = context.CurrentStore.Id;
            ProductFileAssociation img = Find(id);
            if (img == null) return false;
            return Delete(new PrimaryKey(id));            
        }
        public bool DeleteForProductId(string productId)
        {
            List<ProductFileAssociation> toDelete = FindByProductId(productId);
            foreach (ProductFileAssociation item in toDelete)
            {
                Delete(item.Id);
            }
            return true;
        }
        public bool DeleteForFileId(string fileId)
        {
            List<ProductFileAssociation> toDelete = FindByFileId(fileId);
            foreach (ProductFileAssociation item in toDelete)
            {
                Delete(item.Id);
            }
            return true;
        }
        public List<ProductFileAssociation> FindByProductId(string productId)
        {
            return FindByProductIdPaged(productId, 1, int.MaxValue);
        }
        public List<ProductFileAssociation> FindByProductIdPaged(string productId, int pageNumber, int pageSize)
        {
            List<ProductFileAssociation> result = new List<ProductFileAssociation>();

            if (pageNumber < 1) pageNumber = 1;

            int take = pageSize;
            int skip = (pageNumber - 1) * pageSize;
            long storeId = context.CurrentStore.Id;

            IQueryable<Data.EF.bvc_ProductFileXProduct> items = repository.Find().Where(y => y.ProductId == productId)
                                                                          .Where(y => y.StoreId == storeId)
                                                                          .OrderBy(y => y.Id)
                                                                          .Skip(skip)
                                                                          .Take(take);
            if (items != null)
            {
                result = ListPoco(items);
            }

            return result;
        }
        public List<ProductFileAssociation> FindByFileId(string fileId)
        {
            return FindByFileIdPaged(fileId, 1, int.MaxValue);
        }
        public List<ProductFileAssociation> FindByFileIdPaged(string fileId, int pageNumber, int pageSize)
        {
            List<ProductFileAssociation> result = new List<ProductFileAssociation>();

            if (pageNumber < 1) pageNumber = 1;

            int take = pageSize;
            int skip = (pageNumber - 1) * pageSize;
            long storeId = context.CurrentStore.Id;

            IQueryable<Data.EF.bvc_ProductFileXProduct> items = repository.Find().Where(y => y.ProductFileId == fileId)
                                                                          .Where(y => y.StoreId == storeId)
                                                                          .OrderBy(y => y.Id)
                                                                          .Skip(skip)
                                                                          .Take(take);
            if (items != null)
            {
                result = ListPoco(items);
            }

            return result;
        }
        public int CountByFileId(string fileId)
        {
            int result = 0;
            long storeId = context.CurrentStore.Id;

            int temp = repository.Find().Where(y => y.ProductFileId == fileId)
                                        .Where(y => y.StoreId == storeId).Count();
            if (temp >= 0)
            {
                result = temp;
            }
            return result;
        }
        public ProductFileAssociation FindByFileIdAndProductId(string fileId, string productId)
        {
            Data.EF.bvc_ProductFileXProduct data = repository.Find().Where(y => y.ProductFileId == fileId && y.ProductId == productId).FirstOrDefault();
            if (data == null) return null;
            ProductFileAssociation result = new ProductFileAssociation();
            CopyDataToModel(data, result);
            return result;
        }

    }
}
