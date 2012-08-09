using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MerchantTribeStore.Models;
using System.Text;
using MerchantTribe.Commerce.Content;

namespace MerchantTribeStore.Controllers
{
    public class ContentColumnController : Shared.BaseAppController
    {
        [ChildActionOnly]
        public ActionResult Index(string id)
        {
            // TODO: This should be replaced by a direct render call like the tag handler version. 
            // eventually, there should not be controllers or methods for content blocks
            if (string.IsNullOrEmpty(id)) return Content("");

            var renderer = new code.TemplateEngine.TagHandlers.ContentColumn();
            string output = renderer.RenderColumn(id, MTApp, this.ViewBag);

            return Content(output);                        
        }
    }
}
