
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

    partial class BVAdmin_Content_Policies_Edit : BaseAdminPage
    {
        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.PageTitle = "Edit Policy";
            this.CurrentTab = AdminTabType.Content;
            ValidateCurrentUserHasPermission(SystemPermissions.ContentView);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (!Page.IsPostBack)
            {
                if (Request.QueryString["id"] != null)
                {
                    this.PolicyIDField.Value = Request.QueryString["id"];
                }
                LoadPolicy();
            }
        }

        private void LoadPolicy()
        {
            Policy p = MTApp.ContentServices.Policies.Find(this.PolicyIDField.Value);
            this.lblTitle.Text = p.Title;
            this.GridView1.DataSource = p.Blocks;
            this.GridView1.DataBind();
        }

        protected void btnNew_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            Response.Redirect("Policies_EditBlock.aspx?policyId=" + Server.UrlEncode(this.PolicyIDField.Value));
        }

        protected void GridView1_RowCancelingEdit(object sender, System.Web.UI.WebControls.GridViewCancelEditEventArgs e)
        {
            this.msg.ClearMessage();
            string bvin = string.Empty;
            bvin = ((GridView)sender).DataKeys[e.RowIndex].Value.ToString();
            Policy p = MTApp.ContentServices.Policies.Find(this.PolicyIDField.Value);
            if (p != null)
            {
                p.MoveBlockDown(bvin);
                MTApp.ContentServices.Policies.Update(p);
            }            
            LoadPolicy();
        }

        protected void GridView1_RowUpdating(object sender, System.Web.UI.WebControls.GridViewUpdateEventArgs e)
        {
            this.msg.ClearMessage();
            string bvin = string.Empty;
            bvin = ((GridView)sender).DataKeys[e.RowIndex].Value.ToString();
            Policy p = MTApp.ContentServices.Policies.Find(this.PolicyIDField.Value);
            if (p != null)
            {
                p.MoveBlockUp(bvin);
                MTApp.ContentServices.Policies.Update(p);
            }            
            LoadPolicy();
        }

        protected void GridView1_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                PolicyBlock b = (PolicyBlock)e.Row.DataItem;

                if (b != null)
                {
                    Label lblBlockName = (Label)e.Row.FindControl("lblBlockName");
                    Label lblBlockDescription = (Label)e.Row.FindControl("lblBlockDescription");

                    if (lblBlockName != null)
                    {
                        lblBlockName.Text = b.Name;
                    }

                    if (lblBlockDescription != null)
                    {
                        lblBlockDescription.Text = b.Description;
                    }
                }

            }
        }

        protected void GridView1_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
        {
            this.msg.ClearMessage();
            string bvin = string.Empty;
            bvin = ((GridView)sender).DataKeys[e.RowIndex].Value.ToString();
            MTApp.ContentServices.Policies.DeleteBlock(bvin);
            LoadPolicy();
        }

        protected void btnOk_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            Response.Redirect("Policies.aspx");
        }
    }
}