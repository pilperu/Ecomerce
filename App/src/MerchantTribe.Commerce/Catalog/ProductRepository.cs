using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MerchantTribe.Web.Data;
using MerchantTribe.Web.Logging;
using StackExchange.Profiling;
using StackExchange.Profiling.Data;
using StackExchange.Profiling.MVCHelpers;

namespace MerchantTribe.Commerce.Catalog
{
    public class ProductRepository : ConvertingRepositoryBase<Data.EF.bvc_Product, Product>
    {
        private RequestContext context = null;

        private OptionRepository optionRespository = null;
        private VariantRepository variantRepository = null;
        private ProductImageRepository imageRepository = null;
        private ProductReviewRepository reviewRepository = null;
        private ProductOptionAssociationRepository productsXOptions = null;

        public static ProductRepository InstantiateForMemory(RequestContext c)
        {
            return new ProductRepository(c, new MemoryStrategy<Data.EF.bvc_Product>(PrimaryKeyType.Bvin),
                                           new MemoryStrategy<Data.EF.bvc_ProductOptions>(PrimaryKeyType.Bvin),
                                           new MemoryStrategy<Data.EF.bvc_ProductOptionsItems>(PrimaryKeyType.Bvin),
                                           new MemoryStrategy<Data.EF.bvc_ProductXOption>(PrimaryKeyType.Long),
                                           new MemoryStrategy<Data.EF.bvc_Variants>(PrimaryKeyType.Bvin),
                                           new MemoryStrategy<Data.EF.bvc_ProductImage>(PrimaryKeyType.Bvin),
                                           new MemoryStrategy<Data.EF.bvc_ProductReview>(PrimaryKeyType.Bvin),
                                           new TextLogger());
        }
        public static ProductRepository InstantiateForDatabase(RequestContext c)
        {
            ProductRepository result = null;
            result = new ProductRepository(c,
                new EntityFrameworkRepository<Data.EF.bvc_Product>(
                    new Data.EF.EntityFrameworkDevConnectionString(c.ConnectionStringForEntityFramework)),
                new EntityFrameworkRepository<Data.EF.bvc_ProductOptions>(
                        new Data.EF.EntityFrameworkDevConnectionString(c.ConnectionStringForEntityFramework)),
                new EntityFrameworkRepository<Data.EF.bvc_ProductOptionsItems>(
                        new Data.EF.EntityFrameworkDevConnectionString(c.ConnectionStringForEntityFramework)),
                new EntityFrameworkRepository<Data.EF.bvc_ProductXOption>(
                        new Data.EF.EntityFrameworkDevConnectionString(c.ConnectionStringForEntityFramework)),
                new EntityFrameworkRepository<Data.EF.bvc_Variants>(
                        new Data.EF.EntityFrameworkDevConnectionString(c.ConnectionStringForEntityFramework)),
                new EntityFrameworkRepository<Data.EF.bvc_ProductImage>(
                        new Data.EF.EntityFrameworkDevConnectionString(c.ConnectionStringForEntityFramework)),
                new EntityFrameworkRepository<Data.EF.bvc_ProductReview>(
                        new Data.EF.EntityFrameworkDevConnectionString(c.ConnectionStringForEntityFramework)),
                    new SupressLogger()
                    );
            return result;
        }

        private ProductRepository(RequestContext c, IRepositoryStrategy<Data.EF.bvc_Product> r,
                                 IRepositoryStrategy<Data.EF.bvc_ProductOptions> opts,
                                 IRepositoryStrategy<Data.EF.bvc_ProductOptionsItems> optitems,
                                 IRepositoryStrategy<Data.EF.bvc_ProductXOption> optionCrosses,
                                 IRepositoryStrategy<Data.EF.bvc_Variants> vars,
                                 IRepositoryStrategy<Data.EF.bvc_ProductImage> images,
                                 IRepositoryStrategy<Data.EF.bvc_ProductReview> reviews,
                                 ILogger log)
        {
            context = c;
            repository = r;
            this.logger = log;
            repository.Logger = this.logger;
            this.optionRespository = new OptionRepository(c, opts, optitems, optionCrosses, this.logger);
            this.variantRepository = new VariantRepository(c, vars, this.logger);
            this.imageRepository = new ProductImageRepository(c, images, this.logger);
            this.reviewRepository = new ProductReviewRepository(c, reviews, this.logger);
        }

