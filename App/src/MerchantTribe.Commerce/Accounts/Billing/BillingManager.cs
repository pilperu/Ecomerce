using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MerchantTribe.Web;
using MerchantTribe.Web.Logging;
using Stripe;

namespace MerchantTribe.Commerce.Accounts.Billing
{
    public class BillingManager
    {
        private MerchantTribeApplication _app;

        public BillingManager(MerchantTribeApplication app)
        {
            _app = app;
        }

        public string GetStripeEmailForStore(long storeId)
        {
            string result = string.Empty;

            List<UserAccount> users = _app.AccountServices.FindAdminUsersByStoreId(storeId);
            if (users != null)
            {
                if (users.Count > 0)
                {
                    UserAccount u = users[0];
                    
                    string temp = u.Email;
                    if (temp.Contains('@'))
                    {
                        string[] parts = temp.Split('@');
                        result = parts[0] + "+store" + storeId + "@" + parts[1];
                    }
                }
            }
            return result;
        }

        public CreateCustomerResponse CreateCustomer(CreateCustomerRequest req)
        {
            var response = new CreateCustomerResponse();

            try
            {
                var myCustomer = new StripeCustomerCreateOptions();

                myCustomer.Email = req.Email;
                myCustomer.Description = req.Name;

                // set these properties if using a card
                myCustomer.CardNumber = req.CreditCard.CardNumber;
                myCustomer.CardExpirationYear = req.CreditCard.ExpirationYear.ToString();
                myCustomer.CardExpirationMonth = req.CreditCard.ExpirationMonth.ToString();
                myCustomer.CardAddressCountry = "US";                 // optional
                //myCustomer.CardAddressLine1 = "24 Beef Flank St";   // optional
                //myCustomer.CardAddressLine2 = "Apt 24";             // optional
                //myCustomer.CardAddressState = "NC";                 // optional
                myCustomer.CardAddressZip = req.PostalCode; //        // optional
                myCustomer.CardName = req.CreditCard.CardHolderName;  // optional
                if (req.CreditCard.SecurityCode.Length > 0)
                {
                    myCustomer.CardCvc = req.CreditCard.SecurityCode;
                }

                myCustomer.PlanId = req.PlanId;

                var customerService = new StripeCustomerService();
                StripeCustomer stripeCustomer = customerService.Create(myCustomer);

                if (stripeCustomer.Id.Length > 0)
                {
                    response.NewCustomerId = stripeCustomer.Id;
                    response.Success = true;                    
                }
                else
                {
                    response.Success = false;
                    response.Message = "Unable to get new customer Id";
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            
            return response;
        }
        
        public UpdateCustomerResponse UpdateSubscription(UpdateCustomerRequest req)
        {
            var response = new UpdateCustomerResponse();

            try
            {
                    // Update Customer
                    var myCustomer = new StripeCustomerUpdateSubscriptionOptions();

                    if (req.CreditCard.CardNumber.Trim().Length > 0)
                    {
                        myCustomer.CardNumber = req.CreditCard.CardNumber;
                        myCustomer.CardExpirationYear = req.CreditCard.ExpirationYear.ToString();
                        myCustomer.CardExpirationMonth = req.CreditCard.ExpirationMonth.ToString();
                        myCustomer.CardAddressCountry = "US";                 // optional
                        //myCustomer.CardAddressLine1 = "24 Beef Flank St";   // optional
                        //myCustomer.CardAddressLine2 = "Apt 24";             // optional
                        //myCustomer.CardAddressState = "NC";                 // optional
                        myCustomer.CardAddressZip = req.PostalCode; //        // optional
                        myCustomer.CardName = req.CreditCard.CardHolderName;  // optional
                        if (req.CreditCard.SecurityCode.Length > 0)
                        {
                            myCustomer.CardCvc = req.CreditCard.SecurityCode;
                        }
                    }

                    myCustomer.PlanId = req.PlanId;

                    var customerService = new StripeCustomerService();
                    StripeSubscription result = customerService.UpdateSubscription(req.CustomerId, myCustomer);
                    
                    response.Success = true;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "Unable to update subscription: " + ex.Message;
            }

            return response;
        }

        public UpdateCustomerResponse UpdateCustomer(UpdateCustomerRequest req)
        {
            var response = new UpdateCustomerResponse();

            try
            {
                if (req.PlanId.Length > 0)
                {
                    return UpdateSubscription(req);
                }
                else
                {
                    // Update Customer
                    var myCustomer = new StripeCustomerUpdateOptions();
                    
                    if (req.CreditCard.CardNumber.Trim().Length > 0)
                    {
                        myCustomer.CardNumber = req.CreditCard.CardNumber;
                        myCustomer.CardExpirationYear = req.CreditCard.ExpirationYear.ToString();
                        myCustomer.CardExpirationMonth = req.CreditCard.ExpirationMonth.ToString();
                        myCustomer.CardAddressCountry = "US";                 // optional
                        //myCustomer.CardAddressLine1 = "24 Beef Flank St";   // optional
                        //myCustomer.CardAddressLine2 = "Apt 24";             // optional
                        //myCustomer.CardAddressState = "NC";                 // optional
                        myCustomer.CardAddressZip = req.PostalCode; //        // optional
                        myCustomer.CardName = req.CreditCard.CardHolderName;  // optional
                        if (req.CreditCard.SecurityCode.Length > 0)
                        {
                            myCustomer.CardCvc = req.CreditCard.SecurityCode;
                        }
                    }
                                                       
                    var customerService = new StripeCustomerService();
                    StripeCustomer stripeCustomer = customerService.Update(req.CustomerId, myCustomer);

                    response.Success = true;
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public bool CancelSubscription(string customerId)
        {
            try
            {
                var customerService = new StripeCustomerService();
                var response = customerService.CancelSubscription(customerId);
                return true;
            }
            catch (StripeException stripeEx)
            {
                EventLog.LogEvent(stripeEx);
                return false;
            }
            catch (Exception ex)
            {
                EventLog.LogEvent(ex);
                return false;
            }
        }

        public GetCustomerResponse GetCustomerInformation(string customerId)
        {
            var response = new GetCustomerResponse();

            var customerService = new StripeCustomerService();
            try
            {
                StripeCustomer stripeCustomer = customerService.Get(customerId);
                MapStripeCustomerTo(stripeCustomer, response);
                response.Success = true;
            }
            catch (StripeException ex)
            {
                response.Success = false;
                response.Message = ex.Message;
                return response;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }
        
        private string CleanUpStripeEmail(string stripeEmail)
        {
            string response = stripeEmail;

            if (stripeEmail.Contains('@') && stripeEmail.Contains('+'))
            {
                string[] mainParts = stripeEmail.Split('@');

                string temp = string.Empty;

                string[] subParts = mainParts[0].Split('+');
                for (int i = 0; i < subParts.Length - 1; i++)
                {
                    temp += subParts[i] + "+";
                }
                temp = temp.TrimEnd('+');
                temp += "@" + mainParts[1];
                response = temp;
            }
            return response;
        }
        private void MapStripeCustomerTo(StripeCustomer stripeCustomer, GetCustomerResponse response)
        {
            if (response == null) return;

            response.Email = CleanUpStripeEmail(stripeCustomer.Email);
            response.Id = stripeCustomer.Id;
            response.Description = stripeCustomer.Description;

            if (stripeCustomer.StripeSubscription != null)
            {
                response.PlanId = stripeCustomer.StripeSubscription.StripePlan.Id;
                response.PlanName = stripeCustomer.StripeSubscription.StripePlan.Name;
                decimal planAmount = (decimal)stripeCustomer.StripeSubscription.StripePlan.AmountInCents;
                planAmount = planAmount / 100;
                response.PlanAmount = planAmount;
                response.PlanStatus = stripeCustomer.StripeSubscription.Status;                                
            }
            if (stripeCustomer.StripeNextRecurringCharge != null)
            {
                decimal nextCharge = (decimal)stripeCustomer.StripeNextRecurringCharge.AmountInCents;
                nextCharge = nextCharge / 100;
                string nextStatus = "Next charge " + nextCharge.ToString("C") + " on " + stripeCustomer.StripeNextRecurringCharge.Date;
                response.NextCharge = nextStatus;
            }
            if (stripeCustomer.StripeCard == null)
            {
                response.HasCard = false;
            }
            else
            {
                response.HasCard = true;
            }
            
        }
    }

}
