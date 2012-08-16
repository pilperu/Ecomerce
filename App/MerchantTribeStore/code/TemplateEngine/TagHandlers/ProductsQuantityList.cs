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
    public class ProductsQuantityList : BaseProductsDisplayHandler
    {
        public override string TagName
        {
            get { return "sys:productsquantitylist"; }
        }

        protected override void Render(StringBuilder sb,
                            MerchantTribeApplication app,
                            dynamic viewBag,
                            ProductListViewModel model,
                            bool showPagers, int columns)
        {
            var profiler = MiniProfiler.Current;
            using (profiler.Step("Rendering Bulk Quantity List..."))
            {
                var preppedItems = base.PrepProducts(model.Items, columns, app);
                var pagerRenderer = new code.TemplateEngine.TagHandlers.Pager();

                if (showPagers == true)
                {
                    pagerRenderer.Render(sb, model.PagerData);
                }

                if (showPagers == true)
                {
                    pagerRenderer.Render(sb, model.PagerData);
                }


                sb.Append("<form action=\"" + app.CurrentRequestContext.UrlHelper.Content("~/cart/bulkadd") + "\" method=\"post\">");
                sb.Append("<table width=\"100%\">");
                foreach (var item in preppedItems)
                {
                    sb.Append("<tr>");
                    sb.Append("<td class=\"checkbox\">");
                    if (item.Item.HasOptions())
                    {
                        sb.Append("&nbsp;");
                    }
                    else
                    {
                        sb.Append("<input type=\"text\" name=\"bulkqty" + item.Item.Bvin + "\" value=\"0\" size=\"3\" />");                        
                    }
                    sb.Append("</td>");
                    sb.Append("<td class=\"records\">");
                    RenderSingleModel(sb, item, app);
                    sb.Append("</td>");
                    sb.Append("</tr>");
                }
                sb.Append("</table>");

                string buttonUrlAddToCart = app.ThemeManager().ButtonUrl("AddToCart", app.IsCurrentRequestSecure());
                sb.Append("<input type=\"image\" name=\"addtocart\" src=\"" + buttonUrlAddToCart + "\" alt=\"Add To Cart\" />");
                sb.Append("</form>");

            }
        }

        public void RenderSingleModel(StringBuilder sb, SingleProductViewModel model, MerchantTribeApplication app)
        {
            sb.Append("<div class=\"record\">");
            sb.Append("<div class=\"recordsku\">");
            sb.Append("<a href=\"" + model.ProductLink + "\">" + HttpUtility.HtmlEncode(model.Item.Sku) + "</a>");
            sb.Append("</div>");
            sb.Append("<div class=\"recordname\">");
            sb.Append("<a href=\"" + model.ProductLink + "\">" + HttpUtility.HtmlEncode(model.Item.ProductName) + "</a>");
            sb.Append("</div>");
            sb.Append("<div class=\"recordprice\">");
            sb.Append("<a href=\"" + model.ProductLink + "\">" + model.UserPrice.DisplayPrice(true) + "</a>");
            sb.Append("</div>");
            sb.Append("</div>");
        }

    }
}