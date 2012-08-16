using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI;
using MerchantTribe.Commerce.Catalog;

namespace MerchantTribeStore
{

    public partial class BVAdmin_Catalog_ProductsEdit_TabsSort : BaseAdminJsonPage
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
                l.Add(id);
            }

            Product p = MTApp.CatalogServices.Products.Find(productBvin);
            int currentSort = 1;
            if (p != null)
            {
                foreach (string tabid in l)
                {
                    foreach (ProductDescriptionTab t in p.Tabs)
                    {
                        if (t.Bvin == tabid)
                        {
                            t.SortOrder = currentSort;
                            currentSort += 1;
                        }
                    }
                }
            }

            if (MTApp.CatalogServices.ProductsUpdateWithSearchRebuild(p))
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