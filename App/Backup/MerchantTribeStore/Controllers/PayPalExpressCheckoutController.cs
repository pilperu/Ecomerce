using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MerchantTribe.Commerce;
using MerchantTribe.Commerce.Membership;
using MerchantTribeStore.Controllers.Shared;
using MerchantTribe.Commerce.Orders;
using MerchantTribeStore.Filters;
using MerchantTribe.Commerce.Catalog;
using MerchantTribeStore.Models;
using MerchantTribe.Commerce.Content;
using MerchantTribe.Commerce.Contacts;
using MerchantTribe.Commerce.Payment;
using MerchantTribe.Commerce.BusinessRules;
using MerchantTribe.Web.Logging;
using MerchantTribe.Web.Validation;
using MerchantTribe.Payment;
using MerchantTribe.PaypalWebServices;
using MerchantTribe.Web.Geography;
using com.paypal.soap.api;
using MerchantTribe.Commerce.Catalog;
using MerchantTribe.Web.Logging;
using MerchantTribe.Commerce.Utilities;
using MerchantTribe.Commerce.Shipping;

namespace MerchantTribeStore.Controllers
{
    public class PayPalExpressCheckoutController : BaseStoreController
    {
        private CheckoutViewModel IndexSetup()
        {
            ViewBag.Title = "Checkout";
            ViewBag.BodyClass = "store-checkout-page";

            CheckoutViewModel model = new CheckoutViewModel();
            model.CurrentOrder = SessionManager.CurrentShoppingCart(MTApp.OrderServices, MTApp.CurrentStore);

            // Buttons
            ThemeManager themes = MTApp.ThemeManager();
            model.ButtonCheckoutUrl = themes.ButtonUrl("PlaceOrder", Request.IsSecureConnection);            
            model.ButtonCancelUrl = themes.ButtonUrl("keepshopping", Request.IsSecureConnection);
            model.ButtonLoginUrl = themes.ButtonUrl("edit", Request.IsSecureConnection);

            // Agree Checkbox
            if (MTApp.CurrentStore.Settings.ForceTermsAgreement)
            {
                model.ShowAgreeToTerms = true;
                model.AgreedToTerms = false;
                model.AgreedToTermsDescription = SiteTerms.GetTerm(SiteTermIds.TermsAndConditionsAgreement);
                model.LabelTerms = SiteTerms.GetTerm(SiteTermIds.TermsAndConditions);
            }
            else
            {
                model.ShowAgreeToTerms = false;
                model.AgreedToTerms = true;
            }

            // Populate Countries
            model.Countries = MTApp.CurrentStore.Settings.FindActiveCountries();

            // Side Column
            var columnRender = new code.TemplateEngine.TagHandlers.ContentColumn();
            model.SideColumn = columnRender.RenderColumnToString("601", MTApp, ViewBag);

            return model;
        }        
        private void DisplayPaypalExpressMode(CheckoutViewModel model)
        {
            model.PayPalToken = Request.QueryString["token"] ?? string.Empty;
            model.PayPalPayerId = Request.QueryString["payerId"] ?? string.Empty;
            
            if (string.IsNullOrEmpty(model.PayPalToken)) Response.Redirect("~/cart");

            
                PayPalAPI ppAPI = MerchantTribe.Commerce.Utilities.PaypalExpressUtilities.GetPaypalAPI(this.MTApp.CurrentStore);
                bool failed = false;
                GetExpressCheckoutDetailsResponseType ppResponse = null;
                try
                {
                        if (!GetExpressCheckoutDetails(ppAPI, ref ppResponse, model))
                        {
                            failed = true;
                            EventLog.LogEvent("Paypal Express Checkout", "GetExpressCheckoutDetails call failed. Detailed Errors will follow. ", EventLogSeverity.Error);
                            foreach (ErrorType ppError in ppResponse.Errors)
                            {
                                EventLog.LogEvent("Paypal error number: " + ppError.ErrorCode, "Paypal Error: '" + ppError.ShortMessage + "' Message: '" + ppError.LongMessage + "' " + " Values passed to GetExpressCheckoutDetails: Token: " + model.PayPalToken, EventLogSeverity.Error);
                            }
                            this.FlashFailure("An error occurred during the Paypal Express checkout. No charges have been made. Please try again.");
                            ViewBag.HideCheckout = true;
                        }
                }
                finally
                {
                    Order o = model.CurrentOrder;
                    ViewBag.HideEditButton = false;
                    if (o.CustomProperties["PaypalAddressOverride"] != null)
                    {
                        if (o.CustomProperties["PaypalAddressOverride"].Value == "1")
                        {
                            ViewBag.HideEditButton = true;
                        }
                    }

                    o.CustomProperties.Add("bvsoftware", "PayerID", Request.QueryString["PayerID"]);
                    if (!failed)
                    {
                        if (ppResponse != null && ppResponse.GetExpressCheckoutDetailsResponseDetails != null && ppResponse.GetExpressCheckoutDetailsResponseDetails.PayerInfo != null)
                        {
                            o.UserEmail = ppResponse.GetExpressCheckoutDetailsResponseDetails.PayerInfo.Payer;
                            if (string.IsNullOrEmpty(o.ShippingAddress.Phone))
                            {
                                o.ShippingAddress.Phone = ppResponse.GetExpressCheckoutDetailsResponseDetails.ContactPhone ?? string.Empty;
                            }
                        }
                    }
                    MTApp.OrderServices.Orders.Update(o);
                    ppAPI = null;
                }                        
        }
        private bool GetExpressCheckoutDetails(PayPalAPI ppAPI, ref GetExpressCheckoutDetailsResponseType ppResponse, CheckoutViewModel model)
        {
            ppResponse = ppAPI.GetExpressCheckoutDetails(Request.QueryString["token"]);
            if (ppResponse.Ack == AckCodeType.Success || ppResponse.Ack == AckCodeType.SuccessWithWarning)
            {
                model.CurrentOrder.UserEmail = ppResponse.GetExpressCheckoutDetailsResponseDetails.PayerInfo.Payer;
                model.CurrentOrder.ShippingAddress.FirstName = ppResponse.GetExpressCheckoutDetailsResponseDetails.PayerInfo.PayerName.FirstName ?? string.Empty;
                if (ppResponse.GetExpressCheckoutDetailsResponseDetails.PayerInfo.PayerName.MiddleName.Length > 0)
                {
                    model.CurrentOrder.ShippingAddress.MiddleInitial = ppResponse.GetExpressCheckoutDetailsResponseDetails.PayerInfo.PayerName.MiddleName.Substring(0, 1);
                }
                else
                {
                    model.CurrentOrder.ShippingAddress.MiddleInitial = string.Empty;
                }
                model.CurrentOrder.ShippingAddress.LastName = ppResponse.GetExpressCheckoutDetailsResponseDetails.PayerInfo.PayerName.LastName ?? string.Empty;
                model.CurrentOrder.ShippingAddress.Company = ppResponse.GetExpressCheckoutDetailsResponseDetails.PayerInfo.PayerBusiness ?? string.Empty;
                model.CurrentOrder.ShippingAddress.Line1 = ppResponse.GetExpressCheckoutDetailsResponseDetails.PayerInfo.Address.Street1 ?? string.Empty;
                model.CurrentOrder.ShippingAddress.Line2 = ppResponse.GetExpressCheckoutDetailsResponseDetails.PayerInfo.Address.Street2 ?? string.Empty;
                model.CurrentOrder.ShippingAddress.CountryData.Name = ppResponse.GetExpressCheckoutDetailsResponseDetails.PayerInfo.Address.CountryName ?? string.Empty;
                model.CurrentOrder.ShippingAddress.CountryData.Bvin = ppResponse.GetExpressCheckoutDetailsResponseDetails.PayerInfo.Address.Country.ToString();
                model.CurrentOrder.ShippingAddress.City = ppResponse.GetExpressCheckoutDetailsResponseDetails.PayerInfo.Address.CityName ?? string.Empty;
                model.CurrentOrder.ShippingAddress.RegionData.Name = ppResponse.GetExpressCheckoutDetailsResponseDetails.PayerInfo.Address.StateOrProvince ?? string.Empty;
                model.CurrentOrder.ShippingAddress.RegionData.Abbreviation =
                    model.CurrentOrder.ShippingAddress.RegionData.Name ?? string.Empty;
                model.CurrentOrder.ShippingAddress.PostalCode = ppResponse.GetExpressCheckoutDetailsResponseDetails.PayerInfo.Address.PostalCode ?? string.Empty;
                model.CurrentOrder.ShippingAddress.Phone = ppResponse.GetExpressCheckoutDetailsResponseDetails.PayerInfo.ContactPhone ?? string.Empty;

                if (ppResponse.GetExpressCheckoutDetailsResponseDetails.PayerInfo.Address.AddressStatus == AddressStatusCodeType.Confirmed)
                {
                    ViewBag.AddressStatus = "Confirmed";
                }
                else
                {
                    ViewBag.AddressStatus = "Unconfirmed";
                }
                return true;
            }
            else
            {
                return false;
            }
        }        
        private void LoadShippingMethodsForOrder(CheckoutViewModel model)
        {
            Order o = model.CurrentOrder;            
            o.ShippingAddress.CopyTo(o.BillingAddress);
            MTApp.CalculateOrderAndSave(o);
            SessionManager.SaveOrderCookies(o, MTApp.CurrentStore);            
            LoadShippingMethodsForOrder(o);
        }
        private void LoadShippingMethodsForOrder(Order o)
        {

            SortableCollection<ShippingRateDisplay> rates = MTApp.OrderServices.FindAvailableShippingRates(o, MTApp.CurrentStore);

            string rateKey = o.ShippingMethodUniqueKey;
            bool rateIsAvailable = false;

            // See if rate is available
            if ((rateKey.Length > 0))
            {
                foreach (MerchantTribe.Commerce.Shipping.ShippingRateDisplay r in rates)
                {
                    if ((r.UniqueKey == rateKey))
                    {
                        rateIsAvailable = true;
                        MTApp.OrderServices.OrdersRequestShippingMethod(r, o);
                    }
                }
            }

            // if it's not availabe, pick the first one or default
            if ((rateIsAvailable == false))
            {
                if ((rates.Count > 0))
                {
                    MTApp.OrderServices.OrdersRequestShippingMethod(rates[0], o);
                    rateKey = rates[0].UniqueKey;
                }
                else
                {
                    o.ClearShippingPricesAndMethod();
                }
            }

            ViewBag.ShippingRates = HtmlRendering.ShippingRatesToRadioButtons(rates, 300, o.ShippingMethodUniqueKey);            
        }

