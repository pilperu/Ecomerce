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

namespace MerchantTribeStore.BVAdmin.Configuration
{
    public partial class SocialMedia : BaseAdminPage
    {
        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.PageTitle = "Social Media Settings";
            this.CurrentTab = AdminTabType.Configuration;
            ValidateCurrentUserHasPermission(SystemPermissions.SettingsView);
        }

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);

            if (!Page.IsPostBack)
            {
                Store currentStore = MTApp.CurrentStore;                

                // Loading
                this.chkUseFaceBook.Checked = currentStore.Settings.FaceBook.UseFaceBook;
                this.FaceBookAdminsField.Text = currentStore.Settings.FaceBook.Admins;
                this.FaceBookAppIdField.Text = currentStore.Settings.FaceBook.AppId;

                this.chkUseTwitter.Checked = currentStore.Settings.Twitter.UseTwitter;
                this.TwitterHandleField.Text = currentStore.Settings.Twitter.TwitterHandle;
                this.DefaultTweetTextField.Text = currentStore.Settings.Twitter.DefaultTweetText;

                this.chkUseGooglePlus.Checked = currentStore.Settings.GooglePlus.UseGooglePlus;                
            }
        }

        protected void btnSave_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {            
            MTApp.CurrentStore.Settings.FaceBook.UseFaceBook = this.chkUseFaceBook.Checked;
            MTApp.CurrentStore.Settings.FaceBook.Admins = this.FaceBookAdminsField.Text;
            MTApp.CurrentStore.Settings.FaceBook.AppId = this.FaceBookAppIdField.Text;

            MTApp.CurrentStore.Settings.Twitter.UseTwitter = this.chkUseTwitter.Checked;
            MTApp.CurrentStore.Settings.Twitter.TwitterHandle = this.TwitterHandleField.Text.Trim();
            MTApp.CurrentStore.Settings.Twitter.DefaultTweetText = this.DefaultTweetTextField.Text.Trim();

            MTApp.CurrentStore.Settings.GooglePlus.UseGooglePlus = this.chkUseGooglePlus.Checked;

            MTApp.UpdateCurrentStore();

            this.MessageBox1.ShowOk("Settings Saved!");
        }

    }

}