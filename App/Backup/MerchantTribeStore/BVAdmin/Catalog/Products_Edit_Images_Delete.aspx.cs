using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MerchantTribe.Commerce.Catalog;

namespace MerchantTribeStore.BVAdmin.Catalog
{
    public partial class Products_Edit_Images_Delete : BaseAdminJsonPage
    {

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);

            if (!Page.IsPostBack)
            {
                string imageBvin = Request.Form["id"];
                string ProductId = Request.Form["bvin"];
                Delete(imageBvin, ProductId);
            }
        }

        private void Delete(string imageBvin, string productBvin)
        {
            bool result = false;

            ProductImage img = MTApp.CatalogServices.ProductImages.Find(imageBvin);
            if (img.ProductId == productBvin)
            {
                result = MTApp.CatalogServices.ProductImages.Delete(imageBvin);
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