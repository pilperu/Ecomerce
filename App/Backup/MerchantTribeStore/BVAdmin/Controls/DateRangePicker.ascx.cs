using System;
using MerchantTribe.Commerce.Controls;
using MerchantTribe.Commerce.Utilities;

namespace MerchantTribeStore
{

    partial class BVAdmin_Controls_DateRangePicker : System.Web.UI.UserControl
    {

        private MerchantTribe.Commerce.Utilities.DateRange _range = new MerchantTribe.Commerce.Utilities.DateRange();

        public delegate void RangeTypeChangedDelegate(System.EventArgs e);
        public event RangeTypeChangedDelegate RangeTypeChanged;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }

        protected void lstRangeType_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (this.lstRangeType.SelectedValue == "99")
            {
                this.pnlCustom.Visible = true;
                this.StartDateField.SelectedDate = this.StartDate;
                this.EndDateField.SelectedDate = this.EndDate;
            }
            else
            {
                this.pnlCustom.Visible = false;
                this.StartDateField.SelectedDate = this.StartDate;
                this.EndDateField.SelectedDate = this.EndDate;
            }
            if (RangeTypeChanged != null)
            {
                RangeTypeChanged(new EventArgs());
            }
        }

        public DateTime StartDate
        {
            get
            {
                if (RangeType == MerchantTribe.Commerce.Utilities.DateRangeType.Custom)
                {
                    return MerchantTribe.Web.Dates.ZeroOutTime(this.StartDateField.SelectedDate);
                }
                else
                {
                    _range.RangeType = this.RangeType;
                    return _range.StartDate;
                }
            }
            set
            {
                this.StartDateField.SelectedDate = value;
                RangeType = MerchantTribe.Commerce.Utilities.DateRangeType.Custom;
            }
        }

        public DateTime StartDateForZone(TimeZoneInfo timezone)
        {
            if (RangeType == DateRangeType.Custom) return MerchantTribe.Web.Dates.ZeroOutTime(this.StartDateField.SelectedDate);

            _range.RangeType = this.RangeType;
            _range.CalculateDatesFromType(TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timezone));
            return _range.StartDate;
        }

        public DateTime EndDate
        {
            get
            {
                if (RangeType == MerchantTribe.Commerce.Utilities.DateRangeType.Custom)
                {
                    return MerchantTribe.Web.Dates.MaxOutTime(this.EndDateField.SelectedDate);
                }
                else
                {
                    _range.RangeType = this.RangeType;
                    return _range.EndDate;
                }
            }
            set
            {
                this.EndDateField.SelectedDate = value;
                RangeType = MerchantTribe.Commerce.Utilities.DateRangeType.Custom;
            }
        }

        public DateTime EndDateForZone(TimeZoneInfo timezone)
        {
            if (RangeType == DateRangeType.Custom) return MerchantTribe.Web.Dates.MaxOutTime(this.EndDateField.SelectedDate);

            _range.RangeType = this.RangeType;
            _range.CalculateDatesFromType(TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timezone));
            return _range.EndDate;
        }

        public MerchantTribe.Commerce.Utilities.DateRangeType RangeType
        {
            get { return (MerchantTribe.Commerce.Utilities.DateRangeType)int.Parse(this.lstRangeType.SelectedValue); }
            set
            {
                if (this.lstRangeType.Items.FindByValue(((int)value).ToString()) != null)
                {
                    this.lstRangeType.ClearSelection();
                    this.lstRangeType.Items.FindByValue(((int)value).ToString()).Selected = true;
                }
            }
        }

    }
}