using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MerchantTribe.Payment;
using MerchantTribe.Commerce.Orders;
using MerchantTribe.Commerce.Payment;
using MerchantTribe.Commerce.Membership;

namespace MerchantTribe.Commerce.Orders
{
    public class OrderPaymentManager
    {

        private Order o = null;

        private MerchantTribeApplication MTApp = null;

        private OrderService svc = null;
        private Contacts.ContactService contacts = null;
        private Content.ContentService content = null;

        private CustomerPointsManager pointsManager = null;
        

        public OrderPaymentManager(Order ord, MerchantTribeApplication app)
        {
            o = ord;
            this.MTApp = app;
            svc = MTApp.OrderServices;
            this.contacts = this.MTApp.ContactServices;
            this.content = this.MTApp.ContentServices;

            Accounts.Store CurrentStore = app.CurrentRequestContext.CurrentStore;

            pointsManager = CustomerPointsManager.InstantiateForDatabase(CurrentStore.Settings.RewardsPointsIssuedPerDollarSpent,
                                                        CurrentStore.Settings.RewardsPointsNeededPerDollarCredit,
                                                       app.CurrentRequestContext.CurrentStore.Id);
        }

        private List<OrderTransaction> FindAllTransactionsOfType(ActionType type)
        {
            List<OrderTransaction> result = new List<OrderTransaction>();

            foreach (OrderTransaction t in svc.Transactions.FindForOrder(o.bvin))
            {
                if (t.Action == type)
                {
                    result.Add(t);
                }
            }
            return result;
        }
        private OrderTransaction FindSingleTransactionByTypeAndId(ActionType type, string Id)
        {
            foreach (OrderTransaction t in svc.Transactions.FindForOrder(o.bvin))
            {
                if (t.Action == type)
                {
                    if (t.IdAsString == Id) return t;
                }
            }
            return null;
        }

        private decimal EnsurePositiveAmount(decimal input)
        {
            if (input < 0)
            {
                return input * -1;
            }
            return input;
        }
        public OrderTransaction FindTransactionById(string id)
        {
            foreach (OrderTransaction t in svc.Transactions.FindForOrder(o.bvin))
            {
                if (t.IdAsString.ToLower() == id.ToLower())
                {
                    return t;
                }
            }
            return null;
        }
        public bool ClearAllTransactions()
        {            
            foreach (OrderTransaction t in svc.Transactions.FindForOrder(o.bvin))
            {                
                    svc.Transactions.Delete(t.Id);
            }        
    
            //reload local if we were using local store
            return true;
        }
        public bool ClearAllNonStoreCreditTransactions()
        {
            foreach (OrderTransaction t in svc.Transactions.FindForOrder(o.bvin))
            {
                if (t.Action == ActionType.RewardPointsInfo ||
                    t.Action == ActionType.GiftCardInfo)
                {
                    continue;
                }
                svc.Transactions.Delete(t.Id);
            }
            //reload local if we were using local store
            return true;
        }

        private void UpdateOrderPaymentStatus()
        {
            Orders.OrderPaymentStatus previousPaymentStatus = o.PaymentStatus;
            svc.EvaluatePaymentStatus(o);
            BusinessRules.OrderTaskContext context = new BusinessRules.OrderTaskContext(this.MTApp);
            context.Order = o;
            context.UserId = o.UserID;
            context.Inputs.Add("bvsoftware", "PreviousPaymentStatus", previousPaymentStatus.ToString());
            BusinessRules.Workflow.RunByName(context, BusinessRules.WorkflowNames.PaymentChanged);
        }

        // Offline Payment Info
        public bool OfflinePaymentAddInfo(decimal amount, string description)
        {        
            Transaction t = o.GetEmptyTransaction();
            t.Amount = EnsurePositiveAmount(amount);            
            t.Action = ActionType.OfflinePaymentRequest;
            OrderTransaction ot = new OrderTransaction(t);
            ot.Messages = description;
            ot.Success = true;
            return svc.AddPaymentTransactionToOrder(o, ot, this.MTApp);
        }

        // Cash
        public bool CashReceive(decimal amount)
        {
            Transaction t = o.GetEmptyTransaction();
            t.Amount = EnsurePositiveAmount(amount);
            t.Action = ActionType.CashReceived;
            OrderTransaction ot = new OrderTransaction(t);
            ot.Success = true;            
            return svc.AddPaymentTransactionToOrder(o, ot, this.MTApp);
        }
        public bool CashRefund(decimal amount)
        {            
            Transaction t = o.GetEmptyTransaction();
            t.Amount = EnsurePositiveAmount(amount);
            t.Action = ActionType.CashReturned;
            OrderTransaction ot = new OrderTransaction(t);
            ot.Success = true;
            return svc.AddPaymentTransactionToOrder(o, ot, this.MTApp);
        }

        // Checks
        public bool CheckReceive(decimal amount, string checkNumber)
        {            
            Transaction t = o.GetEmptyTransaction();
            t.Amount = EnsurePositiveAmount(amount);
            t.CheckNumber = checkNumber;
            t.Action = ActionType.CheckReceived;
            OrderTransaction ot = new OrderTransaction(t);
            ot.Success = true;
            return svc.AddPaymentTransactionToOrder(o, ot, this.MTApp);
        }
        public bool CheckReturn(decimal amount, string checkNumber)
        {
            Transaction t = o.GetEmptyTransaction();
            t.Amount = EnsurePositiveAmount(amount);
            t.CheckNumber = checkNumber;
            t.Action = ActionType.CheckReturned;
            OrderTransaction ot = new OrderTransaction(t);
            ot.Success = true;
            return svc.AddPaymentTransactionToOrder(o, ot, this.MTApp);
        }      
             
