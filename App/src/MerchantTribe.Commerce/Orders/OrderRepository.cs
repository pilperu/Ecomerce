using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MerchantTribe.Web.Data;
using MerchantTribe.Web.Logging;
using MvcMiniProfiler;

namespace MerchantTribe.Commerce.Orders
{
    public class OrderRepository : ConvertingRepositoryBase<Data.EF.bvc_Order, Order>
    {

        public static OrderRepository InstantiateForMemory(RequestContext c)
        {
            OrderRepository result = null;
            result = new OrderRepository(c, new MemoryStrategy<Data.EF.bvc_Order>(PrimaryKeyType.Bvin),
                                           new MemoryStrategy<Data.EF.bvc_LineItem>(PrimaryKeyType.Long),
                                           new MemoryStrategy<Data.EF.bvc_OrderNote>(PrimaryKeyType.Long),
                                           new MemoryStrategy<Data.EF.bvc_OrderCoupon>(PrimaryKeyType.Long),
                                           new MemoryStrategy<Data.EF.bvc_OrderPackage>(PrimaryKeyType.Long),
                                           new TextLogger()
                                           );
            return result;
        }
        public static OrderRepository InstantiateForDatabase(RequestContext c)
        {
            OrderRepository result = null;
            result = new OrderRepository(c, 
                new EntityFrameworkRepository<Data.EF.bvc_Order>(
                    new Data.EF.EntityFrameworkDevConnectionString(c.ConnectionStringForEntityFramework)),
                new EntityFrameworkRepository<Data.EF.bvc_LineItem>(
                    new Data.EF.EntityFrameworkDevConnectionString(c.ConnectionStringForEntityFramework)),
                new EntityFrameworkRepository<Data.EF.bvc_OrderNote>(
                    new Data.EF.EntityFrameworkDevConnectionString(c.ConnectionStringForEntityFramework)),
                new EntityFrameworkRepository<Data.EF.bvc_OrderCoupon>(
                    new Data.EF.EntityFrameworkDevConnectionString(c.ConnectionStringForEntityFramework)),
                new EntityFrameworkRepository<Data.EF.bvc_OrderPackage>(
                    new Data.EF.EntityFrameworkDevConnectionString(c.ConnectionStringForEntityFramework)),
                new SupressLogger()
                    );
            return result;
        }

        private RequestContext context = null;
        private LineItemRepository itemRepository = null;
        private OrderNoteRepository notesRepository = null;
        private OrderCouponRepository couponRepository = null;
        private OrderPackageRepository packageRepository = null;

        private OrderRepository(RequestContext c, 
                                IRepositoryStrategy<Data.EF.bvc_Order> r,
                                IRepositoryStrategy<Data.EF.bvc_LineItem> itemr,
                                IRepositoryStrategy<Data.EF.bvc_OrderNote> noter,
                                IRepositoryStrategy<Data.EF.bvc_OrderCoupon> couponr,
                                IRepositoryStrategy<Data.EF.bvc_OrderPackage> packager,
                                ILogger log)
        {
            context = c;
            repository = r;            
            this.logger = log;
            repository.Logger = this.logger;
            itemRepository = new LineItemRepository(itemr, this.logger, c);
            notesRepository = new OrderNoteRepository(noter, this.logger);
            couponRepository = new OrderCouponRepository(couponr, this.logger);
            packageRepository = new OrderPackageRepository(packager, this.logger);
        }

