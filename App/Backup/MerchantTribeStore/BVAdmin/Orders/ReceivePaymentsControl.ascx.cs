using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MerchantTribe.Commerce;
using MerchantTribe.Commerce.Orders;
using MerchantTribe.Commerce.Payment;
using MerchantTribe.Payment;

namespace MerchantTribeStore
{

    public partial class BVAdmin_Orders_ReceivePaymentsControl : MerchantTribe.Commerce.Content.BVUserControl
    {

        private Order o = new MerchantTribe.Commerce.Orders.Order();
        private OrderPaymentManager payManager = null;

        public delegate void TransactionEventDelegate();
        public TransactionEventDelegate TransactionEvent;

        private string orderId = string.Empty;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            orderId = Request.QueryString["id"];
            o = MyPage.MTApp.OrderServices.Orders.FindForCurrentStore(orderId);
            payManager = new OrderPaymentManager(o, MyPage.MTApp);

            if (!Page.IsPostBack)
            {
                this.mvPayments.SetActiveView(this.viewCreditCards);
                LoadCreditCardLists();

                if (MyPage.MTApp.CurrentStore.PlanId < 2)
                {
                    //this.lnkCash.Visible = false;
                    this.lnkCheck.Visible = false;
                    this.lnkPO.Visible = false;
                }
            }


        }


        #region tab buttons

        protected void lnkCC_Click(object sender, EventArgs e)
        {
            this.mvPayments.SetActiveView(this.viewCreditCards);
            LoadCreditCardLists();
        }
        protected void lnkPO_Click(object sender, EventArgs e)
        {
            this.mvPayments.SetActiveView(this.viewPO);
            PopulatePOList();
        }
        private void PopulatePOList()
        {
            List<OrderTransaction> pos = payManager.PurchaseOrderInfoListAllNonAccepted();
            this.lstPO.Items.Clear();
            if (pos.Count < 1) this.lstPO.Items.Add(new ListItem("No Purchase Orders Found.", ""));

            foreach (OrderTransaction t in pos)
            {
                this.lstPO.Items.Add(new ListItem(t.PurchaseOrderNumber + " - " + t.Amount.ToString("c"), t.PurchaseOrderNumber));
            }
        }
        protected void lnkCompanyAccount_Click(object sender, EventArgs e)
        {
            this.mvPayments.SetActiveView(this.viewCompanyAccount);
            PopulateCompanyAccountList();
        }
        private void PopulateCompanyAccountList()
        {
            List<OrderTransaction> acts = payManager.CompanyAccountInfoListAllNonAccepted();
            this.lstCompanyAccount.Items.Clear();
            if (acts.Count < 1) this.lstCompanyAccount.Items.Add(new ListItem("No Company Accounts Found.", ""));
            foreach (OrderTransaction t in acts)
            {
                this.lstCompanyAccount.Items.Add(new ListItem(t.CompanyAccountNumber + " - " + t.Amount.ToString("c"), t.CompanyAccountNumber));
            }
        }
        protected void lnkCash_Click(object sender, EventArgs e)
        {
            this.mvPayments.SetActiveView(this.viewCash);
        }
        protected void lnkCheck_Click(object sender, EventArgs e)
        {
            this.mvPayments.SetActiveView(this.viewCheck);
        }
        protected void lnkPayPal_Click(object sender, EventArgs e)
        {
            this.mvPayments.SetActiveView(this.viewPayPal);
            LoadPayPalLists();
        }
        protected void lnkPoints_Click(object sender, EventArgs e)
        {
            this.mvPayments.SetActiveView(this.viewPoints);
            LoadPointsLists();
        }

        #endregion

        private decimal ParseMoney(string input)
        {
            decimal val = 0;
            if (decimal.TryParse(input, System.Globalization.NumberStyles.Currency,
                System.Threading.Thread.CurrentThread.CurrentUICulture, out val))
            {
                return val;
            }
            else
            {
                return 0;
            }
        }

