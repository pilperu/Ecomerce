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
using MerchantTribeStore.Controllers;
using MerchantTribe.Commerce.Content;
using MerchantTribe.Commerce.Contacts;
using MerchantTribe.Commerce.Payment;
using MerchantTribe.Commerce.BusinessRules;
using MerchantTribe.Web.Logging;
using MerchantTribe.Web.Validation;
using MerchantTribe.Payment;

namespace MerchantTribeStore.Controllers
{
    public class CheckoutController : BaseStoreController
    {
        private CheckoutViewModel IndexSetup()
        {
            ViewBag.Title = "Checkout";
            ViewBag.BodyClass = "store-checkout-page";

            CheckoutViewModel model = new CheckoutViewModel();
            LoadOrder(model);
            CheckForPoints(model);

            // Buttons
            ThemeManager themes = MTApp.ThemeManager();
            model.ButtonCheckoutUrl = themes.ButtonUrl("PlaceOrder", Request.IsSecureConnection);
            model.ButtonLoginUrl = MTApp.ThemeManager().ButtonUrl("Login", Request.IsSecureConnection);

            // Labels
            model.LabelRewardPoints = MTApp.CurrentStore.Settings.RewardsPointsName;

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
            model.PaymentViewModel.AcceptedCardTypes = MTApp.CurrentStore.Settings.PaymentAcceptedCards;
            
            ViewData["PassedAnalyticsTop"] += "<script type=\"text/javascript\" src=\"" + Url.Content("~/js/checkout.js") + "\" ></script>";

            // Render Side Column
            var columnRender = new code.TemplateEngine.TagHandlers.ContentColumn();
            model.SideColumn = columnRender.RenderColumnToString("601", MTApp, ViewBag);

            return model;
        }
        private void LoadOrder(CheckoutViewModel model)
        {
            Order result = SessionManager.CurrentShoppingCart(MTApp.OrderServices, MTApp.CurrentStore);
            if (result == null) Response.Redirect("~/cart");           
            model.CurrentOrder = result;

            if (result.Items.Count == 0)
            {
                Response.Redirect("~/cart");
            }

            // Email
            model.IsLoggedIn = false;
            if (SessionManager.IsUserAuthenticated(this.MTApp))
            {
                model.IsLoggedIn = true;
                model.CurrentCustomer = MTApp.CurrentCustomer;
                if (model.CurrentCustomer != null)
                {
                    model.CurrentOrder.UserEmail = model.CurrentCustomer.Email;
                }
                
                // Copy customer addresses to order
                model.CurrentCustomer.ShippingAddress.CopyTo(model.CurrentOrder.ShippingAddress);                
                if (model.BillShipSame == false)
                {
                    Address billAddr = model.CurrentCustomer.BillingAddress;
                    billAddr.CopyTo(model.CurrentOrder.BillingAddress);
                }                                
            }  
            
            // Payment
            DisplayPaymentMethods(model);
        }
        void CheckForPoints(CheckoutViewModel model)
        {
            model.ShowRewards = false;

            if (model.CurrentCustomer == null || model.CurrentCustomer.Bvin == string.Empty) return;

            string uid = model.CurrentCustomer.Bvin;
            int points = MTApp.CustomerPointsManager.FindAvailablePoints(uid);
            if (points > 0 && MTApp.CurrentStore.Settings.RewardsPointsOnPurchasesActive)
            {
                model.ShowRewards = true;                
                int potentialPointsToUse = MTApp.CustomerPointsManager.PointsNeededForPurchaseAmount(model.CurrentOrder.TotalOrderAfterDiscounts);
                int amountToUse = 0;
                if (points > potentialPointsToUse)
                {
                    amountToUse = potentialPointsToUse;
                }
                else
                {
                    amountToUse = points;
                }               
                model.RewardPointsAvailable = "You have " + points.ToString() + " " + model.LabelRewardPoints + " available.";
                decimal dollarValue = MTApp.CustomerPointsManager.DollarCreditForPoints(amountToUse);
                model.LabelRewardsUse = "Use " + amountToUse.ToString() + " [" + dollarValue.ToString("C") + "] " + model.LabelRewardPoints;                
            }
        }
        void DisplayPaymentMethods(CheckoutViewModel model)
        {
            MerchantTribe.Commerce.Payment.AvailablePayments availablePayments = new MerchantTribe.Commerce.Payment.AvailablePayments();
            Collection<DisplayPaymentMethod> enabledMethods;
            enabledMethods = availablePayments.EnabledMethods(MTApp.CurrentStore);

            model.PaymentViewModel.CheckDescription = WebAppSettings.PaymentCheckDescription;
            model.PaymentViewModel.CodDescription = WebAppSettings.PaymentCODDescription;
            model.PaymentViewModel.CompanyAccountDescription = WebAppSettings.PaymentCompanyAccountName;            
            model.PaymentViewModel.PurchaseOrderDescription = WebAppSettings.PaymentPurchaseOrderName;
            model.PaymentViewModel.TelephoneDescription = WebAppSettings.PaymentTelephoneDescription;
            
            if ((model.CurrentOrder.TotalOrderAfterDiscounts > 0) || (!MTApp.CurrentStore.Settings.AllowZeroDollarOrders))
            {
                model.PaymentViewModel.NoPaymentNeeded = false;
                foreach (DisplayPaymentMethod m in enabledMethods)
                {
                    switch (m.MethodId)
                    {
                        case WebAppSettings.PaymentIdCheck:
                            model.PaymentViewModel.IsCheckActive = true;
                            break;
                        case WebAppSettings.PaymentIdCreditCard:
                            model.PaymentViewModel.IsCreditCardActive = true;
                            break;
                        case WebAppSettings.PaymentIdTelephone:
                            model.PaymentViewModel.IsTelephoneActive = true;
                            break;
                        case WebAppSettings.PaymentIdPurchaseOrder:
                            model.PaymentViewModel.IsPurchaseOrderActive = true;
                            break;
                        case WebAppSettings.PaymentIdCompanyAccount:
                            model.PaymentViewModel.IsCompanyAccountActive = true;
                            break;
                        case WebAppSettings.PaymentIdCashOnDelivery:
                            model.PaymentViewModel.IsCodActive = true;
                            break;
                        case WebAppSettings.PaymentIdPaypalExpress:
                            model.PaymentViewModel.IsPayPalActive = true;
                            break;
                        default:
                            // do nothing
                            break;
                    }
                }

                if (enabledMethods.Count == 1)
                {
                    switch (enabledMethods[0].MethodId)
                    {
                        case WebAppSettings.PaymentIdCheck:
                            model.PaymentViewModel.SelectedPayment = "check";
                            break;
                        case WebAppSettings.PaymentIdCreditCard:
                            model.PaymentViewModel.SelectedPayment = "creditcard";
                            break;
                        case WebAppSettings.PaymentIdTelephone:
                            model.PaymentViewModel.SelectedPayment = "telephone";
                            break;
                        case WebAppSettings.PaymentIdPurchaseOrder:
                            model.PaymentViewModel.SelectedPayment = "purchaseorder";
                            break;
                        case WebAppSettings.PaymentIdCompanyAccount:
                            model.PaymentViewModel.SelectedPayment = "companyaccount";
                            break;
                        case WebAppSettings.PaymentIdCashOnDelivery:
                            model.PaymentViewModel.SelectedPayment = "cod";
                            break;
                        case WebAppSettings.PaymentIdPaypalExpress:
                            model.PaymentViewModel.SelectedPayment = "paypal";
                            break;
                    }
                }
                else
                {
                    if (model.PaymentViewModel.IsCreditCardActive)
                    {
                        model.PaymentViewModel.SelectedPayment = "creditcard";
                    }
                }
            }
            else
            {
                model.PaymentViewModel.NoPaymentNeeded = true;
                model.PaymentViewModel.NoPaymentNeededDescription = WebAppSettings.PaymentNoPaymentNeededDescription;                                
            }
        }

