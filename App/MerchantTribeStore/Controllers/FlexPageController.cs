using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MerchantTribe.Commerce;
using MerchantTribe.Commerce.Accounts;
using MerchantTribe.Commerce.Catalog;
using MerchantTribe.Commerce.Content;
using System.Text;
using MerchantTribe.Commerce.Content.Parts;
using MerchantTribeStore.Models;
using MerchantTribeStore.code.TemplateEngine;

namespace MerchantTribeStore.Controllers
{
    public class FlexPageController : Shared.BaseStoreController
    {
        private void CheckFor301(string slug)
        {
            MerchantTribe.Commerce.Content.CustomUrl url = MTApp.ContentServices.CustomUrls.FindByRequestedUrl(slug);
            if (url != null)
            {
                if (url.Bvin != string.Empty)
                {
                    if (url.IsPermanentRedirect)
                    {
                        Response.RedirectPermanent(url.RedirectToUrl);
                    }
                    else
                    {
                        Response.Redirect(url.RedirectToUrl);
                    }
                }
            }
        }        

        [HandleError]
        public ActionResult Index(string slug)
        {            
            Category cat = MTApp.CatalogServices.Categories.FindBySlugForStore(slug, MTApp.CurrentRequestContext.CurrentStore.Id);
            if (cat == null) cat = new Category();
            
            if (cat.SourceType != CategorySourceType.FlexPage)
            {
                CheckFor301("/" + slug);                
                return Redirect("~/Error?type=category");
            }

            if (cat.Hidden)
            {
                return Redirect("~/Error?type=category");
            }

            FlexPageEditorViewModel editorModel = new FlexPageEditorViewModel();
            editorModel.CategoryId = cat.Bvin;
            editorModel.IsPreview = false;
            editorModel.CurrentPageUrl = Request.AppRelativeCurrentExecutionFilePath;
            editorModel.IsEditMode = false;

            ViewBag.Title = cat.MetaTitle;            
            ViewBag.MetaKeywords = cat.MetaKeywords;
            ViewBag.MetaDescription = cat.MetaDescription;
            ViewData["basecss"] = Url.Content("~/content/FlexBase.css");
            ViewData["slug"] = slug;
            MTApp.CurrentRequestContext.FlexPageId = cat.Bvin;
            MTApp.CurrentRequestContext.UrlHelper = this.Url;
            
            if (MTApp.IsEditMode)
            {
                editorModel.IsEditMode = true;
                string editCSS = "<link href=\"" + Url.Content("~/content/flexedit.css") + "\" rel=\"stylesheet\" type=\"text/css\" />";
                string editJS = "<script type=\"text/javascript\" src=\"" + Url.Content("~/content/FlexEdit.js") + "\"></script>";                
                editJS += "<script type=\"text/javascript\" src=\"" + Url.Content("~/scripts/Silverlight.js") + "\"></script>";                              

                // Inject CSS and JS into head section of page
                ViewData["AdditionalMetaTags"] += "\n" + editCSS + "\n" + editJS;
            }

            // Pre-Populate Empty Page
            if (cat.Versions.Count < 1)
            {
                cat.Versions.Add(new CategoryPageVersion() { AdminName = "First Version", PublishedStatus = PublishStatus.Published, Root = cat.GetSimpleSample() });
                MTApp.CatalogServices.Categories.Update(cat);
                cat = MTApp.CatalogServices.Categories.Find(cat.Bvin);
            }

            // Load Content Parts for Page        
            try
            {
                if (MTApp.IsEditMode)
                {
                    if (Request["preview"] == "1")
                    {
                        ViewData["ContentParts"] = cat.Versions[0].Root.RenderForDisplay(MTApp, cat);
                        editorModel.IsPreview = true;
                    }
                    else
                    {
                        ViewData["ContentParts"] = cat.Versions[0].Root.RenderForEdit(MTApp, cat);
                        editorModel.IsPreview = false;
                    }
                }
                else
                {
                    ViewData["ContentParts"] = cat.Versions[0].Root.RenderForDisplay(MTApp, cat);
                    editorModel.IsPreview = false;
                }
            }
            catch (Exception ex)
            {
                ViewData["ContentParts"] = ex.Message + ex.StackTrace;
            }

            // Save Editor Model to View Data
            ViewData["FlexEditorModel"] = editorModel;

            // Stuff the flex page content into the area field on the category
            // it will be rendered in main area tag 
            if (cat.Versions[0].Areas == null) cat.Versions[0].Areas = new AreaData();
            cat.Versions[0].Areas.SetAreaContent("Main", (string)ViewData["ContentParts"]);            

            MTApp.CurrentRequestContext.CurrentCategory = cat;            

            string template = this.MTApp.ThemeManager().GetTemplateFromCurrentTheme("default-no-menu.html");            
            Processor p = new Processor(this.MTApp, template, new TagProvider());
            List<ITemplateAction> model = p.RenderForDisplay();
            
            return View("~/views/shared/templateengine.cshtml", model);
        }        
    }
}
