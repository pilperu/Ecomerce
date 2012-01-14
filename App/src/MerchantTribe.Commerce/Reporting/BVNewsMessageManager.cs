using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MerchantTribe.Web.Data;
using MerchantTribe.Web;
using MerchantTribe.Web.Logging;

namespace MerchantTribe.Commerce.Reporting
{
    public class BVNewsMessageManager : ConvertingRepositoryBase<Data.EF.ecommrc_News, BVNewsMessage>
    {
        private RequestContext context = null;

        public static BVNewsMessageManager InstantiateForMemory(RequestContext c)
        {
            BVNewsMessageManager result = null;
            result = new BVNewsMessageManager(c,
                        new MemoryStrategy<Data.EF.ecommrc_News>(PrimaryKeyType.Long));
            return result;
        }
        public static BVNewsMessageManager InstantiateForDatabase(RequestContext c)
        {
            BVNewsMessageManager result = null;
            result = new BVNewsMessageManager(c,
                     new EntityFrameworkRepository<Data.EF.ecommrc_News>(new Data.EF.EntityFrameworkDevConnectionString(c.ConnectionStringForEntityFramework)));            
            return result;
        }
        public BVNewsMessageManager(RequestContext c, IRepositoryStrategy<Data.EF.ecommrc_News> r)
        {
            context = c;
            repository = r;
            this.logger = new SupressLogger();
            repository.Logger = this.logger;
        }

        protected override void CopyDataToModel(Data.EF.ecommrc_News data, BVNewsMessage model)
        {            
            model.Id = data.Id;
            model.TimeStampUtc = data.TimeStampUtc;
            model.Message = data.Message;
        }

        protected override void CopyModelToData(Data.EF.ecommrc_News data, BVNewsMessage model)
        {
            data.Id = model.Id;
            data.TimeStampUtc = model.TimeStampUtc;
            data.Message = model.Message;
        }
     
        public List<BVNewsMessage> GetLatestNews(int maxItems)
        {
            return this.ListPoco(this.repository.Find().OrderByDescending(y => y.TimeStampUtc).Take(maxItems));
        }
    }
}
