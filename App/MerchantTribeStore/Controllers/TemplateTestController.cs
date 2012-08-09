using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MerchantTribeStore.code.TemplateEngine;

namespace MerchantTribeStore.Controllers
{
    public class TemplateTestController : Shared.BaseStoreController
    {
        //
        // GET: /TemplateTest/

        public ActionResult Index()
        {
            ViewBag.PageTitle = "My Test Page";

            string template;
            Processor p;
            template = this.MTApp.ThemeManager().GetSystemTemplate("home.html");
            p = new Processor(this.MTApp, this.ViewBag, template, new TagProvider());
            List<ITemplateAction> actions;
            actions = p.RenderForDisplay();
            return View("~/views/shared/TemplateEngine.cshtml", actions);
        }

    }
}
