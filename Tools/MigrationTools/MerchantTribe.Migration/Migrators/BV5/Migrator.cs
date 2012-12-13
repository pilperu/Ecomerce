using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using MerchantTribe.CommerceDTO;
using MerchantTribe.CommerceDTO.v1;
using MerchantTribe.CommerceDTO.v1.Catalog;
using MerchantTribe.CommerceDTO.v1.Client;
using MerchantTribe.CommerceDTO.v1.Contacts;
using MerchantTribe.CommerceDTO.v1.Content;
using MerchantTribe.CommerceDTO.v1.Marketing;
using MerchantTribe.CommerceDTO.v1.Membership;
using MerchantTribe.CommerceDTO.v1.Orders;
using MerchantTribe.CommerceDTO.v1.Shipping;
using MerchantTribe.CommerceDTO.v1.Taxes;
using MerchantTribe.Web.Geography;

namespace MerchantTribe.Migration.Migrators.BV5
{
    public class Migrator : IMigrator
    {

        private const int CHUNKSIZE = 131072;
        private MigrationSettings settings = null;
        private data.BV53Entities oldDatabase = null;
        private Dictionary<string, long> AffiliateMapper = new Dictionary<string, long>();
        private Dictionary<string, long> TaxScheduleMapper = new Dictionary<string, long>();
        private List<PropertyMapperInfo> ProductPropertyMapper = new List<PropertyMapperInfo>();

        private List<string> EmptyOptions = new List<String>();

        public event MigrationService.ProgressReportDelegate ProgressReport;
        private void wl(string message)
        {
            if (this.ProgressReport != null)
            {
                this.ProgressReport(message);
            }
        }
        private void Header(string title)
        {
            wl("");
            wl("");
            wl("-----------------------------------------------------------");
            wl("");
            wl("    " + title + " at " + DateTime.UtcNow.ToString());
            wl("");
            wl("-----------------------------------------------------------");
            wl("");
        }
        private string EFConnString(string connString)
        {
            string result = "metadata=res://*/Migrators.BV5.data.OldDatabase.csdl|" +
            "res://*/Migrators.BV5.data.OldDatabase.ssdl|res://*/Migrators.BV5.data.OldDatabase.msl;" +
            "provider=System.Data.SqlClient;provider connection string=\"" +
            connString.TrimEnd('/') +
            "\"";
            return result;
        }
        private void DumpErrors(List<ApiError> errors)
        {
            foreach (ApiError e in errors)
            {
                wl("ERROR: " + e.Code + " | " + e.Description);
            }
        }
        
        private Api GetBV6Proxy()
        {
            Api result = null;
            try
            {
                string serviceUrl = settings.DestinationServiceRootUrl;
                string apiKey = settings.ApiKey;
                result = new Api(serviceUrl, apiKey);
            }
            catch (Exception ex)
            {
                wl("EXCEPTION While attempting to create service proxy for BV 6!");
                wl(ex.Message);
                wl(ex.StackTrace);
            }
            return result;
        }

        public void Migrate(MigrationSettings s)
        {
            wl("");
            wl("BV Commerce 5 Migrator Started");
            wl("");

            settings = s;

            try
            {
                oldDatabase = new data.BV53Entities(EFConnString(s.SourceConnectionString()));
            }
            catch (Exception ex2)
            {
                wl("EXCEPTION While attempting to create old database model!");
                wl(ex2.Message);
                wl(ex2.StackTrace);
                return;
            }


            try
            {

                // Clear Products
                if (s.ClearProducts && s.ImportProductImagesOnly == false)
                {
                    ClearProducts();
                }

                // Clear Categories
                if (s.ClearCategories)
                {
                    ClearCategories();
                }

                // Clear Users
                if (s.ClearUsers)
                {
                    ClearUsers();
                }

                // Users 
                if (s.ImportUsers)
                {
                    //ImportRoles();
                    ImportPriceGroups();
                    ImportUsers();
                }

                // Affiliates
                if (s.ImportAffiliates)
                {
                    ImportAffiliates();
                }

                // Tax Classes are prerequisite for product import
                if (s.ImportOtherSettings || (s.ImportProducts && s.SkipProductPrerequisites == false))
                {
                    ImportTaxSchedules();
                    ImportTaxes();
                }

                // Vendors and Manufacturers
                if ((s.ImportProducts && s.ImportProductImagesOnly == false && s.SkipProductPrerequisites == false) || s.ImportCategories)
                {
                    ImportVendors();
                    ImportManufacturers();
                }

                // Product Types
                if (s.ImportProducts && s.ImportProductImagesOnly == false && s.SkipProductPrerequisites == false)
                {
                    ImportProductProperties();
                    ImportProductTypes();
                }

                // Categories
                if (s.ImportCategories)
                {
                    ImportCategories();
                }

                if (s.ImportProducts)
                {
                    if (s.ImportProductImagesOnly == false && s.SkipProductPrerequisites == false)
                    {
                        ImportProductInputs();
                        ImportProductModifiers();
                        ImportProductChoices();
                    }

                    ImportProducts();
                    MigrateProductFileDownloads();

                    if (s.ImportProductImagesOnly == false)
                    {
                        ImportRelatedItems();
                    }
                }

                if (s.ImportOrders)
                {
                    ImportOrders();
                }

                if (s.ImportOtherSettings)
                {
                    ImportMailingLists();
                    ImportPolicies();
                    ImportFraudData();
                }
            }
            catch (Exception e)
            {
                wl("ERROR: " + e.Message);
                wl(e.StackTrace);
            }
        }

        private void ImportFraudData()
        {
            Header("Importing Fraud Data");
        }

        private void ImportPolicies()
        {
            Header("Importing Policies");
        }

        private void ImportMailingLists()
        {
            Header("Importing Mailing Lists");
        }

