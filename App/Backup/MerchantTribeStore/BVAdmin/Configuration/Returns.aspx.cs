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

    partial class BVAdmin_Configuration_Returns : BaseAdminPage
    {
        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.PageTitle = "Return Settings";
            this.CurrentTab = AdminTabType.Configuration;
            ValidateCurrentUserHasPermission(SystemPermissions.SettingsView);
        }

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);

            if (!Page.IsPostBack)
            {

                if (MTApp.CurrentStore.Settings.AutomaticallyIssueRMANumbers)
                {
                    this.AutomaticallyIssueRMACheckBoxList.SelectedValue = "1";
                }
                else
                {
                    this.AutomaticallyIssueRMACheckBoxList.SelectedValue = "0";
                }

            }
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
            MTApp.CurrentStore.Settings.AutomaticallyIssueRMANumbers = (AutomaticallyIssueRMACheckBoxList.SelectedValue == "1");
            return MTApp.UpdateCurrentStore();            
        }

    }
}