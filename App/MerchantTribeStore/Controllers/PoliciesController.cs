using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MerchantTribe.Commerce.Content;
using MerchantTribeStore.Models;

namespace MerchantTribeStore.Controllers
{
    public class PoliciesController : Shared.BaseStoreController
    {
        
        // GET: /Policies/{action}
        public ActionResult Index(string policykind)
        {
            Policy p = new Policy();

            switch (policykind.Trim().ToLowerInvariant())
            {
                case "privacy":
                    p = LoadPolicy(PolicyType.Privacy);
                    break;
                case "terms":
                    p = LoadPolicy(PolicyType.TermsAndConditions);
                    break;
                case "returns":
                    p = LoadPolicy(PolicyType.Returns);
                    break;
                case "faq":
                    p = LoadPolicy(PolicyType.Faq);
                    break;
                default:
                    p = new Policy();
                    p.Title = "Content Not Found";
                    p.Blocks.Add(new PolicyBlock() { Description = "<p>The requested policy could not be found. Please close this window and try again.</p>" });
                    break;
            }          

            ViewBag.Title = p.Title;
            return View(p);
        }

        private Policy LoadPolicy(PolicyType type)
        {
            Policy p = MTApp.ContentServices.Policies.FindOrCreateByType(type);
            if (p == null) p = new Policy();
            return p;            
        }

        // Get: /Policies/Faq
        public ActionResult Faq()
        {
            // Breadcrumbs
            BreadCrumbViewModel crumbs = new BreadCrumbViewModel();
            crumbs.HomeName = SiteTerms.GetTerm(SiteTermIds.Home);
            crumbs.Items.Enqueue(new BreadCrumbItem()
            {
                Name = SiteTerms.GetTerm(SiteTermIds.CustomerService),
                Link = Url.Content("~/ContactUs")
            });
            crumbs.Items.Enqueue(new BreadCrumbItem() { Name = SiteTerms.GetTerm(SiteTermIds.FAQ) });            
            var breadRender = new code.TemplateEngine.TagHandlers.BreadCrumbs();
            ViewBag.BreadCrumbsFinal = breadRender.Render(MTApp, crumbs);

            // Titles
            ViewBag.Title = SiteTerms.GetTerm(SiteTermIds.FAQ);

            Policy p = MTApp.ContentServices.Policies.FindOrCreateByType(PolicyType.Faq);
            if (p == null) p = new Policy();
            return View(p);
        }

    }
}
