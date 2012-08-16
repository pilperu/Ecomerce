using System.Web.UI;
using MerchantTribe.Commerce.Catalog;

namespace MerchantTribeStore
{

    partial class BVAdmin_Catalog_OptionItems_Update : BaseAdminJsonPage
    {

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);


            if (!Page.IsPostBack)
            {
                string bvin = Request.Form["id"];
                string name = Request.Form["name"];
                string isLabel = Request.Form["islabel"];
                string price = Request.Form["price"];
                string weight = Request.Form["weight"];

                Update(bvin, name, isLabel, price, weight);
            }

        }

        private void Update(string bvin, string name, string isLabel, string price, string weight)
        {

            OptionItem item = MTApp.CatalogServices.ProductOptions.OptionItemFind(bvin);
            if ((item != null))
            {

                item.Name = name;

                isLabel = isLabel ?? string.Empty; // null check on this before length check
                if (isLabel.Trim().Length > 0)
                {
                    item.IsLabel = true;
                }
                else
                {
                    item.IsLabel = false;
                }

                decimal p = item.PriceAdjustment;
                if ((decimal.TryParse(price, out p)))
                {
                    item.PriceAdjustment = p;
                }
                decimal w = item.WeightAdjustment;
                if ((decimal.TryParse(weight, out w)))
                {
                    item.WeightAdjustment = w;
                }

                if (MTApp.CatalogServices.ProductOptions.OptionItemUpdate(item))
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