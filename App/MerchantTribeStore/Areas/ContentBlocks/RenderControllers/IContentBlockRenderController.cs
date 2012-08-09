using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MerchantTribe.Commerce;
using MerchantTribe.Commerce.Content;

namespace MerchantTribeStore.Areas.ContentBlocks.RenderControllers
{
    public interface IContentBlockRenderController
    {
        string Render(MerchantTribeApplication app, dynamic viewBag, ContentBlock block);
    }
}