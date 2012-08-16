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

    partial class BVAdmin_Configuration_Analytics : BaseAdminPage
    {
        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.PageTitle = "Analytics Settings";
            this.CurrentTab = AdminTabType.Configuration;
            ValidateCurrentUserHasPermission(SystemPermissions.SettingsView);
        }

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);

            if (!Page.IsPostBack)
            {
                if (WebAppSettings.IsCommercialVersion)
                {
                    this.MerchantTribeAnalyticsRow.Visible = true;
                    this.MerchantTribeAnalyticsRow2.Visible = true;
                }
                else
                {
                    this.MerchantTribeAnalyticsRow.Visible = false;
                    this.MerchantTribeAnalyticsRow2.Visible = false;
                }

                // Loading
                this.chkUseMerchantTribeAnalytics.Checked = !MTApp.CurrentStore.Settings.Analytics.DisableMerchantTribeAnalytics;
                this.chkGoogleAdwords.Checked = MTApp.CurrentStore.Settings.Analytics.UseGoogleAdWords;
                this.GoogleAdwordsConversionIdField.Text = MTApp.CurrentStore.Settings.Analytics.GoogleAdWordsId;
                this.GoogleAdwordsLabelField.Text = MTApp.CurrentStore.Settings.Analytics.GoogleAdWordsLabel;
                this.GoogleAdwordsBackgroundColorField.Text = MTApp.CurrentStore.Settings.Analytics.GoogleAdWordsBgColor;

                this.chkGoogleEcommerce.Checked = MTApp.CurrentStore.Settings.Analytics.UseGoogleEcommerce;
                this.GoogleEcommerceCategoryNameField.Text = MTApp.CurrentStore.Settings.Analytics.GoogleEcommerceCategory;
                this.GoogleEcommerceStoreNameField.Text = MTApp.CurrentStore.Settings.Analytics.GoogleEcommerceStoreName;

                this.chkGoogleTracker.Checked = MTApp.CurrentStore.Settings.Analytics.UseGoogleTracker;
                this.GoogleTrackingIdField.Text = MTApp.CurrentStore.Settings.Analytics.GoogleTrackerId;

                this.chkYahoo.Checked = MTApp.CurrentStore.Settings.Analytics.UseYahooTracker;
                this.YahooAccountIdField.Text = MTApp.CurrentStore.Settings.Analytics.YahooAccountId;

                this.AdditionalMetaTagsField.Text = MTApp.CurrentStore.Settings.Analytics.AdditionalMetaTags;
                this.BottomAnalyticsField.Text = MTApp.CurrentStore.Settings.Analytics.BottomAnalytics;

            }
        }

        protected void btnSave_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {

            MTApp.CurrentStore.Settings.Analytics.DisableMerchantTribeAnalytics = !this.chkUseMerchantTribeAnalytics.Checked;

            MTApp.CurrentStore.Settings.Analytics.UseGoogleAdWords = this.chkGoogleAdwords.Checked;
            MTApp.CurrentStore.Settings.Analytics.GoogleAdWordsId = this.GoogleAdwordsConversionIdField.Text;
            MTApp.CurrentStore.Settings.Analytics.GoogleAdWordsLabel = this.GoogleAdwordsLabelField.Text;
            MTApp.CurrentStore.Settings.Analytics.GoogleAdWordsBgColor = this.GoogleAdwordsBackgroundColorField.Text;

            MTApp.CurrentStore.Settings.Analytics.UseGoogleEcommerce = this.chkGoogleEcommerce.Checked;
            MTApp.CurrentStore.Settings.Analytics.GoogleEcommerceCategory = this.GoogleEcommerceCategoryNameField.Text;
            MTApp.CurrentStore.Settings.Analytics.GoogleEcommerceStoreName = this.GoogleEcommerceStoreNameField.Text;

            MTApp.CurrentStore.Settings.Analytics.UseGoogleTracker = this.chkGoogleTracker.Checked;
            MTApp.CurrentStore.Settings.Analytics.GoogleTrackerId = this.GoogleTrackingIdField.Text;

            MTApp.CurrentStore.Settings.Analytics.UseYahooTracker = this.chkYahoo.Checked;
            MTApp.CurrentStore.Settings.Analytics.YahooAccountId = this.YahooAccountIdField.Text;

            MTApp.CurrentStore.Settings.Analytics.AdditionalMetaTags = this.AdditionalMetaTagsField.Text;
            MTApp.CurrentStore.Settings.Analytics.BottomAnalytics = this.BottomAnalyticsField.Text;
            
            MTApp.UpdateCurrentStore();

            this.MessageBox1.ShowOk("Settings Saved!");

        }

    }

}