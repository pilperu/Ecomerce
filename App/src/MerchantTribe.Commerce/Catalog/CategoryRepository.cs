using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MerchantTribe.Web.Data;
using MerchantTribe.Web.Logging;

namespace MerchantTribe.Commerce.Catalog
{
    public class CategoryRepository : ConvertingRepositoryBase<Data.EF.bvc_Category, Category>
    {

        public static CategoryRepository InstantiateForMemory(RequestContext c)
        {
            return new CategoryRepository(c, new MemoryStrategy<Data.EF.bvc_Category>(PrimaryKeyType.Bvin),
                                           new MemoryStrategy<Data.EF.PageVersion>(PrimaryKeyType.Long),
                                           new TextLogger());
        }
        public static CategoryRepository InstantiateForDatabase(RequestContext c)
        {
            CategoryRepository result = null;
            result = new CategoryRepository(c, 
                new EntityFrameworkRepository<Data.EF.bvc_Category>(
                    new Data.EF.EntityFrameworkDevConnectionString(c.ConnectionStringForEntityFramework)),
                new EntityFrameworkRepository<Data.EF.PageVersion>(
                    new Data.EF.EntityFrameworkDevConnectionString(c.ConnectionStringForEntityFramework)),
                    new SupressLogger()
                    );
            return result;
        }

        private RequestContext context = null;
        private CategoryPageVersionRepository versionRepository = null;
       
        private CategoryRepository(RequestContext c, IRepositoryStrategy<Data.EF.bvc_Category> r,
                                    IRepositoryStrategy<Data.EF.PageVersion> subr, ILogger log)
        {
            context = c;
            repository = r;
            this.logger = log;
            repository.Logger = this.logger;
            versionRepository = new CategoryPageVersionRepository(subr, this.logger);                        
        }

        protected override void CopyDataToModel(Data.EF.bvc_Category data, Category model)
        {
            model.BannerImageUrl = data.BannerImageURL;
            model.Bvin = data.bvin;
            model.Criteria = data.Criteria;
            model.CustomerChangeableSortOrder = data.CustomerChangeableSortOrder;
            model.CustomPageId = data.CustomPageId;
            model.CustomPageLayout = (CustomPageLayoutType)data.CustomPageLayout;
            model.CustomPageOpenInNewWindow = data.CustomPageNewWindow == 1 ? true : false;
            model.CustomPageUrl = data.CustomPageURL;
            model.Description = data.Description;
            model.DisplaySortOrder = (CategorySortOrder)data.DisplaySortOrder;
            model.Hidden = data.Hidden == 1 ? true : false;
            model.ImageUrl = data.ImageURL;
            model.Keywords = data.Keywords;
            model.LastUpdatedUtc = data.LastUpdated;
            model.LatestProductCount = data.LatestProductCount;
            model.MetaDescription = data.MetaDescription;
            model.MetaKeywords = data.MetaKeywords;
            model.MetaTitle = data.MetaTitle;
            model.Name = data.Name;
            model.ParentId = data.ParentID;
            model.PostContentColumnId = data.PostContentColumnId;
            model.PreContentColumnId = data.PreContentColumnId;
            model.PreTransformDescription = data.PreTransformDescription;
            model.RewriteUrl = data.RewriteUrl;
            model.ShowInTopMenu = data.ShowInTopMenu == 1 ? true : false;
            model.ShowTitle = data.ShowTitle == 1 ? true : false;
            model.SortOrder = data.SortOrder;
            model.SourceType = (CategorySourceType)data.SourceType;
            model.StoreId = data.StoreId;
            model.TemplateName = data.TemplateName;
        }
        protected override void CopyModelToData(Data.EF.bvc_Category data, Category model)
        {
            data.BannerImageURL = model.BannerImageUrl;
            data.bvin = model.Bvin;
            data.Criteria = model.Criteria;
            data.CustomerChangeableSortOrder = model.CustomerChangeableSortOrder;
            data.CustomPageId = model.CustomPageId;
            data.CustomPageLayout = (int)model.CustomPageLayout;
            data.CustomPageNewWindow = model.CustomPageOpenInNewWindow ? 1 : 0;
            data.CustomPageURL = model.CustomPageUrl;
            data.Description = model.Description;
            data.DisplaySortOrder = (int)model.DisplaySortOrder;
            data.Hidden = model.Hidden ? 1 : 0;
            data.ImageURL = model.ImageUrl;
            data.Keywords = model.Keywords;
            data.LastUpdated = model.LastUpdatedUtc;
            data.LatestProductCount = model.LatestProductCount;
            data.MetaDescription = model.MetaDescription;
            data.MetaKeywords = model.MetaKeywords;
            data.MetaTitle = model.MetaTitle;
            data.Name = model.Name;
            data.ParentID = model.ParentId;
            data.PostContentColumnId = model.PostContentColumnId;
            data.PreContentColumnId = model.PreContentColumnId;
            data.PreTransformDescription = model.PreTransformDescription;
            data.RewriteUrl = model.RewriteUrl;
            data.ShowInTopMenu = model.ShowInTopMenu ? 1 : 0;
            data.ShowTitle = model.ShowTitle ? 1 : 0;
            data.SortOrder = model.SortOrder;
            data.SourceType = (int)model.SourceType;
            data.StoreId = model.StoreId;
            data.TemplateName = model.TemplateName;
        }

