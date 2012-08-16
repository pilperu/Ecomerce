using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MerchantTribe.Commerce.Marketing;
using System.IO;
using System.Text;


namespace MerchantTribeStore.BVAdmin.Marketing
{
    public partial class Promotions : BaseAdminPage
    {
        private int pageSize = 50;
        private int rowCount = 0;
        private int currentPage = 1;
        private string keyword = string.Empty;

        protected override void OnPreInit(System.EventArgs e)
        {
            base.OnPreInit(e);
            this.PageTitle = "Promotions";
            this.CurrentTab = AdminTabType.Marketing;
            ValidateCurrentUserHasPermission(MerchantTribe.Commerce.Membership.SystemPermissions.MarketingView);
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
                this.chkShowDisabled.Checked = false;
                if (Request.QueryString["showdisabled"] == "1")
                {
                    this.chkShowDisabled.Checked = true;                
                }

                LoadData();
                this.FilterField.Focus();
            }
        }

        private void LoadData()
        {
            List<Promotion> items = MTApp.MarketingServices.Promotions.FindAllWithFilter(this.keyword,
                                                                                        this.chkShowDisabled.Checked,
                                                                                        currentPage, pageSize, ref rowCount);
            this.lblResults.Text = rowCount.ToString() + " found";
            this.litPager1.Text = MerchantTribe.Web.Paging.RenderPagerWithLimits("Promotions.aspx?page={0}&showdisabled=" + (this.chkShowDisabled.Checked ? "1":"0") + "&keyword=" + System.Web.HttpUtility.UrlEncode(keyword), currentPage, rowCount, pageSize, 20);
            RenderItems(items);
            this.litPager2.Text = this.litPager1.Text;
        }

        protected void btnGo_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            Response.Redirect("Promotions.aspx?page=1&showdisabled=" 
                              + (this.chkShowDisabled.Checked ? "1":"0") 
                              + "&keyword=" 
                              + System.Web.HttpUtility.UrlEncode(this.FilterField.Text.Trim()));
        }

        private void RenderItems(List<Promotion> items)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("<table width=\"100%\"><tr><th style=\"text-align:left;\">Name</th><th style=\"text-align:left;\">Status</th><th style=\"text-align:left;\">Enabled</th><th>&nbsp;</th><th>&nbsp;</th></tr>");

            foreach (Promotion p in items)
            {
                RenderSingleItem(sb, p);
            }

            sb.Append("</table>");
            this.litResults.Text = sb.ToString();
        }

        private void RenderSingleItem(StringBuilder sb, Promotion p)
        {

            string destinationLink = "Promotions_edit.aspx?id=" + p.Id + "&page=" + currentPage + "&showdisabled=" 
                              + (this.chkShowDisabled.Checked ? "1":"0") +
                              "&keyword=" + System.Web.HttpUtility.UrlEncode(keyword);
            string deleteLink = destinationLink.Replace("_edit", "_delete");

            sb.Append("<tr><td><a href=\"" + destinationLink + "\">" + p.Name + "</a></td>");
            sb.Append("<td><a href=\"" + destinationLink + "\">");
            switch (p.GetStatus(DateTime.UtcNow))
            {
                case PromotionStatus.Active:
                    sb.Append("<span style=\"color:#060\">Active</span>");
                    break;
                case PromotionStatus.Disabled:
                    sb.Append("<span style=\"color:#999\">Disabled</span>");                    
                    break;
                case PromotionStatus.Expired:
                    sb.Append("<span style=\"color:#600\">Expired</span>");                    
                    break;
                case PromotionStatus.Unknown:
                    sb.Append("<span style=\"color:#999;\">Unknown</span>");                    
                    break;
                case PromotionStatus.Upcoming:
                    sb.Append("<span style=\"color:#d4d40e;\">Upcoming</span>");                    
                    break;
            }                    
            sb.Append("</a></td>");
            sb.Append("<td><a href=\"" + destinationLink + "\">" + (p.IsEnabled ? "<span style=\"color:#060\">Yes</span" : "<span style=\"color:#999\">No</span>") + "</a></td>");
            sb.Append("<td><a onclick=\"return window.confirm('Delete this item?');\" href=\"" + deleteLink + "\" class=\"btn\"><b>Delete</b></a></td>");
            sb.Append("<td><a href=\"" + destinationLink + "\" class=\"btn\"><b>Edit</b></a></td></tr>");
        }

        protected void btnNew_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            string newType = this.lstNewType.SelectedValue;
            int temp = -1;
            int.TryParse(newType, out temp);

            long newId = 0;

            if (temp >= 0)
            {
                PreDefinedPromotion promo = (PreDefinedPromotion)temp;
                Promotion predefined = MTApp.MarketingServices.GetPredefinedPromotion(promo);
                if (predefined == null) return;
                MTApp.MarketingServices.Promotions.Create(predefined);
                newId = predefined.Id;
            }
            else
            {
                if (temp == -1)
                {
                    Promotion sale = new Promotion();
                    sale.Mode = PromotionType.Sale;
                    sale.Name = "New Custom Sale";
                    MTApp.MarketingServices.Promotions.Create(sale);
                    newId = sale.Id;
                }
                else
                {
                    Promotion offer = new Promotion();
                    offer.Mode = PromotionType.Offer;
                    offer.Name = "New Custom Offer";
                    MTApp.MarketingServices.Promotions.Create(offer);
                    newId = offer.Id;
                }
            }

            string destinationLink = "Promotions_edit.aspx?id=" + newId + "&page=" + currentPage + "&showdisabled="
                             + (this.chkShowDisabled.Checked ? "1" : "0") +
                             "&keyword=" + System.Web.HttpUtility.UrlEncode(keyword);
            Response.Redirect(destinationLink);
        }

    }
}