
using MerchantTribe.Commerce;
using MerchantTribe.Commerce.Controls;

namespace MerchantTribeStore
{

    partial class BVAdmin_Controls_IntegerModifierField : ModificationControl<int>, ITextBoxBasedControl
    {

        private enum Modes
        {
            SetTo = 0,
            AddTo = 1,
            SubtractFrom = 2
        }

        public void AddTextBoxAttribute(string key, string value)
        {
            this.IntegerTextBox.Attributes.Add(key, value);
        }

        public override int ApplyChanges(int item)
        {
            int val = 0;
            if (int.TryParse(this.IntegerTextBox.Text, out val))
            {
                if (this.IntegerDropDownList.SelectedIndex == (int)Modes.SetTo)
                {
                    return val;
                }
                else if (this.IntegerDropDownList.SelectedIndex == (int)Modes.AddTo)
                {
                    return (item + val);
                }
                else if (this.IntegerDropDownList.SelectedIndex == (int)Modes.SubtractFrom)
                {
                    return (item - val);
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