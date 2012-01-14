using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MerchantTribe.Web.Data;
using MerchantTribe.Web.Logging;

namespace MerchantTribe.Commerce.Marketing
{
    public class PromotionRepository: ConvertingRepositoryBase<Data.EF.bvc_Promotions, Promotion>
    {
        private RequestContext context = null;

        public static PromotionRepository InstantiateForMemory(RequestContext c)
        {
            return new PromotionRepository(c, new MemoryStrategy<Data.EF.bvc_Promotions>(PrimaryKeyType.Long), new TextLogger());
        }
        public static PromotionRepository InstantiateForDatabase(RequestContext c)
        {
            return new PromotionRepository(c,
                new EntityFrameworkRepository<Data.EF.bvc_Promotions>(
                    new Data.EF.EntityFrameworkDevConnectionString(c.ConnectionStringForEntityFramework)),
                    new SupressLogger()
                    );
        }
        public PromotionRepository(RequestContext c, IRepositoryStrategy<Data.EF.bvc_Promotions> r, ILogger log)
        {
            context = c;
            this.logger = log;
            repository = r;
            repository.Logger = this.logger;
        }

        protected override void CopyModelToData(Data.EF.bvc_Promotions data, Promotion model)
        {
            data.Id = model.Id;
            data.StoreId = model.StoreId;
            data.Mode = (int)model.Mode;                                    
            data.LastUpdatedUtc = model.LastUpdatedUtc;            
            data.Name = model.Name;
            data.CustomerDescription = model.CustomerDescription;
            data.StartDateUtc = model.StartDateUtc;
            data.EndDateUtc = model.EndDateUtc;
            data.IsEnabled = model.IsEnabled;            
            data.ActionsXml = model.ActionsToXml();
            data.QualificationsXml = model.QualificationsToXml();
        }
        protected override void CopyDataToModel(Data.EF.bvc_Promotions data, Promotion model)
        {
            model.Id = data.Id;
            model.StoreId = data.StoreId;
            model.Mode = (PromotionType)data.Mode;
            model.LastUpdatedUtc = data.LastUpdatedUtc;
            model.Name = data.Name;
            model.CustomerDescription = data.CustomerDescription;
            model.StartDateUtc = data.StartDateUtc;
            model.EndDateUtc = data.EndDateUtc;
            model.IsEnabled = data.IsEnabled;
            model.ActionsFromXml(data.ActionsXml);
            model.QualificationsFromXml(data.QualificationsXml);
        }

        public Promotion Find(long id)
        {
            Data.EF.bvc_Promotions data = repository.FindByPrimaryKey(new PrimaryKey(id));
            if (data == null) return null;

            Promotion result = new Promotion();
            CopyDataToModel(data, result);
            return result;
        }

        public override bool Create(Promotion item)
        {
            item.StoreId = context.CurrentStore.Id;
            item.LastUpdatedUtc = DateTime.UtcNow;
            return base.Create(item);
        }
        
        public bool Update(Promotion item)
        {
            if (item.StoreId != context.CurrentStore.Id)
            {
                return false;
            }
            item.LastUpdatedUtc = DateTime.UtcNow;
            return this.Update(item, new PrimaryKey(item.Id));
        }

        public bool Delete(long id)
        {
            return Delete(new PrimaryKey(id));
        }
       
