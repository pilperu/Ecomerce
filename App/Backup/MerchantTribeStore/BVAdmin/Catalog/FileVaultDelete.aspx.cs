using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MerchantTribe.Commerce.Catalog;

namespace MerchantTribeStore.BVAdmin.Catalog
{
    public partial class FileVaultDelete : BaseAdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                string bvin = Request.QueryString["id"];
                bool result = MTApp.CatalogServices.ProductFiles.Delete(bvin);
                if (result)
                {
                    Response.Redirect("FileVault.aspx", false);
                }
                else
                {
                    Response.Redirect("FileVault.aspx", false);
                }                
                
            }
        }
    }
}