using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MerchantTribe.Web.Geography;
using MerchantTribe.Commerce.Taxes;
using MerchantTribe.Commerce.Shipping;
using System.Collections.ObjectModel;
using System.Transactions;

namespace MerchantTribe.Commerce.Orders
{
    public class OrderService
    {
        private RequestContext context = null;

        public OrderRepository Orders { get; private set; }
        public TaxRepository Taxes { get; private set; }
        public TaxScheduleRepository TaxSchedules { get; private set; }
        public ZoneRepository ShippingZones { get; private set; }
        public ShippingMethodRepository ShippingMethods { get; private set; }
        public OrderTransactionRepository Transactions { get; private set; }
        private Accounts.StoreSettingsRepository storeSettings = null;

        public static OrderService InstantiateForMemory(RequestContext c)
        {
            return new OrderService(c,
                                      OrderRepository.InstantiateForMemory(c),
                                      TaxRepository.InstantiateForMemory(c),
                                      TaxScheduleRepository.InstantiateForMemory(c),
                                      ZoneRepository.InstantiateForMemory(c),
                                      OrderTransactionRepository.InstantiateForMemory(c),
                                      ShippingMethodRepository.InstantiateForMemory(c),
                                      Accounts.StoreSettingsRepository.InstantiateForMemory(c));

        }
        public static OrderService InstantiateForDatabase(RequestContext c)
        {
            return new OrderService(c,
                                    OrderRepository.InstantiateForDatabase(c),
                                    TaxRepository.InstantiateForDatabase(c),
                                    TaxScheduleRepository.InstantiateForDatabase(c),
                                    ZoneRepository.InstantiateForDatabase(c),
                                    OrderTransactionRepository.InstantiateForDatabase(c),
                                    ShippingMethodRepository.InstantiateForDatabase(c),
                                    Accounts.StoreSettingsRepository.InstantiateForDatabase(c));
        }

        public OrderService(RequestContext c)
        {
            context = c;
            Orders = OrderRepository.InstantiateForDatabase(c);
            Taxes = TaxRepository.InstantiateForDatabase(c);
            TaxSchedules = TaxScheduleRepository.InstantiateForDatabase(c);
            ShippingZones = ZoneRepository.InstantiateForDatabase(c);
            Transactions = OrderTransactionRepository.InstantiateForDatabase(c);
            ShippingMethods = ShippingMethodRepository.InstantiateForDatabase(c);
        }
        public OrderService(RequestContext c, 
                            OrderRepository orders,
                            TaxRepository taxes, TaxScheduleRepository taxSchedules, 
                            ZoneRepository shippingZones,
                            OrderTransactionRepository transactions,
                            ShippingMethodRepository shippingMethods,
                            Accounts.StoreSettingsRepository settings)
        {
            context = c;
            Orders = orders;
            Taxes = taxes;
            TaxSchedules = taxSchedules;
            ShippingZones = shippingZones;
            Transactions = transactions;
            ShippingMethods = shippingMethods;
            this.storeSettings = settings;
        }

        // Taxes
     
        public bool TaxSchedulesDestroy(long scheduleId)
        {
            try
            {
                List<Tax> taxes = Taxes.FindByTaxSchedule(context.CurrentStore.Id, scheduleId);
                foreach (Tax t in taxes)
                {
                    Taxes.Delete(t.Id);
                }
                TaxSchedules.Delete(scheduleId);
                return true;
            }
            catch
            {
                return false;
            }
        }

