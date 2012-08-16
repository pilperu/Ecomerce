using System;
using MerchantTribe.Commerce;

namespace MerchantTribeStore
{

    partial class BVAdmin_Controls_BooleanModifierField : MerchantTribe.Commerce.Controls.ModificationControl<bool>
    {

        public enum Modes
        {
            YesNo = 0,
            EnabledDisabled = 1
        }

        public Modes DisplayMode
        {
            get
            {
                object obj = ViewState["mode"];
                if (obj != null)
                {
                    return (Modes)obj;
                }
                else
                {
                    return Modes.YesNo;
                }

            }
            set { ViewState["mode"] = value; }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (!(Page.IsPostBack))
            {
                if (DisplayMode == Modes.YesNo)
                {
                    BooleanDropDownList.Items.Clear();
                    System.Web.UI.WebControls.ListItem li = new System.Web.UI.WebControls.ListItem("Yes", "1", true);
                    li.Selected = true;
                    BooleanDropDownList.Items.Add(li);
                    li = new System.Web.UI.WebControls.ListItem("No", "0", true);
                    BooleanDropDownList.Items.Add(li);
                }
                else if (DisplayMode == Modes.EnabledDisabled)
                {
                    BooleanDropDownList.Items.Clear();
                    System.Web.UI.WebControls.ListItem li = new System.Web.UI.WebControls.ListItem("Enabled", "1", true);
                    li.Selected = true;
                    BooleanDropDownList.Items.Add(li);
                    li = new System.Web.UI.WebControls.ListItem("Disabled", "0", true);
                    BooleanDropDownList.Items.Add(li);
                }
            }
        }

        public override bool ApplyChanges(bool item)
        {
            if (this.BooleanDropDownList.SelectedValue == "0")
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}