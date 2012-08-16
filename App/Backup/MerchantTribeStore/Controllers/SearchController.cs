using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MerchantTribe.Commerce;
using MerchantTribe.Commerce.Catalog;
using MerchantTribe.Commerce.Content;
using MerchantTribe.Commerce.Utilities;
using MerchantTribeStore.Models;
using MerchantTribeStore.Controllers.Shared;
using MerchantTribe.Web.Search;

namespace MerchantTribeStore.Controllers
{
    public class SearchController : BaseStoreController
    {
        //
        // GET: /Search/
        [ValidateInput(false)]
        public ActionResult Index(string q)
        {
            // Initial Setup
            ViewBag.Title = SiteTerms.GetTerm(SiteTermIds.Search);
            ViewBag.MetaTitle = ViewBag.Title + " | " + q;
            ViewBag.MetaDescription = ViewBag.MetaTitle;
            ViewBag.BodyClass = "store-search-page";
            ViewBag.GoButtonUrl = MTApp.ThemeManager().ButtonUrl("Go", Request.IsSecureConnection);
            ViewBag.q = q;

            // Pager Vars
            int pageNumber = GetPageNumber();
            int pageSize = 9;
            int totalItems = 0;

            // Do Search
            CategoryPageViewModel model = new CategoryPageViewModel();
            SearchManager manager = new SearchManager();
            List<SearchObject> objects = manager.DoSearch(MTApp.CurrentStore.Id, 
                                                        q, pageNumber, 
                                                        pageSize, ref totalItems);
            List<string> ids = new List<string>();
            foreach (SearchObject o in objects)
            {
                switch (o.ObjectType)
                {
                    case (int)SearchManagerObjectType.Product:
                        ids.Add(o.ObjectId);
                        break;
                }
            }
            List<Product> products = MTApp.CatalogServices.Products.FindMany(ids);

            // Save to Model
            model.Products = PrepProducts(products);            
            model.PagerData.PageSize = pageSize;
            model.PagerData.TotalItems = totalItems;
            model.PagerData.CurrentPage = pageNumber;
            model.PagerData.PagerUrlFormat = Url.Content("~/search?q=" + HttpUtility.UrlEncode(q) + "&p={0}");
            model.PagerData.PagerUrlFormatFirst = Url.Content("~/search?q=" + HttpUtility.UrlEncode(q));            

            return View(model);
        }

        private int GetPageNumber()
        {
            int result = 1;
            if (Request.QueryString["p"] != null)
            {
                int.TryParse(Request.QueryString["p"], out result);
            }
            if (result < 1) result = 1;
            return result;
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
                
                model.IsFirstItem = isFirstInRow;
                model.IsLastItem = isLastInRow;
                
                result.Add(model);
            }

            return result;
        }

    }
}
