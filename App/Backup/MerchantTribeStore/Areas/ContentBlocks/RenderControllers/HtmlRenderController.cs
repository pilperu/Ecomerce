using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MerchantTribe.Commerce.Utilities;

namespace MerchantTribeStore.Areas.ContentBlocks.RenderControllers
{
    public class HtmlRenderController: IContentBlockRenderController
    {
        string IContentBlockRenderController.Render(MerchantTribe.Commerce.MerchantTribeApplication app, dynamic viewBag, MerchantTribe.Commerce.Content.ContentBlock block)
        {

            string result = string.Empty;
            if (block != null)
            {
                result = block.BaseSettings.GetSettingOrEmpty("HtmlData");
            }
            //result = TagReplacer.ReplaceContentTags(result,
            //                                        app,
            //                                        "",
            //                                        app.IsCurrentRequestSecure());
            return result;
        }
    }
}