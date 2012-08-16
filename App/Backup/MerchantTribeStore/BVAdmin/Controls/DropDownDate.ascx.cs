using System;
using System.Globalization;
using System.Web.UI.WebControls;

namespace MerchantTribeStore
{

    partial class BVAdmin_Controls_DropDownDate : System.Web.UI.UserControl
    {

        public void SetYearRange(int startYear, int endYear)
        {
            this.EnsureChildControls();
            YearList.Items.Clear();
            for (int i = startYear; i <= endYear; i++)
            {
                YearList.Items.Add(i.ToString());
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (!Page.IsPostBack)
            {
                if (this.DayList.Items.Count < 1)
                {
                    PopulateDefaults();
                }
            }
        }
        public void PopulateDefaults()
        {

            for (int i = 1; i <= 12; i++)
            {
                ListItem li = new ListItem();
                li.Value = i.ToString();
                li.Text = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(i);
                MonthList.Items.Add(li);
                li = null;
            }

            for (int i = 1; i <= 31; i++)
            {
                DayList.Items.Add(i.ToString());
            }

            for (int i = System.DateTime.Now.AddYears(-5).Year; i <= System.DateTime.Now.AddYears(10).Year; i++)
            {
                YearList.Items.Add(i.ToString());
            }

        }

        public System.DateTime SelectedDate
        {
            get
            {
                try
                {
                    System.DateTime d = new System.DateTime(int.Parse(YearList.SelectedValue),
                                                            int.Parse(MonthList.SelectedValue),
                                                            int.Parse(DayList.SelectedValue));
                    return d;
                }
                catch (Exception ex)
                {
                    MerchantTribe.Commerce.EventLog.LogEvent(ex);
                    return new System.DateTime(1900, 1, 1);
                }
            }
            set
            {
                if (DayList.Items.Count < 1) PopulateDefaults();

                if (DayList.Items.FindByValue(value.Day.ToString()) != null)
                {
                    DayList.ClearSelection();
                    DayList.Items.FindByValue(value.Day.ToString()).Selected = true;
                }
                if (MonthList.Items.FindByValue(value.Month.ToString()) != null)
                {
                    MonthList.ClearSelection();
                    MonthList.Items.FindByValue(value.Month.ToString()).Selected = true;
                }
                if (YearList.Items.FindByValue(value.Year.ToString()) != null)
                {
                    YearList.ClearSelection();
                    YearList.Items.FindByValue(value.Year.ToString()).Selected = true;
                }
            }
        }

    }
}