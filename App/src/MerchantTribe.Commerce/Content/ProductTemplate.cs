using System;
using System.Collections.ObjectModel;
using System.Web;

namespace MerchantTribe.Commerce.Content
{

	public abstract class ProductTemplate : BVModule
	{

		private Catalog.Product _ModuleProduct;
		private int _ModuleProductQuantity = 1;

		public Catalog.Product ModuleProduct {
			get { return _ModuleProduct; }
			set { _ModuleProduct = value; }
		}

		public int ModuleProductQuantity {
			get { return _ModuleProductQuantity; }
			set { _ModuleProductQuantity = value; }
		}

		protected string GetDestinationUrl()
		{
			if (WebAppSettings.RedirectToCartAfterAddProduct) {

                Catalog.CatalogService catalogServices = Catalog.CatalogService.InstantiateForDatabase(MyPage.MTApp.CurrentRequestContext);
                int relatedCount = catalogServices.ProductRelationships.FindForProduct(this.ModuleProduct.Bvin).Count;

                // TODO: verify for cross-sells and up-sells
				if (relatedCount > 0) {
					return "~/AdditionalProductInfo.aspx?id=" + HttpUtility.UrlEncode(this.ModuleProduct.Bvin) + "&quantity=" + this.ModuleProductQuantity.ToString();
				}
				else {
					return "~/Cart.aspx";
				}
			}
			else {
				return "";
			}
		}
	}
}

