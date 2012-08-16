using System;
using System.Web;
using System.Web.UI.WebControls;
using MerchantTribe.Commerce.Membership;
using MerchantTribe.Commerce.Content;
using MerchantTribe.Commerce;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace MerchantTribeStore
{

    partial class BVAdmin_Content_StoreClosed : BaseAdminPage
    {

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.PageTitle = "Store Closed Content";
            this.CurrentTab = AdminTabType.Content;
            ValidateCurrentUserHasPermission(SystemPermissions.ContentView);
        }

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);

            if (!Page.IsPostBack)
            {

                this.ContentField.Focus();
                LoadData();

            }
        }

        private void LoadData()
        {
            this.ContentField.PreTransformText = MTApp.CurrentStore.Settings.StoreClosedDescriptionPreTransform;

            this.ContentField.Text = MTApp.CurrentStore.Settings.StoreClosedDescription;
            if (this.ContentField.SupportsTransform)
            {
                if (MTApp.CurrentStore.Settings.StoreClosedDescriptionPreTransform.Length > 0)
                {
                    this.ContentField.Text = MTApp.CurrentStore.Settings.StoreClosedDescriptionPreTransform;
                }
            }
        }

        protected void btnSaveChanges_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            if (this.Save() == true)
            {
                Response.Redirect("default.aspx");
            }
        }

        protected void btnCancel_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            Response.Redirect("Default.aspx");
        }

        private bool Save()
        {
            MTApp.CurrentStore.Settings.StoreClosedDescription = this.ContentField.Text;
            MTApp.CurrentStore.Settings.StoreClosedDescriptionPreTransform = this.ContentField.PreTransformText;
            return MTApp.UpdateCurrentStore();
        }


    }

}