using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using MerchantTribeStore.Models;
using MerchantTribe.Commerce;
using MerchantTribe.Commerce.Utilities;
using MerchantTribe.Commerce.Catalog;

namespace MerchantTribeStore.code.TemplateEngine.TagHandlers
{
    /// <summary>
    /// Mini page shows:  "Page x of y, View All"
    /// Source is the current category only in this release
    /// </summary>
    public class MiniPager : ITagHandler
    {
        public string TagName
        {
            get { return "sys:minipager"; }
        }

        public void Process(StringBuilder output,
                            MerchantTribe.Commerce.MerchantTribeApplication app,
                            dynamic viewBag,
                            ITagProvider tagProvider,
                            ParsedTag tag,
                            string innerContents)
        {
            MiniPagerViewModel model = new MiniPagerViewModel();

            model.TotalPages = tag.GetSafeAttributeAsInteger("totalpages");
            if (model.TotalPages >= 1)
            {
                // manual load
                model.CurrentPage = tag.GetSafeAttributeAsInteger("currentpage");
                model.PagerUrlFormat = tag.GetSafeAttribute("urlformat");
                model.PagerUrlFormatFirst = tag.GetSafeAttribute("urlformatfirst");
                if (model.CurrentPage < 1) model.CurrentPage = GetPageFromRequest(app);
            }
            else
            {
                // find everything from current category
                model = FindModelForCurrentCategory(app, viewBag, tag);
            }
            
            Render(output, model);
        }

        public void Render(StringBuilder sb, MiniPagerViewModel model)
        {
            bool viewAllMode = false;

            if (model.CurrentPage == WebAppSettings.ViewAllPagesConstant)
            {
                model.CurrentPage = 1;
                model.TotalPages = 1;
                viewAllMode = true;
            }

            if (model.TotalPages <= 1 && viewAllMode == false) return;

            int pages = model.TotalPages;
            sb.Append("<div class=\"minipager\">");

            if (model.CurrentPage > 1)
            {
                if (model.CurrentPage == 2)
                {
                    sb.Append("<a href=\"" + String.Format(model.PagerUrlFormatFirst, 1) + "\">&laquo;</a>&nbsp;");
                }
                else
                {
                    sb.Append("<a href=\"" + String.Format(model.PagerUrlFormat, model.CurrentPage - 1) + "\">&laquo;</a>&nbsp;");
                }
            }
            sb.Append("Page " + model.CurrentPage + " of " + model.TotalPages);
            if (model.CurrentPage < model.TotalPages)
            {
                sb.Append("&nbsp;<a href=\"" + String.Format(model.PagerUrlFormat, model.CurrentPage + 1) + "\">&raquo;</a>");
            }
            if (!viewAllMode)
            {
                sb.Append("&nbsp;<a href=\"" + String.Format(model.PagerUrlFormat, "all") + "\">View All</a>");
            }
            else
            {
                sb.Append("<a href=\"" + String.Format(model.PagerUrlFormatFirst, 1) + "\">View By Pages</a>&nbsp;");
            }
                        
            sb.Append("</div>");

        }

        public string RenderToString(MiniPagerViewModel model)
        {
            StringBuilder sb = new StringBuilder();
            Render(sb, model);
            return sb.ToString();
        }

        private MiniPagerViewModel FindModelForCurrentCategory(MerchantTribe.Commerce.MerchantTribeApplication app, dynamic viewBag, ParsedTag tag)
        {
            MiniPagerViewModel model = new MiniPagerViewModel();

            // get the current category
            var cat = app.CurrentRequestContext.CurrentCategory;
            if (cat == null) return model;

                        
            model.CurrentPage = GetPageFromRequest(app);
            int currentPageForCount = model.CurrentPage;

            if (model.CurrentPage == WebAppSettings.ViewAllPagesConstant)
            {
                // View All Requested
                model.TotalPages = 1;
            }
            else
            {
                // View by Pages Requested, calculate how many we have
                int pageSize = tag.GetSafeAttributeAsInteger("pagesize");
                if (pageSize < 1) pageSize = 9;

                // Get Count of items in category, ignore pages because we just want the count
                int totalItems = 0;
                app.CatalogServices.FindProductForCategoryWithSort(cat.Bvin, MerchantTribe.Commerce.Catalog.CategorySortOrder.ManualOrder, false,
                                        1, 1, ref totalItems);
                model.TotalPages = MerchantTribe.Web.Paging.TotalPages(totalItems, pageSize);
            }

            model.PagerUrlFormat = UrlRewriter.BuildUrlForCategory(new CategorySnapshot(cat),
                                        app.CurrentRequestContext.RoutingContext,
                                        "{0}");
            model.PagerUrlFormatFirst = UrlRewriter.BuildUrlForCategory(new CategorySnapshot(cat),
                                    app.CurrentRequestContext.RoutingContext);
            return model;
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
                string val = app.CurrentRequestContext.RoutingContext.HttpContext.Request.QueryString["page"];
                if (val == "all") return WebAppSettings.ViewAllPagesConstant;

                int.TryParse(val, out result);
            }
            if (result < 1) result = 1;
            return result;
        }
    }
}