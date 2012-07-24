using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MerchantTribe.Commerce.Catalog;
using MerchantTribe.Commerce.Content;
using MerchantTribe.Commerce.Utilities;
using MerchantTribeStore.Controllers.Shared;
using MerchantTribeStore.Models;
using MerchantTribeStore.Areas.ContentBlocks.Models;

namespace MerchantTribeStore.Areas.ContentBlocks.Controllers
{
    public class LastProductsViewedController : BaseAppController
    {
        //
        // GET: /ContentBlocks/LastProductsViewed/

        public ActionResult Index(ContentBlock block)
        {
            ProductListViewModel model = new ProductListViewModel();
            model.Title = SiteTerms.GetTerm(SiteTermIds.RecentlyViewedItems);
            model.Items = LoadItems();

            return View(model);
        }
        
        private List<Product> LoadItems()
        {
            List<Product> myProducts = MerchantTribe.Commerce.PersonalizationServices.GetProductsViewed(MTApp);            
            List<Product> limited = myProducts.Take(5).ToList();
            return limited;
        }

    }
}
