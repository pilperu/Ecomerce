using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MerchantTribe.Web;

namespace MerchantTribe.Payment.Methods
{
    public class CybersourceSOAP: Method
    {

        private const string LiveUrl = "https://ics2ws.ic3.com/commerce/1.x/transactionProcessor";
        private const string TestUrl = "https://ics2wstest.ic3.com/commerce/1.x/transactionProcessor";

        public override string Name
        {
            get { return "Cybersource (SOAP)"; }
        }

        public override string Id
        {
            get { return "C36CDBAB-28B0-4F47-B61C-87CED8EE7222"; }
        }

        public CybersourceSOAPSettings Settings { get; set; }

        public CybersourceSOAP()
        {
            Settings = new CybersourceSOAPSettings();
        }

        public override void ProcessTransaction(Transaction t)
        {
            bool result = false;

            try
            {
                string url = LiveUrl;                
                if (Settings.TestMode)
                {
                    url = TestUrl;
                }

                // Create Request
                Cybersource.RequestMessage req = new Cybersource.RequestMessage();
                
                // Set MerchantId and Ref Code
                req.merchantID = Settings.MerchantId;
                if (t.Id.ToString().Length < 0)
                {
                    req.merchantReferenceCode = Text.TrimToLength(System.Guid.NewGuid().ToString(), 50);
                    t.AdditionalSettings.Add("CybersourceRefCode", req.merchantReferenceCode);
                }
                else
                {
                    req.merchantReferenceCode = Text.TrimToLength(t.Id.ToString(), 50);
                }

                // for debugging on Cybersource side
                req.clientLibrary = ".NET WCF";
                req.clientLibraryVersion = Environment.Version.ToString();
                req.clientEnvironment =
                    Environment.OSVersion.Platform +
                    Environment.OSVersion.Version.ToString();                
                req.clientApplication = "MerchantTribe";
                req.clientApplicationVersion = "1.3.0";

                // Currency Code
                req.purchaseTotals = new Cybersource.PurchaseTotals();               
                req.purchaseTotals.currency = Settings.CurrencyCode;                
                if (req.purchaseTotals.currency.Trim().Length < 3)
                {
                    req.purchaseTotals.currency = "USD";
                }

                bool includeBillTo = false;
                bool includeCardData = false;
                bool includeMerchantExtraData = false;

                // Set Action
                switch (t.Action)
                {
                    case ActionType.CreditCardCharge:
                        req.ccAuthService = new Cybersource.CCAuthService();
                        req.ccAuthService.run = "true";
                        req.ccCaptureService = new Cybersource.CCCaptureService();
                        req.ccCaptureService.run = "true";
                        includeBillTo = true;
                        includeCardData = true;
                        includeMerchantExtraData = true;
                        req.purchaseTotals.grandTotalAmount = t.Amount.ToString();
                        break;
                    case ActionType.CreditCardHold:
                        // Authorize
                        req.ccAuthService = new Cybersource.CCAuthService();
                        req.ccAuthService.run = "true";                        
                        includeBillTo = true;
                        includeCardData = true;
                        includeMerchantExtraData = true;
                        req.purchaseTotals.grandTotalAmount = t.Amount.ToString();
                        break;
                    case ActionType.CreditCardCapture:
                        // Capture, Post Authorize
                        req.ccCaptureService = new Cybersource.CCCaptureService();
                        req.ccCaptureService.run = "true";
                        req.ccCaptureService.authRequestID = t.PreviousTransactionNumber;
                        includeBillTo = true;
                        includeCardData = true;
                        includeMerchantExtraData = true;
                        req.purchaseTotals.grandTotalAmount = t.Amount.ToString();
                        break;
                    case ActionType.CreditCardVoid:
                        // Void
                        req.voidService = new Cybersource.VoidService();
                        req.voidService.run = "true";
                        req.voidService.voidRequestID = t.PreviousTransactionNumber;
                        req.purchaseTotals.grandTotalAmount = t.Amount.ToString();
                        break;
                    case ActionType.CreditCardRefund:
                        // Refund, Credit
                        req.ccCreditService = new Cybersource.CCCreditService();
                        req.ccCreditService.run = "true";
                        if (t.PreviousTransactionNumber.Length > 0)
                        {
                            // Previous Transaction Credit
                            req.ccCreditService.captureRequestID = t.PreviousTransactionNumber;
                        }
                        else
                        {                         
                            // Stand Alone Credit
                            includeCardData = true;
                        }
                        includeBillTo = true;
                        includeMerchantExtraData = true;
                        req.purchaseTotals.grandTotalAmount = t.Amount.ToString();
                        break;
                }

                if (includeBillTo)
                {
                    // Bill To Address
                    req.billTo = new Cybersource.BillTo();
                    req.billTo.city = Text.TrimToLength(t.Customer.City, 50);
                    req.billTo.company = Text.TrimToLength(t.Customer.Company, 40);

                    var country = MerchantTribe.Web.Geography.Country.FindByName(t.Customer.Country);
                    if (country != null)
                    {
                        req.billTo.country = country.IsoCode;
                    }
                    else
                    {
                        req.billTo.country = "US";                        
                    }                    
                    req.billTo.customerID = Text.TrimToLength(t.Customer.Email, 50);
                    req.billTo.email = Text.TrimToLength(t.Customer.Email, 255);
                    req.billTo.firstName = Text.TrimToLength(t.Customer.FirstName, 60);
                    req.billTo.lastName = Text.TrimToLength(t.Customer.LastName, 60);
                    req.billTo.ipAddress = t.Customer.IpAddress;
                    req.billTo.phoneNumber = Text.TrimToLength(t.Customer.Phone, 15);
                    req.billTo.postalCode = Text.TrimToLength(t.Customer.PostalCode, 10);
                    req.billTo.state = t.Customer.Region;
                    req.billTo.street1 = Text.TrimToLength(t.Customer.Street, 60);
                }

                if (includeCardData)
                {
                    req.card = new Cybersource.Card();
                    req.card.accountNumber = t.Card.CardNumber;
                    switch (MerchantTribe.Payment.CardValidator.GetCardTypeFromNumber(t.Card.CardNumber))
                    {
                        case CardType.Visa:
                            req.card.cardType = "001";
                            break;
                        case CardType.MasterCard:
                            req.card.cardType = "002";
                            break;
                        case CardType.Amex:
                            req.card.cardType = "003";
                            break;
                        case CardType.Discover:
                            req.card.cardType = "004";
                            break;
                        case CardType.DinersClub:
                            req.card.cardType = "005";
                            break;
                        case CardType.JCB:
                            req.card.cardType = "007";
                            break;
                        default:
                            req.card.cardType = "";
                            break;
                    }                    
                    req.card.expirationMonth = t.Card.ExpirationMonthPadded;
                    req.card.expirationYear = t.Card.ExpirationYear.ToString();
                    if (t.Card.SecurityCode.Length > 0)
                    {
                        req.card.cvNumber = t.Card.SecurityCode;
                    }
                }

                if (includeMerchantExtraData)
                {
                    req.merchantDefinedData = new Cybersource.MerchantDefinedData();
                    req.merchantDefinedData.field1 = t.MerchantInvoiceNumber;
                }
        
                var proc = new Cybersource.TransactionProcessorClient();
                proc.ChannelFactory.Credentials.UserName.UserName = req.merchantID;
                proc.ChannelFactory.Credentials.UserName.Password = Settings.TransactionKey;                        
                var reply = proc.runTransaction(req);

                t.Result.ReferenceNumber = reply.requestID;
                t.Result.ResponseCode = reply.reasonCode;
                t.Result.ResponseCodeDescription = ParseReasonCode(reply.reasonCode);                        

                switch (reply.decision.ToUpperInvariant())
                {
                    case "ACCEPT":
                        t.Result.Succeeded = true;
                        result = true;
                        break;
                    case "ERROR":
                        t.Result.Succeeded = false;
                        result = false;
                        break;
                    case "REJECT":
                        t.Result.Succeeded = false;
                        result = false;
                        break;
                }

                if (reply.ccAuthReply != null)
                {
                    t.Result.AvsCode = ParseAvsCode(reply.ccAuthReply.avsCode ?? string.Empty);
                    t.Result.CvvCode = ParseSecurityCode(reply.ccAuthReply.cvCode ?? string.Empty);
                }
                                                                  
            }
            catch (Exception ex)
            {
                result = false;
                t.Result.Messages.Add(new Message("Unknown Payment Error: " + ex.Message, "BVP_CYBERSOURCE_1001", MessageType.Error));
                t.Result.Messages.Add(new Message("Stack Trace " + ex.StackTrace, "STACKTRACE", MessageType.Error));
            }
            t.Result.Succeeded = result;         
        }
                
