using System;
using System.Web;
using System.Web.UI.WebControls;
using MerchantTribe.Commerce.Membership;
using MerchantTribe.Commerce.Content;
using MerchantTribe.Commerce;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Collections.Generic;

namespace MerchantTribeStore
{

    partial class BVAdmin_Content_CustomUrl : BaseAdminPage
    {

        private int pageSize = 50;
        private int rowCount = 0;
        private int currentPage = 1;

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.PageTitle = "Custom Urls";
            this.CurrentTab = AdminTabType.Content;
            ValidateCurrentUserHasPermission(SystemPermissions.ContentView);
        }

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);

            if (!Page.IsPostBack)
            {
                GridView1.PageSize = WebAppSettings.RowsPerPage;
                LoadUrls();
            }
        }

        private void LoadUrls()
        {
            if ((Request.QueryString["page"] != null))
            {
                int.TryParse(Request.QueryString["page"], out currentPage);
                if ((currentPage < 1))
                {
                    currentPage = 1;
                }
            }

            List<CustomUrl> urls = MTApp.ContentServices.CustomUrls.FindAllPaged(currentPage, pageSize,ref rowCount);
            this.lblResults.Text = rowCount.ToString() + " Urls Found";
            this.GridView1.DataSource = urls;
            this.GridView1.DataBind();

            this.litPager1.Text = MerchantTribe.Web.Paging.RenderPagerWithLimits("CustomUrl.aspx?page={0}", currentPage, rowCount, pageSize, 20);            
            this.litPager2.Text = this.litPager1.Text;

        }

        protected void GridView1_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
        {
            msg.ClearMessage();
            string bvin = (string)GridView1.DataKeys[e.RowIndex].Value;
            if (MTApp.ContentServices.CustomUrls.Delete(bvin) == false)
            {
                this.msg.ShowWarning("Unable to delete this custom Url.");
            }

            LoadUrls();
            e.Cancel = true;
        }

        protected void btnNew_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            msg.ClearMessage();
            Response.Redirect("CustomUrl_Edit.aspx");
        }

        protected void GridView1_RowEditing(object sender, System.Web.UI.WebControls.GridViewEditEventArgs e)
        {
            string bvin = (string)GridView1.DataKeys[e.NewEditIndex].Value;
            Response.Redirect("CustomUrl_Edit.aspx?id=" + bvin);
        }

    }

}