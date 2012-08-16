using System;
using MerchantTribe.Commerce;
using System.Text;
using System.Web;

namespace MerchantTribeStore
{

    partial class BVAdmin_Default : BaseAdminPage
    {
        public string NewsUrl { get; set; }

        public override bool RequiresSSL
        {
            get
            {
                return true;
            }
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            this.NewsUrl = BuildNewsUrl();

            if (WebAppSettings.IsIndividualMode)
            {
                // Simple pci check for default admin username
                if (MTApp.CurrentRequestContext.CurrentAdministrator(MTApp).Email == "admin@merchanttribe.com") Response.Redirect("ChangeEmail.aspx?pci=1");
            }

            this.pnlGettingStarted.Visible = !MTApp.CurrentStore.Settings.HideGettingStarted;
            ShowFreeMessage();
        }

        protected override void OnPreInit(System.EventArgs e)
        {
            base.OnPreInit(e);
            this.PageTitle = "Dashboard";
            this.CurrentTab = AdminTabType.Dashboard;
        }

        protected void lnkHideGettingStarted_Click(object sender, EventArgs e)
        {
            MTApp.CurrentStore.Settings.HideGettingStarted = true;
            MTApp.UpdateCurrentStore();
            this.pnlGettingStarted.Visible = false;
        }

        private void ShowFreeMessage()
        {
            if (MTApp.CurrentStore.PlanId == 0)
            {
                this.litFreePlan.Text = "<div class=\"flash-message-info\">Your store is on the Free plan. <a href=\"ChangePlan.aspx\">Upgrade Your Store</a> to support more products and features.</div>";
            }
        }

        private string BuildNewsUrl()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("https://merchanttribe.com/news");
            sb.Append("?uid=" + HttpUtility.UrlEncode(MTApp.CurrentStore.StoreUniqueId(MTApp)));
            sb.Append("&host=" + HttpUtility.UrlEncode(MTApp.CurrentStore.RootUrl()));
            sb.Append("&ver=" + HttpUtility.UrlEncode(WebAppSettings.SystemVersionNumber));
            sb.Append("&com=" + HttpUtility.UrlEncode(WebAppSettings.IsCommercialVersion ? "1" : "0"));
            sb.Append("&email=" + HttpUtility.UrlEncode(this.CurrentUser.Email));
            return sb.ToString();
        }
    }
}