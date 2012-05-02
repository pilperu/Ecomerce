using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MerchantTribeStore.code.TemplateEngine;

namespace MerchantTribeStore.Controllers
{
    public class TemplateEngineController : Shared.BaseStoreController
    {
        [ChildActionOnly]
        public ActionResult FullHeader(string AnalyticsTop, string AnalyticsBottom, string AdditionalMetaTags, string metaTitle = "")
        {
            if (String.IsNullOrEmpty(ViewBag.Title))
            {
                ViewBag.Title = metaTitle;
            }

            ViewData["analyticstop"] = AnalyticsTop;
            ViewData["analyticsbottom"] = AnalyticsBottom;
            ViewData["AdditionalMetaTags"] += AdditionalMetaTags;

            string template = this.MTApp.ThemeManager().GetTemplatePartFromCurrentTheme("full-header.html");            
            Processor p = new Processor(this.MTApp, template, new TagProvider());            
            List<ITemplateAction> model = p.RenderForDisplay();
            return View("~/views/shared/templateengine.cshtml", model);
        }

        [ChildActionOnly]
        public ActionResult FullFooter(string AnalyticsTop, string AnalyticsBottom, string AdditionalMetaTags)
        {            
            ViewData["analyticstop"] = AnalyticsTop;
            ViewData["analyticsbottom"] = AnalyticsBottom;
            ViewData["AdditionalMetaTags"] += AdditionalMetaTags;

            string template = this.MTApp.ThemeManager().GetTemplatePartFromCurrentTheme("full-footer.html");
            Processor p = new Processor(this.MTApp, template, new TagProvider());
            List<ITemplateAction> model = p.RenderForDisplay();
            return View("~/views/shared/templateengine.cshtml", model);
        }
    }
}
