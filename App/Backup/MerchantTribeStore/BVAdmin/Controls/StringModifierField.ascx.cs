
using MerchantTribe.Commerce;
using MerchantTribe.Commerce.Controls;

namespace MerchantTribeStore
{

    partial class BVAdmin_Controls_StringModifierField : ModificationControl<string>, ITextBoxBasedControl
    {

        private enum Modes
        {
            SetTo = 0,
            AppendTo = 1,
            RemoveFromEnd = 2
        }

        public void AddTextBoxAttribute(string key, string value)
        {
            this.StringTextBox.Attributes.Add(key, value);
        }

        public override string ApplyChanges(string item)
        {
            if (this.StringDropDownList.SelectedIndex == (int)Modes.SetTo)
            {
                return this.StringTextBox.Text;
            }
            else if (this.StringDropDownList.SelectedIndex == (int)Modes.AppendTo)
            {
                return item + this.StringTextBox.Text;
            }
            else if (this.StringDropDownList.SelectedIndex == (int)Modes.RemoveFromEnd)
            {
                if (item.EndsWith(this.StringTextBox.Text))
                {
                    return item.Remove(item.LastIndexOf(this.StringTextBox.Text));
                }
                return item;
            }
            return item;
        }
    }
}