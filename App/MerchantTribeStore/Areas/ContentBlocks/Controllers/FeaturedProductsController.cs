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
using MvcMiniProfiler;

namespace MerchantTribeStore.Areas.ContentBlocks.Controllers
{
    public class FeaturedProductsController : BaseAppController
    {
        //
        // GET: /ContentBlocks/FeaturedProducts/
        public ActionResult Index(ContentBlock block)
        {
            var profiler = MvcMiniProfiler.MiniProfiler.Current;
            using (profiler.Step("Block:FeaturedProducts"))
            {
                FeaturedProductsViewModel model = new FeaturedProductsViewModel();
                model.Items = PrepProducts(MTApp.CatalogServices.Products.FindFeatured(1, 100));
                return View(model);
            }
        }

        private List<SingleProductViewModel> PrepProducts(List<Product> products)
        {
            List<SingleProductViewModel> result = new List<SingleProductViewModel>();

            int columnCount = 1;

            foreach (Product p in products)
            {
                SingleProductViewModel model = new SingleProductViewModel(p, MTApp);

                bool isLastInRow = false;
                bool isFirstInRow = false;
                if ((columnCount == 1))
                {
                    isFirstInRow = true;
                }

                if ((columnCount == 3))
                {
                    isLastInRow = true;
                    columnCount = 1;
                }
                else
                {
                    columnCount += 1;
                }

                //model.IsFirstItem = isFirstInRow;
                //model.IsLastItem = isLastInRow;

                result.Add(model);
            }

            return result;
        }
    }
}
