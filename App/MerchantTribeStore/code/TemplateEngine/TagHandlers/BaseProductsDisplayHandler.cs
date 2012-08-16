using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Dynamic;
using System.Text;
using MerchantTribe.Commerce;
using MerchantTribe.Commerce.Catalog;
using MerchantTribe.Commerce.Utilities;
using MerchantTribeStore.Models;
using StackExchange.Profiling;

namespace MerchantTribeStore.code.TemplateEngine.TagHandlers
{
    public abstract class BaseProductsDisplayHandler: ITagHandler
    {
        public abstract string TagName { get; }

        public void Process(StringBuilder output,
                            MerchantTribe.Commerce.MerchantTribeApplication app,
                            dynamic viewBag,
                            ITagProvider tagProvider,
                            ParsedTag tag,
                            string innerContents)
        {
            var profiler = MiniProfiler.Current;

            bool showPagers = false;
            int columns = 3;
            var model = new ProductListViewModel();

            using (profiler.Step("Attribute Gathering"))
            {
                model.PagerData.PageSize = tag.GetSafeAttributeAsInteger("pagesize");
                if (model.PagerData.PageSize < 1) model.PagerData.PageSize = 9;

                columns = tag.GetSafeAttributeAsInteger("columns");
                if (columns < 1) columns = 3;

                model.PagerData.CurrentPage = tag.GetSafeAttributeAsInteger("page");
                if (model.PagerData.CurrentPage < 1) model.PagerData.CurrentPage = GetPageFromRequest(app);

                model.PagerData.TotalItems = 0;

                showPagers = tag.GetSafeAttributeAsBoolean("showpager");
            }

            string source = tag.GetSafeAttribute("source");
            switch (source.Trim().ToLowerInvariant())
            {
                case "featured":
                    model.Items = app.CatalogServices.Products.FindFeatured(1, model.PagerData.PageSize);
                    showPagers = false;
                    break;
                case "manual":
                    string manualProducts = tag.GetSafeAttribute("products");
                    List<string> bvins = manualProducts.Split(',').ToList();
                    model.Items = app.CatalogServices.Products.FindMany(bvins);

                    string manualSkus = tag.GetSafeAttribute("skus");
                    List<string> skus = manualSkus.Split(',').ToList();
                    model.Items.AddRange(app.CatalogServices.Products.FindManySkus(skus));

                    showPagers = false;
                    break;
                default:
                    using (profiler.Step("Pull Products for Category"))
                    {
                        var cat = app.CurrentRequestContext.CurrentCategory;
                        string categoryId = tag.GetSafeAttribute("categoryid");
                        using (profiler.Step("Checking for non-current category on grid"))
                        {
                            if (categoryId.Trim().Length < 1 || categoryId.Trim().ToLowerInvariant() == "current")
                            {
                                if (app.CurrentRequestContext.CurrentCategory != null)
                                {
                                    categoryId = app.CurrentRequestContext.CurrentCategory.Bvin;
                                    cat = app.CatalogServices.Categories.Find(categoryId);
                                }
                            }
                        }

                        using (profiler.Step("Build Data for Render"))
                        {
                            int totalItems = 0;
                            using (profiler.Step("Get the Products"))
                            {
                                model.Items = app.CatalogServices.FindProductForCategoryWithSort(
                                                       categoryId,
                                                       CategorySortOrder.ManualOrder,
                                                       false,
                                                       model.PagerData.CurrentPage, model.PagerData.PageSize, ref totalItems);
                                model.PagerData.TotalItems = totalItems;
                            }
                            using (profiler.Step("Build the Pager Urls"))
                            {
                                model.PagerData.PagerUrlFormat = UrlRewriter.BuildUrlForCategory(new CategorySnapshot(cat),
                                                        app.CurrentRequestContext.RoutingContext,
                                                        "{0}");
                                model.PagerData.PagerUrlFormatFirst = UrlRewriter.BuildUrlForCategory(new CategorySnapshot(cat),
                                                        app.CurrentRequestContext.RoutingContext);
                            }
                        }
                    }
                    break;
            }

            Render(output, app, viewBag, model, showPagers, columns);
        }

        protected int GetPageFromRequest(MerchantTribeApplication app)
        {
            int result = 1;
            if (app.CurrentRequestContext == null) return result;
            if (app.CurrentRequestContext.RoutingContext == null) return result;
            if (app.CurrentRequestContext.RoutingContext.HttpContext == null) return result;
            if (app.CurrentRequestContext.RoutingContext.HttpContext.Request == null) return result;
            if (app.CurrentRequestContext.RoutingContext.HttpContext.Request.QueryString["page"] != null)
            {
                int.TryParse(app.CurrentRequestContext.RoutingContext.HttpContext.Request.QueryString["page"], out result);
            }
            if (result < 1) result = 1;
            return result;
        }

        protected abstract void Render(StringBuilder sb,
                            MerchantTribeApplication app,
                            dynamic viewBag,
                            ProductListViewModel model,
                            bool showPagers, int columns);
        //{
        //    var profiler = MiniProfiler.Current;
        //    using (profiler.Step("Rendering Grid..."))
        //    {
        //        var preppedItems = PrepProducts(model.Items, columns, app);
        //        var pagerRenderer = new code.TemplateEngine.TagHandlers.Pager();
        //        var productRenderer = new code.TemplateEngine.TagHandlers.SingleProduct();

        //        if (showPagers == true)
        //        {
        //            pagerRenderer.Render(sb, model.PagerData);
        //        }
        //        foreach (var item in preppedItems)
        //        {
        //            productRenderer.RenderModel(sb, item, app);
        //        }
        //        if (showPagers == true)
        //        {
        //            pagerRenderer.Render(sb, model.PagerData);
        //        }
        //    }
        //}

        protected List<SingleProductViewModel> PrepProducts(List<Product> products, int columns, MerchantTribeApplication app)
        {
            var profiler = MiniProfiler.Current;
            using (profiler.Step("Prepping Products"))
            {
                List<SingleProductViewModel> result = new List<SingleProductViewModel>();

                int columnCount = 1;

                foreach (Product p in products)
                {
                    SingleProductViewModel model = new SingleProductViewModel(p, app);

                    bool isLastInRow = false;
                    bool isFirstInRow = false;
                    if ((columnCount == 1))
                    {
                        isFirstInRow = true;
                    }

                    if ((columnCount == columns))
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
}