        // GET: /checkout
        [NonCacheableResponseFilter]
        public ActionResult Index()
        {
            CheckoutViewModel model = IndexSetup();            
            
            return View(model);
        }

        // POST: /checkout
        [NonCacheableResponseFilter]
        [ActionName("Index")]
        [HttpPost]
        public ActionResult IndexPost()
        {
            CheckoutViewModel model = IndexSetup();
            TagOrderWithAffiliate(model);                        
            LoadValuesFromForm(model);            
            if (ValidateOrder(model))
            {
                ProcessOrder(model);
            }

            // Render Error Summary
            foreach (var v in model.Violations)
            {
                FlashFailure(v.ErrorMessage);
            }

            return View(model);
        }
        private void TagOrderWithAffiliate(CheckoutViewModel model)
        {
            string affid = MTApp.ContactServices.GetValidAffiliateId(MTApp).ToString();
            if (!string.IsNullOrEmpty(affid))
            {
                model.CurrentOrder.AffiliateID = affid;
            }
        }        
        private void LoadValuesFromForm(CheckoutViewModel model)
        {            
            // Email
            model.CurrentOrder.UserEmail = Request.Form["customeremail"];
                 
            // Addresses            
            model.BillShipSame = (Request.Form["chkbillsame"] != null);            
           
            LoadAddressFromForm("shipping", model.CurrentOrder.ShippingAddress);
            if (model.BillShipSame)
            {
                model.CurrentOrder.ShippingAddress.CopyTo(model.CurrentOrder.BillingAddress);
            }
            else
            {
                LoadAddressFromForm("billing", model.CurrentOrder.BillingAddress);
            }
            // Save addresses to customer account
            if (model.IsLoggedIn)
            {
                
                model.CurrentOrder.ShippingAddress.CopyTo(model.CurrentOrder.ShippingAddress);
                if (model.BillShipSame == false)
                {
                    model.CurrentOrder.BillingAddress.CopyTo(model.CurrentCustomer.BillingAddress);
                }
                MTApp.MembershipServices.Customers.Update(model.CurrentCustomer);
            }

            //Shipping
            string shippingRateKey = Request.Form["shippingrate"];
            MTApp.OrderServices.OrdersRequestShippingMethodByUniqueKey(shippingRateKey, model.CurrentOrder, MTApp.CurrentStore);

            // Save Values so far in case of later errors
            MTApp.CalculateOrder(model.CurrentOrder);

            // Save Payment Information                    
            model.UseRewardsPoints = Request.Form["userewardspoints"] == "1";
            ApplyRewardsPoints(model);
            
            // Payment Methods
            LoadPaymentFromForm(model);
            SavePaymentSelections(model);

            // Instructions
            model.CurrentOrder.Instructions = Request.Form["specialinstructions"];

            // Agree to Terms
            var agreedValue = Request.Form["agreed"];
            if (!String.IsNullOrEmpty(agreedValue))
            {
                model.AgreedToTerms = true;
            }


            // Save all the changes to the order
            MTApp.OrderServices.Orders.Update(model.CurrentOrder);
            SessionManager.SaveOrderCookies(model.CurrentOrder, MTApp.CurrentStore);
        }
        private void LoadPaymentFromForm(CheckoutViewModel model)
        {
            model.PaymentViewModel.SelectedPayment = Request.Form["paymethod"] ?? string.Empty;
            model.PaymentViewModel.DataPurchaseOrderNumber = Request.Form["purchaseorder"] ?? string.Empty;
            model.PaymentViewModel.DataCompanyAccountNumber = Request.Form["companyaccount"] ?? string.Empty;
            model.PaymentViewModel.DataCreditCard.CardHolderName = Request.Form["cccardholder"] ?? string.Empty;
            model.PaymentViewModel.DataCreditCard.CardNumber = MerchantTribe.Payment.CardValidator.CleanCardNumber(Request.Form["cccardnumber"] ?? string.Empty);
            int expMonth = -1;
            int.TryParse(Request.Form["ccexpmonth"] ?? string.Empty, out expMonth);
            model.PaymentViewModel.DataCreditCard.ExpirationMonth = expMonth;
            int expYear = -1;
            int.TryParse(Request.Form["ccexpyear"] ?? string.Empty, out expYear);
            model.PaymentViewModel.DataCreditCard.ExpirationYear = expYear;
            model.PaymentViewModel.DataCreditCard.SecurityCode = Request.Form["ccsecuritycode"] ?? string.Empty;
        }
        private void SavePaymentSelections(CheckoutViewModel model)
        {
            OrderPaymentManager payManager = new OrderPaymentManager(model.CurrentOrder, MTApp);
            payManager.ClearAllNonStoreCreditTransactions();

            bool found = false;

            if (model.PaymentViewModel.SelectedPayment == "creditcard")
            {
                found = true;
                payManager.CreditCardAddInfo(model.PaymentViewModel.DataCreditCard, model.CurrentOrder.TotalGrandAfterStoreCredits(MTApp.OrderServices));
            }

            if ((found == false) && (model.PaymentViewModel.SelectedPayment == "check"))
            {
                found = true;
                payManager.OfflinePaymentAddInfo(model.CurrentOrder.TotalGrandAfterStoreCredits(MTApp.OrderServices), "Customer will pay by check.");
            }

            if ((found == false) && (model.PaymentViewModel.SelectedPayment == "telephone"))
            {
                found = true;
                payManager.OfflinePaymentAddInfo(model.CurrentOrder.TotalGrandAfterStoreCredits(MTApp.OrderServices), "Customer will call with payment info.");
            }

            if ((found == false) && (model.PaymentViewModel.SelectedPayment == "purchaseorder"))
            {
                found = true;
                payManager.PurchaseOrderAddInfo(model.PaymentViewModel.DataPurchaseOrderNumber.Trim(), model.CurrentOrder.TotalGrandAfterStoreCredits(MTApp.OrderServices));
            }
            if ((found == false) && (model.PaymentViewModel.SelectedPayment == "companyaccount"))
            {
                found = true;
                payManager.CompanyAccountAddInfo(model.PaymentViewModel.DataCompanyAccountNumber.Trim(), model.CurrentOrder.TotalGrandAfterStoreCredits(MTApp.OrderServices));
            }

            if ((found == false) && (model.PaymentViewModel.SelectedPayment == "cod"))
            {
                found = true;
                payManager.OfflinePaymentAddInfo(model.CurrentOrder.TotalGrandAfterStoreCredits(MTApp.OrderServices), "Customer will pay cash on delivery.");
            }

            if ((found == false) && (model.PaymentViewModel.SelectedPayment == "paypal"))
            {
                found = true;
                // Need token and id before we can add this to the order
                // Handled on the checkout page.
                //payManager.PayPalExpressAddInfo(o.TotalGrand);
            }
        }
        private void ApplyRewardsPoints(CheckoutViewModel model)
        {
            // Remove any current points info transactions
            foreach (OrderTransaction t in MTApp.OrderServices.Transactions.FindForOrder(model.CurrentOrder.bvin))
            {
                if (t.Action == MerchantTribe.Payment.ActionType.RewardPointsInfo)
                {
                    MTApp.OrderServices.Transactions.Delete(t.Id);
                }
            }

            // Don't add if we're not using points
            if (!model.UseRewardsPoints) return;

            // Apply Info to Order
            OrderPaymentManager payManager = new OrderPaymentManager(model.CurrentOrder, MTApp);
            payManager.RewardsPointsAddInfo(RewardsPotentialCredit(model));
        }
        private decimal RewardsPotentialCredit(CheckoutViewModel model)
        {
            decimal result = 0;
            if (!model.UseRewardsPoints) return result;

            int points = MTApp.CustomerPointsManager.FindAvailablePoints(model.CurrentCustomer.Bvin);
            int potentialPointsToUse = MTApp.CustomerPointsManager.PointsNeededForPurchaseAmount(model.CurrentOrder.TotalOrderAfterDiscounts);
            int amountToUse = 0;
            if (points > potentialPointsToUse)
            {
                amountToUse = potentialPointsToUse;
            }
            else
            {
                amountToUse = points;
            }
            result = MTApp.CustomerPointsManager.DollarCreditForPoints(amountToUse);
            return result;
        }
        private void LoadAddressFromForm(string prefix, Address address)
        {
            address.Bvin = Request.Form[prefix + "addressbvin"] ?? address.Bvin;
            address.CountryBvin = Request.Form[prefix + "country"] ?? address.CountryBvin;
            address.FirstName = Request.Form[prefix + "firstname"] ?? address.FirstName;
            address.LastName = Request.Form[prefix + "lastname"] ?? address.LastName;
            address.Company = Request.Form[prefix + "company"] ?? address.Company;
            address.Line1 = Request.Form[prefix + "address"] ?? address.Line1;
            address.City = Request.Form[prefix + "city"] ?? address.City;
            address.RegionBvin = Request.Form[prefix + "state"] ?? address.RegionBvin;
            address.PostalCode = Request.Form[prefix + "zip"] ?? address.PostalCode;
            address.Phone = Request.Form[prefix + "phone"] ?? address.Phone;                
        }
        private bool ValidateOrder(CheckoutViewModel model)
        {
            bool result = true;

            if (model.AgreedToTerms == false && model.ShowAgreeToTerms == true)
            {
                model.Violations.Add(new MerchantTribe.Web.Validation.RuleViolation("Terms", "Terms", SiteTerms.GetTerm(SiteTermIds.SiteTermsAgreementError)));
                result = false;
            }

            // Validate Email
            MerchantTribe.Web.Validation.ValidationHelper.ValidEmail("Email Address", model.CurrentOrder.UserEmail, model.Violations, "customeremail");

            // Validate Shipping Address
            model.Violations.AddRange(ValidateAddress(model.CurrentOrder.ShippingAddress, "Shipping"));

            // Validate Billing Address
            if (model.BillShipSame == false)
            {
                model.Violations.AddRange(ValidateAddress(model.CurrentOrder.BillingAddress, "Billing"));
            }

            Order Basket = SessionManager.CurrentShoppingCart(MTApp.OrderServices, MTApp.CurrentStore);
            //Collection<GiftCertificate> gcs = Basket.GetGiftCertificates();
            //decimal totalValue = 0m;
            //foreach (GiftCertificate item in gcs) {
            //    totalValue += item.CurrentAmount;
            //}

            // If Gift Certs are Included in the order, we may not need
            // additional payment information
            bool paymentFound = false;
            //Basket.CalculateGrandTotalOnly(false, false);
            //if ((totalValue >= Basket.TotalGrand)) {
            //	paymentFound = true;
            //}

            // Make sure a shipping method is selected
            // Basket validation checks for shipping method unique key
            if (!Basket.IsValid())
            {
                model.Violations.AddRange(Basket.GetRuleViolations());
            }

            // Payment Validation
            model.Violations.AddRange(ValidatePayment(model));

            if ((model.Violations.Count > 0))
            {
                result = false;
            }
            return result;
        }
        private List<MerchantTribe.Web.Validation.RuleViolation> ValidateAddress(Address a, string prefix)
        {
            List<MerchantTribe.Web.Validation.RuleViolation> result = new List<MerchantTribe.Web.Validation.RuleViolation>();

            string pre = prefix.Trim().ToLowerInvariant();

            MerchantTribe.Web.Validation.ValidationHelper.Required(prefix + " Country Name", a.CountryData.Name, result, pre + "countryname");
            MerchantTribe.Web.Validation.ValidationHelper.Required(prefix + " First Name", a.FirstName, result, pre + "firstname");
            MerchantTribe.Web.Validation.ValidationHelper.Required(prefix + " Last Name", a.LastName, result, pre + "lastname");
            MerchantTribe.Web.Validation.ValidationHelper.Required(prefix + " Street", a.Line1, result, pre + "address");
            MerchantTribe.Web.Validation.ValidationHelper.Required(prefix + " City", a.City, result, pre + "city");
            MerchantTribe.Web.Validation.ValidationHelper.Required(prefix + " Postal Code", a.PostalCode, result, pre + "zip");

            MerchantTribe.Web.Validation.ValidationHelper.Required(prefix + " Region/State",
                                                                a.RegionData.Abbreviation,
                                                                result, pre + "state");
            return result;
        }
        private List<MerchantTribe.Web.Validation.RuleViolation> ValidatePayment(CheckoutViewModel model)
        {
            List<MerchantTribe.Web.Validation.RuleViolation> violations = new List<MerchantTribe.Web.Validation.RuleViolation>();

            // Nothing to validate if no payment is needed
            if (model.PaymentViewModel.NoPaymentNeeded)
            {
                return violations;
            }

            if (model.PaymentViewModel.SelectedPayment == "creditcard")
            {
                return ValidateCreditCard(model);
            }
            if (model.PaymentViewModel.SelectedPayment == "check")
            {
                return violations;
            }
            if (model.PaymentViewModel.SelectedPayment == "telephone")
            {
                return violations;
            }
            if (model.PaymentViewModel.SelectedPayment == "purchaseorder")
            {
                ValidationHelper.Required("Purchase Order Number", model.PaymentViewModel.DataPurchaseOrderNumber.Trim(), violations, "purchaseorder");
                return violations;
            }
            if (model.PaymentViewModel.SelectedPayment == "companyaccount")
            {
                ValidationHelper.Required("Company Account Number", model.PaymentViewModel.DataCompanyAccountNumber.Trim(), violations, "companyaccount");
                return violations;
            }
            if (model.PaymentViewModel.SelectedPayment == "cod")
            {
                return violations;
            }
            if (model.PaymentViewModel.SelectedPayment == "paypal")
            {
                return violations;
            }

            // We haven't return anything so nothing is selected.
            // Try CC as default payment method        
            if (model.PaymentViewModel.DataCreditCard.CardNumber.Length > 12)
            {
                model.PaymentViewModel.SelectedPayment = "creditcard";
                return ValidateCreditCard(model);
            }

            // nothing selected, trial of cc failed
            violations.Add(new RuleViolation("Payment Method", "", "Please select a payment method", ""));

            return violations;
        }
        private List<RuleViolation> ValidateCreditCard(CheckoutViewModel model)
        {
            List<RuleViolation> violations = new List<RuleViolation>();
            
            if ((!MerchantTribe.Payment.CardValidator.IsCardNumberValid(model.PaymentViewModel.DataCreditCard.CardNumber)))
            {
                violations.Add(new RuleViolation("Credit Card Number", "", "Please enter a valid credit card number", "cccardnumber"));
            }
            MerchantTribe.Payment.CardType cardTypeCheck = MerchantTribe.Payment.CardValidator.GetCardTypeFromNumber(model.PaymentViewModel.DataCreditCard.CardNumber);
            List<CardType> acceptedCards = MTApp.CurrentStore.Settings.PaymentAcceptedCards;
            if (!acceptedCards.Contains(cardTypeCheck))
            {
                violations.Add(new RuleViolation("Card Type Not Accepted", "", "That card type is not accepted by this store. Please use a different card.", "cccardnumber"));
            }

            ValidationHelper.RequiredMinimum(1, "Card Expiration Year", model.PaymentViewModel.DataCreditCard.ExpirationYear, violations, "ccexpyear");
            ValidationHelper.RequiredMinimum(1, "Card Expiration Month", model.PaymentViewModel.DataCreditCard.ExpirationMonth, violations, "ccexpmonth");
            ValidationHelper.Required("Name on Card", model.PaymentViewModel.DataCreditCard.CardHolderName, violations, "cccardholder");

            if (MTApp.CurrentStore.Settings.PaymentCreditCardRequireCVV == true)
            {
                ValidationHelper.RequiredMinimum(3, "Card Security Code", model.PaymentViewModel.DataCreditCard.SecurityCode.Length, violations, "ccsecuritycode");
            }

            return violations;
        }

