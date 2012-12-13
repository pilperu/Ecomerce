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

namespace MerchantTribeStore.BVAdmin.Configuration
{
    public partial class Avatax : BaseAdminPage
    {

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.PageTitle = "AvaTax";
            this.CurrentTab = AdminTabType.Configuration;
            ValidateCurrentUserHasPermission(SystemPermissions.SettingsView);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (!Page.IsPostBack)
            {                
                var settings = MTApp.CurrentStore.Settings;

                if (settings.Avalara.IsModuleActive == false)
                {
                    this.pnlMain.Visible = false;
                    this.msg.ShowInformation("The Avatax Module is not enabled for your store. Contact BV Software for more information on activating this module");
                }

                this.EnableCheckBox.Checked =  settings.Avalara.Enabled;
                this.AccountTextBox.Text = settings.Avalara.Account;
                this.LicenseKeyTextBox.Text = settings.Avalara.LicenseKey;
                this.CompanyCodeTextBox.Text = settings.Avalara.CompanyCode;
                this.UrlTextBox.Text = settings.Avalara.ServiceUrl;
                this.DebugCheckBox.Checked = settings.Avalara.DebugMode;                
                this.UserNameField.Text = settings.Avalara.Username;
                this.PasswordField.Text = settings.Avalara.Password;
            }

        }

        protected void btnCancel_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            Response.Redirect("Default.aspx");
        }

        protected void btnSave_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            if (this.Save())
            {
                this.msg.ShowOk("Settings Saved at " + DateTime.Now);
            }
        }

        private bool Save()
        {
            var settings = this.MTApp.CurrentStore.Settings;

            settings.Avalara.Enabled = this.EnableCheckBox.Checked;
            settings.Avalara.Account = this.AccountTextBox.Text;
            settings.Avalara.LicenseKey = this.LicenseKeyTextBox.Text;
            settings.Avalara.CompanyCode = this.CompanyCodeTextBox.Text;
            settings.Avalara.ServiceUrl = this.UrlTextBox.Text;
            settings.Avalara.DebugMode = this.DebugCheckBox.Checked;        
            settings.Avalara.Username = this.UserNameField.Text.Trim();
            settings.Avalara.Password = this.PasswordField.Text.Trim();

            // Save Settings 
            MTApp.AccountServices.Stores.Update(MTApp.CurrentStore);

            return true;
        }


 


    }
}