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
using MerchantTribeStore.Models;

namespace MerchantTribeStore.Areas.ContentBlocks.RenderControllers
{
    public class LastViewedProductsRenderController : BaseRenderController, IContentBlockRenderController
    {
        public string Render(MerchantTribe.Commerce.MerchantTribeApplication app, dynamic viewBag, MerchantTribe.Commerce.Content.ContentBlock block)
        {
            ProductListViewModel model = new ProductListViewModel();
            model.Title = SiteTerms.GetTerm(SiteTermIds.RecentlyViewedItems);
            model.Items = LoadItems(app);
            
            return RenderModel(model, app);
        }
        public string RenderModel(ProductListViewModel model, MerchantTribeApplication app)
        {
            StringBuilder sb = new StringBuilder();            

            var productRenderer = new code.TemplateEngine.TagHandlers.SingleProduct();

            sb.Append("<div class=\"productgrid\">");    
            sb.Append("<div class=\"decoratedblock\">");
            sb.Append("<h4>" + HttpUtility.HtmlEncode(model.Title) + "</h4>");
            foreach (var p in model.Items)
            {
                sb.Append(productRenderer.Render(p, app));
            }
            sb.Append("<div class=\"clear\"></div>");
            sb.Append("</div>");
            sb.Append("</div>");
            
            return sb.ToString();
        }

        private List<Product> LoadItems(MerchantTribeApplication app)
        {
            List<Product> myProducts = MerchantTribe.Commerce.PersonalizationServices.GetProductsViewed(app);
            List<Product> limited = myProducts.Take(5).ToList();
            return limited;
        }

    }
}