        public override MethodSettings BaseSettings
        {
            get { return Settings; }
        }

        #region Helper Methods       
        private string ParseReasonCode(string code)
        {
            Dictionary<string, string> codes = new Dictionary<string,string>();
            codes.Add("100", "Successful Transaction");
            codes.Add("101", "Request has missing fields");
            codes.Add("102", "Request has invalid data");
            codes.Add("110", "Only partial amount approved");
            codes.Add("150", "Error: General System Failure at Cybersource");
            codes.Add("151", "Error: Request received but timeout at Cybersource");
            codes.Add("152", "Error: Request received but did not complete at Cybersource");
            codes.Add("200", "Accepted but declined due to AVS checks");
            codes.Add("201", "Issuing bank has question about request. Call bank.");
            codes.Add("202", "Expired Card / Invalid Expiration Date");
            codes.Add("203", "Card Declined. No reason Given.");
            codes.Add("204", "Insufficient Funds");
            codes.Add("205", "Stolen or Lost Card");
            codes.Add("207", "Issuing Bank Unavailable");
            codes.Add("208", "Inactive Card");
            codes.Add("209", "Amex Security Code Mismatch");
            codes.Add("210", "Card has reached spending limit");
            codes.Add("211", "Invalid Security Code");
            codes.Add("221", "Customer had negative file information");
            codes.Add("230", "Accepted but declined due to Security Code check");
            codes.Add("231", "Invalid account number");
            codes.Add("232", "Card Type Not Accepted");
            codes.Add("233", "General Decline by Processor");
            codes.Add("234", "There is a problem with information in your Cybersource account. Contact Cybersource.");
            codes.Add("235", "Requested capture amount exceeds authorized amount.");
            codes.Add("236", "Processor failure at Cybersource");
            codes.Add("237", "Authorization already voided.");
            codes.Add("238", "Authorization already captured.");
            codes.Add("239", "Requested transaction amount must match previous amount.");
            codes.Add("240", "Invalid Card Type");
            codes.Add("241", "Request ID is invalid");
            codes.Add("242", "Capture request did not match any authorization at Cybersource");
            codes.Add("243", "Transaction already settled/reversed");
            codes.Add("246", "Transaction can not be voided");
            codes.Add("247", "Requested credit for a capture that was voided.");
            codes.Add("250", "Error: request received but a timeout happened at issuing bank");
            codes.Add("520", "Accepted but declined because of your Cybersource SmartAuthorization settings.");

            if (codes.ContainsKey(code.ToLowerInvariant()))
            {
                return codes[code.ToLowerInvariant()];
            }
            return code;            
        }
        private AvsResponseType ParseAvsCode(string code)
        {
            AvsResponseType result = AvsResponseType.Unavailable;

            switch (code.ToUpper())
            {
                case "A":                
                case "D":
                case "F":
                case "K":
                case "L":
                case "M":
                case "O":
                case "T":
                    result = AvsResponseType.PartialMatchAddress;
                    break;
                case "E":
                    result = AvsResponseType.Error;
                    break;
                case "I":
                case "N":
                    result = AvsResponseType.NoMatch;
                    break;
                case "B":
                case "G":
                case "P":
                case "R":
                case "S":
                case "U":
                    result = AvsResponseType.Unavailable;
                    break;
                case "W":
                case "Z":
                    result = AvsResponseType.PartialMatchPostalCode;
                    break;
                case "V":
                case "X":
                case "Y":
                    result = AvsResponseType.FullMatch;
                    break;
            }

            return result;
        }
        private CvnResponseType ParseSecurityCode(string code)
        {
            CvnResponseType result = CvnResponseType.Unavailable;

            switch (code.ToUpper())
            {
                case "D": // suspicious transaction
                case "I":
                    result = CvnResponseType.NoMatch;
                    break;
                case "M":
                    result = CvnResponseType.Match;
                    break;
                case "N":
                    result = CvnResponseType.NoMatch;
                    break;
                case "P":
                    result = CvnResponseType.Unavailable;
                    break;
                case "S":
                    result = CvnResponseType.Error;
                    break;
                case "U":
                    result = CvnResponseType.Unavailable;
                    break;
            }
            return result;
        }
        #endregion

    }
}
