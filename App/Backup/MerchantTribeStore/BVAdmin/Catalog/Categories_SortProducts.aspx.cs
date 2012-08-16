using System.Collections.Generic;
using System.Web.UI;
using MerchantTribe.Commerce.Catalog;

namespace MerchantTribeStore
{

    partial class BVAdmin_Catalog_Categories_SortProducts : BaseAdminJsonPage
    {

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);

            if (!Page.IsPostBack)
            {
                string ids = Request.Form["ids"];
                string catId = Request.Form["categoryid"];
                Resort(ids, catId);
            }

        }

        private void Resort(string ids, string catId)
        {

            string[] sorted = ids.Split(',');
            List<string> l = new List<string>();
            foreach (string id in sorted)
            {
                l.Add(id);
            }

            if ((this.MTApp.CatalogServices.CategoriesXProducts.ResortProducts(catId, l)))
            {
                this.litOutput.Text = "true";
            }
            else
            {
                this.litOutput.Text = "false";
            }

        }

    }
}