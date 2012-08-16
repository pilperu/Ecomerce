using System.Web.UI;
using MerchantTribe.Commerce.Catalog;

namespace MerchantTribeStore
{

    partial class BVAdmin_Catalog_ProductVariants_Update : BaseAdminJsonPage
    {

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);

            if (!Page.IsPostBack)
            {
                string bvin = Request.Form["id"];
                string sku = Request.Form["sku"];
                string price = Request.Form["price"];
                Update(bvin, sku, price);
            }

        }

        private void Update(string bvin, string sku, string price)
        {

            Variant item = MTApp.CatalogServices.ProductVariants.Find(bvin);
            if ((item != null))
            {

                item.Sku = sku;
                decimal p = item.Price;
                if ((decimal.TryParse(price, out p)))
                {
                    item.Price = p;
                }

                if (MTApp.CatalogServices.ProductVariants.Update(item))
                {
                    this.litOutput.Text = "{\"result\":true}";
                }
                else
                {
                    this.litOutput.Text = "{\"result\":false}";
                }
            }
            else
            {
                this.litOutput.Text = "{\"result\":false}";
            }
        }

    }
}