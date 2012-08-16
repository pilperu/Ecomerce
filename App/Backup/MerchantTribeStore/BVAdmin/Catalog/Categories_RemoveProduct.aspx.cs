using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MerchantTribe.Commerce.Catalog;


namespace MerchantTribeStore.BVAdmin.Catalog
{
    public partial class Categories_RemoveProduct : BaseAdminJsonPage
    {

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);

            if (!Page.IsPostBack)
            {
                string productId = Request.Form["id"];
                string categoryId = Request.Form["categoryid"];

                Delete(productId, categoryId);
            }
        }

        private void Delete(string productId, string categoryId)
        {
            bool result = false;

            CategoryProductAssociationRepository repo = MTApp.CatalogServices.CategoriesXProducts;
             
            result = repo.RemoveProductFromCategory(productId, categoryId);

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