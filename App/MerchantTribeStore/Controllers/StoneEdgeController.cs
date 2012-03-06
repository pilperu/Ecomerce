using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Linq;
using MerchantTribeStore;
using MerchantTribeStore.Models;
using MerchantTribe.Web;
using MerchantTribe.Commerce;
using MerchantTribe.Commerce.Catalog;
using MerchantTribe.Commerce.Membership;
using MerchantTribe.Commerce.Orders;

namespace MerchantTribeStore.Controllers
{
    [ValidateInput(false)]
    public class StoneEdgeController : Shared.BaseStoreController
    {
        
        List<StoneEdgeError> _Errors = new List<StoneEdgeError>();
        StoneEdgeIntegrationModel _model;

        private bool ValidateSSL()
        {
            if (!Request.IsSecureConnection)
            {
                _Errors.Add(new StoneEdgeError() { ErrorType = StoneEdgeErrorType.General,
                                                    Message = "Requests must be secured using SSL. Make sure you're request is using https:// at the beginning."});                
                return false;
            }
            return true;
        }
        private bool Authenticate()
        {
            string email = Request["setiuser"] ?? string.Empty;
            string password = Request["password"] ?? string.Empty;
            string errorMessage = string.Empty;

            var u = this.MTApp.AccountServices.AdminUsers.FindByEmail(email);
            if (u == null)
            {
                this._Errors.Add(new StoneEdgeError() { Message = "User does not have access to store." });
                return false;
            }

            bool result = this.MTApp.AccountServices.AuthenticateAdminUser(email, password, ref errorMessage);
            if (!result)
            {
                this._Errors.Add(new StoneEdgeError() { Message = errorMessage });
                return false;
            }            
             
            if (!this.MTApp.AccountServices.DoesUserHaveAccessToStore(this.MTApp.CurrentStore.Id, u.Id))
            {
               this._Errors.Add(new StoneEdgeError() { Message = "User does not have access." });
               return false;
            }
                        
            return result;
        }

        //
        // GET: /StoneEdge/
        public ActionResult Index(string setiFunction)
        {   
            // grab command request
            _model = new StoneEdgeIntegrationModel(setiFunction);

            // Ensure we're validated and 
            if (_model.SetiFunction != StoneEdgeFunction.SendVersion &&
                _model.SetiFunction != StoneEdgeFunction.Unknown &&
                _model.SetiFunction != StoneEdgeFunction.SendSampleError)
            {
                if (!ValidateSSL()) return RenderErrors();
                if (!Authenticate()) return RenderErrors();                
            }

            // process
            switch (_model.SetiFunction)
            {                
                case StoneEdgeFunction.SendVersion:
                    return SendVersion();                    
                case StoneEdgeFunction.SendSampleError:
                    return SendSampleError();

                case StoneEdgeFunction.OrderCount:
                    return OrderCount();
                case StoneEdgeFunction.DownloadOrders:
                    return OrderDownload();

                case StoneEdgeFunction.GetProductsCount:
                    return ProductsCount();
                case StoneEdgeFunction.DownloadProds:
                    return ProductsDownload(false);
                case StoneEdgeFunction.DownloadQoh:
                    return ProductsDownload(true);

                case StoneEdgeFunction.GetCustomersCount:
                    return CustomersCount();
                case StoneEdgeFunction.DownloadCustomers:
                    return CustomersDownload();

                case StoneEdgeFunction.QohReplace:
                    return QohReplace();
                case StoneEdgeFunction.InvUpdate:
                    return InventoryUpdate();

                case StoneEdgeFunction.UpdateStatus:
                    return UpdateOrder();

                default:
                    return View(_model);
            }            
        }

        private ActionResult SendVersion()
        {
            string version = "SETIResponse: version=" + _model.SetiVersion + "\n";
            return Content(version, "text/plain");            
        }

        // Sends a sample generic error
        private ActionResult SendSampleError()
        {
            string type = Request["SampleErrorType"] ?? "0";
            int typeAsInt = 0;
            int.TryParse(type, out typeAsInt);                        
            return RenderError((StoneEdgeErrorType)typeAsInt, "This is a sample Error!");            
        }

        // Renders Generic Errors, not order, customer, product specific ones
        private ActionResult RenderError(StoneEdgeErrorType type, string message)
        {
            this._Errors.Add(new StoneEdgeError() { ErrorType = type, Message = message });
            return RenderErrors();
        }
        private ActionResult RenderErrors()
        {
            if (_Errors.Count < 1) return Content("SETIError: No Error Message To Render", "text/plain");

            var err = _Errors[0];

            // Dump Generic Errors
                string errorTag = "SETI";

                switch (err.ErrorType)
                {                                        
                    case StoneEdgeErrorType.Customers:
                        errorTag += "Customers";
                        break;
                    case StoneEdgeErrorType.Orders:
                        errorTag += "Orders";
                        break;
                    case StoneEdgeErrorType.Products:
                        errorTag += "Products";
                        break;
                    default:
                        return Content("SETIError: " + err.Message, "text/plain");
                }

                XDocument xdoc = 
                    new XDocument(
                        new XElement(errorTag,
                            new XElement("Response",
                                new XElement("ResponseCode", "3"),
                                new XElement("ResponseDescription", err.Message)
                                    )
                                )
                            );                
                return Content(xdoc.ToStringWithDeclaration(), "application/xml");
        }

