using System;
using System.Web;
using System.Web.UI.WebControls;
using MerchantTribe.Commerce;
using MerchantTribe.Commerce.Catalog;
using MerchantTribe.Commerce.Content;

namespace MerchantTribeStore
{

    partial class BVAdmin_Controls_ProductEditingDisplay : BVUserControl
    {

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (Request.QueryString["id"] != null)
            {
                Product prod = MyPage.MTApp.CatalogServices.Products.Find(Request.QueryString["id"]);
                productImage.ImageUrl = MerchantTribe.Commerce.Storage.DiskStorage.ProductImageUrlSmall(this.MyPage.MTApp, prod.Bvin, prod.ImageFileSmall, true);
                productLabel.Text = prod.ProductName;
                productSkuLabel.Text = prod.Sku;
                productPrice.Text = prod.SitePrice.ToString("C");
            }

        }
    }
}