        private void ProcessOrder(CheckoutViewModel model)
        {
            // Save as Order
            OrderTaskContext c = new OrderTaskContext(MTApp);
            c.UserId = MTApp.CurrentCustomerId;
            c.Order = model.CurrentOrder;

            // Check for PayPal Request            
            bool paypalCheckoutSelected = model.PaymentViewModel.SelectedPayment == "paypal";                        
            if (paypalCheckoutSelected)
            {
                c.Inputs.Add("bvsoftware", "Mode", "PaypalExpress");
                c.Inputs.Add("bvsoftware", "AddressSupplied", "1");
                if (!Workflow.RunByName(c, WorkflowNames.ThirdPartyCheckoutSelected))
                {
                    EventLog.LogEvent("Paypal Express Checkout Failed", "Specific Errors to follow", EventLogSeverity.Error);
                    // Show Errors     
                    List<MerchantTribe.Web.Validation.RuleViolation> violations = new List<MerchantTribe.Web.Validation.RuleViolation>();
                    foreach (WorkflowMessage item in c.GetCustomerVisibleErrors())
                    {
                        violations.Add(new MerchantTribe.Web.Validation.RuleViolation("Workflow", item.Name, item.Description));
                    }
                }
            }
            else
            {
                if (Workflow.RunByName(c, WorkflowNames.ProcessNewOrder) == true)
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
                    else
                    {
                        // Redirect to Payment Error
                        SessionManager.SetCurrentPaymentPendingCartId(MTApp.CurrentStore, model.CurrentOrder.bvin);
                        Response.Redirect("~/checkout/paymenterror");
                    }
                }
                else
                {
                    // Show Errors      
                    List<MerchantTribe.Web.Validation.RuleViolation> violations = new List<MerchantTribe.Web.Validation.RuleViolation>();
                    foreach (WorkflowMessage item in c.GetCustomerVisibleErrors())
                    {
                        violations.Add(new MerchantTribe.Web.Validation.RuleViolation("Workflow", item.Name, item.Description));
                    }
                    if (violations.Count < 1)
                    {
                        violations.Add(new MerchantTribe.Web.Validation.RuleViolation("Workflow", "Internal Error",
                            "An internal error occured while attempting to place your order. Please contact the store owner directly to complete your order."));
                    }
                }
            }
        }

