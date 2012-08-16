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
using System.Collections.ObjectModel;

namespace MerchantTribeStore
{

    public partial class BVAdmin_Configuration_ShippingHandling : BaseAdminPage
    {

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.PageTitle = "Shipping | Handling Settings";
            this.CurrentTab = AdminTabType.Configuration;
            ValidateCurrentUserHasPermission(SystemPermissions.SettingsView);
        }

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);

            if (!Page.IsPostBack)
            {
                LoadHandlingSettings();
            }
        }

        private void LoadHandlingSettings()
        {
            this.HandlingFeeAmountTextBox.Text = MTApp.CurrentStore.Settings.HandlingAmount.ToString("c");
            this.HandlingRadioButtonList.SelectedIndex = MTApp.CurrentStore.Settings.HandlingType;
            this.NonShippingCheckBox.Checked = MTApp.CurrentStore.Settings.HandlingNonShipping;
        }

        protected void HandlingFeeAmountCustomValidator_ServerValidate(object source, System.Web.UI.WebControls.ServerValidateEventArgs args)
        {
            decimal temp;
            if (decimal.TryParse(args.Value, System.Globalization.NumberStyles.Currency,
                System.Threading.Thread.CurrentThread.CurrentUICulture, out temp))
            {
                args.IsValid = true;
            }
            else
            {
                args.IsValid = false;
            }
        }

        protected void CancelImageButton_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            Response.Redirect("~/BVAdmin/Configuration/default.aspx");
        }

        protected void SaveImageButton_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            MTApp.CurrentStore.Settings.HandlingAmount = decimal.Parse(this.HandlingFeeAmountTextBox.Text, System.Globalization.NumberStyles.Currency);
            MTApp.CurrentStore.Settings.HandlingType = this.HandlingRadioButtonList.SelectedIndex;
            MTApp.CurrentStore.Settings.HandlingNonShipping = this.NonShippingCheckBox.Checked;
            MTApp.UpdateCurrentStore();

            this.MessageBox1.ShowOk("Settings saved successfully.");
        }
    }
}