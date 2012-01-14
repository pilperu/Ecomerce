using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using MerchantTribe.Commerce.Content;
using MerchantTribe.Commerce.Shipping;
using MerchantTribe.Commerce;
using MerchantTribe.Commerce.Datalayer;
using MerchantTribe.Shipping;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using MerchantTribe.Shipping.USPostal;
using MerchantTribe.Shipping.USPostal.v4;


namespace MerchantTribeStore.BVModules.Shipping.US_Postal_Service___International
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
                LoadServiceCodes();                
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

        private void LoadServiceCodes()
        {
            MerchantTribe.Shipping.IShippingService uspostal = AvailableServices.FindById(ShippingMethod.ShippingProviderId, MyPage.MTApp.CurrentStore);
            this.ShippingTypesCheckBoxList.DataSource = uspostal.ListAllServiceCodes();
            this.ShippingTypesCheckBoxList.DataTextField = "DisplayName";
            this.ShippingTypesCheckBoxList.DataValueField = "Code";
            this.ShippingTypesCheckBoxList.DataBind();
        }

        private void LoadData()
        {
            this.NameField.Text = ShippingMethod.Name;
            if (this.NameField.Text == string.Empty)
            {
                this.NameField.Text = "US Postal Service International";
            }

            // Adjustment
            AdjustmentDropDownList.SelectedValue = ((int)ShippingMethod.AdjustmentType).ToString();
            if (ShippingMethod.AdjustmentType == ShippingMethodAdjustmentType.Amount)
            {
                AdjustmentTextBox.Text = string.Format("{0:c}", ShippingMethod.Adjustment);
            }
            else
            {
                AdjustmentTextBox.Text = string.Format("{0:f}", ShippingMethod.Adjustment);
            }


            // Zones
            if (this.lstZones.Items.FindByValue(ShippingMethod.ZoneId.ToString()) != null)
            {
                this.lstZones.ClearSelection();
                this.lstZones.Items.FindByValue(ShippingMethod.ZoneId.ToString()).Selected = true;
            }

            // Global
            this.Diagnostics.Checked = MyPage.MTApp.CurrentStore.Settings.ShippingUSPostalDiagnostics;

            USPostalServiceSettings settings = new USPostalServiceSettings();
            settings.Merge(ShippingMethod.Settings);

            foreach (MerchantTribe.Shipping.ServiceCode code in settings.ServiceCodeFilter)
            {
                foreach (ListItem item in ShippingTypesCheckBoxList.Items)
                {
                    if (string.Compare(item.Value, code.Code, true) == 0)
                    {
                        item.Selected = true;
                        break;
                    }
                }
            }            
        }

        private void SaveData()
        {
            ShippingMethod.Name = this.NameField.Text.Trim();
            ShippingMethod.ZoneId = long.Parse(this.lstZones.SelectedItem.Value);
            ShippingMethod.AdjustmentType = (ShippingMethodAdjustmentType)int.Parse(AdjustmentDropDownList.SelectedValue);
            ShippingMethod.Adjustment = decimal.Parse(AdjustmentTextBox.Text, System.Globalization.NumberStyles.Currency);

            // Global Settings
            MyPage.MTApp.CurrentStore.Settings.ShippingUSPostalDiagnostics = this.Diagnostics.Checked;
            
            // Method Settings
            USPostalServiceSettings Settings = new USPostalServiceSettings();
            Settings.Merge(ShippingMethod.Settings);

            // Service Code
            List<MerchantTribe.Shipping.IServiceCode> filter = new List<MerchantTribe.Shipping.IServiceCode>();
                foreach (ListItem item in ShippingTypesCheckBoxList.Items)
                {
                    if (item.Selected)
                    {
                        MerchantTribe.Shipping.ServiceCode code = new MerchantTribe.Shipping.ServiceCode() { Code = item.Value, DisplayName = item.Text };
                        filter.Add(code);
                    }
                }
            Settings.ServiceCodeFilter = filter;
                        
            ShippingMethod.Settings.Merge(Settings);

            MyPage.MTApp.UpdateCurrentStore();
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