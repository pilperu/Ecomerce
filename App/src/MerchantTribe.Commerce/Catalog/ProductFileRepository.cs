using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MerchantTribe.Web.Data;
using MerchantTribe.Web.Logging;


namespace MerchantTribe.Commerce.Catalog
{
    public class ProductFileRepository : ConvertingRepositoryBase<Data.EF.bvc_ProductFile, ProductFile>
    {
        private RequestContext context = null;
        private ProductFileAssociationRepository crosses = null;

        public static ProductFileRepository InstantiateForMemory(RequestContext c)
        {
            return new ProductFileRepository(c, new MemoryStrategy<Data.EF.bvc_ProductFile>(PrimaryKeyType.Bvin), 
                                            new MemoryStrategy<Data.EF.bvc_ProductFileXProduct>(PrimaryKeyType.Long),
                                            new NullLogger());
        }
        public static ProductFileRepository InstantiateForDatabase(RequestContext c)
        {
            ProductFileRepository result = null;
            result = new ProductFileRepository(c,
                new EntityFrameworkRepository<Data.EF.bvc_ProductFile>(
                    new Data.EF.EntityFrameworkDevConnectionString(c.ConnectionStringForEntityFramework)),
                new EntityFrameworkRepository<Data.EF.bvc_ProductFileXProduct>(
                    new Data.EF.EntityFrameworkDevConnectionString(c.ConnectionStringForEntityFramework)),
                    new SupressLogger());
            return result;
        }
        public ProductFileRepository(RequestContext c, IRepositoryStrategy<Data.EF.bvc_ProductFile> r, 
                                    IRepositoryStrategy<Data.EF.bvc_ProductFileXProduct> x, ILogger log)
        {
            context = c;
            repository = r;
            this.logger = log;
            this.crosses = new ProductFileAssociationRepository(c, x, this.logger);
            repository.Logger = this.logger;
        }

        protected override void CopyDataToModel(Data.EF.bvc_ProductFile data, ProductFile model)
        {
            model.Bvin = data.bvin;
            model.FileName = data.FileName;
            model.LastUpdated = data.LastUpdated;
            model.ShortDescription = data.ShortDescription;
            model.StoreId = data.StoreId;            
        }
        protected override void CopyModelToData(Data.EF.bvc_ProductFile data, ProductFile model)
        {
            data.bvin = model.Bvin;
            data.FileName = model.FileName;
            data.LastUpdated = model.LastUpdated;
            data.ShortDescription = model.ShortDescription;
            data.StoreId = model.StoreId;
        }