        public List<Promotion> FindAll()
        {
            int rows = 0;
            return FindAllPaged(1, int.MaxValue, ref rows);
        }
        public List<Promotion> FindAllPaged(int pageNumber, int pageSize, ref int totalRowCount)
        {
            long storeId = context.CurrentStore.Id;

            List<Promotion> result = new List<Promotion>();

            if (pageNumber < 1) pageNumber = 1;
            int take = pageSize;
            int skip = (pageNumber - 1) * pageSize;

            IQueryable<Data.EF.bvc_Promotions> items = repository.Find().Where(y => y.StoreId == storeId).OrderByDescending(y => y.StartDateUtc);

            totalRowCount = items.Count();
            items = items.Skip(skip).Take(take);

            if (items != null)
            {
                result = ListPoco(items);
            }

            return result;
        }
        public List<Promotion> FindAllWithFilter(string keyword, bool showDisabled, int pageNumber, int pageSize, ref int totalRowCount)
        {
            long storeId = context.CurrentStore.Id;

            List<Promotion> result = new List<Promotion>();

            if (pageNumber < 1) pageNumber = 1;
            int take = pageSize;
            int skip = (pageNumber - 1) * pageSize;

            IQueryable<Data.EF.bvc_Promotions> items = repository.Find()
                                .Where(y => y.StoreId == storeId);

            // keyword 
            if (keyword.Trim().Length > 0)
            {
                items = items.Where(y => y.Name.Contains(keyword) || y.CustomerDescription.Contains(keyword));
            }

            // show/hide disabled flag
            if (!showDisabled)
            {
                items = items.Where(y => y.IsEnabled == true);
            }
            items = items.OrderByDescending(y => y.StartDateUtc);


            totalRowCount = items.Count();
            items = items.Skip(skip).Take(take);
            if (items != null)
            {
                result = ListPoco(items);
            }

            return result;
        }
        public List<Promotion> FindAllSales(int pageNumber, int pageSize, ref int totalRowCount)
        {
            long storeId = context.CurrentStore.Id;

            List<Promotion> result = new List<Promotion>();

            if (pageNumber < 1) pageNumber = 1;
            int take = pageSize;
            int skip = (pageNumber - 1) * pageSize;

            IQueryable<Data.EF.bvc_Promotions> items = repository.Find()
                                .Where(y => y.StoreId == storeId)
                                .Where(y => y.Mode == 1)
                                .OrderByDescending(y => y.StartDateUtc);

            totalRowCount = items.Count();
            items = items.Skip(skip).Take(take);

            if (items != null)
            {
                result = ListPoco(items);
            }

            return result;
        }
        public List<Promotion> FindAllOffers(int pageNumber, int pageSize, ref int totalRowCount)
        {
            long storeId = context.CurrentStore.Id;

            List<Promotion> result = new List<Promotion>();

            if (pageNumber < 1) pageNumber = 1;
            int take = pageSize;
            int skip = (pageNumber - 1) * pageSize;

            IQueryable<Data.EF.bvc_Promotions> items = repository.Find()
                                .Where(y => y.StoreId == storeId)
                                .Where(y => y.Mode == 2)
                                .OrderByDescending(y => y.StartDateUtc);

            totalRowCount = items.Count();
            items = items.Skip(skip).Take(take);

            if (items != null)
            {
                result = ListPoco(items);
            }

            return result;
        }

        public List<Promotion> FindAllPotentiallyActive(DateTime currentDateTimeUtc)
        {
            long storeId = context.CurrentStore.Id;
            List<Promotion> result = new List<Promotion>();            
            IQueryable<Data.EF.bvc_Promotions> items = repository.Find().Where(y => y.StoreId == storeId)
                                                                       .Where(y => y.IsEnabled == true)
                                                                       .Where(y => y.StartDateUtc <= currentDateTimeUtc)
                                                                       .Where(y => y.EndDateUtc >= currentDateTimeUtc)
                                                                       .OrderByDescending(y => y.StartDateUtc);

            if (items != null)
            {
                result = ListPoco(items);
            }

            return result;
        }
        public List<Promotion> FindAllPotentiallyActiveSales(DateTime currentDateTimeUtc)
        {
            long storeId = context.CurrentStore.Id;
            List<Promotion> result = new List<Promotion>();
            IQueryable<Data.EF.bvc_Promotions> items = repository.Find().Where(y => y.StoreId == storeId)
                                                                       .Where(y => y.IsEnabled == true)
                                                                       .Where(y => y.StartDateUtc <= currentDateTimeUtc)
                                                                       .Where(y => y.EndDateUtc >= currentDateTimeUtc)
                                                                       .Where(y => y.Mode == 1)
                                                                       .OrderByDescending(y => y.StartDateUtc);

            if (items != null)
            {
                result = ListPoco(items);
            }

            return result;
        }
        public List<Promotion> FindAllPotentiallyActiveOffers(DateTime currentDateTimeUtc)
        {
            long storeId = context.CurrentStore.Id;
            List<Promotion> result = new List<Promotion>();
            IQueryable<Data.EF.bvc_Promotions> items = repository.Find().Where(y => y.StoreId == storeId)
                                                                       .Where(y => y.IsEnabled == true)
                                                                       .Where(y => y.StartDateUtc <= currentDateTimeUtc)
                                                                       .Where(y => y.EndDateUtc >= currentDateTimeUtc)
                                                                       .Where(y => y.Mode == 2)
                                                                       .OrderByDescending(y => y.StartDateUtc);

            if (items != null)
            {
                result = ListPoco(items);
            }

            return result;
        }

    }
}
