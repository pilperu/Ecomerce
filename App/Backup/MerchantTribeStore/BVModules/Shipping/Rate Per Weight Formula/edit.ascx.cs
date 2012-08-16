using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using MerchantTribe.Commerce.Content;
using MerchantTribe.Commerce.Shipping;
using MerchantTribe.Commerce;
using MerchantTribe.Shipping.Services;


namespace MerchantTribeStore.BVModules.Shipping.Rate_Per_Weight_Formula
{
    public partial class edit : BVShippingModule
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
                this.NameField.Text = "Rate Per Weight Formula";
            }

            RatePerWeightFormulaSettings Settings = new RatePerWeightFormulaSettings();
            Settings.Merge(ShippingMethod.Settings);
            if (Settings.BaseAmount < 0) Settings.BaseAmount = 0;
            if (Settings.BaseWeight < 0) Settings.BaseWeight = 0;
            if (Settings.MinWeight < 0) Settings.MinWeight = 0;
            if (Settings.MaxWeight < 0) Settings.MaxWeight = 9999;
            if (Settings.AdditionalWeightCharge < 0) Settings.AdditionalWeightCharge = 0;


            this.BaseAmountField.Text = Settings.BaseAmount.ToString("C");
            this.AdditionalWeightChargeField.Text = Settings.AdditionalWeightCharge.ToString("C");
            this.BaseWeightField.Text = Settings.BaseWeight.ToString();
            this.MinWeightField.Text = Settings.MinWeight.ToString();
            this.MaxWeightField.Text = Settings.MaxWeight.ToString();

            

            // ZONES
            if (this.lstZones.Items.FindByValue(ShippingMethod.ZoneId.ToString()) != null)
            {
                this.lstZones.ClearSelection();
                this.lstZones.Items.FindByValue(ShippingMethod.ZoneId.ToString()).Selected = true;
            }
        }

        private void SaveData()
        {            
            decimal baseAmount = decimal.Parse(BaseAmountField.Text, System.Globalization.NumberStyles.Currency);
            decimal additionalWeightCharge = decimal.Parse(AdditionalWeightChargeField.Text, System.Globalization.NumberStyles.Currency);
            decimal baseWeight = decimal.Parse(BaseWeightField.Text);
            decimal minWeight = decimal.Parse(MinWeightField.Text);
            decimal maxWeight = decimal.Parse(MaxWeightField.Text);

            RatePerWeightFormulaSettings Settings = new RatePerWeightFormulaSettings();
            Settings.Merge(ShippingMethod.Settings);

            Settings.BaseAmount = baseAmount;
            Settings.AdditionalWeightCharge = additionalWeightCharge;
            Settings.BaseWeight = baseWeight;
            Settings.MinWeight = minWeight;
            Settings.MaxWeight = maxWeight;

            ShippingMethod.Settings.Merge(Settings);

            ShippingMethod.Name = this.NameField.Text.Trim();
            ShippingMethod.AdjustmentType = ShippingMethodAdjustmentType.Amount;
            ShippingMethod.Adjustment = 0;
            ShippingMethod.ZoneId = long.Parse(this.lstZones.SelectedItem.Value);
        }

    }
}