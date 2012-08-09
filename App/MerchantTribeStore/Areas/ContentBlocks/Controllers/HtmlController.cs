using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MerchantTribe.Commerce.Content;
using MerchantTribe.Commerce.Utilities;
using MerchantTribeStore.Controllers.Shared;
using MerchantTribeStore.Areas.ContentBlocks.RenderControllers;

namespace MerchantTribeStore.Areas.ContentBlocks.Controllers
{
    public class HtmlController : BaseAppController
    {
        //
        // GET: /ContentBlocks/Html/
        public ActionResult Index(ContentBlock block)
        {
            return Content(ContentBlockRenderFactory.RenderBlock(block, this.MTApp, this.ViewBag));            
        }

    }
}