        // Purchase Order
        public bool PurchaseOrderAccept(string poNumber)
        {
            Transaction t = o.GetEmptyTransaction();                        
            t.PurchaseOrderNumber = poNumber;
            t.Action = ActionType.PurchaseOrderAccepted;
            OrderTransaction ot = new OrderTransaction(t);

            OrderTransaction existing = LocateExistingPurchaseOrder(poNumber);
            if (existing == null)
            {
                ot.Success = false;
                ot.Messages = "Could not located the requested PO.";
            }
            else
            {

                if (existing.HasSuccessfulLinkedAction(ActionType.PurchaseOrderAccepted, svc.Transactions.FindForOrder(o.bvin)))
                {
                    // Fail, already accepted
                    ot.Success = false;
                    ot.Messages = "The requested PO has already been accepted.";
                }

                // Succes, receive it, link it to the info transaction
                ot.Amount = EnsurePositiveAmount(existing.Amount);
                ot.LinkedToTransaction = existing.IdAsString;
                ot.Success = true;                
            }
            return svc.AddPaymentTransactionToOrder(o, ot, this.MTApp);
        }
        private OrderTransaction LocateExistingPurchaseOrder(string poNumber)
        {
            foreach (OrderTransaction t in PurchaseOrderInfoListAll())
            {
                if (t.PurchaseOrderNumber.ToLower() == poNumber.Trim().ToLower())
                {
                    return t;
                }
            }
            return null;
        }
        public List<OrderTransaction> PurchaseOrderInfoListAll()
        {
            return FindAllTransactionsOfType(ActionType.PurchaseOrderInfo);
        }
        public List<OrderTransaction> PurchaseOrderInfoListAllNonAccepted()
        {
            List<OrderTransaction> result = new List<OrderTransaction>();
            {
                foreach (OrderTransaction t in PurchaseOrderInfoListAll())
                {
                    if (t.HasSuccessfulLinkedAction(ActionType.PurchaseOrderAccepted, svc.Transactions.FindForOrder(o.bvin)) == false)
                    {
                        result.Add(t);
                    }
                }
            }

            return result;
        }
        public bool PurchaseOrderAddInfo(string poNumber, decimal amount)
        {
            Transaction t = o.GetEmptyTransaction();
            t.Amount = EnsurePositiveAmount(amount);
            t.PurchaseOrderNumber = poNumber;
            t.Action = ActionType.PurchaseOrderInfo;
            OrderTransaction ot = new OrderTransaction(t);
            ot.Success = true;
            return svc.AddPaymentTransactionToOrder(o, ot, this.MTApp);
        }

        // CompanyAccount
        public bool CompanyAccountAccept(string accountNumber)
        {
            Transaction t = o.GetEmptyTransaction();
            t.CompanyAccountNumber = accountNumber;
            t.Action = ActionType.CompanyAccountAccepted;
            OrderTransaction ot = new OrderTransaction(t);

            OrderTransaction existing = LocateExistingCompanyAccount(accountNumber);
            if (existing == null)
            {
                ot.Success = false;
                ot.Messages = "Could not located the requested Company Account.";
            }
            else
            {

                if (existing.HasSuccessfulLinkedAction(ActionType.CompanyAccountAccepted, svc.Transactions.FindForOrder(o.bvin)))
                {
                    // Fail, already accepted
                    ot.Success = false;
                    ot.Messages = "The requested Company Account has already been accepted.";
                }

                // Succes, receive it, link it to the info transaction
                ot.Amount = EnsurePositiveAmount(existing.Amount);
                ot.LinkedToTransaction = existing.IdAsString;
                ot.Success = true;
            }
            return svc.AddPaymentTransactionToOrder(o, ot, this.MTApp);
        }
        private OrderTransaction LocateExistingCompanyAccount(string accountNumber)
        {
            foreach (OrderTransaction t in CompanyAccountInfoListAll())
            {
                if (t.CompanyAccountNumber.ToLower() == accountNumber.Trim().ToLower())
                {
                    return t;
                }
            }
            return null;
        }
        public List<OrderTransaction> CompanyAccountInfoListAll()
        {
            return FindAllTransactionsOfType(ActionType.CompanyAccountInfo);
        }
        public List<OrderTransaction> CompanyAccountInfoListAllNonAccepted()
        {
            List<OrderTransaction> result = new List<OrderTransaction>();
            {
                foreach (OrderTransaction t in CompanyAccountInfoListAll())
                {
                    if (t.HasSuccessfulLinkedAction(ActionType.CompanyAccountAccepted, svc.Transactions.FindForOrder(o.bvin)) == false)
                    {
                        result.Add(t);
                    }
                }
            }

            return result;
        }
        public bool CompanyAccountAddInfo(string accountNumber, decimal amount)
        {
            Transaction t = o.GetEmptyTransaction();
            t.Amount = EnsurePositiveAmount(amount);
            t.CompanyAccountNumber = accountNumber;
            t.Action = ActionType.CompanyAccountInfo;
            OrderTransaction ot = new OrderTransaction(t);
            ot.Success = true;
            return svc.AddPaymentTransactionToOrder(o, ot, this.MTApp);
        }

