using System;
using System.Web;
using System.Web.UI.WebControls;
using System.Text;
using MerchantTribe.Commerce;
using MerchantTribe.Commerce.Accounts;
using MerchantTribe.Commerce.BusinessRules;
using MerchantTribe.Commerce.Catalog;
using MerchantTribe.Commerce.Contacts;
using MerchantTribe.Commerce.Content;
using MerchantTribe.Commerce.Membership;
using MerchantTribe.Commerce.Metrics;
using MerchantTribe.Commerce.Orders;
using MerchantTribe.Commerce.Payment;
using MerchantTribe.Commerce.Shipping;
using MerchantTribe.Commerce.Taxes;
using MerchantTribe.Commerce.Utilities;
using System.Collections.Generic;

using System.Collections.ObjectModel;

namespace MerchantTribeStore
{

    partial class BVAdmin_Configuration_Shipping : BaseAdminPage
    {

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.PageTitle = "Shipping Settings";
            this.CurrentTab = AdminTabType.Configuration;
            ValidateCurrentUserHasPermission(SystemPermissions.SettingsView);
        }

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);

            if (!Page.IsPostBack)
            {
                LoadMethods();
                LoadProviders();
            }
        }

        private void LoadMethods()
        {
            List<ShippingMethod> methods;
            methods = MTApp.OrderServices.ShippingMethods.FindAll(MTApp.CurrentStore.Id);
            this.GridView1.DataSource = methods;
            this.GridView1.DataBind();
        }

        private void LoadProviders()
        {
            this.lstProviders.ClearSelection();
            foreach (MerchantTribe.Shipping.IShippingService p in MerchantTribe.Commerce.Shipping.AvailableServices.FindAll(this.MTApp.CurrentStore))
            {
                this.lstProviders.Items.Add(new System.Web.UI.WebControls.ListItem(p.Name, p.Id));
            }
        }

        private void NewMethod()
        {
            ShippingMethod m = new ShippingMethod();
            m.ShippingProviderId = this.lstProviders.SelectedValue;
            m.Name = "New Shipping Method";
            if (MTApp.OrderServices.ShippingMethods.Create(m) == true)
            {
                Response.Redirect("Shipping_EditMethod.aspx?id=" + m.Bvin + "&doc=1");
            }
        }

        protected void GridView1_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
        {
            string bvin = (string)GridView1.DataKeys[e.RowIndex].Value;
            MTApp.OrderServices.ShippingMethods.Delete(bvin);
            LoadMethods();
        }

        protected void GridView1_RowEditing(object sender, System.Web.UI.WebControls.GridViewEditEventArgs e)
        {
            string bvin = (string)GridView1.DataKeys[e.NewEditIndex].Value;
            Response.Redirect("Shipping_EditMethod.aspx?id=" + bvin);
        }

        protected void btnNew_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            NewMethod();
        }

    }
}