        protected override void CopyDataToModel(Data.EF.bvc_Order data, Order model)
        {
            model.AffiliateID = data.AffiliateId;
            model.BillingAddress.FromXmlString(data.BillingAddress);
            model.bvin = data.bvin;
            model.CustomProperties = CustomPropertyCollection.FromXml(data.CustomProperties);
            model.FraudScore = data.FraudScore;
            //model.TotalGrand = data.GrandTotal;
            model.TotalHandling = data.HandlingTotal;
            model.Id = data.Id;
            model.Instructions = data.Instructions;
            model.IsPlaced = data.IsPlaced == 1;
            model.LastUpdatedUtc = data.LastUpdated;            
            model.OrderDiscountDetails = Marketing.DiscountDetail.ListFromXml(data.OrderDiscountDetails);
            model.OrderNumber = data.OrderNumber;
            model.PaymentStatus = (OrderPaymentStatus)data.PaymentStatus;
            model.ShippingDiscountDetails = Marketing.DiscountDetail.ListFromXml(data.ShippingDiscountDetails);
            model.ShippingAddress.FromXmlString(data.ShippingAddress);            
            model.ShippingMethodDisplayName = data.ShippingMethodDisplayName;
            model.ShippingMethodId = data.ShippingMethodId;
            model.ShippingProviderId = data.ShippingProviderId;
            model.ShippingProviderServiceCode = data.ShippingProviderServiceCode;
            model.ShippingStatus = (OrderShippingStatus)data.ShippingStatus;
            model.TotalShippingBeforeDiscounts = data.ShippingTotal;
            model.StatusCode = data.StatusCode;
            model.StatusName = data.StatusName;
            model.StoreId = data.StoreId;
            model.TotalTax = data.TaxTotal;
            model.TotalTax2 = data.TaxTotal2;
            model.ThirdPartyOrderId = data.ThirdPartyOrderId;
            model.TimeOfOrderUtc = data.TimeOfOrder;
            model.UserEmail = data.UserEmail;
            model.UserID = data.UserId;                       
        }
        protected override void CopyModelToData(Data.EF.bvc_Order data, Order model)
        {
            data.AffiliateId = model.AffiliateID;
            data.BillingAddress = model.BillingAddress.ToXml(true);
            data.bvin = model.bvin;
            data.CustomProperties = model.CustomProperties.ToXml();
            data.FraudScore = model.FraudScore;
            data.GrandTotal = model.TotalGrand;
            data.HandlingTotal = model.TotalHandling;
            data.Id = model.Id;
            data.Instructions = model.Instructions;
            data.IsPlaced = model.IsPlaced ? 1 : 0;
            data.LastUpdated = model.LastUpdatedUtc;
            data.OrderDiscountDetails = Marketing.DiscountDetail.ListToXml(model.OrderDiscountDetails);
            data.OrderDiscounts = model.TotalOrderDiscounts;
            data.OrderNumber = model.OrderNumber;
            data.PaymentStatus = (int)model.PaymentStatus;
            data.ShippingAddress = model.ShippingAddress.ToXml(true);
            data.ShippingDiscounts = model.TotalShippingDiscounts;
            data.ShippingDiscountDetails = Marketing.DiscountDetail.ListToXml(model.ShippingDiscountDetails);
            data.ShippingMethodDisplayName = model.ShippingMethodDisplayName;
            data.ShippingMethodId = model.ShippingMethodId;
            data.ShippingProviderId = model.ShippingProviderId;
            data.ShippingProviderServiceCode = model.ShippingProviderServiceCode;
            data.ShippingStatus = (int)model.ShippingStatus;
            data.ShippingTotal = model.TotalShippingBeforeDiscounts;
            data.StatusCode = model.StatusCode;
            data.StatusName = model.StatusName;
            data.StoreId = model.StoreId;
            data.SubTotal = model.TotalOrderBeforeDiscounts;
            data.TaxTotal = model.TotalTax;
            data.TaxTotal2 = model.TotalTax2;
            data.ThirdPartyOrderId = model.ThirdPartyOrderId;
            data.TimeOfOrder = model.TimeOfOrderUtc;
            data.UserEmail = model.UserEmail;
            data.UserId = model.UserID;            
        }

        protected override void DeleteAllSubItems(Order model)
        {
            packageRepository.DeleteForOrder(model.bvin);
            itemRepository.DeleteForOrder(model.bvin);
            notesRepository.DeleteForOrder(model.bvin);
            couponRepository.DeleteForOrder(model.bvin);            
        }
        protected override void GetSubItems(Order model)
        {
            model.Items = itemRepository.FindForOrder(model.bvin);
            model.Notes = notesRepository.FindForOrder(model.bvin);
            model.Coupons = couponRepository.FindForOrder(model.bvin);
            model.Packages = packageRepository.FindForOrder(model.bvin);            
        }
        protected override void MergeSubItems(Order model)
        {
            itemRepository.MergeList(model.bvin, model.StoreId, model.Items);
            notesRepository.MergeList(model.bvin, model.StoreId, model.Notes);
            couponRepository.MergeList(model.bvin, model.StoreId, model.Coupons);
            packageRepository.MergeList(model.bvin, model.StoreId, model.Packages);            
        }
    
