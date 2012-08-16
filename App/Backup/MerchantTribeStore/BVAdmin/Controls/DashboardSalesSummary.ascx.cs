using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MerchantTribe.Commerce.Reporting;

namespace MerchantTribeStore
{

    public partial class BVAdmin_Controls_DashboardSalesSummary : MerchantTribe.Commerce.Content.BVUserControl
    {
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            LoadSummary();
        }

        private void LoadSummary()
        {
            SalesSummary manager = new SalesSummary(MyPage.MTApp.CurrentStore.Id);
            WeeklySummary summary = manager.GetWeeklySummary(TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, MyPage.MTApp.CurrentStore.Settings.TimeZone), MyPage.MTApp.OrderServices);

            if (summary == null) return;

            this.litM.Text = summary.Monday.ToString("c");
            this.litT.Text = summary.Tuesday.ToString("c");
            this.litW.Text = summary.Wednesday.ToString("c");
            this.litR.Text = summary.Thursday.ToString("c");
            this.litF.Text = summary.Friday.ToString("c");
            this.litS.Text = summary.Saturday.ToString("c");
            this.litY.Text = summary.Sunday.ToString("c");
            this.litWeek.Text = summary.Week.ToString("c");

            this.litML.Text = summary.MondayLast.ToString("c");
            this.litTL.Text = summary.TuesdayLast.ToString("c");
            this.litWL.Text = summary.WednesdayLast.ToString("c");
            this.litRL.Text = summary.ThursdayLast.ToString("c");
            this.litFL.Text = summary.FridayLast.ToString("c");
            this.litSL.Text = summary.SaturdayLast.ToString("c");
            this.litYL.Text = summary.SundayLast.ToString("c");
            this.litWeekL.Text = summary.WeekLast.ToString("c");

            this.litMC.Text = FormatPercent(summary.MondayChange);
            this.litTC.Text = FormatPercent(summary.TuesdayChange);
            this.litWC.Text = FormatPercent(summary.WednesdayChange);
            this.litRC.Text = FormatPercent(summary.ThursdayChange);
            this.litFC.Text = FormatPercent(summary.FridayChange);
            this.litSC.Text = FormatPercent(summary.SaturdayChange);
            this.litYC.Text = FormatPercent(summary.SundayChange);
            this.litWeekC.Text = FormatPercent(summary.WeekChange);

        }

        private string FormatPercent(decimal percentage)
        {

            if (percentage == 0) return "<span class=\"neutral\">0.00%</span>";
            if (percentage > 0) return "<span class=\"positive\">+" + percentage.ToString("0.##%") + "</span>";

            return "<span class=\"negative\">" + percentage.ToString("0.##%") + "</span>";

        }

    }
}