        protected override void CopyDataToModel(Data.EF.bvc_Product data, Product model)
        {
            model.AllowReviews = data.AllowReviews;
            model.Bvin = data.bvin;
            model.CreationDateUtc = data.CreationDate;
            model.CustomPropertiesFromXml(data.CustomProperties);
            model.Featured = data.Featured;
            model.GiftWrapAllowed = data.GiftWrapAllowed == 1 ? true : false;
            model.GiftWrapPrice = data.GiftWrapPrice;
            model.ImageFileMedium = data.ImageFileMedium;
            model.ImageFileMediumAlternateText = data.MediumImageAlternateText;
            model.ImageFileSmall = data.ImageFileSmall;
            model.ImageFileSmallAlternateText = data.SmallImageAlternateText;
            model.InventoryMode = (ProductInventoryMode)data.OutOfStockMode;
            model.IsAvailableForSale = data.IsAvailableForSale;
            model.Keywords = data.Keywords;
            model.LastUpdated = data.LastUpdated;
            model.ListPrice = data.ListPrice;
            model.LongDescription = data.LongDescription;
            model.ManufacturerId = data.ManufacturerID;
            model.MetaDescription = data.MetaDescription;
            model.MetaKeywords = data.MetaKeywords;
            model.MetaTitle = data.MetaTitle;
            model.MinimumQty = data.MinimumQty;
            model.PostContentColumnId = data.PostContentColumnId;
            model.PreContentColumnId = data.PreContentColumnId;
            model.PreTransformLongDescription = data.PreTransformLongDescription;
            model.ProductName = data.ProductName;
            model.ProductTypeId = data.ProductTypeId;
            model.ShippingDetails.ExtraShipFee = data.ExtraShipFee;
            model.ShippingDetails.Height = data.ShippingHeight;
            model.ShippingDetails.IsNonShipping = data.NonShipping == 1 ? true : false;
            model.ShippingDetails.Length = data.ShippingLength;
            model.ShippingDetails.Weight = data.ShippingWeight;
            model.ShippingDetails.Width = data.ShippingWidth;
            model.ShippingDetails.ShipSeparately = data.ShipSeparately == 1 ? true : false;
            model.ShippingMode = (Shipping.ShippingMode)data.ShippingMode;
            model.ShortDescription = data.ShortDescription;
            model.SiteCost = data.SiteCost;
            model.SitePrice = data.SitePrice;
            model.SitePriceOverrideText = data.SitePriceOverrideText;
            model.Sku = data.SKU;
            model.Status = (ProductStatus)data.Status;
            model.StoreId = data.StoreId;
            model.TabsFromXml(data.DescriptionTabs);
            model.TaxExempt = data.TaxExempt == 1 ? true : false;
            string taxString = data.TaxClass;
            long tempTax = -1; if (long.TryParse(taxString, out tempTax)) model.TaxSchedule = tempTax;
            model.TemplateName = data.TemplateName;
            model.UrlSlug = data.RewriteUrl;
            model.VendorId = data.VendorID;
        }
        protected override void CopyModelToData(Data.EF.bvc_Product data, Product model)
        {
            data.AllowReviews = model.AllowReviews;
            data.bvin = model.Bvin;
            data.CreationDate = model.CreationDateUtc;
            data.CustomProperties = model.CustomPropertiesToXml();
            data.Featured = model.Featured;
            data.GiftWrapAllowed = model.GiftWrapAllowed ? 1 : 0;
            data.GiftWrapPrice = model.GiftWrapPrice;
            data.ImageFileMedium = model.ImageFileMedium;
            data.MediumImageAlternateText = model.ImageFileMediumAlternateText;
            data.ImageFileSmall = model.ImageFileSmall;
            data.SmallImageAlternateText = model.ImageFileSmallAlternateText;
            data.OutOfStockMode = (int)model.InventoryMode;
            data.IsAvailableForSale = model.IsAvailableForSale;
            data.Keywords = model.Keywords;
            data.LastUpdated = model.LastUpdated;
            data.ListPrice = model.ListPrice;
            data.LongDescription = model.LongDescription;
            data.ManufacturerID = model.ManufacturerId;
            data.MetaDescription = model.MetaDescription;
            data.MetaKeywords = model.MetaKeywords;
            data.MetaTitle = model.MetaTitle;
            data.MinimumQty = model.MinimumQty;
            data.PostContentColumnId = model.PostContentColumnId;
            data.PreContentColumnId = model.PreContentColumnId;
            data.PreTransformLongDescription = model.PreTransformLongDescription;
            data.ProductName = model.ProductName;
            data.ProductTypeId = model.ProductTypeId;
            data.ExtraShipFee = model.ShippingDetails.ExtraShipFee;
            data.ShippingHeight = model.ShippingDetails.Height;
            data.NonShipping = model.ShippingDetails.IsNonShipping ? 1 : 0;
            data.ShippingLength = model.ShippingDetails.Length;
            data.ShippingWeight = model.ShippingDetails.Weight;
            data.ShippingWidth = model.ShippingDetails.Width;
            data.ShipSeparately = model.ShippingDetails.ShipSeparately ? 1 : 0;
            data.ShippingMode = (int)model.ShippingMode;
            data.ShortDescription = model.ShortDescription;
            data.SiteCost = model.SiteCost;
            data.SitePrice = model.SitePrice;
            data.SitePriceOverrideText = model.SitePriceOverrideText;
            data.SKU = model.Sku;
            data.Status = (int)model.Status;
            data.StoreId = model.StoreId;
            data.DescriptionTabs = model.TabsToXml();
            data.TaxExempt = model.TaxExempt ? 1 : 0;
            data.TaxClass = model.TaxSchedule.ToString();
            data.TemplateName = model.TemplateName;
            data.RewriteUrl = model.UrlSlug;
            data.VendorID = model.VendorId;
        }