        //GET: /checkout/receipt
        [NonCacheableResponseFilter]
        public ActionResult Receipt()
        {            
            ViewBag.Title = "Order Receipt";
            ViewBag.BodyClass = "store-receipt-page";
            ViewBag.MetaDescription = "Order Receipt";            
            ViewData["PassedAnalyticsTop"] += "<script type=\"text/javascript\" src=\"" + Url.Content("~/scripts/receipt.js") + "\" ></script>";
            LoadOrder();            
            return View();           
        }
        private void LoadOrder()
        {
            if (Request.Params["id"] != null)
            {
                Order o = MTApp.OrderServices.Orders.FindForCurrentStore(Request.Params["id"]);
                if (o == null)
                {
                    FlashFailure("Order could not be found. Please contact store for assistance.");
                    return;
                }

                if (o.CustomProperties.Where(y => (y.DeveloperId == "bvsoftware")
                                                    && (y.Key == "allowpasswordreset")
                                                    && (y.Value == "1")
                                                    ).Count() > 0)
                {
                    ViewBag.AllowPasswordReset = true;
                    ViewBag.Email = o.UserEmail;
                    ViewBag.OrderBvin = o.bvin;
                    ViewBag.SubmitButtonUrl = MTApp.ThemeManager().ButtonUrl("Submit", Request.IsSecureConnection);
                }
                else
                {
                    ViewBag.AllowPasswordReset = false;
                }


                ViewBag.Order = o;
                ViewBag.AcumaticaOrder = null;

                OrderPaymentSummary paySummary = MTApp.OrderServices.PaymentSummary(o);
                ViewBag.OrderPaymentSummary = paySummary;

                // File Downloads
                List<ProductFile> fileDownloads = new List<ProductFile>();
                if ((o.PaymentStatus == OrderPaymentStatus.Paid) && (o.StatusCode != OrderStatusCode.OnHold))
                {
                    foreach (LineItem item in o.Items)
                    {
                        if (item.ProductId != string.Empty)
                        {
                            List<ProductFile> productFiles = MTApp.CatalogServices.ProductFiles.FindByProductId(item.ProductId);
                            foreach (ProductFile file in productFiles)
                            {
                                fileDownloads.Add(file);
                            }
                        }
                    }
                }
                ViewBag.FileDownloads = fileDownloads;
                ViewBag.FileDownloadsButtonUrl = MTApp.ThemeManager().ButtonUrl("download", Request.IsSecureConnection);

                RenderAnalytics(o);
            }
            else
            {
                FlashFailure("Order Number missing. Please contact an administrator.");
                return;
            }            
        }
        private void RenderAnalytics(Order o)
        {

            // Reset Analytics for receipt page            
            this.ViewData["PassedAnalyticsTop"] = string.Empty;

            // Add Tracker and Maybe Ecommerce Tracker to Top
            if (MTApp.CurrentStore.Settings.Analytics.UseGoogleTracker)
            {
                if (MTApp.CurrentStore.Settings.Analytics.UseGoogleEcommerce)
                {
                    // Ecommerce + Page Tracker
                    this.ViewData["PassedAnalyticsTop"] = MerchantTribe.Commerce.Metrics.GoogleAnalytics.RenderLatestTrackerAndTransaction(
                        MTApp.CurrentStore.Settings.Analytics.GoogleTrackerId,
                        o,
                        MTApp.CurrentStore.Settings.Analytics.GoogleEcommerceStoreName,
                        MTApp.CurrentStore.Settings.Analytics.GoogleEcommerceCategory);
                }
                else
                {
                    // Page Tracker Only
                    this.ViewData["PassedAnalyticsTop"] = MerchantTribe.Commerce.Metrics.GoogleAnalytics.RenderLatestTracker(MTApp.CurrentStore.Settings.Analytics.GoogleTrackerId);
                }
            }
            
            // Adwords Tracker at bottom if needed
            if (MTApp.CurrentStore.Settings.Analytics.UseGoogleAdWords)
            {
                this.ViewData["PassedAnalyticsBottom"] += MerchantTribe.Commerce.Metrics.GoogleAnalytics.RenderGoogleAdwordTracker(
                                                        o.TotalGrand,
                                                        MTApp.CurrentStore.Settings.Analytics.GoogleAdWordsId,
                                                        MTApp.CurrentStore.Settings.Analytics.GoogleAdWordsLabel,
                                                        MTApp.CurrentStore.Settings.Analytics.GoogleAdWordsBgColor,
                                                        Request.IsSecureConnection);
            }

            // Add Yahoo Tracker to Bottom if Needed
            if (MTApp.CurrentStore.Settings.Analytics.UseYahooTracker)
            {
                this.ViewData["PassedAnalyticsBottom"] += MerchantTribe.Commerce.Metrics.YahooAnalytics.RenderYahooTracker(
                    o, MTApp.CurrentStore.Settings.Analytics.YahooAccountId);
            }

            //if (ViewBag.HideAnalytics != null)
            //{
            //    if (!(bool)ViewBag.HideAnalytics)
            //    {
            //        System.Text.StringBuilder sbA = new System.Text.StringBuilder();
            //        sbA.Append("<script type=\"text/javascript\">");
            //        sbA.Append("MerchantTribeAnalytics.EventCode = '2';");
            //        sbA.Append("MerchantTribeAnalytics.EventAmount = '@Grand';");
            //        sbA.Append("MerchantTribeAnalytics.EventDescription = '@o.OrderNumber';");
            //        sbA.Append("</script>");
            //        this.ViewData["PassedAnalyticsBottom"] += sbA.ToString();
            //    }
            //}
        }

