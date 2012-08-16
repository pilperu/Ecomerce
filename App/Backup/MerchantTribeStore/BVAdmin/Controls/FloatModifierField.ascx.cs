using System;
using MerchantTribe.Commerce;
using MerchantTribe.Commerce.Controls;

namespace MerchantTribeStore
{

    partial class BVAdmin_Controls_FloatModifierField : ModificationControl<double>, ITextBoxBasedControl
    {

        private enum Modes
        {
            SetTo = 0,
            AddTo = 1,
            SubtractFrom = 2
        }

        public void AddTextBoxAttribute(string key, string value)
        {
            this.FloatTextBox.Attributes.Add(key, value);
        }

        public override double ApplyChanges(double item)
        {
            if (this.FloatDropDownList.SelectedIndex == (int)Modes.SetTo)
            {
                double val = 0;
                if (double.TryParse(this.FloatTextBox.Text, out val))
                {
                    return val;
                }
                else
                {
                    return item;
                }
            }
            else if (this.FloatDropDownList.SelectedIndex == (int)Modes.AddTo)
            {
                double val = 0;
                if (double.TryParse(this.FloatTextBox.Text, out val))
                {
                    return (item + val);
                }
                else
                {
                    return item;
                }
            }
            else if (this.FloatDropDownList.SelectedIndex == (int)Modes.SubtractFrom)
            {
                double val = 0;
                if (double.TryParse(this.FloatTextBox.Text, out val))
                {
                    return (item - val);
                }
                else
                {
                    return item;
                }
            }

            return item;
        }
    }
}