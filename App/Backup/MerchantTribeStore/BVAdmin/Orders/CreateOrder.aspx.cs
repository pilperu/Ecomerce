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

namespace MerchantTribeStore.BVAdmin.Orders
{
    public partial class CreateOrder : BaseAdminPage
    {
        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.PageTitle = "New Order";
            this.CurrentTab = AdminTabType.Orders;
            ValidateCurrentUserHasPermission(SystemPermissions.OrdersEdit);
        }

        protected Order o;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            // Tag an id to the querystring to support back button
            if (Request.QueryString["id"] != null)
            {
                this.BvinField.Value = Request.QueryString["id"];
            }
            else
            {
                o = new Order();
                MTApp.OrderServices.Orders.Create(o);
                Response.Redirect("CreateOrder.aspx?id=" + o.bvin);
            }

            if (this.BvinField.Value.Trim() != string.Empty)
            {
                o = MTApp.OrderServices.Orders.FindForCurrentStore(this.BvinField.Value);
            }
            if (!Page.IsPostBack)
            {
                LoadOrder();
                LoadPaymentMethods();
                LoadCurrentUser();
            }

            //Me.UserPicker1.MessageBox = MessageBox1
            //AddHandler Me.UserPicker1.UserSelected, AddressOf Me.UserSelected

