using System;
using System.Web.UI.WebControls;

namespace MerchantTribe.Commerce.Content
{

	public class BVShippingModule : BVModule
	{

		private Shipping.ShippingMethod _shippingMethod = null;

		public Shipping.ShippingMethod ShippingMethod {
			get { return _shippingMethod; }
			set { _shippingMethod = value; }
		}

        public void AddHighlightColors(DropDownList lst)
        {
            ListItem liNone = new ListItem("- None -", "");
            lst.Items.Add(liNone);

            ListItem liBlue = new ListItem("Blue", "Blue");
            liBlue.Attributes.Add("style", "background:#8AA5CE;color:#fff;");
            lst.Items.Add(liBlue);

            ListItem liYellow = new ListItem("Yellow", "Yellow");
            liYellow.Attributes.Add("style", "background:#FFF958;color:#000;");
            lst.Items.Add(liYellow);            

            ListItem liLime = new ListItem("Lime", "Lime");
            liLime.Attributes.Add("style", "background:#5EFF69;color:#000;");
            lst.Items.Add(liLime);

            ListItem liOrange = new ListItem("Orange", "Orange");
            liOrange.Attributes.Add("style", "background:#F19E32;color:#000;");
            lst.Items.Add(liOrange);

            ListItem liPurple = new ListItem("Purple", "Purple");
            liPurple.Attributes.Add("style", "background:#D377D2;color:#000;");
            lst.Items.Add(liPurple);

            ListItem liTan = new ListItem("Tan", "Tan");
            liTan.Attributes.Add("style", "background:#C9B193;color:#000;");
            lst.Items.Add(liTan);

            
        }
	}
}
