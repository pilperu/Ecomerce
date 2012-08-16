using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI;
using MerchantTribe.Commerce.Catalog;

namespace MerchantTribeStore
{

    public partial class BVAdmin_Catalog_ProductsEdit_TabsDelete : BaseAdminJsonPage
    {

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);

            if (!Page.IsPostBack)
            {
                string TabId = Request.Form["id"];
                string ProductId = Request.Form["bvin"];
                Delete(TabId, ProductId);
                string Redirect = Request.Form["redirect"];
                if ((Redirect == "1") | (Redirect == "y"))
                {
                    Response.Redirect("ProductsEdit_Tabs.aspx?id=" + ProductId);
                }
            }
        }

        private void Delete(string id, string bvin)
        {
            bool result = false;

            Product p = MTApp.CatalogServices.Products.Find(bvin);
            if ((p != null))
            {
                List<ProductDescriptionTab> newTabs = new List<ProductDescriptionTab>();

                int currentSort = 1;
                foreach (ProductDescriptionTab t in p.Tabs)
                {
                    if (t.Bvin != id)
                    {
                        t.SortOrder = currentSort;
                        currentSort += 1;
                        newTabs.Add(t);
                    }
                }
                p.Tabs = newTabs;
                result = MTApp.CatalogServices.ProductsUpdateWithSearchRebuild(p);
            }

            if ((result))
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