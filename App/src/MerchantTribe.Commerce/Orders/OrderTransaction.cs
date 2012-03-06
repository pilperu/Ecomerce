using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MerchantTribe.Payment;
using MerchantTribe.CommerceDTO.v1.Orders;

namespace MerchantTribe.Commerce.Orders
{
    public class OrderTransaction
    {    
        public Guid Id { get; set; }
        public string IdAsString
        {
            get { return this.Id.ToString().Replace("{", "").Replace("}", ""); }
        }
        public long StoreId { get; set; }
        public string OrderId { get; set; }
        public string OrderNumber { get; set; }
        public DateTime TimeStampUtc { get; set; }
        public ActionType Action { get; set; }
        public decimal Amount { get; set; }
        public CardData CreditCard { get; set; }
        public bool Success { get; set; }
        public bool Voided { get; set; }
        public string RefNum1 { get; set; }
        public string RefNum2 { get; set; }
        public string LinkedToTransaction { get; set; }
        public string Messages { get; set; }
        public string CheckNumber { get; set; }
        public string PurchaseOrderNumber { get; set; }
        public string GiftCardNumber { get; set; }
        public string CompanyAccountNumber { get; set; }

        // Not saved to database, calculated on the fly in reports
        // In a future update these will get real values stored 
        // in DB.
        public decimal TempEstimatedItemPortion { get; set; }
        public decimal TempEstimatedItemDiscount { get; set; }
        public decimal TempEstimatedShippingPortion { get; set; }
        public decimal TempEstimatedShippingDiscount { get; set; }
        public decimal TempEstimatedHandlingPortion { get; set; }
        public decimal TempEstimatedTaxPortion { get; set; }
        public string TempCustomerName { get; set; }
        public string TempCustomerEmail { get; set; }

        public OrderTransaction()
        {
            Id = System.Guid.NewGuid();
            StoreId = 0;
            OrderId = string.Empty;
            OrderNumber = string.Empty;
            TimeStampUtc = DateTime.UtcNow;
            Action = ActionType.Uknown;
            Amount = 0m;
            CreditCard = new CardData();
            Success = false;
            Voided = false;
            RefNum1 = string.Empty;
            RefNum2 = string.Empty;
            LinkedToTransaction = string.Empty;
            Messages = string.Empty;
            PurchaseOrderNumber = string.Empty;
            GiftCardNumber = string.Empty;
            CheckNumber = string.Empty;
            CompanyAccountNumber = string.Empty;
        }

        public OrderTransaction(MerchantTribe.Payment.Transaction t)
        {
            Id = System.Guid.NewGuid();
            StoreId = 0;
            OrderId = string.Empty;
            LinkedToTransaction = string.Empty;
            PopulateFromPaymentTransaction(t);
        }

        public void PopulateFromPaymentTransaction(MerchantTribe.Payment.Transaction t)
        {
            if (t != null)
            {
                TimeStampUtc = DateTime.UtcNow;
                Action = t.Action;
                Amount = t.Amount;
                if (t.IsRefundTransaction)
                {
                    Amount = (t.Amount * -1);
                }
                CreditCard = t.Card;
                Success = t.Result.Succeeded;
                Voided = false;
                RefNum1 = t.Result.ReferenceNumber;
                RefNum2 = t.Result.ReferenceNumber2;
                Messages = string.Empty;
                if (t.Result.Messages.Count > 0)
                {
                    foreach (Message m in t.Result.Messages)
                    {
                        Messages += ":: " + m.Code + " - " + m.Description + " ";
                    }
                }
                this.CheckNumber = t.CheckNumber;
                this.PurchaseOrderNumber = t.PurchaseOrderNumber;
                this.GiftCardNumber = t.GiftCardNumber;
                this.CompanyAccountNumber = t.CompanyAccountNumber;                
            }
        }
       
        // Only allow voids for about 16 hours since most credit card
        // companies don't allow voids after the batch is processed for the day
        // Merchants should use a Refund instead of void at that point
        public bool IsVoidable { 
            get {
                
                bool isWithinTimeWindow = false;

                long cutOffTicks= DateTime.UtcNow.AddHours(-16).Ticks;
                long timestampTicks = TimeStampUtc.Ticks;
                if (timestampTicks >= cutOffTicks)
                {
                    isWithinTimeWindow =  true;
                }

                if (isWithinTimeWindow)
                {
                    if (this.Action == ActionType.CreditCardCapture ||
                        this.Action == ActionType.CreditCardCharge ||
                        this.Action == ActionType.CreditCardHold ||
                        this.Action == ActionType.CreditCardRefund ||
                        this.Action == ActionType.PayPalCapture ||
                        this.Action == ActionType.PayPalCharge ||
                        this.Action == ActionType.PayPalHold ||
                        this.Action == ActionType.PayPalRefund)
                        return true;
                }
                return false;             
            } 
        }

