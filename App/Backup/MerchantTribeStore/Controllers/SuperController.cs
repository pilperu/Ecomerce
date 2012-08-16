using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MerchantTribe.Commerce;
using MerchantTribe.Commerce.Catalog;
using MerchantTribe.Web.Search;

namespace MerchantTribeStore.Controllers
{
    public class SuperController : Shared.BaseSuperController
    {
        //
        // GET: /Super/
        public ActionResult Index()
        {

            ViewData["ActiveStores"] = MTApp.AccountServices.Stores.CountOfActive().ToString();
            ViewData["FreeStores"] = MTApp.AccountServices.Stores.CountOfFree().ToString();
            ViewData["PaidStores"] = MTApp.AccountServices.Stores.CountOfPaid().ToString();
            ViewData["Products"] = MTApp.CatalogServices.Products.FindAllForAllStoresCount().ToString();
            ViewData["SalesVolume"] = MTApp.OrderServices.Transactions.FindTotalTransactionsForever().ToString("C");

            return View();
        }

        // GET: /Super/Search
        public ActionResult Search()
        {
            TempData["results"] = string.Empty;
            return View();
        }
        // POST: /Super/Search
        [AcceptVerbs(HttpVerbs.Post)]
        [ActionName("Search")]
        public ActionResult Search(string keyword)
        {
            string r = "No Results Found";

            SearchManager searcher = new SearchManager();
            int total = 0;
            List<SearchObject> results = searcher.DoSearchForAllStores(keyword.Trim(), 1, 100, ref total);

            if (results != null)
            {
                r = "Found " + total.ToString() + " matches";
                r += "<ul>";

                foreach (SearchObject obj in results)
                {
                    r += "<li>";
                    r += obj.Title;
                    r += "</li>";
                }
                r += "</ul>";
            }

            TempData["results"] = r;
            return View();
        }

        // Get: /Super/RebuildSearch
        public ActionResult RebuildSearch()
        {
            TempData["message"] = string.Empty;
            return View();
        }

        // POST: /Super/RebuildSearch
        [AcceptVerbs(HttpVerbs.Post)]
        [ActionName("RebuildSearch")]
        public ActionResult RebuildSearchPost()
        {
            SearchManager manager = new SearchManager();
            manager.RebuildProductSearchIndex(MTApp);
            TempData["message"] = "Finished rebuild at " + DateTime.Now.ToLocalTime();                        
            return View();
        }
    }
}
