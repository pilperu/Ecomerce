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
    public class ProductsList : BaseProductsDisplayHandler
    {
        public override string TagName
        {
            get { return "sys:productslist"; }
        }
  
        protected override void Render(StringBuilder sb,
                            MerchantTribeApplication app,
                            dynamic viewBag,
                            ProductListViewModel model,
                            bool showPagers, int columns)
        {
            var profiler = MiniProfiler.Current;
            using (profiler.Step("Rendering Simple List..."))
            {
                var preppedItems = base.PrepProducts(model.Items, columns, app);
                var pagerRenderer = new code.TemplateEngine.TagHandlers.Pager();
                
                if (showPagers == true)
                {
                    pagerRenderer.Render(sb, model.PagerData);
                }
                foreach (var item in preppedItems)
                {
                    RenderSingleModel(sb, item, app);                    
                }
                if (showPagers == true)
                {
                    pagerRenderer.Render(sb, model.PagerData);
                }
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