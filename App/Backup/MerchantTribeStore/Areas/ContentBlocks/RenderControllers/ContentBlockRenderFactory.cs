using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MerchantTribe.Commerce.Content;
using MerchantTribe.Commerce;

namespace MerchantTribeStore.Areas.ContentBlocks.RenderControllers
{
    public class ContentBlockRenderFactory
    {
        private static Dictionary<string, IContentBlockRenderController> _Controllers = null;

        private static Dictionary<string, IContentBlockRenderController> Controllers
        {
            get
            {
                if (_Controllers == null)
                {
                    Init();
                }
                return _Controllers;
            }
        }

        private static void Init()
        {
            _Controllers = new Dictionary<string, IContentBlockRenderController>();            
            _Controllers.Add("bannerad", new BannerAdRenderController());
            _Controllers.Add("html", new HtmlRenderController());
            _Controllers.Add("categorymenu", new CategoryMenuRenderController());
            _Controllers.Add("top10products", new Top10ProductsRenderController());
            _Controllers.Add("sidemenu", new SideMenuRenderController());
            _Controllers.Add("topweeklysellers", new TopWeeklySellersRenderController());
            _Controllers.Add("imagerotator", new ImageRotatorRenderController());
            _Controllers.Add("rssfeedviewer", new ImageRotatorRenderController());
            _Controllers.Add("featuredproducts", new FeaturedProductsRenderController());
            _Controllers.Add("lastproductsviewed", new LastViewedProductsRenderController());
            _Controllers.Add("lastviewedproducts", new LastViewedProductsRenderController());
            _Controllers.Add("productgrid", new ProductGridRenderController());
            _Controllers.Add("productrotator", new ProductRotatorRenderController());
            _Controllers.Add("categoryrotator", new CategoryRotatorRenderController());
        }

        public static IContentBlockRenderController GetRenderer(ContentBlock block)
        {
            string noSpacesName = block.ControlName.Replace(" ", "");
            noSpacesName = noSpacesName.ToLowerInvariant();
            if (Controllers.ContainsKey(noSpacesName))
            {
                return Controllers[noSpacesName];
            }
            return new NullRenderer();
        }

        public static string RenderBlock(ContentBlock block, MerchantTribeApplication app, dynamic viewBag)
        {
            var renderer = GetRenderer(block);
            return renderer.Render(app, viewBag, block);
        }
    }
}