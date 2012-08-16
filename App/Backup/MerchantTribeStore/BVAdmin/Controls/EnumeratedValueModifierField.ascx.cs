using MerchantTribe.Commerce.Controls;
using MerchantTribe.Commerce;

namespace MerchantTribeStore
{

    partial class BVAdmin_Controls_EnumeratedValueModifierField : ModificationControl<string>
    {

        public bool DisplayNone
        {
            get
            {
                object val = ViewState["displayNone"];
                if (val != null)
                {
                    return (bool)val;
                }
                else
                {
                    return false;
                }
            }
            set { ViewState["displayNone"] = value; }
        }

        public object Datasource
        {
            get { return EnumeratedValueDropDownList.DataSource; }
            set { EnumeratedValueDropDownList.DataSource = value; }
        }

        public string DataTextField
        {
            get { return EnumeratedValueDropDownList.DataTextField; }
            set { EnumeratedValueDropDownList.DataTextField = value; }
        }

        public string DataValueField
        {
            get { return EnumeratedValueDropDownList.DataValueField; }
            set { EnumeratedValueDropDownList.DataValueField = value; }
        }

        public override string ApplyChanges(string item)
        {
            return EnumeratedValueDropDownList.SelectedValue;
        }

        public override void DataBind()
        {
            EnumeratedValueDropDownList.DataBind();
            if (DisplayNone)
            {
                System.Web.UI.WebControls.ListItem li = new System.Web.UI.WebControls.ListItem("None", "");
                EnumeratedValueDropDownList.Items.Insert(0, li);
            }
        }
    }


}