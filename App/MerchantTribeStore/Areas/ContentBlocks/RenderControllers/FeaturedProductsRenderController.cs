using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MerchantTribeStore.Areas.ContentBlocks.Models;
using MerchantTribe.Commerce.Utilities;
using System.Text;
using MerchantTribeStore.Models;
using MerchantTribe.Commerce.Catalog;
using MerchantTribe.Commerce;

namespace MerchantTribeStore.Areas.ContentBlocks.RenderControllers
{
    public class FeaturedProductsRenderController : BaseRenderController, IContentBlockRenderController
    {
        public string Render(MerchantTribe.Commerce.MerchantTribeApplication app, dynamic viewBag, MerchantTribe.Commerce.Content.ContentBlock block)
        {
            FeaturedProductsViewModel model = new FeaturedProductsViewModel();
            model.Items = PrepProducts(app.CatalogServices.Products.FindFeatured(1, 100), app);            
            return RenderModel(model, app);
        }

        private string RenderModel(FeaturedProductsViewModel model, MerchantTribeApplication app)
        {
            StringBuilder sb = new StringBuilder();

            var productRenderer = new code.TemplateEngine.TagHandlers.SingleProduct();

            sb.Append("<div class=\"featuredproducts\">");    
            foreach (var p in model.Items)
            {
                sb.Append(productRenderer.RenderModel(p, app));
            }
            sb.Append("<div class=\"clear\"></div>");
            sb.Append("</div>");
            
            return sb.ToString();
        }

        private List<SingleProductViewModel> PrepProducts(List<Product> products, MerchantTribeApplication app)
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

                if ((columnCount == 3))
                {
                    isLastInRow = true;
                    columnCount = 1;
                }
                else
                {
                    columnCount += 1;
                }

                //model.IsFirstItem = isFirstInRow;
                //model.IsLastItem = isLastInRow;

                result.Add(model);
            }

            return result;
        }
    }
}