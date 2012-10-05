using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MerchantTribeStore.code.TemplateEngine;
using System.Text;

namespace MerchantTribeStore.Controllers
{
    public class TemplateEngineController : Shared.BaseStoreController
    {
        [ChildActionOnly]
        public ActionResult FullHeader(dynamic currentViewBag)
        {

            string template = this.MTApp.ThemeManager().GetTemplatePartFromCurrentTheme("full-header.html");            
            Processor p = new Processor(this.MTApp, currentViewBag, template, new TagProvider());

            StringBuilder output = new StringBuilder();
            p.RenderForDisplay(output);
            return Content(output.ToString());                   
        }

        [ChildActionOnly]
        public ActionResult FullFooter(dynamic currentViewBag)
        {            
            string template = this.MTApp.ThemeManager().GetTemplatePartFromCurrentTheme("full-footer.html");
            Processor p = new Processor(this.MTApp, currentViewBag, template, new TagProvider());

            StringBuilder output = new StringBuilder();
            p.RenderForDisplay(output);
            return Content(output.ToString());       
        }
    }
}
