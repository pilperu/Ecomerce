using System.Web.UI;
using System.Collections.Generic;
using MerchantTribe.Commerce.Catalog;

namespace MerchantTribeStore
{

    partial class BVAdmin_Catalog_ProductChoices_Sort : BaseAdminJsonPage
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
                // strip off 'item_' prefix because GUID ids
                // may start with numbers which is invalid for HTML
                string temp = id.Replace("item_", "");
                l.Add(temp);
            }

            if ((MTApp.CatalogServices.ProductsXOptions.ResortOptionsForProduct(productBvin, l)))
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