        // Shipping
        public Zone ShippingZoneFindInList(List<Zone> zones, long id)
        {
            Zone result = null;

            foreach (Zone z in zones)
            {
                if (z.Id == id)
                {
                    return z;
                }
            }

            return result;
        }
        public bool ShippingZoneAddArea(long zoneId, string countryIso3, string regionAbbreviation)
        {
            Zone z = ShippingZones.Find(zoneId);
            if (z != null)
            {
                if (z.Id == zoneId)
                {
                    bool exists = false;
                    foreach (ZoneArea a in z.Areas)
                    {
                        if (a.CountryIsoAlpha3 == countryIso3 &&
                            a.RegionAbbreviation == regionAbbreviation)
                        {
                            exists = true;
                            break;
                        }
                    }
                    if (!exists)
                    {
                        z.Areas.Add(new ZoneArea() { CountryIsoAlpha3 = countryIso3, RegionAbbreviation = regionAbbreviation });
                        return ShippingZones.Update(z);
                    }
                }
            }
            return false;
        }
        public bool ShippingZoneRemoveArea(long zoneId, string countryIso3, string regionAbbreviation)
        {
            Zone z = ShippingZones.Find(zoneId);
            if (z != null)
            {
                if (z.Id == zoneId)
                {
                    ZoneArea located = null;
                    foreach (ZoneArea a in z.Areas)
                    {
                        if (a.CountryIsoAlpha3 == countryIso3 &&
                            a.RegionAbbreviation == regionAbbreviation)
                        {
                            located = a;
                            break;
                        }
                    }
                    if (located != null)
                    {
                        if (z.Areas.Remove(located))
                        {
                            return ShippingZones.Update(z);
                        }
                    }
                }
            }
            return false;
        }

        //Orders and Items                           
        public SystemOperationResult OrdersUpdateItemQuantity(long itemId, int quantity, Order o)
        {
            SystemOperationResult result = new SystemOperationResult();
            result.Success = true;

            LineItem item = o.Items.Where(y => y.Id == itemId).SingleOrDefault();
            if (item == null)
            {
                result.Success = false;
                result.Message = "Could not locate that item";
                return result;
            }

            if (item.Quantity == quantity)
            {
                result.Success = true;
                return result;
            }

            if (quantity == 0)
            {
                result.Success = o.Items.Remove(item);
            }

            item.Quantity = quantity;
            result.Success = true; 
            return result;
        }
        
        public Utilities.SortableCollection<Shipping.ShippingRateDisplay> FindAvailableShippingRates(Order o, Accounts.Store currentStore)
        {
            Utilities.SortableCollection<Shipping.ShippingRateDisplay> result = new Utilities.SortableCollection<Shipping.ShippingRateDisplay>();

            // Get all the methods that apply to this shipping address and store
            List<Zone> zones = ShippingZones.FindAllZonesForAddress(o.ShippingAddress, o.StoreId);
            List<ShippingMethod> methods = new List<ShippingMethod>();
            methods = this.ShippingMethods.FindForZones(zones);

            // Get Rates for each Method
            foreach (Shipping.ShippingMethod m in methods)
            {

                Collection<Shipping.ShippingRateDisplay> tempRates = m.GetRates(o, currentStore);
                if (tempRates != null)
                {
                    for (int i = 0; i <= tempRates.Count - 1; i++)
                    {
                        ShippingRateDisplay fRate = tempRates[i].GetCopy();
                        //fRate.AdjustRate(m.AdjustmentType, m.Adjustment);                        
                        result.Add(fRate);
                    }
                }
            }
            
            // Tally up extra ship fees
            decimal totalExtraFees = 0m;
            foreach (LineItem li in o.Items)
            {
                if (li.ExtraShipCharge > 0)
                {
                    totalExtraFees += li.ExtraShipCharge;
                }
            }

            // update results with extra ship fees and handling
            foreach (Shipping.ShippingRateDisplay displayRate in result)
            {
                displayRate.Rate += totalExtraFees + o.TotalHandling;
            }


            // Apply promotions to rates here:

            // Run workflow to get shipping rate discounts
            //BusinessRules.ShippingTaskContext c = new BusinessRules.ShippingTaskContext();
            //c.Rates = result;
            //c.UserId = this.UserID;
            //c.Order = this;
            //BusinessRules.Workflow.RunByName(c, BusinessRules.WorkflowNames.ApplyShippingDiscounts);


            // Sort Rates
            result.Sort("Rate", Utilities.SortDirection.Ascending);

            if (result.Count < 1)
            {
                if (o.IsOrderFreeShipping())
                {
                    result.Add(new Shipping.ShippingRateDisplay("Free Shipping.", "", "", 0m, "FREESHIPPING"));
                }
                else
                {
                    string value = Content.SiteTerms.GetTerm(Content.SiteTermIds.ShippingUnknown);
                    if (!string.IsNullOrEmpty(value))
                    {
                        result.Add(new Shipping.ShippingRateDisplay(value, "", "", 0m, "UNKNOWN"));
                    }
                    else
                    {
                        result.Add(new Shipping.ShippingRateDisplay("To Be Determined. Contact Store for Details", "", "", 0m, "TOBEDETERMINED"));
                    }

                }
            }

            return result;
        }
        public Shipping.ShippingRateDisplay OrdersFindShippingRateByUniqueKey(string key, Order o, Accounts.Store currentStore)
        {
            Utilities.SortableCollection<Shipping.ShippingRateDisplay> rates = FindAvailableShippingRates(o, currentStore);

            if (rates == null) return null;
            if (rates.Count < 1) return null;

            foreach (Shipping.ShippingRateDisplay r in rates)
            {
                if (r.UniqueKey == key)
                {
                    return r;
                }
            }

            return null;
        }

