namespace MerchantTribeStore
{

    partial class BVAdmin_Controls_TimespanPicker : System.Web.UI.UserControl
    {

        public int Months
        {
            get
            {
                if (MonthsDropDownList.SelectedIndex != 0)
                {
                    return int.Parse(MonthsDropDownList.SelectedValue);
                }
                else
                {
                    return 0;
                }
            }
            set
            {
                if (value > 0)
                {
                    MonthsDropDownList.SelectedIndex = value;
                }
            }
        }

        public int Days
        {
            get
            {
                if (DaysDropDownList.SelectedIndex != 0)
                {
                    return int.Parse(DaysDropDownList.SelectedValue);
                }
                else
                {
                    return 0;
                }
            }
            set
            {
                if (value > 0)
                {
                    DaysDropDownList.SelectedIndex = value;
                }
            }
        }

        public int Hours
        {
            get
            {
                if (HoursDropDownList.SelectedIndex != 0)
                {
                    return int.Parse(HoursDropDownList.SelectedValue);
                }
                else
                {
                    return 0;
                }
            }
            set
            {
                if (value > 0)
                {
                    HoursDropDownList.SelectedIndex = value;
                }
            }
        }

        public int Minutes
        {
            get
            {
                if (MinutesDropDownList.SelectedIndex != 0)
                {
                    return int.Parse(MinutesDropDownList.SelectedValue);
                }
                else
                {
                    return 0;
                }
            }
            set
            {
                if (value > 0)
                {
                    MinutesDropDownList.SelectedIndex = value;
                }
            }
        }
    }
}