        public decimal AmountAppliedToOrder
        {
            get
            {
                decimal result = 0;

                if (this.Success)
                {
                    if (!this.Voided)
                    {                        
                        if (this.Action == ActionType.CreditCardCapture || 
                            this.Action == ActionType.CreditCardCharge ||
                            this.Action == ActionType.CreditCardRefund || 
                            this.Action == ActionType.CashReceived ||
                            this.Action == ActionType.CashReturned ||
                            this.Action == ActionType.CheckReceived ||
                            this.Action == ActionType.CheckReturned ||
                            this.Action == ActionType.GiftCardCapture ||
                            this.Action == ActionType.GiftCardDecrease ||
                            this.Action == ActionType.GiftCardIncrease ||
                            this.Action == ActionType.PayPalCapture ||
                            this.Action == ActionType.PayPalCharge ||
                            this.Action == ActionType.PayPalRefund ||
                            this.Action == ActionType.PurchaseOrderAccepted ||
                            this.Action == ActionType.CompanyAccountAccepted ||
                            this.Action == ActionType.RewardPointsDecrease ||
                            this.Action == ActionType.RewardPointsCapture ||
                            this.Action == ActionType.RewardPointsIncrease
                            )
                        {                            
                            result += this.Amount;
                        }
                    }
                }

                return result;
            }
        }
        public decimal AmountHeldForOrder
        {
            get
            {
                decimal result = 0;

                if (this.Success)
                {
                    if (!this.Voided)
                    {
                        if (this.Action == ActionType.CreditCardHold || 
                            this.Action == ActionType.GiftCardHold ||
                            this.Action == ActionType.PayPalHold ||
                            this.Action == ActionType.RewardPointsHold ||
                            this.Action == ActionType.RewardPointsUnHold)
                        {
                            result += this.Amount;
                        }
                    }
                }

                return result;
            }
        }

        //public Orders.Order FindOrderForThis()
        //{
        //    if (!String.IsNullOrEmpty(this.OrderId))
        //    {
        //        return Orders.Order.FindByBvin(this.OrderId);
        //    }
        //    return null;
        //}

        public bool HasSuccessfulLinkedAction(MerchantTribe.Payment.ActionType action, List<OrderTransaction> transactions)
        {
            foreach (OrderTransaction t in transactions)
            {
                if (t.Success)
                {
                    if (!t.Voided)
                    {
                        if (t.Action == action)
                        {
                            if (t.LinkedToTransaction == this.IdAsString)
                            {
                                return true;
                            }
                        }
                    }
                }
            }

            return false;
        }

        //DTO
        public OrderTransactionDTO ToDto()
        {
            OrderTransactionDTO dto = new OrderTransactionDTO();

            dto.Action = (OrderTransactionActionDTO)((int)this.Action);
            dto.Amount = this.Amount;
            dto.CheckNumber = this.CheckNumber;
            dto.CompanyAccountNumber = this.CompanyAccountNumber;
            dto.CreditCard = new OrderTransactionCardDataDTO();
            dto.CreditCard.CardHolderName = this.CreditCard.CardHolderName;
            dto.CreditCard.CardIsEncrypted = false;
            dto.CreditCard.CardNumber = this.CreditCard.CardNumber;
            dto.CreditCard.ExpirationMonth = this.CreditCard.ExpirationMonth;
            dto.CreditCard.ExpirationYear = this.CreditCard.ExpirationYear;
            dto.CreditCard.SecurityCode = this.CreditCard.SecurityCode;
            dto.GiftCardNumber = this.GiftCardNumber;
            dto.Id = this.Id;
            dto.LinkedToTransaction = this.LinkedToTransaction;
            dto.Messages = this.Messages;
            dto.OrderId = this.OrderId;
            dto.OrderNumber = this.OrderNumber;
            dto.PurchaseOrderNumber = this.PurchaseOrderNumber;
            dto.RefNum1 = this.RefNum1;
            dto.RefNum2 = this.RefNum2;
            dto.StoreId = this.StoreId;
            dto.Success = this.Success;
            dto.TimeStampUtc = this.TimeStampUtc;
            dto.Voided = this.Voided;

            return dto;
        }
        public void FromDto(OrderTransactionDTO dto)
        {
            if (dto == null) return;

            this.Action = (ActionType)((int)dto.Action);
            this.Amount = dto.Amount;
            this.CheckNumber = dto.CheckNumber ?? string.Empty;
            this.CompanyAccountNumber = dto.CompanyAccountNumber ?? string.Empty;
            if (dto.CreditCard != null)
            {
                if (dto.CreditCard.CardIsEncrypted == false)
                {
                    this.CreditCard.CardNumber = dto.CreditCard.CardNumber ?? string.Empty;                        
                }
                this.CreditCard.CardHolderName = dto.CreditCard.CardHolderName ?? string.Empty;                
                this.CreditCard.ExpirationMonth = dto.CreditCard.ExpirationMonth;
                this.CreditCard.ExpirationYear = dto.CreditCard.ExpirationYear;
                this.CreditCard.SecurityCode = dto.CreditCard.SecurityCode ?? string.Empty;
            }
            this.GiftCardNumber = dto.GiftCardNumber ?? string.Empty;
            this.Id = dto.Id;
            this.LinkedToTransaction = dto.LinkedToTransaction ?? string.Empty;
            this.Messages = dto.Messages ?? string.Empty;
            this.OrderId = dto.OrderId ?? string.Empty;
            this.OrderNumber = dto.OrderNumber ?? string.Empty;
            this.PurchaseOrderNumber = dto.PurchaseOrderNumber ?? string.Empty;
            this.RefNum1 = dto.RefNum1 ?? string.Empty;
            this.RefNum2 = dto.RefNum2 ?? string.Empty;
            this.StoreId = dto.StoreId;
            this.Success = dto.Success;
            this.TimeStampUtc = dto.TimeStampUtc;
            this.Voided = dto.Voided;
        }
    }
}