        private void ImportOrders()
        {
            Header("Importing Orders");

            if (settings.SingleOrderImport.Trim().Length > 0)
            {
                var o = (from old in oldDatabase.bvc_Order where old.OrderNumber == settings.SingleOrderImport select old).FirstOrDefault();
                if (o == null)
                {
                    wl("Unable to locate order " + settings.SingleOrderImport);
                    return;
                }
                // Single Order Mode
                ImportSingleOrder(o);
            }
            else
            {
                // Multi-Order Mode
                int pageSize = 100;
                int totalRecords = oldDatabase.bvc_Order.Where(y => y.IsPlaced == 1).Count();
                int totalPages = (int)(Math.Ceiling((decimal)totalRecords / (decimal)pageSize));
                for (int i = 0; i < totalPages; i++)
                {
                    wl("Getting Orders page " + (i + 1) + " of " + totalPages.ToString());
                    int startRecord = i * pageSize;
                    var orders = (from order in oldDatabase.bvc_Order where order.IsPlaced == 1 select order).OrderBy(y => y.id).Skip(startRecord).Take(pageSize).ToList();
                    if (orders == null) continue;

                    if (settings.DisableMultiThreading)
                    {
                        foreach (data.bvc_Order o in orders)
                        {
                            ImportSingleOrder(o);
                        }
                    }
                    else
                    {
                        System.Threading.Tasks.Parallel.ForEach(orders, ImportSingleOrder);
                    }
                }
            }
        }
        private void ImportSingleOrder(data.bvc_Order old)
        {
            if (old == null) return;
            wl("Processing Order: " + old.OrderNumber);

            OrderDTO o = new OrderDTO();
            PopulateDto(old, o);
            if (o != null)
            {
                Api proxy = GetBV6Proxy();
                var res = proxy.OrdersCreate(o);
                if (res != null)
                {
                    if (res.Errors.Count > 0)
                    {
                        DumpErrors(res.Errors);
                        wl("FAILED");
                    }
                    else
                    {
                        wl("SUCCESS");
                        ImportOrderTransactions(o.Bvin, o.OrderNumber);
                    }
                }

            }
        }        
        private void PopulateDto(data.bvc_Order old, OrderDTO o)
        {
            o.AffiliateID = old.AffiliateId ?? string.Empty;
            BV5Address oldBilling = new BV5Address();
            oldBilling.FromXmlString(old.BillingAddress);
            if (oldBilling != null)
            {
                oldBilling.CopyTo(o.BillingAddress, EFConnString(settings.SourceConnectionString()));
            }
            o.Bvin = old.Bvin ?? string.Empty;
            o.CustomProperties = TranslateOldProperties(old.CustomProperties);
            o.FraudScore = old.FraudScore;
            o.Id = old.id;
            o.Instructions = old.Instructions;
            o.IsPlaced = old.IsPlaced == 1;
            o.LastUpdatedUtc = old.LastUpdated;
            o.OrderNumber = old.OrderNumber ?? string.Empty;
            if (old.OrderDiscounts != 0)
            {
                o.OrderDiscountDetails.Add(new DiscountDetailDTO() { Amount = -1 * old.OrderDiscounts, Description = "BV 5 Order Discounts", Id = new Guid() });
            }
            o.PaymentStatus = OrderPaymentStatusDTO.Unknown;
            switch (old.PaymentStatus)
            {
                case 0:
                    o.PaymentStatus = OrderPaymentStatusDTO.Unknown;
                    break;
                case 1:
                    o.PaymentStatus = OrderPaymentStatusDTO.Unpaid;
                    break;
                case 2:
                    o.PaymentStatus = OrderPaymentStatusDTO.PartiallyPaid;
                    break;
                case 3:
                    o.PaymentStatus = OrderPaymentStatusDTO.Paid;
                    break;
                case 4:
                    o.PaymentStatus = OrderPaymentStatusDTO.Overpaid;
                    break;
            }
            BV5Address oldShipping = new BV5Address();
            oldShipping.FromXmlString(old.ShippingAddress);
            if (oldShipping != null) oldShipping.CopyTo(o.ShippingAddress, EFConnString(settings.SourceConnectionString()));
            o.ShippingMethodDisplayName = old.ShippingMethodDisplayName;
            o.ShippingMethodId = old.ShippingMethodId;
            o.ShippingProviderId = old.ShippingProviderId;
            o.ShippingProviderServiceCode = old.ShippingProviderServiceCode;
            o.ShippingStatus = OrderShippingStatusDTO.Unknown;
            switch (old.ShippingStatus)
            {
                case 0:
                    o.ShippingStatus = OrderShippingStatusDTO.Unknown;
                    break;
                case 1:
                    o.ShippingStatus = OrderShippingStatusDTO.Unshipped;
                    break;
                case 2:
                    o.ShippingStatus = OrderShippingStatusDTO.PartiallyShipped;
                    break;
                case 3:
                    o.ShippingStatus = OrderShippingStatusDTO.FullyShipped;
                    break;
                case 4:
                    o.ShippingStatus = OrderShippingStatusDTO.NonShipping;
                    break;
            }
            o.StatusCode = old.StatusCode ?? string.Empty;
            o.StatusName = old.StatusName;
            o.ThirdPartyOrderId = old.ThirdPartyOrderId;
            o.TimeOfOrderUtc = old.TimeOfOrder;
            o.TotalHandling = old.HandlingTotal;
            o.TotalShippingBeforeDiscounts = old.ShippingTotal + old.ShippingDiscounts;
            o.ShippingDiscountDetails.Add(new DiscountDetailDTO() { Amount = -1 * old.ShippingDiscounts, Description = "BV5 Shipping Discount", Id = new Guid() });
            o.TotalTax = old.TaxTotal;
            o.TotalTax2 = old.TaxTotal2;
            o.UserEmail = old.UserEmail;
            o.UserID = old.UserId;

            wl(" - Coupons for Order " + old.OrderNumber);
            o.Coupons = TranslateCoupons(o.Bvin);

            wl(" - Items For order " + old.OrderNumber);
            o.Items = TranslateItems(o.Bvin);
            LineItemHelper.SplitTaxesAcrossItems(old.TaxTotal2 + old.TaxTotal, old.SubTotal, o.Items);

            wl(" - Notes For order " + old.OrderNumber);
            o.Notes = TranslateNotes(o.Bvin);

            wl(" - Packages For order " + old.OrderNumber);
            o.Packages = TranslatePackages(o.Bvin);

        }
        private List<OrderCouponDTO> TranslateCoupons(string orderBvin)
        {
            List<OrderCouponDTO> result = new List<OrderCouponDTO>();

            data.BV53Entities db = new data.BV53Entities(EFConnString(settings.SourceConnectionString()));

            var old = db.bvc_OrderCoupon.Where(y => y.OrderBvin == orderBvin);
            if (old == null) return result;

            foreach (data.bvc_OrderCoupon oldCoupon in old)
            {
                OrderCouponDTO c = new OrderCouponDTO();
                c.CouponCode = oldCoupon.CouponCode ?? string.Empty;
                c.IsUsed = true;
                c.OrderBvin = orderBvin ?? string.Empty;
                result.Add(c);
            }

            return result;
        }
        private List<OrderNoteDTO> TranslateNotes(string orderBvin)
        {
            List<OrderNoteDTO> result = new List<OrderNoteDTO>();

            data.BV53Entities db = new data.BV53Entities(EFConnString(settings.SourceConnectionString()));

            var old = db.bvc_OrderNote.Where(y => y.OrderId == orderBvin);
            if (old == null) return result;

            foreach (data.bvc_OrderNote item in old)
            {
                OrderNoteDTO n = new OrderNoteDTO();
                n.AuditDate = item.AuditDate;
                n.IsPublic = item.NoteType == 3;
                n.LastUpdatedUtc = item.LastUpdated;
                n.Note = item.Note ?? string.Empty;
                n.OrderID = orderBvin;
                result.Add(n);
            }

            return result;
        }
        private List<OrderPackageDTO> TranslatePackages(string orderBvin)
        {
            List<OrderPackageDTO> result = new List<OrderPackageDTO>();

            data.BV53Entities db = new data.BV53Entities(EFConnString(settings.SourceConnectionString()));

            var old = db.bvc_OrderPackage.Where(y => y.OrderId == orderBvin);
            if (old == null) return result;

            foreach (data.bvc_OrderPackage item in old)
            {
                OrderPackageDTO pak = new OrderPackageDTO();

                pak.CustomProperties = TranslateOldProperties(item.CustomProperties);
                pak.Description = item.Description ?? string.Empty;
                pak.EstimatedShippingCost = item.EstimatedShippingCost;
                pak.HasShipped = item.HasShipped == 1;
                pak.Height = item.Height;
                pak.Items = TranslateOldPackageItems(item.Items);
                pak.LastUpdatedUtc = item.LastUpdated;
                pak.Length = item.Length;
                pak.OrderId = orderBvin;
                pak.ShipDateUtc = item.ShipDate;
                pak.ShippingMethodId = string.Empty;
                pak.ShippingProviderId = item.ShippingProviderId;
                pak.ShippingProviderServiceCode = item.ShippingProviderServiceCode;
                pak.SizeUnits = LengthTypeDTO.Inches;
                if (item.SizeUnits == 2)
                {
                    pak.SizeUnits = LengthTypeDTO.Centimeters;
                }
                pak.TrackingNumber = item.TrackingNumber;
                pak.Weight = item.Weight;
                pak.WeightUnits = WeightTypeDTO.Pounds;
                if (item.WeightUnits == 2)
                {
                    pak.WeightUnits = WeightTypeDTO.Kilograms;
                }
                pak.Width = item.Width;
                result.Add(pak);
            }

            return result;
        }
        private List<OrderPackageItemDTO> TranslateOldPackageItems(string xml)
        {
            List<OrderPackageItemDTO> result = new List<OrderPackageItemDTO>();

            System.Collections.ObjectModel.Collection<PackageItem> old = PackageItem.FromXml(xml);
            if (old != null)
            {
                foreach (PackageItem p in old)
                {
                    OrderPackageItemDTO pi = new OrderPackageItemDTO();
                    pi.LineItemId = p.LineItemId;
                    pi.ProductBvin = p.ProductBvin;
                    pi.Quantity = p.Quantity;
                    result.Add(pi);
                }
            }
            return result;
        }
        private List<LineItemDTO> TranslateItems(string orderBvin)
        {
            List<LineItemDTO> result = new List<LineItemDTO>();

            data.BV53Entities db = new data.BV53Entities(EFConnString(settings.SourceConnectionString()));

            var old = db.bvc_LineItem.Where(y => y.OrderBvin == orderBvin);
            if (old == null) return result;

            foreach (data.bvc_LineItem item in old)
            {
                LineItemDTO li = new LineItemDTO();

                li.BasePricePerItem = item.BasePrice;
                li.CustomProperties = TranslateOldProperties(item.CustomProperties);
                li.DiscountDetails = new List<DiscountDetailDTO>();
                li.ExtraShipCharge = 0;
                li.Id = -1;
                li.LastUpdatedUtc = item.LastUpdated;
                li.OrderBvin = orderBvin;
                li.ProductId = item.ProductId;
                li.ProductName = item.ProductName;
                li.ProductShortDescription = item.ProductShortDescription;
                li.ProductSku = item.ProductSku;
                li.ProductShippingHeight = 0;
                li.ProductShippingLength = 0;
                li.ProductShippingWeight = 0;
                li.ProductShippingWidth = 0;
                li.Quantity = (int)item.Quantity;
                li.QuantityReturned = (int)item.QuantityReturned;
                li.QuantityShipped = (int)item.QuantityShipped;
                li.SelectionData = new List<OptionSelectionDTO>();
                li.ShipFromAddress = new AddressDTO();
                li.ShipFromMode = ShippingModeDTO.ShipFromSite;
                li.ShipFromNotificationId = string.Empty;
                li.ShippingPortion = 0;
                li.ShippingSchedule = 0;
                li.ShipSeparately = false;
                li.StatusCode = item.StatusCode;
                li.StatusName = item.StatusName;
                li.TaxPortion = 0m;
                li.TaxSchedule = 0;
                li.VariantId = string.Empty;

                // Calculate Adjustments and Discounts
                decimal lineTotal = item.LineTotal;
                decimal prediscountTotal = (li.BasePricePerItem * (decimal)li.Quantity);
                decimal allDiscounts = prediscountTotal - lineTotal;
                if (allDiscounts != 0)
                {
                    li.DiscountDetails.Add(new DiscountDetailDTO() { Amount = -1 * allDiscounts, Description = "BV5 Discounts", Id = new Guid() });
                }

                result.Add(li);
            }

            return result;
        }
        private void ImportOrderTransactions(string orderBvin, string orderNumber)
        {
            wl(" - Transactions for Order " + orderNumber);

            data.BV53Entities db = new data.BV53Entities(EFConnString(settings.SourceConnectionString()));
            Api proxy = GetBV6Proxy();

            var old = db.bvc_OrderPayment.Where(y => y.orderID == orderBvin);
            if (old == null) return;

            foreach (data.bvc_OrderPayment item in old)
            {
                wl("Transaction: " + item.bvin);

                bool hasAuth = item.AmountAuthorized != 0;
                bool hasCharge = item.AmountCharged != 0;
                bool hasRefund = item.AmountRefunded != 0;

                Guid AuthTransactionID = new Guid();
                Guid ChargeTransactionId = new Guid();
                Guid RefundTransactionId = new Guid();

                // Get Guids for transactions
                Guid.TryParse(item.bvin, out ChargeTransactionId);
                if (hasAuth && (hasCharge == false && hasRefund == false))
                {
                    // Auth only, no refund or charge
                    Guid.TryParse(item.bvin, out AuthTransactionID);
                }
                if (hasRefund && (hasCharge == false && hasAuth == false))
                {
                    // Refund only, no auth or charge
                    Guid.TryParse(item.bvin, out RefundTransactionId);
                }

                if (hasAuth)
                {
                    OrderTransactionDTO opAuth = new OrderTransactionDTO();
                    opAuth.Id = AuthTransactionID;
                    switch (item.paymentMethodId)
                    {

                        case "4A807645-4B9D-43f1-BC07-9F233B4E713C": // Credit Card
                            opAuth.Action = OrderTransactionActionDTO.CreditCardHold;
                            break;
                        case "9FD35C50-CDCB-42ac-9549-14119BECBD0C": // Telephone
                            opAuth.Action = OrderTransactionActionDTO.OfflinePaymentRequest;
                            break;
                        case "494A61C8-D7E7-457f-B293-4838EF010C32": // Check
                            opAuth.Action = OrderTransactionActionDTO.OfflinePaymentRequest;
                            break;
                        case "7FCC4B3F-6E67-4f58-86B0-25BCCC035A0E": // Cash
                        case "EE171EFD-9E4A-4eda-AD70-4CB99F28E06C": // COD                            
                            opAuth.Action = OrderTransactionActionDTO.OfflinePaymentRequest;
                            break;
                        case "A0300DBD-39EE-472C-9179-D4B96F27913B": // CredEx
                            opAuth.Action = OrderTransactionActionDTO.CreditCardHold;
                            break;
                        case "26C948F3-22EF-4bcb-9AE9-DEB9839BF4A7": // PO
                            opAuth.Action = OrderTransactionActionDTO.PurchaseOrderInfo;
                            break;
                        case "91a205f1-8c1c-4267-bed0-c8e410e7e680": // Gift Card
                            opAuth.Action = OrderTransactionActionDTO.GiftCardHold;
                            break;
                        case "49de5510-dfe4-4b18-91a6-3dc9925566a1": // Google Checkout
                            opAuth.Action = OrderTransactionActionDTO.CreditCardHold;
                            break;
                        case "33eeba60-e5b7-4864-9b57-3f8d614f8301": // PayPal Express
                            opAuth.Action = OrderTransactionActionDTO.PayPalHold;
                            break;
                        default:
                            opAuth.Action = OrderTransactionActionDTO.CreditCardHold;
                            break;
                    }
                    opAuth.Amount = item.AmountAuthorized;
                    opAuth.CheckNumber = item.checkNumber ?? string.Empty;
                    opAuth.CreditCard = new OrderTransactionCardDataDTO();
                    opAuth.CreditCard.CardHolderName = item.creditCardHolder ?? string.Empty;
                    opAuth.CreditCard.CardIsEncrypted = true;
                    opAuth.CreditCard.CardNumber = string.Empty;
                    opAuth.CreditCard.ExpirationMonth = item.creditCardExpMonth;
                    opAuth.CreditCard.ExpirationYear = item.creditCardExpYear;
                    opAuth.CreditCard.SecurityCode = string.Empty;
                    opAuth.GiftCardNumber = item.giftCertificateNumber ?? string.Empty;
                    opAuth.LinkedToTransaction = string.Empty;
                    opAuth.Messages = item.note ?? string.Empty;
                    opAuth.OrderId = orderBvin ?? string.Empty;
                    opAuth.OrderNumber = orderNumber ?? string.Empty;
                    opAuth.PurchaseOrderNumber = item.purchaseOrderNumber ?? string.Empty;
                    opAuth.RefNum1 = item.transactionReferenceNumber ?? string.Empty;
                    opAuth.RefNum2 = item.transactionResponseCode ?? string.Empty;
                    opAuth.Success = true;
                    opAuth.TimeStampUtc = item.auditDate;
                    opAuth.Voided = false;

                    var res = proxy.OrderTransactionsCreate(opAuth);
                    if (res != null)
                    {
                        if (res.Errors.Count > 0)
                        {
                            DumpErrors(res.Errors);
                            wl("FAILED TRANSACTION: " + item.bvin);
                        }
                    }
                    else
                    {
                        wl("FAILED! EXCEPTION! TRANSACTION: " + item.bvin);
                    }
                }

                if (hasCharge)
                {
                    OrderTransactionDTO opCharge = new OrderTransactionDTO();
                    opCharge.Id = ChargeTransactionId;
                    switch (item.paymentMethodId)
                    {

                        case "4A807645-4B9D-43f1-BC07-9F233B4E713C": // Credit Card
                            opCharge.Action = OrderTransactionActionDTO.CreditCardCharge;
                            break;
                        case "9FD35C50-CDCB-42ac-9549-14119BECBD0C": // Telephone
                            opCharge.Action = OrderTransactionActionDTO.OfflinePaymentRequest;
                            break;
                        case "494A61C8-D7E7-457f-B293-4838EF010C32": // Check
                            opCharge.Action = OrderTransactionActionDTO.CheckReceived;
                            break;
                        case "7FCC4B3F-6E67-4f58-86B0-25BCCC035A0E": // Cash
                        case "EE171EFD-9E4A-4eda-AD70-4CB99F28E06C": // COD                            
                            opCharge.Action = OrderTransactionActionDTO.CashReceived;
                            break;
                        case "A0300DBD-39EE-472C-9179-D4B96F27913B": // CredEx
                            opCharge.Action = OrderTransactionActionDTO.CashReceived;
                            break;
                        case "26C948F3-22EF-4bcb-9AE9-DEB9839BF4A7": // PO
                            opCharge.Action = OrderTransactionActionDTO.PurchaseOrderAccepted;
                            break;
                        case "91a205f1-8c1c-4267-bed0-c8e410e7e680": // Gift Card
                            opCharge.Action = OrderTransactionActionDTO.GiftCardCapture;
                            break;
                        case "49de5510-dfe4-4b18-91a6-3dc9925566a1": // Google Checkout
                            opCharge.Action = OrderTransactionActionDTO.CreditCardCharge;
                            break;
                        case "33eeba60-e5b7-4864-9b57-3f8d614f8301": // PayPal Express
                            opCharge.Action = OrderTransactionActionDTO.PayPalCharge;
                            break;
                        default:
                            opCharge.Action = OrderTransactionActionDTO.CreditCardCharge;
                            break;
                    }
                    opCharge.Amount = item.AmountCharged;
                    opCharge.CheckNumber = item.checkNumber ?? string.Empty;
                    opCharge.CreditCard = new OrderTransactionCardDataDTO();
                    opCharge.CreditCard.CardHolderName = item.creditCardHolder ?? string.Empty;
                    opCharge.CreditCard.CardIsEncrypted = true;
                    opCharge.CreditCard.CardNumber = string.Empty;
                    opCharge.CreditCard.ExpirationMonth = item.creditCardExpMonth;
                    opCharge.CreditCard.ExpirationYear = item.creditCardExpYear;
                    opCharge.CreditCard.SecurityCode = string.Empty;
                    opCharge.GiftCardNumber = item.giftCertificateNumber ?? string.Empty;
                    opCharge.LinkedToTransaction = string.Empty;
                    opCharge.Messages = item.note ?? string.Empty;
                    opCharge.OrderId = orderBvin ?? string.Empty;
                    opCharge.OrderNumber = orderNumber ?? string.Empty;
                    opCharge.PurchaseOrderNumber = item.purchaseOrderNumber ?? string.Empty;
                    opCharge.RefNum1 = item.transactionReferenceNumber ?? string.Empty;
                    opCharge.RefNum2 = item.transactionResponseCode ?? string.Empty;
                    opCharge.Success = true;
                    opCharge.TimeStampUtc = item.auditDate;
                    opCharge.Voided = false;

                    var res = proxy.OrderTransactionsCreate(opCharge);
                    if (res != null)
                    {
                        if (res.Errors.Count > 0)
                        {
                            DumpErrors(res.Errors);
                            wl("FAILED TRANSACTION: " + item.bvin);
                        }
                    }
                    else
                    {
                        wl("FAILED! EXCEPTION! TRANSACTION: " + item.bvin);
                    }
                }

                if (hasRefund)
                {
                    OrderTransactionDTO opRefund = new OrderTransactionDTO();
                    opRefund.Id = RefundTransactionId;
                    switch (item.paymentMethodId)
                    {

                        case "4A807645-4B9D-43f1-BC07-9F233B4E713C": // Credit Card
                            opRefund.Action = OrderTransactionActionDTO.CreditCardRefund;
                            break;
                        case "9FD35C50-CDCB-42ac-9549-14119BECBD0C": // Telephone
                            opRefund.Action = OrderTransactionActionDTO.OfflinePaymentRequest;
                            break;
                        case "494A61C8-D7E7-457f-B293-4838EF010C32": // Check
                            opRefund.Action = OrderTransactionActionDTO.CheckReturned;
                            break;
                        case "7FCC4B3F-6E67-4f58-86B0-25BCCC035A0E": // Cash
                        case "EE171EFD-9E4A-4eda-AD70-4CB99F28E06C": // COD                            
                            opRefund.Action = OrderTransactionActionDTO.CashReturned;
                            break;
                        case "A0300DBD-39EE-472C-9179-D4B96F27913B": // CredEx
                            opRefund.Action = OrderTransactionActionDTO.CashReturned;
                            break;
                        case "26C948F3-22EF-4bcb-9AE9-DEB9839BF4A7": // PO
                            opRefund.Action = OrderTransactionActionDTO.CashReturned;
                            break;
                        case "91a205f1-8c1c-4267-bed0-c8e410e7e680": // Gift Card
                            opRefund.Action = OrderTransactionActionDTO.GiftCardIncrease;
                            break;
                        case "49de5510-dfe4-4b18-91a6-3dc9925566a1": // Google Checkout
                            opRefund.Action = OrderTransactionActionDTO.CreditCardRefund;
                            break;
                        case "33eeba60-e5b7-4864-9b57-3f8d614f8301": // PayPal Express
                            opRefund.Action = OrderTransactionActionDTO.PayPalRefund;
                            break;
                        default:
                            opRefund.Action = OrderTransactionActionDTO.CreditCardRefund;
                            break;
                    }
                    opRefund.Amount = -1 * item.AmountCharged;
                    opRefund.CheckNumber = item.checkNumber ?? string.Empty;
                    opRefund.CreditCard = new OrderTransactionCardDataDTO();
                    opRefund.CreditCard.CardHolderName = item.creditCardHolder ?? string.Empty;
                    opRefund.CreditCard.CardIsEncrypted = true;
                    opRefund.CreditCard.CardNumber = string.Empty;
                    opRefund.CreditCard.ExpirationMonth = item.creditCardExpMonth;
                    opRefund.CreditCard.ExpirationYear = item.creditCardExpYear;
                    opRefund.CreditCard.SecurityCode = string.Empty;
                    opRefund.GiftCardNumber = item.giftCertificateNumber ?? string.Empty;
                    opRefund.LinkedToTransaction = string.Empty;
                    opRefund.Messages = item.note ?? string.Empty;
                    opRefund.OrderId = orderBvin ?? string.Empty;
                    opRefund.OrderNumber = orderNumber ?? string.Empty;
                    opRefund.PurchaseOrderNumber = item.purchaseOrderNumber ?? string.Empty;
                    opRefund.RefNum1 = item.transactionReferenceNumber ?? string.Empty;
                    opRefund.RefNum2 = item.transactionResponseCode ?? string.Empty;
                    opRefund.Success = true;
                    opRefund.TimeStampUtc = item.auditDate;
                    opRefund.Voided = false;

                    var res = proxy.OrderTransactionsCreate(opRefund);
                    if (res != null)
                    {
                        if (res.Errors.Count > 0)
                        {
                            DumpErrors(res.Errors);
                            wl("FAILED TRANSACTION: " + item.bvin);
                        }
                    }
                    else
                    {
                        wl("FAILED! EXCEPTION! TRANSACTION: " + item.bvin);
                    }
                }



            }
        }

