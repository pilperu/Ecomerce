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
    public class ProductGridRenderController : BaseRenderController, IContentBlockRenderController
    {
        public string Render(MerchantTribe.Commerce.MerchantTribeApplication app, dynamic viewBag, MerchantTribe.Commerce.Content.ContentBlock block)
        {
            List<SingleProductViewModel> model = LoadProductGrid(block, app);            
            return RenderModel(model, app);
        }
        public string RenderModel(List<SingleProductViewModel> model, MerchantTribeApplication app)
        {
            StringBuilder sb = new StringBuilder();

            var productRenderer = new code.TemplateEngine.TagHandlers.SingleProduct();

            sb.Append("<div class=\"productgrid\">");
            sb.Append("<div class=\"decoratedblock\">");
            sb.Append("<div class=\"blockcontent\">");
            foreach (var p in model)
            {
                sb.Append(productRenderer.RenderModel(p, app));
            }
            sb.Append("<div class=\"clear\"></div>");
            sb.Append("</div>");
            sb.Append("</div>");
            sb.Append("</div>");                      

            return sb.ToString();
        }

        private List<SingleProductViewModel> LoadProductGrid(ContentBlock b, MerchantTribeApplication app)
        {
            List<SingleProductViewModel> result = new List<SingleProductViewModel>();

            List<ContentBlockSettingListItem> myProducts = b.Lists.FindList("ProductGrid");
            if (myProducts != null)
            {
                int column = 1;

                if (b != null)
                {
                    int maxColumns = b.BaseSettings.GetIntegerSetting("GridColumns");
                    if (maxColumns < 1) maxColumns = 3;


                    // Pull all products in a single db call instead of individual calls
                    List<string> allProductBvins = myProducts.Select(y => y.Setting1).ToList();
                    List<Product> allProducts = app.CatalogServices.Products.FindMany(allProductBvins);

                    foreach (ContentBlockSettingListItem sett in myProducts)
                    {
                        string bvin = sett.Setting1;
                        Product p = allProducts.Where(y => y.Bvin == bvin).FirstOrDefault(); // app.CatalogServices.Products.Find(bvin);
                        if (p != null)
                        {
                            bool isLastInRow = false;
                            bool isFirstInRow = false;
                            if ((column == 1))
                            {
                                isFirstInRow = true;
                            }

                            if ((column == maxColumns))
                            {
                                column = 1;
                                isLastInRow = true;
                            }
                            else
                            {
                                column += 1;
                            }

                            SingleProductViewModel vm = new SingleProductViewModel(p, app);
                            vm.IsFirstItem = isFirstInRow;
                            vm.IsLastItem = isLastInRow;

                            result.Add(vm);
                        }
                    }
                }
            }

            return result;
        }

    }
}