        public Order FindForCurrentStore(string bvin)
        {
            Order result = FindForAllStores(bvin);
            if (result != null)
            {
                if (result.StoreId == context.CurrentStore.Id)
                {
                    return result;
                }
            }
            return null;
        }
        public Order FindForAllStores(string bvin)
        {
            return this.Find(new PrimaryKey(bvin));            
        }
        public Order FindByThirdPartyOrderId(string thirdPartyOrderId)
        {
            var finder = repository.Find().Where(y => y.ThirdPartyOrderId == thirdPartyOrderId).Select(y => y.bvin).SingleOrDefault();
            
            if (finder == null) return null;
            
            return FindForCurrentStore((string)finder);            
        }
        public Order FindByOrderNumber(string orderNumber)
        {
            var finder = repository.Find().Where(y => y.StoreId == context.CurrentStore.Id)
                                            .Where(y => y.OrderNumber == orderNumber)
                                            .Select(y => y.bvin).SingleOrDefault();
            if (finder == null) return null;
            return FindForCurrentStore((string)finder);                                                        
        }

        public List<Order> FindMany(List<string> bvins)
        {
            long storeId = context.CurrentStore.Id;

            List<Order> result = new List<Order>();

            IQueryable<Data.EF.bvc_Order> items = repository.Find().Where(y => bvins.Contains(y.bvin))
                                                                      .Where(y => y.StoreId == storeId).OrderBy(y => y.Id);
            if (items != null)
            {
                result = ListPoco(items);
            }

            return result;
        }
        public override bool  Create(Order item)
        {
            item.LastUpdatedUtc = DateTime.UtcNow;            
            if (item.bvin == string.Empty)
            {
                item.bvin = System.Guid.NewGuid().ToString();
            }
            item.StoreId = context.CurrentStore.Id;
 	        return base.Create(item);
        }
        public bool Upsert(Order item)
        {
            if (item.bvin == string.Empty)
            {
                return Create(item);
            }
            else
            {
                return Update(item);
            }
        }
        public bool Update(Order item)
        {
            if (item.StoreId != context.CurrentStore.Id)
            {
                return false;
            }

            item.LastUpdatedUtc = DateTime.UtcNow;
            return this.Update(item, new PrimaryKey(item.bvin));            
        }

        public bool Delete(string bvin)
        {
            return Delete(new PrimaryKey(bvin));
        }

