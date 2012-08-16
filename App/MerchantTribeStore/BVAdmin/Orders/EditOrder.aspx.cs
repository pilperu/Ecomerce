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
using MerchantTribe.Web.Logging;

namespace MerchantTribeStore.BVAdmin.Orders
{
    public partial class EditOrder : BaseAdminPage
    {
        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.PageTitle = "Edit Order";
            this.CurrentTab = AdminTabType.Orders;
            ValidateCurrentUserHasPermission(SystemPermissions.OrdersView);
        }

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
            this.UserPicker1.MessageBox = this.MessageBox1;
            InitializeAddresses();
            this.UserPicker1.UserSelected += new BVAdmin_Controls_UserPicker.UserSelectedDelegate(UserPicker1_UserSelected);
            base.OnLoad(e);





            if (!Page.IsPostBack)
            {
                //LoadTemplates();
                if (Request.QueryString["id"] != null)
                {
                    BvinField.Value = Request.QueryString["id"];
                }
                else
                {
                    this.MessageBox1.ShowWarning("Unable to locate that order.");
                    // Show Error
                }
                LoadOrder();
            }


            //    If EditLineBvin.Value <> String.Empty Then
            //    EditLineVariantsDisplay.Initialize(False)
            //    EditLineKitComponentsDisplay.Initialize(EditLineBvin.Value)
            //End If

