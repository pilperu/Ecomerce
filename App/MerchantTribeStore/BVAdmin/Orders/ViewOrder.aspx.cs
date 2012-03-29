using System;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;
using MerchantTribe.Commerce;
using MerchantTribe.Commerce.Orders;
using MerchantTribe.Commerce.Catalog;
using MerchantTribe.Commerce.Membership;
using MerchantTribe.Commerce.Utilities;
using MerchantTribe.Commerce.Shipping;
using MerchantTribe.Commerce.Payment;
using MerchantTribe.Commerce.Content;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Linq;

namespace MerchantTribeStore
{

    partial class BVAdmin_Orders_ViewOrder : BaseAdminPage
    {

        protected override void OnInit(System.EventArgs e)
        {
            base.OnInit(e);

            decimal val = 0m;
            PaymentAuthorizedField.Text = val.ToString("c");
            PaymentChargedField.Text = val.ToString("c");
            PaymentRefundedField.Text = val.ToString("c");
            PaymentDueField.Text = val.ToString("c");
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (!Page.IsPostBack)
            {
                LoadTemplates();
                if (Request.QueryString["id"] != null)
                {
                    BvinField.Value = Request.QueryString["id"];
                }
                else
                {
                    // Show Error
                }
                LoadOrder();

                // Acumatica Warning
                if (MTApp.CurrentStore.Settings.Acumatica.IntegrationEnabled)
                {
                    this.MessageBox1.ShowWarning(MerchantTribe.Commerce.Content.SiteTerms.GetTerm(MerchantTribe.Commerce.Content.SiteTermIds.AcumaticaWarning));
                }
            }
            //ViewUtilities.DisplayKitInLineItem(this.Page, ItemsGridView, false);
        }

        private void LoadTemplates()
        {
            this.lstEmailTemplate.Items.Clear();
            List<HtmlTemplate> templates = MTApp.ContentServices.GetAllTemplatesForStoreOrDefaults();
            if (templates != null)
            {
                foreach (HtmlTemplate t in templates)
                {
                    System.Web.UI.WebControls.ListItem li = new System.Web.UI.WebControls.ListItem();
                    li.Text = t.DisplayName;
                    li.Value = t.Id.ToString();
                    if (t.TemplateType == HtmlTemplateType.OrderUpdated)
                    {
                        this.lstEmailTemplate.ClearSelection();
                        li.Selected = true;
                    }
                    this.lstEmailTemplate.Items.Add(li);
                }
            }
        }

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.PageTitle = "View Order";
            this.CurrentTab = AdminTabType.Orders;
            ValidateCurrentUserHasPermission(SystemPermissions.OrdersView);
        }

        private void LoadOrder()
        {
            string bvin = this.BvinField.Value.ToString();
            Order o = MTApp.OrderServices.Orders.FindForCurrentStore(bvin);
            if (o != null)
            {
                if (o.bvin != string.Empty)
                {
                    PopulateFromOrder(o);
                }
            }            
        }

