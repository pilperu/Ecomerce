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
    public class ProductsDetailedList : BaseProductsDisplayHandler
    {
        public override string TagName
        {
            get { return "sys:productsdetailedlist"; }
        }

        protected override void Render(StringBuilder sb,
                            MerchantTribeApplication app,
                            dynamic viewBag,
                            ProductListViewModel model,
                            bool showPagers, int columns)
        {
            var profiler = MiniProfiler.Current;
            using (profiler.Step("Rendering Detailed List..."))
            {
                var preppedItems = base.PrepProducts(model.Items, columns, app);
                var pagerRenderer = new code.TemplateEngine.TagHandlers.Pager();

                string buttonUrlDetails = app.ThemeManager().ButtonUrl("View", app.IsCurrentRequestSecure());
                string buttonUrlAddToCart = app.ThemeManager().ButtonUrl("AddToCart", app.IsCurrentRequestSecure());

                if (showPagers == true)
                {
                    pagerRenderer.Render(sb, model.PagerData);
                }
                foreach (var item in preppedItems)
                {
                    RenderSingleModel(sb, item, app, buttonUrlDetails, buttonUrlAddToCart);
                }
                if (showPagers == true)
                {
                    pagerRenderer.Render(sb, model.PagerData);
                }
            }
        }

        private void RenderSingleModel(StringBuilder sb, SingleProductViewModel model, MerchantTribeApplication app, string buttonDetails, string buttonAdd)
        {
            sb.Append("<div class=\"record\">");

            // Image
            sb.Append("<div class=\"recordimage\">");
            sb.Append("<a href=\"" + model.ProductLink + "\">");
            sb.Append("<img src=\"" + model.ImageUrl + "\" border=\"0\" alt=\"" + HttpUtility.HtmlEncode(model.Item.ImageFileMediumAlternateText) + "\" /></a>");
            sb.Append("</div>");

            // SKU
            sb.Append("<div class=\"recordsku\">");
            sb.Append("<a href=\"" + model.ProductLink + "\">" + HttpUtility.HtmlEncode(model.Item.Sku) + "</a>");
            sb.Append("</div>");

            // Name
            sb.Append("<div class=\"recordname\">");
            sb.Append("<a href=\"" + model.ProductLink + "\">" + HttpUtility.HtmlEncode(model.Item.ProductName) + "</a>");
            sb.Append("</div>");

            // Description
            sb.Append("<div class=\"recordshortdescription\">");
            sb.Append(model.Item.LongDescription);
            sb.Append("</div>");

            // Price
            sb.Append("<div class=\"recordprice\">");
            sb.Append("<a href=\"" + model.ProductLink + "\">" + model.UserPrice.DisplayPrice(true) + "</a>");
            sb.Append("</div>");

            // Controls
            sb.Append("<div class=\"recordcontrols\">");            
            if (model.Item.HasOptions() == true)
            {
                sb.Append("<a href=\"" + model.ProductLink + "\"><img src=\"" + buttonDetails + "\" alt=\"View Product\" /></a>");
            }
            else
            {
                sb.Append("<a href=\"" + app.CurrentRequestContext.UrlHelper.Content("~/cart?quickaddsku=" + HttpUtility.UrlEncode(model.Item.Sku)) + "\"><img src=\"" + buttonAdd + "\" alt=\"View Product\" /></a>");
            }
            sb.Append("</div>");

            sb.Append("</div>");
                
        }

    }
}