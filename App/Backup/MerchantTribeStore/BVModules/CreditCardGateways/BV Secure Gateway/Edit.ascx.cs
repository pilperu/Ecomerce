using System;
using System.Web.UI.WebControls;
using MerchantTribe.Commerce;
using MerchantTribe.Commerce.Content;
using MerchantTribe.Commerce.Payment;
using MerchantTribe.Payment.Methods;

namespace MerchantTribeStore
{

    public partial class BVModules_CreditCardGateways_BV_Secure_Gateway_Edit : BVModule
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
            PayLeapSettings settings = new PayLeapSettings();
            settings.Merge(MyPage.MTApp.CurrentStore.Settings.PaymentSettingsGet(this.BlockId));

            this.UsernameField.Text = settings.Username;

            if (settings.Password.Length > 0)
            {
                this.PasswordField.Text = "************";
            }
            //this.PasswordField.Text = settings.Password;

            this.chkTestMode.Checked = settings.TrainingMode;
            this.chkDebugMode.Checked = settings.DeveloperMode;
            this.chkEnableTracing.Checked = settings.EnableDebugTracing;
        }

        private void SaveData()
        {
            PayLeapSettings settings = new PayLeapSettings();
            settings.Merge(MyPage.MTApp.CurrentStore.Settings.PaymentSettingsGet(this.BlockId));

            settings.Username = this.UsernameField.Text.Trim();
            if (this.PasswordField.Text != "************")
            {
                settings.Password = this.PasswordField.Text.Trim();
            }
            settings.TrainingMode = this.chkTestMode.Checked;
            settings.EnableDebugTracing = this.chkEnableTracing.Checked;
            settings.DeveloperMode = this.chkDebugMode.Checked;

            MyPage.MTApp.CurrentStore.Settings.PaymentSettingsSet(this.BlockId, settings);

            MyPage.MTApp.AccountServices.Stores.Update(MyPage.MTApp.CurrentStore);
        }

    }
}