        private void ImportRelatedItems()
        {
            Header("Importing Related Items");

            data.BV53Entities db = new data.BV53Entities(EFConnString(settings.SourceConnectionString()));
            Api proxy = GetBV6Proxy();

            var crosses = db.bvc_ProductCrossSell;
            if (crosses == null) return;
            foreach (data.bvc_ProductCrossSell x in crosses)
            {
                wl("Relating " + x.ProductBvin + " to " + x.CrossSellBvin);
                proxy.ProductRelationshipsQuickCreate(x.ProductBvin, x.CrossSellBvin, false);
            }

            var ups = db.bvc_ProductUpSell;
            if (ups == null) return;
            foreach (data.bvc_ProductUpSell up in ups)
            {
                wl("Relating Up " + up.ProductBvin + " to " + up.UpSellBvin);
                proxy.ProductRelationshipsQuickCreate(up.ProductBvin, up.UpSellBvin, true);
            }
        }

        private void ImportProducts()
        {
            Header("Importing Products");

            int limit = -1;
            if (settings.SingleProductMode == true)
            {
                limit = 1;
            }
            if (settings.ImportProductLimit > 0)
            {
                limit = settings.ImportProductLimit;
            }
            int totalMigrated = 0;

            int pageSize = 10;
            int totalRecords = oldDatabase.bvc_Product.Count();
            if (settings.SingleProductMode == true)
            {
                totalRecords = 1;
            }
            int totalPages = (int)(Math.Ceiling((decimal)totalRecords / (decimal)pageSize));

            int startPage = settings.ProductStartPage;
            if (startPage < 1) startPage = 1;
            if (startPage > totalPages) startPage = totalPages;

            for (int i = (startPage - 1); i < totalPages; i++)
            {
                wl("Getting Products page " + (i + 1) + " of " + totalPages.ToString());
                int startRecord = i * pageSize;
                var products = (from p in oldDatabase.bvc_Product select p).OrderBy(y => y.id).Skip(startRecord).Take(pageSize).ToList();

                if (settings.SingleProductMode == true)
                {
                    var singleProd = (from p in oldDatabase.bvc_Product select p).Where(y => y.SKU == settings.SingleSkuImport).FirstOrDefault();
                    if (singleProd == null)
                    {
                        wl("!! Missing Product for Import SKU: " + settings.SingleSkuImport);
                        return;
                    }
                    ImportSingleProduct(singleProd);
                    return;
                }
                else if (settings.DisableMultiThreading)
                {
                    foreach (data.bvc_Product p in products)
                    {
                       
                        ImportSingleProduct(p);

                        totalMigrated += 1;
                        if (limit > 0 && totalMigrated >= limit)
                        {
                            return;
                        }
                    }
                }
                else
                {
                    System.Threading.Tasks.Parallel.ForEach(products, ImportSingleProduct);
                }

            }
        }
        private List<CustomPropertyDTO> TranslateOldProperties(string oldXml)
        {
            List<CustomPropertyDTO> result = new List<CustomPropertyDTO>();

            CustomPropertyCollection props = CustomPropertyCollection.FromXml(oldXml);
            if (props != null)
            {
                foreach (CustomProperty prop in props)
                {
                    result.Add(prop.ToDto());
                }
            }
            return result;
        }
        private void ImportSingleProduct(data.bvc_Product old)
        {
            if (old == null) return;

            if (old.ParentID.Trim() != string.Empty)
            {
                wl("Skipping product [" + old.SKU + "] because it is a variant");
                return;
            }

            wl("Product: " + old.ProductName + " [" + old.SKU + "]");
            ProductDTO p = new ProductDTO();
            p.AllowReviews = true;
            p.Bvin = old.bvin;
            p.CreationDateUtc = old.CreationDate;
            p.CustomProperties = TranslateOldProperties(old.CustomProperties);
            p.Featured = false;
            p.GiftWrapAllowed = true;
            p.GiftWrapPrice = 0;
            p.ImageFileSmall = TextHelper.CleanFileName(System.IO.Path.GetFileName(old.ImageFileMedium));
            p.ImageFileSmallAlternateText = old.MediumImageAlternateText;
            p.InventoryMode = ProductInventoryModeDTO.AlwayInStock;
            if (old.TrackInventory == 1) p.InventoryMode = ProductInventoryModeDTO.WhenOutOfStockShow;
            p.IsAvailableForSale = true;
            p.Keywords = old.Keywords;
            p.ListPrice = old.ListPrice;
            p.LongDescription = old.LongDescription;
            p.ManufacturerId = old.ManufacturerID;
            p.MetaDescription = TextHelper.RemoveHtmlTags(old.MetaDescription, " ");
            p.MetaKeywords = TextHelper.RemoveHtmlTags(old.MetaKeywords, " ");
            p.MetaTitle = TextHelper.RemoveHtmlTags(old.MetaTitle, " ");
            p.MinimumQty = old.MinimumQty;
            p.PostContentColumnId = string.Empty;
            p.PreContentColumnId = string.Empty;
            p.PreTransformLongDescription = old.PreTransformLongDescription;
            p.ProductName = TextHelper.RemoveHtmlTags(old.ProductName, " ");
            p.ProductTypeId = old.ProductTypeId;            
            p.ShippingDetails = new ShippableItemDTO();
            p.ShippingDetails.ExtraShipFee = old.ExtraShipFee;
            p.ShippingDetails.Height = old.ShippingHeight;
            p.ShippingDetails.IsNonShipping = old.NonShipping == 1;
            p.ShippingDetails.Length = old.ShippingLength;
            p.ShippingDetails.ShippingScheduleId = 0;
            p.ShippingDetails.ShipSeparately = old.ShipSeparately == 1;
            p.ShippingDetails.Weight = old.ShippingWeight;
            p.ShippingDetails.Width = old.ShippingWidth;
            switch (old.ShippingMode)
            {
                case 1:
                    p.ShippingMode = ShippingModeDTO.ShipFromSite;
                    break;
                case 2:
                    p.ShippingMode = ShippingModeDTO.ShipFromVendor;
                    break;
                case 3:
                    p.ShippingMode = ShippingModeDTO.ShipFromManufacturer;
                    break;
                default:
                    p.ShippingMode = ShippingModeDTO.ShipFromSite;
                    break;
            }
            p.ShortDescription = old.ShortDescription;
            p.SiteCost = old.SiteCost;
            p.SitePrice = old.SitePrice;
            p.SitePriceOverrideText = old.SitePriceOverrideText;
            p.Sku = old.SKU;
            switch (old.Status)
            {
                case 0:
                    p.Status = ProductStatusDTO.Disabled;
                    break;
                default:
                    p.Status = ProductStatusDTO.Active;
                    break;
            }
            p.Tabs = new List<ProductDescriptionTabDTO>();
            p.TaxExempt = old.TaxExempt == 1;
            p.TaxSchedule = 0;
            if (TaxScheduleMapper.ContainsKey(old.TaxClass)) p.TaxSchedule = TaxScheduleMapper[old.TaxClass];
            p.VendorId = old.VendorID;
            p.UrlSlug = GetCustomUrlSlug(old.bvin);

            byte[] bytes = GetBytesForLocalImage(old.ImageFileMedium);
            if (bytes != null)
            {
                wl("Found Image: " + p.ImageFileSmall + " [" + FriendlyFormatBytes(bytes.Length) + "]");
            }
            else
            {
                wl("Missing Image: " + p.ImageFileSmall);
                bytes = new byte[0];
            }


            Api proxy = GetBV6Proxy();
            
            ApiResponse<ProductDTO> res = null;
            //if (settings.ImportProductImagesOnly == true)
            //{
            //    proxy.ProductsMainImageUpload(old.bvin, p.ImageFileSmall, bytes);
            //    res = new ApiResponse<ProductDTO>();
            //    res.Errors.Clear();
            //}
            //else
            //{
                res = proxy.ProductsCreate(p, bytes);
            //}
            if (res != null)
            {
                if (res.Errors.Count > 0)
                {
                    DumpErrors(res.Errors);
                    wl("FAILED");
                }
                else
                {
                    if (settings.ImportProductImagesOnly == false)
                    {
                        AssignOptionsToProduct(old.bvin);
                        AssignProductPropertyValues(old.bvin);
                    }
                    wl("SUCCESS");
                }
            }


            if (settings.ImportProductImagesOnly == false)
            {
                // Inventory                        
                MigrateProductInventory(old.bvin);
            }

            // Additional Images
            MigrateProductAdditionalImages(old.bvin);

            if (settings.ImportProductImagesOnly == false)
            {
                // Volume Prices
                MigrateProductVolumePrices(old.bvin);

                // Reviews
                MigrateProductReviews(old.bvin);

                // Link to Categories
                MigrateProductCategoryLinks(old.bvin);
            }
        }
        private string GetCustomUrlSlug(string oldBvin)
        {
            wl(" - Getting Custom Url - ");
            data.BV53Entities db = new data.BV53Entities(EFConnString(settings.SourceConnectionString()));
            var customUrl = db.bvc_CustomUrl.Where(y => y.SystemUrl == 1).Where(y => y.SystemData == oldBvin).FirstOrDefault();
            if (customUrl != null)
            {
                wl("using URL: " + customUrl.RequestedUrl + " for " + oldBvin);
                return customUrl.RequestedUrl.TrimStart('/');
            }
            wl("No custom URL Found for " + oldBvin);
            return string.Empty;
        }
        private void AssignOptionsToProduct(string bvin)
        {
            wl(" - Migrating Options...");

            data.BV53Entities db = new data.BV53Entities(EFConnString(settings.SourceConnectionString()));


            Api proxy = GetBV6Proxy();

            var choices = db.bvc_ProductXChoice.Where(y => y.ProductId == bvin);
            if (choices == null) return;
            foreach (data.bvc_ProductXChoice choice in choices)
            {
                if (this.EmptyOptions.Contains(choice.ChoiceId)) continue; // Skip already assigned
                proxy.ProductOptionsAssignToProduct(choice.ChoiceId, bvin, false);
            }

            var modifiers = db.bvc_ProductXModifier.Where(y => y.ProductId == bvin);
            if (modifiers == null) return;
            foreach (data.bvc_ProductXModifier mod in modifiers)
            {
                if (this.EmptyOptions.Contains(mod.ModifierId)) continue; // Skip already assigned
                proxy.ProductOptionsAssignToProduct(mod.ModifierId, bvin, false);
            }

            var inputs = db.bvc_ProductXInput.Where(y => y.ProductId == bvin);
            if (inputs == null) return;
            foreach (data.bvc_ProductXInput input in inputs)
            {
                proxy.ProductOptionsAssignToProduct(input.InputId, bvin, false);
            }

            // Only generate variants after all options are added. Saves Time
            proxy.ProductOptionsGenerateAllVariants(bvin);

            // Now Assign Variant Skus
            var combos = db.bvc_ProductChoiceCombinations.Where(y => y.ParentProductId == bvin).ToList();
            if (combos == null) return;
            if (combos.Count < 1)
            {
                wl("No Combos Found.");
                return;
            }
            else
            {
                wl(combos.Count + " combos found");
            }
            var children = db.bvc_Product.Where(y => y.ParentID == bvin).ToList();
            if (children == null) return;
            if (children.Count < 1)
            {
                wl("No children found.");
                return;
            }
            else
            {
                wl(children.Count + " children found to update skus");
            }

            int childCounter = 0;
            foreach (var child in children)
            {
                childCounter++;
                wl("Updating Child Variant Sku " + childCounter);
                var dto = new ProductVariantSkuUpdateDTO();
                dto.Sku = child.SKU.Trim();
                dto.ProductBvin = bvin;

                if (dto.Sku.Length > 0)
                {
                    var matchingChoices = combos.Where(y => y.ProductId == child.bvin).ToList();
                    foreach (var match in matchingChoices)
                    {
                        dto.MatchingOptions.Add(new VariantOptionDataDTO()
                        {
                            ChoiceId = match.ChoiceId,
                            ChoiceItemId = match.ChoiceOptionId
                        });
                    }
                }
                
                wl("SKU: " + dto.Sku + " of parent " + dto.ProductBvin + " matches ");
                foreach (var match in dto.MatchingOptions)
                {
                    wl("Choice: " + match.ChoiceId + " Option:" + match.ChoiceItemId);
                }

                proxy.ProductVariantUpdateSku(dto);
            }            
        }
        private void AssignProductPropertyValues(string bvin)
        {
            wl(" - Migrating Property Values...");

            data.BV53Entities db = new data.BV53Entities(EFConnString(settings.SourceConnectionString()));
            Api proxy = GetBV6Proxy();

            var items = db.bvc_ProductPropertyValue.Where(y => y.ProductBvin == bvin);
            if (items == null) return;
            foreach (data.bvc_ProductPropertyValue item in items)
            {
                var map = ProductPropertyMapper.Where(y => y.OldBvin == item.PropertyBvin).FirstOrDefault();
                if (map == null)
                {
                    wl("!!Missing Map for old property bvin of " + item.PropertyBvin);
                    continue;
                }

                string newPropertyValue = item.PropertyValue;
                long newChoiceId = -1;

                switch (map.PropertyType)
                {
                    case ProductPropertyTypeDTO.MultipleChoiceField:
                        newChoiceId = FindNewChoiceId(map, item.PropertyValue);
                        newPropertyValue = newChoiceId.ToString();                        
                        break;
                    default:
                        newPropertyValue = item.PropertyValue;
                        break;
                }
                
                long newId = map.NewBvin;                
                if (newId > 0)
                {
                    proxy.ProductPropertiesSetValueForProduct(newId, bvin, newPropertyValue, newChoiceId);
                }
            }
        }
        private long FindNewChoiceId(PropertyMapperInfo map, string oldPropertyValue)
        {
            if (map == null) return -1;
            if (map.Choices == null) return -1;
            var matchingChoice = map.Choices.Where(y => y.OldBvin == oldPropertyValue).FirstOrDefault();
            if (matchingChoice == null) return -1;
            return matchingChoice.NewBvin;
        }

