using System;
using System.Collections.Generic;
using System.Web.UI;
using MerchantTribe.Commerce;

namespace MerchantTribeStore
{

    public partial class BVAdmin_SetupWizard_WizardPayment : BaseAdminPage
    {
        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.PageTitle = "Setup Wizard | Payment Methods";
            this.CurrentTab = AdminTabType.Configuration;
        }

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);
            if (!Page.IsPostBack)
            {
                this.PayPalFastSignupEmail.Text = MTApp.CurrentStore.Settings.MailServer.EmailForNewOrder;
                if (MTApp.CurrentStore.Settings.IsPayPalLead || MTApp.CurrentStore.PlanId == 0)
                {
                    this.pnlMain.Visible = false;
                    this.pnlPayPay.Visible = true;
                }
                else
                {
                    this.pnlMain.Visible = true;
                    this.pnlPayPay.Visible = false;
                }
            }
        }


        protected void btnSaveMain_Click(object sender, ImageClickEventArgs e)
        {
            Dictionary<string, string> newList = new Dictionary<string, string>();

            if (this.pnlMain.Visible)
            {
                if (chkCheck.Checked) newList.Add(WebAppSettings.PaymentIdCheck, string.Empty);
                if (chkCOD.Checked) newList.Add(WebAppSettings.PaymentIdCashOnDelivery, string.Empty);
                if (chkCreditCard.Checked) newList.Add(WebAppSettings.PaymentIdCreditCard, string.Empty);
                if (chkPurchaseOrder.Checked) newList.Add(WebAppSettings.PaymentIdPurchaseOrder, string.Empty);
                if (chkTelephone.Checked) newList.Add(WebAppSettings.PaymentIdTelephone, string.Empty);
            }
            else
            {
                if (chkPayPalCreditCards.Checked) newList.Add(WebAppSettings.PaymentIdCreditCard, string.Empty);
                //newList.Add(WebAppSettings.PaymentIdCreditCard, string.Empty);
            }
            if (chkPayPalExpress.Checked) newList.Add(WebAppSettings.PaymentIdPaypalExpress, string.Empty);


            MTApp.CurrentStore.Settings.PaymentMethodsEnabled = newList;
            SavePayPalInfo();
            MTApp.UpdateCurrentStore();
            Response.Redirect("WizardSocialMedia.aspx");
        }

        private void SavePayPalInfo()
        {
            MTApp.CurrentStore.Settings.PayPal.FastSignupEmail = this.PayPalFastSignupEmail.Text.Trim();
            if (this.btnSlowSignup.Checked)
            {
                MTApp.CurrentStore.Settings.PayPal.UserName = this.APIUsername.Text;
                MTApp.CurrentStore.Settings.PayPal.Password = this.APIPassword.Text;
                MTApp.CurrentStore.Settings.PayPal.Signature = this.APISignature.Text;
            }
            MTApp.CurrentStore.Settings.PayPal.Mode = this.ModeRadioButtonList.SelectedValue;
            MTApp.CurrentStore.Settings.PayPal.ExpressAuthorizeOnly = false;
            MTApp.CurrentStore.Settings.PayPal.AllowUnconfirmedAddresses = true;
            MTApp.CurrentStore.Settings.PayPal.Currency = "USD";
        }

        protected void btnFastSignup_CheckedChanged(object sender, EventArgs e)
        {
            EnableApi();
        }
        protected void btnSlowSignup_CheckedChanged(object sender, EventArgs e)
        {
            EnableApi();
        }

        private void EnableApi()
        {
            this.pnlApi.Visible = this.btnSlowSignup.Checked;
        }
    }
}