        private void PopulateFromOrder(Order o)
        {

            // Header
            this.OrderNumberField.Text = o.OrderNumber;
            this.TimeOfOrderField.Text = TimeZoneInfo.ConvertTimeFromUtc(o.TimeOfOrderUtc, MTApp.CurrentStore.Settings.TimeZone).ToString();

            // Fraud Score Display            
            if (o.FraudScore < 0) this.lblFraudScore.Text = "No Fraud Score Data";
            if (o.FraudScore >= 0 && o.FraudScore < 3) this.lblFraudScore.Text = o.FraudScore.ToString() + "<span class=\"fraud-low\"><b>low risk</b></span>";
            if (o.FraudScore >= 3 && o.FraudScore <= 5) this.lblFraudScore.Text = "<span class=\"fraud-medium\"><b>medium risk</b></span>";
            if (o.FraudScore > 5) this.lblFraudScore.Text = "<span class=\"fraud-high\"><b>high risk</b></span>";

            // Billing
            this.BillingAddressField.Text = o.BillingAddress.ToHtmlString();

            //Email
            this.EmailAddressField.Text = MerchantTribe.Commerce.Utilities.MailServices.MailToLink(o.UserEmail, "Order " + o.OrderNumber, o.BillingAddress.FirstName + ",");

            // Shipping (hide if the same as billing address)
            this.pnlShipTo.Visible = true;
            this.ShippingAddressField.Text = o.ShippingAddress.ToHtmlString();


            // Payment
            OrderPaymentSummary paySummary = MTApp.OrderServices.PaymentSummary(o);
            this.lblPaymentSummary.Text = paySummary.PaymentsSummary;
            this.PaymentAuthorizedField.Text = string.Format("{0:C}", paySummary.AmountAuthorized);
            this.PaymentChargedField.Text = string.Format("{0:C}", paySummary.AmountCharged);
            this.PaymentDueField.Text = string.Format("{0:C}", paySummary.AmountDue);
            this.PaymentRefundedField.Text = string.Format("{0:C}", paySummary.AmountRefunded);

            //Items
            this.ItemsGridView.DataSource = o.Items;
            this.ItemsGridView.DataBind();

            // Instructions
            if (o.Instructions.Trim().Length > 0)
            {
                this.pnlInstructions.Visible = true;
                this.InstructionsField.Text = o.Instructions.Replace("\r\n", "<br />").Replace("\r", "<br />").Replace("\n", "<br />");
            }

            // Totals
            this.litTotals.Text = o.TotalsAsTable();            

            // Coupons
            this.CouponField.Text = string.Empty;
            for (int i = 0; i <= o.Coupons.Count - 1; i++)
            {
                this.CouponField.Text += o.Coupons[i].CouponCode.Trim().ToUpper() + "<br />";
            }

            // Notes
            Collection<OrderNote> publicNotes = new Collection<OrderNote>();
            Collection<OrderNote> privateNotes = new Collection<OrderNote>();
            for (int i = 0; i <= o.Notes.Count - 1; i++)
            {
                if (o.Notes[i].IsPublic)
                {
                    publicNotes.Add(o.Notes[i]);
                }
                else
                {
                    privateNotes.Add(o.Notes[i]);
                }
            }
            this.PublicNotesField.DataSource = publicNotes;
            this.PublicNotesField.DataBind();
            this.PrivateNotesField.DataSource = privateNotes;
            this.PrivateNotesField.DataBind();

        }

