using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MerchantTribeStore.Areas.ContentBlocks.RenderControllers
{
    public class NullRenderer: IContentBlockRenderController
    {
        public string Render(MerchantTribe.Commerce.MerchantTribeApplication app, dynamic viewBag, MerchantTribe.Commerce.Content.ContentBlock block)
        {
            return "<div style=\"background:#ffcccc;\">Missing Block: " + block.ControlName + "</div>";
            //return string.Empty;
        }
    }
}