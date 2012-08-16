
using MerchantTribe.Commerce;
using MerchantTribe.Commerce.Controls;

namespace MerchantTribeStore
{

    partial class BVAdmin_Controls_HtmlModifierField : ModificationControl<string>
    {

        private enum Modes
        {
            SetTo = 0,
            AppendTo = 1
        }

        public override string ApplyChanges(string item)
        {
            if (HtmlDropDownList.SelectedIndex == (int)Modes.SetTo)
            {
                return HtmlEditor.Text;
            }
            else if (HtmlDropDownList.SelectedIndex == (int)Modes.AppendTo)
            {
                return item + HtmlEditor.Text;
            }
            return item;
        }
    }
}