        [HttpPost] // POST: /checkout/applyshipping
        public ActionResult ApplyShipping()
        {
            ApplyShippingResponse result = new ApplyShippingResponse();

            string rateKey = Request.Form["MethodId"];
            string orderid = Request.Form["OrderId"];
            if (rateKey == null)
            {
                rateKey = "";
            }
            if (orderid == null)
            {
                orderid = "";
            }


            Order o = MTApp.OrderServices.Orders.FindForCurrentStore(orderid);
            MTApp.OrderServices.OrdersRequestShippingMethodByUniqueKey(rateKey, o, MTApp.CurrentStore);
            MTApp.CalculateOrderAndSave(o);
            SessionManager.SaveOrderCookies(o, MTApp.CurrentStore);

            result.totalsastable = o.TotalsAsTable();
            
            return new PreJsonResult(MerchantTribe.Web.Json.ObjectToJson(result));
        }
        public class ApplyShippingResponse
        {
            public string totalsastable = string.Empty;
        }        
                
        [HttpPost] // POST: /checkout/cleancreditcard
        public ActionResult CleanCreditCard()
        {
            CleanCreditCardResponse result = new CleanCreditCardResponse();
            string notclean = Request.Form["CardNumber"];
            result.CardNumber = MerchantTribe.Payment.CardValidator.CleanCardNumber(notclean);
            return new PreJsonResult(MerchantTribe.Web.Json.ObjectToJson(result));
        }
        public class CleanCreditCardResponse
        {
            public string CardNumber = "";
        }

