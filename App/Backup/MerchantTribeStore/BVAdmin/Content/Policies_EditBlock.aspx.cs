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

    partial class BVAdmin_Content_Policies_EditBlock : BaseAdminPage
    {

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);

            if (!Page.IsPostBack)
            {

                if (Request.QueryString["id"] != null)
                {
                    this.BvinField.Value = Request.QueryString["id"];
                }
                else
                {
                    if (Request.QueryString["policyId"] != null)
                    {
                        this.PolicyIdField.Value = Request.QueryString["policyId"];
                    }
                }
                LoadBlock();
            }

        }

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.PageTitle = "Edit Policy Block";
            this.CurrentTab = AdminTabType.Content;
            ValidateCurrentUserHasPermission(SystemPermissions.ContentView);
        }

        private void LoadBlock()
        {
            if (this.BvinField.Value.Trim().Length > 0)
            {
                PolicyBlock b = MTApp.ContentServices.Policies.FindBlock(this.BvinField.Value);
                if (b != null)
                {
                    this.NameField.Text = b.Name;
                    this.DescriptionField.Text = b.Description;
                    if (this.DescriptionField.SupportsTransform == true)
                    {
                        if (b.DescriptionPreTransform.Trim().Length > 0)
                        {
                            this.DescriptionField.Text = b.DescriptionPreTransform;
                        }
                    }
                    this.PolicyIdField.Value = b.PolicyID;
                }
            }
        }

        protected void btnCancel_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            RedirectOnComplete();
        }

        protected void btnSaveChanges_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            if (this.Save() == true)
            {
                RedirectOnComplete();
            }
            else
            {
                msg.ShowError("Unable to save block!");
            }
        }

        private void RedirectOnComplete()
        {
            if (this.PolicyIdField.Value.Trim().Length > 0)
            {
                Response.Redirect("Policies_Edit.aspx?id=" + Server.UrlEncode(this.PolicyIdField.Value.Trim()));
            }
            else
            {
                Response.Redirect("Policies.aspx");
            }
        }

        private bool Save()
        {
            bool result = false;

            PolicyBlock b;
            b = MTApp.ContentServices.Policies.FindBlock(this.BvinField.Value);
            if (b == null) b = new PolicyBlock();
            
                b.Name = this.NameField.Text.Trim();
                b.Description = this.DescriptionField.Text.Trim();
                b.DescriptionPreTransform = this.DescriptionField.PreTransformText;

                Policy p = MTApp.ContentServices.Policies.Find(this.PolicyIdField.Value);
                
                if (this.BvinField.Value == string.Empty)
                {
                    b.PolicyID = this.PolicyIdField.Value;
                    if (p != null)
                    {
                        p.Blocks.Add(b);
                        result = MTApp.ContentServices.Policies.Update(p);                        
                    }                    
                }
                else
                {
                    result = MTApp.ContentServices.Policies.UpdateBlock(b);
                }

                if (result == true)
                {
                    // Update bvin field so that next save will call updated instead of create
                    this.BvinField.Value = b.Bvin;
                }
            
            return result;
        }

    }
}