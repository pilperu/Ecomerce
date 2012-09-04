using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using MerchantTribe.Commerce.Content;
using MerchantTribe.Commerce.Shipping;
using MerchantTribe.Commerce;
using MerchantTribe.Commerce.Utilities;
using System.Collections.ObjectModel;
using MerchantTribe.Commerce.Datalayer;
using MerchantTribe.Shipping;
using System.Collections.Generic;
using MerchantTribe.Shipping.FedEx;
using System.Text;

namespace MerchantTribeStore
{

    partial class BVModules_Shipping_FedEx_edit : BVShippingModule
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
                this.AddHighlightColors(this.lstHighlights);
                PopulateLists();
                LoadData();
                LoadZones();
                this.SourceAddress.LoadFromAddress(MyPage.MTApp.ContactServices.Addresses.FindStoreContactAddress());

                this.DestinationAddress.StreetLine1 = "9702 Gayton Rd";
                this.DestinationAddress.StateCode = "VA";
                this.DestinationAddress.StateName = "VA";
                this.DestinationAddress.City = "Richmond";
                this.DestinationAddress.PostalCode = "23238";
            }
        }

        //private void LoadSharedSettingsManager()
        //{
        //    ShippingMethod m = MyPage.MTApp.OrderServices.ShippingMethods.Find(this.BlockId);
        //}

        private void LoadZones()
        {
            this.lstZones.DataSource = MyPage.MTApp.OrderServices.ShippingZones.FindForStore(MyPage.MTApp.CurrentStore.Id);
            this.lstZones.DataTextField = "Name";
            this.lstZones.DataValueField = "id";
            this.lstZones.DataBind();
        }

        private void PopulateLists()
        {
            IShippingService provider = MerchantTribe.Commerce.Shipping.AvailableServices.FindById(ShippingMethod.ShippingProviderId, MyPage.MTApp.CurrentStore);
            if (provider != null)
            {
                List<IServiceCode> codes = provider.ListAllServiceCodes();
                this.lstServiceCode.Items.Clear();
                foreach (IServiceCode code in codes)
                {
                    this.lstServiceCode.Items.Add(new System.Web.UI.WebControls.ListItem(code.DisplayName, code.Code));
                    this.lstServicesTest.Items.Add(new System.Web.UI.WebControls.ListItem(code.DisplayName, code.Code));
                }
            }
        }

        private void LoadData()
        {
            // Method Settings
            FedExServiceSettings Settings = new FedExServiceSettings();
            Settings.Merge(ShippingMethod.Settings);


            this.NameField.Text = ShippingMethod.Name;
            if (this.NameField.Text == string.Empty)
            {
                this.NameField.Text = "FedEx";
            }
            if (this.lstServiceCode.Items.FindByValue(((int)Settings.ServiceCode).ToString()) != null)
            {
                this.lstServiceCode.ClearSelection();
                this.lstServiceCode.Items.FindByValue(((int)Settings.ServiceCode).ToString()).Selected = true;
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
            // Zones
            if (this.lstZones.Items.FindByValue(ShippingMethod.ZoneId.ToString()) != null)
            {
                this.lstZones.ClearSelection();
                this.lstZones.Items.FindByValue(ShippingMethod.ZoneId.ToString()).Selected = true;
            }


            //Globals
            if (this.lstPackaging.Items.FindByValue(MyPage.MTApp.CurrentStore.Settings.ShippingFedExDefaultPackaging.ToString()) != null)
            {
                this.lstPackaging.ClearSelection();
                this.lstPackaging.Items.FindByValue(MyPage.MTApp.CurrentStore.Settings.ShippingFedExDefaultPackaging.ToString()).Selected = true;
            }
            if (this.lstPackaging.Items.FindByValue(Settings.Packaging.ToString()) != null)
            {
                this.lstPackaging.ClearSelection();
                this.lstPackaging.Items.FindByValue(Settings.Packaging.ToString()).Selected = true;
            }

            this.KeyField.Text = MyPage.MTApp.CurrentStore.Settings.ShippingFedExKey;
            this.PasswordField.Text = MyPage.MTApp.CurrentStore.Settings.ShippingFedExPassword;
            this.AccountNumberField.Text = MyPage.MTApp.CurrentStore.Settings.ShippingFedExAccountNumber;
            this.MeterNumberField.Text = MyPage.MTApp.CurrentStore.Settings.ShippingFedExMeterNumber;
            if (this.lstDefaultPackaging.Items.FindByValue(MyPage.MTApp.CurrentStore.Settings.ShippingFedExDefaultPackaging.ToString()) != null)
            {
                this.lstDefaultPackaging.ClearSelection();
                this.lstDefaultPackaging.Items.FindByValue(MyPage.MTApp.CurrentStore.Settings.ShippingFedExDefaultPackaging.ToString()).Selected = true;
            }
            this.chkListRates.Checked = MyPage.MTApp.CurrentStore.Settings.ShippingFedExUseListRates;
            if (this.lstDropOffType.Items.FindByValue(MyPage.MTApp.CurrentStore.Settings.ShippingFedExDropOffType.ToString()) != null)
            {
                this.lstDropOffType.ClearSelection();
                this.lstDropOffType.Items.FindByValue(MyPage.MTApp.CurrentStore.Settings.ShippingFedExDropOffType.ToString()).Selected = true;
            }
            this.chkResidential.Checked = MyPage.MTApp.CurrentStore.Settings.ShippingFedExForceResidentialRates;

            this.chkDiagnostics.Checked = MyPage.MTApp.CurrentStore.Settings.ShippingFedExDiagnostics;

            // Select Hightlights
            string highlight = Settings.GetSettingOrEmpty("highlight");
            if (this.lstHighlights.Items.FindByText(highlight) != null)
            {
                this.lstHighlights.ClearSelection();
                this.lstHighlights.Items.FindByText(highlight).Selected = true;
            }
        }

        private void SaveData()
        {
            ShippingMethod.Name = this.NameField.Text.Trim();
            ShippingMethod.ZoneId = long.Parse(this.lstZones.SelectedItem.Value);
            ShippingMethod.AdjustmentType = (ShippingMethodAdjustmentType)int.Parse(AdjustmentDropDownList.SelectedValue);
            ShippingMethod.Adjustment = decimal.Parse(AdjustmentTextBox.Text, System.Globalization.NumberStyles.Currency);

            // Method Settings
            FedExServiceSettings Settings = new FedExServiceSettings();
            Settings.Merge(ShippingMethod.Settings);
            Settings.ServiceCode = int.Parse(this.lstServiceCode.SelectedValue);
            Settings.Packaging = int.Parse(this.lstPackaging.SelectedValue);
            Settings["highlight"] = this.lstHighlights.SelectedValue;
            ShippingMethod.Settings.Merge(Settings);

            // Globals
            MyPage.MTApp.CurrentStore.Settings.ShippingFedExKey = this.KeyField.Text.Trim();
            MyPage.MTApp.CurrentStore.Settings.ShippingFedExPassword = this.PasswordField.Text.Trim();
            MyPage.MTApp.CurrentStore.Settings.ShippingFedExAccountNumber = this.AccountNumberField.Text.Trim();
            MyPage.MTApp.CurrentStore.Settings.ShippingFedExMeterNumber = this.MeterNumberField.Text.Trim();
            MyPage.MTApp.CurrentStore.Settings.ShippingFedExDefaultPackaging = int.Parse(this.lstDefaultPackaging.SelectedValue);
            MyPage.MTApp.CurrentStore.Settings.ShippingFedExDropOffType = int.Parse(this.lstDropOffType.SelectedValue);
            MyPage.MTApp.CurrentStore.Settings.ShippingFedExForceResidentialRates = this.chkResidential.Checked;
            MyPage.MTApp.CurrentStore.Settings.ShippingFedExUseListRates = this.chkListRates.Checked;
            MyPage.MTApp.CurrentStore.Settings.ShippingFedExDiagnostics = this.chkDiagnostics.Checked;

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

        protected void btnTest_Click(object sender, EventArgs e)
        {
            this.SaveData();

            var testSettings = new FedExGlobalServiceSettings();
            testSettings.AccountNumber = this.AccountNumberField.Text;
            testSettings.DefaultDropOffType = (DropOffType)int.Parse(this.lstDropOffType.SelectedValue);
            testSettings.DefaultPackaging = (PackageType)int.Parse(this.lstPackaging.SelectedValue);
            testSettings.DiagnosticsMode = true;
            testSettings.ForceResidentialRates = this.chkResidential.Checked;
            testSettings.MeterNumber = this.MeterNumberField.Text.Trim();
            testSettings.UseListRates = this.chkListRates.Checked;
            testSettings.UserKey = this.KeyField.Text.Trim();
            testSettings.UserPassword = this.PasswordField.Text.Trim();

            var logger = new MerchantTribe.Web.Logging.TextLogger();

            var testSvc = new MerchantTribe.Shipping.FedEx.FedExProvider(testSettings, logger);
            testSvc.Settings.ServiceCode = int.Parse(this.lstServicesTest.SelectedValue);
            testSvc.Settings.Packaging = (int)testSettings.DefaultPackaging;

            var testShipment = new Shipment();
            testShipment.DestinationAddress = this.DestinationAddress.GetAsAddress();
            testShipment.SourceAddress = this.SourceAddress.GetAsAddress();
            var testItem = new Shippable();
            testItem.BoxHeight = decimal.Parse(this.TestHeight.Text);
            testItem.BoxLength = decimal.Parse(this.TestLength.Text);
            testItem.BoxWidth = decimal.Parse(this.TestWidth.Text);
            testItem.BoxLengthType = LengthType.Inches;
            testItem.BoxWeight = decimal.Parse(this.TestWeight.Text);
            testItem.BoxWeightType = MerchantTribe.Shipping.WeightType.Pounds;
            testShipment.Items.Add(testItem);
            
            StringBuilder sb = new StringBuilder();
            sb.Append("Starting Rate Test at " + DateTime.Now + "<br />");
            var rates = testSvc.RateShipment(testShipment);
            foreach (var r in rates)
            {
                sb.Append("Rate Found: " + r.EstimatedCost.ToString("C") + " | " + r.DisplayName + " (" + r.ServiceCodes + ", " + r.ServiceId + ")<br />");
            }
            sb.Append("<br />");
            sb.Append("LOG:<br />");
            foreach (var m in logger.Messages)
            {
                sb.Append(m + "<br />");
            }
            sb.Append("Finished Rate Test at " + DateTime.Now);
            this.litTestOuput.Text = sb.ToString();
        }
    }
}