        public ProductFile Find(string bvin)
        {
            ProductFile result = FindForAllStores(bvin);
            if (result != null)
            {
                if (result.StoreId == context.CurrentStore.Id)
                {
                    return result;
                }
            }
            return null;
        }
        public ProductFile FindForAllStores(string bvin)
        {
            Data.EF.bvc_ProductFile data = repository.FindByPrimaryKey(new PrimaryKey(bvin));
            if (data == null) return null;

            ProductFile result = new ProductFile();
            CopyDataToModel(data, result);
            return result;
        }
        public override bool Create(ProductFile item)
        {
            item.LastUpdated = DateTime.UtcNow;            
            if (item.Bvin == string.Empty)
            {
                item.Bvin = System.Guid.NewGuid().ToString();
            }
            item.StoreId = context.CurrentStore.Id;
 	        return base.Create(item);
        }
        public bool Update(ProductFile c)
        {
            if (c.StoreId != context.CurrentStore.Id)
            {
                return false;
            }
            c.LastUpdated = DateTime.UtcNow;
            return this.Update(c, new PrimaryKey(c.Bvin));            
        }
        public bool Delete(string bvin)
        {
            long storeId = context.CurrentStore.Id;
            ProductFile item = Find(bvin);
            if (item == null) return false;
            
            // remove from products
            crosses.DeleteForFileId(item.Bvin);

            string diskFileName = item.Bvin + "_" + item.FileName + ".config";
            Storage.DiskStorage.FileVaultRemove(item.StoreId, diskFileName);

            return Delete(new PrimaryKey(bvin));            
        }
        public bool DeleteForProductId(string productId)
        {
            List<ProductFile> toDelete = FindByProductId(productId);
            foreach (ProductFile item in toDelete)
            {
                Delete(item.Bvin);
            }
            return true;
        }
        public List<ProductFile> FindByProductId(string productId)
        {           
            List<ProductFile> result = new List<ProductFile>();

            long storeId = context.CurrentStore.Id;

            List<ProductFileAssociation> linkedFiles = this.crosses.FindByProductId(productId);
            foreach (ProductFileAssociation x in linkedFiles)
            {
                ProductFile f = Find(x.FileId);
                if (f != null)
                {
                    f.AvailableMinutes = x.AvailableMinutes;
                    f.MaxDownloads = x.MaxDownloads;
                    result.Add(f);
                }
            }
            
            return result;
        }
        public List<ProductFile> FindByFileNameAndDescription(string fileName, string description)
        {
            List<ProductFile> result = new List<ProductFile>();
            long storeId = context.CurrentStore.Id;

            IQueryable<Data.EF.bvc_ProductFile> items = repository.Find().Where(y => y.StoreId == storeId)
                                                                          .Where(y => y.FileName == fileName)
                                                                          .Where(y => y.ShortDescription == description);
            if (items != null)
            {
                result = ListPoco(items);
            }

            return result;
        }
        public ProductFile FindByBvinAndProductBvin(string bvin, string productBvin)
        {                     
            ProductFileAssociation x = this.crosses.FindByFileIdAndProductId(bvin, productBvin);
            if (x != null)
            {
                ProductFile f = Find(x.FileId);
                f.AvailableMinutes = x.AvailableMinutes;
                f.MaxDownloads = x.MaxDownloads;
                return f;
            }
            return null;
        }
        public bool FileAlreadyExists(string fileBvin)
        {
            ProductFile f = Find(fileBvin);
            if (f != null) return true;
            return false;
        }
        public int CountOfProductsUsingFile(string fileId)
        {
            int result = 0;
            result = this.crosses.CountByFileId(fileId);
            return result;
        }
        public List<string> FindProductIdsForFile(string fileId)
        {
            List<string> ids = new List<string>();
            List<ProductFileAssociation> data = this.crosses.FindByFileId(fileId);
            foreach (ProductFileAssociation x in data)
            {
                ids.Add(x.ProductId);
            }
            return ids;
        }
        public bool RemoveAssociatedProduct(string fileBvin, string productBvin)
        {
            ProductFileAssociation x = this.crosses.FindByFileIdAndProductId(fileBvin, productBvin);
            if (x == null) return false;
            return this.crosses.Delete(x.Id);
        }
        public bool AddAssociatedProduct(string fileBvin, string productBvin, int availableMinutes, int maxDownloads)
        {
            long storeId = context.CurrentStore.Id;
            RemoveAssociatedProduct(fileBvin, productBvin);
            ProductFileAssociation x = new ProductFileAssociation();
            x.AvailableMinutes = availableMinutes;
            x.FileId = fileBvin;
            x.MaxDownloads = maxDownloads;
            x.ProductId = productBvin;
            x.StoreId = storeId;
            return this.crosses.Create(x);
        }

        public int FindAllCount()
        {
            long storeId = context.CurrentStore.Id;
            int result = repository.Find().Where(y => y.StoreId == storeId)
                                          .Count();
            return result;
        }
        public List<ProductFile> FindAll(int pageNumber, int pageSize)
        {
            List<ProductFile> result = new List<ProductFile>();
            long storeId = context.CurrentStore.Id;
            
            if (pageNumber < 1) pageNumber = 1;
            int take = pageSize;
            int skip = (pageNumber - 1) * pageSize;
            IQueryable<Data.EF.bvc_ProductFile> items = repository.Find().Where(y => y.StoreId == storeId)
                                                                          .OrderBy(y => y.FileName)
                                                                          .Skip(skip)
                                                                          .Take(take);
            if (items != null)
            {
                result = ListPoco(items);
            }

            return result;
        }
    }
}
