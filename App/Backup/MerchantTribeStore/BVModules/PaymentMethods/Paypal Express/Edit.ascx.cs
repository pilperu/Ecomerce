using System;
using System.Web.UI.WebControls;
using MerchantTribe.Commerce;
using MerchantTribe.Commerce.Content;
using MerchantTribe.Commerce.Payment;

namespace MerchantTribeStore
{

    partial class BVModules_PaymentMethods_Paypal_Express_Edit : BVModule
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
            this.PayPalFastSignupEmail.Text = MyPage.MTApp.CurrentStore.Settings.PayPal.FastSignupEmail;
            if (MyPage.MTApp.CurrentStore.Settings.PayPal.UserName.Trim().Length < 1)
            {
                this.btnFastSignup.Checked = true;
                this.btnSlowSignup.Checked = false;
            }
            else
            {
                this.btnFastSignup.Checked = false;
                this.btnSlowSignup.Checked = true;
            }
            this.UsernameTextBox.Text = MyPage.MTApp.CurrentStore.Settings.PayPal.UserName;
            this.PasswordTextBox.Text = MyPage.MTApp.CurrentStore.Settings.PayPal.Password;
            this.SignatureTextBox.Text = MyPage.MTApp.CurrentStore.Settings.PayPal.Signature;
            this.ModeRadioButtonList.SelectedValue = MyPage.MTApp.CurrentStore.Settings.PayPal.Mode;
            this.lstCaptureMode.ClearSelection();
            if (MyPage.MTApp.CurrentStore.Settings.PayPal.ExpressAuthorizeOnly)
            {
                this.lstCaptureMode.SelectedValue = "1";
            }
            else
            {
                this.lstCaptureMode.SelectedValue = "0";
            }

            this.UnconfirmedAddressCheckBox.Checked = MyPage.MTApp.CurrentStore.Settings.PayPal.AllowUnconfirmedAddresses;
            this.PaypalMonetaryFormatRadioButtonList.SelectedValue = MyPage.MTApp.CurrentStore.Settings.PayPal.Currency;
        }

        private void SaveData()
        {
            MyPage.MTApp.CurrentStore.Settings.PayPal.FastSignupEmail = this.PayPalFastSignupEmail.Text.Trim();
            MyPage.MTApp.CurrentStore.Settings.PayPal.UserName = this.UsernameTextBox.Text;
            MyPage.MTApp.CurrentStore.Settings.PayPal.Password = this.PasswordTextBox.Text;
            MyPage.MTApp.CurrentStore.Settings.PayPal.Signature = this.SignatureTextBox.Text;
            MyPage.MTApp.CurrentStore.Settings.PayPal.Mode = this.ModeRadioButtonList.SelectedValue;
            MyPage.MTApp.CurrentStore.Settings.PayPal.ExpressAuthorizeOnly = (this.lstCaptureMode.SelectedValue == "1");
            MyPage.MTApp.CurrentStore.Settings.PayPal.AllowUnconfirmedAddresses = this.UnconfirmedAddressCheckBox.Checked;
            MyPage.MTApp.CurrentStore.Settings.PayPal.Currency = this.PaypalMonetaryFormatRadioButtonList.SelectedValue;

            MyPage.MTApp.AccountServices.Stores.Update(MyPage.MTApp.CurrentStore);

        }

    }
}