        private void ShowTransaction(bool result)
        {
            if (result)
            {
                this.MessageBox1.ShowInformation("&laquo; Transaction Processed at " + DateTime.Now.ToString());
            }
            else
            {
                this.MessageBox1.ShowWarning("Could not record transaction. See Administrator!");
            }
            TransactionEvent();
        }

        // Cash
        protected void btnCashRefund_Click(object sender, EventArgs e)
        {
            this.MessageBox1.ClearMessage();
            decimal amount = ParseMoney(this.CashAmount.Text);
            ShowTransaction(payManager.CashRefund(amount));
        }
        protected void btnCashReceive_Click(object sender, EventArgs e)
        {
            this.MessageBox1.ClearMessage();
            decimal amount = ParseMoney(this.CashAmount.Text);
            ShowTransaction(payManager.CashReceive(amount));
        }

        // Check
        protected void lnkCheckReturn_Click(object sender, EventArgs e)
        {
            this.MessageBox1.ClearMessage();
            decimal amount = ParseMoney(this.CheckAmountField.Text);
            string checkNumber = this.CheckNumberField.Text.Trim();
            ShowTransaction(payManager.CheckReturn(amount, checkNumber));
        }
        protected void lnkCheckReceive_Click(object sender, EventArgs e)
        {
            this.MessageBox1.ClearMessage();
            decimal amount = ParseMoney(this.CheckAmountField.Text);
            string checkNumber = this.CheckNumberField.Text.Trim();
            ShowTransaction(payManager.CheckReceive(amount, checkNumber));
        }

        // Purchase Order
        protected void lnkPOAdd_Click(object sender, EventArgs e)
        {
            this.MessageBox1.ClearMessage();
            decimal amount = ParseMoney(this.PONewAmount.Text);
            string poNumber = this.PONewNumber.Text.Trim();
            ShowTransaction(payManager.PurchaseOrderAddInfo(poNumber, amount));
            PopulatePOList();
        }
        protected void lnkPOAccept_Click(object sender, EventArgs e)
        {
            this.MessageBox1.ClearMessage();
            string poNumber = this.lstPO.SelectedItem.Value;
            ShowTransaction(payManager.PurchaseOrderAccept(poNumber));
            PopulatePOList();
        }

        // Company Account
        protected void lnkCompanyAccountAdd_Click(object sender, EventArgs e)
        {
            this.MessageBox1.ClearMessage();
            decimal amount = ParseMoney(this.CompanyAccountNewAmount.Text);
            string accountNumber = this.CompanyAccountNewNumber.Text.Trim();
            ShowTransaction(payManager.CompanyAccountAddInfo(accountNumber, amount));
            PopulateCompanyAccountList();
        }

        protected void lnkCompanyAccountAccept_Click(object sender, EventArgs e)
        {
            this.MessageBox1.ClearMessage();
            string accountNumber = this.lstCompanyAccount.SelectedItem.Value;
            ShowTransaction(payManager.CompanyAccountAccept(accountNumber));
            PopulateCompanyAccountList();
        }

        // Credit Cards

