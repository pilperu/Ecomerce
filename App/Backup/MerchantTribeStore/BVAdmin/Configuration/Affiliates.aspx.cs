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

    partial class BVAdmin_Configuration_Affiliates : BaseAdminPage
    {
        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.PageTitle = "Affiliate Settings";
            this.CurrentTab = AdminTabType.Configuration;
            ValidateCurrentUserHasPermission(SystemPermissions.SettingsView);
        }

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);

            if (!Page.IsPostBack)
            {

                this.AffiliateCommissionAmountField.Text = MTApp.CurrentStore.Settings.AffiliateCommissionAmount.ToString();
                this.AffiliateReferralDays.Text = MTApp.CurrentStore.Settings.AffiliateReferralDays.ToString();
                this.lstCommissionType.ClearSelection();
                switch (MTApp.CurrentStore.Settings.AffiliateCommissionType)
                {
                    case AffiliateCommissionType.PercentageCommission:
                    case AffiliateCommissionType.None:
                        this.lstCommissionType.Items.FindByValue("1").Selected = true;
                        break;
                    case AffiliateCommissionType.FlatRateCommission:
                        this.lstCommissionType.Items.FindByValue("2").Selected = true;
                        break;
                    default:
                        this.lstCommissionType.Items.FindByValue("1").Selected = true;
                        break;
                }
                this.AffiliateConflictModeField.ClearSelection();
                switch (MTApp.CurrentStore.Settings.AffiliateConflictMode)
                {
                    case AffiliateConflictMode.FavorOldAffiliate:
                    case AffiliateConflictMode.None:
                        this.AffiliateConflictModeField.Items.FindByValue("1").Selected = true;
                        break;
                    case AffiliateConflictMode.FavorNewAffiliate:
                        this.AffiliateConflictModeField.Items.FindByValue("2").Selected = true;
                        break;
                    default:
                        this.AffiliateConflictModeField.Items.FindByValue("1").Selected = true;
                        break;
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
            MTApp.CurrentStore.Settings.AffiliateCommissionAmount = decimal.Parse(this.AffiliateCommissionAmountField.Text);
            MTApp.CurrentStore.Settings.AffiliateReferralDays = int.Parse(this.AffiliateReferralDays.Text);
            int typeSelection = int.Parse(this.lstCommissionType.SelectedValue);
            MTApp.CurrentStore.Settings.AffiliateCommissionType = (AffiliateCommissionType)typeSelection;
            int conflictSelection = int.Parse(this.AffiliateConflictModeField.SelectedValue);
            MTApp.CurrentStore.Settings.AffiliateConflictMode = (AffiliateConflictMode)conflictSelection;

            return MTApp.UpdateCurrentStore();
        }

    }
}