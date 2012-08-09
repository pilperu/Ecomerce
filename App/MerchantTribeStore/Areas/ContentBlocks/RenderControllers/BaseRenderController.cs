using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MerchantTribeStore.Areas.ContentBlocks.RenderControllers
{
    public class BaseRenderController
    {
        protected string SafeHtml(string input)
        {
            return System.Web.HttpUtility.HtmlEncode(input);
        }
    }
}