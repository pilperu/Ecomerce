using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using MerchantTribe.Commerce.Content;
using MerchantTribe.Commerce.Shipping;
using MerchantTribe.Commerce;
using MerchantTribe.Shipping.Services;

namespace MerchantTribeStore
{

    partial class BVModules_Shipping_Per_Order_edit : BVShippingModule
    {

        protected void btnCancel_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            this.NotifyFinishedEditing("Canceled");
        }

        protected void btnSave_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            SaveData();
            this.NotifyFinishedEditing(this.NameField.Text.Trim());
        }

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);
            if (!Page.IsPostBack)
            {
                LoadZones();
                LoadData();
            }
        }

        private void LoadZones()
        {
            this.lstZones.DataSource = MyPage.MTApp.OrderServices.ShippingZones.FindForStore(MyPage.MTApp.CurrentStore.Id);
            this.lstZones.DataTextField = "Name";
            this.lstZones.DataValueField = "id";
            this.lstZones.DataBind();
        }

        private void LoadData()
        {

            this.NameField.Text = ShippingMethod.Name;
            if (this.NameField.Text == string.Empty)
            {
                this.NameField.Text = "Flat Rate Per Order";
            }

            FlatRatePerOrderSettings Settings = new FlatRatePerOrderSettings();
            Settings.Merge(ShippingMethod.Settings);
            if (Settings.Amount < 0) Settings.Amount = 0;
            this.AmountField.Text = Settings.Amount.ToString("C");

            // ZONES
            if (this.lstZones.Items.FindByValue(ShippingMethod.ZoneId.ToString()) != null)
            {
                this.lstZones.ClearSelection();
                this.lstZones.Items.FindByValue(ShippingMethod.ZoneId.ToString()).Selected = true;
            }
        }

        private void SaveData()
        {
            decimal amount = decimal.Parse(AmountField.Text, System.Globalization.NumberStyles.Currency);
            FlatRatePerOrderSettings Settings = new FlatRatePerOrderSettings();
            Settings.Merge(ShippingMethod.Settings);
            Settings.Amount = amount;
            ShippingMethod.Settings.Merge(Settings);

            ShippingMethod.Name = this.NameField.Text.Trim();
            ShippingMethod.AdjustmentType = ShippingMethodAdjustmentType.Amount;
            ShippingMethod.Adjustment = 0;
            ShippingMethod.ZoneId = long.Parse(this.lstZones.SelectedItem.Value);
        }

    }
}