            //If Me.NewSkuField.Text <> String.Empty Then
            //VariantsDisplay1.Initialize(False)
            //End If

        }


        protected void chkBillToSame_CheckedChanged(object sender, System.EventArgs e)
        {
            if (chkBillToSame.Checked == true)
            {
                this.BillToAddress.LoadFromAddress(this.ShipToAddress.GetAsAddress());
                this.pnlBillTo.Visible = false;
            }
            else
            {
                this.pnlBillTo.Visible = true;
            }
            ClearShipping();
        }

        private void LoadOrder(string bvin)
        {
            o = MTApp.OrderServices.Orders.FindForCurrentStore(bvin);
            LoadOrder();
        }

        private void LoadOrder()
        {

            // Status
            this.StatusField.Text = o.FullOrderStatusDescription();
            //Me.StatusField.Text += "<br />BVIN: " & o.Bvin

            //Items
            this.ItemsGridView.DataSource = o.Items;
            this.ItemsGridView.DataBind();

            // Totals
            if (o.TotalOrderDiscounts > 0)
            {
                this.SubTotalField.Text = "<span class=\"MarkDownPrice\">" + string.Format("{0:c}", o.TotalOrderBeforeDiscounts) + "</span><br />";
                this.SubTotalField.Text += string.Format("{0:c}", o.TotalOrderBeforeDiscounts - o.TotalOrderDiscounts);
            }

            else
            {
                this.SubTotalField.Text = string.Format("{0:c}", o.TotalOrderBeforeDiscounts);
            }
            this.SubTotalField2.Text = this.SubTotalField.Text;

            if (o.TotalShippingDiscounts > 0)
            {
                this.ShippingTotalField.Text = "<span class=\"MarkDownPrice\">" + string.Format("{0:c}", o.TotalShippingBeforeDiscounts) + "</span><br />";
                this.ShippingTotalField.Text += string.Format("{0:c}", o.TotalShippingBeforeDiscounts - o.TotalShippingDiscounts);
            }
            else
            {
                this.ShippingTotalField.Text = string.Format("{0:c}", o.TotalShippingBeforeDiscounts);
            }
            this.TaxTotalField.Text = string.Format("{0:c}", o.TotalTax);
            this.HandlingTotalField.Text = string.Format("{0:c}", o.TotalHandling);
            this.GrandTotalField.Text = string.Format("{0:c}", o.TotalGrand);

            if (o.Items.Count > 0)
            {
                this.btnUpdateQuantities.Visible = true;
            }
            else
            {
                this.btnUpdateQuantities.Visible = false;
            }

            this.CouponGrid.DataSource = o.Coupons;
            this.CouponGrid.DataBind();

        }

        private void LoadCurrentUser()
        {
            this.UserIdField.Value = o.UserID;
            this.BillToAddress.LoadFromAddress(o.BillingAddress);
            this.ShipToAddress.LoadFromAddress(o.ShippingAddress);
            this.EmailAddressTextBox.Text = o.UserEmail;                        
        }

        #region " Add Products Functions "

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

            Order o = MTApp.OrderServices.Orders.FindForCurrentStore(this.BvinField.Value);

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
            MTApp.OrderServices.Orders.Update(o);
            LoadOrder(o.bvin);
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
                        MTApp.OrderServices.Orders.Update(o);                        
                        LoadOrder(o.bvin);
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

        #endregion

        protected void btnUpdateQuantities_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            this.MessageBox1.ClearMessage();
            for (int i = 0; i <= this.ItemsGridView.Rows.Count - 1; i++)
            {
                if (ItemsGridView.Rows[i].RowType == DataControlRowType.DataRow)
                {
                    TextBox qty = (TextBox)ItemsGridView.Rows[i].FindControl("QtyField");
                    if (qty != null)
                    {
                        
                        long itemId = (long)ItemsGridView.DataKeys[ItemsGridView.Rows[i].RowIndex].Value;                        

                        LineItem li = o.GetLineItem(itemId);
                        if (li == null) li = new LineItem();

                        if (ValidateInventoryForLineItem(li.ProductId, qty.Text.Trim()))
                        {
                            SystemOperationResult val = MTApp.OrderServices.OrdersUpdateItemQuantity(itemId, int.Parse(qty.Text.Trim()), o);
                            if (val.Message != string.Empty)
                            {
                                MessageBox1.ShowError(val.Message);
                            }
                        }
                    }

                    //TextBox price = (TextBox)ItemsGridView.Rows[i].FindControl("PriceField");
                    //if (price != null)
                    //{
                    //    decimal temp;
                    //    if (decimal.TryParse(price.Text, System.Globalization.NumberStyles.Currency, System.Threading.Thread.CurrentThread.CurrentUICulture, out temp))
                    //    {
                    //        string bvin = (string)ItemsGridView.DataKeys[ItemsGridView.Rows[i].RowIndex].Value;
                    //        string val = MTApp.OrderServices o.SetItemAdminPrice(bvin, decimal.Parse(price.Text, System.Globalization.NumberStyles.Currency)).Message;
                    //        if (val != string.Empty)
                    //        {
                    //            MessageBox1.ShowError(val);
                    //        }
                    //    }
                    //}
                }
            }
            LoadOrder();
            ClearShipping();

        }

        protected void ItemGridView_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
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

        protected void ItemGridView_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
        {
            this.MessageBox1.ClearMessage();
            
            long Id = (long)ItemsGridView.DataKeys[e.RowIndex].Value;
                                   
            if (o != null)
            {
                var li = o.Items.Where(y => y.Id == Id).SingleOrDefault();
                if (li != null)
                {
                    o.Items.Remove(li);
                    MTApp.CalculateOrderAndSave(o);                    
                }
                ClearShipping();
                LoadOrder(o.bvin);
            }           
        }

        private void LoadPaymentMethods()
        {
            MerchantTribe.Commerce.Payment.AvailablePayments availablePayments = new MerchantTribe.Commerce.Payment.AvailablePayments();
            Collection<DisplayPaymentMethod> enabledMethods;
            enabledMethods = availablePayments.EnabledMethods(MTApp.CurrentStore);

            this.rowNoPaymentNeeded.Visible = false;
            foreach (DisplayPaymentMethod m in enabledMethods)
            {
                switch (m.MethodId)
                {
                    case WebAppSettings.PaymentIdCheck:
                        this.lblCheckDescription.Text = WebAppSettings.PaymentCheckDescription;
                        this.rowCheck.Visible = true;
                        break;
                    case WebAppSettings.PaymentIdCreditCard:
                        this.rowCreditCard.Visible = true;
                        break;
                    case WebAppSettings.PaymentIdTelephone:
                        this.lblTelephoneDescription.Text = WebAppSettings.PaymentTelephoneDescription;
                        this.rowTelephone.Visible = true;
                        break;
                    case WebAppSettings.PaymentIdPurchaseOrder:
                        this.lblPurchaseOrderDescription.Text = WebAppSettings.PaymentPurchaseOrderName;
                        this.trPurchaseOrder.Visible = true;
                        break;
                    case WebAppSettings.PaymentIdCashOnDelivery:
                        this.lblCOD.Text = WebAppSettings.PaymentCODDescription;
                        this.trCOD.Visible = true;
                        break;
                    case WebAppSettings.PaymentIdCompanyAccount:
                        this.lblCompanyAccountDescription.Text = WebAppSettings.PaymentCompanyAccountName;
                        this.trCompanyAccount.Visible = true;
                        break;
                    default:
                        // do nothign
                        break;
                }

            }

            if (enabledMethods.Count == 1)
            {
                switch (enabledMethods[0].MethodId)
                {
                    case WebAppSettings.PaymentIdCheck:
                        rbCheck.Checked = true;
                        break;
                    case WebAppSettings.PaymentIdCreditCard:
                        rbCreditCard.Checked = true;
                        break;
                    case WebAppSettings.PaymentIdTelephone:
                        rbTelephone.Checked = true;
                        break;
                    case WebAppSettings.PaymentIdPurchaseOrder:
                        rbPurchaseOrder.Checked = true;
                        break;
                    case WebAppSettings.PaymentIdCompanyAccount:
                        rbCompanyAccount.Checked = true;
                        break;
                    case WebAppSettings.PaymentIdCashOnDelivery:
                        rbCOD.Checked = true;
                        break;
                }
            }
            else
            {
                if (rbCreditCard.Visible)
                {
                    rbCreditCard.Checked = true;
                }
            }

        }

        private bool SaveInfoToOrder(bool savePaymentData)
        {
            bool result = true;

            if (o != null)
            {
                if (this.chkBillToSame.Checked == true)
                {
                    this.BillToAddress.LoadFromAddress(this.ShipToAddress.GetAsAddress());
                }

                // Save Information to Basket in Case Save as Order Fails
                o.BillingAddress = this.BillToAddress.GetAsAddress();
                o.ShippingAddress = this.ShipToAddress.GetAsAddress();
                TagOrderWithUser();

                o.UserEmail = this.EmailAddressTextBox.Text;

                // Save Shipping Selection
                ShippingRateDisplay r = FindSelectedRate(this.ShippingRatesList.SelectedValue, o);
                if (r != null)
                {
                    MTApp.OrderServices.OrdersRequestShippingMethodByUniqueKey(r.UniqueKey, o, MTApp.CurrentStore);
                }
                
                if (savePaymentData)
                {
                    // Save Payment Information
                    SavePaymentInfo(o);
                }

                MTApp.CalculateOrderAndSave(o);                
            }

            else
            {
                result = false;
            }

            return result;
        }

        protected void btnSubmit_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            this.MessageBox1.ClearMessage();

            if (this.ValidateSelections() == true)
            {
                if (o != null)
                {
                    SaveInfoToOrder(true);

                    //we can't put this in "SaveInfoToOrder" because
                    //that is called multiple times on the page
                    OrderNote note = new OrderNote();
                    note.Note = InstructionsField.Text;
                    note.IsPublic = false;
                    o.Notes.Add(note);
                    MTApp.OrderServices.Orders.Update(o);

                    // Save as Order
                    MerchantTribe.Commerce.BusinessRules.OrderTaskContext c = new MerchantTribe.Commerce.BusinessRules.OrderTaskContext(MTApp);
                    c.UserId = string.Empty;
                    // Use currently selected user for process new order context
                    CustomerAccount u = GetSelectedUserAccount();
                    if (u != null)
                    {
                        if (u.Bvin != string.Empty)
                        {
                            c.UserId = u.Bvin;
                            o.UserID = u.Bvin;
                        }
                    }
                    c.Order = o;

                    if (MerchantTribe.Commerce.BusinessRules.Workflow.RunByName(c, MerchantTribe.Commerce.BusinessRules.WorkflowNames.ProcessNewOrder))
                    {
                        Response.Redirect("ViewOrder.aspx?id=" + o.bvin);
                    }
                    else
                    {
                        // Show Errors   
                        string errorString = string.Empty;
                        for (int i = 0; i <= c.Errors.Count - 1; i++)
                        {
                            errorString += c.Errors[i].Description + "<br />";
                        }
                        if (errorString.Length > 0)
                        {
                            this.MessageBox1.ShowWarning(errorString);
                        }
                    }
                }
            }
        }

        private bool ValidateSelections()
        {
            bool result = true;

            if (((this.ShippingRatesList.SelectedValue == null) | (this.ShippingRatesList.SelectedIndex == -1) | (this.ShippingRatesList.SelectedValue == string.Empty)))
            {
                MessageBox1.ShowWarning("Please Select a Shipping Method");
                result = false;
            }

            bool paymentFound = false;

            if (this.rbCreditCard.Checked == true)
            {
                paymentFound = true;
                if (!this.CreditCardInput1.IsValid())
                {
                    foreach (MerchantTribe.Web.Validation.RuleViolation item in this.CreditCardInput1.GetRuleViolations())
                    {
                        this.MessageBox1.ShowWarning(item.ErrorMessage);
                    }
                    result = false;
                }
            }
            if (this.rbCheck.Checked == true)
            {
                paymentFound = true;
            }
            if (this.rbTelephone.Checked == true)
            {
                paymentFound = true;
            }
            if (this.rbCompanyAccount.Checked == true)
            {
                paymentFound = true;
            }
            if (this.rbPurchaseOrder.Checked == true)
            {
                paymentFound = true;
            }
            if (this.rbCOD.Checked == true)
            {
                paymentFound = true;
            }

            if (paymentFound == false)
            {
                this.MessageBox1.ShowWarning("Please select a Payment Method");
                result = false;
            }

            return result;
        }

        private CustomerAccount GetSelectedUserAccount()
        {
            CustomerAccount result = new CustomerAccount();
            result = MTApp.MembershipServices.Customers.Find(this.UserIdField.Value);
            return result;
        }
        private void TagOrderWithUser()
        {
            TagOrderWithUser(GetSelectedUserAccount());
        }

        private void TagOrderWithUser(CustomerAccount account)
        {
            CustomerAccount u = account;
            if (u != null)
            {
                if (u.Bvin != string.Empty)
                {
                    this.UserIdField.Value = u.Bvin;
                    o.UserID = u.Bvin;
                    u.CheckIfNewAddressAndAddNoUpdate(this.BillToAddress.GetAsAddress());
                    u.CheckIfNewAddressAndAddNoUpdate(this.ShipToAddress.GetAsAddress());
                    MTApp.MembershipServices.Customers.Update(u);
                }
            }
        }

        private void SavePaymentInfo(Order o)
        {

            OrderPaymentManager payManager = new OrderPaymentManager(o, MTApp);

            payManager.ClearAllNonStoreCreditTransactions();

            bool found = false;

            if (this.rbCreditCard.Checked == true)
            {
                found = true;
                payManager.CreditCardAddInfo(this.CreditCardInput1.GetCardData(), o.TotalGrandAfterStoreCredits(MTApp.OrderServices));
            }

            if ((found == false) && (this.rbCheck.Checked == true))
            {
                found = true;
                payManager.OfflinePaymentAddInfo(o.TotalGrandAfterStoreCredits(MTApp.OrderServices), "Customer will pay by check.");
            }

            if ((found == false) && (this.rbTelephone.Checked == true))
            {
                found = true;
                payManager.OfflinePaymentAddInfo(o.TotalGrandAfterStoreCredits(MTApp.OrderServices), "Customer will call with payment info.");
            }

            if ((found == false) && (this.rbPurchaseOrder.Checked == true))
            {
                found = true;
                payManager.PurchaseOrderAddInfo(this.PurchaseOrderField.Text.Trim(), o.TotalGrandAfterStoreCredits(MTApp.OrderServices));
            }
            if ((found == false) && (this.rbCompanyAccount.Checked == true))
            {
                found = true;
                payManager.CompanyAccountAddInfo(this.accountnumber.Text.Trim(), o.TotalGrandAfterStoreCredits(MTApp.OrderServices));
            }

            if ((found == false) && (this.rbCOD.Checked == true))
            {
                found = true;
                payManager.OfflinePaymentAddInfo(o.TotalGrandAfterStoreCredits(MTApp.OrderServices), "Customer will pay cash on delivery.");
            }

            MTApp.OrderServices.Orders.Update(o);       
        }

        private ShippingRateDisplay FindSelectedRate(string uniqueKey, Order o)
        {
            ShippingRateDisplay result = null;

            if (o.HasShippingItems == false)
            {
                result = new ShippingRateDisplay();
                result.DisplayName = "No Shipping Required";
                result.ProviderId = "NOSHIPPING";
                result.ProviderServiceCode = "NOSHIPPING";
                result.Rate = 0m;
                result.ShippingMethodId = "NOSHIPPING";
            }
            else
            {
                MerchantTribe.Commerce.Utilities.SortableCollection<ShippingRateDisplay> rates = SessionManager.LastShippingRates;
                if ((rates == null) | (rates.Count < 1))
                {
                    rates = MTApp.OrderServices.FindAvailableShippingRates(o, MTApp.CurrentStore);
                }

                foreach (ShippingRateDisplay r in rates)
                {
                    if (r.UniqueKey == uniqueKey)
                    {
                        result = r;
                        break;
                    }
                }
            }

            return result;
        }

        protected void btnCalculateShipping_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            this.MessageBox1.ClearMessage();
            SaveInfoToOrder(false);
            //o = Orders.Order.FindByBvin(Me.BvinField.Value)
            LoadShippingMethodsForOrder(o);            
        }

        private void LoadShippingMethodsForOrder(Order o)
        {
            // Shipping Methods
            this.ShippingRatesList.Items.Clear();

            if (o.HasShippingItems == false)
            {
                this.ShippingRatesList.Items.Add(new System.Web.UI.WebControls.ListItem("No Shipping Required", "NOSHIPPING"));
            }
            else
            {
                MerchantTribe.Commerce.Utilities.SortableCollection<ShippingRateDisplay> Rates;
                Rates = MTApp.OrderServices.FindAvailableShippingRates(o, MTApp.CurrentStore);
                SessionManager.LastShippingRates = Rates;
                this.ShippingRatesList.DataTextField = "RateAndNameForDisplay";
                this.ShippingRatesList.DataValueField = "UniqueKey";
                this.ShippingRatesList.DataSource = Rates;
                this.ShippingRatesList.DataBind();
            }

        }

        private void ClearShipping()
        {
            int selectedIndex = this.ShippingRatesList.SelectedIndex;
            this.ShippingRatesList.ClearSelection();
            this.ShippingRatesList.Items.Clear();
            LoadShippingMethodsForOrder(o);
            if (this.ShippingRatesList.Items.Count > selectedIndex)
            {
                this.ShippingRatesList.SelectedIndex = selectedIndex;
            }
            if (this.ShippingRatesList.SelectedItem != null)
            {
                SaveInfoToOrder(false);
                LoadOrder();
            }
        }

        protected void btnAddCoupon_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            MessageBox1.ClearMessage();
            bool couponResult = o.AddCouponCode(this.CouponField.Text.Trim());
            if (couponResult == false)
            {
                MessageBox1.ShowError("Coupon does not apply or already exists.");
            }
            else
            {
                MTApp.OrderServices.Orders.Update(o);
            }
            LoadOrder(o.bvin);
        }

        protected void CouponGrid_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
        {
            this.MessageBox1.ClearMessage();
            string code = string.Empty;
            code = (string)this.CouponGrid.DataKeys[e.RowIndex].Value;
            if (code != string.Empty)
            {
                o.RemoveCouponCodeByCode(code);
                MTApp.OrderServices.Orders.Update(o);
            }
            LoadOrder(o.bvin);
        }

        protected void UserSelected(MerchantTribe.Commerce.Controls.UserSelectedEventArgs args)
        {
            if (args.UserAccount == null) return;

            this.UserIdField.Value = args.UserAccount.Bvin;

            o.UserID = args.UserAccount.Bvin;

            MTApp.OrderServices.Orders.Update(o);            

            this.BillToAddress.LoadFromAddress(args.UserAccount.BillingAddress);
            this.ShipToAddress.LoadFromAddress(args.UserAccount.ShippingAddress);
            if (this.BillToAddress.FirstName == string.Empty)
            {
                this.BillToAddress.FirstName = args.UserAccount.FirstName;
            }
            if (this.BillToAddress.LastName == string.Empty)
            {
                this.BillToAddress.LastName = args.UserAccount.LastName;
            }
            if (this.ShipToAddress.FirstName == string.Empty)
            {
                this.ShipToAddress.FirstName = args.UserAccount.FirstName;
            }
            if (this.ShipToAddress.LastName == string.Empty)
            {
                this.ShipToAddress.LastName = args.UserAccount.LastName;
            }

            this.EmailAddressTextBox.Text = args.UserAccount.Email;
            
            
            ClearShipping();

            o.UserEmail = this.EmailAddressTextBox.Text;
            o.BillingAddress = this.BillToAddress.GetAsAddress();
            o.ShippingAddress = this.ShipToAddress.GetAsAddress();
            MTApp.OrderServices.Orders.Update(o);   
        }

        protected bool ValidateInventoryForLineItem(string productBvin, string qty)
        {
            bool result = true;
            Product p = MTApp.CatalogServices.Products.Find(productBvin);
            if (p != null)
            {
                result = ValidateInventory(p, qty);
            }
            return result;
        }
        protected bool ValidateInventory(Product p, string qty)
        {
            bool result = true;

            decimal quantity = 1;
            decimal.TryParse(qty, out quantity);
            if (quantity < 1)
            {
                return true;
            }

            InventoryCheckData inv = MTApp.CatalogServices.InventoryCheck(p, string.Empty);
            
                if (inv.IsAvailableForSale && inv.Qty >= quantity)
                {
                    result = true;
                }
                else
                {
                    result = false;
                    if (inv.IsAvailableForSale == true)
                    {
                        MessageBox1.ShowWarning("Only " + inv.Qty.ToString() + " of the item " + p.Sku + " are available for purchase. Please adjust your requested amount.");
                    }
                }            

            return result;
        }

        #region " Find User Code "

        protected void btnFindUsers_Click(object sender, System.EventArgs e)
        {
            this.viewFindUsers.ActiveViewIndex = 0;
        }

        protected void btnNewUsers_Click(object sender, System.EventArgs e)
        {
            this.viewFindUsers.ActiveViewIndex = 1;
        }

        protected void btnFindOrders_Click(object sender, System.EventArgs e)
        {
            this.viewFindUsers.ActiveViewIndex = 2;
        }

        protected void btnGoFindOrder_Click(object sender, System.EventArgs e)
        {
            this.lblFindOrderMessage.Text = string.Empty;

            OrderSearchCriteria c = new OrderSearchCriteria();
            c.OrderNumber = this.FindOrderNumberField.Text.Trim();
            c.IsPlaced = true;
            c.StartDateUtc = DateTime.UtcNow.AddYears(-5);
            c.EndDateUtc = DateTime.UtcNow.AddYears(1);

            bool found = false;

            int totalCount = 0;
            List<OrderSnapshot> results = MTApp.OrderServices.Orders.FindByCriteriaPaged(c, 1, 10, ref totalCount);
            if (results != null)
            {
                if (results.Count > 0)
                {
                    found = true;
                    MerchantTribe.Commerce.Controls.UserSelectedEventArgs args = new MerchantTribe.Commerce.Controls.UserSelectedEventArgs();
                    args.UserAccount = MTApp.MembershipServices.Customers.Find(results[0].UserID);
                    this.UserSelected(args);
                }
            }

            if (!found)
            {
                this.lblFindOrderMessage.Text = "<span class=\"errormessage\">That order was not found</span>";
            }
        }

        protected void btnNewUserSave_Click(object sender, System.EventArgs e)
        {
            this.lblNewUserMessage.Text = string.Empty;
            CustomerAccount u = new CustomerAccount();
            u.Email = this.NewUserEmailField.Text.Trim();
            u.FirstName = this.NewUserFirstNameField.Text.Trim();
            u.LastName = this.NewUserLastNameField.Text.Trim();
            string clearPassword = MerchantTribe.Web.PasswordGenerator.GeneratePassword(12);            
            
            if (MTApp.MembershipServices.CreateCustomer(u, clearPassword) == true)
            {
                MerchantTribe.Commerce.Controls.UserSelectedEventArgs args = new MerchantTribe.Commerce.Controls.UserSelectedEventArgs();
                args.UserAccount = MTApp.MembershipServices.Customers.Find(u.Bvin);
                this.UserSelected(args);
            }
            else
            {
                this.lblNewUserMessage.Text = "<span class=\"errormessage\">Unable to create this account at this time. Unknown Error.</span>";
            }
        }

        protected void btnFindUser_Click(object sender, System.EventArgs e)
        {
            this.lblNewUserMessage.Text = string.Empty;
            this.GridView1.Visible = false;
                        
            int totalCount = 0;
            bool found = false;
            List<CustomerAccount> users = MTApp.MembershipServices.Customers.FindByFilter(this.FilterUserField.Text.Trim(), 1, 10, ref totalCount);
            if (users != null)
            {
                if (users.Count > 0)
                {
                    found = true;

                    if (users.Count == 1)
                    {
                        this.lblFindUserMessage.Text = "<span class=\"successmessage\">User " + users[0].Email + " Found and Selected</span>";
                        MerchantTribe.Commerce.Controls.UserSelectedEventArgs args = new MerchantTribe.Commerce.Controls.UserSelectedEventArgs();
                        args.UserAccount = users[0];
                        this.UserSelected(args);
                    }
                    else
                    {
                        this.lblFindUserMessage.Text = "<span class=\"successmessage\">Found " + totalCount.ToString() + " matching users.</span>";
                        this.GridView1.Visible = true;
                        this.GridView1.DataSource = users;
                        this.GridView1.DataBind();
                    }
                }
            }

            if (!found)
            {
                this.lblFindUserMessage.Text = "<span class=\"errormessage\">No matching users were found.</span>";
            }
        }

        protected void GridView1_RowEditing(object sender, System.Web.UI.WebControls.GridViewEditEventArgs e)
        {
            string bvin = (string)GridView1.DataKeys[e.NewEditIndex].Value;
            CustomerAccount u = MTApp.MembershipServices.Customers.Find(bvin);
            MerchantTribe.Commerce.Controls.UserSelectedEventArgs args = new MerchantTribe.Commerce.Controls.UserSelectedEventArgs();
            args.UserAccount = u;
            this.UserSelected(args);
        }
        #endregion

        protected void ShippingRatesList_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (this.ShippingRatesList.SelectedItem != null)
            {
                SaveInfoToOrder(false);
                LoadOrder();
            }
        }

        protected void btnUpdateShipping_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            SaveInfoToOrder(false);
            LoadOrder();
        }

    }
}