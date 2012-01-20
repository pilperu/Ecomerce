
using System.Collections.Generic;
using MerchantTribe.Commerce;
using MerchantTribe.Commerce.Membership;
using System.Text;
using System.IO;
using System.Text;

namespace MerchantTribeStore
{

    partial class BVAdmin_People_Default : BaseAdminPage
    {
        private int pageSize = 20;
        private int rowCount = 0;
        private int currentPage = 1;
        private string keyword = string.Empty;

        protected override void OnPreInit(System.EventArgs e)
        {
            base.OnPreInit(e);
            this.PageTitle = "Customers";
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

                LoadUsers();
                this.FilterField.Focus();
            }
        }

        private void LoadUsers()
        {                       
            List<CustomerAccount> accounts = MTApp.MembershipServices.Customers.FindByFilter(this.keyword, currentPage, pageSize,ref rowCount);

            this.lblResults.Text = rowCount.ToString() + " found";
            this.litPager1.Text = MerchantTribe.Web.Paging.RenderPagerWithLimits("Default.aspx?page={0}&keyword=" + System.Web.HttpUtility.UrlEncode(keyword), currentPage, rowCount, pageSize, 20);
            RenderItems(accounts);
            this.litPager2.Text = this.litPager1.Text;         
        }

        private void RenderItems(List<CustomerAccount> accounts)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<table width=\"100%\">");                                       
            foreach (CustomerAccount a in accounts)
            {
                RenderSingleItem(sb, a);
            }

            sb.Append("</table>");
            this.litResults.Text = sb.ToString();
        }

        private void RenderSingleItem(StringBuilder sb, CustomerAccount a)
        {
            string destinationLink = "users_edit.aspx?id=" + a.Bvin + "&page=" + currentPage + "&keyword=" + System.Web.HttpUtility.UrlEncode(keyword);
            string deleteLink = destinationLink.Replace("_edit", "_delete");

            sb.Append("<tr><td><a href=\"" + destinationLink + "\">");
            sb.Append("<img src=\"" + MerchantTribe.Commerce.Contacts.GravatarHelper.GetGravatarUrlForEmailWithSize(a.Email, 40) + "\" alt=\"" + a.Email + "\" />");
            sb.Append("</a></td>");


            sb.Append("<td><a href=\"" + destinationLink + "\">");
            sb.Append(a.LastName + "," + a.FirstName);
            sb.Append("</a></td>");

            sb.Append("<td><a href=\"" + destinationLink + "\">");
            sb.Append(a.Email);
            sb.Append("</a></td>");

            
            //sb.Append("<td><a onclick=\"return window.confirm('Delete this item?');\" href=\"" + deleteLink + "\" class=\"btn\"><b>Delete</b></a></td>");
            sb.Append("<td><a href=\"" + destinationLink + "\" class=\"btn\"><b>Edit</b></a></td></tr>");
        }
     
        protected void btnNew_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            string destinationLink = "users_edit.aspx?id=&page=" + currentPage + 
                           "&keyword=" + System.Web.HttpUtility.UrlEncode(keyword);
            Response.Redirect(destinationLink);            
        }

        protected void btnGo_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            Response.Redirect("Default.aspx?page=1&keyword="
                              + System.Web.HttpUtility.UrlEncode(this.FilterField.Text.Trim()));            
        }
           
     
    }
}