        private void MigrateProductInventory(string bvin)
        {
            wl(" - Migrating Inventory...");

            data.BV53Entities db = new data.BV53Entities(EFConnString(settings.SourceConnectionString()));
            Api proxy = GetBV6Proxy();

            var old = db.bvc_ProductInventory.Where(y => y.ProductBvin == bvin).FirstOrDefault();
            if (old == null) return;

            ProductInventoryDTO inv = new ProductInventoryDTO();
            inv.LowStockPoint = (int)(old.ReorderLevel ?? 0);
            inv.ProductBvin = bvin;
            inv.QuantityOnHand = (int)((old.QuantityAvailableForSale ?? 0) + old.QuantityReserved);
            inv.QuantityReserved = (int)old.QuantityReserved;

            var res = proxy.ProductInventoryCreate(inv);
            if (res != null)
            {
                if (res.Errors.Count > 0)
                {
                    DumpErrors(res.Errors);
                    wl("FAILED");
                }
            }
            else
            {
                wl("FAILED! EXCEPTION!");
            }
        }
        private void MigrateProductAdditionalImages(string bvin)
        {
            wl(" - Migrating AdditionalImages...");

            data.BV53Entities db = new data.BV53Entities(EFConnString(settings.SourceConnectionString()));
            Api proxy = GetBV6Proxy();

            var items = db.bvc_ProductImage.Where(y => y.ProductID == bvin);
            if (items == null) return;
            foreach (data.bvc_ProductImage old in items)
            {
                ProductImageDTO img = new ProductImageDTO();
                img.AlternateText = old.AlternateText;
                img.Bvin = old.bvin;
                img.Caption = old.Caption;
                img.FileName = TextHelper.CleanFileName(System.IO.Path.GetFileName(old.FileName));
                img.LastUpdatedUtc = old.LastUpdated;
                img.ProductId = old.ProductID;
                img.SortOrder = old.SortOrder;

                byte[] bytes = GetBytesForLocalImage(old.FileName);
                if (bytes == null) return;
                wl("Found Image: " + img.FileName + " [" + bytes.Length + " bytes]");

                var res = proxy.ProductImagesCreate(img, bytes);
                if (res != null)
                {
                    if (res.Errors.Count > 0)
                    {
                        DumpErrors(res.Errors);
                        wl("FAILED");
                    }
                }
                else
                {
                    wl("FAILED! EXCEPTION!");
                }
            }
        }
        private void MigrateProductFileDownloads()
        {
            Header("Migrating File Downloads");

            data.BV53Entities db = new data.BV53Entities(EFConnString(settings.SourceConnectionString()));
            Api proxy = GetBV6Proxy();

            var items = db.bvc_ProductFile;
            if (items == null) return;
            foreach (data.bvc_ProductFile old in items)
            {
                wl("File: " + old.FileName);

                string safeFileName = "\\files\\" + old.bvin + "_" + old.FileName + ".config";

                byte[] bytes = GetBytesForLocalImage(safeFileName);
                if (bytes == null)
                {
                    wl("Missing File: " + old.FileName);
                    continue;
                }
                else
                {
                    wl("Found File: " + old.FileName + " [" + FriendlyFormatBytes(bytes.Length) + "]");
                }

                int totalChunks = 0;
                byte[] partial = null;
                if (bytes != null && bytes.Length > 0)
                {
                    double ChunkCount = 0;
                    ChunkCount = (double)bytes.Length / (double)CHUNKSIZE;
                    ChunkCount = Math.Ceiling(ChunkCount);
                    totalChunks = (int)ChunkCount;
                    if (totalChunks > 0)
                    {
                        partial = GetAChunkFromFullBytes(bytes, 0);
                    }
                }

                var res = proxy.ProductFilesDataUploadFirstPart(old.bvin, old.FileName, old.ShortDescription, partial);
                if (res != null)
                {
                    if (res.Errors.Count > 0)
                    {
                        DumpErrors(res.Errors);
                        wl("FAILED");
                    }

                    wl("File Created");
                    if (totalChunks > 1)
                    {
                        wl("Uploading: ");
                        for (int i = 1; i < totalChunks; i++)
                        {
                            partial = GetAChunkFromFullBytes(bytes, i);
                            wl("+ " + old.FileName + " [" + FriendlyFormatBytes(bytes.Length) + "] part " + (i + 1) + " of " + totalChunks.ToString());
                            var res2 = proxy.ProductFilesDataUploadAdditionalPart(old.bvin, old.FileName, partial);
                        }
                    }
                    wl("File Done Uploading!");
                }
                else
                {
                    wl("FAILED! EXCEPTION!");
                }
            }

            var crosses = db.bvc_ProductFileXProduct;
            if (crosses == null) return;
            foreach (data.bvc_ProductFileXProduct x in crosses)
            {
                wl("Linking Product " + x.ProductId + " to " + x.ProductFileId);
                var res2 = proxy.ProductFilesAddToProduct(x.ProductId, x.ProductFileId, x.AvailableMinutes, x.MaxDownloads);
                if (res2 != null)
                {
                    if (res2.Errors.Count > 0)
                    {
                        DumpErrors(res2.Errors);
                        wl("FAILED");
                    }
                    else
                    {
                        wl("SUCCESS");
                    }
                }
                else
                {
                    wl("FAILED! EXCEPTION!");
                }
            }
        }
        private byte[] GetAChunkFromFullBytes(byte[] fullBytes, int chunkIndex)
        {
            // Get A Chunk
            byte[] chunk = null;

            try
            {
                int startIndex = (chunkIndex) * CHUNKSIZE;
                if (startIndex + CHUNKSIZE > fullBytes.Length)
                {
                    chunk = new byte[fullBytes.Length - startIndex];
                }
                else
                {
                    chunk = new byte[CHUNKSIZE];
                }

                Array.Copy(fullBytes, startIndex, chunk, 0, chunk.Length);
            }
            catch (Exception ex)
            {
                wl("EXCEPTION: " + ex.Message + " | " + ex.StackTrace);
            }
            return chunk;
        }
        private string FriendlyFormatBytes(long sizeInBytes)
        {
            if (sizeInBytes < 1024)
            {
                return sizeInBytes + " bytes";
            }
            else
            {
                if (sizeInBytes < 1048576)
                {
                    return Math.Round((double)sizeInBytes / 1024, 1) + " KB";
                }
                else
                {
                    return Math.Round((double)sizeInBytes / 1048576, 1) + " MB";
                }
            }
        }
        private void MigrateProductVolumePrices(string bvin)
        {
            wl(" - Migrating Volume Prices...");

            data.BV53Entities db = new data.BV53Entities(EFConnString(settings.SourceConnectionString()));
            Api proxy = GetBV6Proxy();

            var items = db.bvc_ProductVolumeDiscounts.Where(y => y.ProductID == bvin);
            if (items == null) return;
            foreach (data.bvc_ProductVolumeDiscounts item in items)
            {
                ProductVolumeDiscountDTO v = new ProductVolumeDiscountDTO();
                v.Amount = item.Amount;
                v.Bvin = item.bvin;
                switch (item.DiscountType)
                {
                    case 1:
                        v.DiscountType = ProductVolumeDiscountTypeDTO.Percentage;
                        break;
                    case 2:
                        v.DiscountType = ProductVolumeDiscountTypeDTO.Amount;
                        break;
                }
                v.DiscountType = (ProductVolumeDiscountTypeDTO)item.DiscountType;
                v.LastUpdated = item.LastUpdated;
                v.ProductId = item.ProductID;
                v.Qty = item.Qty;

                wl("Discount for qty: " + v.Qty + " [" + v.Bvin + "]");
                var res = proxy.ProductVolumeDiscountsCreate(v);
                if (res != null)
                {
                    if (res.Errors.Count > 0)
                    {
                        DumpErrors(res.Errors);
                        wl("FAILED");
                    }
                }
                else
                {
                    wl("FAILED! EXCEPTION!");
                }
            }
        }
        private void MigrateProductReviews(string bvin)
        {
            wl(" - Migrating Reviews...");

            data.BV53Entities db = new data.BV53Entities(EFConnString(settings.SourceConnectionString()));
            Api proxy = GetBV6Proxy();

            var items = db.bvc_ProductReview.Where(y => y.ProductBvin == bvin);
            if (items == null) return;
            foreach (data.bvc_ProductReview item in items)
            {
                ProductReviewDTO r = new ProductReviewDTO();
                r.Approved = item.Approved == 1;
                r.Bvin = item.bvin;
                r.Description = item.Description;
                r.Karma = item.Karma;
                r.ProductBvin = item.ProductBvin;
                switch (item.Rating)
                {
                    case 0:
                        r.Rating = ProductReviewRatingDTO.ZeroStars;
                        break;
                    case 1:
                        r.Rating = ProductReviewRatingDTO.OneStar;
                        break;
                    case 2:
                        r.Rating = ProductReviewRatingDTO.TwoStars;
                        break;
                    case 3:
                        r.Rating = ProductReviewRatingDTO.ThreeStars;
                        break;
                    case 4:
                        r.Rating = ProductReviewRatingDTO.FourStars;
                        break;
                    case 5:
                        r.Rating = ProductReviewRatingDTO.FiveStars;
                        break;
                }
                r.ReviewDateUtc = item.ReviewDate;
                r.UserID = item.UserID;

                wl("Review [" + r.Bvin + "]");
                var res = proxy.ProductReviewsCreate(r);
                if (res != null)
                {
                    if (res.Errors.Count > 0)
                    {
                        DumpErrors(res.Errors);
                        wl("FAILED");
                    }
                }
                else
                {
                    wl("FAILED! EXCEPTION!");
                }
            }
        }
        private void MigrateProductCategoryLinks(string bvin)
        {
            wl(" - Migrating Category Links");

            data.BV53Entities db = new data.BV53Entities(EFConnString(settings.SourceConnectionString()));
            Api proxy = GetBV6Proxy();

            var items = db.bvc_ProductXCategory.Where(y => y.ProductId == bvin);
            if (items == null) return;
            foreach (data.bvc_ProductXCategory item in items)
            {
                wl("To Category: " + item.CategoryId);
                var res = proxy.CategoryProductAssociationsQuickCreate(bvin, item.CategoryId);
                if (res != null)
                {
                    if (res.Errors.Count > 0)
                    {
                        DumpErrors(res.Errors);
                        wl("FAILED");
                    }
                }
                else
                {
                    wl("FAILED! EXCEPTION!");
                }
            }
        }

