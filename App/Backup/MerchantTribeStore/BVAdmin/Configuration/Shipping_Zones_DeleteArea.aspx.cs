using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MerchantTribe.Commerce.Shipping;

namespace MerchantTribeStore
{

    public partial class BVAdmin_Configuration_Shipping_Zones_DeleteArea : BaseAdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string zoneid = Request.QueryString["id"];
            string countryIso = Request.QueryString["country"];
            string region = Request.QueryString["region"];

            MTApp.OrderServices.ShippingZoneRemoveArea(long.Parse(zoneid),countryIso, region);
            Response.Redirect("Shipping_Zones_Edit.aspx?id=" + zoneid);
        }
    }
}