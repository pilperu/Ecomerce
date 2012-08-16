using System;
using System.Web.UI.WebControls;
using MerchantTribe.Commerce.Controls;
using MerchantTribe.Commerce;

namespace MerchantTribeStore
{

    partial class BVAdmin_Controls_DateModifierField : MerchantTribe.Commerce.Controls.ModificationControl<System.DateTime>
    {

        private enum Modes
        {
            SetTo = 0,
            AddDays = 1,
            SubtractDays = 2,
            AddMonths = 3,
            SubtractMonths = 4,
            AddYears = 5,
            SubtractYears = 6
        }

        public void InitializeValue(string val)
        {
            this.DateTextBox.Text = val;
        }

        public override System.DateTime ApplyChanges(System.DateTime item)
        {
            int num = 0;
            try
            {
                if (DateDropDownList.SelectedIndex > (int)Modes.SetTo)
                {
                    if (!int.TryParse(DateTextBox.Text, out  num))
                    {
                        num = 0;
                    }
                }
                if (DateDropDownList.SelectedIndex == (int)Modes.SetTo)
                {
                    System.DateTime val;
                    if (System.DateTime.TryParse(DateTextBox.Text, out val))
                    {
                        return val;
                    }
                    else
                    {
                        return item;
                    }
                }
                else if (DateDropDownList.SelectedIndex == (int)Modes.AddDays)
                {
                    return item.AddDays(num);
                }
                else if (DateDropDownList.SelectedIndex == (int)Modes.SubtractDays)
                {
                    return item.AddDays((num * -1));
                }
                else if (DateDropDownList.SelectedIndex == (int)Modes.AddMonths)
                {
                    return item.AddMonths(num);
                }
                else if (DateDropDownList.SelectedIndex == (int)Modes.SubtractMonths)
                {
                    return item.AddMonths((num * -1));
                }
                else if (DateDropDownList.SelectedIndex == (int)Modes.AddYears)
                {
                    return item.AddYears(num);
                }
                else if (DateDropDownList.SelectedIndex == (int)Modes.SubtractYears)
                {
                    return item.AddYears((num * -1));
                }
            }
            catch (System.ArgumentOutOfRangeException ex)
            {
                return item;
            }
            return item;
        }

        protected void CustomValidator1_ServerValidate(object source, System.Web.UI.WebControls.ServerValidateEventArgs args)
        {
            if (args.Value != string.Empty)
            {
                if (DateDropDownList.SelectedIndex == (int)Modes.SetTo)
                {
                    System.DateTime temp;
                    if (!System.DateTime.TryParse(args.Value, out temp))
                    {
                        ((CustomValidator)source).ErrorMessage = "Field must be a valid date.";
                        args.IsValid = false;
                    }
                }
                else
                {
                    int temp;
                    if (!int.TryParse(args.Value, out temp))
                    {
                        ((CustomValidator)source).ErrorMessage = "Field must be a number";
                        args.IsValid = false;
                    }
                }
            }
        }
    }
}