using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MerchantTribe.Commerce.Content;
using MerchantTribe.Commerce.Utilities;
using MerchantTribeStore.Controllers.Shared;
using MvcMiniProfiler;

namespace MerchantTribeStore.Areas.ContentBlocks.Controllers
{
    public class HtmlController : BaseAppController
    {
        //
        // GET: /ContentBlocks/Html/

        public ActionResult Index(ContentBlock block)
        {
            var profiler = MvcMiniProfiler.MiniProfiler.Current;
            using (profiler.Step("Block:Html"))
            {

                string result = string.Empty;
                if (block != null)
                {
                    result = block.BaseSettings.GetSettingOrEmpty("HtmlData");
                }

                result = TagReplacer.ReplaceContentTags(result,
                                                        MTApp,
                                                        "",
                                                        Request.IsSecureConnection);
                return Content(result);
            }
        }

    }
}
