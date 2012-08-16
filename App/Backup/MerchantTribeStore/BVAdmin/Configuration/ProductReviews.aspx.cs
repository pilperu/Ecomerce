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

    partial class BVAdmin_Configuration_ProductReviews : BaseAdminPage
    {
        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.PageTitle = "Product Review Settings";
            this.CurrentTab = AdminTabType.Configuration;
            ValidateCurrentUserHasPermission(SystemPermissions.SettingsView);
        }

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);

            if (!Page.IsPostBack)
            {

                this.ProductReviewCountField.Text = MTApp.CurrentStore.Settings.ProductReviewCount.ToString();
                this.chkProductReviewModerate.Checked = MTApp.CurrentStore.Settings.ProductReviewModerate;
                this.chkProductReviewShowRating.Checked = MTApp.CurrentStore.Settings.ProductReviewShowRating;

            }
        }

        protected void btnCancel_Click(System.Object sender, System.Web.UI.ImageClickEventArgs e)
        {
            Response.Redirect("default.aspx");
        }

        protected void btnSave_Click(System.Object sender, System.Web.UI.ImageClickEventArgs e)
        {
            msg.ClearMessage();


            MTApp.CurrentStore.Settings.ProductReviewCount = int.Parse(this.ProductReviewCountField.Text.Trim());
            MTApp.CurrentStore.Settings.ProductReviewModerate = this.chkProductReviewModerate.Checked;
            MTApp.CurrentStore.Settings.ProductReviewShowRating = this.chkProductReviewShowRating.Checked;
            MTApp.UpdateCurrentStore();

            this.msg.ShowOk("Settings saved successfully.");

        }


    }
}