        private void LoadCreditCardLists()
        {

            // List Auths for Collection
            List<OrderTransaction> auths = payManager.CreditCardHoldListAll();
            this.lstCreditCardAuths.Items.Clear();
            if (auths.Count < 1)
            {
                this.lstCreditCardAuths.Items.Add(new ListItem("No Pending Holds", ""));
                this.lnkCreditCardCaptureAuth.Enabled = false;
                this.lnkCreditCardVoidAuth.Enabled = false;
            }
            else
            {
                foreach (OrderTransaction t in auths)
                {
                    this.lstCreditCardAuths.Items.Add(new ListItem(t.CreditCard.CardTypeName + "-" + t.CreditCard.CardNumberLast4Digits + " - " + t.Amount.ToString("c"), t.IdAsString));
                }
                this.lnkCreditCardCaptureAuth.Enabled = true;
                this.lnkCreditCardVoidAuth.Enabled = true;
            }


            // List charges for refunds
            List<OrderTransaction> charges = payManager.CreditCardChargeListAllRefundable();
            this.lstCreditCardCharges.Items.Clear();
            if (charges.Count < 1)
            {
                this.lstCreditCardCharges.Items.Add(new ListItem("No Charges to Refund", ""));
                this.lnkCreditCardRefund.Enabled = false;
            }
            else
            {
                foreach (OrderTransaction t in charges)
                {
                    this.lstCreditCardCharges.Items.Add(new ListItem(t.CreditCard.CardTypeName + "-" + t.CreditCard.CardNumberLast4Digits + " - " + t.Amount.ToString("c"), t.IdAsString));
                }
                this.lnkCreditCardRefund.Enabled = true;
            }



            // Load Cards for Charges and Auths
            List<OrderTransaction> cards = payManager.CreditCardInfoListAll();
            this.lstCreditCards.Items.Clear();
            if (cards.Count < 1)
            {
                this.lstCreditCards.Items.Add(new ListItem("No Saved Cards", ""));
                this.lnkCreditCardCharge.Enabled = false;
            }
            else
            {
                foreach (OrderTransaction t in cards)
                {
                    this.lstCreditCards.Items.Add(new ListItem(t.CreditCard.CardTypeName + "-"
                                                               + t.CreditCard.CardNumberLast4Digits + " "
                                                               + t.CreditCard.ExpirationMonth + "/"
                                                               + t.CreditCard.ExpirationYearTwoDigits, t.IdAsString));
                }
                this.lnkCreditCardCharge.Enabled = true;
            }


        }
        protected void lnkCreditCardAddInfo_Click(object sender, EventArgs e)
        {
            this.MessageBox1.ClearMessage();
            CardData card = this.CreditCardInput1.GetCardData();
            ShowTransaction(payManager.CreditCardAddInfo(card, 0));
            LoadCreditCardLists();
        }
        protected void lnkCreditCardVoidAuth_Click(object sender, EventArgs e)
        {
            this.MessageBox1.ClearMessage();
            string transactionId = this.lstCreditCardAuths.SelectedItem.Value;
            decimal amount = ParseMoney(this.CreditCardAuthAmount.Text);
            ShowTransaction(payManager.CreditCardVoid(transactionId, amount));
            LoadCreditCardLists();
        }
        protected void lnkCreditCardCaptureAuth_Click(object sender, EventArgs e)
        {
            this.MessageBox1.ClearMessage();
            string transactionId = this.lstCreditCardAuths.SelectedItem.Value;
            decimal amount = ParseMoney(this.CreditCardAuthAmount.Text);
            if (this.CreditCardAuthAmount.Text.Trim() == string.Empty)
            {
                OrderTransaction authTrans = payManager.FindTransactionById(transactionId);
                amount = authTrans.Amount;
            }
            ShowTransaction(payManager.CreditCardCapture(transactionId, amount));
            LoadCreditCardLists();
        }
        protected void lnkCreditCardRefund_Click(object sender, EventArgs e)
        {
            this.MessageBox1.ClearMessage();
            string transactionId = this.lstCreditCardCharges.SelectedItem.Value;
            decimal amount = ParseMoney(this.CreditCardRefundAmount.Text);
            if (this.CreditCardRefundAmount.Text.Trim() == string.Empty)
            {
                OrderTransaction refTrans = payManager.FindTransactionById(transactionId);
                amount = refTrans.Amount;
            }
            ShowTransaction(payManager.CreditCardRefund(transactionId, amount));
            LoadCreditCardLists();
        }
        protected void lnkCreditCardNewAuth_Click(object sender, EventArgs e)
        {
            this.MessageBox1.ClearMessage();
            string cardId = this.lstCreditCards.SelectedItem.Value;
            decimal amount = ParseMoney(this.CreditCardChargeAmount.Text);
            ShowTransaction(payManager.CreditCardHold(cardId, amount));
            LoadCreditCardLists();
        }
        protected void lnkCreditCardCharge_Click(object sender, EventArgs e)
        {
            this.MessageBox1.ClearMessage();
            string cardId = this.lstCreditCards.SelectedItem.Value;
            decimal amount = ParseMoney(this.CreditCardChargeAmount.Text);
            ShowTransaction(payManager.CreditCardCharge(cardId, amount));
            LoadCreditCardLists();
        }

