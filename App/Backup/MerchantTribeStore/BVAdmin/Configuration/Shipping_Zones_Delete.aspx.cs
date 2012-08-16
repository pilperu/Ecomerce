using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MerchantTribe.Commerce.Shipping;

namespace MerchantTribeStore
{

    public partial class BVAdmin_Configuration_Shipping_Zones_Delete : BaseAdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string zoneid = Request.QueryString["id"];
            MTApp.OrderServices.ShippingZones.Delete(long.Parse(zoneid));
            Response.Redirect("Shipping_Zones.aspx");
        }
    }
}