        // Orders and Payments
        public string OrdersListPaymentMethods(Order o)
        {
            bool found = false;
            StringBuilder sb = new StringBuilder();

            List<OrderTransaction> allTransactions = Transactions.FindForOrder(o.bvin);

            ReadOnlyCollection<OrderTransaction> transactions = allTransactions.AsReadOnly();
            foreach (OrderTransaction t in transactions)
            {
                switch (t.Action)
                {
                    case MerchantTribe.Payment.ActionType.OfflinePaymentRequest:
                        found = true;
                        sb.Append(t.Amount.ToString("c") + " | " + t.RefNum1);
                        break;
                    case MerchantTribe.Payment.ActionType.CreditCardInfo:
                        found = true;
                        sb.Append(t.Amount.ToString("c") + " | Credit Card " + t.CreditCard.CardNumberLast4Digits);
                        break;
                    case MerchantTribe.Payment.ActionType.GiftCardInfo:
                        found = true;
                        sb.Append(t.Amount.ToString("c") + " | Gift Card ");
                        break;
                    case MerchantTribe.Payment.ActionType.PurchaseOrderInfo:
                        found = true;
                        sb.Append(t.Amount.ToString("c") + " | PO #" + t.PurchaseOrderNumber);
                        break;
                }
            }

            if (found)
            {
                return sb.ToString();
            }
            return "No Payment Methods Selected";
        }
        public bool AddPaymentTransactionToOrder(Order o, MerchantTribe.Payment.Transaction t, MerchantTribeApplication app)
        {
            Orders.OrderTransaction ot = new OrderTransaction(t);
            return AddPaymentTransactionToOrder(o, ot, app);
        }
        public bool AddPaymentTransactionToOrder(Order o, OrderTransaction t, MerchantTribeApplication app)
        {
            // Save Order First if no bvin
            Orders.Upsert(o);            

            t.OrderId = o.bvin;
            t.OrderNumber = o.OrderNumber;
            t.StoreId = o.StoreId;

            if (Transactions.Create(t))
            {
                OrderPaymentStatus previous = o.PaymentStatus;
                EvaluatePaymentStatus(o);
                OnPaymentChanged(previous, o, app);
                return Orders.Update(o);
            }

            return false;
        }
        public OrderPaymentSummary PaymentSummary(Order o)
        {
            OrderPaymentSummary result = new OrderPaymentSummary();
            result.Populate(o, this);
            o.PaymentStatus = EvaluatePaymentStatus(o, result);
            return result;
        }
        public OrderPaymentStatus EvaluatePaymentStatus(Order o)
        {
            OrderPaymentSummary s = new OrderPaymentSummary();
            s.Populate(o, this);
            OrderPaymentStatus result = EvaluatePaymentStatus(o, s);
            s = null;
            return result;
        }
        private OrderPaymentStatus EvaluatePaymentStatus(Order o, OrderPaymentSummary s)
        {
            OrderPaymentStatus result = OrderPaymentStatus.Unknown;

            if (s.AmountDue < 0)
            {
                // Refund Due = Overpaid
                result = OrderPaymentStatus.Overpaid;
            }
            else
            {
                if (s.AmountDue == 0)
                {
                    result = OrderPaymentStatus.Paid;
                }
                else
                {
                    // Amount Due = positive at this point
                    if (s.TotalCredit > 0)
                    {
                        result = OrderPaymentStatus.PartiallyPaid;
                    }
                    else
                    {
                        result = OrderPaymentStatus.Unpaid;
                    }
                }
            }

            o.PaymentStatus = result;
            return result;
        }
        private void OnPaymentChanged(OrderPaymentStatus previousPaymentStatus, Order o, MerchantTribeApplication app)
        {
            BusinessRules.OrderTaskContext context = new BusinessRules.OrderTaskContext(app);
            context.Order = o;
            context.UserId = o.UserID;
            context.Inputs.Add("bvsoftware", "PreviousPaymentStatus", previousPaymentStatus.ToString());
            BusinessRules.Workflow.RunByName(context, BusinessRules.WorkflowNames.PaymentChanged);
        }

