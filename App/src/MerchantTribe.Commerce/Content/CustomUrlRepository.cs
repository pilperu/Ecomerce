using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MerchantTribe.Web.Data;
using MerchantTribe.Web.Logging;

namespace MerchantTribe.Commerce.Content
{
    public class CustomUrlRepository: ConvertingRepositoryBase<Data.EF.bvc_CustomUrl, CustomUrl>
    {
        private RequestContext context = null;

        public static CustomUrlRepository InstantiateForMemory(RequestContext c)
        {
            return new CustomUrlRepository(c, new MemoryStrategy<Data.EF.bvc_CustomUrl>(PrimaryKeyType.Bvin),                                           
                                           new TextLogger());
        }
        public static CustomUrlRepository InstantiateForDatabase(RequestContext c)
        {
            CustomUrlRepository result = null;
            result = new CustomUrlRepository(c,
                new EntityFrameworkRepository<Data.EF.bvc_CustomUrl>(
                    new Data.EF.EntityFrameworkDevConnectionString(c.ConnectionStringForEntityFramework)),
                    new SupressLogger()
                    );
            return result;
        }
        public CustomUrlRepository(RequestContext c, IRepositoryStrategy<Data.EF.bvc_CustomUrl> r,                                    
                                    ILogger log)
        {
            context = c;
            repository = r;
            this.logger = log;
            repository.Logger = this.logger;
        }

        protected override void CopyDataToModel(Data.EF.bvc_CustomUrl data, CustomUrl model)
        {
            model.Bvin = data.bvin;            
            model.StoreId = data.StoreId;
            model.IsPermanentRedirect = data.IsPermanentRedirect;
            model.LastUpdated = data.LastUpdated;
            model.RedirectToUrl = data.RedirectToUrl;
            model.RequestedUrl = data.RequestedUrl;
            model.SystemData = data.SystemData;
            model.SystemDataType = (CustomUrlType)data.SystemDataType;            
        }
        protected override void CopyModelToData(Data.EF.bvc_CustomUrl data, CustomUrl model)
        {
            data.bvin = model.Bvin;
            data.StoreId = model.StoreId;
            data.IsPermanentRedirect = model.IsPermanentRedirect;
            data.LastUpdated = model.LastUpdated;
            data.RedirectToUrl = model.RedirectToUrl;
            data.RequestedUrl = model.RequestedUrl;
            data.SystemData = model.SystemData;
            data.SystemDataType = (int)model.SystemDataType;                          
        }

        public CustomUrl Find(string bvin)
        {
            CustomUrl result = FindForAllStores(bvin);
            if (result != null)
            {
                if (result.StoreId == context.CurrentStore.Id)
                {
                    return result;
                }
            }
            return null;
        }
        public CustomUrl FindForAllStores(string bvin)
        {
            return this.Find(new PrimaryKey(bvin));
        }
        public override bool Create(CustomUrl item)
        {
            if (item.Bvin == string.Empty)
            {
                item.Bvin = System.Guid.NewGuid().ToString();
            }
            item.StoreId = context.CurrentStore.Id;
            item.LastUpdated = DateTime.UtcNow;
 	        return base.Create(item);
        }
        public bool Update(CustomUrl c)
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
            CustomUrl item = Find(bvin);
            if (item == null) return false;

           return Delete(new PrimaryKey(bvin));            
        }

        public List<CustomUrl> FindBySystemData(string systemData)
        {
            long storeId = context.CurrentStore.Id;
            IQueryable<Data.EF.bvc_CustomUrl> data = repository.Find().Where(y => y.StoreId == storeId)
                                                       .Where(y => y.SystemData == systemData);                                                                   
            return ListPoco(data);
        }
        public CustomUrl FindByRequestedUrl(string requestedUrl)
        {
            string match = requestedUrl.ToLowerInvariant();
            long storeId = context.CurrentStore.Id;
            IQueryable<Data.EF.bvc_CustomUrl> data = repository.Find().Where(y => y.StoreId == storeId)
                                                       .Where(y => y.RequestedUrl == match);
            return FirstPoco(data);
        }
        public CustomUrl FindByRedirectToUrl(string redirectToUrl)
        {
            string match = redirectToUrl.ToLowerInvariant();
            long storeId = context.CurrentStore.Id;
            IQueryable<Data.EF.bvc_CustomUrl> data = repository.Find().Where(y => y.StoreId == storeId)
                                                       .Where(y => y.RedirectToUrl == match);
            return FirstPoco(data);
        }

        public List<CustomUrl> FindAll()
        {
            int totalCount = 0;
            return FindAllPaged(1, int.MaxValue, ref totalCount);
        }
        public List<CustomUrl> FindAllPaged(int pageNumber, int pageSize, ref int totalCount)
        {
            List<CustomUrl> result = new List<CustomUrl>();
            long storeId = context.CurrentStore.Id;
            IQueryable<Data.EF.bvc_CustomUrl> data = repository.Find().Where(y => y.StoreId == storeId)
                                      .OrderBy(y => y.RequestedUrl);

            var countData = data;
            totalCount = countData.Count();

            var paged = PageItems(pageNumber, pageSize, data);
            result = ListPoco(paged);
            if (result == null) result = new List<CustomUrl>();
                      
            return result;
        }

        public void Register301(string requestedUrl, string redirectUrl, string objectId, CustomUrlType customUrlType, RequestContext context, MerchantTribeApplication app)
        {
            bool AlreadyInUse = Utilities.UrlRewriter.IsUrlInUse(requestedUrl, string.Empty, context, app);
            if (AlreadyInUse) return;
            CustomUrl c = new CustomUrl();
            c.IsPermanentRedirect = true;
            c.RedirectToUrl = redirectUrl;
            c.RequestedUrl = requestedUrl;
            c.StoreId = app.CurrentRequestContext.CurrentStore.Id;
            c.SystemData = objectId;
            c.SystemDataType = customUrlType;
            Create(c);
            UpdateAllUrlsForObject(objectId, redirectUrl);
        }

        public void UpdateAllUrlsForObject(string objectId, string redirectToUrl)
        {
            List<CustomUrl> all = FindBySystemData(objectId);
            if (all == null) return;
            foreach (CustomUrl u in all)
            {
                u.RedirectToUrl = redirectToUrl;
                Update(u);
            }
        }
      
      
    }
}