        [HttpPost] // POST: /checkout/IsEmailKnown
        public ActionResult IsEmailKnown()
        {
            IsEmailKnownResponse result = new IsEmailKnownResponse();
            string email = Request.Form["email"];
            CustomerAccount act = MTApp.MembershipServices.Customers.FindByEmail(email);
            if (act != null)
            {
                if (act.Bvin != string.Empty)
                {
                    result.success = "1";
                }
            }
            return new PreJsonResult(MerchantTribe.Web.Json.ObjectToJson(result));
        }
        public class IsEmailKnownResponse
        {
            public string success = "0";
        }


        private CheckoutViewModel PaymentErrorSetup()
        {
            ViewBag.Title = "Checkout Payment Error";
            ViewBag.BodyClass = "store-checkout-page";

            CheckoutViewModel model = new CheckoutViewModel();
            LoadPendingOrder(model);

            // Buttons
            ThemeManager themes = MTApp.ThemeManager();
            model.ButtonCheckoutUrl = themes.ButtonUrl("PlaceOrder", Request.IsSecureConnection);
            model.ButtonCancelUrl = themes.ButtonUrl("Cancel", Request.IsSecureConnection);

            // Populate Countries
            model.Countries = MTApp.CurrentStore.Settings.FindActiveCountries();
            model.PaymentViewModel.AcceptedCardTypes = MTApp.CurrentStore.Settings.PaymentAcceptedCards;

            // Render Side Column
            var columnRender = new code.TemplateEngine.TagHandlers.ContentColumn();
            model.SideColumn = columnRender.RenderColumnToString("601", MTApp, ViewBag);

            ViewData["PassedAnalyticsTop"] += "<script type=\"text/javascript\" src=\"" + Url.Content("~/js/checkout.js") + "\" ></script>";
            return model;
        }
        private void LoadPendingOrder(CheckoutViewModel model)
        {
            string bvin = SessionManager.GetCurrentPaymentPendingCartId(MTApp.CurrentStore);
            if (bvin.Trim().Length < 1) Response.Redirect("~/cart");

            Order result = MTApp.OrderServices.Orders.FindForCurrentStore(bvin);
            if (result == null) Response.Redirect("~/cart");
            model.CurrentOrder = result;

            if (result.Items.Count == 0)
            {
                Response.Redirect("~/cart");
            }

            // Email
            model.IsLoggedIn = false;
            if (SessionManager.IsUserAuthenticated(this.MTApp))
            {
                model.IsLoggedIn = true;
                model.CurrentCustomer = MTApp.CurrentCustomer;
                if (model.CurrentCustomer != null)
                {
                    model.CurrentOrder.UserEmail = model.CurrentCustomer.Email;
                }

                // Copy customer addresses to order
                model.CurrentCustomer.ShippingAddress.CopyTo(model.CurrentOrder.ShippingAddress);
                if (model.BillShipSame == false)
                {
                    Address billAddr = model.CurrentCustomer.BillingAddress;
                    billAddr.CopyTo(model.CurrentOrder.BillingAddress);
                }
            }

            // Payment
            DisplayPaymentMethods(model);

        }