        // Credit Cards
        public bool CreditCardAddInfo(CardData card, decimal amount)
        {
            Transaction t = o.GetEmptyTransaction();
            t.Amount = EnsurePositiveAmount(amount);
            t.Card = card;
            t.Action = ActionType.CreditCardInfo;
            OrderTransaction ot = new OrderTransaction(t);
            ot.Success = true;
            return svc.AddPaymentTransactionToOrder(o, ot, this.MTApp);
        }
        public bool CreditCardHold(string infoTransactionId, decimal amount)
        {
            OrderTransaction t = CreditCardInfoFind(infoTransactionId);
            return CreditCardHold(t, amount);
        }
        public bool CreditCardHold(OrderTransaction infoTransaction, decimal amount)
        {
            if (infoTransaction == null) return false;

            Transaction t = o.GetEmptyTransaction();
            t.Card = infoTransaction.CreditCard;
            t.Action = ActionType.CreditCardHold;
            t.Amount = EnsurePositiveAmount(amount);
            OrderTransaction ot = new OrderTransaction(t);
            
            if (infoTransaction.Action != ActionType.CreditCardInfo)
            {
                ot.Success = false;
                ot.Messages = "Transaction must be CC info type to process.";
                return svc.AddPaymentTransactionToOrder(o, ot, this.MTApp);
            }
            
            RequestContext context = this.MTApp.CurrentRequestContext;
            Method processor = context.CurrentStore.Settings.PaymentCurrentCreditCardProcessor();
            if (processor != null)
            {
                processor.ProcessTransaction(t);
                ot = new OrderTransaction(t);
                ot.LinkedToTransaction = infoTransaction.IdAsString;
            }

            return svc.AddPaymentTransactionToOrder(o, ot, this.MTApp);         
        }
        public List<OrderTransaction> CreditCardInfoListAll()
        {
            return FindAllTransactionsOfType(ActionType.CreditCardInfo);
        }
        public OrderTransaction CreditCardInfoFind(string id)
        {
            return FindSingleTransactionByTypeAndId(ActionType.CreditCardInfo, id);
        }
        public List<OrderTransaction> CreditCardHoldListAll()
        {
            List<OrderTransaction> result = new List<OrderTransaction>();

            foreach (OrderTransaction t in svc.Transactions.FindForOrder(o.bvin))
            {
                if (t.Action == ActionType.CreditCardHold)
                {
                    if (!t.Voided && t.Success)
                    {
                        result.Add(t);
                    }
                }
            }
            return result;
        }
        public List<OrderTransaction> CreditCardChargeOrCaptureListAll()
        {
            List<OrderTransaction> result = new List<OrderTransaction>();

            foreach (OrderTransaction t in svc.Transactions.FindForOrder(o.bvin))
            {
                if (t.Action == ActionType.CreditCardCapture || t.Action == ActionType.CreditCardCharge)
                {
                    if (!t.Voided && t.Success)
                    {
                        result.Add(t);
                    }
                }
            }
            return result;
        }
        public OrderTransaction CreditCardHoldFind(string id)
        {
            foreach (OrderTransaction t in CreditCardHoldListAll())
            {
                if (t.IdAsString == id)
                {
                    return t;
                }
            }
            return null;
        }
        public List<OrderTransaction> CreditCardChargeListAllRefundable()
        {
            List<OrderTransaction> result = new List<OrderTransaction>();

            foreach (OrderTransaction t in svc.Transactions.FindForOrder(o.bvin))
            {
                if (t.Action == ActionType.CreditCardCapture ||
                    t.Action == ActionType.CreditCardCharge)
                {
                    if (!t.Voided)
                    {
                        if (t.Success)
                        {
                            result.Add(t);
                        }
                    }
                }
            }
            return result;
        }        
        public bool CreditCardCapture(string holdTransactionId, decimal amount)
        {
            OrderTransaction t = CreditCardHoldFind(holdTransactionId);
            return CreditCardCapture(t, amount);
        }
        public bool CreditCardCapture(OrderTransaction holdTransaction, decimal amount)
        {
            if (holdTransaction == null) return false;

            Transaction t = o.GetEmptyTransaction();
            t.Card = holdTransaction.CreditCard;
            t.Action = ActionType.CreditCardCapture;
            t.Amount = EnsurePositiveAmount(amount);
            t.PreviousTransactionNumber = holdTransaction.RefNum1;
            t.PreviousTransactionAuthCode = holdTransaction.RefNum2;
            OrderTransaction ot = new OrderTransaction(t);

            if (holdTransaction.Action != ActionType.CreditCardHold)
            {
                ot.Success = false;
                ot.Messages = "Transaction must be CC hold type to process.";
                return svc.AddPaymentTransactionToOrder(o, ot, this.MTApp);
            }

            RequestContext context = this.MTApp.CurrentRequestContext;
            Method processor = context.CurrentStore.Settings.PaymentCurrentCreditCardProcessor();
            if (processor != null)
            {
                processor.ProcessTransaction(t);
                ot = new OrderTransaction(t);
                ot.LinkedToTransaction = holdTransaction.IdAsString;
            }
            return svc.AddPaymentTransactionToOrder(o, ot, this.MTApp);
        }
        public bool CreditCardCharge(string infoTransactionId, decimal amount)
        {
            OrderTransaction t = CreditCardInfoFind(infoTransactionId);
            return CreditCardCharge(t, amount);
        }
        public bool CreditCardCharge(OrderTransaction infoTransaction, decimal amount)
        {
            if (infoTransaction == null) return false;

            Transaction t = o.GetEmptyTransaction();
            t.Card = infoTransaction.CreditCard;
            t.Action = ActionType.CreditCardCharge;
            t.Amount = EnsurePositiveAmount(amount);
            OrderTransaction ot = new OrderTransaction(t);

            if (infoTransaction.Action != ActionType.CreditCardInfo)
            {
                ot.Success = false;
                ot.Messages = "Transaction must be CC info type to process.";
                return svc.AddPaymentTransactionToOrder(o, ot, this.MTApp);
            }

            RequestContext context = this.MTApp.CurrentRequestContext;
            Method processor = context.CurrentStore.Settings.PaymentCurrentCreditCardProcessor();
            if (processor != null)
            {
                processor.ProcessTransaction(t);
                ot = new OrderTransaction(t);
                ot.LinkedToTransaction = infoTransaction.IdAsString;
            }

            return svc.AddPaymentTransactionToOrder(o, ot, this.MTApp);
        }
        public bool CreditCardRefund(string previousTransaction, decimal amount)
        {
            OrderTransaction t = FindTransactionById(previousTransaction);
            return CreditCardRefund(t, amount);
        }
        public bool CreditCardRefund(OrderTransaction previousTransaction, decimal amount)
        {
            if (previousTransaction == null) return false;

            Transaction t = o.GetEmptyTransaction();
            t.Card = previousTransaction.CreditCard;
            t.Action = ActionType.CreditCardRefund;
            t.Amount = EnsurePositiveAmount(amount);
            t.PreviousTransactionNumber = previousTransaction.RefNum1;
            t.PreviousTransactionAuthCode = previousTransaction.RefNum2;
            OrderTransaction ot = new OrderTransaction(t);

            if (previousTransaction.Action != ActionType.CreditCardCapture 
                && previousTransaction.Action != ActionType.CreditCardCharge
                && previousTransaction.Action != ActionType.CreditCardInfo)
            {
                ot.Success = false;
                ot.Messages = "Transaction must be CC capture or charge type to refund.";
                return svc.AddPaymentTransactionToOrder(o, ot, this.MTApp);
            }

            RequestContext context = this.MTApp.CurrentRequestContext;
            Method processor = context.CurrentStore.Settings.PaymentCurrentCreditCardProcessor();
            if (processor != null)
            {
                processor.ProcessTransaction(t);
                ot = new OrderTransaction(t);
                ot.LinkedToTransaction = previousTransaction.IdAsString;
            }
            return svc.AddPaymentTransactionToOrder(o, ot, this.MTApp);
        }
        public bool CreditCardVoid(string previousTransaction, decimal amount)
        {
            OrderTransaction t = FindTransactionById(previousTransaction);
            return CreditCardVoid(t, amount);
        }
        public bool CreditCardVoid(OrderTransaction previousTransaction, decimal amount)
        {
            if (previousTransaction == null) return false;
            
            Transaction t = o.GetEmptyTransaction();
            t.Card = previousTransaction.CreditCard;
            t.Action = ActionType.CreditCardVoid;
            t.Amount = EnsurePositiveAmount(amount);
            t.PreviousTransactionNumber = previousTransaction.RefNum1;
            t.PreviousTransactionAuthCode = previousTransaction.RefNum2;
            OrderTransaction ot = new OrderTransaction(t);

            if (!previousTransaction.IsVoidable)
            {
                ot.Success = false;
                ot.Messages = "Transaction can not be voided.";
                return svc.AddPaymentTransactionToOrder(o, ot, this.MTApp);
            }

            RequestContext context = this.MTApp.CurrentRequestContext;
            Method processor = context.CurrentStore.Settings.PaymentCurrentCreditCardProcessor();
            if (processor != null)
            {
                processor.ProcessTransaction(t);
                ot = new OrderTransaction(t);
                ot.LinkedToTransaction = previousTransaction.IdAsString;

                // if the void went through, make sure we mark the previous transaction as voided
                if (t.Result.Succeeded)
                {
                    previousTransaction.Voided = true;
                    svc.Transactions.Update(previousTransaction);
                }
            }
            return svc.AddPaymentTransactionToOrder(o, ot, this.MTApp);
        }
        public bool CreditCardCompleteAllCreditCards()
        {
            bool result = true;

            RequestContext currentContext = this.MTApp.CurrentRequestContext;

            foreach (Orders.OrderTransaction p in svc.Transactions.FindForOrder(o.bvin))
            {

                List<Orders.OrderTransaction> transactions = svc.Transactions.FindForOrder(o.bvin);

                if (p.Action == MerchantTribe.Payment.ActionType.CreditCardInfo ||
                    p.Action == MerchantTribe.Payment.ActionType.CreditCardHold)
                {
                    // if we already have an auth or charge on the card, skip
                    if (p.HasSuccessfulLinkedAction(MerchantTribe.Payment.ActionType.CreditCardCharge, transactions) ||
                        p.HasSuccessfulLinkedAction(MerchantTribe.Payment.ActionType.CreditCardHold, transactions) ||
                        p.HasSuccessfulLinkedAction(MerchantTribe.Payment.ActionType.CreditCardCapture, transactions)
                        )
                    {
                        continue;
                    }

                    try
                    {
                        MerchantTribe.Payment.Transaction t = o.GetEmptyTransaction();
                        t.Card = p.CreditCard;
                        t.Amount = p.Amount;

                        if (p.Action == MerchantTribe.Payment.ActionType.CreditCardHold)
                        {
                            t.Action = MerchantTribe.Payment.ActionType.CreditCardCapture;
                        }
                        else
                        {
                            t.Action = MerchantTribe.Payment.ActionType.CreditCardCharge;
                        }

                        MerchantTribe.Payment.Method proc = currentContext.CurrentStore.Settings.PaymentCurrentCreditCardProcessor();
                        proc.ProcessTransaction(t);

                        Orders.OrderTransaction ot = new Orders.OrderTransaction(t);
                        ot.LinkedToTransaction = p.IdAsString;
                        svc.AddPaymentTransactionToOrder(o, ot, this.MTApp);

                        if (t.Result.Succeeded == false) result = false;

                    }
                    catch (Exception ex)
                    {
                        EventLog.LogEvent(ex);
                    }
                }

                Orders.OrderPaymentStatus previousPaymentStatus = o.PaymentStatus;
                svc.EvaluatePaymentStatus(o);
                BusinessRules.OrderTaskContext context = new BusinessRules.OrderTaskContext(this.MTApp);
                context.Order = o;
                context.UserId = o.UserID;
                context.Inputs.Add("bvsoftware", "PreviousPaymentStatus", previousPaymentStatus.ToString());
                BusinessRules.Workflow.RunByName(context, BusinessRules.WorkflowNames.PaymentChanged);
            }

            return result;
        }