        // Orders
        private ActionResult OrderCount()
        {
            string formLastDate = Request["lastdate"] ?? string.Empty;
            string formLastOrder = Request["lastorder"] ?? string.Empty;                        
            if (formLastDate == string.Empty && formLastOrder == string.Empty)
            {                
                return RenderError(StoneEdgeErrorType.General,"Missing LastDate and LastOrder from Request");
            }

            int returnCount = 0;
            
            DateTime startDate = new DateTime(1900, 1, 1);
            bool sendAll = (formLastDate.ToLowerInvariant() == "all" || formLastOrder.ToLowerInvariant() == "all");
            if (!sendAll)
            {
                if (!DateTime.TryParse(formLastDate, out startDate)) return RenderError(StoneEdgeErrorType.General, "Unable to parse date!");                
            }
            // Always use start of day!
            startDate = MerchantTribe.Web.Dates.ZeroOutTime(startDate);

            var criteria = new MerchantTribe.Commerce.Orders.OrderSearchCriteria();
            criteria.EndDateUtc = DateTime.UtcNow.AddYears(1);
            criteria.StartDateUtc = TimeZoneInfo.ConvertTimeToUtc(startDate, this.MTApp.CurrentStore.Settings.TimeZone);
            criteria.IsPlaced = true;
            
            var results = this.MTApp.OrderServices.Orders.FindByCriteriaPaged(criteria, 1, 1,ref returnCount);
            
            
            return Content(String.Format("SETIResponse: ordercount={0}", returnCount), "text/plain");
        }
        private ActionResult OrderDownload()
        {
            string formLastDate = Request["lastdate"] ?? string.Empty;
            string formLastOrder = Request["lastorder"] ?? string.Empty;
            string formStartNum = Request["startnum"] ?? string.Empty;
            string formBatchSize = Request["batchsize"] ?? string.Empty;
            string formDKey = Request["dkey"] ?? string.Empty;

            // Get Date
            if (formLastDate == string.Empty && formLastOrder == string.Empty)
            {
                return RenderError(StoneEdgeErrorType.General, "Missing LastDate and LastOrder from Request");
            }

            // Get Page Size
            int tempPageSize = 100;
            if (formBatchSize.Length > 0)
            {
                int.TryParse(formBatchSize, out tempPageSize);
            }
            if (tempPageSize < 1) tempPageSize = 100;

            // Get Page Number
            int pageNumber = 1;
            int tempStartRecord = 1;
            if (formStartNum.Length > 0)
            {
                if (int.TryParse(formStartNum, out tempStartRecord))
                {
                    // Stone Edge passes in a 1 based number of record to start with
                    // subtract one to make it zero based, then divide by page size to get 
                    // page number
                    pageNumber = (tempStartRecord - 1) / tempPageSize;

                    // Increase page number because pages are 1-based in MerchantTribe
                    pageNumber += 1;
                }
            }
            if (pageNumber < 1) pageNumber = 1;            

            DateTime startDate = new DateTime(1900, 1, 1);
            bool sendAll = (formLastDate.ToLowerInvariant() == "all" || formLastOrder.ToLowerInvariant() == "all");
            if (!sendAll)
            {
                if (!DateTime.TryParse(formLastDate, out startDate)) return RenderError(StoneEdgeErrorType.General, "Unable to parse date!");
            }
            // Always use start of day!
            startDate = MerchantTribe.Web.Dates.ZeroOutTime(startDate);

            var criteria = new MerchantTribe.Commerce.Orders.OrderSearchCriteria();
            criteria.EndDateUtc = DateTime.UtcNow.AddYears(1);
            criteria.StartDateUtc = TimeZoneInfo.ConvertTimeToUtc(startDate, this.MTApp.CurrentStore.Settings.TimeZone);
            criteria.IsPlaced = true;

            int returnCount = 0;
            var results = this.MTApp.OrderServices.Orders.FindByCriteriaPaged(criteria, pageNumber, tempPageSize, ref returnCount);

            return RenderOrders(results);                        
        }
        private ActionResult RenderOrders(List<MerchantTribe.Commerce.Orders.OrderSnapshot> orders)
        {

            var fullOrders = this.MTApp.OrderServices.Orders.FindMany(orders.Select(y => y.bvin).ToList());

            StringBuilder sb = new StringBuilder();            
            using (TextWriter writer = new StringWriter(sb))
            {
                using (XmlTextWriter xw = new XmlTextWriter(writer))
                {
                    xw.WriteStartDocument();

                    xw.WriteStartElement("SETIOrders");

                    if (fullOrders.Count == 0)
                    {
                        xw.WriteStartElement("Response");
                        xw.WriteElementString("ResponseCode", "2");
                        xw.WriteElementString("ResponseDescription", "success");
                        xw.WriteEndElement();
                    }
                    else
                    {
                        xw.WriteStartElement("Response");
                        xw.WriteElementString("ResponseCode", "1");
                        xw.WriteElementString("ResponseDescription", "success");
                        xw.WriteEndElement();

                        foreach (var o in fullOrders)
                        {
                            RenderSingleOrder(xw, o);
                        }
                    }

                    xw.WriteEndElement(); // End SetiOrders;

                    xw.WriteEndDocument();
                    xw.Flush();
                }                
            }
                            
            return Content(sb.ToString(), "application/xml");
        }
        private void RenderSingleOrder(XmlTextWriter xw, MerchantTribe.Commerce.Orders.Order o)
        {
            xw.WriteStartElement("Order");

            xw.WriteElementString("OrderNumber", Text.TrimToLength(o.OrderNumber, 50));
            DateTime TimeOfOrderLocal = TimeZoneInfo.ConvertTimeFromUtc(o.TimeOfOrderUtc, MTApp.CurrentStore.Settings.TimeZone);
            xw.WriteElementString("OrderDate", TimeOfOrderLocal.ToString("MMM/dd/yyyy HH:mm:ss"));
            xw.WriteElementString("OrderStatus", Text.TrimToLength(o.StatusName, 50));

            xw.WriteStartElement("Billing");
            xw.WriteElementString("FullName", Text.TrimToLength(o.BillingAddress.FirstName + " " + o.BillingAddress.LastName, 255));
            xw.WriteElementString("Company", Text.TrimToLength(o.BillingAddress.Company, 255));
            xw.WriteElementString("Phone", Text.TrimToLength(o.BillingAddress.Phone, 255));
            xw.WriteElementString("Email", Text.TrimToLength(o.UserEmail, 255));
            xw.WriteStartElement("Address");
            xw.WriteElementString("Addr1", Text.TrimToLength(o.BillingAddress.Street, 255));
            xw.WriteElementString("Addr2", Text.TrimToLength(o.BillingAddress.Street2, 255));
            xw.WriteElementString("City", Text.TrimToLength(o.BillingAddress.City, 255));
            xw.WriteElementString("State", Text.TrimToLength(o.BillingAddress.RegionData.Abbreviation, 2));
            xw.WriteElementString("Zip", Text.TrimToLength(o.BillingAddress.PostalCode, 255));
            var country = MerchantTribe.Web.Geography.Country.FindByBvin(o.BillingAddress.CountryBvin);
            xw.WriteElementString("Country", Text.TrimToLength(country.IsoCode, 2));
            xw.WriteEndElement(); // End Address
            xw.WriteEndElement(); // End Billing

            xw.WriteStartElement("Shipping");
            xw.WriteElementString("FullName", Text.TrimToLength(o.ShippingAddress.FirstName + " " + o.ShippingAddress.LastName, 255));
            xw.WriteElementString("Company", Text.TrimToLength(o.ShippingAddress.Company, 255));
            xw.WriteElementString("Phone", Text.TrimToLength(o.ShippingAddress.Phone, 255));
            xw.WriteElementString("Email", Text.TrimToLength(o.UserEmail, 255));
            xw.WriteStartElement("Address");
            xw.WriteElementString("Addr1", Text.TrimToLength(o.ShippingAddress.Street, 255));
            xw.WriteElementString("Addr2", Text.TrimToLength(o.ShippingAddress.Street2, 255));
            xw.WriteElementString("City", Text.TrimToLength(o.ShippingAddress.City, 255));
            xw.WriteElementString("State", Text.TrimToLength(o.ShippingAddress.RegionData.Abbreviation, 2));
            xw.WriteElementString("Zip", Text.TrimToLength(o.ShippingAddress.PostalCode, 255));
            var country2 = MerchantTribe.Web.Geography.Country.FindByBvin(o.ShippingAddress.CountryBvin);
            xw.WriteElementString("Country", Text.TrimToLength(country2.IsoCode, 2));
            xw.WriteEndElement(); // End Address

            // Items are inside the "shipping" tag
            foreach (var li in o.Items)
            {
                RenderSingleItem(xw, li, o.PaymentStatus == MerchantTribe.Commerce.Orders.OrderPaymentStatus.Paid);
            }
            xw.WriteEndElement(); // End Shipping

            var payments = this.MTApp.OrderServices.Transactions.FindForOrder(o.bvin);
            if (payments != null)
            {
                if (payments.Count > 0)
                {
                    xw.WriteStartElement("Payment");
                    foreach (var payment in payments)
                    {
                        RenderSinglePayment(xw, payment, o);
                    }
                    xw.WriteEndElement();
                }
            }

            xw.WriteStartElement("Totals");
            xw.WriteElementString("ProductTotal", o.TotalOrderBeforeDiscounts.ToString("0.00"));
            
            xw.WriteStartElement("Discount");
            xw.WriteElementString("Type", "Flat");
            xw.WriteElementString("Description", "Order Discounts");
            xw.WriteElementString("Amount", o.TotalOrderDiscounts.ToString("0.00"));
            xw.WriteElementString("ApplyDiscount", "Pre");
            xw.WriteEndElement(); // End Discounts

            xw.WriteElementString("SubTotal", o.TotalOrderAfterDiscounts.ToString("0.00"));

            xw.WriteStartElement("Tax");
            xw.WriteElementString("TaxAmount", o.TotalTax.ToString("0.00"));
            xw.WriteElementString("TaxShipping", "No");
            xw.WriteEndElement(); // End Tax           

            xw.WriteElementString("GrandTotal", o.TotalGrand.ToString("0.00"));

            if (o.TotalHandling > 0)
            {
                xw.WriteStartElement("Surcharge");
                xw.WriteElementString("Total", o.TotalHandling.ToString());
                xw.WriteElementString("Description", "Handling");
                xw.WriteEndElement();
            }

            xw.WriteStartElement("ShippingTotal");
            xw.WriteElementString("Total", o.TotalShippingAfterDiscounts.ToString("0.00"));
            xw.WriteElementString("Description", Text.TrimToLength(o.ShippingMethodDisplayName, 255));
            xw.WriteEndElement();

            xw.WriteEndElement(); // End Totals

            foreach (var code in o.Coupons)
            {
                xw.WriteStartElement("Coupon");
                xw.WriteElementString("Name", code.CouponCode);
                xw.WriteElementString("Total", "0.00");
                xw.WriteEndElement(); // End Coupon
            }

            xw.WriteStartElement("Other");
            xw.WriteElementString("Associate", Text.TrimToLength(o.AffiliateID, 50));
            xw.WriteElementString("OrderInstructions", o.Instructions);
            xw.WriteElementString("TotalOrderWeight", o.TotalWeight.ToString("0.00"));
            xw.WriteEndElement(); // End Other

            xw.WriteEndElement(); // End Order
        }
        private void RenderSingleItem(XmlTextWriter xw, MerchantTribe.Commerce.Orders.LineItem li, bool isPaid)
        {
            xw.WriteStartElement("Product");
            
            xw.WriteElementString("Sku", Text.TrimToLength(li.ProductSku, 255));
            xw.WriteElementString("Name", Text.TrimToLength(li.ProductName, 255));
            xw.WriteElementString("Quantity", li.Quantity.ToString());
            xw.WriteElementString("ItemPrice", li.BasePricePerItem.ToString("0.00"));
            xw.WriteElementString("Weight", li.ProductShippingWeight.ToString());
            string prodType = (li.ShippingStatus == MerchantTribe.Commerce.Orders.OrderShippingStatus.NonShipping) 
                ? "Download" : "Tangible";
            xw.WriteElementString("ProdType", prodType);
            xw.WriteElementString("Taxable", li.TaxableValue() > 0 ? "Yes" : "No");
            xw.WriteElementString("CustomerText", li.ProductShortDescription);
            xw.WriteElementString("LineID", li.Id.ToString());
            xw.WriteElementString("Status", Text.TrimToLength(li.StatusName, 50));
            if (li.ShipFromMode == MerchantTribe.Commerce.Shipping.ShippingMode.ShipFromManufacturer
                || li.ShipFromMode == MerchantTribe.Commerce.Shipping.ShippingMode.ShipFromVendor)
            {
                if (isPaid)
                {
                    xw.WriteElementString("FulfillmentCenter", Text.TrimToLength(li.ShipFromNotificationId, 20));
                }
            }
            xw.WriteElementString("Total", li.LineTotal.ToString("0.00"));
            xw.WriteStartElement("Dimensions");
            xw.WriteElementString("Length", li.ProductShippingLength.ToString());
            xw.WriteElementString("Width", li.ProductShippingWidth.ToString());
            xw.WriteElementString("Height", li.ProductShippingHeight.ToString());
            xw.WriteEndElement(); // End Dimensions
            
            xw.WriteEndElement(); // End Product
        }
        private void RenderSinglePayment(XmlTextWriter xw, 
                                        MerchantTribe.Commerce.Orders.OrderTransaction payment, 
                                        MerchantTribe.Commerce.Orders.Order o)
        {
            if (payment.Action == MerchantTribe.Payment.ActionType.CreditCardInfo ||
                payment.Action == MerchantTribe.Payment.ActionType.CreditCardRefund ||
                payment.Action == MerchantTribe.Payment.ActionType.CreditCardHold ||
                payment.Action == MerchantTribe.Payment.ActionType.CreditCardCharge ||
                payment.Action == MerchantTribe.Payment.ActionType.CreditCardCapture)
            {
                //CreditCard 
                xw.WriteStartElement("CreditCard");
                if (payment.CreditCard.CardNumber.Length > 4)
                {
                    xw.WriteElementString("Number", payment.CreditCard.CardNumber);
                }
                else
                {
                    xw.WriteElementString("Issuer", payment.CreditCard.CardTypeName);
                    xw.WriteElementString("Number", payment.CreditCard.CardNumberLast4Digits);
                }
                xw.WriteElementString("ExpirationDate", payment.CreditCard.ExpirationMonthPadded + "/" + payment.CreditCard.ExpirationYear.ToString());
                xw.WriteElementString("FullName", Text.TrimToLength(payment.CreditCard.CardHolderName, 255));
                xw.WriteElementString("TransID", Text.TrimToLength(payment.RefNum1, 100));
                xw.WriteElementString("AuthCode", Text.TrimToLength(payment.RefNum2, 100));
                xw.WriteElementString("Amount", payment.Amount.ToString());

                xw.WriteEndElement(); // End CreditCard
            }
            if (payment.Action == MerchantTribe.Payment.ActionType.PayPalCapture ||
                payment.Action == MerchantTribe.Payment.ActionType.PayPalCharge ||
                payment.Action == MerchantTribe.Payment.ActionType.PayPalHold ||
                payment.Action == MerchantTribe.Payment.ActionType.PayPalRefund)
            {
                //PayPal 
                xw.WriteStartElement("PayPal");

                xw.WriteStartElement("Payer");
                xw.WriteElementString("Email", Text.TrimToLength(payment.TempCustomerEmail, 255));
                xw.WriteEndElement(); // End Payer

                xw.WriteStartElement("Transaction");
                xw.WriteElementString("TransID", Text.TrimToLength(payment.RefNum1, 50));
                xw.WriteElementString("Amount", payment.Amount.ToString("0.00"));
                xw.WriteEndElement(); // End Transaction

                xw.WriteEndElement(); // End PayPal
            }
            if (payment.Action == MerchantTribe.Payment.ActionType.PurchaseOrderAccepted
                || payment.Action == MerchantTribe.Payment.ActionType.PurchaseOrderInfo)
            {
                //PurchaseOrder 
                xw.WriteStartElement("PurchaseOrder");
                xw.WriteElementString("PurchaseNumber", Text.TrimToLength(payment.PurchaseOrderNumber, 50));
                xw.WriteEndElement(); // End Purchase Order
            }
            if (payment.Action == MerchantTribe.Payment.ActionType.CashReceived ||
                payment.Action == MerchantTribe.Payment.ActionType.CashReturned)
            {
                //Generic1 
                xw.WriteStartElement("Generic1");
                xw.WriteElementString("Name", "Cash");
                xw.WriteElementString("Description", "Cash");
                xw.WriteElementString("Field1", payment.Amount.ToString("0.00"));
                xw.WriteEndElement();
            }
            if (payment.Action == MerchantTribe.Payment.ActionType.CompanyAccountAccepted ||
                payment.Action == MerchantTribe.Payment.ActionType.CompanyAccountInfo)
            {
                //Generic2 
                xw.WriteStartElement("Generic1");
                xw.WriteElementString("Name", "Account");
                xw.WriteElementString("Description", "Company Account");
                xw.WriteElementString("Field1", payment.Amount.ToString("0.00"));
                xw.WriteEndElement();
            }
            if (payment.Action == MerchantTribe.Payment.ActionType.GiftCardCapture ||
                payment.Action == MerchantTribe.Payment.ActionType.GiftCardDecrease ||
                payment.Action == MerchantTribe.Payment.ActionType.GiftCardHold ||
                payment.Action == MerchantTribe.Payment.ActionType.GiftCardIncrease ||
                payment.Action == MerchantTribe.Payment.ActionType.GiftCardInfo)
            {
                //GiftCard 
                xw.WriteStartElement("GiftCard");
                xw.WriteElementString("Number", Text.TrimToLength(payment.GiftCardNumber, 20));
                xw.WriteElementString("FaceValue", payment.Amount.ToString("0.00"));
                xw.WriteEndElement(); // End Gift Card
            }
            if (payment.Action == MerchantTribe.Payment.ActionType.RewardPointsCapture ||
                payment.Action == MerchantTribe.Payment.ActionType.RewardPointsDecrease ||
                payment.Action == MerchantTribe.Payment.ActionType.RewardPointsHold ||
                payment.Action == MerchantTribe.Payment.ActionType.RewardPointsIncrease ||
                payment.Action == MerchantTribe.Payment.ActionType.RewardPointsInfo)
            {
                //StoreCredit
                xw.WriteStartElement("StoreCredit");
                xw.WriteElementString("Total", payment.Amount.ToString("0.00"));
                switch (payment.Action)
                {
                    case MerchantTribe.Payment.ActionType.RewardPointsCapture:
                        xw.WriteElementString("Description", "Capture Reserved Points from Customer");
                        break;
                    case MerchantTribe.Payment.ActionType.RewardPointsDecrease:
                        xw.WriteElementString("Description", "Decrease Points from Customer");
                        break;
                    case MerchantTribe.Payment.ActionType.RewardPointsHold:
                        xw.WriteElementString("Description", "Reserve Points from Customer");
                        break;
                    case MerchantTribe.Payment.ActionType.RewardPointsIncrease:
                        xw.WriteElementString("Description", "Issue Points to Customer");
                        break;
                    case MerchantTribe.Payment.ActionType.RewardPointsInfo:
                        xw.WriteElementString("Description", "Info Only");
                        break;
                }
                xw.WriteEndElement();
            }

        }