        [HttpGet]
        [NonCacheableResponseFilter]
        public ActionResult PaymentError()
        {
            CheckoutViewModel model = PaymentErrorSetup();

            return View(model);
        }

        [HttpPost]
        [NonCacheableResponseFilter]
        [ActionName("PaymentError")]
        public ActionResult PaymentErrorPost()
        {
            CheckoutViewModel model = PaymentErrorSetup();

            // Load Post Data and Update Order
            LoadAddressFromForm("billing", model.CurrentOrder.BillingAddress);            
            LoadPaymentFromForm(model);
            SavePaymentSelections(model);                                   
            MTApp.OrderServices.Orders.Update(model.CurrentOrder);            
            
            // Validate Data
            if (ValidatePaymentErrorOrder(model))
            {
                ProcessPaymentErrorOrder(model);
            }

            // Render Error Summary
            foreach (var v in model.Violations)
            {
                FlashFailure(v.ErrorMessage);
            }

            return View(model);
        }
        private bool ValidatePaymentErrorOrder(CheckoutViewModel model)
        {            
            model.Violations.AddRange(ValidateAddress(model.CurrentOrder.BillingAddress, "Billing"));            
            model.Violations.AddRange(ValidatePayment(model));
            return (model.Violations.Count <= 0);            
        }
        private void ProcessPaymentErrorOrder(CheckoutViewModel model)
        {
            // Save as Order
            OrderTaskContext c = new OrderTaskContext(MTApp);
            c.UserId = MTApp.CurrentCustomerId;
            c.Order = model.CurrentOrder;

            if (Workflow.RunByName(c, WorkflowNames.ProcessNewOrderPayments) == true)
            {
                // Clear Pending Cart ID because payment is good
                SessionManager.SetCurrentPaymentPendingCartId(MTApp.CurrentStore, string.Empty);

                // Process Post Payment Stuff                    
                MerchantTribe.Commerce.BusinessRules.Workflow.RunByName(c, MerchantTribe.Commerce.BusinessRules.WorkflowNames.ProcessNewOrderAfterPayments);
                Order tempOrder = MTApp.OrderServices.Orders.FindForCurrentStore(model.CurrentOrder.bvin);
                MTApp.CurrentRequestContext.IntegrationEvents.OrderReceived(tempOrder, MTApp);
                Response.Redirect("~/checkout/receipt?id=" + model.CurrentOrder.bvin);
            }
        }

        [HttpPost]
        public ActionResult Cancel()
        {
            string bvin = SessionManager.GetCurrentPaymentPendingCartId(MTApp.CurrentStore);
            if (bvin.Trim().Length < 1) Response.Redirect("~/cart");

            Order Basket = MTApp.OrderServices.Orders.FindForCurrentStore(bvin);
            if (Basket != null)
            {
                Basket.StatusCode = OrderStatusCode.Cancelled;
                Basket.StatusName = "Cancelled";

                OrderNote n = new OrderNote();
                n.IsPublic = true;
                n.Note = "Cancelled by Customer";
                Basket.Notes.Add(n);

                MTApp.OrderServices.Orders.Update(Basket);
                SessionManager.SetCurrentPaymentPendingCartId(MTApp.CurrentStore, string.Empty);                
            }            
            return Redirect("~/cart");
        }

    }
}
