using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stripe;

namespace MerchantTribe.Payment.Methods
{
    public class StripeProcessor: Method
    {

        private const string LiveUrl = "https://secure.authorize.net/gateway/transact.dll";
        private const string DeveloperUrl = "https://test.authorize.net/gateway/transact.dll";

        public override string Name
        {
            get { return "Stripe"; }
        }

        public override string Id
        {
            get { return "15011DF5-13DA-42BE-9DFF-31C71ED64D4A"; }
        }

        public StripeSettings Settings { get; set; }

        public StripeProcessor()
        {
            Settings = new StripeSettings();
        }

        private void CreateCharge(Transaction t)
        {
            Stripe.Configuration.SetApiKey(Settings.StripeApiKey);

            var myCharge = new StripeChargeCreateOptions();
            
            // always set these properties
            myCharge.AmountInCents = (int)(t.Amount * 100);
            myCharge.Currency = Settings.CurrencyCode;
            
            // set this if you want to
            myCharge.Description = t.MerchantDescription;
            
            // set these properties if using a card
            myCharge.CardNumber = t.Card.CardNumber;
            myCharge.CardExpirationYear = t.Card.ExpirationYear.ToString();
            myCharge.CardExpirationMonth = t.Card.ExpirationMonthPadded;
            //myCharge.CardAddressCountry = "US";             // optional
            if (t.Customer.Street.Length > 0) myCharge.CardAddressLine1 = t.Customer.Street; // optional
            //myCharge.CardAddressLine2 = "Apt 24";           // optional
            //myCharge.CardAddressState = "NC";               // optional
            if (t.Customer.PostalCode.Length > 0) myCharge.CardAddressZip = t.Customer.PostalCode; // optional
            myCharge.CardName = t.Card.CardHolderName;      // optional
            if (t.Card.SecurityCode.Length > 0)
            {
                myCharge.CardCvc = t.Card.SecurityCode;     // optional
            }
            
            // set this property if using a customer
            //myCharge.CustomerId = *customerId*;
            
            // set this property if using a token
            //myCharge.TokenId = *tokenId*;
            
            var chargeService = new StripeChargeService();
            
            StripeCharge stripeCharge = chargeService.Create(myCharge);

            if (stripeCharge.Id.Length > 0 &&
                stripeCharge.AmountInCents.HasValue && stripeCharge.AmountInCents > 0)
            {
                t.Result.Succeeded = true;
                t.Result.ReferenceNumber = stripeCharge.Id;
            }
            else
            {
                    
            }
            {
                t.Result.Succeeded = false;
                t.Result.ResponseCode = "FAIL";
                t.Result.ResponseCodeDescription = "Stripe Failure";
            }
            
        }

        private void CreateRefund(Transaction t)
        {
            Stripe.Configuration.SetApiKey(Settings.StripeApiKey);

            var chargeService = new StripeChargeService();
            StripeCharge stripeCharge = chargeService.Refund(t.PreviousTransactionNumber, 
                (int)(t.Amount * 100));
            if (stripeCharge.Id.Length > 0)
            {
                t.Result.Succeeded = true;
                t.Result.ReferenceNumber = stripeCharge.Id;
            }
            else
            {
                t.Result.Succeeded = false;
                t.Result.ResponseCode = "FAIL";
                t.Result.ResponseCodeDescription = "Stripe Failure";
            }
        }
        public override void ProcessTransaction(Transaction t)
        {
            try
            {
                switch (t.Action)
                {
                    case ActionType.CreditCardCapture:
                        t.Result.Succeeded = false;
                        t.Result.Errors.Add(new Message() { Description = "Stripe Processor does not support capture, only chareges.", Code = "UNSUPPORTED", Severity = MessageType.Warning });
                        break;
                    case ActionType.CreditCardCharge:
                        CreateCharge(t);
                        break;
                    case ActionType.CreditCardHold:
                        t.Result.Succeeded = false;
                        t.Result.Errors.Add(new Message() { Description = "Stripe Processor does not support holds, only chareges.", Code = "UNSUPPORTED", Severity = MessageType.Warning });
                        break;
                    case ActionType.CreditCardRefund:
                        CreateRefund(t);
                        break;
                    case ActionType.CreditCardVoid:
                        t.Result.Succeeded = false;
                        t.Result.Errors.Add(new Message() { Description = "Stripe Processor does not voids.", Code = "UNSUPPORTED", Severity = MessageType.Warning });
                        break;
                }

            }
            catch (StripeException stripeEx)
            {                
                t.Result.Messages.Add(new Message("Stripe Error: " + stripeEx.StripeError.Message, "STRIPE" + stripeEx.StripeError.Code, MessageType.Error));
                t.Result.Succeeded = false;
            }
            catch (Exception ex)
            {                
                t.Result.Messages.Add(new Message("Unknown Payment Error: " + ex.Message, "BVP_AN_1001", MessageType.Error));
                t.Result.Messages.Add(new Message("Stack Trace " + ex.StackTrace, "STACKTRACE", MessageType.Error));
                t.Result.Succeeded = false;
            }                     
        }
                
        public override MethodSettings BaseSettings
        {
            get { return Settings; }
        }

    }
}
