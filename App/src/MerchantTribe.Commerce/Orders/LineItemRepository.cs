using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MerchantTribe.Web.Data;
using MerchantTribe.Web.Logging;

namespace MerchantTribe.Commerce.Orders
{
    public class LineItemRepository : ConvertingRepositoryBase<Data.EF.bvc_LineItem, LineItem>
    {
        RequestContext currentContext = null;

        public static LineItemRepository InstantiateForMemory(RequestContext c)
        {
            LineItemRepository result = null;
            ILogger logger = new MerchantTribe.Commerce.EventLog();
            result = new LineItemRepository(new MemoryStrategy<Data.EF.bvc_LineItem>(PrimaryKeyType.Long), logger, c);
            return result;
        }
        public static LineItemRepository InstantiateForDatabase(RequestContext c)
        {
            LineItemRepository result = null;
            ILogger logger = new MerchantTribe.Commerce.EventLog();
            result = new LineItemRepository(new EntityFrameworkRepository<Data.EF.bvc_LineItem>(
                    new Data.EF.EntityFrameworkDevConnectionString(c.ConnectionStringForEntityFramework)), logger, c);
            return result;
        }
        public LineItemRepository(IRepositoryStrategy<Data.EF.bvc_LineItem> strategy, ILogger log, RequestContext context)
        {
            repository = strategy;
            this.logger = log;
            repository.Logger = this.logger;
            this.currentContext = context;
        }

        protected override void CopyModelToData(Data.EF.bvc_LineItem data, LineItem model)
        {
            data.Id = model.Id;
            data.AdjustedPrice = model.AdjustedPricePerItem;
            data.BasePrice = model.BasePricePerItem;
            data.CustomProperties = model.CustomPropertiesToXml();
            data.DiscountDetails = Marketing.DiscountDetail.ListToXml(model.DiscountDetails);
            data.LastUpdated = model.LastUpdatedUtc;
            data.LineTotal = model.LineTotal;
            data.OrderBvin = model.OrderBvin;
            data.ProductId = model.ProductId;
            data.ProductName = model.ProductName;
            data.ProductShippingHeight = model.ProductShippingHeight;
            data.ProductShippingLength = model.ProductShippingLength;
            data.ProductShippingWeight = model.ProductShippingWeight;
            data.ProductShippingWidth = model.ProductShippingWidth;
            data.ProductShortDescription = model.ProductShortDescription;
            data.ProductSku = model.ProductSku;
            data.Quantity = model.Quantity;
            data.QuantityReturned = model.QuantityReturned;
            data.QuantityShipped = model.QuantityShipped;
            data.SelectionData = model.SelectionData.SerializeToXml();
            data.ShippingPortion = model.ShippingPortion;
            data.ShippingScheduleId = model.ShippingSchedule;
            data.StatusCode = model.StatusCode;
            data.StatusName = model.StatusName;
            data.StoreId = model.StoreId;
            data.TaxPortion = model.TaxPortion;
            data.TaxScheduleId = model.TaxSchedule;
            data.VariantId = model.VariantId;
            data.ShipFromAddress = model.ShipFromAddress.ToXml(true);
            data.ShipFromMode = (int)model.ShipFromMode;
            data.ShipFromNotificationId = model.ShipFromNotificationId;
            data.ExtraShipCharge = model.ExtraShipCharge;

        }
        protected override void CopyDataToModel(Data.EF.bvc_LineItem data, LineItem model)
        {
            model.Id = data.Id;
            model.BasePricePerItem = data.BasePrice;
            model.CustomPropertiesFromXml(data.CustomProperties);
            model.DiscountDetails = Marketing.DiscountDetail.ListFromXml(data.DiscountDetails);
            model.LastUpdatedUtc = data.LastUpdated;
            model.OrderBvin = data.OrderBvin;
            model.ProductId = data.ProductId;
            model.ProductName = data.ProductName;
            model.ProductShippingHeight = data.ProductShippingHeight;
            model.ProductShippingLength = data.ProductShippingLength;
            model.ProductShippingWeight = data.ProductShippingWeight;
            model.ProductShippingWidth = data.ProductShippingWidth;
            model.ProductShortDescription = data.ProductShortDescription;
            model.ProductSku = data.ProductSku;
            model.Quantity = data.Quantity;
            model.QuantityReturned = data.QuantityReturned;
            model.QuantityShipped = data.QuantityShipped;
            model.SelectionData.DeserializeFromXml(data.SelectionData);
            model.ShippingPortion = data.ShippingPortion;
            model.ShippingSchedule = data.ShippingScheduleId;
            model.StatusCode = data.StatusCode;
            model.StatusName = data.StatusName;
            model.StoreId = data.StoreId;
            model.TaxPortion = data.TaxPortion;
            model.TaxSchedule = data.TaxScheduleId;
            model.VariantId = data.VariantId;
            model.ShipFromAddress.FromXmlString(data.ShipFromAddress);
            model.ShipFromMode = (Shipping.ShippingMode)data.ShipFromMode;
            model.ShipFromNotificationId = data.ShipFromNotificationId;
            model.ExtraShipCharge = data.ExtraShipCharge;
        }

