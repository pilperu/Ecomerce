using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using MerchantTribe.Commerce;
using MerchantTribe.Commerce.Catalog;
using MerchantTribe.Commerce.Content;
using MerchantTribe.Commerce.Utilities;

namespace MerchantTribeStore
{

    partial class BVModules_ContentBlocks_Last_Products_Viewed_adminview : BVModule
    {

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);
            this.LPVTitle.Text = SiteTerms.GetTerm(SiteTermIds.RecentlyViewedItems);
            LoadProductGrid();
        }

        private void LoadProductGrid()
        {
            List<Product> myProducts = PersonalizationServices.GetProductsViewed(MyPage.MTApp);

            List<Product> n = new List<Product>();
            int i = 0;

            foreach (Product p in myProducts)
            {
                if (i < WebAppSettings.LastProductsViewedMaxResults)
                {
                    n.Add(p);
                    i += 1;
                }
            }

        }

    }
}