        protected override void DeleteAllSubItems(Product model)
        {
            variantRepository.DeleteForProductId(model.Bvin);
            optionRespository.DeleteForProductId(model.Bvin);
            imageRepository.DeleteForProductId(model.Bvin);
            reviewRepository.DeleteForProductId(model.Bvin);
        }
        protected override void GetSubItems(Product model)
        {
            model.Variants.Clear();
            model.Variants.AddRange(variantRepository.FindByProductId(model.Bvin));
            model.Options.Clear();
            model.Options.AddRange(optionRespository.FindByProductId(model.Bvin));
            model.Images = imageRepository.FindByProductId(model.Bvin);
            model.Reviews = reviewRepository.FindByProductId(model.Bvin);
        }
        protected override void MergeSubItems(Product model)
        {
            variantRepository.MergeList(model.Bvin, model.Variants);
            optionRespository.MergeList(model.Bvin, model.Options);
            imageRepository.MergeList(model.Bvin, model.Images);
            reviewRepository.MergeList(model.Bvin, model.Reviews);
        }

        public Product FindWithCache(string bvin)
        {
            Product result = CacheManager.GetProduct(bvin, context.CurrentStore.Id);
            if (result != null) return result;

            result = FindForAllStores(bvin);
            if (result != null)
            {
                if (result.StoreId == context.CurrentStore.Id)
                {
                    CacheManager.AddProduct(result);
                    return result;
                }
            }

            return null;
        }
        public Product Find(string bvin)
        {
            Product result = FindForAllStores(bvin);
            if (result != null)
            {
                if (result.StoreId == context.CurrentStore.Id)
                {
                    return result;
                }
            }
            return null;
        }
        public Product FindForAllStores(string bvin)
        {
            return this.Find(new PrimaryKey(bvin));
        }
        public Product FindBySlug(string urlSlug)
        {
            return FindBySlugForStore(urlSlug, context.CurrentStore.Id);
        }
        public Product FindBySlugForStore(string urlSlug, long storeId)
        {
            IQueryable<Data.EF.bvc_Product> data = repository.Find().Where(y => y.RewriteUrl == urlSlug).Where(y => y.StoreId == storeId).OrderBy(y => y.Id);
            if (data.Count() > 0)
            {
                var result = SinglePoco(data);
                if (result != null) CacheManager.AddProduct(result);
                return result;
            }
            return null;
        }
        public Product FindBySku(string sku)
        {
            return FindBySkuForStore(sku, context.CurrentStore.Id);
        }
        public Product FindBySkuForStore(string sku, long storeId)
        {
            IQueryable<Data.EF.bvc_Product> data = repository.Find().Where(y => y.SKU == sku).Where(y => y.StoreId == storeId).OrderBy(y => y.Id);
            if (data.Count() > 0)
            {
                var result = SinglePoco(data);
                if (result != null) CacheManager.AddProduct(result);
                return result;
            }
            return null;
        }