        // Products
        private ActionResult ProductsCount()
        {
            int returnCount = 0;
            returnCount = this.MTApp.CatalogServices.Products.FindAllCount();
            return Content(String.Format("SETIResponse: itemcount={0}", returnCount), "text/plain");
        }
        private ActionResult ProductsDownload(bool onlyQOH)
        {
            string formStartNum = Request["startnum"] ?? string.Empty;
            string formBatchSize = Request["batchsize"] ?? string.Empty;
            string formDKey = Request["dkey"] ?? string.Empty;

            // Get Page Size
            int tempPageSize = 100;
            if (formBatchSize.Length > 0)
            {
                int.TryParse(formBatchSize, out tempPageSize);
            }
            if (tempPageSize < 1) tempPageSize = 100;

            // Get Page Number
            int pageNumber = 1;
            int tempStartRecord = 1;
            if (formStartNum.Length > 0)
            {
                if (int.TryParse(formStartNum, out tempStartRecord))
                {
                    // Stone Edge passes in a 1 based number of record to start with
                    // subtract one to make it zero based, then divide by page size to get 
                    // page number
                    pageNumber = (tempStartRecord - 1) / tempPageSize;

                    // Increase page number because pages are 1-based in MerchantTribe
                    pageNumber += 1;
                }
            }
            if (pageNumber < 1) pageNumber = 1;

            var results = this.MTApp.CatalogServices.Products.FindAllPaged(pageNumber, tempPageSize);
            if (onlyQOH)
            {
                return RenderQOH(results);
            }            
            return RenderProducts(results);
        }
        private ActionResult RenderProducts(List<MerchantTribe.Commerce.Catalog.Product> products)
        {
            StringBuilder sb = new StringBuilder();
            using (TextWriter writer = new StringWriter(sb))
            {
                using (XmlTextWriter xw = new XmlTextWriter(writer))
                {
                    xw.WriteStartDocument();

                    xw.WriteStartElement("SETIProducts");

                    if (products.Count == 0)
                    {
                        xw.WriteStartElement("Response");
                        xw.WriteElementString("ResponseCode", "2");
                        xw.WriteElementString("ResponseDescription", "success");
                        xw.WriteEndElement();
                    }
                    else
                    {
                        xw.WriteStartElement("Response");
                        xw.WriteElementString("ResponseCode", "1");
                        xw.WriteElementString("ResponseDescription", "success");
                        xw.WriteEndElement();

                        foreach (var p in products)
                        {
                            xw.WriteStartElement("Product");

                            xw.WriteElementString("Code", Text.TrimToLength(p.Sku, 255));
                            xw.WriteElementString("WebId", Text.TrimToLength(p.Bvin, 50));
                            xw.WriteElementString("Name", Text.TrimToLength(p.ProductName, 200));
                            xw.WriteElementString("Price", p.SitePrice.ToString("0.00"));
                            xw.WriteElementString("Cost", p.SiteCost.ToString("0.00"));
                            xw.WriteElementString("Description", Text.TrimToLength(p.LongDescription, 4096));
                            xw.WriteElementString("Weight", p.ShippingDetails.Weight.ToString());
                            string smallImageUrl = MerchantTribe.Commerce.Storage.DiskStorage.ProductImageUrlSmall(this.MTApp, p.Bvin, p.ImageFileSmall, false);
                            string mediumImageUrl = MerchantTribe.Commerce.Storage.DiskStorage.ProductImageUrlMedium(this.MTApp, p.Bvin, p.ImageFileSmall, false);
                            xw.WriteElementString("Thumb", Text.TrimToLength(smallImageUrl, 250));
                            xw.WriteElementString("Image", Text.TrimToLength(smallImageUrl, 250));
                            xw.WriteElementString("Taxable", p.TaxExempt ? "No" : "Yes");
                            xw.WriteElementString("Discontinued", p.Status == MerchantTribe.Commerce.Catalog.ProductStatus.Disabled ? "Yes" : "No");

                            var inventories = MTApp.CatalogServices.ProductInventories.FindByProductId(p.Bvin);
                            if (inventories.Count > 0)
                            {
                                xw.WriteElementString("QOH", inventories[0].QuantityOnHand.ToString());
                            }

                            if (p.HasOptions())
                            {
                                xw.WriteStartElement("OptionLists");

                                int sortOrder = 0;

                                foreach (var opt in p.Options)
                                {
                                    xw.WriteStartElement("ProductOption");

                                    //xw.WriteElementString("WebId", opt.Bvin); // MerchantTribe doesn't have an int value
                                    xw.WriteElementString("Name", Text.TrimToLength(opt.Name, 50));
                                    xw.WriteElementString("Prompt", Text.TrimToLength(opt.Name, 50));
                                    switch (opt.OptionType)
                                    {
                                        case MerchantTribe.Commerce.Catalog.OptionTypes.CheckBoxes:
                                            xw.WriteElementString("Type", "checkbox");
                                            break;
                                        case MerchantTribe.Commerce.Catalog.OptionTypes.DropDownList:
                                            xw.WriteElementString("Type", "select");
                                            break;
                                        case MerchantTribe.Commerce.Catalog.OptionTypes.FileUpload:
                                            xw.WriteElementString("Type", "memo");
                                            break;
                                        case MerchantTribe.Commerce.Catalog.OptionTypes.Html:
                                            xw.WriteElementString("Type", "memo");
                                            break;
                                        case MerchantTribe.Commerce.Catalog.OptionTypes.RadioButtonList:
                                            xw.WriteElementString("Type", "radio");
                                            break;
                                        case MerchantTribe.Commerce.Catalog.OptionTypes.TextInput:
                                            xw.WriteElementString("Type", "text");
                                            break;
                                        default:
                                            xw.WriteElementString("Type", "text");
                                            break;
                                    }
                                    //xw.WriteElementString("Price", opt.PriceAdjustment); // Doesn't apply, options display instead
                                    xw.WriteElementString("SortOrder", sortOrder.ToString());
                                    foreach (var optItem in opt.Items)
                                    {
                                        xw.WriteStartElement("OptionValue");
                                        
                                        xw.WriteElementString("Name", Text.TrimToLength(optItem.Name, 250));                                        
                                        xw.WriteElementString("Code", optItem.Bvin);
                                        xw.WriteElementString("Prompt", optItem.Name);
                                        xw.WriteElementString("Price", optItem.PriceAdjustment.ToString("0.00"));
                                        xw.WriteElementString("Weight", optItem.WeightAdjustment.ToString());
                                        xw.WriteElementString("SortOrder", optItem.SortOrder.ToString());

                                        xw.WriteEndElement(); // OptionValue
                                    }
                                    
                                    xw.WriteEndElement(); // End ProductOption
                                    sortOrder += 1;
                                }

                                xw.WriteEndElement(); // End OptionsList
                            }
                            
                            xw.WriteStartElement("CustomFields");

                            xw.WriteStartElement("CustomField");
                            xw.WriteElementString("FieldName", "HasVariants");
                            xw.WriteElementString("FieldValue", p.HasVariants() ? "Yes" : "No");
                            xw.WriteEndElement(); // End CustomField
                            xw.WriteStartElement("CustomField");
                            xw.WriteElementString("FieldName", "VendorId");
                            xw.WriteElementString("FieldValue", p.VendorId);
                            xw.WriteEndElement(); // End CustomField
                            xw.WriteStartElement("CustomField");
                            xw.WriteElementString("FieldName", "ManufacturerId");
                            xw.WriteElementString("FieldValue", p.ManufacturerId);
                            xw.WriteEndElement(); // End CustomField
                            xw.WriteStartElement("CustomField");
                            xw.WriteElementString("FieldName", "ProductTypeId");
                            xw.WriteElementString("FieldValue", p.ProductTypeId);
                            xw.WriteEndElement(); // End CustomField
                            xw.WriteStartElement("CustomField");
                            xw.WriteElementString("FieldName", "CreationDateUtc");
                            xw.WriteElementString("FieldValue", p.CreationDateUtc.ToString());
                            xw.WriteEndElement(); // End CustomField

                            xw.WriteEndElement(); // End CustomFields

                            xw.WriteEndElement(); // End Product;
                        }
                    }

                    xw.WriteEndElement(); // End SetiProducts;

                    xw.WriteEndDocument();
                    xw.Flush();
                }
            }

            return Content(sb.ToString(), "application/xml");
        }        
        private ActionResult RenderQOH(List<MerchantTribe.Commerce.Catalog.Product> products)
        {
            StringBuilder sb = new StringBuilder();
            using (TextWriter writer = new StringWriter(sb))
            {
                using (XmlTextWriter xw = new XmlTextWriter(writer))
                {
                    xw.WriteStartDocument();

                    xw.WriteStartElement("SETIProducts");

                    if (products.Count == 0)
                    {
                        xw.WriteStartElement("Response");
                        xw.WriteElementString("ResponseCode", "2");
                        xw.WriteElementString("ResponseDescription", "success");
                        xw.WriteEndElement();
                    }
                    else
                    {
                        xw.WriteStartElement("Response");
                        xw.WriteElementString("ResponseCode", "1");
                        xw.WriteElementString("ResponseDescription", "success");
                        xw.WriteEndElement();

                        foreach (var p in products)
                        {
                            xw.WriteStartElement("Product");
                            
                            var inventories = MTApp.CatalogServices.ProductInventories.FindByProductId(p.Bvin);
                            if (inventories.Count > 0)
                            {
                                xw.WriteElementString("Code", Text.TrimToLength(p.Sku, 255));
                                xw.WriteElementString("WebId", Text.TrimToLength(p.Bvin, 50));
                                xw.WriteElementString("QOH", inventories[0].QuantityOnHand.ToString());
                            }
                                                                                
                            xw.WriteEndElement(); // End Product;
                        }
                    }

                    xw.WriteEndElement(); // End SetiProducts;

                    xw.WriteEndDocument();
                    xw.Flush();
                }
            }

            return Content(sb.ToString(), "application/xml");
        }

