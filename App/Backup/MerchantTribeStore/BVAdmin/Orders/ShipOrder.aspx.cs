using System;
using System.Web;
using System.Web.UI.WebControls;
using MerchantTribe.Commerce;
using MerchantTribe.Commerce.Orders;
using MerchantTribe.Commerce.Membership;
using MerchantTribe.Commerce.Utilities;
using MerchantTribe.Commerce.Shipping;
using MerchantTribe.Commerce.Payment;
using MerchantTribe.Commerce.Catalog;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using MerchantTribe.Shipping;
using System.Collections.Generic;
using System.Linq;

namespace MerchantTribeStore
{

    partial class BVAdmin_Orders_ShipOrder : BaseAdminPage
    {

        private Order o = new MerchantTribe.Commerce.Orders.Order();

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.PageTitle = "Shipping";
            this.CurrentTab = AdminTabType.Orders;
            ValidateCurrentUserHasPermission(SystemPermissions.OrdersEdit);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (!Page.IsPostBack)
            {
                //PopulateShippingProviders();
                LoadOrder();
                
                // Acumatica Warning
                if (MTApp.CurrentStore.Settings.Acumatica.IntegrationEnabled)
                {
                    this.MessageBox1.ShowWarning(MerchantTribe.Commerce.Content.SiteTerms.GetTerm(MerchantTribe.Commerce.Content.SiteTermIds.AcumaticaWarning));
                }
            }
        }

        private void LoadOrder()
        {
            o = MTApp.OrderServices.Orders.FindForCurrentStore(Request.QueryString["id"]);
            if (o != null)
            {
                this.lblOrderNumber.Text = "Order " + o.OrderNumber + " ";
                this.ShippingAddressField.Text = o.ShippingAddress.ToHtmlString();
                this.ShippingTotalLabel.Text = o.TotalShippingAfterDiscounts.ToString("c");

                this.ItemsGridView.DataSource = o.Items;
                this.ItemsGridView.DataBind();

                this.PackagesGridView.DataSource = o.FindShippedPackages();
                this.PackagesGridView.DataBind();

                //this.SuggestedPackagesGridView.DataSource = o.FindSuggestedPackages();
                //this.SuggestedPackagesGridView.DataBind();

                this.UserSelectedShippingMethod.Text = "User Selected Shipping Method: <b>" + o.ShippingMethodDisplayName + "</b>";

                //if (this.lstShippingProvider.Items.FindByValue(o.ShippingProviderId) != null) {
                //    this.lstShippingProvider.ClearSelection();
                //    this.lstShippingProvider.Items.FindByValue(o.ShippingProviderId).Selected = true;
                //    LoadServiceCodes();
                //    this.lstServiceCode.SelectedValue = o.ShippingProviderServiceCode;
                //}
            }
        }

        private void ReloadOrder(OrderShippingStatus previousShippingStatus)
        {
            o = MTApp.OrderServices.Orders.FindForCurrentStore(Request.QueryString["id"]);
            o.EvaluateCurrentShippingStatus();
            MTApp.OrderServices.Orders.Update(o);
            MerchantTribe.Commerce.BusinessRules.OrderTaskContext context 
                = new MerchantTribe.Commerce.BusinessRules.OrderTaskContext(MTApp);
            context.Order = o;
            context.UserId = o.UserID;
            context.Inputs.Add("bvsoftware", "PreviousShippingStatus", previousShippingStatus.ToString());
            MerchantTribe.Commerce.BusinessRules.Workflow.RunByName(context, MerchantTribe.Commerce.BusinessRules.WorkflowNames.ShippingChanged);
            LoadOrder();
            this.OrderStatusDisplay1.LoadStatusForOrder(o);
        }

        //private void PopulateShippingProviders()
        //{
        //    this.lstShippingProvider.DataSource = MerchantTribe.Commerce.Shipping.AvailableServices.FindAll();
        //    this.lstShippingProvider.DataTextField = "Name";
        //    this.lstShippingProvider.DataValueField = "Id";
        //    this.lstShippingProvider.DataBind();
        //}

      

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

