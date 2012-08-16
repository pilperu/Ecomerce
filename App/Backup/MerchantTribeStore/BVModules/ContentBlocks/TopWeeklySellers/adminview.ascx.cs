using System;
using System.Web.UI;
using MerchantTribe.Commerce.Catalog;
using MerchantTribe.Commerce.Content;
using System.Collections.Generic;

namespace MerchantTribeStore
{

    partial class BVModules_ContentBlocks_Top_Weekly_Sellers_adminview : BVModule
    {

        private DateTime _StartDate = DateTime.Now;
        private DateTime _EndDate = DateTime.Now;

        public DateTime StartDate
        {
            get { return _StartDate; }
            set { _StartDate = value; }
        }
        public DateTime EndDate
        {
            get { return _EndDate; }
            set { _EndDate = value; }
        }

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);

            if (!Page.IsPostBack)
            {
                System.DateTime c = DateTime.Now;
                CalculateDates(c);
                LoadProducts();
            }
        }

        public void CalculateDates(DateTime currentTime)
        {
            _StartDate = FindStartOfWeek(currentTime);
            _EndDate = _StartDate.AddDays(7);
            _EndDate = _EndDate.AddMilliseconds(-1);
        }

        private DateTime FindStartOfWeek(DateTime currentDate)
        {
            DateTime result = currentDate;
            switch (currentDate.DayOfWeek)
            {
                case DayOfWeek.Sunday:
                    result = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, 0, 0, 0, 0);
                    break;
                case DayOfWeek.Monday:
                    result = currentDate.AddDays(-1);
                    break;
                case DayOfWeek.Tuesday:
                    result = currentDate.AddDays(-2);
                    break;
                case DayOfWeek.Wednesday:
                    result = currentDate.AddDays(-3);
                    break;
                case DayOfWeek.Thursday:
                    result = currentDate.AddDays(-4);
                    break;
                case DayOfWeek.Friday:
                    result = currentDate.AddDays(-5);
                    break;
                case DayOfWeek.Saturday:
                    result = currentDate.AddDays(-6);
                    break;
            }
            result = new DateTime(result.Year, result.Month, result.Day, 0, 0, 0, 0);
            return result;
        }

        private void LoadProducts()
        {
            System.DateTime s = StartDate;
            System.DateTime e = EndDate;

            List<Product> t = MyPage.MTApp.ReportingTopSellersByDate(s, e, 10);

            this.GridView1.DataSource = t;
            this.GridView1.DataBind();

        }

    }
}