        // Choices, Modifiers, Inputs
        private void ImportProductInputs()
        {
            Header("Importing Product Inputs");

            foreach (data.bvc_ProductInputs oldInput in oldDatabase.bvc_ProductInputs)
            {


                OptionDTO input = new OptionDTO();
                string fullName = oldInput.InputName;
                if (oldInput.InputDisplayName.Trim().Length > 0) fullName = oldInput.InputDisplayName;

                input.Settings = new List<OptionSettingDTO>();
                input.Items = new List<OptionItemDTO>();
                input.Bvin = oldInput.bvin;
                input.IsShared = oldInput.SharedInput;
                input.IsVariant = false; // Inputs can't be variants

                input.NameIsHidden = false;
                switch (oldInput.InputType.Trim().ToLowerInvariant())
                {
                    case "html area":
                        input.OptionType = OptionTypesDTO.Html;
                        BV5OptionHtmlSettings htmlSettings = GetOptionSettingsHtml(input.Bvin);
                        input.Settings.Add(new OptionSettingDTO() { Key = "html", Value = htmlSettings.HtmlData });
                        break;
                    default: // Text Input
                        input.OptionType = OptionTypesDTO.TextInput;
                        BV5OptionTextSettings textSettings = GetOptionSettingsText(input.Bvin);
                        input.Settings.Add(new OptionSettingDTO() { Key = "rows", Value = textSettings.Rows });
                        input.Settings.Add(new OptionSettingDTO() { Key = "cols", Value = textSettings.Columns });
                        input.Settings.Add(new OptionSettingDTO() { Key = "required", Value = textSettings.Required });
                        input.Settings.Add(new OptionSettingDTO() { Key = "wraptext", Value = textSettings.WrapText });
                        input.Settings.Add(new OptionSettingDTO() { Key = "maxlength", Value = "255" });
                        if (textSettings.DisplayName.Trim().Length > 0) fullName = textSettings.DisplayName;
                        break;
                }
                input.Name = fullName;
                wl("Input: " + fullName);

                Api bv6proxy = GetBV6Proxy();
                var res = bv6proxy.ProductOptionsCreate(input);
                if (res != null)
                {
                    if (res.Errors.Count() > 0)
                    {
                        DumpErrors(res.Errors);
                        wl("FAILED");
                    }
                    else
                    {
                        wl("SUCCESS");
                    }
                }
            }
        }
        private BV5OptionTextSettings GetOptionSettingsText(string bvin)
        {
            BV5OptionTextSettings result = new BV5OptionTextSettings();

            data.BV53Entities db = new data.BV53Entities(EFConnString(settings.SourceConnectionString()));
            var componentSettings = db.bvc_ComponentSetting.Where(y => y.ComponentID == bvin).OrderBy(y => y.SettingName);
            if (componentSettings == null) return result;

            foreach (data.bvc_ComponentSetting cs in componentSettings)
            {
                switch (cs.SettingName.Trim().ToLowerInvariant())
                {
                    case "columns":
                        result.Columns = cs.SettingValue;
                        break;
                    case "rows":
                        result.Rows = cs.SettingValue;
                        break;
                    case "displayname":
                        result.DisplayName = cs.SettingValue;
                        break;
                    case "required":
                        result.Required = cs.SettingValue;
                        break;
                    case "wraptext":
                        result.WrapText = cs.SettingValue;
                        break;
                }
            }
            return result;
        }
        private BV5OptionHtmlSettings GetOptionSettingsHtml(string bvin)
        {
            BV5OptionHtmlSettings result = new BV5OptionHtmlSettings();

            data.BV53Entities db = new data.BV53Entities(EFConnString(settings.SourceConnectionString()));
            var componentSettings = db.bvc_ComponentSetting.Where(y => y.ComponentID == bvin).OrderBy(y => y.SettingName);
            if (componentSettings == null) return result;

            foreach (data.bvc_ComponentSetting cs in componentSettings)
            {
                switch (cs.SettingName.Trim().ToLowerInvariant())
                {
                    case "htmldata":
                        result.HtmlData = cs.SettingValue;
                        break;
                }
            }
            return result;
        }
        private void ImportProductModifiers()
        {
            Header("Importing Modifiers");

            foreach (data.bvc_ProductModifier old in oldDatabase.bvc_ProductModifier)
            {
                OptionDTO o = new OptionDTO();
                string fullName = old.Name;
                if (old.Displayname.Trim().Length > 0) fullName = old.Displayname;

                o.Settings = new List<OptionSettingDTO>();
                o.Items = new List<OptionItemDTO>();
                o.Bvin = old.bvin;
                o.IsShared = old.Shared;
                o.IsVariant = false; // Modifiers can't be variants
                o.NameIsHidden = false;
                if (old.Required) o.Settings.Add(new OptionSettingDTO() { Key = "required", Value = "1" });

                switch (old.Type.Trim().ToLowerInvariant())
                {
                    case "radio button list":
                    case "image radio button list":
                        o.OptionType = OptionTypesDTO.RadioButtonList;
                        break;
                    default: // Drop Down List
                        o.OptionType = OptionTypesDTO.DropDownList;
                        break;
                }
                o.Name = fullName;
                wl("Modifier: " + fullName);

                // Load Items for Option Here
                o.Items = LoadOptionItemsModifier(o.Bvin);

                if (o.Items.Count < 1)
                {
                    this.EmptyOptions.Add(o.Bvin);
                    wl("No Items Found for Modifier Import. Skipping...");
                    continue;
                }

                Api bv6proxy = GetBV6Proxy();
                var res = bv6proxy.ProductOptionsCreate(o);
                if (res != null)
                {
                    if (res.Errors.Count() > 0)
                    {
                        DumpErrors(res.Errors);
                        wl("FAILED");
                    }
                    else
                    {
                        wl("SUCCESS");
                    }
                }
            }
        }
        private void ImportProductChoices()
        {
            Header("Importing Shared Choices");

            foreach (data.bvc_ProductChoices old in oldDatabase.bvc_ProductChoices)
            {
                OptionDTO o = new OptionDTO();
                string fullName = old.ChoiceName;
                if (old.ChoiceDisplayName.Trim().Length > 0) fullName = old.ChoiceDisplayName;

                o.Settings = new List<OptionSettingDTO>();
                o.Items = new List<OptionItemDTO>();
                o.Bvin = old.bvin;
                o.IsShared = old.SharedChoice;
                o.IsVariant = true; // Choices are always variants in BV6
                o.NameIsHidden = false;

                switch (old.ChoiceType.Trim().ToLowerInvariant())
                {
                    case "radio button list":
                    case "image radio button list":
                        o.OptionType = OptionTypesDTO.RadioButtonList;
                        break;
                    default: // Drop Down List
                        o.OptionType = OptionTypesDTO.DropDownList;
                        break;
                }
                o.Name = fullName;
                wl("Choice: " + fullName);

                // Load Items for Option Here
                o.Items = LoadOptionItemsChoice(o.Bvin);

                if (o.Items.Count < 1)
                {
                    this.EmptyOptions.Add(o.Bvin);
                    wl("No Items for Option. Skipping...");
                    continue;
                }

                Api bv6proxy = GetBV6Proxy();
                var res = bv6proxy.ProductOptionsCreate(o);
                if (res != null)
                {
                    if (res.Errors.Count() > 0)
                    {
                        DumpErrors(res.Errors);
                        wl("FAILED");
                    }
                    else
                    {
                        wl("SUCCESS");
                    }
                }
            }
        }
        private List<OptionItemDTO> LoadOptionItemsModifier(string bvin)
        {
            List<OptionItemDTO> result = new List<OptionItemDTO>();

            data.BV53Entities db = new data.BV53Entities(EFConnString(settings.SourceConnectionString()));
            var items = db.bvc_ProductModifierOption.Where(y => y.ModifierId == bvin)
                            .OrderBy(y => y.Order);
            if (items == null) return result;

            foreach (data.bvc_ProductModifierOption item in items)
            {
                OptionItemDTO dto = new OptionItemDTO();
                dto.Bvin = item.bvin;
                dto.IsLabel = item.Null;
                dto.Name = item.Name;
                dto.OptionBvin = bvin;
                dto.PriceAdjustment = item.PriceAdjustment;
                dto.SortOrder = item.Order;
                dto.WeightAdjustment = item.WeightAdjustment;
                result.Add(dto);
            }

            return result;
        }
        private List<OptionItemDTO> LoadOptionItemsChoice(string bvin)
        {
            List<OptionItemDTO> result = new List<OptionItemDTO>();

            data.BV53Entities db = new data.BV53Entities(EFConnString(settings.SourceConnectionString()));
            var items = db.bvc_ProductChoiceOptions.Where(y => y.ProductChoiceId == bvin)
                            .OrderBy(y => y.Order);
            if (items == null) return result;

            foreach (data.bvc_ProductChoiceOptions item in items)
            {
                OptionItemDTO dto = new OptionItemDTO();
                dto.Bvin = item.bvin;
                dto.IsLabel = item.Null;
                dto.Name = item.ProductChoiceName;
                dto.OptionBvin = bvin;
                dto.PriceAdjustment = 0;
                dto.SortOrder = item.Order;
                dto.WeightAdjustment = 0;
                result.Add(dto);
            }

            return result;
        }

