using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MerchantTribe.Commerce.Catalog;

namespace MerchantTribeStore.BVAdmin.Catalog
{
    public partial class Products_Edit_Images_Sort : BaseAdminJsonPage
    {

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);

            if (!Page.IsPostBack)
            {
                string ids = Request.Form["ids"];
                string productBvin = Request.Form["bvin"];
                Resort(ids, productBvin);
            }

        }

        private void Resort(string ids, string productBvin)
        {

            string[] sorted = ids.Split(',');
            List<string> l = new List<string>();
            foreach (string id in sorted)
            {
                string temp = id.Replace("img", "");
                l.Add(temp);
            }
                        
            if ((MTApp.CatalogServices.ProductImages.Resort(productBvin, l)))
            {
                this.litOutput.Text = "{\"result\":true}";
            }
            else
            {
                this.litOutput.Text = "{\"result\":false}";
            }

        }

    }
}