        // PayPal
        private void LoadPayPalLists()
        {

            // List Auths for Collection
            List<OrderTransaction> paypalAuths = payManager.PayPalExpressHoldListAll();
            this.lstPayPalHold.Items.Clear();
            if (paypalAuths.Count < 1)
            {
                this.lstPayPalHold.Items.Add(new ListItem("No Pending Holds.", ""));
                this.lnkPayPalCaptureHold.Enabled = false;
                this.lnkPayPalVoidHold.Enabled = false;
            }
            else
            {
                foreach (OrderTransaction t in paypalAuths)
                {
                    this.lstPayPalHold.Items.Add(new ListItem(t.Amount.ToString("c"), t.IdAsString));
                }
                this.lnkPayPalCaptureHold.Enabled = true;
                this.lnkPayPalVoidHold.Enabled = true;
            }


            // List charges for refunds
            List<OrderTransaction> charges = payManager.PayPalExpressListAllRefundable();
            this.lstPayPalRefund.Items.Clear();
            if (charges.Count < 1)
            {
                this.lstPayPalRefund.Items.Add(new ListItem("No Charges to Refund", ""));
                this.lnkPayPalRefund.Enabled = false;
            }
            else
            {
                foreach (OrderTransaction t in charges)
                {
                    this.lstPayPalRefund.Items.Add(new ListItem(t.Amount.ToString("c"), t.IdAsString));
                }
                this.lnkPayPalRefund.Enabled = true;
            }

        }

        protected void lnkPayPalVoidHold_Click(object sender, EventArgs e)
        {
            this.MessageBox1.ClearMessage();
            string transactionId = this.lstPayPalHold.SelectedItem.Value;
            decimal amount = ParseMoney(this.PayPalHoldAmount.Text);
            ShowTransaction(payManager.PayPalExpressVoid(transactionId, amount));
            LoadPayPalLists();
        }
        protected void lnkPayPalCaptureHold_Click(object sender, EventArgs e)
        {
            this.MessageBox1.ClearMessage();
            string transactionId = this.lstPayPalHold.SelectedItem.Value;
            decimal amount = ParseMoney(this.PayPalHoldAmount.Text);
            if (this.PayPalHoldAmount.Text.Trim() == string.Empty)
            {
                OrderTransaction authTrans = payManager.FindTransactionById(transactionId);
                amount = authTrans.Amount;
            }
            ShowTransaction(payManager.PayPalExpressCapture(transactionId, amount));
            LoadPayPalLists();
        }
        protected void lnkPayPalRefund_Click(object sender, EventArgs e)
        {
            this.MessageBox1.ClearMessage();
            string transactionId = this.lstPayPalRefund.SelectedItem.Value;
            decimal amount = ParseMoney(this.PayPalRefundAmount.Text);
            if (this.PayPalRefundAmount.Text.Trim() == string.Empty)
            {
                OrderTransaction refTrans = payManager.FindTransactionById(transactionId);
                amount = refTrans.Amount;
            }
            ShowTransaction(payManager.PayPalExpressRefund(transactionId, amount));
            LoadPayPalLists();
        }


