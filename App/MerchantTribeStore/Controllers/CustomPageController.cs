using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MerchantTribeStore.Controllers.Shared;
using MerchantTribe.Commerce;
using MerchantTribe.Commerce.Catalog;
using MerchantTribe.Commerce.Content;
using MerchantTribe.Commerce.Utilities;
using MerchantTribeStore.code.TemplateEngine;
using System.Text;
using MerchantTribeStore.Models;

namespace MerchantTribeStore.Controllers
{
    public class CustomPageController : BaseStoreController
    {
        //
        // GET: /CustomPage/
        public ActionResult Index(string slug)
        {
            Category cat = MTApp.CatalogServices.Categories.FindBySlugForStore(slug, 
                                        MTApp.CurrentRequestContext.CurrentStore.Id);
            if (cat == null) cat = new Category();
            MTApp.CurrentRequestContext.CurrentCategory = cat;

            ViewBag.Title = cat.MetaTitle;
            if (String.IsNullOrEmpty(ViewBag.Title)) { ViewBag.Title = cat.Name; }
            ViewBag.MetaKeywords = cat.MetaKeywords;
            ViewBag.MetaDescription = cat.MetaDescription;

            // Record View for Analytics
            RecordCategoryView(cat.Bvin);
            
            // Get page.html Template
            ThemeManager tm = MTApp.ThemeManager();
            if (cat.TemplateName == string.Empty) { cat.TemplateName = "default.html"; }
            string template = tm.GetTemplateFromCurrentTheme(cat.TemplateName, "default.html"); // Try default in theme before system

            // Fill with data from category, making sure legacy description is used if no area data
            CategoryPageVersion version = cat.GetCurrentVersion();
            if (version.Id == 0)
            {
                // Create Initial Version
                version.PublishedStatus = PublishStatus.Draft;
                version.PageId = cat.Bvin;
                cat.Versions.Add(version);
                MTApp.CatalogServices.Categories.Update(cat);
                version = cat.GetCurrentVersion();
            }
            if (!version.Areas.HasArea("main"))
            {
                version.Areas.SetAreaContent("main", cat.PreTransformDescription);
            }

            ITagProvider tagProvider = new TagProvider();
            Processor proc = new Processor(this.MTApp, this.ViewBag, template, tagProvider);

            // Render Bread Crumbs
            var breadRender = new code.TemplateEngine.TagHandlers.BreadCrumbs();
            ViewBag.BreadCrumbsFinal = breadRender.RenderCategory(MTApp, new List<BreadCrumbItem>(), cat);

            var columnRenderer = new code.TemplateEngine.TagHandlers.ContentColumn();
            ViewBag.SideColumn = columnRenderer.RenderColumn("4", MTApp, ViewBag);

            var model = proc.RenderForDisplay();
            return View("~/views/shared/templateengine.cshtml", model);                                    
        }

        private void RecordCategoryView(string bvin)
        {
            MerchantTribe.Commerce.SessionManager.CategoryLastId = bvin;
        }
        
    }
}