        // PayPal        
        public bool PayPalExpressAddInfo(decimal amount, string token, string payerId)
        {
            Transaction t = o.GetEmptyTransaction();
            t.Amount = EnsurePositiveAmount(amount);
            t.Action = ActionType.PayPalExpressCheckoutInfo;
            OrderTransaction ot = new OrderTransaction(t);            
            ot.Success = true;
            ot.RefNum1 = token;
            ot.RefNum2 = payerId;
            return svc.AddPaymentTransactionToOrder(o, ot, this.MTApp);
        }
        public bool PayPalExpressHasInfo()
        {
            List<OrderTransaction> result = PayPalExpressInfoListAll();
            if (result.Count > 0) return true;
            return false;          
        }
        public List<OrderTransaction> PayPalExpressInfoListAll()
        {
            List<OrderTransaction> result = new List<OrderTransaction>();

            foreach (OrderTransaction t in svc.Transactions.FindForOrder(o.bvin))
            {
                if (t.Action == ActionType.PayPalExpressCheckoutInfo)
                {
                    result.Add(t);
                }
            }
            return result;
        }        
        public OrderTransaction PayPalExpressInfoFind(string id)
        {
            foreach (OrderTransaction t in PayPalExpressInfoListAll())
            {
                if (t.IdAsString == id)
                {
                    return t;
                }
            }
            return null;
        }
        public List<OrderTransaction> PayPalExpressHoldListAll()
        {
            List<OrderTransaction> result = new List<OrderTransaction>();

            foreach (OrderTransaction t in svc.Transactions.FindForOrder(o.bvin))
            {
                if (t.Action == ActionType.PayPalHold)
                {
                    if (!t.Voided && t.Success)
                    {
                        result.Add(t);
                    }
                }
            }
            return result;
        }
        public OrderTransaction PayPalExpressHoldFind(string id)
        {
            foreach (OrderTransaction t in PayPalExpressHoldListAll())
            {
                if (t.IdAsString == id)
                {
                    return t;
                }
            }
            return null;
        }
        public List<OrderTransaction> PayPalExpressListAllRefundable()
        {
            List<OrderTransaction> result = new List<OrderTransaction>();

            foreach (OrderTransaction t in svc.Transactions.FindForOrder(o.bvin))
            {
                if (t.Action == ActionType.PayPalCapture ||
                    t.Action == ActionType.PayPalCharge)
                {
                    if (!t.Voided)
                    {
                        if (t.Success)
                        {
                            result.Add(t);
                        }
                    }
                }
            }
            return result;
        }        
        public bool PayPalExpressHold(string infoTransactionId, decimal amount)
        {
            OrderTransaction t = PayPalExpressInfoFind(infoTransactionId);
            return PayPalExpressHold(t, amount);
        }
        public bool PayPalExpressHold(OrderTransaction infoTransaction, decimal amount)
        {
            if (infoTransaction == null) return false;

            Transaction t = o.GetEmptyTransaction();
            t.Card = infoTransaction.CreditCard;
            t.Action = ActionType.PayPalHold;
            t.Amount = EnsurePositiveAmount(amount);
            t.PreviousTransactionNumber = infoTransaction.RefNum1;
            t.PreviousTransactionAuthCode = infoTransaction.RefNum2;
            OrderTransaction ot = new OrderTransaction(t);

            if (infoTransaction.Action != ActionType.PayPalExpressCheckoutInfo)
            {
                ot.Success = false;
                ot.Messages = "Transaction must be PayPal info type to process.";
                return svc.AddPaymentTransactionToOrder(o, ot, this.MTApp);
            }

            Payment.Method.PaypalExpress processor = new Payment.Method.PaypalExpress();
            if (processor != null)
            {
                processor.Authorize(t, this.MTApp);
                ot = new OrderTransaction(t);
                ot.LinkedToTransaction = infoTransaction.IdAsString;
            }

            return svc.AddPaymentTransactionToOrder(o, ot, this.MTApp);
        }
        public bool PayPalExpressCapture(string holdTransactionId, decimal amount)
        {
            OrderTransaction t = PayPalExpressHoldFind(holdTransactionId);
            return PayPalExpressCapture(t, amount);
        }
        public bool PayPalExpressCapture(OrderTransaction holdTransaction, decimal amount)
        {
            if (holdTransaction == null) return false;

            Transaction t = o.GetEmptyTransaction();
            t.Action = ActionType.PayPalCapture;
            t.Amount = EnsurePositiveAmount(amount);
            t.PreviousTransactionNumber = holdTransaction.RefNum1;
            t.PreviousTransactionAuthCode = holdTransaction.RefNum2;
            OrderTransaction ot = new OrderTransaction(t);

            if (holdTransaction.Action != ActionType.PayPalHold)
            {
                ot.Success = false;
                ot.Messages = "Transaction must be PayPal hold type to process.";
                return svc.AddPaymentTransactionToOrder(o, ot, this.MTApp);
            }

            Payment.Method.PaypalExpress processor = new Payment.Method.PaypalExpress();
            if (processor != null)
            {
                processor.Capture(t, this.MTApp);
                ot = new OrderTransaction(t);
                ot.LinkedToTransaction = holdTransaction.IdAsString;
            }
            return svc.AddPaymentTransactionToOrder(o, ot, this.MTApp);
        }
        public bool PayPalExpressCharge(string infoTransactionId, decimal amount)
        {
            OrderTransaction t = PayPalExpressInfoFind(infoTransactionId);
            return PayPalExpressCharge(t, amount);
        }
        public bool PayPalExpressCharge(OrderTransaction infoTransaction, decimal amount)
        {
            if (infoTransaction == null) return false;

            Transaction t = o.GetEmptyTransaction();
            t.Action = ActionType.PayPalCharge;
            t.Amount = EnsurePositiveAmount(amount);
            t.PreviousTransactionNumber = infoTransaction.RefNum1;
            t.PreviousTransactionAuthCode = infoTransaction.RefNum2;
            OrderTransaction ot = new OrderTransaction(t);

            if (infoTransaction.Action != ActionType.PayPalExpressCheckoutInfo)
            {
                ot.Success = false;
                ot.Messages = "Transaction must be PayPal info type to process.";
                return svc.AddPaymentTransactionToOrder(o, ot, this.MTApp);
            }

            Payment.Method.PaypalExpress processor = new Payment.Method.PaypalExpress();
            if (processor != null)
            {
                processor.Charge(t, this.MTApp);
                ot = new OrderTransaction(t);
                ot.LinkedToTransaction = infoTransaction.IdAsString;
            }

            return svc.AddPaymentTransactionToOrder(o, ot, this.MTApp);
        }
        public bool PayPalExpressRefund(string previousTransaction, decimal amount)
        {
            OrderTransaction t = FindTransactionById(previousTransaction);
            return PayPalExpressRefund(t, amount);
        }
        public bool PayPalExpressRefund(OrderTransaction previousTransaction, decimal amount)
        {
            if (previousTransaction == null) return false;

            Transaction t = o.GetEmptyTransaction();
            t.Action = ActionType.PayPalRefund;
            t.Amount = EnsurePositiveAmount(amount);
            t.PreviousTransactionNumber = previousTransaction.RefNum1;
            t.PreviousTransactionAuthCode = previousTransaction.RefNum2;
            OrderTransaction ot = new OrderTransaction(t);

            if (previousTransaction.Action != ActionType.PayPalCapture
                && previousTransaction.Action != ActionType.PayPalCharge
                && previousTransaction.Action != ActionType.PayPalExpressCheckoutInfo)
            {
                ot.Success = false;
                ot.Messages = "Transaction must be PayPal capture or charge type to refund.";
                return svc.AddPaymentTransactionToOrder(o, ot, this.MTApp);
            }

            Payment.Method.PaypalExpress processor = new Payment.Method.PaypalExpress();
            if (processor != null)
            {
                processor.Refund(t, this.MTApp);
                ot = new OrderTransaction(t);
                ot.LinkedToTransaction = previousTransaction.IdAsString;
            }
            return svc.AddPaymentTransactionToOrder(o, ot, this.MTApp);
        }
        public bool PayPalExpressVoid(string previousTransaction, decimal amount)
        {
            OrderTransaction t = FindTransactionById(previousTransaction);
            return PayPalExpressVoid(t, amount);
        }
        public bool PayPalExpressVoid(OrderTransaction previousTransaction, decimal amount)
        {
            if (previousTransaction == null) return false;

            Transaction t = o.GetEmptyTransaction();
            t.Action = ActionType.PayPalVoid;
            t.Amount = EnsurePositiveAmount(amount);
            t.PreviousTransactionNumber = previousTransaction.RefNum1;
            t.PreviousTransactionAuthCode = previousTransaction.RefNum2;
            OrderTransaction ot = new OrderTransaction(t);

            if (!previousTransaction.IsVoidable)
            {
                ot.Success = false;
                ot.Messages = "Transaction can not be voided.";
                return svc.AddPaymentTransactionToOrder(o, ot, this.MTApp);
            }

            Payment.Method.PaypalExpress processor = new Payment.Method.PaypalExpress();  
            if (processor != null)
            {
                processor.Void(t, this.MTApp);
                ot = new OrderTransaction(t);
                ot.LinkedToTransaction = previousTransaction.IdAsString;

                // if the void went through, make sure we mark the previous transaction as voided
                if (t.Result.Succeeded)
                {
                    previousTransaction.Voided = true;
                    svc.Transactions.Update(previousTransaction);
                }
            }
            return svc.AddPaymentTransactionToOrder(o, ot, this.MTApp);
        }
        public bool PayPalExpressCompleteAllPayments()
        {
            bool result = true;

            foreach (Orders.OrderTransaction p in svc.Transactions.FindForOrder(o.bvin))
            {

                List<Orders.OrderTransaction> transactions = svc.Transactions.FindForOrder(o.bvin);

                if (p.Action == MerchantTribe.Payment.ActionType.PayPalExpressCheckoutInfo ||
                    p.Action == MerchantTribe.Payment.ActionType.PayPalHold)
                {
                    // if we already have an auth or charge on the card, skip
                    if (
                        p.HasSuccessfulLinkedAction(MerchantTribe.Payment.ActionType.PayPalCharge, transactions) ||
                        p.HasSuccessfulLinkedAction(MerchantTribe.Payment.ActionType.PayPalCapture, transactions) ||
                        p.HasSuccessfulLinkedAction(MerchantTribe.Payment.ActionType.PayPalHold, transactions) 
                        )
                    {
                        continue;
                    }

                    try
                    {
                        MerchantTribe.Payment.Transaction t = o.GetEmptyTransaction();
                        t.Card = p.CreditCard;
                        t.Amount = p.Amount;

                        Payment.Method.PaypalExpress processor = new Payment.Method.PaypalExpress();

                        if (p.Action == MerchantTribe.Payment.ActionType.PayPalHold)
                        {
                            t.Action = MerchantTribe.Payment.ActionType.PayPalCapture;
                            processor.Capture(t, this.MTApp);
                        }
                        else
                        {
                            t.Action = MerchantTribe.Payment.ActionType.PayPalCharge;
                            processor.Charge(t, this.MTApp);
                        }
                                                
                        Orders.OrderTransaction ot = new Orders.OrderTransaction(t);
                        ot.LinkedToTransaction = p.IdAsString;
                        svc.AddPaymentTransactionToOrder(o, ot, this.MTApp);

                        if (t.Result.Succeeded == false) result = false;

                    }
                    catch (Exception ex)
                    {
                        EventLog.LogEvent(ex);
                    }
                }

                Orders.OrderPaymentStatus previousPaymentStatus = o.PaymentStatus;
                svc.EvaluatePaymentStatus(o);
                BusinessRules.OrderTaskContext context = new BusinessRules.OrderTaskContext(this.MTApp);
                context.Order = o;
                context.UserId = o.UserID;
                context.Inputs.Add("bvsoftware", "PreviousPaymentStatus", previousPaymentStatus.ToString());
                BusinessRules.Workflow.RunByName(context, BusinessRules.WorkflowNames.PaymentChanged);
            }

            return result;
        }


