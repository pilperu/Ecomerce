using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MerchantTribeStore.BVAdmin.Marketing
{
    public partial class Promotions_Delete : BaseAdminPage
    {
        protected override void OnPreInit(System.EventArgs e)
        {
            base.OnPreInit(e);
            this.PageTitle = "Promotions";
            this.CurrentTab = AdminTabType.Marketing;
            ValidateCurrentUserHasPermission(MerchantTribe.Commerce.Membership.SystemPermissions.MarketingView);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (!Page.IsPostBack)
            {

                int currentPage = 1;
                string keyword = string.Empty;
                string id = string.Empty;
                string showdisabled = string.Empty;

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
                    keyword = Request.QueryString["keyword"];
                }
                if ((Request.QueryString["showdisabled"] != null))
                {
                    showdisabled = Request.QueryString["showdisabled"];
                }

                if (Request.QueryString["id"] != null)
                {
                    string itemId = Request.QueryString["id"];
                    long temp = 0;
                    long.TryParse(itemId,out temp);
                    MTApp.MarketingServices.Promotions.Delete(temp);
                }

                Response.Redirect("Promotions.aspx?page=" + currentPage.ToString() 
                    + "&showdisabled=" + System.Web.HttpUtility.UrlEncode(showdisabled)
                    + "&keyword=" + System.Web.HttpUtility.UrlEncode(keyword));
            }
        }
    }
}