        // Reward Points
        private void LoadPointsLists()
        {

            // List Auths for Collection
            List<OrderTransaction> auths = payManager.RewardsPointsHoldListAll();
            this.lstPointsHeld.Items.Clear();
            if (auths.Count < 1)
            {
                this.lstPointsHeld.Items.Add(new ListItem("No Pending Holds", ""));
                this.lnkPointsCaptureAuth.Enabled = false;
                this.lnkPointsVoidAuth.Enabled = false;
            }
            else
            {
                foreach (OrderTransaction t in auths)
                {
                    this.lstPointsHeld.Items.Add(new ListItem(t.Amount.ToString("c"), t.IdAsString));
                }
                this.lnkPointsCaptureAuth.Enabled = true;
                this.lnkPointsVoidAuth.Enabled = true;
            }


            // List charges for refunds
            List<OrderTransaction> charges = payManager.RewardsPointsListAllRefundable();
            this.lstPointsRefundable.Items.Clear();
            if (charges.Count < 1)
            {
                this.lstPointsRefundable.Items.Add(new ListItem("No Charges to Refund", ""));
                this.lnkPointsRefund.Enabled = false;
            }
            else
            {
                foreach (OrderTransaction t in charges)
                {
                    this.lstPointsRefundable.Items.Add(new ListItem(t.Amount.ToString("c"), t.IdAsString));
                }
                this.lnkPointsRefund.Enabled = true;
            }

            this.lblPointsAvailable.Text = payManager.RewardsPointsAvailableDescription();
                       
        }
        protected void lnkPointsVoidAuth_Click(object sender, EventArgs e)
        {
            this.MessageBox1.ClearMessage();
            string transactionId = this.lstPointsHeld.SelectedItem.Value;
            decimal amount = ParseMoney(this.PointsHeldAmount.Text);
            if (this.PointsHeldAmount.Text.Trim() == string.Empty)
            {
                OrderTransaction authTrans = payManager.FindTransactionById(transactionId);
                amount = authTrans.Amount;
            }
            ShowTransaction(payManager.RewardsPointsUnHold(transactionId, amount));
            LoadPointsLists();
        }
        protected void lnkPointsCaptureAuth_Click(object sender, EventArgs e)
        {
            this.MessageBox1.ClearMessage();
            string transactionId = this.lstPointsHeld.SelectedItem.Value;
            decimal amount = ParseMoney(this.PointsHeldAmount.Text);
            if (this.PointsHeldAmount.Text.Trim() == string.Empty)
            {
                OrderTransaction authTrans = payManager.FindTransactionById(transactionId);
                amount = authTrans.Amount;
            }
            ShowTransaction(payManager.RewardsPointsCapture(transactionId, amount));
            LoadPointsLists();
        }
        protected void lnkPointsNewAuth_Click(object sender, EventArgs e)
        {
            this.MessageBox1.ClearMessage();
            decimal amount = ParseMoney(this.PointsNewAmountField.Text);
            ShowTransaction(payManager.RewardsPointsHold(string.Empty, amount));
            LoadPointsLists();
        }
        protected void lnkPointsNewCharge_Click(object sender, EventArgs e)
        {
            this.MessageBox1.ClearMessage();
            decimal amount = ParseMoney(this.PointsNewAmountField.Text);
            ShowTransaction(payManager.RewardsPointsCharge(string.Empty, amount));
            LoadPointsLists();
        }
        protected void lnkPointsRefund_Click(object sender, EventArgs e)
        {
            this.MessageBox1.ClearMessage();
            string transactionId = this.lstPointsRefundable.SelectedItem.Value;
            decimal amount = ParseMoney(this.PointsRefundAmount.Text);
            if (this.PointsRefundAmount.Text.Trim() == string.Empty)
            {
                OrderTransaction refTrans = payManager.FindTransactionById(transactionId);
                amount = refTrans.Amount;
            }
            ShowTransaction(payManager.RewardsPointsRefund(transactionId, amount));
            LoadPointsLists();
        }

        
      

     
    }
}