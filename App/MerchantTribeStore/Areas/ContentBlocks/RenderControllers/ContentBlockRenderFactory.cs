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
            _Controllers.Add("Banner Ad", new BannerAdRenderController());
            _Controllers.Add("Html", new HtmlRenderController());
        }

        public static IContentBlockRenderController GetRenderer(ContentBlock block)
        {            
            if (Controllers.ContainsKey(block.ControlName))
            {
                return Controllers[block.ControlName];
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