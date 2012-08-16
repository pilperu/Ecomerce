
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

namespace MerchantTribeStore
{

    partial class BVAdmin_Configuration_Orders : BaseAdminPage
    {

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.PageTitle = "Order Settings";
            this.CurrentTab = AdminTabType.Configuration;
            ValidateCurrentUserHasPermission(SystemPermissions.SettingsView);
        }

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);

            if (!Page.IsPostBack)
            {
                this.OrderLimiteQuantityField.Text = MTApp.CurrentStore.Settings.MaxItemsPerOrder.ToString();
                this.OrderLimitWeightField.Text = MTApp.CurrentStore.Settings.MaxWeightPerOrder.ToString();
                this.OrderLimitErrorMessage.Text = MTApp.CurrentStore.Settings.MaxOrderMessage;
                this.ZeroDollarOrdersCheckBox.Checked = MTApp.CurrentStore.Settings.AllowZeroDollarOrders;
                this.LastOrderNumberField.Text = MTApp.CurrentStore.Settings.LastOrderNumber.ToString();
                this.ForceSiteTermsCheckBox.Checked = MTApp.CurrentStore.Settings.ForceTermsAgreement;
                this.chkRejectFailedCC.Checked = MTApp.CurrentStore.Settings.RejectFailedCreditCardOrdersAutomatically;
            }
        }

        protected void btnCancel_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            Response.Redirect("default.aspx");
        }

        protected void btnSave_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            this.MessageBox1.ClearMessage();
            if (this.Save() == true)
            {
                this.MessageBox1.ShowOk("Settings saved successfully.");
            }
        }

        private bool Save()
        {
            MTApp.CurrentStore.Settings.MaxItemsPerOrder = int.Parse(this.OrderLimiteQuantityField.Text.Trim());
            MTApp.CurrentStore.Settings.MaxWeightPerOrder = decimal.Parse(this.OrderLimitWeightField.Text.Trim());
            MTApp.CurrentStore.Settings.MaxOrderMessage = this.OrderLimitErrorMessage.Text.Trim();
            MTApp.CurrentStore.Settings.AllowZeroDollarOrders = this.ZeroDollarOrdersCheckBox.Checked;
            MTApp.CurrentStore.Settings.LastOrderNumber = int.Parse(this.LastOrderNumberField.Text.Trim());
            MTApp.CurrentStore.Settings.ForceTermsAgreement = this.ForceSiteTermsCheckBox.Checked;
            MTApp.CurrentStore.Settings.RejectFailedCreditCardOrdersAutomatically = this.chkRejectFailedCC.Checked;

            return MTApp.UpdateCurrentStore();
        }

    }
}