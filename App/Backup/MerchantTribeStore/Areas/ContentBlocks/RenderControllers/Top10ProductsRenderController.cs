using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using MerchantTribe.Commerce;
using MerchantTribe.Commerce.Catalog;
using MerchantTribe.Commerce.Content;
using MerchantTribeStore.Areas.ContentBlocks.Models;
using MerchantTribe.Commerce.Utilities;

namespace MerchantTribeStore.Areas.ContentBlocks.RenderControllers
{
    public class Top10ProductsRenderController: BaseRenderController, IContentBlockRenderController
    {
        public string Render(MerchantTribe.Commerce.MerchantTribeApplication app, dynamic viewBag, MerchantTribe.Commerce.Content.ContentBlock block)
        {
            SideMenuViewModel model = new SideMenuViewModel();

            System.DateTime s = new System.DateTime(1900, 1, 1);
            System.DateTime e = new System.DateTime(3000, 12, 31);
            List<Product> products;
            products = app.ReportingTopSellersByDate(s, e, 10);
            foreach (Product p in products)
            {
                SideMenuItem item = new SideMenuItem();
                item.Title = p.ProductName;
                item.Name = p.ProductName;
                item.Url = UrlRewriter.BuildUrlForProduct(p, app.CurrentRequestContext.RoutingContext, string.Empty);
                item.Name += " - " + p.SitePrice.ToString("C");
                model.Items.Add(item);
            }

            model.Title = "Top Sellers";

            return RenderModel(model);      
        }
        public string RenderModel(SideMenuViewModel model)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("<div class=\"sidemenu topsellers\">");
            sb.Append("<div class=\"decoratedblock\">");
            sb.Append("<h3>" + HttpUtility.HtmlEncode(model.Title) + "</h3>");  
            sb.Append("<ol>");
            foreach (var item in model.Items)
            {
            sb.Append("<li><a href=\"" + item.Url + "\" title=\"" + HttpUtility.HtmlEncode(item.Title) + "\">" + HttpUtility.HtmlEncode(item.Name) + "</a></li>");
            }
            sb.Append("</ol>");
            sb.Append("</div>");
            sb.Append("</div>");  

            return sb.ToString();
        }
    }
}