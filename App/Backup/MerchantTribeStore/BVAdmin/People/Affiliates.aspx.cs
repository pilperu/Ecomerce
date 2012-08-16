using System;
using System.Web;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using MerchantTribe.Commerce;
using MerchantTribe.Commerce.Contacts;
using System.IO;
using System.Text;

namespace MerchantTribeStore
{

    partial class BVAdmin_People_Affiliates : BaseAdminPage
    {
        private int pageSize = 50;
        private int rowCount = 0;
        private int currentPage = 1;
        private string keyword = string.Empty;

        protected override void OnPreInit(System.EventArgs e)
        {
            base.OnPreInit(e);
            this.PageTitle = "Affiliates";
            this.CurrentTab = AdminTabType.People;
            ValidateCurrentUserHasPermission(MerchantTribe.Commerce.Membership.SystemPermissions.PeopleView);
        }

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);

            if (!Page.IsPostBack)
            {
                if ((Request.QueryString["page"] != null))
                {
                    int.TryParse(Request.QueryString["page"], out currentPage);
                    if ((currentPage < 1))
                    {
                        currentPage = 1;
                    }
                }
                if ((Request.QueryString["keyword"] != null))
                {
                    this.keyword = Request.QueryString["keyword"];
                    this.FilterField.Text = keyword;
                }
                LoadData();
                this.FilterField.Focus();
            }
        }

        private void LoadData()
        {
            List<Affiliate> items = MTApp.ContactServices.Affiliates.FindAllWithFilter(this.keyword, currentPage, pageSize, ref rowCount);
            this.lblResults.Text = rowCount.ToString() + " found";
            this.litPager1.Text = MerchantTribe.Web.Paging.RenderPagerWithLimits("Affiliates.aspx?page={0}&keyword=" + System.Web.HttpUtility.UrlEncode(keyword), currentPage, rowCount, pageSize, 20);
            RenderItems(items);
            this.litPager2.Text = this.litPager1.Text;
        }

        protected void btnGo_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            Response.Redirect("Affiliates.aspx?page=1&keyword=" + System.Web.HttpUtility.UrlEncode(this.FilterField.Text.Trim()));
        }

        private void RenderItems(List<Affiliate> items)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("<table width=\"100%\"><tr><th>Name</th><th>Referral Id</th><th>Enabled</th><th>&nbsp;</th><th>&nbsp;</th></tr>");

            foreach (Affiliate a in items)
            {
                RenderSingleItem(sb, a);
            }

            sb.Append("</table>");
            this.litResults.Text = sb.ToString();
        }

        private void RenderSingleItem(StringBuilder sb, Affiliate a)
        {

            string destinationLink = "Affiliates_edit.aspx?id=" + a.Id + "&page=" + currentPage + "&keyword=" + System.Web.HttpUtility.UrlEncode(keyword);
            string deleteLink = destinationLink.Replace("_edit", "_delete");

            sb.Append("<tr><td><a href=\"" + destinationLink + "\">" + a.DisplayName + "</a></td>");
            sb.Append("<td><a href=\"" + destinationLink + "\">" + a.ReferralId + "</a></td>");
            sb.Append("<td><a href=\"" + destinationLink + "\">" + (a.Enabled ? "YES":"NO")  + "</a></td>");
            sb.Append("<td><a onclick=\"return window.confirm('Delete this item?');\" href=\"" + deleteLink + "\" class=\"btn\"><b>Delete</b></a></td>");
            sb.Append("<td><a href=\"" + destinationLink + "\" class=\"btn\"><b>Edit</b></a></td></tr>");
        }

        protected void btnNew_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            Response.Redirect("Affiliates_edit.aspx?id=0&page=" + currentPage + "&keyword=" + System.Web.HttpUtility.UrlEncode(keyword));
        }

    }
}