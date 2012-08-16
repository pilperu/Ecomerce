using System;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using MerchantTribe.Commerce;
using MerchantTribe.Commerce.Orders;
using MerchantTribe.Commerce.Membership;
using MerchantTribe.Commerce.Utilities;
using MerchantTribe.Commerce.Shipping;
using MerchantTribe.Commerce.Payment;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Linq;

namespace MerchantTribeStore
{

    partial class BVAdmin_Orders_ReceivePayments : BaseAdminPage
    {

        private Order o = new MerchantTribe.Commerce.Orders.Order();

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.PageTitle = "Payments";
            this.CurrentTab = AdminTabType.Orders;
            ValidateCurrentUserHasPermission(SystemPermissions.OrdersEdit);
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.ReceivePaymentsControl1.TransactionEvent += this.TransactionHappened;
        }

        protected void TransactionHappened()
        {
            o = MTApp.OrderServices.Orders.FindForCurrentStore(o.bvin);
            this.OrderStatusDisplay1.LoadStatusForOrder(o);
            this.LoadTransactions();
            this.PopulateFromOrder(o);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (!Page.IsPostBack)
            {
                if (Request.QueryString["id"] != null)
                {
                    BvinField.Value = Request.QueryString["id"];

                }
                decimal val = 0m;
                PaymentAuthorizedField.Text = val.ToString("c");
                PaymentChargedField.Text = val.ToString("c");
                PaymentRefundedField.Text = val.ToString("c");
                PaymentDueField.Text = val.ToString("c");
            }

            o = MTApp.OrderServices.Orders.FindForCurrentStore(this.BvinField.Value);
            LoadTransactions();
            PopulateFromOrder(o);

            // Acumatica Warning
            if (MTApp.CurrentStore.Settings.Acumatica.IntegrationEnabled)
            {
                this.MessageBox1.ShowWarning(MerchantTribe.Commerce.Content.SiteTerms.GetTerm(MerchantTribe.Commerce.Content.SiteTermIds.AcumaticaWarning));
            }
        }


        private void LoadTransactions()
        {
            
            if (o == null) return;
            List<OrderTransaction> transactions = MTApp.OrderServices.Transactions.FindForOrder(o.bvin);
            if (transactions == null) return;
            if (transactions.Count < 1)
            {
                this.litTransactions.Text = "No Transactions Found";
                return;
            }

            StringBuilder sb = new StringBuilder();

             transactions = transactions.OrderByDescending(y => y.TimeStampUtc).ToList();

            if (transactions == null) return;

            TimeZoneInfo tz = MTApp.CurrentStore.Settings.TimeZone;
            foreach (OrderTransaction t in transactions)
            {
                RenderTransaction(t, sb, tz);
            }
            this.litTransactions.Text = sb.ToString();
        }

        private void PopulateFromOrder(Order o)
        {

            // Header
            this.OrderNumberField.Text = o.OrderNumber;

            // Payment
            OrderPaymentSummary paySummary = MTApp.OrderServices.PaymentSummary(o);
            //Me.lblPaymentSummary.Text = paySummary.PaymentsSummary
            this.PaymentAuthorizedField.Text = string.Format("{0:C}", paySummary.AmountAuthorized);
            this.PaymentChargedField.Text = string.Format("{0:C}", paySummary.AmountCharged);
            this.PaymentDueField.Text = string.Format("{0:C}", paySummary.AmountDue);
            this.PaymentRefundedField.Text = string.Format("{0:C}", paySummary.AmountRefunded);
        }

        private void RenderTransaction(OrderTransaction t, StringBuilder sb, TimeZoneInfo timezone)
        {
            sb.Append("<div class=\"controlarea1");
            if (t.Voided)
            {
                sb.Append(" transactionvoided");
            }
            else
            {
                if (t.Success)
                {
                    sb.Append(" transactionsuccess");
                }
                else
                {
                    sb.Append(" transactionfailed");
                }
            }
            sb.Append("\">");

            if (t.Voided)
            {
                sb.Append("VOIDED<br />");
            }
            sb.Append(t.Amount.ToString("c") + " - ");
            sb.Append(MerchantTribe.Payment.EnumHelper.ActionTypeToString(t.Action) + "<br />");
            sb.Append(TimeZoneInfo.ConvertTimeFromUtc(t.TimeStampUtc, timezone).ToString() + "<br />");
            if (t.Success)
            {
                sb.Append("OK<br />");
            }
            else
            {
                sb.Append("FAILED<br />");
            }
            if (t.Action == MerchantTribe.Payment.ActionType.PurchaseOrderInfo || t.Action == MerchantTribe.Payment.ActionType.PurchaseOrderAccepted)
            {
                sb.Append("PO # " + t.PurchaseOrderNumber + "<br />");
            }
            if (t.Action == MerchantTribe.Payment.ActionType.CheckReceived || t.Action == MerchantTribe.Payment.ActionType.CheckReturned)
            {
                sb.Append("Check # " + t.CheckNumber + "<br />");
            }
            if (t.Action == MerchantTribe.Payment.ActionType.CreditCardCapture ||
                t.Action == MerchantTribe.Payment.ActionType.CreditCardCharge ||
                t.Action == MerchantTribe.Payment.ActionType.CreditCardHold ||
                t.Action == MerchantTribe.Payment.ActionType.CreditCardInfo ||
                t.Action == MerchantTribe.Payment.ActionType.CreditCardRefund ||
                t.Action == MerchantTribe.Payment.ActionType.CreditCardVoid)
            {
                sb.Append(t.CreditCard.CardTypeName + " xxxx-xxxx-xxxx-" + t.CreditCard.CardNumberLast4Digits + "<br />");
                sb.Append("exp: " + t.CreditCard.ExpirationMonth + "/" + t.CreditCard.ExpirationYear + "<br />");
            }
            if (t.RefNum1 != string.Empty)
            {
                sb.Append("Ref#: " + t.RefNum1);
            }
            if (t.RefNum2 != string.Empty)
            {
                sb.Append("Ref2#: " + t.RefNum2);
            }
            sb.Append(t.Messages);
            sb.Append("</div>");
        }

    }
}