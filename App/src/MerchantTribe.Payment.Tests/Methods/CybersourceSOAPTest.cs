using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MerchantTribe.Payment;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MerchantTribe.Payment.Tests.Methods
{
    [TestClass]
    public class CybersourceSOAPTest
    {
        private string _TestAmex = "378282246310005";
        private string _TestVisa = "4111111111111111";
        private string _TestMastercard = "5555555555554444";
        private string _TestDiscover = "6011111111111117";
        private string _RejectCard = "4111111111111112";

        // See this doc for complete testing amounts
        // http://apps.cybersource.com/library/documentation/sbc/SB_API_Testing/SB_API_Testing.pdf
        private decimal _AmountSuccess = 1;
        private decimal _AmountReject = -1;
        private decimal _AmountAcceptPaymentech = 2521.66m;
        private decimal _AmountAcceptFDCCompass = 164;
        private decimal _AmountAcceptFDCNashville = 3000.00m;
        private decimal _AmountAcceptFDMSNashville = 3027.00m;
        private decimal _AmountAcceptFDMSSouth = 2201.0m;
        private decimal _AmountAcceptFDMSSouthAmex = 12.34m;
        private decimal _AmountAcceptGPN = 1006.00m;
        private decimal _AmountAcceptRBSWorldpay = 97.00m;
        private decimal _AmountAcceptTSYS = 2157.0m;

        private MerchantTribe.Payment.Methods.CybersourceSOAP processor;
                       
        [TestInitialize]
        public void Setup()
        {
            processor = new Payment.Methods.CybersourceSOAP();
            processor.Settings.MerchantId = "<put your real merchant id here>";
            processor.Settings.TransactionKey = "<Put your real transaction key here>";                        
            processor.Settings.CurrencyCode = "USD";
        }

        private Transaction GetSampleTransaction()
        {
            Transaction t = new Transaction();
            
            t.Action = ActionType.CreditCardHold;
            t.Amount = _AmountSuccess;

            t.Card.CardNumber = _TestVisa;
            t.Card.CardHolderName = "No One";
            t.Card.ExpirationMonth = 12;
            t.Card.ExpirationYear = 2014;
            t.Card.SecurityCode = "123";

            t.Customer.City = "New York";
            t.Customer.Company = "None";
            t.Customer.Country = "United States";
            t.Customer.Email = "noone@nowhere.dev";
            t.Customer.FirstName = "No";
            t.Customer.LastName = "One";
            t.Customer.IpAddress = "127.0.0.1";
            t.Customer.Phone = "2125551212";
            t.Customer.PostalCode = "10038";
            t.Customer.Region = "NY";
            t.Customer.Street = "99 John Street";
            
            return t;
        }
        
        [TestMethod]
        public void CanSendHoldTransactionToCybersource()
        {
            Transaction t = GetSampleTransaction();            
            processor.ProcessTransaction(t);            
            Assert.IsNotNull(t);
            Assert.IsTrue(t.Result.Succeeded);
        }

        [TestMethod]
        public void CanSendChargeTransactionToCybersource()
        {
            Transaction t = GetSampleTransaction();
            t.Action = ActionType.CreditCardCharge;
            processor.ProcessTransaction(t);
            Assert.IsNotNull(t);
            Assert.IsTrue(t.Result.Succeeded);
        }

        [TestMethod]
        public void CanSendCaptureTransactionToCybersource()
        {
            Transaction auth = GetSampleTransaction();
            processor.ProcessTransaction(auth);
            
            Transaction t = GetSampleTransaction();
            t.Action = ActionType.CreditCardCapture;
            t.PreviousTransactionNumber = auth.Result.ReferenceNumber;
            processor.ProcessTransaction(t);
            Assert.IsNotNull(t);
            Assert.IsTrue(t.Result.Succeeded);
        }

        [TestMethod]
        public void CanSendRefundTransactionToCybersource()
        {
            Transaction t = GetSampleTransaction();
            t.Action = ActionType.CreditCardRefund;
            processor.ProcessTransaction(t);
            Assert.IsNotNull(t);
            Assert.IsTrue(t.Result.Succeeded);
        }

        //[TestMethod]
        //public void CanAnyProcessorAmountSucceed()
        //{            
        //    bool Paymentech = IsSuccess(_AmountAcceptPaymentech);
        //    bool FDCCompass = IsSuccess(_AmountAcceptFDCCompass);
        //    bool FDCNashville = IsSuccess(_AmountAcceptFDCNashville);
        //    bool FDMSNashville = IsSuccess(_AmountAcceptFDMSNashville);
        //    bool FDMSSouth = IsSuccess(_AmountAcceptFDMSSouth);
        //    bool GPN = IsSuccess(_AmountAcceptGPN);
        //    bool RBSWorldpay = IsSuccess(_AmountAcceptRBSWorldpay);
        //    bool TSYS = IsSuccess(_AmountAcceptTSYS);

        //    bool any = (Paymentech || FDCCompass || FDCNashville || FDMSNashville
        //                || FDMSSouth || GPN || RBSWorldpay || TSYS);                        
        //    Assert.IsTrue(any);
        //}

        //private bool IsSuccess(decimal amount)
        //{
        //    Transaction t = GetSampleTransaction();
        //    t.Amount = amount;
        //    processor.ProcessTransaction(t);
        //    Console.WriteLine(t.Result.Succeeded + " " + t.Result.ResponseCodeDescription);
        //    return t.Result.Succeeded;
        //}
    }
}