            if (this.AddProductSkuHiddenField.Value != string.Empty)
            {
                Product p = MTApp.CatalogServices.Products.Find(this.AddProductSkuHiddenField.Value);
                if (p != null)
                {
                    // Render Options
                    this.phChoices.Controls.Add(new System.Web.UI.LiteralControl("<div id=\"options\">"));
                    MerchantTribe.Commerce.Utilities.HtmlRendering.ProductOptionsAsControls(p.Options, phChoices);
                    this.phChoices.Controls.Add(new System.Web.UI.LiteralControl("<div class=\"clear\"></div></div>"));
                    this.litProductInfo.Text = p.Sku + " | " + p.ProductName;
                }
            }
            
        }



        private void InitializeAddresses()
        {
            this.ShippingAddressEditor.RequireCompany = false;
            this.ShippingAddressEditor.RequireFirstName = true;
            this.ShippingAddressEditor.RequireLastName = true;
            this.ShippingAddressEditor.RequirePhone = false;
            this.ShippingAddressEditor.ShowCompanyName = true;
            this.ShippingAddressEditor.ShowPhoneNumber = true;
            this.ShippingAddressEditor.ShowCounty = true;

            this.BillingAddressEditor.RequireCompany = false;
            this.BillingAddressEditor.RequireFirstName = true;
            this.BillingAddressEditor.RequireLastName = true;
            this.BillingAddressEditor.RequirePhone = false;
            this.BillingAddressEditor.ShowCompanyName = true;
            this.BillingAddressEditor.ShowPhoneNumber = true;
            this.BillingAddressEditor.ShowCounty = true;
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

            // Billing
            this.BillingAddressEditor.LoadFromAddress(o.BillingAddress);

            //Email            
            this.UserPicker1.UserName = o.UserEmail;

            // Shipping (hide if the same as billing address)                        
            this.ShippingAddressEditor.LoadFromAddress(o.ShippingAddress);


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
            this.pnlInstructions.Visible = true;
            this.InstructionsField.Text = o.Instructions.Replace("\r\n", "<br />").Replace("\r", "<br />").Replace("\n", "<br />");

            // Totals
            this.litTotals.Text = o.TotalsAsTable();

            if (o.TotalShippingBeforeDiscountsOverride >= 0)
            {
                this.ShippingOverride.Text = o.TotalShippingBeforeDiscountsOverride.ToString("C");
            }
            else
            {
                this.ShippingOverride.Text = string.Empty;
            }
            // Coupons            
            this.lstCoupons.DataSource = o.Coupons;
            this.lstCoupons.DataBind();

            LoadShippingMethods(o);
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

        protected void ItemsGridView_RowDataBound1(object sender, GridViewRowEventArgs e)
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

                }
            }
        }

        protected void btnSaveChanges_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            this.MessageBox1.ClearMessage();
            SystemOperationResult saveResult = SaveOrder();
            if (saveResult.Success)
            {
                RunOrderEditedWorkflow(saveResult);
            }
            else
            {
                this.MessageBox1.ShowError(saveResult.Message);
            }
            LoadOrder();
        }

        private void RunOrderEditedWorkflow(SystemOperationResult saveResult)
        {
            MerchantTribe.Commerce.BusinessRules.OrderTaskContext c = new MerchantTribe.Commerce.BusinessRules.OrderTaskContext(MTApp);
            c.Order = MTApp.OrderServices.Orders.FindForCurrentStore(this.BvinField.Value);
            c.UserId = c.Order.UserID;

            if (!MerchantTribe.Commerce.BusinessRules.Workflow.RunByName(c, MerchantTribe.Commerce.BusinessRules.WorkflowNames.OrderEdited))
            {
                foreach (MerchantTribe.Commerce.BusinessRules.WorkflowMessage msg in c.Errors)
                {
                    EventLog.LogEvent("Order Edited Workflow", msg.Description, EventLogSeverity.Warning);
                }
                if (!String.IsNullOrEmpty(saveResult.Message))
                {
                    this.MessageBox1.ShowError(saveResult.Message + " Error Occurred. Please see event log.");
                }
                else
                {
                    this.MessageBox1.ShowError("Error Occurred. Please see event log.");
                }
            }
            else
            {
                if (!String.IsNullOrEmpty(saveResult.Message))
                {
                    this.MessageBox1.ShowError(saveResult.Message);
                }
                else
                {
                    this.MessageBox1.ShowOk("Changes Saved");
                }
            }
        }

        private SystemOperationResult SaveOrder()
        {
            SystemOperationResult result = new SystemOperationResult(false, "");

            Order o = MTApp.OrderServices.Orders.FindForCurrentStore(this.BvinField.Value);
            if (o == null)
            {
                result.Success = false;
                result.Message = "Unable to reload order.";
                return result;
            }

            o.UserID = this.UserIdField.Value;
            o.UserEmail = this.UserPicker1.UserName;
            o.Instructions = this.InstructionsField.Text.Trim();

            o.BillingAddress = this.BillingAddressEditor.GetAsAddress();
            o.ShippingAddress = this.ShippingAddressEditor.GetAsAddress();

            SystemOperationResult modifyResult = ModifyQuantities(o);
            if (!modifyResult.Success)
            {
                result.Message = modifyResult.Message;
            }

            // Save Shipping Selection
            string shippingRateKey = Request.Form["shippingrate"];
            MTApp.OrderServices.OrdersRequestShippingMethodByUniqueKey(shippingRateKey, o, MTApp.CurrentStore);

            // Shipping Override
            if (this.ShippingOverride.Text.Trim().Length < 1)
            {
                o.TotalShippingBeforeDiscountsOverride = -1m;
            }
            else
            {
                decimal shipOverride = o.TotalShippingBeforeDiscountsOverride;
                decimal.TryParse(this.ShippingOverride.Text, System.Globalization.NumberStyles.Currency, System.Threading.Thread.CurrentThread.CurrentUICulture, out shipOverride);
                o.TotalShippingBeforeDiscountsOverride = shipOverride;
            }

            result.Success = MTApp.CalculateOrderAndSave(o);

            return result;
        }

        private SystemOperationResult ModifyQuantities(Order o)
        {
            SystemOperationResult result = new SystemOperationResult(true, "");

            for (int i = 0; i < this.ItemsGridView.Rows.Count - 1; i++)
            {
                GridViewRow currentRow = ItemsGridView.Rows[i];

                if (currentRow.RowType == DataControlRowType.DataRow)
                {
                    long lineitemId = (long)ItemsGridView.DataKeys[currentRow.RowIndex].Value;
                    var li = o.Items.Where(y => y.Id == lineitemId).FirstOrDefault();
                    if (li != null)
                    {
                        TextBox qty = (TextBox)currentRow.FindControl("QtyField");
                        if (qty != null)
                        {
                            string tempQty = qty.Text.Trim();
                            int actualQty = li.Quantity;
                            int.TryParse(tempQty, out actualQty);
                            li.Quantity = actualQty;
                        }
                    }
                }
            }
            return result;
        }

        void UserPicker1_UserSelected(object sender, MerchantTribe.Commerce.Controls.UserSelectedEventArgs e)
        {
            this.UserIdField.Value = e.UserAccount.Bvin;
        }

        protected void btnDeleteCoupon_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            Order o = MTApp.OrderServices.Orders.FindForCurrentStore(this.BvinField.Value);
            if (o != null)
            {
                foreach (System.Web.UI.WebControls.ListItem li in this.lstCoupons.Items)
                {
                    if (li.Selected)
                    {
                        o.RemoveCouponCodeByCode(li.Text);
                    }
                }
            }
            MTApp.OrderServices.Orders.Update(o);
            SaveOrder();
            LoadOrder();
        }

        protected void btnAddCoupon_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            Order o = MTApp.OrderServices.Orders.FindForCurrentStore(this.BvinField.Value);
            if (o != null)
            {
                o.AddCouponCode(this.CouponField.Text.Trim().ToUpper());
            }
            MTApp.OrderServices.Orders.Update(o);
            SaveOrder();
            LoadOrder();
        }

        private void LoadShippingMethods(Order o)
        {
            SortableCollection<ShippingRateDisplay> Rates = new SortableCollection<ShippingRateDisplay>();

            if (o.HasShippingItems == false)
            {
                ShippingRateDisplay r = new ShippingRateDisplay();
                r.DisplayName = SiteTerms.GetTerm(SiteTermIds.NoShippingRequired);
                r.ProviderId = "";
                r.ProviderServiceCode = "";
                r.Rate = 0;
                r.ShippingMethodId = "NOSHIPPING";
                Rates.Add(r);
            }
            else
            {
                // Shipping Methods

                Rates = MTApp.OrderServices.FindAvailableShippingRates(o, MTApp.CurrentStore);

                if ((Rates.Count < 1))
                {
                    ShippingRateDisplay result = new ShippingRateDisplay();
                    result.DisplayName = "Shipping can not be calculated at this time. We will contact you after receiving your order with the exact shipping charges.";
                    result.ShippingMethodId = "TOBEDETERMINED";
                    result.Rate = 0;
                    Rates.Add(result);
                }

            }

            this.litShippingMethods.Text = MerchantTribe.Commerce.Utilities.HtmlRendering.ShippingRatesToRadioButtons(Rates, 300, o.ShippingMethodUniqueKey);
        }

        protected void ItemsGridView_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            MessageBox1.ClearMessage();
            long Id = (long)ItemsGridView.DataKeys[e.RowIndex].Value;
            Order o = MTApp.OrderServices.Orders.FindForCurrentStore(this.BvinField.Value);
            if (o != null)
            {
                var li = o.Items.Where(y => y.Id == Id).SingleOrDefault();
                if (li != null)
                {
                    o.Items.Remove(li);
                    MTApp.OrderServices.Orders.Update(o);
                }
            }
            SaveOrder();
            LoadOrder();
        }

        protected void btnBrowseProducts_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            this.MessageBox1.ClearMessage();

            this.pnlProductPicker.Visible = !this.pnlProductPicker.Visible;

            if (this.NewSkuField.Text.Trim().Length > 0)
            {
                if (this.pnlProductPicker.Visible)
                {
                    this.ProductPicker1.Keyword = this.NewSkuField.Text;
                    this.ProductPicker1.LoadSearch();
                }
            }
        }

        protected void btnAddProductBySku_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            AddProductBySku();
        }

        protected void btnProductPickerCancel_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            this.MessageBox1.ClearMessage();
            this.pnlProductPicker.Visible = false;
        }

        protected void btnProductPickerOkay_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            this.MessageBox1.ClearMessage();
            if (this.ProductPicker1.SelectedProducts.Count > 0)
            {
                Product p = MTApp.CatalogServices.Products.Find(this.ProductPicker1.SelectedProducts[0]);
                if (p != null)
                {
                    this.NewSkuField.Text = p.Sku;
                }
                AddProductBySku();
                this.pnlProductPicker.Visible = false;
            }
            else
            {
                this.MessageBox1.ShowWarning("Please select a product first.");
            }
        }

        private void AddProductBySku()
        {
            this.MessageBox1.ClearMessage();
            this.pnlProductChoices.Visible = false;

            if (this.NewSkuField.Text.Trim().Length < 1)
            {
                this.MessageBox1.ShowWarning("Please enter a sku first.");
            }
            else
            {
                Product p = MTApp.CatalogServices.Products.FindBySku(this.NewSkuField.Text.Trim());

                if (p != null && p.Sku.Length > 0)
                {
                    if (p.HasOptions())
                    {
                        this.pnlAddControls.Visible = false;
                        this.pnlProductChoices.Visible = true;
                        this.AddProductSkuHiddenField.Value = p.Bvin;

                        // Render Options
                        this.phChoices.Controls.Clear();
                        this.phChoices.Controls.Add(new System.Web.UI.LiteralControl("<div id=\"options\">"));
                        MerchantTribe.Commerce.Utilities.HtmlRendering.ProductOptionsAsControls(p.Options, phChoices);
                        this.phChoices.Controls.Add(new System.Web.UI.LiteralControl("<div class=\"clear\"></div></div>"));
                        this.litProductInfo.Text = p.Sku + " | " + p.ProductName;
                    }
                    else
                    {
                        Order o = MTApp.OrderServices.Orders.FindForCurrentStore(this.BvinField.Value);
                        if (o != null)
                        {
                            int quantity = 1;
                            int.TryParse(this.NewProductQuantity.Text, out quantity);
                            OptionSelectionList selections = new OptionSelectionList();
                            LineItem li = MTApp.CatalogServices.ConvertProductToLineItem(p,
                                                                                    selections,
                                                                                    quantity,
                                                                                    MTApp);
                            o.Items.Add(li);
                            MTApp.OrderServices.Orders.Update(o);
                            this.MessageBox1.ShowOk("Product Added!");
                        }
                    }
                }
                else
                {
                    this.MessageBox1.ShowInformation("That SKU could not be located. Please try again.");
                }
            }
            SaveOrder();
            LoadOrder();
        }

        protected void btnCloseVariants_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {            
            this.MessageBox1.ClearMessage();
            this.pnlAddControls.Visible = true;
            this.pnlProductChoices.Visible = false;
        }

        protected void btnAddVariant_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            this.MessageBox1.ClearMessage();
            this.pnlAddControls.Visible = true;
            this.pnlProductChoices.Visible = false;

            Product product = MTApp.CatalogServices.Products.Find(this.AddProductSkuHiddenField.Value);
            if (product != null && product.Sku.Trim().Length > 0)
            {
                
                int quantity = 1;
                int.TryParse(this.NewProductQuantity.Text, out quantity);
                OptionSelectionList selections = ParseSelections(product);
                if (ValidateSelections(product, selections))
                {
                    Order o = MTApp.OrderServices.Orders.FindForCurrentStore(this.BvinField.Value);
                    if (o != null)
                    {
                        LineItem li = MTApp.CatalogServices.ConvertProductToLineItem(product,
                                                                                selections,
                                                                                quantity,
                                                                                MTApp);
                        o.Items.Add(li);
                        MTApp.OrderServices.Orders.Update(o);
                        this.MessageBox1.ShowOk("Product Added!");
                        this.AddProductSkuHiddenField.Value = string.Empty;
                        SaveOrder();
                        LoadOrder();
                    }                    
                }
                else
                {
                    this.litMessage.Text = "That combination of choices/options is not available at the moment.";
                }                                                
            }
        }

        private OptionSelectionList ParseSelections(Product p)
        {
            OptionSelectionList result = new OptionSelectionList();

            foreach (Option opt in p.Options)
            {
                OptionSelection selected = opt.ParseFromPlaceholder(this.phChoices);
                if (selected != null)
                {
                    result.Add(selected);
                }
            }

            return result;
        }

        private bool ValidateSelections(Product p, OptionSelectionList selections)
        {
            bool result = false;

            if ((p.HasOptions()))
            {
                if ((p.HasVariants()))
                {
                    Variant v = p.Variants.FindBySelectionData(selections, p.Options);
                    if ((v != null))
                    {
                        result = true;
                    }
                    else
                    {
                        this.litMessage.Text = "<div class=\"flash-message-warning\">The options you've selected aren't available at the moment. Please select different options.</div>";
                    }
                }
                else
                {
                    result = true;
                }

                // Price Modifiers Here

            }
            else
            {
                result = true;
            }

            return result;
        }

    }
}