        // Customers
        private ActionResult CustomersCount()
        {
            int returnCount = 0;
            var results = this.MTApp.MembershipServices.Customers.FindAllPaged(1, 1, ref returnCount);
            return Content(String.Format("SETIResponse: itemcount={0}", returnCount), "text/plain");
        }
        private ActionResult CustomersDownload()
        {
            string formStartNum = Request["startnum"] ?? string.Empty;
            string formBatchSize = Request["batchsize"] ?? string.Empty;
            string formDKey = Request["dkey"] ?? string.Empty;

            // Get Page Size
            int tempPageSize = 100;
            if (formBatchSize.Length > 0)
            {
                int.TryParse(formBatchSize, out tempPageSize);
            }
            if (tempPageSize < 1) tempPageSize = 100;

            // Get Page Number
            int pageNumber = 1;
            int tempStartRecord = 1;
            if (formStartNum.Length > 0)
            {
                if (int.TryParse(formStartNum, out tempStartRecord))
                {
                    // Stone Edge passes in a 1 based number of record to start with
                    // subtract one to make it zero based, then divide by page size to get 
                    // page number
                    pageNumber = (tempStartRecord - 1) / tempPageSize;

                    // Increase page number because pages are 1-based in MerchantTribe
                    pageNumber += 1;
                }
            }
            if (pageNumber < 1) pageNumber = 1;
            
            var results = this.MTApp.MembershipServices.Customers.FindAllPaged(pageNumber, tempPageSize);
            return RenderCustomers(results);
        }
        private ActionResult RenderCustomers(List<MerchantTribe.Commerce.Membership.CustomerAccount> customers)
        {
            StringBuilder sb = new StringBuilder();
            using (TextWriter writer = new StringWriter(sb))
            {
                using (XmlTextWriter xw = new XmlTextWriter(writer))
                {
                    xw.WriteStartDocument();

                    xw.WriteStartElement("SETICustomers");

                    if (customers.Count == 0)
                    {
                        xw.WriteStartElement("Response");
                        xw.WriteElementString("ResponseCode", "2");
                        xw.WriteElementString("ResponseDescription", "success");
                        xw.WriteEndElement();
                    }
                    else
                    {
                        xw.WriteStartElement("Response");
                        xw.WriteElementString("ResponseCode", "1");
                        xw.WriteElementString("ResponseDescription", "success");
                        xw.WriteEndElement();

                        foreach (var c in customers)
                        {
                            xw.WriteStartElement("Customer");

                            //xw.WriteElementString("WebId", c.Id); // MerchantTribe doesn't have a int Id for customers
                            xw.WriteElementString("UserName", Text.TrimToLength(c.Email, 25));
                            //xw.WriteElementString("Password", Text.TrimToLength(c.Password, 50)); // We don't want to send passwords from MT
                            //xw.WriteElementString("AffiliateId", c.AffiliateId); // MerchantTribe doesn't tag users with Affiliate Ids, Just orders.

                            
                            if (c.BillingAddress != null)
                            {
                                xw.WriteStartElement("BillAddr");

                                xw.WriteElementString("FirstName", Text.TrimToLength(c.BillingAddress.FirstName, 50));
                                xw.WriteElementString("MiddleName", Text.TrimToLength(c.BillingAddress.MiddleInitial, 50));
                                xw.WriteElementString("LastName", Text.TrimToLength(c.BillingAddress.LastName, 50));
                                xw.WriteElementString("CompanyName", Text.TrimToLength(c.BillingAddress.Company, 50));
                                xw.WriteElementString("Phone", Text.TrimToLength(c.BillingAddress.Phone, 50));
                                xw.WriteElementString("Fax", Text.TrimToLength(c.BillingAddress.Fax, 50));
                                xw.WriteElementString("Email", Text.TrimToLength(c.Email, 255));

                                xw.WriteStartElement("Address");
                                xw.WriteElementString("Addr1", Text.TrimToLength(c.BillingAddress.Street, 255));
                                xw.WriteElementString("Addr2", Text.TrimToLength(c.BillingAddress.Street2, 255));
                                xw.WriteElementString("City", Text.TrimToLength(c.BillingAddress.City, 255));
                                xw.WriteElementString("State", Text.TrimToLength(c.BillingAddress.RegionData.Abbreviation, 2));
                                xw.WriteElementString("Zip", Text.TrimToLength(c.BillingAddress.PostalCode, 255));
                                var country = MerchantTribe.Web.Geography.Country.FindByBvin(c.BillingAddress.CountryBvin);
                                xw.WriteElementString("Country", Text.TrimToLength(country.IsoCode, 2));
                                xw.WriteEndElement(); // End Address

                                xw.WriteEndElement(); // End BillAddr
                            }                            
                                                        
                            if (c.ShippingAddress != null)
                            {
                                xw.WriteStartElement("ShipAddr");

                                xw.WriteElementString("FirstName", Text.TrimToLength(c.ShippingAddress.FirstName, 50));
                                xw.WriteElementString("MiddleName", Text.TrimToLength(c.ShippingAddress.MiddleInitial, 50));
                                xw.WriteElementString("LastName", Text.TrimToLength(c.ShippingAddress.LastName, 50));
                                xw.WriteElementString("CompanyName", Text.TrimToLength(c.ShippingAddress.Company, 50));
                                xw.WriteElementString("Phone", Text.TrimToLength(c.ShippingAddress.Phone, 50));
                                xw.WriteElementString("Fax", Text.TrimToLength(c.ShippingAddress.Fax, 50));
                                xw.WriteElementString("Email", Text.TrimToLength(c.Email, 255));

                                xw.WriteStartElement("Address");
                                xw.WriteElementString("Addr1", Text.TrimToLength(c.ShippingAddress.Street, 255));
                                xw.WriteElementString("Addr2", Text.TrimToLength(c.ShippingAddress.Street2, 255));
                                xw.WriteElementString("City", Text.TrimToLength(c.ShippingAddress.City, 255));
                                xw.WriteElementString("State", Text.TrimToLength(c.ShippingAddress.RegionData.Abbreviation, 2));
                                xw.WriteElementString("Zip", Text.TrimToLength(c.ShippingAddress.PostalCode, 255));
                                var country = MerchantTribe.Web.Geography.Country.FindByBvin(c.ShippingAddress.CountryBvin);
                                xw.WriteElementString("Country", Text.TrimToLength(country.IsoCode, 2));
                                xw.WriteEndElement(); // End Address

                                xw.WriteEndElement(); // End BillAddr
                            }

                            xw.WriteStartElement("CustomFields");
                            
                            xw.WriteStartElement("CustomField");
                            xw.WriteElementString("FieldName", "bvin");
                            xw.WriteElementString("FieldValue", c.Bvin);
                            xw.WriteEndElement(); // End CustomField

                            xw.WriteEndElement(); // End CustomFields

                            xw.WriteEndElement(); // End Customer;
                        }
                    }

                    xw.WriteEndElement(); // End SetiCustomers;

                    xw.WriteEndDocument();
                    xw.Flush();
                }
            }

            return Content(sb.ToString(), "application/xml");
        }