        public bool Update(LineItem item)
        {
            item.LastUpdatedUtc = DateTime.UtcNow;
            return base.Update(item, new PrimaryKey(item.Id));
        }
        public bool Delete(long id)
        {
            return Delete(new PrimaryKey(id));
        }

        public List<LineItem> FindForOrder(string orderBvin)
        {
            var items = repository.Find().Where(y => y.OrderBvin == orderBvin)
                                        .OrderBy(y => y.Id);
            return ListPoco(items);
        }
        public List<LineItem> FindForOrders(List<string> bvins)
        {
            var items = repository.Find().Where(y => bvins.Contains(y.OrderBvin))
                                        .OrderBy(y => y.Id);
            return ListPoco(items);
        }
        public void DeleteForOrder(string orderBvin)
        {
            List<LineItem> existing = FindForOrder(orderBvin);
            foreach (LineItem sub in existing)
            {
                Delete(sub.Id);
            }
        }

        public void MergeList(string orderBvin, long storeId, List<LineItem> subitems)
        {
            // Set Base Key Field
            foreach (LineItem item in subitems)
            {
                item.OrderBvin = orderBvin;
                item.StoreId = storeId;
            }

            // Create or Update
            foreach (LineItem itemnew in subitems)
            {
                if (itemnew.Id < 1)
                {
                    itemnew.LastUpdatedUtc = DateTime.UtcNow;
                    Create(itemnew);
                }
                else
                {
                    Update(itemnew);
                }
            }

            // Delete missing
            List<LineItem> existing = FindForOrder(orderBvin);
            foreach (LineItem ex in existing)
            {
                var count = (from sub in subitems
                             where sub.Id == ex.Id
                             select sub).Count();
                if (count < 1)
                {
                    Delete(ex.Id);
                }
            }
        }

        public Dictionary<string, int> FindPopularItems(DateTime startDateUtc, DateTime endDateUtc, int maxItems)
        {
            Dictionary<string, int> result = new Dictionary<string, int>();
            var storeId = currentContext.CurrentStore.Id;
            var query = (from lineitems in this.repository.Find()
                         where lineitems.StoreId == storeId &&
                         lineitems.bvc_Order.TimeOfOrder >= startDateUtc &&
                         lineitems.bvc_Order.TimeOfOrder <= endDateUtc
                         group lineitems by lineitems.ProductId into groupedItems
                         orderby groupedItems.Sum(y => y.Quantity) descending
                         select new { ProductId = groupedItems.Key, Quantity = groupedItems.Sum(y => y.Quantity) }
                         );

            var query2 = query.Take(maxItems).ToList();
            foreach (var popular in query2)
            {
                result.Add(popular.ProductId, popular.Quantity);
            }

            return result;
        }

    }
}
