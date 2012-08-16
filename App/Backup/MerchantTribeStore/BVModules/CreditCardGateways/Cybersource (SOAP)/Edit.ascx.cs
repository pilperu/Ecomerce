using System;
using System.Web.UI.WebControls;
using MerchantTribe.Commerce;
using MerchantTribe.Commerce.Content;
using MerchantTribe.Commerce.Payment;
using MerchantTribe.Payment.Methods;

namespace MerchantTribeStore.BVModules.CreditCardGateways.CybersourceSOAP
{
    public partial class Edit : BVModule
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
            CybersourceSOAPSettings settings = new CybersourceSOAPSettings();
            settings.Merge(MyPage.MTApp.CurrentStore.Settings.PaymentSettingsGet(this.BlockId));

            this.MerchantIdField.Text = settings.MerchantId;
            this.TransactionKeyField.Text = settings.TransactionKey;
        }

        private void SaveData()
        {
            CybersourceSOAPSettings settings = new CybersourceSOAPSettings();
            settings.Merge(MyPage.MTApp.CurrentStore.Settings.PaymentSettingsGet(this.BlockId));

            settings.MerchantId = this.MerchantIdField.Text.Trim();
            settings.TransactionKey = this.TransactionKeyField.Text.Trim();

            MyPage.MTApp.CurrentStore.Settings.PaymentSettingsSet(this.BlockId, settings);

            MyPage.MTApp.AccountServices.Stores.Update(MyPage.MTApp.CurrentStore);
        }

    }
}