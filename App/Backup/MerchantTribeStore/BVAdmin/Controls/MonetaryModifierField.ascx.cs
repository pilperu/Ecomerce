using System;
using System.Web.UI.WebControls;
using MerchantTribe.Commerce;
using MerchantTribe.Commerce.Controls;

namespace MerchantTribeStore
{

    partial class BVAdmin_Controls_MonetaryModifierField : ModificationControl<decimal>, ITextBoxBasedControl
    {

        private enum Modes
        {
            SetTo = 0,
            IncreaseByAmount = 1,
            DecreaseByAmount = 2,
            IncreaseByPercent = 3,
            DecreaseByPercent = 4
        }

        public void AddTextBoxAttribute(string key, string value)
        {
            MonetaryTextBox.Attributes.Add(key, value);
        }

        public override decimal ApplyChanges(decimal item)
        {
            decimal val = 0;
            if (decimal.TryParse(this.MonetaryTextBox.Text, out val))
            {
                if (this.MonetaryDropDownList.SelectedIndex == (int)Modes.SetTo)
                {
                    return val;
                }
                else if (this.MonetaryDropDownList.SelectedIndex == (int)Modes.IncreaseByAmount)
                {
                    return MerchantTribe.Commerce.Utilities.Money.ApplyIncreasedAmount(item, val);
                }
                else if (this.MonetaryDropDownList.SelectedIndex == (int)Modes.DecreaseByAmount)
                {
                    return MerchantTribe.Commerce.Utilities.Money.ApplyDiscountAmount(item, val);
                }
                else if (this.MonetaryDropDownList.SelectedIndex == (int)Modes.IncreaseByPercent)
                {
                    return MerchantTribe.Commerce.Utilities.Money.ApplyIncreasedPercent(item, val);
                }
                else if (this.MonetaryDropDownList.SelectedIndex == (int)Modes.DecreaseByPercent)
                {
                    return MerchantTribe.Commerce.Utilities.Money.ApplyDiscountPercent(item, val);
                }
            }
            else
            {
                return item;
            }

            return item;
        }

    }
}