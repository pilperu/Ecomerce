

using MerchantTribe.Commerce;
using MerchantTribe.Commerce.Marketing;

namespace MerchantTribeStore
{

    partial class BVAdmin_Controls_PercentAmountSelection : System.Web.UI.UserControl
    {

        private string _validationGroup = string.Empty;

        public AmountTypes AmountType
        {
            get { return (AmountTypes)AmountDropDownList.SelectedIndex; }
            set { AmountDropDownList.SelectedIndex = (int)value; }
        }

        public decimal Amount
        {
            get { return decimal.Parse(AmountTextBox.Text, System.Globalization.NumberStyles.Currency); }
            set
            {
                if (this.AmountType == AmountTypes.MonetaryAmount)
                {
                    AmountTextBox.Text = value.ToString("c");
                }
                else
                {
                    AmountTextBox.Text = value.ToString();
                }

            }
        }

        public string ValidationGroup
        {
            get { return _validationGroup; }
            set
            {
                _validationGroup = value;
                PercentCustomValidator.ValidationGroup = _validationGroup;
            }
        }

        protected void PercentCustomValidator_ServerValidate(object source, System.Web.UI.WebControls.ServerValidateEventArgs args)
        {
            if (AmountDropDownList.SelectedIndex == (int)AmountTypes.Percent)
            {
                if (source is System.Web.UI.WebControls.CustomValidator)
                {
                    ((System.Web.UI.WebControls.CustomValidator)source).ErrorMessage = "Percent must be between 0.00 and 100.00 percent.";
                }
                decimal val = 0;
                if (decimal.TryParse(AmountTextBox.Text,
                                    System.Globalization.NumberStyles.Number,
                                    System.Threading.Thread.CurrentThread.CurrentUICulture, out val))
                {
                    if (val < 0 | val > 100)
                    {
                        args.IsValid = false;
                    }
                }
                else
                {
                    args.IsValid = false;
                }
            }
            else if (AmountDropDownList.SelectedIndex == (int)AmountTypes.MonetaryAmount)
            {
                if (source is System.Web.UI.WebControls.CustomValidator)
                {
                    ((System.Web.UI.WebControls.CustomValidator)source).ErrorMessage = "Value must be a monetary amount.";
                }
                decimal val = 0;
                if (decimal.TryParse(AmountTextBox.Text, System.Globalization.NumberStyles.Currency,
                            System.Threading.Thread.CurrentThread.CurrentUICulture, out val))
                {
                    if (val < 0)
                    {
                        args.IsValid = false;
                    }
                }
                else
                {
                    args.IsValid = false;
                }
            }
        }
    }

}