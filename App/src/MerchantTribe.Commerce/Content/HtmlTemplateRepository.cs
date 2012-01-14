using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MerchantTribe.Web.Data;
using MerchantTribe.Web.Logging;

namespace MerchantTribe.Commerce.Content
{
    public class HtmlTemplateRepository : ConvertingRepositoryBase<Data.EF.bvc_HtmlTemplates, HtmlTemplate>
    {

        public static HtmlTemplateRepository InstantiateForMemory(RequestContext c)
        {
            HtmlTemplateRepository result = null;
            result = new HtmlTemplateRepository(c, new MemoryStrategy<Data.EF.bvc_HtmlTemplates>(PrimaryKeyType.Long), new TextLogger());
            return result;
        }
        public static HtmlTemplateRepository InstantiateForDatabase(RequestContext c)
        {
            HtmlTemplateRepository result = null;
            result = new HtmlTemplateRepository(c,
                new EntityFrameworkRepository<Data.EF.bvc_HtmlTemplates>(
                    new Data.EF.EntityFrameworkDevConnectionString(c.ConnectionStringForEntityFramework)),
                    new SupressLogger()
                    );
            return result;
        }

        private RequestContext context = null;

        private HtmlTemplateRepository(RequestContext c, IRepositoryStrategy<Data.EF.bvc_HtmlTemplates> r, ILogger log)
        {
            context = c;
            repository = r;            
            this.logger = log;
            repository.Logger = this.logger;
        }

        protected override void CopyDataToModel(Data.EF.bvc_HtmlTemplates data, HtmlTemplate model)
        {
            model.Id = data.Id;
            model.StoreId = data.StoreId;
            model.LastUpdatedUtc = data.LastUpdatedUtc;
            model.Body = data.Body;
            model.DisplayName = data.DisplayName;
            model.From = data.FromEmail;
            model.RepeatingSection = data.RepeatingSection;
            model.Subject = data.Subject;
            model.TemplateType = (HtmlTemplateType)data.TemplateType;
        }
        protected override void CopyModelToData(Data.EF.bvc_HtmlTemplates data, HtmlTemplate model)
        {
            data.Id = model.Id;
            data.StoreId = model.StoreId;
            data.LastUpdatedUtc = model.LastUpdatedUtc;
            data.Body = model.Body;            
            data.DisplayName = model.DisplayName;
            data.FromEmail = model.From;
            data.RepeatingSection = model.RepeatingSection;
            data.Subject = model.Subject;
            data.TemplateType = (int)model.TemplateType;
        }

        public HtmlTemplate Find(long Id)
        {
            HtmlTemplate result = FindForAllStores(Id);
            if (result != null)
            {
                if (result.StoreId == context.CurrentStore.Id)
                {
                    return result;
                }
            }
            return null;
        }
        public HtmlTemplate FindForAllStores(long Id)
        {
            return this.Find(new PrimaryKey(Id));            
        }
        public HtmlTemplate FindByStoreAndType(long storeId, HtmlTemplateType templateType)
        {
            int typeId = (int)templateType;

            IQueryable<Data.EF.bvc_HtmlTemplates> items = repository.Find().Where(y => y.StoreId == storeId)
                                                                            .Where(y => y.TemplateType == typeId);
            return SinglePoco(items);
        }

        public override bool Create(HtmlTemplate item)
        {
            item.LastUpdatedUtc = DateTime.UtcNow;            
            item.StoreId = context.CurrentStore.Id;
 	        return base.Create(item);
        }
        public bool Update(HtmlTemplate c)
        {
            if (c.StoreId != context.CurrentStore.Id)
            {
                return false;
            }

            c.LastUpdatedUtc = DateTime.UtcNow;
            return this.Update(c, new PrimaryKey(c.Id));            
        }
        public bool Delete(long Id)
        {
            HtmlTemplate t = Find(Id);
            if (t == null) return false;
            if (t.StoreId != context.CurrentStore.Id) return false;            
            return Delete(new PrimaryKey(Id));
        }
       
        public List<HtmlTemplate> FindAll()
        {
            long storeId = context.CurrentStore.Id;
            IQueryable<Data.EF.bvc_HtmlTemplates> result = repository.Find().Where(y => y.StoreId == storeId).OrderBy(y => y.DisplayName);
            return ListPoco(result);
        }     

    }
}