        // Quantity On Hand Updates
        private ActionResult QohReplace()
        {
            string data = Request["update"] ?? string.Empty;
            data = data.Trim();
            Dictionary<string, int> UpdateData = new Dictionary<string, int>();
            if (data.StartsWith("<"))
            {
                UpdateData = ParseInventoryXml(data);
            }
            else
            {
                UpdateData = ParseInventoryDelimited(data);
            }

            StringBuilder sb = new StringBuilder();
            sb.Append("SETIResponse\n");
            foreach (var item in UpdateData)
            {
                var p = MTApp.CatalogServices.Products.FindBySku(item.Key);
                if (p == null)
                {
                    sb.Append(item.Key + "=NF\n");
                }
                else
                {
                    MTApp.CatalogServices.InventorySetAvailableQuantity(p.Bvin, string.Empty, item.Value);
                    sb.Append(item.Key + "=OK\n");
                }
            }
            sb.Append("SETIEndOfData\n");
            
            return Content(sb.ToString(), "text/plain");
        }
        private ActionResult InventoryUpdate()
        {
            string data = Request["update"] ?? string.Empty;
            data = data.Trim();
            Dictionary<string, int> UpdateData = new Dictionary<string, int>();
            if (data.StartsWith("<"))
            {
                UpdateData = ParseInventoryXml(data);
            }
            else
            {
                UpdateData = ParseInventoryDelimited(data);
            }

            string Note = string.Empty;
            string QOH = "NF";

            bool hasError = false;
            if (UpdateData.Count < 1)
            {
                Note = "Could not parse data";
                QOH = "NF";
                hasError = true;
            }
            else
            {
                var p = MTApp.CatalogServices.Products.FindBySku(UpdateData.First().Key);
                if (p == null)
                {
                    hasError = true;
                    Note = "Product Not Found";
                    QOH = "NF";
                }
                else
                {
                    if (p.InventoryMode == MerchantTribe.Commerce.Catalog.ProductInventoryMode.AlwayInStock)
                    {
                        QOH = "NA";
                    }                    
                    MTApp.CatalogServices.InventoryAdjustAvailableQuantity(p.Bvin, string.Empty, UpdateData.First().Value);
                    int available = MTApp.CatalogServices.InventoryQuantityAvailableForPurchase(p, string.Empty);
                    QOH = available.ToString();
                }
            }

            StringBuilder sb = new StringBuilder();
            if (hasError)
            {
                sb.Append("SETIResponse=False;");
            }
            else
            {
                sb.Append("SETIResponse=OK;");
            }
            sb.Append("SKU=" + UpdateData.First().Key + ";");
            sb.Append("QOH=" + QOH + ";");
            sb.Append("NOTE=" + Note);
            
            return Content(sb.ToString(), "text/plain");
        }
        private Dictionary<string, int> ParseInventoryXml(string data)
        {
            Dictionary<string, int> result = new Dictionary<string, int>();

            try
            {
                XElement xml = XElement.Parse(data);
                foreach (XElement child in xml.Elements())
                {
                    if (child.Name == "Product")
                    {
                        if (child.HasElements)
                        {
                            var sku = child.Element("SKU");
                            var qty = child.Element("QOH");                                                        
                            if (sku == null) continue;
                            if (qty == null) continue;
                            string itemSku = sku.Value;
                            string itemQtyString = qty.Value;
                            int tempQty = 0;
                            if (int.TryParse(itemQtyString, out tempQty))
                            {
                                result.Add(itemSku, tempQty);
                            }
                        }
                    }
                }
            }
            catch
            {

            }
            return result;            
        }
        private Dictionary<string, int> ParseInventoryDelimited(string data)
        {
            Dictionary<string, int> result = new Dictionary<string, int>();

            string[] bigParts = data.Split('|');
            foreach (string part in bigParts)
            {
                string[] itemData = part.Split('~');
                if (itemData.Length > 1)
                {
                    int qty = 0;
                    int.TryParse(itemData[1], out qty);
                    result.Add(itemData[0], qty);
                }
            }

            return result;                        
        }