        protected override void DeleteAllSubItems(Category model)
        {
            versionRepository.DeleteForPage(model.Bvin);
        }
        protected override void GetSubItems(Category model)
        {
            model.Versions = versionRepository.FindForPage(model.Bvin);
        }
        protected override void MergeSubItems(Category model)
        {
            versionRepository.MergeList(model.Bvin, model.Versions);
        }
    
        public Category Find(string bvin)
        {
            Category result = FindForAllStores(bvin);
            if (result != null)
            {
                if (result.StoreId == context.CurrentStore.Id)
                {
                    return result;
                }
            }
            return null;
        }
        public Category FindForAllStores(string bvin)
        {
            return this.Find(new PrimaryKey(bvin));            
        }

        public Category FindBySlug(string urlSlug)
        {
            return FindBySlugForStore(urlSlug, context.CurrentStore.Id);
        }
        public Category FindBySlugForStore(string urlSlug, long storeId)
        {
            IQueryable<Data.EF.bvc_Category> data = repository.Find().Where(y => y.RewriteUrl == urlSlug).Where(y => y.StoreId == storeId).OrderBy(y => y.SortOrder);

            if (data.Count() > 0) return SinglePoco(data);

            return null;
        }

        public override bool  Create(Category item)
        {
            item.LastUpdatedUtc = DateTime.UtcNow;            
            if (item.Bvin == string.Empty)
            {
                item.Bvin = System.Guid.NewGuid().ToString();
            }
            item.StoreId = context.CurrentStore.Id;

            item.SortOrder = FindMaxSort(item.ParentId) + 1;

            if (item.SourceType == CategorySourceType.FlexPage)
            {
                item.Versions.Add(new CategoryPageVersion() { AdminName = "First Version", PublishedStatus = Content.PublishStatus.Published });
            }

            // Make sure we have a rewrite URL if missing
            if (item.RewriteUrl == string.Empty)
            {
                item.RewriteUrl = MerchantTribe.Web.Text.Slugify(item.Name, true, true);
            }

            // Try ten times to append to URL if in use
            bool rewriteUrlInUse = MerchantTribe.Commerce.Utilities.UrlRewriter.IsCategorySlugInUse(item.RewriteUrl, item.Bvin, this);
            for (int i = 0; i < 10; i++)
            {
                if (rewriteUrlInUse)
                {
                    item.RewriteUrl = item.RewriteUrl + "-2";
                    rewriteUrlInUse = MerchantTribe.Commerce.Utilities.UrlRewriter.IsCategorySlugInUse(item.RewriteUrl, item.Bvin, this);
                    if (rewriteUrlInUse == false) break;
                }
            }


 	        return base.Create(item);
        }
        private int FindMaxSort(string parentId)
        {
            int maxSort = 0;
            long storeId = context.CurrentStore.Id;
            List<Category> result = new List<Category>();
            IQueryable<Data.EF.bvc_Category> items = repository.Find().Where(y => y.ParentID == parentId)
                                                                      .Where(y => y.StoreId == storeId).OrderByDescending(y => y.SortOrder)
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
        
        public bool Update(Category c)
        {
            if (c.StoreId != context.CurrentStore.Id)
            {
                return false;
            }

            c.LastUpdatedUtc = DateTime.UtcNow;
            return this.Update(c, new PrimaryKey(c.Bvin));            
        }

        public bool Delete(string bvin)
        {
            return Delete(new PrimaryKey(bvin));
        }


        protected virtual List<CategorySnapshot> ListPocoSnapshot(IQueryable<Data.EF.bvc_Category> items)
        {
            List<CategorySnapshot> result = new List<CategorySnapshot>();

            if (items != null)
            {
                foreach (Data.EF.bvc_Category item in items)
                {
                    CategorySnapshot temp = new CategorySnapshot();
                    temp.Bvin = item.bvin;
                    temp.CustomPageId = item.CustomPageId;
                    temp.CustomPageLayout = (CustomPageLayoutType)item.CustomPageLayout;
                    temp.CustomPageOpenInNewWindow = (item.CustomPageNewWindow == 1);
                    temp.CustomPageUrl = item.CustomPageURL;
                    temp.Hidden = (item.Hidden == 1);
                    temp.ImageUrl = item.ImageURL;
                    temp.LatestProductCount = item.LatestProductCount;
                    temp.Name = item.Name;
                    temp.ParentId = item.ParentID;
                    temp.RewriteUrl = item.RewriteUrl;
                    temp.ShowInTopMenu = (item.ShowInTopMenu == 1);
                    temp.SourceType = (CategorySourceType)item.SourceType;
                    temp.StoreId = item.StoreId;
                    temp.SortOrder = item.SortOrder;
                    temp.MetaTitle = item.MetaTitle;
                    result.Add(temp);
                }
            }

            return result;
        }
        public List<CategorySnapshot> FindAll()
        {
            long storeId = context.CurrentStore.Id;
            IQueryable<Data.EF.bvc_Category> result = repository.Find().Where(y => y.StoreId == storeId).OrderBy(y => y.SortOrder);
            return ListPocoSnapshot(result);
        }
        public List<CategorySnapshot> FindAllFlexPages()
        {
            long storeId = context.CurrentStore.Id;
            IQueryable<Data.EF.bvc_Category> result = repository.Find().Where(y => y.StoreId == storeId)
                    .Where(y => y.SourceType == (int)Catalog.CategorySourceType.FlexPage).OrderBy(y => y.SortOrder);
            return ListPocoSnapshot(result);
        }
        public List<CategorySnapshot> FindAllForAllStores()
        {
            return this.FindAllPagedForAllStores(1, int.MaxValue);
        }        
        public new List<CategorySnapshot> FindAllPaged(int pageNumber, int pageSize)
        {
            List<CategorySnapshot> result = new List<CategorySnapshot>();

            if (pageNumber < 1) pageNumber = 1;

            int take = pageSize;
            int skip = (pageNumber - 1) * pageSize;
            long storeId = context.CurrentStore.Id;

            IQueryable<Data.EF.bvc_Category> items = repository.Find().Where(y => y.StoreId == storeId).OrderBy(y => y.SortOrder).Skip(skip).Take(take);
            if (items != null)
            {
                result = ListPocoSnapshot(items);
            }

            return result;
        }
        public List<CategorySnapshot> FindAllPagedForAllStores(int pageNumber, int pageSize)
        {
            List<CategorySnapshot> result = new List<CategorySnapshot>();

            if (pageNumber < 1) pageNumber = 1;

            int take = pageSize;
            int skip = (pageNumber - 1) * pageSize;            

            IQueryable<Data.EF.bvc_Category> items = repository.Find().OrderBy(y => y.SortOrder).Skip(skip).Take(take);
            if (items != null)
            {
                result = ListPocoSnapshot(items);
            }

            return result;
        }
        public List<CategorySnapshot> FindChildren(string parentId)
        {
            int totalRowCount = 0;
            return FindChildren(parentId, 1, int.MaxValue, ref totalRowCount);
        }
        public List<CategorySnapshot> FindChildren(string parentId, int pageNumber, int pageSize, ref int totalRowCount)
        {
            long storeId = context.CurrentStore.Id;

             List<CategorySnapshot> result = new List<CategorySnapshot>();

            if (pageNumber < 1) pageNumber = 1;

            int take = pageSize;
            int skip = (pageNumber - 1) * pageSize;           

            IQueryable<Data.EF.bvc_Category> items = repository.Find().Where(y => y.ParentID == parentId)
                                                                      .Where(y => y.StoreId == storeId).OrderBy(y => y.SortOrder);
            totalRowCount = items.Count();            
            items = items.Skip(skip).Take(take);

            if (items != null)
            {
                result = ListPocoSnapshot(items);
            }

            
            return result;

        }
        public List<CategorySnapshot> FindVisibleChildren(string parentId)
        {
            int total = 0;
            return FindVisibleChildren(parentId, 1, int.MaxValue, ref total);
        }
        public List<CategorySnapshot> FindVisibleChildren(string parentId, int pageNumber, int pageSize, ref int totalRowCount)
        {
            long storeId = context.CurrentStore.Id;

            List<CategorySnapshot> result = new List<CategorySnapshot>();

            if (pageNumber < 1) pageNumber = 1;

            int take = pageSize;
            int skip = (pageNumber - 1) * pageSize;

            IQueryable<Data.EF.bvc_Category> items = repository.Find().Where(y => y.ParentID == parentId)
                                                                      .Where(y => y.StoreId == storeId)
                                                                      .Where(y => y.Hidden == 0)
                                                                      .OrderBy(y => y.SortOrder);
            totalRowCount = items.Count();
            items = items.Skip(skip).Take(take);

            if (items != null)
            {
                result = ListPocoSnapshot(items);
            }

            return result;
        }

        public List<CategorySnapshot> FindForMainMenu()
        {
            long storeId = context.CurrentStore.Id;

            List<CategorySnapshot> result = new List<CategorySnapshot>();

            IQueryable<Data.EF.bvc_Category> items = repository.Find().Where(y => y.ParentID == "0")
                                                                      .Where(y => y.StoreId == storeId)
                                                                      .Where(y => y.Hidden == 0)                                                                      
                                                                      .OrderBy(y => y.SortOrder);
                                                                      
            if (items != null)
            {
                result = ListPocoSnapshot(items);
            }

            return result;
        }

        //public CategoryPeerSet FindAllNeighbors(string bvin)
        //{
        //    CategoryPeerSet result = new CategoryPeerSet();
        //    long storeId = context.CurrentStore.Id;
        //    int totalRows = 0;

        //    Category main = Find(new PrimaryKey(bvin));
        //    if (main == null) return result;

        //    if (bvin == "0") // Requested Root Categories, so children only
        //    {
        //        IQueryable<Data.EF.bvc_Category> peers = repository.Find().Where(y => y.ParentID == "0")
        //                                                                  .Where(y => y.StoreId == storeId)
        //                                                                  .OrderBy(y => y.SortOrder);
        //        if (peers != null) result.Peers = ListPoco(peers);

        //        result.Children = FindChildren(bvin, 1, int.MaxValue, ref totalRows);
        //        return result;
        //    }
        //    else
        //    {
        //        Category parent = Find(new PrimaryKey(main.ParentId));
        //        if (parent != null)
        //        {
        //            result.Parents = FindChildren(parent.ParentId, 1, int.MaxValue, ref totalRows);
        //        }

        //        IQueryable<Data.EF.bvc_Category> peers = repository.Find().Where(y => y.ParentID == main.ParentId)
        //                                                                  .Where(y => y.StoreId == storeId)
        //                                                                  .OrderBy(y => y.SortOrder);                
        //        if (peers != null) result.Peers = ListPoco(peers);

        //        result.Children = FindChildren(bvin, 1, int.MaxValue, ref totalRows);
        //        return result;
        //    }
            
        //}

        public CategoryPeerSet FindVisibleNeighbors(string bvin)
        {
            CategoryPeerSet result = new CategoryPeerSet();
            long storeId = context.CurrentStore.Id;
            int totalRows = 0;

            Category main = Find(new PrimaryKey(bvin));
            if (main == null) return result;

            if (bvin == "0") // Requested Root Categories, so children only
            {
                IQueryable<Data.EF.bvc_Category> peers = repository.Find().Where(y => y.ParentID == "0")
                                                                          .Where(y => y.StoreId == storeId)
                                                                          .Where(y => y.Hidden == 0)
                                                                          .OrderBy(y => y.SortOrder);
                if (peers != null) result.Peers = ListPocoSnapshot(peers);

                result.Children = FindVisibleChildren(bvin, 1, int.MaxValue, ref totalRows);
                return result;
            }
            else
            {
                Category parent = Find(new PrimaryKey(main.ParentId));
                if (parent != null)
                {
                    result.Parents = FindVisibleChildren(parent.ParentId, 1, int.MaxValue, ref totalRows);
                }

                IQueryable<Data.EF.bvc_Category> peers = repository.Find().Where(y => y.ParentID == main.ParentId)
                                                                          .Where(y => y.StoreId == storeId)
                                                                          .Where(y => y.Hidden == 0)
                                                                          .OrderBy(y => y.SortOrder);
                if (peers != null) result.Peers = ListPocoSnapshot(peers);

                result.Children = FindVisibleChildren(bvin, 1, int.MaxValue, ref totalRows);
                return result;
            }
        }

        public List<Category> FindMany(List<string> bvins)
        {
            long storeId = context.CurrentStore.Id;

            List<Category> result = new List<Category>();
            
            IQueryable<Data.EF.bvc_Category> items = repository.Find().Where(y => bvins.Contains(y.bvin))
                                                                      .Where(y => y.StoreId == storeId).OrderBy(y => y.SortOrder);            
            if (items != null)
            {
                result = ListPoco(items);
            }

            return result;
        }

        public List<CategorySnapshot> FindManySnapshots(List<string> bvins)
        {
            long storeId = context.CurrentStore.Id;

            List<CategorySnapshot> result = new List<CategorySnapshot>();

            IQueryable<Data.EF.bvc_Category> items = repository.Find().Where(y => bvins.Contains(y.bvin))
                                                                      .Where(y => y.StoreId == storeId).OrderBy(y => y.SortOrder);
            if (items != null)
            {
                result = ListPocoSnapshot(items);
            }

            return result;
        }

    }
}