        // GET: /paypalexpresscheckout
        [NonCacheableResponseFilter]
        public ActionResult Index()
        {
            CheckoutViewModel model = IndexSetup();            
            DisplayPaypalExpressMode(model);
            LoadShippingMethodsForOrder(model);

            ViewBag.HideEditButton = true;
            return View(model);
        }

        // POST: /paypalexpresscheckout
        [NonCacheableResponseFilter]
        [ActionName("Index")]
        [HttpPost]
        public ActionResult IndexPost()
        {
            CheckoutViewModel model = IndexSetup();
            DisplayPaypalExpressMode(model);
            LoadShippingMethodsForOrder(model);

            if (ValidatePage(model))
            {
                SaveShippingSelections(model);
                // Save Payment Information
                SavePaymentInfo(model);
                MTApp.OrderServices.Orders.Update(model.CurrentOrder);

                // Save as Order
                MerchantTribe.Commerce.BusinessRules.OrderTaskContext c
                    = new MerchantTribe.Commerce.BusinessRules.OrderTaskContext(MTApp);
                c.UserId = MTApp.CurrentCustomerId;
                c.Order = model.CurrentOrder;

                if (MerchantTribe.Commerce.BusinessRules.Workflow.RunByName(c, MerchantTribe.Commerce.BusinessRules.WorkflowNames.ProcessNewOrder))
                {
                    // Clear Cart ID because we're now an order
                    SessionManager.SetCurrentCartId(MTApp.CurrentStore, string.Empty);

                    // Process Payment
                    if (MerchantTribe.Commerce.BusinessRules.Workflow.RunByName(c, MerchantTribe.Commerce.BusinessRules.WorkflowNames.ProcessNewOrderPayments))
                    {
                        MerchantTribe.Commerce.BusinessRules.Workflow.RunByName(c, MerchantTribe.Commerce.BusinessRules.WorkflowNames.ProcessNewOrderAfterPayments);
                        Order tempOrder = MTApp.OrderServices.Orders.FindForCurrentStore(model.CurrentOrder.bvin);
                        MTApp.CurrentRequestContext.IntegrationEvents.OrderReceived(tempOrder, MTApp);
                        Response.Redirect("~/checkout/receipt?id=" + model.CurrentOrder.bvin);
                    }
                }            
            }
            // Render Error Summary
            foreach (var v in model.Violations)
            {
                FlashFailure(v.ErrorMessage);
            }

            ViewBag.HideEditButton = true;
            return View(model);
        }
        private bool ValidatePage(CheckoutViewModel model)
        {
            bool result = true;
            if (!MTApp.CurrentStore.Settings.PayPal.AllowUnconfirmedAddresses)
            {
                if (string.Compare(ViewBag.AddressStatus, "Unconfirmed", true) == 0)
                {
                    model.Violations.Add(new RuleViolation() { ControlName = "", 
                        ErrorMessage = "Unconfirmed addresses are not allowed by this store.", 
                        PropertyName = "", PropertyValue = "" });
                    result = false;
                }
            }
            if (Request.Form["shippingrate"] == null)
            {
                model.Violations.Add(new RuleViolation()
                {
                    ControlName = "",
                    ErrorMessage = "Please select a shipping method.",
                    PropertyName = "",
                    PropertyValue = ""
                });
                result =  false;
            }
            if (model.AgreedToTerms == false && model.ShowAgreeToTerms == true)
            {
                model.Violations.Add(new MerchantTribe.Web.Validation.RuleViolation("Terms", "Terms", SiteTerms.GetTerm(SiteTermIds.SiteTermsAgreementError)));
                result = false;
            }
            return result;
        }

