using System;
using System.Web.UI.WebControls;
using MerchantTribe.Commerce;
using MerchantTribe.Commerce.Content;
using MerchantTribe.Commerce.Payment;
using MerchantTribe.Payment.Methods;

namespace MerchantTribeStore
{

    partial class BVModules_CreditCardGateways_BV_Test_Gateway_edit : BVModule
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

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (!Page.IsPostBack)
            {
                LoadData();
            }
        }

        private void LoadData()
        {
            TestGatewaySettings settings = new TestGatewaySettings();
            settings.Merge(MyPage.MTApp.CurrentStore.Settings.PaymentSettingsGet(this.BlockId));

            this.chkAuthorizeFails.Checked = !settings.ResponseForHold;
            this.chkCaptureFails.Checked = !settings.ResponseForCapture;
            this.chkChargeFails.Checked = !settings.ResponseForCharge;
            this.chkRefundFails.Checked = !settings.ResponseForRefund;
            this.chkVoidFails.Checked = !settings.ResponseForVoid;
        }

        private void SaveData()
        {
            TestGatewaySettings settings = new TestGatewaySettings();
            settings.Merge(MyPage.MTApp.CurrentStore.Settings.PaymentSettingsGet(this.BlockId));

            settings.ResponseForCapture = !this.chkCaptureFails.Checked;
            settings.ResponseForCharge = !this.chkChargeFails.Checked;
            settings.ResponseForHold = !this.chkAuthorizeFails.Checked;
            settings.ResponseForRefund = !this.chkRefundFails.Checked;
            settings.ResponseForVoid = !this.chkVoidFails.Checked;

            MyPage.MTApp.CurrentStore.Settings.PaymentSettingsSet(this.BlockId, settings);

            MyPage.MTApp.AccountServices.Stores.Update(MyPage.MTApp.CurrentStore);
        }

    }
}