using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MerchantTribe.Commerce.Catalog;
using MerchantTribe.Commerce;
using MerchantTribeStore.Models;
using System.Text;

namespace MerchantTribeStore.code.TemplateEngine.TagHandlers
{
    public class SingleProduct : ITagHandler
    {

        public string TagName
        {
            get { return "sys:product"; }
        }

        public void Process(StringBuilder output,
                            MerchantTribe.Commerce.MerchantTribeApplication app,
                            dynamic viewBag,
                            ITagProvider tagProvider,
                            ParsedTag tag,
                            string innerContents)
        {
            string bvin = tag.GetSafeAttribute("bvin");
            string sku = tag.GetSafeAttribute("sku");

            var product = app.CatalogServices.Products.Find(bvin);
            if (product == null)
            {
                product = app.CatalogServices.Products.FindBySku(sku);
            }

            Render(output, product, app);            
        }

        public void Render(StringBuilder sb, MerchantTribe.Commerce.Catalog.Product p, MerchantTribeApplication app)
        {
            if (p == null) return;
            if (p.Bvin == string.Empty) return;
            var model = new SingleProductViewModel(p, app);
            RenderModel(sb, model, app);
        }
        public string RenderToString(MerchantTribe.Commerce.Catalog.Product p, MerchantTribeApplication app)
        {
            StringBuilder sb = new StringBuilder();
            Render(sb, p, app);
            return sb.ToString();            
        }
        public void RenderModel(StringBuilder sb, SingleProductViewModel model, MerchantTribeApplication app)
        {
            if (model.IsLastItem == true)
            {
                sb.Append("<div class=\"record lastrecord\">");
            }
            else if (model.IsFirstItem == true)
            {
                sb.Append("<div class=\"record firstrecord\">");
            }
            else
            {
                sb.Append("<div class=\"record\">");
            }
            sb.Append("<div class=\"recordimage\">");
            sb.Append("<a href=\"" + model.ProductLink + "\">");
            sb.Append("<img src=\"" + model.ImageUrl + "\" alt=\"" + HttpUtility.HtmlEncode(model.Item.ImageFileSmallAlternateText) + "\" /></a>");
            sb.Append("</div>");

            sb.Append("<div class=\"recordname\">");
            sb.Append("<a href=\"" + model.ProductLink + "\">" + HttpUtility.HtmlEncode(model.Item.ProductName) + "</a>");
            sb.Append("</div>");
            sb.Append("<div class=\"recordsku\">");
            sb.Append("<a href=\"" + model.ProductLink + "\">" + HttpUtility.HtmlEncode(model.Item.Sku) + "</a>");
            sb.Append("</div>");
            sb.Append("<div class=\"recordprice\">");
            sb.Append("<a href=\"" + model.ProductLink + "\">" + model.UserPrice.DisplayPrice(true) + "</a>");
            sb.Append("</div>");
            sb.Append(model.SwatchDisplay);
            sb.Append("</div>");
        }
        public string RenderModelToString(SingleProductViewModel model, MerchantTribeApplication app)
        {
            StringBuilder sb = new StringBuilder();
            RenderModel(sb, model, app);            
            return sb.ToString();
        }


    }
}