using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MerchantTribe.Commerce.Reporting;

namespace MerchantTribeStore
{

    public partial class BVAdmin_Controls_DashboardNews : MerchantTribe.Commerce.Content.BVUserControl
    {
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            LoadNews();
        }

        private void LoadNews()
        {
            BVNewsMessageManager manager = BVNewsMessageManager.InstantiateForDatabase(MyPage.MTApp.CurrentRequestContext);
            List<BVNewsMessage> messages = manager.GetLatestNews(10);
            foreach (BVNewsMessage m in messages)
            {
                this.litNews.Text += "<div class=\"flash-message-minor\"><b>";
                this.litNews.Text += TimeZoneInfo.ConvertTimeFromUtc(m.TimeStampUtc, MyPage.MTApp.CurrentStore.Settings.TimeZone).ToShortDateString();
                this.litNews.Text += "</b><br />" + m.Message + "</div>";
            }
        }

    }
}