        protected virtual List<OrderSnapshot> ListPocoSnapshot(IQueryable<Data.EF.bvc_Order> items)
        {
            List<OrderSnapshot> result = new List<OrderSnapshot>();

            if (items != null)
            {
                foreach (Data.EF.bvc_Order item in items)
                {
                    OrderSnapshot temp = new OrderSnapshot();
                    temp.AffiliateID = item.AffiliateId;
                    temp.BillingAddress.FromXmlString(item.BillingAddress);
                    temp.bvin = item.bvin;
                    temp.CustomProperties = CustomPropertyCollection.FromXml(item.CustomProperties);
                    temp.FraudScore = item.FraudScore;
                    temp.TotalGrand = item.GrandTotal;
                    temp.TotalHandling = item.HandlingTotal;
                    temp.Id = item.Id;
                    temp.Instructions = item.Instructions;
                    temp.IsPlaced = item.IsPlaced == 1;
                    temp.LastUpdatedUtc = item.LastUpdated;
                    temp.TotalOrderDiscounts = item.OrderDiscounts;
                    temp.OrderNumber = item.OrderNumber;
                    temp.PaymentStatus = (OrderPaymentStatus)item.PaymentStatus;
                    temp.TotalShippingDiscounts = item.ShippingDiscounts;
                    temp.ShippingAddress.FromXmlString(item.ShippingAddress);
                    temp.ShippingMethodDisplayName = item.ShippingMethodDisplayName;
                    temp.ShippingMethodId = item.ShippingMethodId;
                    temp.ShippingProviderId = item.ShippingProviderId;
                    temp.ShippingProviderServiceCode = item.ShippingProviderServiceCode;
                    temp.ShippingStatus = (OrderShippingStatus)item.ShippingStatus;
                    temp.TotalShippingBeforeDiscounts = item.ShippingTotal;
                    temp.StatusCode = item.StatusCode;
                    temp.StatusName = item.StatusName;
                    temp.StoreId = item.StoreId;
                    temp.TotalOrderBeforeDiscounts = item.SubTotal;
                    temp.TotalTax = item.TaxTotal;
                    temp.TotalTax2 = item.TaxTotal2;
                    temp.ThirdPartyOrderId = item.ThirdPartyOrderId;
                    temp.TimeOfOrderUtc = item.TimeOfOrder;
                    temp.UserEmail = item.UserEmail;
                    temp.UserID = item.UserId;            
                    result.Add(temp);
                }
            }

            return result;
        }
        public List<OrderSnapshot> FindAll()
        {
            long storeId = context.CurrentStore.Id;
            IQueryable<Data.EF.bvc_Order> result = repository.Find().Where(y => y.StoreId == storeId).OrderBy(y => y.Id);
            return ListPocoSnapshot(result);
        }
        public List<OrderSnapshot> FindAllForAllStores()
        {
            return this.FindAllPagedForAllStores(1, int.MaxValue);
        }
        public new List<OrderSnapshot> FindAllPaged(int pageNumber, int pageSize)
        {
            List<OrderSnapshot> result = new List<OrderSnapshot>();

            if (pageNumber < 1) pageNumber = 1;

            int take = pageSize;
            int skip = (pageNumber - 1) * pageSize;
            long storeId = context.CurrentStore.Id;

            IQueryable<Data.EF.bvc_Order> items = repository.Find().Where(y => y.StoreId == storeId).OrderBy(y => y.Id).Skip(skip).Take(take);
            if (items != null)
            {
                result = ListPocoSnapshot(items);
            }

            return result;
        }
        public List<OrderSnapshot> FindAllPagedForAllStores(int pageNumber, int pageSize)
        {
            List<OrderSnapshot> result = new List<OrderSnapshot>();

            if (pageNumber < 1) pageNumber = 1;

            int take = pageSize;
            int skip = (pageNumber - 1) * pageSize;            

            IQueryable<Data.EF.bvc_Order> items = repository.Find().OrderBy(y => y.Id).Skip(skip).Take(take);
            if (items != null)
            {
                result = ListPocoSnapshot(items);
            }

            return result;
        }
        public List<OrderSnapshot> FindManySnapshots(List<string> bvins)
        {
            long storeId = context.CurrentStore.Id;

            List<OrderSnapshot> result = new List<OrderSnapshot>();

            IQueryable<Data.EF.bvc_Order> items = repository.Find().Where(y => bvins.Contains(y.bvin))
                                                                      .Where(y => y.StoreId == storeId).OrderBy(y => y.Id);
            if (items != null)
            {
                result = ListPocoSnapshot(items);
            }

            return result;
        }        
        public List<OrderSnapshot> FindByCriteria(OrderSearchCriteria criteria)
        {
            int temp = -1;
            return FindByCriteriaPaged(criteria, 1, int.MaxValue, ref temp);
        }
        public List<OrderSnapshot> FindByCriteriaPaged(OrderSearchCriteria criteria, int pageNumber, int pageSize, ref int rowCount)
        {
            List<OrderSnapshot> result = new List<OrderSnapshot>();

            if (pageNumber < 1) pageNumber = 1;
            int take = pageSize;
            int skip = (pageNumber - 1) * pageSize;

            IQueryable<Data.EF.bvc_Order> items = repository.Find()
                      .Where(y => y.StoreId == context.CurrentStore.Id)
                      .Where(y => y.TimeOfOrder >= criteria.StartDateUtc 
                               && y.TimeOfOrder <= criteria.EndDateUtc);                                                                                

            // Order Number
            if (criteria.OrderNumber != string.Empty)
            {
                items = items.Where(y => y.OrderNumber == criteria.OrderNumber);
            }

            // Is Placed
            items = items.Where(y => y.IsPlaced == (criteria.IsPlaced ? 1 : 0));

            // Status Code
            if (criteria.StatusCode != string.Empty)
            {
                items = items.Where(y => y.StatusCode == criteria.StatusCode);
            }

            // Affiliate Id
            if (criteria.AffiliateId != string.Empty)
            {
                items = items.Where(y => y.AffiliateId == criteria.AffiliateId);
            }

            // Payment Status
            if (criteria.PaymentStatus != OrderPaymentStatus.Unknown)
            {
                int tempPay = (int)criteria.PaymentStatus;
                items = items.Where(y => y.PaymentStatus == tempPay);
            }

            // Shipping Status
            if (criteria.ShippingStatus != OrderShippingStatus.Unknown)
            {
                int tempShip = (int)criteria.ShippingStatus;
                items = items.Where(y => y.ShippingStatus == tempShip);
            }

            // Keyword (most expensive operation)
            if (criteria.Keyword != string.Empty)
            {
                items = items.Where(y => y.OrderNumber.Contains(criteria.Keyword) 
                                      || y.UserEmail.Contains(criteria.Keyword)
                                      || y.BillingAddress.Contains(criteria.Keyword) 
                                      || y.ShippingAddress.Contains(criteria.Keyword));
            }

            // return total item count;
            var counter = items.Count();
            rowCount = counter;
            
            if (criteria.SortDescending)
            {
                var paged = items.OrderByDescending(y => y.Id).Skip(skip).Take(take);
                return ListPocoSnapshot(paged);                        
            }
            else
            {
                var paged2 = items.OrderBy(y => y.Id).Skip(skip).Take(take);
                return ListPocoSnapshot(paged2);                        
            }                        
        }

