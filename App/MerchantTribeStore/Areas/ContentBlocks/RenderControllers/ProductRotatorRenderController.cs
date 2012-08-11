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
    public class ProductRotatorRenderController : BaseRenderController, IContentBlockRenderController
    {
        public string Render(MerchantTribe.Commerce.MerchantTribeApplication app, dynamic viewBag, MerchantTribe.Commerce.Content.ContentBlock block)
        {
            Product p = null;
            List<ContentBlockSettingListItem> myProducts = block.Lists.FindList("Products");
            if (myProducts != null)
            {
                if (myProducts.Count > 0)
                {
                    int displayIndex = GetProductIndex(myProducts.Count - 1);

                    ContentBlockSettingListItem data = myProducts[displayIndex];
                    string bvin = data.Setting1;
                    p = app.CatalogServices.Products.Find(bvin);                    
                }
            }

            if (p != null)
            {
                SingleProductViewModel model = new SingleProductViewModel(p, app);
                return RenderModel(model, app);
            }
            return string.Empty;            
        }

        private int GetProductIndex(int maxIndex)
        {
            int result = 0;

            result = MerchantTribe.Web.RandomNumbers.RandomInteger(maxIndex, 0);

            return result;
        }

        public string RenderModel(SingleProductViewModel model, MerchantTribeApplication app)
        {
            StringBuilder sb = new StringBuilder();

            var productRenderer = new code.TemplateEngine.TagHandlers.SingleProduct();

            sb.Append("<div class=\"productrotator\"><div class=\"decoratedblock\"><div class=\"blockcontent\">");            
            sb.Append(productRenderer.RenderModel(model, app));            
            sb.Append("<div class=\"clear\"></div></div></div></div>");

            return sb.ToString();
        }      
    }
}