        // Rewards Points
        public string RewardsPointsAvailableDescription()
        {
            string result = "0 - " + 0.ToString("c");

            if (o.UserID != string.Empty)
            {
                int points = pointsManager.FindAvailablePoints(o.UserID);
                result = points + " - " + pointsManager.DollarCreditForPoints(points).ToString("c");
            }

            return result;
        }
        public bool RewardsPointsAddInfo(decimal amount)
        {
            int points = pointsManager.PointsNeededForPurchaseAmount(EnsurePositiveAmount(amount));           
            Transaction t = o.GetEmptyTransaction();
            t.Amount = EnsurePositiveAmount(amount);
            t.Customer.UserId = o.UserID;
            t.RewardPoints = points;
            t.Action = ActionType.RewardPointsInfo;
            OrderTransaction ot = new OrderTransaction(t);
            ot.Success = true;
            return svc.AddPaymentTransactionToOrder(o, ot, this.MTApp);
        }
        private OrderTransaction FindOrCreateRewardsInfo(string infoTransactionId)
        {
            OrderTransaction t = null;

            if (infoTransactionId == string.Empty)
            {
                if (RewardsPointsInfoListAll().Count < 1)
                {
                    RewardsPointsAddInfo(0);
                }
                List<OrderTransaction> infos = RewardsPointsInfoListAll();
                t = infos[0];
            }
            else
            {
                t = RewardsPointsInfoFind(infoTransactionId);
            }

            return t;
        }
        public bool RewardsPointsHold(string infoTransactionId, decimal amount)
        {
            OrderTransaction t = FindOrCreateRewardsInfo(infoTransactionId);                                                           
            return RewardsPointsHold(t, amount);
        }
        public bool RewardsPointsHold(OrderTransaction infoTransaction, decimal amount)
        {
            if (infoTransaction == null) return false;

            Transaction t = o.GetEmptyTransaction();
            t.Action = ActionType.RewardPointsHold;
            int points = pointsManager.PointsNeededForPurchaseAmount(EnsurePositiveAmount(amount));
            t.RewardPoints = points;
            t.Customer.UserId = o.UserID;
            t.Amount = EnsurePositiveAmount(amount);
            OrderTransaction ot = new OrderTransaction(t);

            if (infoTransaction.Action != ActionType.RewardPointsInfo)
            {
                ot.Success = false;
                ot.Messages = "Transaction must be Rewards Points info type to process.";
                return svc.AddPaymentTransactionToOrder(o, ot, this.MTApp);
            }


            Payment.RewardPoints processor = new RewardPoints();
            processor.Settings = new RewardPointsSettings(this.MTApp.CurrentStore.Id)
            {
                PointsIssuedPerDollarSpent = pointsManager.PointsToIssueForSpend(1),
                PointsNeededForDollarCredit = pointsManager.PointsNeededForPurchaseAmount(1)
            };
                                                            
            if (processor != null)
            {
                processor.ProcessTransaction(t);
                ot = new OrderTransaction(t);
                ot.LinkedToTransaction = infoTransaction.IdAsString;
            }

            return svc.AddPaymentTransactionToOrder(o, ot, this.MTApp);
        }
        public bool RewardsPointsUnHold(string holdTransactionId, decimal amount)
        {
            OrderTransaction t = RewardsPointsHoldFind(holdTransactionId);
            return RewardsPointsUnHold(t, amount);
        }
        public bool RewardsPointsUnHold(OrderTransaction holdTransaction, decimal amount)
        {
            if (holdTransaction == null) return false;            

            Transaction t = o.GetEmptyTransaction();
            t.Action = ActionType.RewardPointsUnHold;
            int points = pointsManager.PointsNeededForPurchaseAmount(EnsurePositiveAmount(amount));
            t.RewardPoints = points;
            t.Customer.UserId = o.UserID;
            t.Amount = EnsurePositiveAmount(amount);
            OrderTransaction ot = new OrderTransaction(t);

            if (holdTransaction.Action != ActionType.RewardPointsHold)
            {
                ot.Success = false;
                ot.Messages = "Transaction must be Rewards Points Hold type to process.";
                return svc.AddPaymentTransactionToOrder(o, ot, this.MTApp);
            }
            
            Payment.RewardPoints processor = new RewardPoints();
            processor.Settings = new RewardPointsSettings(this.MTApp.CurrentStore.Id)
            {
                PointsIssuedPerDollarSpent = pointsManager.PointsToIssueForSpend(1),
                PointsNeededForDollarCredit = pointsManager.PointsNeededForPurchaseAmount(1)
            };

            if (processor != null)
            {
                processor.ProcessTransaction(t);
                ot = new OrderTransaction(t);
                ot.LinkedToTransaction = holdTransaction.IdAsString;

                if (ot.Success)
                {
                    holdTransaction.Voided = true;
                    svc.Transactions.Update(holdTransaction);
                }
            }

            return svc.AddPaymentTransactionToOrder(o, ot, this.MTApp);
        }
        public List<OrderTransaction> RewardsPointsInfoListAll()
        {
            return FindAllTransactionsOfType(ActionType.RewardPointsInfo);
        }
        public OrderTransaction RewardsPointsInfoFind(string id)
        {
            return FindSingleTransactionByTypeAndId(ActionType.RewardPointsInfo, id);
        }
        public List<OrderTransaction> RewardsPointsHoldListAll()
        {
            List<OrderTransaction> result = new List<OrderTransaction>();

            foreach (OrderTransaction t in svc.Transactions.FindForOrder(o.bvin))
            {
                if (t.Action == ActionType.RewardPointsHold)
                {
                    if (!t.Voided && t.Success)
                    {
                        result.Add(t);
                    }
                }
            }
            return result;
        }
        public OrderTransaction RewardsPointsHoldFind(string id)
        {
            foreach (OrderTransaction t in RewardsPointsHoldListAll())
            {
                if (t.IdAsString == id)
                {
                    return t;
                }
            }
            return null;
        }
        public List<OrderTransaction> RewardsPointsListAllRefundable()
        {
            List<OrderTransaction> result = new List<OrderTransaction>();

            foreach (OrderTransaction t in svc.Transactions.FindForOrder(o.bvin))
            {
                if (t.Action == ActionType.RewardPointsCapture ||
                    t.Action == ActionType.RewardPointsDecrease)
                {
                    if (!t.Voided)
                    {
                        if (t.Success)
                        {
                            result.Add(t);
                        }
                    }
                }
            }
            return result;
        }
        public bool RewardsPointsCapture(string holdTransactionId, decimal amount)
        {
            OrderTransaction t = RewardsPointsHoldFind(holdTransactionId);
            return RewardsPointsCapture(t, amount);
        }
        public bool RewardsPointsCapture(OrderTransaction holdTransaction, decimal amount)
        {
            if (holdTransaction == null) return false;

            Transaction t = o.GetEmptyTransaction();
            t.Action = ActionType.RewardPointsCapture;
            int points = pointsManager.PointsNeededForPurchaseAmount(EnsurePositiveAmount(amount));
            t.RewardPoints = points;
            t.Customer.UserId = o.UserID;
            t.Amount = EnsurePositiveAmount(amount);
            OrderTransaction ot = new OrderTransaction(t);

            if (holdTransaction.Action != ActionType.RewardPointsHold)
            {
                ot.Success = false;
                ot.Messages = "Transaction must be Rewards Points hold type to process.";
                return svc.AddPaymentTransactionToOrder(o, ot, this.MTApp);
            }

            Payment.RewardPoints processor = new RewardPoints();
            processor.Settings = new RewardPointsSettings(this.MTApp.CurrentStore.Id)
            {
                PointsIssuedPerDollarSpent = pointsManager.PointsToIssueForSpend(1),
                PointsNeededForDollarCredit = pointsManager.PointsNeededForPurchaseAmount(1)
            };

            if (processor != null)
            {
                processor.ProcessTransaction(t);
                ot = new OrderTransaction(t);
                ot.LinkedToTransaction = holdTransaction.IdAsString;
            }
            return svc.AddPaymentTransactionToOrder(o, ot, this.MTApp);
        }
        public bool RewardsPointsCharge(string infoTransactionId, decimal amount)
        {
            OrderTransaction t = FindOrCreateRewardsInfo(infoTransactionId);
            return RewardsPointsCharge(t, amount);
        }
        public bool RewardsPointsCharge(OrderTransaction infoTransaction, decimal amount)
        {
            if (infoTransaction == null) return false;

            Transaction t = o.GetEmptyTransaction();
            t.Action = ActionType.RewardPointsDecrease;
            t.Amount = EnsurePositiveAmount(amount);
            t.RewardPoints = pointsManager.PointsNeededForPurchaseAmount(EnsurePositiveAmount(amount));
            t.Customer.UserId = o.UserID;
            OrderTransaction ot = new OrderTransaction(t);

            if (infoTransaction.Action != ActionType.RewardPointsInfo)
            {
                ot.Success = false;
                ot.Messages = "Transaction must be Rewards Points info type to process.";
                return svc.AddPaymentTransactionToOrder(o, ot, this.MTApp);
            }

            Payment.RewardPoints processor = new RewardPoints();
            processor.Settings = new RewardPointsSettings(this.MTApp.CurrentStore.Id)
            {
                PointsIssuedPerDollarSpent = pointsManager.PointsToIssueForSpend(1),
                PointsNeededForDollarCredit = pointsManager.PointsNeededForPurchaseAmount(1)
            };
            if (processor != null)
            {
                processor.ProcessTransaction(t);
                ot = new OrderTransaction(t);
                ot.LinkedToTransaction = infoTransaction.IdAsString;
            }

            return svc.AddPaymentTransactionToOrder(o, ot, this.MTApp);
        }
        public bool RewardsPointsRefund(string previousTransaction, decimal amount)
        {
            OrderTransaction t = FindTransactionById(previousTransaction);
            return RewardsPointsRefund(t, amount);
        }
        public bool RewardsPointsRefund(OrderTransaction previousTransaction, decimal amount)
        {
            if (previousTransaction == null) return false;

            Transaction t = o.GetEmptyTransaction();
            t.Action = ActionType.RewardPointsIncrease;
            t.Amount = EnsurePositiveAmount(amount);
            t.RewardPoints = pointsManager.PointsNeededForPurchaseAmount(EnsurePositiveAmount(amount));
            t.Customer.UserId = o.UserID;
            OrderTransaction ot = new OrderTransaction(t);

            if (previousTransaction.Action != ActionType.RewardPointsCapture
                && previousTransaction.Action != ActionType.RewardPointsDecrease
                && previousTransaction.Action != ActionType.RewardPointsInfo)
            {
                ot.Success = false;
                ot.Messages = "Transaction must be Rewards Points capture or charge type to refund.";
                return svc.AddPaymentTransactionToOrder(o, ot, this.MTApp);
            }

            Payment.RewardPoints processor = new RewardPoints();
            processor.Settings = new RewardPointsSettings(this.MTApp.CurrentStore.Id)
            {
                PointsIssuedPerDollarSpent = pointsManager.PointsToIssueForSpend(1),
                PointsNeededForDollarCredit = pointsManager.PointsNeededForPurchaseAmount(1)
            };
            if (processor != null)
            {
                processor.ProcessTransaction(t);
                ot = new OrderTransaction(t);
                ot.LinkedToTransaction = previousTransaction.IdAsString;
            }
            return svc.AddPaymentTransactionToOrder(o, ot, this.MTApp);
        }
        public bool RewardsPointsAcceptAll()
        {
            bool result = true;

            Payment.RewardPoints processor = new RewardPoints();
            processor.Settings = new RewardPointsSettings(this.MTApp.CurrentStore.Id)
            {
                PointsIssuedPerDollarSpent = pointsManager.PointsToIssueForSpend(1),
                PointsNeededForDollarCredit = pointsManager.PointsNeededForPurchaseAmount(1)
            };

            foreach (Orders.OrderTransaction p in svc.Transactions.FindForOrder(o.bvin))
            {

                List<Orders.OrderTransaction> transactions = svc.Transactions.FindForOrder(o.bvin);

                if (p.Action == MerchantTribe.Payment.ActionType.RewardPointsInfo ||
                    p.Action == MerchantTribe.Payment.ActionType.RewardPointsHold)
                {
                    // if we already have an auth or charge on the card, skip
                    if (p.HasSuccessfulLinkedAction(MerchantTribe.Payment.ActionType.RewardPointsDecrease, transactions) ||
                        p.HasSuccessfulLinkedAction(MerchantTribe.Payment.ActionType.RewardPointsHold, transactions) ||
                        p.HasSuccessfulLinkedAction(MerchantTribe.Payment.ActionType.RewardPointsCapture, transactions)
                        )
                    {
                        continue;
                    }

                    try
                    {
                        MerchantTribe.Payment.Transaction t = o.GetEmptyTransaction();
                        t.Amount = p.Amount;
                        t.RewardPoints = pointsManager.PointsNeededForPurchaseAmount(p.Amount);
                        t.Customer.UserId = o.UserID;

                        if (p.Action == MerchantTribe.Payment.ActionType.RewardPointsHold)
                        {
                            t.Action = MerchantTribe.Payment.ActionType.RewardPointsCapture;
                        }
                        else
                        {
                            t.Action = MerchantTribe.Payment.ActionType.RewardPointsDecrease;
                        }

                        
                        processor.ProcessTransaction(t);

                        Orders.OrderTransaction ot = new Orders.OrderTransaction(t);
                        ot.LinkedToTransaction = p.IdAsString;
                        svc.AddPaymentTransactionToOrder(o, ot, this.MTApp);

                        if (t.Result.Succeeded == false) result = false;

                    }
                    catch (Exception ex)
                    {
                        EventLog.LogEvent(ex);
                    }
                }

                Orders.OrderPaymentStatus previousPaymentStatus = o.PaymentStatus;
                svc.EvaluatePaymentStatus(o);
                BusinessRules.OrderTaskContext context = new BusinessRules.OrderTaskContext(this.MTApp);
                context.Order = o;
                context.UserId = o.UserID;
                context.Inputs.Add("bvsoftware", "PreviousPaymentStatus", previousPaymentStatus.ToString());
                BusinessRules.Workflow.RunByName(context, BusinessRules.WorkflowNames.PaymentChanged);
            }

            return result;
        }
        //public bool RewardsPointsIssueNew(decimal amount)
        //{
        //    Transaction t = o.GetEmptyTransaction();
        //    t.Action = ActionType.RewardPointsIncrease;
        //    t.Amount = EnsurePositiveAmount(amount);
        //    t.RewardPoints = pointsManager.PointsNeededForPurchaseAmount(EnsurePositiveAmount(amount));
        //    OrderTransaction ot = new OrderTransaction(t);
            
        //    Payment.RewardPoints processor = new RewardPoints();
        //    processor.Settings = new RewardPointsSettings()
        //    {
        //        PointsIssuedPerDollarSpent = pointsManager.PointsToIssueForSpend(1),
        //        PointsNeededForDollarCredit = pointsManager.PointsNeededForPurchaseAmount(1)
        //    };
        //    if (processor != null)
        //    {
        //        processor.ProcessTransaction(t);
        //        ot = new OrderTransaction(t);
        //    }
        //    return o.AddPayment(ot);
        //}

    }
}
