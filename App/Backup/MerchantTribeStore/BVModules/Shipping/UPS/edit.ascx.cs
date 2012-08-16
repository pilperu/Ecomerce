using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using MerchantTribe.Commerce.Content;
using MerchantTribe.Commerce.Shipping;
using MerchantTribe.Commerce;
using MerchantTribe.Shipping.Ups;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MerchantTribeStore
{

    partial class BVModules_Shipping_UPS_edit : BVShippingModule
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
            MerchantTribe.Shipping.IShippingService ups = AvailableServices.FindById(ShippingMethod.ShippingProviderId, MyPage.MTApp.CurrentStore);
            this.ShippingTypesCheckBoxList.DataSource= ups.ListAllServiceCodes();
            this.ShippingTypesCheckBoxList.DataTextField = "DisplayName";
            this.ShippingTypesCheckBoxList.DataValueField = "Code";
            this.ShippingTypesCheckBoxList.DataBind();
        }

        private void LoadData()
        {
            // Name
            this.NameField.Text = ShippingMethod.Name;
            if (this.NameField.Text == string.Empty)
            {
                this.NameField.Text = "UPS Shipping";
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

            // Global Settings
            this.AccountNumberField.Text = MyPage.MTApp.CurrentStore.Settings.ShippingUpsAccountNumber;
            this.ResidentialAddressCheckBox.Checked = MyPage.MTApp.CurrentStore.Settings.ShippingUpsForceResidential;
            this.PickupTypeRadioButtonList.SelectedValue = MyPage.MTApp.CurrentStore.Settings.ShippingUpsPickupType.ToString();
            this.DefaultPackagingField.SelectedValue = MyPage.MTApp.CurrentStore.Settings.ShippingUpsDefaultPackaging.ToString();
            //this.DefaultPaymentField.SelectedValue = MyPage.Services.CurrentStore.ShippingUpsDefaultPayment.ToString();
            this.DefaultServiceField.SelectedValue = MyPage.MTApp.CurrentStore.Settings.ShippingUpsDefaultService.ToString();
            if (MyPage.MTApp.CurrentStore.Settings.ShippingUpsLicense.Trim().Length > 0)
            {
                this.lnkRegister.Text = "Already Registered with UPS (click to register again)";
            }
            else
            {
                this.lnkRegister.Text = "Register with UPS to use Online Tools";
            }
            this.SkipDimensionsCheckBox.Checked = MyPage.MTApp.CurrentStore.Settings.ShippingUpsSkipDimensions;
            this.chkDiagnostics.Checked = MyPage.MTApp.CurrentStore.Settings.ShippingUPSDiagnostics;


            // Method Settings
            UPSServiceSettings Settings = new UPSServiceSettings();
            Settings.Merge(ShippingMethod.Settings);


            if (Settings.ServiceCodeFilter == null)
            {
                Settings.ServiceCodeFilter = new List<MerchantTribe.Shipping.IServiceCode>();
            }

            if (Settings.ServiceCodeFilter.Count < 1 || Settings.GetAllRates)
            {
                this.litMessage.Text = "Setting rbfilter to 1";

                if (rbFilterMode.Items.FindByValue("1") != null)
                {
                    this.rbFilterMode.ClearSelection();
                    this.rbFilterMode.Items.FindByValue("1").Selected = true;
                }
            }
            else
            {
                this.litMessage.Text = "Setting rbfilter to 0";
                if (rbFilterMode.Items.FindByValue("0") != null)
                {
                    this.rbFilterMode.ClearSelection();
                    this.rbFilterMode.Items.FindByValue("0").Selected = true;
                }

                foreach (MerchantTribe.Shipping.ServiceCode code in Settings.ServiceCodeFilter)
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
            
        }

        private void SaveData()
        {
            ShippingMethod.Name = this.NameField.Text.Trim();            
            ShippingMethod.ZoneId = long.Parse(this.lstZones.SelectedItem.Value);
            ShippingMethod.AdjustmentType = (ShippingMethodAdjustmentType)int.Parse(AdjustmentDropDownList.SelectedValue);
            ShippingMethod.Adjustment = decimal.Parse(AdjustmentTextBox.Text, System.Globalization.NumberStyles.Currency);

            // Global Settings
            MyPage.MTApp.CurrentStore.Settings.ShippingUpsAccountNumber = this.AccountNumberField.Text.Trim();
            MyPage.MTApp.CurrentStore.Settings.ShippingUpsForceResidential = this.ResidentialAddressCheckBox.Checked;
            MyPage.MTApp.CurrentStore.Settings.ShippingUpsPickupType = int.Parse(this.PickupTypeRadioButtonList.SelectedValue);
            MyPage.MTApp.CurrentStore.Settings.ShippingUpsDefaultService = int.Parse(this.DefaultServiceField.SelectedValue);
            MyPage.MTApp.CurrentStore.Settings.ShippingUpsDefaultPackaging = int.Parse(this.DefaultPackagingField.SelectedValue);
            //MyPage.Services.CurrentStore.Settings.ShippingUpsDefaultPayment = int.Parse(this.DefaultPaymentField.SelectedValue);
            MyPage.MTApp.CurrentStore.Settings.ShippingUpsSkipDimensions = this.SkipDimensionsCheckBox.Checked;
            MyPage.MTApp.CurrentStore.Settings.ShippingUPSDiagnostics = this.chkDiagnostics.Checked;
            


            // Method Settings
            UPSServiceSettings Settings = new UPSServiceSettings();
            Settings.Merge(ShippingMethod.Settings);

            List<MerchantTribe.Shipping.IServiceCode> filter = new List<MerchantTribe.Shipping.IServiceCode>();

            if (this.rbFilterMode.SelectedValue == "0")
            {
                Settings.GetAllRates = false;

                foreach (ListItem item in ShippingTypesCheckBoxList.Items)
                {
                    if (item.Selected)
                    {
                        MerchantTribe.Shipping.ServiceCode code = new MerchantTribe.Shipping.ServiceCode() { Code = item.Value, DisplayName = item.Text };
                        filter.Add(code);
                    }
                }
            }
            else
            {
                Settings.GetAllRates = true;
            }

            Settings.ServiceCodeFilter = filter;
            ShippingMethod.Settings.Merge(Settings);

            MyPage.MTApp.AccountServices.Stores.Update(MyPage.MTApp.CurrentStore);
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

        protected void rbFilterMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rbFilterMode.SelectedValue == "1")
            {
                this.pnlFilter.Visible = false;
            }
            else
            {
                this.pnlFilter.Visible = true;
            }
        }
    }
}