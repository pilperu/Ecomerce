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
    public class Top10ProductsController : BaseAppController
    {
        //
        // GET: /ContentBlocks/Top10Products/

        public ActionResult Index(ContentBlock b)
        {
            var profiler = MvcMiniProfiler.MiniProfiler.Current;
            using (profiler.Step("Block:Top10Products"))
            {
                SideMenuViewModel model = new SideMenuViewModel();

                System.DateTime s = new System.DateTime(1900, 1, 1);
                System.DateTime e = new System.DateTime(3000, 12, 31);
                List<Product> products;
                using (profiler.Step("Load top 10 products"))
                {
                    products = MTApp.ReportingTopSellersByDate(s, e, 10);
                }
                using (profiler.Step("Prep Products"))
                {
                    foreach (Product p in products)
                    {
                        SideMenuItem item = new SideMenuItem();
                        item.Title = p.ProductName;
                        item.Name = p.ProductName;
                        item.Url = UrlRewriter.BuildUrlForProduct(p, MTApp.CurrentRequestContext.RoutingContext, string.Empty);
                        item.Name += " - " + p.SitePrice.ToString("C");
                        model.Items.Add(item);
                    }
                }

                model.Title = "Top Sellers";
                return View(model);
            }
        }
    }
}
