using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MerchantTribe.Web.Data;
using MerchantTribe.Web.Logging;

namespace MerchantTribe.Commerce.Metrics
{
    public class SearchQueryRepository: ConvertingRepositoryBase<Data.EF.bvc_SearchQuery, SearchQuery>
    {
        private RequestContext context = null;

        public static SearchQueryRepository InstantiateForMemory(RequestContext c)
        {
            return new SearchQueryRepository(c, new MemoryStrategy<Data.EF.bvc_SearchQuery>(PrimaryKeyType.Bvin),                                           
                                           new TextLogger());
        }
        public static SearchQueryRepository InstantiateForDatabase(RequestContext c)
        {
            SearchQueryRepository result = null;
            result = new SearchQueryRepository(c,
                new EntityFrameworkRepository<Data.EF.bvc_SearchQuery>(
                    new Data.EF.EntityFrameworkDevConnectionString(c.ConnectionStringForEntityFramework)),
                    new SupressLogger()
                    );
            return result;
        }
        public SearchQueryRepository(RequestContext c, IRepositoryStrategy<Data.EF.bvc_SearchQuery> r,                                    
                                    ILogger log)
        {
            context = c;
            repository = r;
            this.logger = log;
            repository.Logger = this.logger;
        }

        protected override void CopyDataToModel(Data.EF.bvc_SearchQuery data, SearchQuery model)
        {
            model.Bvin = data.bvin;            
            model.StoreId = data.StoreId;
            model.LastUpdated = data.LastUpdated;
            model.QueryPhrase = data.QueryPhrase;
            model.ShopperID = data.ShopperId;            
        }
        protected override void CopyModelToData(Data.EF.bvc_SearchQuery data, SearchQuery model)
        {
            data.bvin = model.Bvin;
            data.StoreId = model.StoreId;
            data.LastUpdated = model.LastUpdated;
            data.QueryPhrase = model.QueryPhrase;
            data.ShopperId = model.ShopperID;
        }

        public SearchQuery Find(string bvin)
        {
            SearchQuery result = FindForAllStores(bvin);
            if (result != null)
            {
                if (result.StoreId == context.CurrentStore.Id)
                {
                    return result;
                }
            }
            return null;
        }
        public SearchQuery FindForAllStores(string bvin)
        {
            return this.Find(new PrimaryKey(bvin));
        }
        public override bool Create(SearchQuery item)
        {
            if (item.Bvin == string.Empty)
            {
                item.Bvin = System.Guid.NewGuid().ToString();
            }
            item.StoreId = context.CurrentStore.Id;
            item.LastUpdated = DateTime.UtcNow;
 	        return base.Create(item);
        }
        public bool Update(SearchQuery c)
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
            SearchQuery item = Find(bvin);
            if (item == null) return false;

           return Delete(new PrimaryKey(bvin));            
        }
        public bool DeleteAll()
        {
            List<SearchQuery> all = FindAll();
            foreach (SearchQuery q in all)
            {
                Delete(q.Bvin);
            }
            return true;
        }
        public List<SearchQuery> FindAll()
        {
            int totalCount = 0;
            return FindAllPaged(1, int.MaxValue, ref totalCount);
        }
        public List<SearchQuery> FindAllPaged(int pageNumber, int pageSize, ref int totalCount)
        {
            List<SearchQuery> result = new List<SearchQuery>();
            long storeId = context.CurrentStore.Id;
            IQueryable<Data.EF.bvc_SearchQuery> data = repository.Find().Where(y => y.StoreId == storeId)
                                      .OrderBy(y => y.QueryPhrase);

            var countData = data;
            totalCount = countData.Count();

            var paged = PageItems(pageNumber, pageSize, data);
            result = ListPoco(paged);
            if (result == null) result = new List<SearchQuery>();
                      
            return result;
        }
       

        public List<SearchQuery> FindByShopperId(string shopperId, int pageNumber, int pageSize, ref int totalCount)
        {
            List<SearchQuery> result = new List<SearchQuery>();
            long storeId = context.CurrentStore.Id;
            IQueryable<Data.EF.bvc_SearchQuery> data = repository.Find().Where(y => y.StoreId == storeId)
                                                                .Where(y => y.ShopperId == shopperId)
                                                                .OrderByDescending(y => y.LastUpdated);

            var countData = data;
            totalCount = countData.Count();

            var paged = PageItems(pageNumber, pageSize, data);
            result = ListPoco(paged);
            if (result == null) result = new List<SearchQuery>();

            return result;
        }

        public class SearchQueryData
        {
            public string QueryPhrase { get; set; }
            public int Count { get; set; }
            public decimal Percentage { get; set; }

            public SearchQueryData()
            {
                QueryPhrase = string.Empty;
                Count = 0;
                Percentage = 0;
            }
        }
        public List<SearchQueryData> FindQueryCountReport()
        {
            List<SearchQueryData> result = new List<SearchQueryData>();
            long storeId = context.CurrentStore.Id;
           
            var data2 = repository.Find().Where(y => y.StoreId == storeId)
                                        .GroupBy(y => y.QueryPhrase)
                                        .Select(grouping => grouping.Select(y => new SearchQueryData()
                                        {
                                            QueryPhrase = y.QueryPhrase,
                                            Count = grouping.Count()
                                        })
                                        .FirstOrDefault())
                                        .OrderByDescending(y => y.Count).ToList();

            result = data2;
            if (result == null) result = new List<SearchQueryData>();
            return result;
        }

             
    }
}
