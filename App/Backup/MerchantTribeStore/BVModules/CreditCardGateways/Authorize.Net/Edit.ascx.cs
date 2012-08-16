using System;
using System.Web.UI.WebControls;
using MerchantTribe.Commerce;
using MerchantTribe.Commerce.Content;
using MerchantTribe.Commerce.Payment;
using MerchantTribe.Payment.Methods;

namespace MerchantTribeStore
{

    partial class BVModules_CreditCardGateways_Authorize_Net_Edit : BVModule
    {

        protected void btnCancel_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            this.NotifyFinishedEditing();
        }

        protected void btnSave_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            SaveData();
            this.NotifyFinishedEditing();
        }

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);
            if (!Page.IsPostBack)
            {
                LoadData();
            }
        }

        private void LoadData()
        {
            AuthorizeNetSettings settings = new AuthorizeNetSettings();
            settings.Merge(MyPage.MTApp.CurrentStore.Settings.PaymentSettingsGet(this.BlockId));

            this.UsernameField.Text = settings.MerchantLoginId;
            this.PasswordField.Text = settings.TransactionKey;
            this.chkTestMode.Checked = settings.TestMode;
            this.chkDebugMode.Checked = settings.DeveloperMode;
            this.EmailCustomerCheckBox.Checked = settings.SendEmailToCustomer;
        }

        private void SaveData()
        {
            AuthorizeNetSettings settings = new AuthorizeNetSettings();
            settings.Merge(MyPage.MTApp.CurrentStore.Settings.PaymentSettingsGet(this.BlockId));

            settings.MerchantLoginId = this.UsernameField.Text.Trim();
            settings.TransactionKey = this.PasswordField.Text.Trim();
            settings.TestMode = this.chkTestMode.Checked;
            settings.SendEmailToCustomer = this.EmailCustomerCheckBox.Checked;
            settings.DeveloperMode = this.chkDebugMode.Checked;

            MyPage.MTApp.CurrentStore.Settings.PaymentSettingsSet(this.BlockId, settings);

            MyPage.MTApp.AccountServices.Stores.Update(MyPage.MTApp.CurrentStore);
        }

    }
}