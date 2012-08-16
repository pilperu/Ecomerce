using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using MerchantTribe.Commerce.Content;
using MerchantTribe.Commerce.Shipping;
using MerchantTribe.Commerce;
using MerchantTribe.Commerce.Utilities;
using MerchantTribe.Shipping;
using MerchantTribe.Shipping.Services;
using System.Collections.Generic;

namespace MerchantTribeStore
{

    partial class BVModules_Shipping_By_Order_Total_edit : BVShippingModule
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
                LoadLevels();
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
            //this.NameField.Text = SettingsManager.GetSetting("Name");
            if (this.NameField.Text == string.Empty)
            {
                this.NameField.Text = "Rate Table By Total Price";
            }


            AdjustmentDropDownList.SelectedValue = ((int)ShippingMethod.AdjustmentType).ToString();
            if (ShippingMethod.AdjustmentType == ShippingMethodAdjustmentType.Amount)
            {
                AdjustmentTextBox.Text = string.Format("{0:c}", ShippingMethod.Adjustment);
            }
            else
            {
                AdjustmentTextBox.Text = string.Format("{0:f}", ShippingMethod.Adjustment);
            }

            // ZONES
            if (this.lstZones.Items.FindByValue(ShippingMethod.ZoneId.ToString()) != null)
            {
                this.lstZones.ClearSelection();
                this.lstZones.Items.FindByValue(ShippingMethod.ZoneId.ToString()).Selected = true;
            }
        }

        private void LoadLevels()
        {
            RateTableSettings settings = new RateTableSettings();
            settings.Merge(ShippingMethod.Settings);

            List<RateTableLevel> levels = settings.GetLevels();
            this.GridView1.DataSource = levels;
            this.GridView1.DataBind();
        }

        private void SaveData()
        {
            ShippingMethod.Name = this.NameField.Text.Trim();
            ShippingMethod.AdjustmentType = (ShippingMethodAdjustmentType)int.Parse(AdjustmentDropDownList.SelectedValue);
            ShippingMethod.Adjustment = decimal.Parse(AdjustmentTextBox.Text, System.Globalization.NumberStyles.Currency);
            ShippingMethod.ZoneId = long.Parse(this.lstZones.SelectedItem.Value);
        }

        protected void btnNew_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            RateTableLevel r = new RateTableLevel();
            r.Level = decimal.Parse(this.NewLevelField.Text);
            r.Rate = decimal.Parse(this.NewAmountField.Text);


            RateTableSettings settings = new RateTableSettings();
            settings.Merge(ShippingMethod.Settings);

            settings.AddLevel(r);

            ShippingMethod.Settings = settings;
            MyPage.MTApp.OrderServices.ShippingMethods.Update(ShippingMethod);
            LoadLevels();
        }


        private void RemoveLevel(string level, string rate)
        {
            RateTableSettings settings = new RateTableSettings();
            settings.Merge(ShippingMethod.Settings);

            RateTableLevel r = new RateTableLevel();
            r.Level = decimal.Parse(level);
            r.Rate = decimal.Parse(rate, System.Globalization.NumberStyles.Currency);

            settings.RemoveLevel(r);

            ShippingMethod.Settings = settings;
            MyPage.MTApp.OrderServices.ShippingMethods.Update(ShippingMethod);
            LoadLevels();
        }

        protected void GridView1_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
        {
            Label lblLevel = (Label)GridView1.Rows[e.RowIndex].FindControl("lblLevel");
            Label lblRate = (Label)GridView1.Rows[e.RowIndex].FindControl("lblAmount");
            if (lblLevel != null)
            {
                if (lblRate != null)
                {
                    RemoveLevel(lblLevel.Text, lblRate.Text);
                }
            }
        }

        protected void CustomValidator1_ServerValidate(object source, System.Web.UI.WebControls.ServerValidateEventArgs args)
        {
            decimal val = 0m;
            if (decimal.TryParse(AdjustmentTextBox.Text, System.Globalization.NumberStyles.Currency, System.Globalization.CultureInfo.CurrentCulture, out val))
            {
                args.IsValid = true;
            }
            else
            {
                args.IsValid = false;
            }
        }
    }
}