        public override bool Create(Product item)
        {
            item.LastUpdated = DateTime.UtcNow;
            if (item.Bvin == string.Empty)
            {
                item.Bvin = System.Guid.NewGuid().ToString();
            }
            item.StoreId = context.CurrentStore.Id;

            return base.Create(item);
        }
        public bool Update(Product c)
        {
            if (c.StoreId != context.CurrentStore.Id)
            {
                return false;
            }
            CacheManager.ClearProduct(c.Bvin, c.StoreId);
            c.LastUpdated = DateTime.UtcNow;
            return this.Update(c, new PrimaryKey(c.Bvin));
        }
        public bool Delete(string bvin)
        {
            var existing = Find(bvin);
            if (existing == null) return true;
            CacheManager.ClearProduct(bvin, existing.StoreId);
            return Delete(new PrimaryKey(bvin));
        }

        //public List<Product> FindAll()
        //{
        //    long storeId = context.CurrentStore.Id;
        //    IQueryable<Data.EF.bvc_Product> result = repository.Find().Where(y => y.StoreId == storeId).OrderBy(y => y.ProductName);
        //    return ListPoco(result);
        //}
        //public List<Product> FindAllForAllStores()
        //{
        //    return this.FindAllPagedForAllStores(1, int.MaxValue);
        //}        
        public override List<Product> FindAllPaged(int pageNumber, int pageSize)
        {
            List<Product> result = new List<Product>();

            if (pageNumber < 1) pageNumber = 1;

            int take = pageSize;
            int skip = (pageNumber - 1) * pageSize;
            long storeId = context.CurrentStore.Id;

            IQueryable<Data.EF.bvc_Product> items = repository.Find().Where(y => y.StoreId == storeId).OrderBy(y => y.ProductName).Skip(skip).Take(take);
            if (items != null)
            {
                result = ListPoco(items);
            }

            return result;
        }
        public int FindAllCount()
        {
            long storeId = context.CurrentStore.Id;
            return FindAllCount(storeId);
        }
        public int FindAllCount(long storeId)
        {
            int result = 0;
            result = repository.Find().Where(y => y.StoreId == storeId).Count();
            return result;
        }
        public List<Product> FindAllPagedForAllStores(int pageNumber, int pageSize)
        {
            List<Product> result = new List<Product>();

            if (pageNumber < 1) pageNumber = 1;

            int take = pageSize;
            int skip = (pageNumber - 1) * pageSize;

            IQueryable<Data.EF.bvc_Product> items = repository.Find().OrderBy(y => y.ProductName).Skip(skip).Take(take);
            if (items != null)
            {
                result = ListPoco(items);
            }

            return result;
        }
        public int FindAllForAllStoresCount()
        {
            int result = repository.CountOfAll();
            return result;
        }
        public int FindCountByProductType(string productTypeId)
        {
            long storeId = context.CurrentStore.Id;
            int result = 0;
            result = repository.Find().Where(y => y.StoreId == storeId)
                                      .Where(y => y.ProductTypeId == productTypeId)
                                      .Count();
            return result;
        }

