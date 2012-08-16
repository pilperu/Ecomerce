using System.Web.UI;
using MerchantTribe.Commerce.Catalog;

namespace MerchantTribeStore
{

    partial class BVAdmin_Catalog_ProductChoices_Delete : BaseAdminJsonPage
    {

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);

            if (!Page.IsPostBack)
            {
                string ChoiceId = Request.Form["id"];
                string ProductId = Request.Form["bvin"];
                Delete(ChoiceId, ProductId);
                string Redirect = Request.Form["redirect"];
                if ((Redirect == "1") | (Redirect == "y"))
                {
                    Response.Redirect("ProductChoices.aspx?id=" + ProductId);
                }
            }
        }

        private void Delete(string id, string bvin)
        {
            bool result = false;

            Product p = MTApp.CatalogServices.Products.Find(bvin);
            if ((p != null))
            {
                result = MTApp.CatalogServices.ProductsRemoveOption(p, id);                
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