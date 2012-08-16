using System;
using System.Web;
using System.Web.UI.WebControls;
using MerchantTribe.Commerce.Membership;
using MerchantTribe.Commerce.Content;
using MerchantTribe.Commerce;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace MerchantTribeStore
{

    partial class BVAdmin_Content_EmailTemplates : BaseAdminPage
    {

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.PageTitle = "Email Templates";
            this.CurrentTab = AdminTabType.Content;
            ValidateCurrentUserHasPermission(SystemPermissions.ContentView);
        }

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);

            if (!Page.IsPostBack)
            {
                LoadTemplates();
            }
        }

        private void LoadTemplates()
        {
            List<HtmlTemplate> templates = MTApp.ContentServices.GetAllTemplatesForStoreOrDefaults();
            this.GridView1.DataSource = templates;
            this.GridView1.DataBind();
            this.lblResults.Text = templates.Count + " Templates Found";
        }

        protected void GridView1_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
        {
            msg.ClearMessage();
            long templateId = (long)GridView1.DataKeys[e.RowIndex].Value;
            if (MTApp.ContentServices.HtmlTemplates.Delete(templateId) == false)
            {
                this.msg.ShowWarning("Unable to delete this template.");
            }
            LoadTemplates();
        }

        protected void btnNew_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            msg.ClearMessage();
            Response.Redirect("EmailTemplates_Edit.aspx?id=0");
        }

        protected void GridView1_RowEditing(object sender, System.Web.UI.WebControls.GridViewEditEventArgs e)
        {
            long templateId = (long)GridView1.DataKeys[e.NewEditIndex].Value;
            Response.Redirect("EmailTemplates_Edit.aspx?id=" + templateId.ToString());
        }

    }
}