        public List<OrderTransaction> FindTransactionsByThirdPartyOrderId(string thirdPartyId)
        {
            List<OrderTransaction> result = new List<OrderTransaction>();

            string oid = string.Empty;
            Order o = Orders.FindByThirdPartyOrderId(thirdPartyId);
            if (o != null)
            {
                result = Transactions.FindForOrder(o.bvin);
            }

            return result;
        }

        public bool OrdersRequestShippingMethod(Shipping.ShippingRateDisplay r, Order o)
		{
			bool result = false;            
			if (r != null) {
                o.ClearShippingPricesAndMethod();
				o.ShippingMethodId = r.ShippingMethodId;
                o.ShippingProviderId = r.ProviderId;
                o.ShippingProviderServiceCode = r.ProviderServiceCode;
				result = true;
			}
			return result;
		}     
        public bool OrdersRequestShippingMethodByUniqueKey(string rateUniqueKey, Order o, Accounts.Store currentStore)
        {
            bool result = false;

            Utilities.SortableCollection<Shipping.ShippingRateDisplay> rates = FindAvailableShippingRates(o, currentStore);
            foreach (Shipping.ShippingRateDisplay r in rates)
            {
                if (r.UniqueKey == rateUniqueKey)
                {
                    return OrdersRequestShippingMethod(r, o);
                }
            }

            return result;
        }

        public bool OrdersDeleteWithInventoryReturn(string orderBvin, Catalog.CatalogService catalogService)
        {
            Order o = Orders.FindForCurrentStore(orderBvin);
            if (o == null) return true;

            // return items to inventory
            foreach(LineItem li in o.Items)
            {
                catalogService.InventoryAdjustAvailableQuantity(li.ProductId, li.VariantId, li.Quantity);                
            }

            return Orders.Delete(orderBvin);
        }

        public long GenerateNewOrderNumber(long storeId)
        {
            return MerchantTribe.Commerce.Orders.OrderNumberGenerator.GenerateNewOrderNumber(storeId);
        }

        public string FindHightlightColorForOrder(OrderSnapshot o)
        {
            string result = "";
            if (o == null) return result;
            if (o.ShippingMethodId.Trim().Length < 1) return result;
            var m = ShippingMethods.Find(o.ShippingMethodId.Trim());
            if (m == null) return result;
            string highlight = m.Settings["highlight"];
            if (highlight.Trim().Length > 0) return highlight;
            return result;
        }

    }
}