        // Update Order Tracking
        private ActionResult UpdateOrder()
        {
            string data = Request["update"] ?? string.Empty;
            if (data != string.Empty)
            {
                return UpdateOrderXml(data);
            }
            else
            {
                return UpdateOrderDelimited();
            }            
        }
        private ActionResult UpdateOrderXml(string data)
        {
            try
            {
                XElement xml = XElement.Parse(data);
                foreach (XElement order in xml.Elements("Order"))
                {
                    string orderNumber = order.TryGetElementValue("OrderNumber", "");
                    string stoneEdgeNumber = order.TryGetElementValue("ReferenceNumber", "");
                    string orderStatus = order.TryGetElementValue("Status", "");
                    string notes = order.TryGetElementValue("Notes", "");
                    string comments = order.TryGetElementValue("Comments", "");
                    string stringChangeDate = order.TryGetElementValue("ChangeDateTime", "");

                    // Find Order
                    var o = MTApp.OrderServices.Orders.FindByOrderNumber(orderNumber);
                    if (o == null)
                    {
                        return Content("SETIResponse: update=False;Notes=Order " + orderNumber + " could not be found", "text/plain");
                    }


                    List<OrderPackage> packages = new List<OrderPackage>();
                    XElement xpackages = order.Element("Packages");
                    if (xpackages != null)
                    {
                        foreach (var xpak in xpackages.Elements("Package"))
                        {
                            OrderPackage pak = new OrderPackage();

                            pak.ThirdPartyId = xpak.TryGetElementValue("PackageID", "");
                            pak.TrackingNumber = xpak.TryGetElementValue("TrackingID", "");
                            string trackingPickupDate = xpak.TryGetElementValue("PickupDate", "");
                            DateTime temp = new DateTime();
                            if (DateTime.TryParse(trackingPickupDate, out temp))
                            {
                                pak.ShipDateUtc = TimeZoneInfo.ConvertTimeToUtc(temp, MTApp.CurrentStore.Settings.TimeZone);
                            }
                            else
                            {
                                pak.ShipDateUtc = DateTime.UtcNow;
                            }
                            pak.ShippingProviderId = xpak.TryGetElementValue("Shipper", "Unknown");
                            pak.ShippingMethodId = xpak.TryGetElementValue("Method", "Unknown");                            
                            pak.Description = "Carrier: " + pak.ShippingProviderId + " | Imported from Stone Edge Ref:# " + stoneEdgeNumber + " at " + DateTime.Now;
                            pak.HasShipped = true;                            
                                                                                  
                            packages.Add(pak);
                        }
                    }

                    // Line Item Details
                    if (packages.Count > 0)
                    {
                        XElement xitems = order.Element("Items");
                        if (xitems != null)
                        {
                            foreach (XElement xitem in xitems.Elements("Item"))
                            {
                                long lineItemId = xitem.TryGetElementValueAsLong("ItemNumber", 0);
                                string stoneEdgeItemId = xitem.TryGetElementValue("RefNumber", "");
                                string status = xitem.TryGetElementValue("Status", "");
                                int CountOrdered = xitem.TryGetElementValueAsInt("Ordered", 0);
                                int CountShipped = xitem.TryGetElementValueAsInt("Shipped", 0);
                                int CountNeeded = xitem.TryGetElementValueAsInt("Needed", 0);
                                string note = xitem.TryGetElementValue("Notes", "");

                                XElement itemPackages = xitem.Element("Packages");
                                if (itemPackages != null)
                                {
                                    foreach (var itemPackage in itemPackages.Elements("Package"))
                                    {
                                        string packageId = itemPackage.TryGetElementValue("PackageID", "");
                                        int quantity = itemPackage.TryGetElementValueAsInt("Quantity", 0);

                                        if (packageId.Trim().Length > 0 && quantity > 0)
                                        {
                                            // Update the Package with line item quantity
                                            var thePak = packages.Where(y => y.ThirdPartyId == packageId).FirstOrDefault();
                                            if (thePak != null)
                                            {
                                                // Get Product Bvin from Line Item
                                                string pid = string.Empty;
                                                var li = o.Items.Where(y => y.Id == lineItemId).FirstOrDefault();
                                                if (li != null) pid = li.ProductId;
                                                
                                                thePak.Items.Add(new OrderPackageItem()
                                                {
                                                    LineItemId = lineItemId,
                                                    ProductBvin = pid,
                                                    Quantity = quantity
                                                });
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    

                    // Update Status
                    var statusMatch = OrderStatusCode.FindAll().Where(y => y.StatusName.ToUpperInvariant() == orderStatus.ToUpperInvariant()).FirstOrDefault();
                    if (statusMatch != null)
                    {
                        o.StatusCode = statusMatch.Bvin;
                        o.StatusName = statusMatch.StatusName;

                    }

                    // Add Packages
                    if (packages.Count > 0)
                    {
                        o.Packages.AddRange(packages);   
                    }

                    if (!MTApp.OrderServices.Orders.Update(o))
                    {
                        return Content("SETIResponse: update=false;Notes=Error While Saving Order", "text/plain");
                    }

                    if (packages.Count > 0)
                    {
                        foreach (var p in packages)
                        {
                            MTApp.OrdersShipPackage(p, o);
                        }
                        MTApp.OrderServices.Orders.Update(o);
                    }
                    
                }
                
                return Content("SETIResponse: update=OK;Notes=", "text/plain");
            }
            catch (Exception ex)
            {
                return Content("SETIResponse: update=False;Notes=Exception:" + ex.Message, "text/plain");
            }            
        }

        private ActionResult UpdateOrderDelimited()
        {            
            string orderNumber = Request["ordernumber"] ?? string.Empty;
            string orderStatus = Request["orderstatus"] ?? string.Empty;
            string stoneEdgeNumber = Request["refnumber"] ?? string.Empty;
            string notes = Request["orderdetail"] ?? string.Empty;

            List<StoneEdgeTrackingData> trackingData = new List<StoneEdgeTrackingData>();

            try
            {
                string trackingCount = Request["trackcount"] ?? string.Empty;
                int count = 0;
                int.TryParse(trackingCount, out count);

                // Parse Package Data
                if (count == 1)
                {
                    StoneEdgeTrackingData t = new StoneEdgeTrackingData();
                    t.TrackingNumber = Request["tracknum"] ?? string.Empty;
                    t.Carrier = Request["trackcarrier"] ?? string.Empty;
                    string trackingPickupDate = Request["trackpickupdate"] ?? string.Empty;
                    DateTime temp = new DateTime();
                    if (DateTime.TryParse(trackingPickupDate, out temp))
                    {
                        t.PickUpDate = temp;
                    }
                    trackingData.Add(t);
                }
                else
                {
                    for (int i = 1; i <= count; i++)
                    {
                        StoneEdgeTrackingData t = new StoneEdgeTrackingData();
                        t.TrackingNumber = Request["tracknum" + i] ?? string.Empty;
                        t.Carrier = Request["trackcarrier" + 1] ?? string.Empty;
                        string trackingPickupDate = Request["trackpickupdate" + 1] ?? string.Empty;
                        DateTime temp = new DateTime();
                        if (DateTime.TryParse(trackingPickupDate, out temp))
                        {
                            t.PickUpDate = temp;
                        }
                        trackingData.Add(t);
                    }
                }

                // Find Order
                var o = MTApp.OrderServices.Orders.FindByOrderNumber(orderNumber);
                if (o == null)
                {
                    return Content("SETIResponse: update=False;Notes=Order " + orderNumber + " could not be found", "text/plain");
                }

                // Update Status
                var statusMatch = OrderStatusCode.FindAll().Where(y => y.StatusName.ToUpperInvariant() == orderStatus.ToUpperInvariant()).FirstOrDefault();
                if (statusMatch != null)
                {
                    o.StatusCode = statusMatch.Bvin;
                    o.StatusName = statusMatch.StatusName;

                }

                // Packages                
                foreach (var data in trackingData)
                {
                    OrderPackage pak = new OrderPackage();
                    pak.Description = "Carrier: " + data.Carrier + " | Imported from Stone Edge Ref:# " + stoneEdgeNumber + " at " + DateTime.Now;
                    pak.HasShipped = true;
                    pak.OrderId = o.bvin;
                    if (data.PickUpDate.HasValue)
                    {
                        pak.ShipDateUtc = TimeZoneInfo.ConvertTimeToUtc(data.PickUpDate.Value, MTApp.CurrentStore.Settings.TimeZone);
                    }
                    else
                    {
                        pak.ShipDateUtc = DateTime.UtcNow;
                    }
                    pak.ShippingMethodId = data.Carrier;
                    pak.ShippingProviderId = data.Carrier;
                    pak.TrackingNumber = data.TrackingNumber;                    
                    o.Packages.Add(pak);
                }

                if (MTApp.OrderServices.Orders.Update(o))
                {
                    return Content("SETIResponse: update=OK;Notes=", "text/plain");
                }
                return Content("SETIResponse: update=false;Notes=Error While Saving Order", "text/plain");
            }
            catch (Exception ex)
            {
                return Content("SETIResponse: update=False;Notes=Exception:" + ex.Message, "text/plain");
            }            
        }
        
    }
}
