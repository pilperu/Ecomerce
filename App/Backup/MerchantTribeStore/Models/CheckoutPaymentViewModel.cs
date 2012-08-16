using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MerchantTribe.Commerce;
using MerchantTribe.Commerce.Orders;
using MerchantTribe.Payment;

namespace MerchantTribeStore.Models
{
    public class CheckoutPaymentViewModel
    {
        public bool NoPaymentNeeded { get; set; }        
        public bool IsCreditCardActive { get; set; }        
        public bool IsPayPalActive { get; set; }        
        public bool IsTelephoneActive { get; set; }
        public bool IsPurchaseOrderActive { get; set; }
        public bool IsCheckActive { get; set; }
        public bool IsCodActive { get; set; }
        public bool IsCompanyAccountActive { get; set; }
        public string NoPaymentNeededDescription { get; set; }
        public string TelephoneDescription { get; set; }
        public string PurchaseOrderDescription { get; set; }
        public string CheckDescription { get; set; }
        public string CodDescription { get; set; }
        public string CompanyAccountDescription { get; set; }
        public string SelectedPayment { get; set; }
        public string DataPurchaseOrderNumber { get; set; }
        public string DataCompanyAccountNumber { get; set; }
        public CardData DataCreditCard { get; set; }
        public List<CardType> AcceptedCardTypes { get; set; }

        public CheckoutPaymentViewModel()
        {
            this.NoPaymentNeeded = false;
            this.IsCreditCardActive = false;
            this.IsPayPalActive = false;
            this.IsTelephoneActive = false;
            this.IsPurchaseOrderActive = false;
            this.IsCheckActive = false;
            this.IsCodActive = false;
            this.IsCompanyAccountActive = false;
            this.NoPaymentNeededDescription = string.Empty;                        
            this.TelephoneDescription = string.Empty;
            this.PurchaseOrderDescription = string.Empty;
            this.CheckDescription = string.Empty;
            this.CodDescription = string.Empty;
            this.CompanyAccountDescription = string.Empty;
            this.SelectedPayment = string.Empty;
            this.DataCompanyAccountNumber = string.Empty;
            this.DataCreditCard = new CardData();
            this.DataPurchaseOrderNumber = string.Empty;
            this.AcceptedCardTypes = new List<CardType>();
        }
        
    }
}