        public decimal FindTotalSpentByUser(string userId, long storeId)
        {
            var total = repository.Find()
                       .Where(y => y.UserId == userId)
                       .Where(y => y.StoreId == storeId)
                       .Where(y => y.IsPlaced == 1).Sum(y => y.GrandTotal);
            return total;                       
        }

        public List<OrderSnapshot> FindByUserId(string userId, int pageNumber, int pageSize, ref int totalCount)
        {
            if (pageNumber < 1) pageNumber = 1;
            int take = pageSize;
            int skip = (pageNumber - 1) * pageSize;

            IQueryable<Data.EF.bvc_Order> items = repository.Find()                                                
                        .Where(y => y.UserId == userId)
                        .Where(y => y.StoreId == context.CurrentStore.Id)
                        .Where(y => y.IsPlaced == 1)
                        .OrderBy(y => y.Id);

            var count = items.Count();
            totalCount = count;

            var paged = items.Skip(skip).Take(take);
            return ListPocoSnapshot(paged);
        }

        public List<OrderSnapshot> FindByIdRange(int startId, int endId, int pageNumber, int pageSize, ref int totalCount)
        {
            if (pageNumber < 1) pageNumber = 1;
            int take = pageSize;
            int skip = (pageNumber - 1) * pageSize;

            IQueryable<Data.EF.bvc_Order> items = repository.Find()
                        .Where(y=> y.StoreId == context.CurrentStore.Id)
                        .Where(y=> y.Id >= startId && y.Id <= endId)                       
                        .OrderBy(y => y.Id);

            var count = items.Count();
            totalCount = count;

            var paged = items.Skip(skip).Take(take);            
            return ListPocoSnapshot(paged);
        }

        public List<LineItem> FindLineItemsForOrders(List<OrderSnapshot> snaps)
        {
            List<LineItem> items = this.itemRepository.FindForOrders(snaps.Select(y => y.bvin).ToList());
            return items;
        }

        public Dictionary<string, int> FindPopularItems(DateTime startDateUtc, DateTime endDateUtc, int maxItems)
        {
            return this.itemRepository.FindPopularItems(startDateUtc, endDateUtc, maxItems);
        }

    }
}