        // Categories

        private void ImportCategories()
        {
            Header("Importing Categories");

            foreach (data.bvc_Category old in oldDatabase.bvc_Category)
            {
                wl("Category: " + old.Name);

                CategoryDTO cat = new CategoryDTO();
                cat.BannerImageUrl = System.IO.Path.GetFileName(old.BannerImageURL);
                cat.Bvin = old.bvin;
                cat.Criteria = old.Criteria;
                cat.CustomerChangeableSortOrder = old.CustomerChangeableSortOrder;
                cat.CustomPageId = old.CustomPageId;
                cat.CustomPageLayout = CustomPageLayoutTypeDTO.WithSideBar;
                cat.CustomPageOpenInNewWindow = old.CustomPageNewWindow == 1;
                cat.CustomPageUrl = old.CustomPageURL;
                cat.Description = old.Description;
                switch (old.DisplaySortOrder)
                {
                    case 0:
                        cat.DisplaySortOrder = CategorySortOrderDTO.None;
                        break;
                    case 1:
                        cat.DisplaySortOrder = CategorySortOrderDTO.ManualOrder;
                        break;
                    case 2:
                        cat.DisplaySortOrder = CategorySortOrderDTO.ProductName;
                        break;
                    case 3:
                        cat.DisplaySortOrder = CategorySortOrderDTO.ProductPriceAscending;
                        break;
                    case 4:
                        cat.DisplaySortOrder = CategorySortOrderDTO.ProductPriceDescending;
                        break;
                    case 5:
                        cat.DisplaySortOrder = CategorySortOrderDTO.ManufacturerName;
                        break;
                }
                cat.Hidden = old.Hidden == 1;
                cat.ImageUrl = System.IO.Path.GetFileName(old.ImageURL);
                cat.Keywords = old.Keywords;
                cat.LastUpdatedUtc = old.LastUpdated;
                cat.LatestProductCount = old.LatestProductCount;
                cat.MetaDescription = old.MetaDescription;
                cat.MetaKeywords = old.MetaKeywords;
                cat.MetaTitle = old.MetaTitle;
                cat.Name = old.Name;
                cat.Operations = new List<ApiOperation>();
                cat.ParentId = old.ParentID;
                cat.PostContentColumnId = old.PostContentColumnId;
                if (cat.PostContentColumnId.Trim() == "-None -") cat.PostContentColumnId = string.Empty;
                cat.PreContentColumnId = old.PreContentColumnId;
                if (cat.PreContentColumnId.Trim() == "-None -") cat.PreContentColumnId = string.Empty;
                cat.PreTransformDescription = old.PreTransformDescription;
                cat.RewriteUrl = old.RewriteUrl;
                cat.ShowInTopMenu = old.ShowInTopMenu == 1;
                cat.ShowTitle = old.ShowTitle == 1;
                cat.SortOrder = old.SortOrder;
                cat.SourceType = CategorySourceTypeDTO.Manual;
                switch (old.SourceType)
                {
                    case 0:
                        cat.SourceType = CategorySourceTypeDTO.Manual;
                        break;
                    case 1:
                        cat.SourceType = CategorySourceTypeDTO.ByRules;
                        break;
                    case 2:
                        cat.SourceType = CategorySourceTypeDTO.CustomLink;
                        break;
                }
                cat.TemplateName = old.TemplateName;

                Api bv6proxy = GetBV6Proxy();
                var res = bv6proxy.CategoriesCreate(cat);
                if (res != null)
                {
                    if (res.Errors.Count() > 0)
                    {
                        DumpErrors(res.Errors);
                        wl("FAILED");
                    }
                    else
                    {
                        string bannerImageSource = old.BannerImageURL;
                        string imageSource = old.ImageURL;
                        MigrateCategoryBanner(old.bvin, bannerImageSource);
                        MigrateCategoryImage(old.bvin, imageSource);
                        wl("SUCCESS");
                    }
                }
            }
        }
        private void MigrateCategoryImage(string catBvin, string imageSource)
        {
            byte[] bytes = GetBytesForLocalImage(imageSource);
            if (bytes == null) return;
            string fileName = Path.GetFileName(imageSource);
            wl("Found Image: " + fileName + " [" + FriendlyFormatBytes(bytes.Length) + "]");
            Api bv6proxy = GetBV6Proxy();
            bv6proxy.CategoriesImagesIconUpload(catBvin, fileName, bytes);
        }
        private void MigrateCategoryBanner(string catBvin, string imageSource)
        {
            byte[] bytes = GetBytesForLocalImage(imageSource);
            if (bytes == null) return;
            string fileName = Path.GetFileName(imageSource);
            wl("Found Image: " + fileName + " [" + FriendlyFormatBytes(bytes.Length) + "]");
            Api bv6proxy = GetBV6Proxy();
            bv6proxy.CategoriesImagesBannerUpload(catBvin, fileName, bytes);
        }

