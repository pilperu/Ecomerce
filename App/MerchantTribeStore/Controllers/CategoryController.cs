using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MerchantTribeStore.Models;
using MerchantTribeStore.Controllers.Shared;
using MerchantTribe.Commerce.Catalog;
using MerchantTribe.Commerce.Utilities;
using System.Text;

namespace MerchantTribeStore.Controllers
{
    public class CategoryController : BaseStoreController
    {
        public ActionResult Index(string slug)
        {
            ViewBag.BodyClass = "store-category-page";

            Category cat = MTApp.CatalogServices.Categories.FindBySlugForStore(slug,
                                        MTApp.CurrentRequestContext.CurrentStore.Id);
            if (cat == null) cat = new Category();

            ViewBag.Title = cat.MetaTitle;
            if (String.IsNullOrEmpty(ViewBag.Title)) { ViewBag.Title = cat.Name; }
            ViewBag.MetaKeywords = cat.MetaKeywords;
            ViewBag.MetaDescription = cat.MetaDescription;
            ViewBag.DisplayHtml = TagReplacer.ReplaceContentTags(cat.Description,
                                                                 this.MTApp,
                                                                 "",
                                                                 Request.IsSecureConnection);

            ViewBag.AddToCartButton = this.MTApp.ThemeManager().ButtonUrl("AddToCart", Request.IsSecureConnection);
            ViewBag.DetailsButton = this.MTApp.ThemeManager().ButtonUrl("View", Request.IsSecureConnection);

            int pageNumber = GetPageNumber();
            int pageSize = 9;
            int totalItems = 0;

            CategoryPageViewModel model = new CategoryPageViewModel();
            List<Product> products = MTApp.CatalogServices.FindProductForCategoryWithSort(
                                            cat.Bvin, 
                                            CategorySortOrder.ManualOrder,
                                            false, 
                                            pageNumber, pageSize, ref totalItems);
            model.Products = PrepProducts(products);
            model.LocalCategory = cat;
            model.PagerData.PageSize = pageSize;
            model.PagerData.TotalItems = totalItems;
            model.PagerData.CurrentPage = pageNumber;
            model.PagerData.PagerUrlFormat = UrlRewriter.BuildUrlForCategory(new CategorySnapshot(cat), 
                                            MTApp.CurrentRequestContext.RoutingContext, 
                                            "{0}");
            model.PagerData.PagerUrlFormatFirst = UrlRewriter.BuildUrlForCategory(new CategorySnapshot(cat),
                                            MTApp.CurrentRequestContext.RoutingContext);
            model.SubCategories = PrepSubCategories(MTApp.CatalogServices.Categories.FindVisibleChildren(cat.Bvin));


            // Banner
            if (cat.BannerImageUrl.Trim().Length > 0)
            {
                ViewBag.ShowBanner = true;
                ViewBag.BannerUrl = MerchantTribe.Commerce.Storage.DiskStorage.CategoryBannerUrl(
                                        MTApp, 
                                        cat.Bvin, 
                                        cat.BannerImageUrl, 
                                        Request.IsSecureConnection);                
            }
            else
            {
                ViewBag.ShowBanner = false;
            }

            // Record Category View
            MerchantTribe.Commerce.SessionManager.CategoryLastId = cat.Bvin;            
            MTApp.CurrentRequestContext.CurrentCategory = cat;

            if (cat.TemplateName == "BV Grid") cat.TemplateName = "Grid"; // Safety Check from older versions
            string viewName = cat.TemplateName.Trim();
            return View(viewName, model);
        }
        private int GetPageNumber()
        {
            int result = 1;
            if (Request.QueryString["page"] != null)
            {
                int.TryParse(Request.QueryString["page"], out result);
            }
            if (result < 1) result = 1;
            return result;
        }
        private List<SingleCategoryViewModel> PrepSubCategories(List<CategorySnapshot> snaps)
        {
            List<SingleCategoryViewModel> result = new List<SingleCategoryViewModel>();

            int columnCount = 1;

            foreach (CategorySnapshot snap in snaps)
            {
                SingleCategoryViewModel model = new SingleCategoryViewModel();

                model.LinkUrl = UrlRewriter.BuildUrlForCategory(snap, 
                                                                MTApp.CurrentRequestContext.RoutingContext);
                model.IconUrl = MerchantTribe.Commerce.Storage.DiskStorage.CategoryIconUrl(
                                                                MTApp, 
                                                                snap.Bvin, 
                                                                snap.ImageUrl, 
                                                                Request.IsSecureConnection);
                model.AltText = snap.Name;
                model.Name = snap.Name;


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


        public ActionResult DrillDownIndex(string slug)
        {
            ViewBag.BodyClass = "store-categorydrilldown-page";

            Category cat = MTApp.CatalogServices.Categories.FindBySlugForStore(slug,
                                        MTApp.CurrentRequestContext.CurrentStore.Id);
            if (cat == null) cat = new Category();

            ViewBag.Title = cat.MetaTitle;
            if (String.IsNullOrEmpty(ViewBag.Title)) { ViewBag.Title = cat.Name; }
            ViewBag.MetaKeywords = cat.MetaKeywords;
            ViewBag.MetaDescription = cat.MetaDescription;
            ViewBag.DisplayHtml = TagReplacer.ReplaceContentTags(cat.Description,
                                                                 this.MTApp,
                                                                 "",
                                                                 Request.IsSecureConnection);

            string key = Request.QueryString["node"] ?? string.Empty;
            if (key == "-") key = string.Empty;
            ViewBag.Key = key;

            int pageNumber = GetPageNumber();
            int pageSize = 9;
            int totalItems = 0;




            CategoryPageViewModel model = new CategoryPageViewModel();


            CategoryFacetManager manager = CategoryFacetManager.InstantiateForDatabase(MTApp.CurrentRequestContext);
            List<CategoryFacet> facets = manager.FindByCategory(cat.Bvin);
            List<ProductProperty> properties = LoadProperties(facets);
            if (key == string.Empty)
            {
                key = CategoryFacetKeyHelper.BuildEmptyKey(facets.Count);
            }
            List<long> parsedKey = CategoryFacetKeyHelper.ParseKeyToList(key);

            List<Product> products = MTApp.CatalogServices.FindProductsMatchingKey(key,
                                                                            pageNumber,
                                                                            pageSize,
                                                                            ref totalItems);

            List<ProductFacetCount> productCounts = manager.FindCountsOfVisibleFacets(key, facets, properties);
            List<long> visibleIds = manager.FindVisibleFacetsIdsForKey(key, facets);

            model.Products = PrepProducts(products);
            model.LocalCategory = cat;
            model.PagerData.PageSize = pageSize;
            model.PagerData.TotalItems = totalItems;
            model.PagerData.CurrentPage = pageNumber;
            model.PagerData.PagerUrlFormat = UrlRewriter.BuildUrlForCategory(new CategorySnapshot(cat),
                                            MTApp.CurrentRequestContext.RoutingContext,
                                            "{0}");
            model.PagerData.PagerUrlFormatFirst = UrlRewriter.BuildUrlForCategory(new CategorySnapshot(cat),
                                            MTApp.CurrentRequestContext.RoutingContext);
            model.SubCategories = PrepSubCategories(MTApp.CatalogServices.Categories.FindVisibleChildren(cat.Bvin));



            StringBuilder sbNotSelected = new StringBuilder();

            List<BreadCrumbItem> extraCrumbs = new List<BreadCrumbItem>();

            for (int i = 0; i < facets.Count; i++)
            {
                if (visibleIds.Contains(facets[i].Id))
                {
                    // Find the property that matches the facet
                    var p = (from pr in properties
                             where pr.Id == facets[i].PropertyId
                             select pr).SingleOrDefault();
                    if (p != null)
                    {
                        if (parsedKey[i] < 1)
                        {
                            // not selected
                            this.RenderNonSelection(sbNotSelected, i, parsedKey[i], key, facets[i], p, productCounts, model);
                        }
                        else
                        {
                            // selected
                            this.RenderSelection(sbNotSelected, i, parsedKey[i], key, facets[i], p, facets, model, extraCrumbs);
                        }
                    }
                }
            }

            ViewBag.ExtraCrumbs = extraCrumbs;
            ViewBag.Filters = sbNotSelected.ToString();

            

            // Banner
            if (cat.BannerImageUrl.Trim().Length > 0)
            {
                ViewBag.ShowBanner = true;
                ViewBag.BannerUrl = MerchantTribe.Commerce.Storage.DiskStorage.CategoryBannerUrl(
                                        MTApp,
                                        cat.Bvin,
                                        cat.BannerImageUrl,
                                        Request.IsSecureConnection);
            }
            else
            {
                ViewBag.ShowBanner = false;
            }

            // Record Category View
            MerchantTribe.Commerce.SessionManager.CategoryLastId = cat.Bvin;

            return View("DrillDown", model);
        }
       
        private List<ProductProperty> LoadProperties(List<CategoryFacet> facets)
        {

            List<ProductProperty> result = new List<ProductProperty>();

            // Collect ids
            List<long> ids = new List<long>();
            foreach (CategoryFacet facet in facets)
            {
                ids.Add(facet.PropertyId);
            }

            // Get unsorted
            List<ProductProperty> unsorted = MTApp.CatalogServices.ProductProperties.FindMany(ids);

            // sort
            foreach (CategoryFacet facet in facets)
            {
                ProductProperty temp = unsorted.Where(y => y.Id == facet.PropertyId).FirstOrDefault();
                if (temp != null)
                {
                    result.Add(temp);
                }
            }

            return result;
        }

        private void RenderSelection(StringBuilder sb, 
                                     int index, 
                                     long selectedValue, 
                                     string key,
                                     CategoryFacet facet, 
                                     ProductProperty property, 
                                     List<CategoryFacet> allFacets,
                                     CategoryPageViewModel model, 
                                     List<BreadCrumbItem> extraCrumbs)
        {
            string selectedName = string.Empty;
            var sn = (from c in property.Choices
                      where c.Id == selectedValue
                      select c.ChoiceName).SingleOrDefault();
            if (sn != null)
            {
                selectedName = sn;
                extraCrumbs.Add(new BreadCrumbItem() { Name = sn });                                
                ViewBag.MetaDescription += " | " + sn;
                ViewBag.Title += " | " + sn;
            }
            sb.Append("<h6 class=\"filter\">" + property.DisplayName + "</h6> ");

            string baseUrl = model.LocalCategory.RewriteUrl + "?node=" + CategoryFacetKeyHelper.ClearSelfAndChildrenFromKey(key, allFacets, facet.Id);
            sb.Append("<ul class=\"filterselected\"><li>");
            sb.Append(" <a href=\"" + baseUrl + "\">");
            sb.Append(selectedName);
            sb.Append(" [clear]</a></li></ul>");
        }


        private void RenderNonSelection(StringBuilder sb, int index, long selectedValue, string key,
                                    CategoryFacet facet, ProductProperty property, List<ProductFacetCount> counts,
                                    CategoryPageViewModel model)
        {

            sb.Append("<h6 class=\"filter\">" + property.DisplayName + "</h6>");
            sb.Append("<ul class=\"filterselections\">");
            foreach (ProductPropertyChoice c in property.Choices)
            {
                string newKey = CategoryFacetKeyHelper.ReplaceKeyValue(key, index, c.Id);
                string baseUrl = model.LocalCategory.RewriteUrl + "?node=" + newKey;
                string sqlKey = CategoryFacetKeyHelper.ParseKeyToSqlList(newKey);
                ProductFacetCount fc = new ProductFacetCount();
                foreach (ProductFacetCount f in counts)
                {
                    if (f.Key == sqlKey)
                    {
                        fc = f;
                        break;
                    }
                }

                if (fc.ProductCount > 0)
                {
                    sb.Append("<li><a href=\"" + baseUrl + "\">");
                    sb.Append(c.ChoiceName);
                    sb.Append(" (" + fc.ProductCount.ToString() + ")");
                    sb.Append("</a></li>");
                }
            }
            sb.Append("</ul>");
        }
        
    }
}