        // POST: /paypalexpresscheckout/edit
        [NonCacheableResponseFilter]
        [HttpPost]
        public ActionResult Edit()
        {
            CheckoutViewModel model = IndexSetup();
            DisplayPaypalExpressMode(model);
            LoadShippingMethodsForOrder(model);
            Order o = model.CurrentOrder;

            PayPalAPI ppAPI = MerchantTribe.Commerce.Utilities.PaypalExpressUtilities.GetPaypalAPI(this.MTApp.CurrentStore);
            try
            {
                string cartReturnUrl = MTApp.CurrentStore.RootUrlSecure() + "paypalexpresscheckout";
                string cartCancelUrl = MTApp.CurrentStore.RootUrlSecure() + "checkout";

                SetExpressCheckoutResponseType expressResponse;
                if (MTApp.CurrentStore.Settings.PayPal.ExpressAuthorizeOnly)
                {
                    expressResponse = ppAPI.SetExpressCheckout(string.Format("{0:N}", o.TotalOrderBeforeDiscounts), cartReturnUrl, cartCancelUrl, PaymentActionCodeType.Order, PayPalAPI.GetCurrencyCodeType(MTApp.CurrentStore.Settings.PayPal.Currency), o.OrderNumber);
                }
                else
                {
                    expressResponse = ppAPI.SetExpressCheckout(string.Format("{0:N}", o.TotalOrderBeforeDiscounts), cartReturnUrl, cartCancelUrl, PaymentActionCodeType.Sale, PayPalAPI.GetCurrencyCodeType(MTApp.CurrentStore.Settings.PayPal.Currency), o.OrderNumber);
                }

                if (expressResponse.Ack == AckCodeType.Success || expressResponse.Ack == AckCodeType.SuccessWithWarning)
                {
                    o.ThirdPartyOrderId = expressResponse.Token;
                    if (MTApp.OrderServices.Orders.Update(o))
                    {
                        if (string.Compare(MTApp.CurrentStore.Settings.PayPal.Mode, "Live", true) == 0)
                        {
                            System.Web.HttpContext.Current.Response.Redirect("https://www.paypal.com/cgi-bin/webscr?cmd=_express-checkout&token=" + expressResponse.Token, false);
                        }
                        else
                        {
                            System.Web.HttpContext.Current.Response.Redirect("https://www.sandbox.paypal.com/cgi-bin/webscr?cmd=_express-checkout&token=" + expressResponse.Token, false);
                        }

                    }
                }
                else
                {
                    foreach (ErrorType ppError in expressResponse.Errors)
                    {
                        EventLog.LogEvent("Paypal error number: " + ppError.ErrorCode, "Paypal Error: '" + ppError.ShortMessage + "' Message: '" + ppError.LongMessage + "' " + " Values passed to SetExpressCheckout: Total=" + string.Format("{0:c}", o.TotalOrderBeforeDiscounts) + " Cart Return Url: " + cartReturnUrl + " Cart Cancel Url: " + cartCancelUrl, EventLogSeverity.Error);
                    }
                }
            }
            finally
            {
                ppAPI = null;
            }

            return Redirect("~/paypalexpresscheckout");
        }
        
        private void SaveShippingSelections(CheckoutViewModel model)
        {
            //Shipping
            string shippingRateKey = Request.Form["shippingrate"];
            MTApp.OrderServices.OrdersRequestShippingMethodByUniqueKey(shippingRateKey, model.CurrentOrder, MTApp.CurrentStore);


            // Save Values so far in case of later errors
            MTApp.CalculateOrder(model.CurrentOrder);

            // Save all the changes to the order
            MTApp.OrderServices.Orders.Update(model.CurrentOrder);
            SessionManager.SaveOrderCookies(model.CurrentOrder, MTApp.CurrentStore);
        }
        private void SavePaymentInfo(CheckoutViewModel model)
        {
            OrderPaymentManager payManager = new OrderPaymentManager(model.CurrentOrder, MTApp);
            payManager.ClearAllTransactions();

            string token = model.PayPalToken;
            string payerId = model.PayPalPayerId;
            if (!string.IsNullOrEmpty(payerId))
            {
                // This is to fix a bug with paypal returning multiple payerId's
                payerId = payerId.Split(',')[0];
            }

            payManager.PayPalExpressAddInfo(model.CurrentOrder.TotalGrand, token, payerId);
        }
    }
}