        private byte[] GetBytesForLocalImage(string relativePath)
        {
            byte[] result = null;
            string source = settings.ImagesRootFolder;
            string relative = relativePath.Replace('/', '\\');
            if (relative.StartsWith("\\") == false)
            {
                relative = "\\" + relative;
            }
            source = source + relative;
            if (File.Exists(source))
            {
                result = File.ReadAllBytes(source);
            }
            return result;
        }

        // Properties and Types
        private void ImportProductTypes()
        {
            Header("Importing Product Types");

            foreach (data.bvc_ProductType old in oldDatabase.bvc_ProductType)
            {
                wl("Item: " + old.ProductTypeName);

                ProductTypeDTO pt = new ProductTypeDTO();

                pt.Bvin = old.bvin;
                pt.IsPermanent = old.IsPermanent;
                pt.LastUpdated = old.LastUpdated;
                pt.ProductTypeName = old.ProductTypeName;

                Api bv6proxy = GetBV6Proxy();
                var res = bv6proxy.ProductTypesCreate(pt);
                if (res != null)
                {
                    if (res.Errors.Count() > 0)
                    {
                        DumpErrors(res.Errors);
                        wl("FAILED");
                    }
                    else
                    {
                        MigratePropertiesForType(pt.Bvin);
                        wl("SUCCESS");
                    }
                }
            }
        }
        private void MigratePropertiesForType(string typeBvin)
        {
            wl("Migrating Properties to Type...");

            data.BV53Entities db = new data.BV53Entities(EFConnString(settings.SourceConnectionString()));
            var crosses = db.bvc_ProductTypeXProductProperty.Where(y => y.ProductTypeBvin == typeBvin);
            if (crosses == null) return;

            foreach (data.bvc_ProductTypeXProductProperty cross in crosses)
            {
                int sort = cross.SortOrder;
                string oldPropertyBvin = cross.ProductPropertyBvin;
                long newId = 0;
                var map = ProductPropertyMapper.Where(y => y.OldBvin == oldPropertyBvin).FirstOrDefault();
                if (map != null)
                {                
                    newId = map.NewBvin;
                }
                if (newId <= 0) continue;
                wl("Mapping " + oldPropertyBvin + " to " + newId.ToString());
                Api bv6proxy = GetBV6Proxy();
                bv6proxy.ProductTypesAddProperty(typeBvin, newId, sort);
            }
        }
        private void ImportProductProperties()
        {
            Header("Importing Product Properties");

            ProductPropertyMapper = new List<PropertyMapperInfo>();
            foreach (data.bvc_ProductProperty old in oldDatabase.bvc_ProductProperty)
            {
                wl("Item: " + old.DisplayName);

                // Skip creation if we've already mapped this one before
                if (ProductPropertyMapper.Where(y => y.OldBvin == old.bvin).Count() > 0) continue;

                PropertyMapperInfo mapInfo = new PropertyMapperInfo();
                mapInfo.OldBvin = old.bvin;                

                ProductPropertyDTO pp = new ProductPropertyDTO();
                pp.Choices = GetPropertyChoices(old.bvin, mapInfo);
                pp.CultureCode = old.CultureCode;
                pp.DefaultValue = old.DefaultValue;
                pp.DisplayName = old.DisplayName;
                pp.DisplayOnSite = old.DisplayOnSite == 1;
                pp.DisplayToDropShipper = old.DisplayToDropShipper == 1;
                pp.PropertyName = old.PropertyName;
                switch (old.TypeCode)
                {
                    case 0:
                        pp.TypeCode = ProductPropertyTypeDTO.None;
                        break;
                    case 1:
                        pp.TypeCode = ProductPropertyTypeDTO.TextField;
                        break;
                    case 2:
                        pp.TypeCode = ProductPropertyTypeDTO.MultipleChoiceField;
                        break;
                    case 3:
                        pp.TypeCode = ProductPropertyTypeDTO.CurrencyField;
                        break;
                    case 4:
                        pp.TypeCode = ProductPropertyTypeDTO.DateField;
                        break;
                    case 7:
                        pp.TypeCode = ProductPropertyTypeDTO.HyperLink;
                        break;
                }
                mapInfo.PropertyType = pp.TypeCode;

                Api bv6proxy = GetBV6Proxy();
                var res = bv6proxy.ProductPropertiesCreate(pp);
                if (res != null)
                {
                    if (res.Errors.Count() > 0)
                    {
                        DumpErrors(res.Errors);
                        wl("FAILED");
                    }
                    else
                    {
                        long newId = res.Content.Id;
                        mapInfo.NewBvin = newId;
                        SynchronizeChoices(res.Content, mapInfo);
                        ProductPropertyMapper.Add(mapInfo);

                        wl("SUCCESS");
                    }
                }
            }
        }
        private List<ProductPropertyChoiceDTO> GetPropertyChoices(string propertyBvin, PropertyMapperInfo mapInfo)
        {
            List<ProductPropertyChoiceDTO> result = new List<ProductPropertyChoiceDTO>();

            data.BV53Entities db = new data.BV53Entities(EFConnString(settings.SourceConnectionString()));
            var choices = db.bvc_ProductPropertyChoice.Where(y => y.PropertyBvin == propertyBvin)
                            .OrderBy(y => y.SortOrder);
            if (choices == null) return result;

            foreach (data.bvc_ProductPropertyChoice ppc in choices)
            {
                ProductPropertyChoiceDTO dto = new ProductPropertyChoiceDTO();
                dto.ChoiceName = ppc.ChoiceName;
                dto.LastUpdated = ppc.LastUpdated;
                //dto.PropertyId = ppc.PropertyBvin;
                dto.SortOrder = ppc.SortOrder;
                result.Add(dto);

                PropertyChoiceMapperInfo choiceMapInfo = new PropertyChoiceMapperInfo();
                choiceMapInfo.OldBvin = ppc.bvin;
                choiceMapInfo.SortOrder = ppc.SortOrder;
                choiceMapInfo.TextValue = ppc.ChoiceName;                
                mapInfo.Choices.Add(choiceMapInfo);
            }

            return result;
        }        
        // Maps the new choice (long)id value to the old (string)bvin value for a choice
        private void SynchronizeChoices(ProductPropertyDTO dto, PropertyMapperInfo mapInfo)
        {
            if (dto == null) return;
            if (dto.Choices == null) return;
            if (mapInfo == null) return;

            foreach(PropertyChoiceMapperInfo mapChoice in mapInfo.Choices)
            {
                var dtoChoice = dto.Choices.Where(y => y.ChoiceName == mapChoice.TextValue).FirstOrDefault();
                if (dtoChoice != null)
                {
                    mapChoice.NewBvin = dtoChoice.Id;
                }
            }
        }

        // Manufacturer Vendor
        private void ImportManufacturers()
        {
            Header("Importing Manufacturers");

            foreach (data.bvc_Manufacturer old in oldDatabase.bvc_Manufacturer)
            {
                wl("Item: " + old.DisplayName);

                VendorManufacturerDTO vm = new VendorManufacturerDTO();

                BV5Address oldAddr = new BV5Address();
                oldAddr.FromXmlString(old.Address);
                if (oldAddr != null)
                {
                    oldAddr.CopyTo(vm.Address, EFConnString(settings.SourceConnectionString()));
                }
                vm.Bvin = old.bvin;
                vm.ContactType = VendorManufacturerTypeDTO.Manufacturer;
                vm.DisplayName = old.DisplayName;
                vm.DropShipEmailTemplateId = old.DropShipEmailTemplateId;
                vm.EmailAddress = old.EmailAddress;
                vm.LastUpdated = old.LastUpdated;

                Api bv6proxy = GetBV6Proxy();
                var res = bv6proxy.ManufacturerCreate(vm);
                if (res != null)
                {
                    if (res.Errors.Count() > 0)
                    {
                        DumpErrors(res.Errors);
                        wl("FAILED");
                    }
                    else
                    {
                        wl("SUCCESS");
                    }
                }
            }
        }
        private void ImportVendors()
        {
            Header("Importing Vendors");

            foreach (data.bvc_Vendor old in oldDatabase.bvc_Vendor)
            {
                wl("Item: " + old.DisplayName);

                VendorManufacturerDTO vm = new VendorManufacturerDTO();

                BV5Address oldAddr = new BV5Address();
                oldAddr.FromXmlString(old.Address);
                if (oldAddr != null)
                {
                    oldAddr.CopyTo(vm.Address, EFConnString(settings.SourceConnectionString()));
                }
                vm.Bvin = old.bvin;
                vm.ContactType = VendorManufacturerTypeDTO.Vendor;
                vm.DisplayName = old.DisplayName;
                vm.DropShipEmailTemplateId = old.DropShipEmailTemplateId;
                vm.EmailAddress = old.EmailAddress;
                vm.LastUpdated = old.LastUpdated;

                Api bv6proxy = GetBV6Proxy();
                var res = bv6proxy.VendorCreate(vm);
                if (res != null)
                {
                    if (res.Errors.Count() > 0)
                    {
                        DumpErrors(res.Errors);
                        wl("FAILED");
                    }
                    else
                    {
                        wl("SUCCESS");
                    }
                }
            }
        }

        // Taxes
        private void ImportTaxSchedules()
        {
            Header("Importing Tax Schedules");

            TaxScheduleMapper = new Dictionary<string, long>();

            foreach (data.bvc_TaxClass old in oldDatabase.bvc_TaxClass)
            {
                wl("Tax Schedule: " + old.DisplayName);

                TaxScheduleDTO ts = new TaxScheduleDTO();
                ts.Name = old.DisplayName;

                Api bv6proxy = GetBV6Proxy();
                var res = bv6proxy.TaxSchedulesCreate(ts);
                if (res != null)
                {
                    if (res.Errors.Count() > 0)
                    {
                        DumpErrors(res.Errors);
                        wl("FAILED");
                    }
                    else
                    {
                        long newId = res.Content.Id;
                        TaxScheduleMapper.Add(old.bvin, newId);
                        wl("SUCCESS");
                    }
                }
            }

            // Migrate Default Tax Schedule
            TaxScheduleDTO defaultTaxSchedule = new TaxScheduleDTO();
            defaultTaxSchedule.Name = "Default";
            Api bv6proxy2 = GetBV6Proxy();
            var res2 = bv6proxy2.TaxSchedulesCreate(defaultTaxSchedule);
            if (res2 != null)
            {
                long defId = res2.Content.Id;
                TaxScheduleMapper.Add("Default", defId);
                TaxScheduleMapper.Add("", defId);
            }
        }
        private void ImportTaxes()
        {
            Header("Importing Taxes");


            foreach (data.bvc_Tax old in oldDatabase.bvc_Tax)
            {
                Country newCountry = GeographyHelper.TranslateCountry(EFConnString(settings.SourceConnectionString()), old.CountryBvin);
                string RegionAbbreviation = GeographyHelper.TranslateRegionBvinToAbbreviation(EFConnString(settings.SourceConnectionString()), old.RegionBvin);

                wl("Tax: " + newCountry.DisplayName + ", " + RegionAbbreviation + " " + old.PostalCode);

                TaxDTO tx = new TaxDTO();
                tx.ApplyToShipping = old.ApplyToShipping;
                tx.CountryName = newCountry.DisplayName;
                tx.PostalCode = old.PostalCode;
                tx.Rate = old.Rate;
                tx.RegionAbbreviation = RegionAbbreviation;

                string matchId = old.TaxClass;
                if (matchId.Trim().Length < 1) matchId = "Default";

                if (TaxScheduleMapper.ContainsKey(matchId))
                {
                    tx.TaxScheduleId = TaxScheduleMapper[matchId];
                }

                Api bv6proxy = GetBV6Proxy();
                var res = bv6proxy.TaxesCreate(tx);
                if (res != null)
                {
                    if (res.Errors.Count() > 0)
                    {
                        DumpErrors(res.Errors);
                        wl("FAILED");
                    }
                    else
                    {
                        wl("SUCCESS");
                    }
                }
            }

        }

