using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MerchantTribeStore.code.TemplateEngine;
using MvcMiniProfiler;

namespace MerchantTribeStore.Controllers
{
    public class TemplateTestController : Shared.BaseStoreController
    {
        //
        // GET: /TemplateTest/

        public ActionResult Index()
        {
            ViewBag.PageTitle = "My Test Page";

            var profiler = MvcMiniProfiler.MiniProfiler.Current;
            string template;
            Processor p;
            using (profiler.Step("Load Template"))
            {
                template = this.MTApp.ThemeManager().GetSystemTemplate("home.html");
                
            }
            using (profiler.Step("Create Processor"))
            {
                p = new Processor(this.MTApp, template, new TagProvider());
            }
            List<ITemplateAction> actions;
            using (profiler.Step("Get Actions"))
            {
                actions = p.RenderForDisplay();
            }
            return View("~/views/shared/TemplateEngine.cshtml", actions);
        }

    }
}
