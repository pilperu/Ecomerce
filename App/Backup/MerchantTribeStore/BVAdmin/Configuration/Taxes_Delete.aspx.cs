using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MerchantTribe.Commerce.Taxes;

namespace MerchantTribeStore
{

    public partial class BVAdmin_Configuration_Taxes_Delete : BaseAdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string taxid = Request.QueryString["id"];
            string scheduleId = Request.QueryString["sid"];

            MTApp.OrderServices.Taxes.Delete(long.Parse(taxid));

            Response.Redirect("Taxes_Edit.aspx?id=" + scheduleId);
        }
    }
}