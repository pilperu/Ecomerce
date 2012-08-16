using System;
using System.Web.UI.WebControls;
using MerchantTribe.Commerce;
using MerchantTribe.Commerce.Content;
using MerchantTribe.Commerce.Payment;
using MerchantTribe.Payment.Methods;

namespace MerchantTribeStore
{

    partial class BVModules_CreditCardGateways_PayPayPaymentsPro_Edit : BVModule
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
            PayPalPaymentsProSettings settings = new PayPalPaymentsProSettings();
            settings.Merge(MyPage.MTApp.CurrentStore.Settings.PaymentSettingsGet(this.BlockId));

            this.UsernameTextBox.Text = settings.PayPalUserName;
            if (settings.PayPalPassword.Length > 0)
            {
                this.PasswordTextBox.Text = "**********";
            }
            //this.PasswordTextBox.Text = settings.PayPalPassword;
            this.SignatureTextBox.Text = settings.PayPalSignature;
            this.ModeRadioButtonList.SelectedValue = settings.PayPalMode;

            this.chkDebugMode.Checked = settings.DebugMode;
            //this.UsernameTextBox.Text = MyPage.CurrentStore.PaypalUserName;
            //this.PasswordTextBox.Text = MyPage.CurrentStore.PaypalPassword;
            //this.SignatureTextBox.Text = MyPage.CurrentStore.PaypalSignature;
            //this.ModeRadioButtonList.SelectedValue = MyPage.CurrentStore.PaypalMode;
        }

        private void SaveData()
        {
            PayPalPaymentsProSettings settings = new PayPalPaymentsProSettings();
            settings.Merge(MyPage.MTApp.CurrentStore.Settings.PaymentSettingsGet(this.BlockId));

            settings.PayPalUserName = this.UsernameTextBox.Text;
            if (this.PasswordTextBox.Text != "**********")
            {
                settings.PayPalPassword = this.PasswordTextBox.Text;
            }
            settings.PayPalSignature = this.SignatureTextBox.Text;
            settings.PayPalMode = this.ModeRadioButtonList.SelectedValue;

            //MyPage.Services.CurrentStore.PaypalUserName = this.UsernameTextBox.Text;
            //MyPage.Services.CurrentStore.PaypalPassword = this.PasswordTextBox.Text;
            //MyPage.Services.CurrentStore.PaypalSignature = this.SignatureTextBox.Text;
            //MyPage.Services.CurrentStore.PaypalMode = this.ModeRadioButtonList.SelectedValue;
            settings.DebugMode = this.chkDebugMode.Checked;

            MyPage.MTApp.CurrentStore.Settings.PaymentSettingsSet(this.BlockId, settings);

            MyPage.MTApp.AccountServices.Stores.Update(MyPage.MTApp.CurrentStore);
        }

    }
}