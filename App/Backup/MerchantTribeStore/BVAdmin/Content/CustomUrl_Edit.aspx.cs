using System;
using System.Web;
using System.Web.UI.WebControls;
using MerchantTribe.Commerce.Membership;
using MerchantTribe.Commerce.Content;
using MerchantTribe.Commerce;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using MerchantTribe.Commerce.Utilities;

namespace MerchantTribeStore
{

    partial class BVAdmin_Content_CustomUrl_Edit : BaseAdminPage
    {
        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.PageTitle = "Edit Custom Url";
            this.CurrentTab = AdminTabType.Content;
            ValidateCurrentUserHasPermission(SystemPermissions.ContentView);
        }

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);

            if (!Page.IsPostBack)
            {

                this.RequestedUrlField.Focus();

                if (Request.QueryString["id"] != null)
                {
                    this.BvinField.Value = Request.QueryString["id"];
                    LoadUrl();
                }
                else
                {
                    this.BvinField.Value = string.Empty;
                }
            }
        }

        private void LoadUrl()
        {
            CustomUrl c;
            c = MTApp.ContentServices.CustomUrls.Find(this.BvinField.Value);
            if (c != null)
            {
                if (c.Bvin != string.Empty)
                {
                    this.RequestedUrlField.Text = c.RequestedUrl;
                    this.RedirectToUrlField.Text = c.RedirectToUrl;
                    this.chkPermanent.Checked = c.IsPermanentRedirect;
                }
            }
        }

        protected void btnSaveChanges_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            if (this.Save() == true)
            {
                Response.Redirect("CustomUrl.aspx");
            }
        }

        protected void btnCancel_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            Response.Redirect("CustomUrl.aspx");
        }

        private bool Save()
        {
            bool result = false;

            if (UrlRewriter.IsUrlInUse(this.RequestedUrlField.Text.Trim(), this.BvinField.Value, MTApp.CurrentRequestContext, MTApp))
            {
                this.MessageBox1.ShowWarning("Another item already uses this URL. Please choose another one");
                return false;
            }
            
            CustomUrl c;
            c = MTApp.ContentServices.CustomUrls.Find(this.BvinField.Value);
            if (c == null) c = new CustomUrl();
            if (c != null)
            {
                c.RequestedUrl = this.RequestedUrlField.Text.Trim();
                c.RedirectToUrl = this.RedirectToUrlField.Text.Trim();
                c.IsPermanentRedirect = this.chkPermanent.Checked;

                if (this.BvinField.Value == string.Empty)
                {
                    result = MTApp.ContentServices.CustomUrls.Create(c);
                }
                else
                {
                    result = MTApp.ContentServices.CustomUrls.Update(c);
                }

                if (result == true)
                {
                    // Update bvin field so that next save will call updated instead of create
                    this.BvinField.Value = c.Bvin;
                }
            }

            return result;
        }

    }
}