        protected void btnOkay_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            Response.Redirect("default.aspx");
        }

        protected void ItemsGridView_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LineItem lineItem = (LineItem)e.Row.DataItem;
                if (lineItem != null)
                {

                    Label SKUField = (Label)e.Row.FindControl("SKUField");
                    if (SKUField != null)
                    {
                        SKUField.Text = lineItem.ProductSku;
                    }

                    Label description = (Label)e.Row.FindControl("DescriptionField");
                    if (description != null)
                    {
                        description.Text = lineItem.ProductName;
                        description.Text += "<br />" + lineItem.ProductShortDescription;
                    }

                    Label ShippingStatusField = (Label)e.Row.FindControl("ShippingStatusField");
                    if (ShippingStatusField != null)
                    {
                        ShippingStatusField.Text = MerchantTribe.Commerce.Utilities.EnumToString.OrderShippingStatus(lineItem.ShippingStatus);
                    }

                    if (lineItem.LineTotal != lineItem.LineTotalWithoutDiscounts)
                    {
                        Label LineTotalWithoutDiscounts = (Label)e.Row.FindControl("LineTotalWithoutDiscounts");
                        if (LineTotalWithoutDiscounts != null)
                        {
                            LineTotalWithoutDiscounts.Visible = true;
                            LineTotalWithoutDiscounts.Text = lineItem.LineTotalWithoutDiscounts.ToString("c");
                        }
                        Literal litDiscounts = (Literal)e.Row.FindControl("litDiscounts");
                        if (litDiscounts != null)
                        {
                            litDiscounts.Text = "<div class=\"discounts\">" + lineItem.DiscountDetailsAsHtml() + "</div>";
                        }                        
                    }

                    //Literal lblGiftWrap = (Literal)e.Row.FindControl("lblGiftWrap");
                    //Literal lblGiftWrapQty = (Literal)e.Row.FindControl("lblGiftWrapQty");
                    //Literal lblGiftWrapPrice = (Literal)e.Row.FindControl("lblGiftWrapPrice");
                    //if (lineItem.AssociatedProduct != null) 
                    //{
                    //    if (lineItem.AssociatedProduct.GiftWrapAllowed == false) {
                    //        if (lblGiftWrap != null) {
                    //            lblGiftWrap.Visible = false;
                    //        }
                    //    }
                    //    else {
                    //        if (lblGiftWrapQty != null) {
                    //            lblGiftWrapQty.Text = string.Format("{0:#}", lineItem.GiftWrapCount);
                    //        }
                    //        if (lblGiftWrapPrice != null) {
                    //            lblGiftWrapPrice.Text = string.Format("{0:c}", lineItem.AssociatedProduct.GiftWrapPrice);
                    //        }

                    //        //build gift wrap details
                    //        StringBuilder details = new StringBuilder();
                    //        foreach (GiftWrapDetails item in lineItem.GiftWrapDetails) {
                    //            if (item.GiftWrapEnabled == true) {
                    //                details.Append("To: " + item.ToField + "<br/>");
                    //                details.Append("From: " + item.FromField + "<br/>");
                    //                details.Append("Message: " + item.MessageField + "<br/>");
                    //                details.Append("<br/>");
                    //            }
                    //        }

                    //        Literal lblGiftWrapDetails = (Literal)e.Row.FindControl("lblGiftWrapDetails");
                    //        if (lblGiftWrapDetails != null) {
                    //            lblGiftWrapDetails.Text = details.ToString();
                    //        }
                    //    }
                    //}
                }
            }
        }

        protected void btnNewPublicNote_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            this.MessageBox1.ClearMessage();
            if (this.NewPublicNoteField.Text.Trim().Length > 0)
            {
                AddNote(this.NewPublicNoteField.Text.Trim(), true);
            }
            this.NewPublicNoteField.Text = string.Empty;
        }

        private void AddNote(string message, bool isPublic)
        {
            Order o = MTApp.OrderServices.Orders.FindForCurrentStore(this.BvinField.Value);
            OrderNote n = new OrderNote();
            n.OrderID = this.BvinField.Value;
            n.IsPublic = isPublic;
            n.Note = message;
            o.Notes.Add(n);
            MTApp.OrderServices.Orders.Update(o);
            LoadOrder();
        }

        protected void btnNewPrivateNote_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            this.MessageBox1.ClearMessage();
            if (this.NewPrivateNoteField.Text.Trim().Length > 0)
            {
                AddNote(this.NewPrivateNoteField.Text.Trim(), false);
            }
            this.NewPrivateNoteField.Text = string.Empty;
        }

        protected void PublicNotesField_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
        {
            this.MessageBox1.ClearMessage();
            long Id = (long)PublicNotesField.DataKeys[e.RowIndex].Value;
            Order o = MTApp.OrderServices.Orders.FindForCurrentStore(this.BvinField.Value);
            var n = o.Notes.Where(y => y.Id == Id).SingleOrDefault();
            if (n != null)
            {
                o.Notes.Remove(n);
                MTApp.OrderServices.Orders.Update(o);
            }            
            LoadOrder();
        }

        protected void PrivateNotesField_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
        {            
            this.MessageBox1.ClearMessage();
            long Id = (long)PrivateNotesField.DataKeys[e.RowIndex].Value;
            Order o = MTApp.OrderServices.Orders.FindForCurrentStore(this.BvinField.Value);
            var n = o.Notes.Where(y => y.Id == Id).SingleOrDefault();
            if (n != null)
            {
                o.Notes.Remove(n);
                MTApp.OrderServices.Orders.Update(o);
            }
            LoadOrder();
        }

        protected void btnRMA_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            //this.MessageBox1.ClearMessage();
            //Order o = Order.FindByBvin(Request.QueryString["id"]);

            //RMA rma = new RMA();
            //rma.OrderBvin = o.Bvin;
            //foreach (GridViewRow row in ItemsGridView.Rows) {
            //    object obj = row.FindControl("SelectedCheckBox");
            //    if (obj != null) {
            //        if (((CheckBox)obj).Checked) {
            //            LineItem li = LineItem.FindByBvin((string)ItemsGridView.DataKeys[row.RowIndex].Value);
            //            RMAItem rmaItem = new RMAItem();
            //            rmaItem.RMABvin = rma.Bvin;
            //            rmaItem.LineItemBvin = li.Bvin;
            //            rma.Items.Add(rmaItem);
            //        }
            //    }
            //}

            //if (RMA.Insert(rma)) {
            //    Response.Redirect("~/RMA.aspx?orderId=" + HttpUtility.UrlEncode(o.Bvin) + "&rmaId=" + HttpUtility.UrlEncode(rma.Bvin));
            //}
        }

        protected void btnSendStatusEmail_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            this.MessageBox1.ClearMessage();
            Order o = MTApp.OrderServices.Orders.FindForCurrentStore(this.BvinField.Value);
            if (o != null)
            {
                if (o.bvin != string.Empty)
                {
                    long templateId = 0;
                    long.TryParse(this.lstEmailTemplate.SelectedValue, out templateId);
                    HtmlTemplate t = MTApp.ContentServices.HtmlTemplates.Find(templateId);

                    if (t == null) return;

                    string toEmail = o.UserEmail;

                    if (t.TemplateType == HtmlTemplateType.NewOrderForAdmin ||
                        t.TemplateType == HtmlTemplateType.DropShippingNotice ||
                        t.TemplateType == HtmlTemplateType.ContactFormToAdmin)
                    {
                        toEmail = MTApp.CurrentStore.Settings.MailServer.EmailForNewOrder;
                    }

                        
                        if (toEmail.Trim().Length > 0)
                        {
                            System.Net.Mail.MailMessage m = new System.Net.Mail.MailMessage();
                            t = t.ReplaceTagsInTemplate(MTApp, o, o.ItemsAsReplaceable());
                            m = t.ConvertToMailMessage(toEmail);
                            if (m != null)
                            {
                                    if (MerchantTribe.Commerce.Utilities.MailServices.SendMail(m, MTApp.CurrentStore))
                                    {
                                        this.MessageBox1.ShowOk("Email Sent!");
                                    }
                                    else
                                    {
                                        this.MessageBox1.ShowError("Message Send Failed.");
                                    }
                            }
                            else
                            {
                                this.MessageBox1.ShowError("Message was sent successfully.");
                            }
                        }                    
                }
            }
            LoadOrder();
        }
        protected void btnDelete_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            bool success = false;

            Order o = MTApp.OrderServices.Orders.FindForCurrentStore(Request.QueryString["id"]);
            switch (o.ShippingStatus)
            {
                case OrderShippingStatus.FullyShipped:
                    success = MTApp.OrderServices.Orders.Delete(o.bvin);
                    break;
                case OrderShippingStatus.NonShipping:
                    success = MTApp.OrderServices.Orders.Delete(o.bvin);
                    break;
                case OrderShippingStatus.PartiallyShipped:
                    this.MessageBox1.ShowWarning("Partially shipped orders can't be deleted. Either unship or ship all items before deleting.");
                    break;
                case OrderShippingStatus.Unknown:
                    success = MTApp.OrderServices.OrdersDeleteWithInventoryReturn(o.bvin, MTApp.CatalogServices);
                    break;
                case OrderShippingStatus.Unshipped:
                    success = MTApp.OrderServices.OrdersDeleteWithInventoryReturn(o.bvin, MTApp.CatalogServices);
                    break;
            }

            if (success)
            {
                Response.Redirect("~/BVAdmin/Orders/Default.aspx");
            }
        }
    }
}