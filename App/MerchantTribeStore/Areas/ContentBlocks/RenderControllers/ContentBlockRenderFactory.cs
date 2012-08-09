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
            _Controllers.Add("BannerAd", new BannerAdRenderController());
            _Controllers.Add("Html", new HtmlRenderController());
            _Controllers.Add("CategoryMenu", new CategoryMenuRenderController());
            _Controllers.Add("Top10Products", new Top10ProductsRenderController());
            _Controllers.Add("SideMenu", new SideMenuRenderController());
            _Controllers.Add("TopWeeklySellers", new TopWeeklySellersRenderController());
        }

        public static IContentBlockRenderController GetRenderer(ContentBlock block)
        {
            string noSpacesName = block.ControlName.Replace(" ", "");
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