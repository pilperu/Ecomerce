
using System;
using System.Web;
using System.Web.UI.WebControls;
using System.Text;
using MerchantTribe.Commerce;
using MerchantTribe.Commerce.Accounts;
using MerchantTribe.Commerce.BusinessRules;
using MerchantTribe.Commerce.Catalog;
using MerchantTribe.Commerce.Contacts;
using MerchantTribe.Commerce.Content;
using MerchantTribe.Commerce.Membership;
using MerchantTribe.Commerce.Metrics;
using MerchantTribe.Commerce.Orders;
using MerchantTribe.Commerce.Payment;
using MerchantTribe.Commerce.Shipping;
using MerchantTribe.Commerce.Taxes;
using MerchantTribe.Commerce.Utilities;

namespace MerchantTribeStore
{

    partial class BVAdmin_Configuration_InventoryNotices : BaseAdminPage
    {

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);

            if (!Page.IsPostBack)
            {
                this.EmailReportToTextBox.Text = MTApp.CurrentStore.Settings.MailServer.EmailForGeneral;
                this.LowStockHoursTextBox.Text = WebAppSettings.InventoryLowHours.ToString();
                this.LinePrefixTextBox.Text = WebAppSettings.InventoryLowReportLinePrefix;
                
                //if (this.lstInventoryMode.Items.FindByValue(((int)WebAppSettings.InventoryMode).ToString()) != null)
                //{
                //    this.lstInventoryMode.ClearSelection();
                //    this.lstInventoryMode.Items.FindByValue(((int)WebAppSettings.InventoryMode).ToString()).Selected = true;
                //}

                //this.TrackInventoryNewProductsCheckBox.Checked = WebAppSettings.InventoryEnabledNewProductDefault;
                //this.DefaultInventoryModeDropDownList.SelectedValue = ((int)WebAppSettings.InventoryNewProductDefaultMode).ToString();

            }
        }

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.PageTitle = "Inventory Settings";
            this.CurrentTab = AdminTabType.Configuration;
            ValidateCurrentUserHasPermission(SystemPermissions.SettingsView);
        }

        protected void btnCancel_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            Response.Redirect("default.aspx");
        }

        protected void btnSave_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            if (this.Save() == true)
            {
                this.MessageBox1.ShowOk("Settings saved successfully.");
            }
        }

        private bool Save()
        {
            bool result = false;
            MTApp.CurrentStore.Settings.MailServer.EmailForGeneral = this.EmailReportToTextBox.Text;
            //WebAppSettings.InventoryLowHours = int.Parse(this.LowStockHoursTextBox.Text);
            //WebAppSettings.InventoryLowReportLinePrefix = this.LinePrefixTextBox.Text;
            //WebAppSettings.DisableInventory = this.chkDisableInventory.Checked;
            //WebAppSettings.InventoryMode = (StoreProductInventoryMode)int.Parse(this.lstInventoryMode.SelectedValue);

            //WebAppSettings.InventoryEnabledNewProductDefault = this.TrackInventoryNewProductsCheckBox.Checked;
            //WebAppSettings.InventoryNewProductDefaultMode = (ProductInventoryMode)int.Parse(this.DefaultInventoryModeDropDownList.SelectedValue);
            result = true;

            return result;
        }

        protected void SendLowStockReportImageButton_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            MessageBox1.ClearMessage();
            if (this.EmailReportToTextBox.Text.Length > 0)
            {

                if (ProductInventory.EmailLowStockReport(this.EmailReportToTextBox.Text, MTApp.CurrentStore.StoreName, MTApp))
                {
                    MessageBox1.ShowOk("Report sent!");
                }
                else
                {
                    MessageBox1.ShowWarning("Report failed to send.");
                }
            }
            else
            {
                MessageBox1.ShowWarning("You must enter an email address to send the report!");
            }
        }
    }
}