                        Product associatedProduct = lineItem.GetAssociatedProduct(MTApp);
                        if (associatedProduct != null)
                        {
                            if (lineItem.ShippingStatus == OrderShippingStatus.NonShipping)
                            {
                                description.Text += "<br />Non-Shipping";
                            }
                            if (associatedProduct.ShippingMode == ShippingMode.ShipFromManufacturer)
                            {
                                description.Text += "<br />Ships From Manufacturer";
                            }
                            else if (associatedProduct.ShippingMode == ShippingMode.ShipFromVendor)
                            {
                                description.Text += "<br />Ships From Vendor";
                            }
                            if (associatedProduct.ShippingDetails.ShipSeparately)
                            {
                                description.Text += "<br /> Ships Separately ";
                            }
                        }
                    }


                    TextBox QtyToShip = (TextBox)e.Row.FindControl("QtyToShip");

                    if (QtyToShip != null)
                    {
                        if (lineItem.ShippingStatus == OrderShippingStatus.NonShipping)
                        {
                            QtyToShip.Visible = false;
                        }
                        else
                        {
                            QtyToShip.Visible = true;
                        }

                        int q = (int)lineItem.Quantity - (int)lineItem.QuantityShipped;
                        if (q > 0)
                        {
                            QtyToShip.Text = string.Format("{0:#}", q);
                        }
                    }

                    if (lineItem.QuantityShipped > 0m)
                    {
                        Label shipped = (Label)e.Row.FindControl("shipped");
                        if (shipped != null)
                        {
                            shipped.Text = string.Format("{0:#}", lineItem.QuantityShipped);
                        }
                    }

                }
            }
        }

        protected void btnShipItems_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            ShipOrPackageItems(false);
        }

        protected void btnCreatePackage_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            ShipOrPackageItems(true);
        }

        private void ShipOrPackageItems(bool dontShip)
        {
            Order order = MTApp.OrderServices.Orders.FindForCurrentStore(Request.QueryString["id"]);
            OrderShippingStatus previousShippingStatus = order.ShippingStatus;

            string serviceCode = string.Empty;
            if (this.lstTrackingProvider.SelectedValue != null)
            {
                serviceCode = this.lstTrackingProvider.SelectedValue;
            }

            OrderPackage p = ShipItems(order, this.TrackingNumberField.Text.Trim(), order.ShippingProviderId, serviceCode, dontShip);

            ReloadOrder(previousShippingStatus);
        }

        private OrderPackage ShipItems(Order o, string trackingNumber, string serviceProvider, string serviceCode, bool dontShip)
        {
            OrderPackage p = new OrderPackage();

            p.ShipDateUtc = DateTime.UtcNow;
            p.TrackingNumber = trackingNumber;
            p.ShippingProviderId = serviceProvider;
            p.ShippingProviderServiceCode = serviceCode;
            foreach (GridViewRow gvr in this.ItemsGridView.Rows)
            {
                if (gvr.RowType == DataControlRowType.DataRow)
                {
                    LineItem li = o.GetLineItem((long)this.ItemsGridView.DataKeys[gvr.RowIndex].Value);
                    if (li != null)
                    {

                        int qty = 0;
                        TextBox QtyToShip = (TextBox)gvr.FindControl("QtyToShip");
                        if (QtyToShip != null)
                        {
                            if (!int.TryParse(QtyToShip.Text, out qty))
                            {
                                qty = 0;
                            }
                        }
                        p.Items.Add(new OrderPackageItem(li.ProductId, li.Id, qty));
                        p.Weight += (li.ProductShippingWeight * qty);
                    }
                }
            }
            p.WeightUnits = WebAppSettings.ApplicationWeightUnits;

            o.Packages.Add(p);            
            if (!dontShip)
            {
                MTApp.OrdersShipPackage(p, o);
            }
            MTApp.OrderServices.Orders.Update(o);

            return p;
        }

        protected void PackagesGridView_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {
            OrderPackage p = (OrderPackage)e.Row.DataItem;
            if (p != null)
            {


                string serviceCodeName = "Other";
                int trackerCode = 0;

                int.TryParse(p.ShippingProviderId, out trackerCode);

                TrackingProvider trackProvider = TrackingProvider.None;

                switch (trackerCode)
                {
                    case 1:
                        serviceCodeName = "UPS";
                        trackProvider = TrackingProvider.UPS;
                        break;
                    case 2:
                        serviceCodeName = "FedEx";
                        trackProvider = TrackingProvider.FedEx;
                        break;
                    case 3:
                        serviceCodeName = "US Postal Service";
                        trackProvider = TrackingProvider.USPostal;
                        break;
                }
                //IShippingService provider = MerchantTribe.Commerce.Shipping.AvailableServices.FindById(p.ShippingProviderId);


                //List<IServiceCode> codes = provider.ListAllServiceCodes();
                //foreach (IServiceCode item in codes) {
                //    if (item.Code == p.ShippingProviderServiceCode) {
                //        serviceCodeName = item.DisplayName;
                //    }
                //}

                Label ShippedByField = (Label)e.Row.FindControl("ShippedByField");
                if (ShippedByField != null)
                {
                    //if (provider != null) {
                    //    if (serviceCodeName != string.Empty) {
                    ShippedByField.Text = serviceCodeName;
                    //    }
                    //    else {
                    //        ShippedByField.Text = provider.Name;
                    //    }
                    //}
                }

                HyperLink TrackingLink = (HyperLink)e.Row.FindControl("TrackingLink");
                if (TrackingLink != null)
                {
                    TrackingLink.Text = p.TrackingNumber;
                    //if (provider != null) {
                    //    if (o != null && o.Bvin != string.Empty) {
                    //        //if (provider.SupportsTracking()) {
                    //        //    TrackingLink.Text = "Track Package";
                    //        //    TrackingLink.NavigateUrl = provider.GetTrackingUrl(p.TrackingNumber);
                    //        //}
                    //    }
                    //}
                }

                TextBox TrackingTextBox = (TextBox)e.Row.FindControl("TrackingNumberTextBox");
                if (TrackingTextBox != null)
                {
                    TrackingTextBox.Text = p.TrackingNumber;
                }

                Label pakdescription = (Label)e.Row.FindControl("pakdescription");
                if (pakdescription != null)
                {
                    pakdescription.Text = p.Description;
                }

                Label items = (Label)e.Row.FindControl("items");
                if (items != null)
                {
                    items.Text = string.Empty;
                    foreach (OrderPackageItem pi in p.Items)
                    {
                        if (pi.Quantity > 0)
                        {
                            items.Text += pi.Quantity.ToString("#") + " - ";
                            if (o != null)
                            {
                                foreach (LineItem li in o.Items)
                                {
                                    if (li.Id == pi.LineItemId)
                                    {
                                        items.Text += li.ProductSku + ": " + li.ProductName + "<br />";
                                    }
                                }
                            }
                        }
                    }
                }

            }
        }

        protected void PackagesGridView_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
        {
            Order order = MTApp.OrderServices.Orders.FindForCurrentStore(Request.QueryString["id"]);
            OrderShippingStatus previousShippingStatus = order.ShippingStatus;

            long Id = (long)PackagesGridView.DataKeys[e.RowIndex].Value;

            var p = order.Packages.Where(y => y.Id == Id).SingleOrDefault();
            if (p != null)
            {
                order.Packages.Remove(p);
                MTApp.OrderServices.Orders.Update(order);
            }
            ReloadOrder(previousShippingStatus);
        }

        //protected void SuggestedPackagesGridView_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        //{
        //    Package p = (Package)e.Row.DataItem;
        //    if (p != null)
        //    {

        //        IShippingService provider = MerchantTribe.Commerce.Shipping.AvailableServices.FindById(p.ShippingProviderId, Services.CurrentStore);

        //        string serviceCodeName = string.Empty;
        //        List<IServiceCode> codes = provider.ListAllServiceCodes();
        //        foreach (IServiceCode item in codes)
        //        {
        //            if (item.Code == p.ShippingProviderServiceCode)
        //            {
        //                serviceCodeName = item.DisplayName;
        //            }
        //        }

        //        Label ShippedByField = (Label)e.Row.FindControl("ShippedByField");
        //        if (ShippedByField != null)
        //        {
        //            if (provider != null)
        //            {
        //                if (serviceCodeName != string.Empty)
        //                {
        //                    ShippedByField.Text = serviceCodeName;
        //                }
        //                else
        //                {
        //                    ShippedByField.Text = provider.Name;
        //                }
        //            }
        //        }

        //        Label items = (Label)e.Row.FindControl("items");
        //        if (items != null)
        //        {
        //            items.Text = string.Empty;
        //            foreach (PackageItem pi in p.Items)
        //            {
        //                if (pi.Quantity > 0)
        //                {
        //                    items.Text += pi.Quantity.ToString("#") + " - ";
        //                    if (o != null)
        //                    {
        //                        foreach (LineItem li in o.Items)
        //                        {
        //                            if (li.Bvin == pi.LineItemBvin)
        //                            {
        //                                items.Text += li.ProductSku + "<br />";
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //        }

        //    }
        //}

        //protected void SuggestedPackagesGridView_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
        //{
        //    Order order = Order.FindByBvin(Request.QueryString["id"]);
        //    OrderShippingStatus previousShippingStatus = order.ShippingStatus;

        //    string bvin = (string)SuggestedPackagesGridView.DataKeys[e.RowIndex].Value;
        //    Package.Delete(bvin);
        //    ReloadOrder(previousShippingStatus);
        //}

        //protected void SuggestedPackagesGridView_RowEditing(object sender, System.Web.UI.WebControls.GridViewEditEventArgs e)
        //{
        //    Order order = Order.FindByBvin(Request.QueryString["id"]);
        //    OrderShippingStatus previousShippingStatus = order.ShippingStatus;

        //    string bvin = (string)this.SuggestedPackagesGridView.DataKeys[e.NewEditIndex].Value;
        //    Package p = Package.FindByBvin(bvin);
        //    if (p != null)
        //    {
        //        TextBox TrackingNumber = (TextBox)this.SuggestedPackagesGridView.Rows[e.NewEditIndex].FindControl("TrackingNumber");
        //        if (TrackingNumber != null)
        //        {
        //            p.TrackingNumber = TrackingNumber.Text.Trim();
        //        }
        //        p.Ship(Services.OrderServices, Services.ContactServices);
        //    }
        //    ReloadOrder(previousShippingStatus);
        //}

        protected void btnShipByUPS_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            //Order order = Order.FindByBvin(Request.QueryString["id"]);
            //OrderShippingStatus previousShippingStatus = order.ShippingStatus;

            //string serviceCode = string.Empty;
            //if (this.lstServiceCode.SelectedValue != null) {
            //    serviceCode = this.lstServiceCode.SelectedValue;
            //}

            //Package p;
            //if ((this.lstShippingProvider.SelectedValue == MerchantTribe.Shipping.Ups.UpsProvider.UPSProviderID) && (serviceCode != string.Empty)) {
            //    p = ShipItems(string.Empty, MerchantTribe.Shipping.Ups.UpsProvider.UPSProviderID, serviceCode, true);
            //}
            //else {
            //    p = ShipItems(string.Empty, 
            //                  MerchantTribe.Shipping.Ups.UpsProvider.UPSProviderID,
            //                  CurrentStore.ShippingUpsDefaultService.ToString(), 
            //                  true);
            //}

            //if (p.Bvin != string.Empty) {
            //    Response.Redirect("UpsOnlineTools_Ship.aspx?PackageId=" + p.Bvin + "&ReturnUrl=" + Server.UrlEncode(Page.ResolveUrl("~/BVAdmin/Orders/ShipOrder.aspx?id=" + p.OrderId)));
            //}
            //else {
            //    ReloadOrder(previousShippingStatus);
            //}
        }

        //protected void lstShippingProvider_SelectedIndexChanged(object sender, System.EventArgs e)
        //{
        //    LoadServiceCodes();
        //}

        //private void LoadServiceCodes()
        //{
        //    this.lstServiceCode.Items.Clear();

        //    IShippingService p = MerchantTribe.Commerce.Shipping.AvailableServices.FindById(this.lstShippingProvider.SelectedValue);
        //    if (p != null) {
        //        foreach (IServiceCode li in p.ListAllServiceCodes()) 
        //        {
        //            this.lstServiceCode.Items.Add(new System.Web.UI.WebControls.ListItem(li.DisplayName, li.Code));
        //        }
        //    }

        //    if (this.lstServiceCode.Items.Count > 0) {
        //        this.lstServiceCode.Visible = true;
        //    }
        //    else {
        //        this.lstServiceCode.Visible = false;
        //    }

        //    this.lstShippingProvider.UpdateAfterCallBack = true;
        //    this.lstServiceCode.UpdateAfterCallBack = true;
        //}

        protected void PackagesGridView_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            

            if (e.CommandName == "TrackingNumberUpdate")
            {
                if (e.CommandSource is System.Web.UI.Control)
                {

                    Order o = MTApp.OrderServices.Orders.FindForCurrentStore(Request.QueryString["id"]);

                    GridViewRow row = (GridViewRow)((System.Web.UI.Control)e.CommandSource).NamingContainer;
                    TextBox trackingNumberTextBox = (TextBox)row.FindControl("TrackingNumberTextBox");
                    if (trackingNumberTextBox != null)
                    {
                        string idstring = (string)e.CommandArgument;
                        long pid = 0;
                        long.TryParse(idstring, out pid);

                        OrderPackage package = o.Packages.Where(y => y.Id == pid).SingleOrDefault();
                        if (package != null)
                        {
                            package.TrackingNumber = trackingNumberTextBox.Text;
                            MTApp.OrderServices.Orders.Update(o);
                        }
                    }
                }
            }
            LoadOrder();
        }

        protected void SuggestedPackagesGridView_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            if (e.CommandName == "AddTrackingNumber")
            {
                if (e.CommandSource is System.Web.UI.Control)
                {
                    Order o = MTApp.OrderServices.Orders.FindForCurrentStore(Request.QueryString["id"]);

                    GridViewRow row = (GridViewRow)((System.Web.UI.Control)e.CommandSource).NamingContainer;
                    TextBox trackingNumberTextBox = (TextBox)row.FindControl("TrackingNumber");
                    if (trackingNumberTextBox != null)
                    {
                        OrderPackage package = o.Packages.Where(y => y.Id == (long)e.CommandArgument).SingleOrDefault();
                        if (package != null)
                        {
                            package.TrackingNumber = trackingNumberTextBox.Text;
                            MTApp.OrderServices.Orders.Update(o);
                        }
                    }
                }
            }
        }


    }
}