using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MerchantTribe.Commerce;
using MerchantTribe.Commerce.Catalog;
using MerchantTribeStore.code.TemplateEngine;
using MvcMiniProfiler;

namespace MerchantTribeStore.Controllers
{
    public class HomeController : Shared.BaseStoreController
    {
        // Redirects to the correct home URL without page name
        public ActionResult ToIndex()
        {
            string homeUrl = Url.Content("~");
            return RedirectPermanent(homeUrl);
        }

        //
        // GET: /Home/
        public ActionResult Index()
        {
            var profiler = MvcMiniProfiler.MiniProfiler.Current;
            using (profiler.Step("Home Page Index Action"))
            {
                using (profiler.Step("Multi-Store Check"))
                {
                    // Redirect to Sign up if we're multi-store
                    // TODO - Change this to return the signup view instead
                    if (!WebAppSettings.IsIndividualMode)
                    {
                        if (MTApp.CurrentStore.StoreName == "www")
                        {
                            return Redirect("/signup/home");
                        }
                    }
                }

                using (profiler.Step("Wizard Check"))
                {
                    if (WebAppSettings.IsCommercialVersion || WebAppSettings.IsIndividualMode)
                    {
                        // Wizard Check
                        if (MTApp.CurrentStore.Settings.WizardComplete == false)
                        {
                            Response.Redirect(this.MTApp.StoreUrl(false, false) + "adminaccount/login?wizard=1");
                        }
                    }
                }

                using (profiler.Step("Session Setup"))
                {
                    SessionManager.CategoryLastId = string.Empty;
                    ViewBag.Title = MTApp.CurrentStore.Settings.FriendlyName;
                    ViewBag.BodyClass = "store-home-page";
                }
                using (profiler.Step("Load Template"))
                {
                    string template = this.MTApp.ThemeManager().GetTemplateFromCurrentTheme("home.html");
                    using (profiler.Step("Process Template"))
                    {
                        Processor p = new Processor(this.MTApp, template, new TagProvider());
                        using (profiler.Step("Render Template Actions"))
                        {
                            List<ITemplateAction> model = p.RenderForDisplay();
                            using (profiler.Step("return view"))
                            {
                                return View("~/views/shared/templateengine.cshtml", model);
                            }
                        }
                    }                    
                }
                
                
            }
        }
    }
}