        public List<Product> FindByCategoryId(string categoryBvin, CategorySortOrder sort,
                                            bool showUnavailable,
                                            int pageNumber, int pageSize, ref int totalResults)
        {
            var profiler = MiniProfiler.Current;
            
            ProductSearchCriteria criteria = new ProductSearchCriteria();
            criteria.CategorySort = sort;
            criteria.CategoryId = categoryBvin;
            criteria.DisplayInactiveProducts = showUnavailable;
            if (showUnavailable)
            {
                criteria.InventoryStatus = ProductInventoryStatus.NotSet;
            }
            else
            {
                criteria.InventoryStatus = ProductInventoryStatus.Available;
            }
            int totalFromSearch = 0;
            List<Product> results = FindByCriteria(criteria, pageNumber, pageSize, ref totalFromSearch);
            totalResults = totalFromSearch;
            return results;

        }
        public List<Product> FindByCriteria(ProductSearchCriteria criteria)
        {
            int temp = -1;
            return FindByCriteria(criteria, 1, int.MaxValue, ref temp);
        }
        public List<Product> FindByCriteria(ProductSearchCriteria criteria, int pageNumber, int pageSize, ref int totalCount)
        {

            var profiler = MiniProfiler.Current;
            using (profiler.Step("Find By Criteria"))
            {
                List<Product> result = new List<Product>();

                if (pageNumber < 1) pageNumber = 1;

                int take = pageSize;
                int skip = (pageNumber - 1) * pageSize;
                long storeId = context.CurrentStore.Id;

                IQueryable<Data.EF.bvc_Product> items = repository.Find()
                                                .Where(y => y.StoreId == storeId);

                using (profiler.Step("Build Expression Tree"))
                {
                    // Display Inactive
                    if (criteria.DisplayInactiveProducts == false)
                    {
                        items = items.Where(y => y.Status == 1);
                    }
                    // Status
                    if (criteria.Status != ProductStatus.NotSet)
                    {
                        items = items.Where(y => y.Status == (int)criteria.Status);
                    }
                    // Inventory Status
                    if (criteria.InventoryStatus != ProductInventoryStatus.NotSet)
                    {
                        if (criteria.InventoryStatus == ProductInventoryStatus.NotAvailable)
                        {
                            items = items.Where(y => y.IsAvailableForSale == false);
                        }
                        else
                        {
                            items = items.Where(y => y.IsAvailableForSale == true);
                        }
                    }
                    // Manufacturer
                    if (criteria.ManufacturerId != string.Empty)
                    {
                        items = items.Where(y => y.ManufacturerID == criteria.ManufacturerId);
                    }
                    // Vendor
                    if (criteria.VendorId != string.Empty)
                    {
                        items = items.Where(y => y.VendorID == criteria.VendorId);
                    }
                    // Keywords
                    if (criteria.Keyword != string.Empty)
                    {
                        items = items.Where(y => y.SKU.Contains(criteria.Keyword) ||
                                              y.ProductName.Contains(criteria.Keyword) ||
                                              y.MetaDescription.Contains(criteria.Keyword) ||
                                              y.MetaKeywords.Contains(criteria.Keyword) ||
                                              y.ShortDescription.Contains(criteria.Keyword) ||
                                              y.LongDescription.Contains(criteria.Keyword) ||
                                              y.Keywords.Contains(criteria.Keyword));
                    }
                    if (criteria.CategoryId != string.Empty)
                    {
                        items = items.Where(y => y.bvc_ProductXCategory.Where(z => z.CategoryId == criteria.CategoryId).FirstOrDefault() != null);
                        // Sort
                        switch (criteria.CategorySort)
                        {
                            case CategorySortOrder.ProductName:
                                items = items.OrderBy(y => y.ProductName);
                                break;
                            case CategorySortOrder.ProductPriceAscending:
                                items = items.OrderBy(y => y.SitePrice);
                                break;
                            case CategorySortOrder.ProductPriceDescending:
                                items = items.OrderByDescending(y => y.SitePrice);
                                break;
                            default:
                                // TODO: This needs to respect merchant set sort order instead.
                                items = items.OrderBy(y => y.ProductName);
                                items = items.OrderBy(y => y.bvc_ProductXCategory.Where(z => z.CategoryId == criteria.CategoryId).Select(z => z.SortOrder).FirstOrDefault());
                                break;
                        }
                    }
                    else
                    {
                        items = items.OrderBy(y => y.ProductName);
                    }
                }

                
                // Get Total Count
                IQueryable<Data.EF.bvc_Product> itemsForCount = items;
                using (profiler.Step("Count Items"))
                {
                    totalCount = itemsForCount.Count();
                }

                // Get Paged            
                using (profiler.Step("Page Items"))
                {
                    items = items.Skip(skip).Take(take);
                }
                if (items != null)
                {
                    using (profiler.Step("List Poco Products"))
                    {
                        result = ListPoco(items);
                    }
                }

                return result;
            }
        }

        public List<string> FindFeaturedProductBvins(int pageNumber, int pageSize)
        {

            List<string> result = new List<string>();

            if (pageNumber < 1) pageNumber = 1;

            int take = pageSize;
            int skip = (pageNumber - 1) * pageSize;
            long storeId = context.CurrentStore.Id;

            var bvins = repository.Find().Where(y => y.StoreId == storeId)
                             .Where(y => y.Featured == true)
                             .OrderByDescending(y => y.LastUpdated)
                             .Select(y => y.bvin)
                             .Skip(skip).Take(take).ToList();

            if (bvins != null) result.AddRange(bvins);

            return result;
        }

        public List<Product> FindFeatured(int pageNumber, int pageSize)
        {
            List<string> bvins = FindFeaturedProductBvins(pageNumber, pageSize);
            return FindMany(bvins);
        }

        public List<Product> FindMany(List<string> bvins)
        {

            long storeId = context.CurrentStore.Id;

            List<Product> result = new List<Product>();

            foreach (string bvin in bvins)
            {
                // Hopefully they're cached
                var p = FindWithCache(bvin);
                if (p != null) result.Add(p);
            }

            return result;

        }
    }
}