        // Affiliates

        private void ImportAffiliates()
        {
            Header("Importing Affiliates");
            AffiliateMapper = new Dictionary<string, long>();

            foreach (data.bvc_Affiliate aff in oldDatabase.bvc_Affiliate)
            {
                wl("Affiliate: " + aff.DisplayName + " | " + aff.bvin);

                try
                {
                    AffiliateDTO a = OldToNewAffiliate(aff);

                    Api bv6proxy = GetBV6Proxy();
                    var res = bv6proxy.AffiliatesCreate(a);
                    if (res != null)
                    {
                        if (res.Errors.Count > 0)
                        {
                            DumpErrors(res.Errors);
                            return;
                        }
                        if (res.Content == null) throw new ArgumentNullException("Result object was null");
                        long newId = res.Content.Id;
                        AffiliateMapper.Add(aff.bvin, newId);
                        wl("SUCCESS");
                        ImportAffiliateReferrals(aff.bvin, newId);
                    }
                }
                catch (Exception ex)
                {
                    wl("FAILED: " + ex.Message + " | " + ex.StackTrace);
                }

            }
        }
        private AffiliateDTO OldToNewAffiliate(data.bvc_Affiliate aff)
        {
            AffiliateDTO affiliate = new AffiliateDTO();

            BV5Address oldAddress = new BV5Address();
            oldAddress.FromXmlString(aff.Address);

            affiliate.Address = new AddressDTO();
            if (oldAddress != null)
            {
                oldAddress.CopyTo(affiliate.Address, EFConnString(settings.SourceConnectionString()));
            }
            affiliate.CommissionAmount = aff.CommissionAmount;
            switch (aff.CommissionType)
            {
                case 0:
                    affiliate.CommissionType = AffiliateCommissionTypeDTO.None;
                    break;
                case 1:
                    affiliate.CommissionType = AffiliateCommissionTypeDTO.PercentageCommission;
                    break;
                case 2:
                    affiliate.CommissionType = AffiliateCommissionTypeDTO.FlatRateCommission;
                    break;
            }
            affiliate.CustomThemeName = aff.StyleSheet;
            affiliate.DisplayName = aff.DisplayName;
            affiliate.DriversLicenseNumber = aff.DriversLicenseNumber;
            affiliate.Enabled = aff.Enabled;
            affiliate.Id = -1;
            affiliate.LastUpdatedUtc = aff.LastUpdated;
            affiliate.Notes = aff.Notes;
            affiliate.ReferralDays = aff.ReferralDays;
            affiliate.ReferralId = aff.ReferralID;
            if (affiliate.ReferralId == string.Empty) affiliate.ReferralId = aff.bvin;
            affiliate.TaxId = aff.TaxID;
            affiliate.WebSiteUrl = aff.WebSiteURL;
            affiliate.Contacts = new List<AffiliateContactDTO>();

            return affiliate;
        }
        private void ImportAffiliateReferrals(string bvin, long newId)
        {
            wl(" - Migrating Referrals...");

            data.BV53Entities db = new data.BV53Entities(EFConnString(settings.SourceConnectionString()));

            var referrals = db.bvc_AffiliateReferral.Where(y => y.affid == bvin);
            if (referrals == null) return;

            foreach (data.bvc_AffiliateReferral r in referrals)
            {
                AffiliateReferralDTO rnew = new AffiliateReferralDTO();
                rnew.AffiliateId = newId;
                rnew.TimeOfReferralUtc = r.TimeOfReferral;
                rnew.ReferrerUrl = r.referrerurl;

                Api bv6proxy = GetBV6Proxy();
                var res = bv6proxy.AffiliateReferralsCreate(rnew);
            }

        }

        // Users
        private void ImportUsers()
        {
            Header("Importing Users");


            int pageSize = 100;
            int totalRecords = oldDatabase.bvc_User.Count();
            int totalPages = (int)(Math.Ceiling((decimal)totalRecords / (decimal)pageSize));

            //System.Threading.Tasks.Parallel.For(0, totalPages, ProcessPage);
            int startIndex = 0;
            if (settings.UserStartPage > 1)
            {
                startIndex = settings.UserStartPage - 1;
            }
            for (int i = startIndex; i < totalPages; i++)
            {
                wl("Getting Users page " + (i + 1) + " of " + totalPages.ToString());
                int startRecord = i * pageSize;
                var users = (from u in oldDatabase.bvc_User select u).OrderBy(y => y.Email).Skip(startRecord).Take(pageSize).ToList();
                foreach (data.bvc_User u in users)
                {
                    ImportSingleUser(u);
                }
            }

        }
        private void ProcessPage(int i)
        {
            wl("Getting Users page " + (i + 1));
            int startRecord = i * 100;
            var users = (from u in oldDatabase.bvc_User select u).OrderBy(y => y.Email).Skip(startRecord).Take(100).ToList();
            foreach (data.bvc_User u in users)
            {
                ImportSingleUser(u);
            }
        }
        private void ImportSingleUser(data.bvc_User u)
        {
            if (u == null)
            {
                wl("Customer was null!");
                return;
            }
            wl("Importing Customer: " + u.Email);

            CustomerAccountDTO customer = new CustomerAccountDTO();
            customer.Bvin = u.bvin;
            customer.CreationDateUtc = u.CreationDate;
            customer.Email = u.Email;
            customer.FailedLoginCount = u.FailedLoginCount;
            customer.FirstName = u.FirstName;
            customer.LastLoginDateUtc = u.LastLoginDate;
            customer.LastName = u.LastName;
            customer.LastUpdatedUtc = u.LastUpdated;
            customer.Notes = u.Comment;
            customer.Password = string.Empty;
            customer.PricingGroupId = u.PricingGroup;
            customer.Salt = string.Empty;
            customer.TaxExempt = u.TaxExempt == 1 ? true : false;
            customer.Addresses = new List<AddressDTO>();

            // Preserve clear text passwords
            string newPassword = string.Empty;
            if (u.PasswordFormat == 0)
            {
                newPassword = u.Password;
            }

            BV5Address shipping = new BV5Address();
            shipping.FromXmlString(u.ShippingAddress);
            AddressDTO ship = new AddressDTO();
            ship.AddressType = AddressTypesDTO.Shipping;
            shipping.CopyTo(ship, EFConnString(settings.SourceConnectionString()));
            customer.Addresses.Add(ship);

            BV5Address billing = new BV5Address();
            billing.FromXmlString(u.BillingAddress);
            AddressDTO bill = new AddressDTO();
            bill.AddressType = AddressTypesDTO.Billing;
            billing.CopyTo(bill, EFConnString(settings.SourceConnectionString()));
            customer.Addresses.Add(bill);

            List<BV5Address> addresses = BV5Address.ReadAddressesFromXml(u.AddressBook);
            foreach (BV5Address addr in addresses)
            {
                AddressDTO a = new AddressDTO();
                a.AddressType = AddressTypesDTO.General;
                addr.CopyTo(a, EFConnString(settings.SourceConnectionString()));
                customer.Addresses.Add(a);
            }

            Api bv6proxy = GetBV6Proxy();
            var res = bv6proxy.CustomerAccountsCreateWithPassword(customer, newPassword);
            if (res != null)
            {
                if (res.Errors.Count > 0)
                {
                    DumpErrors(res.Errors);
                    return;
                }
                wl("SUCCESS");
            }
        }

        // Price Groups and Roles
        private void ImportPriceGroups()
        {
            Header("Importing Price Groups");

            foreach (data.bvc_PriceGroup oldGroup in oldDatabase.bvc_PriceGroup)
            {
                wl("Price Group: " + oldGroup.Name);

                PriceGroupDTO pg = new PriceGroupDTO();
                pg.AdjustmentAmount = oldGroup.AdjustmentAmount;
                pg.Bvin = oldGroup.bvin;
                pg.Name = oldGroup.Name;
                switch (oldGroup.PricingType)
                {

                    case 3:
                        pg.PricingType = PricingTypesDTO.AmountAboveCost;
                        break;
                    case 1:
                        pg.PricingType = PricingTypesDTO.AmountOffListPrice;
                        break;
                    case 5:
                        pg.PricingType = PricingTypesDTO.AmountOffSitePrice;
                        break;
                    case 2:
                        pg.PricingType = PricingTypesDTO.PercentageAboveCost;
                        break;
                    case 0:
                        pg.PricingType = PricingTypesDTO.PercentageOffListPrice;
                        break;
                    case 4:
                        pg.PricingType = PricingTypesDTO.PercentageOffSitePrice;
                        break;
                }

                Api bv6proxy = GetBV6Proxy();
                var res = bv6proxy.PriceGroupsCreate(pg);
                if (res != null)
                {
                    if (res.Errors.Count() > 0)
                    {
                        DumpErrors(res.Errors);
                        wl("FAILED");
                    }
                    else
                    {
                        if (res.Content == null)
                        {
                            wl("FAILED!");
                        }
                        else
                        {
                            wl(res.Content.Bvin == string.Empty ? "FAILED" : "SUCCESS");
                        }
                    }
                }
            }

        }
        private void ImportRoles()
        {
            Header("Importing Roles");
        }

        private void ClearCategories()
        {
            Header("Clearing All Categories");

            Api bv6proxy = GetBV6Proxy();
            var res = bv6proxy.CategoriesClearAll();
            if (res != null)
            {
                if (res.Errors.Count > 0)
                {
                    DumpErrors(res.Errors);
                    wl("FAILED");
                }
                else
                {
                    wl("SUCCESS");
                }
            }
        }
        private void ClearUsers()
        {
            Header("Clearing All Customer Records");

            Api bv6proxy = GetBV6Proxy();
            var res = bv6proxy.CustomerAccountsClearAll();
            if (res != null)
            {
                if (res.Errors.Count > 0)
                {
                    DumpErrors(res.Errors);
                    wl("FAILED");
                }
                else
                {
                    wl("SUCCESS");
                }
            }
        }
        private void ClearProducts()
        {
            Header("Clearing All Products");

            int remaining = int.MaxValue;

            while (remaining > 0)
            {
                int pageSize = 100;

                Api bv6proxy = GetBV6Proxy();
                var res = bv6proxy.ProductsClearAll(pageSize);
                if (res == null)
                {
                    wl("FAILED TO CLEAR PRODUCTS!");
                }
                else
                {
                    if (res.Errors.Count > 0)
                    {
                        DumpErrors(res.Errors);
                        wl("FAILED");
                        return;
                    }
                    else
                    {
                        if (res.Content != null)
                        {
                            remaining = res.Content.ProductsRemaining;
                            wl("Clearing products: " + res.Content.ProductsRemaining + " remaining at " + DateTime.UtcNow.ToString());
                        }
                        else
                        {
                            wl("Invalid Response. Skipping Clear..");
                            remaining = 0;
                        }
                    }
                }
            }
            wl("Finished Clearing Products at : " + DateTime.UtcNow.ToString());
        }
    }
}
