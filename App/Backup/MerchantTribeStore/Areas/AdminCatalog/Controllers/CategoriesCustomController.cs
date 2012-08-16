using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MerchantTribe.Commerce.Catalog;
using MerchantTribe.Commerce.Content;
using MerchantTribe.Commerce;
using MerchantTribeStore.Filters;
using MerchantTribe.Commerce.Membership;
using MerchantTribe.Commerce.Utilities;
using System.Text;

namespace MerchantTribeStore.Areas.AdminCatalog.Controllers
{
    [HandleError]
    [ValidateInput(false)]
    public class CategoriesCustomController : MerchantTribeStore.Controllers.Shared.BaseAdminController
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // Set Tab Type before other stuff happens
            base.SelectedTab = AdminTabType.Catalog;
            base.OnActionExecuting(filterContext);
        }

        private Category EditorSetup(string id)
        {
            Category result = MTApp.CatalogServices.Categories.Find(id);
            if (result == null) result = new Category();

            // Fill with data from category, making sure legacy description is used if no area data
            CategoryPageVersion version = result.GetCurrentVersion();
            if (version.Id == 0)
            {
                // Create Initial Version
                version.PublishedStatus = PublishStatus.Draft;
                version.PageId = result.Bvin;
                result.Versions.Add(version);
                MTApp.CatalogServices.Categories.Update(result);
                version = result.GetCurrentVersion();
            }            
            if (!version.Areas.HasArea("main"))
            {
                version.Areas.SetAreaContent("main", result.PreTransformDescription);
                MTApp.CatalogServices.Categories.Update(result);
            }
            string areaMain = version.Areas.GetAreaContent("main");
            if (areaMain == string.Empty && result.PreTransformDescription != string.Empty)
            {
                result.GetCurrentVersion().Areas.SetAreaContent("main", result.PreTransformDescription);
            }


            ViewData["ViewInStoreUrl"] = UrlRewriter.BuildUrlForCategory(new CategorySnapshot(result), MTApp.CurrentRequestContext.RoutingContext);
            LoadTemplates();
            ViewData["OtherUrls"] = LoadUrls(result);
            return result;
        }

                
        [HttpGet]
        public ActionResult Edit(string id)
        {
            Category c = EditorSetup(id);

            BreadCrumbs(c);
            return View(c);
        }

        [HttpPost]
        [ActionName("Edit")]
        public ActionResult EditPost(string id, Category posted)
        {
            Category c = EditorSetup(id);

            bool success = this.Save(c, posted, Request.Form);
            if (!success)
            {
                this.FlashFailure("Error during update. Please check event log.");
                
            }
            else
            {
                this.FlashSuccess("Category Updated Successfully at " + DateTime.Now.ToLongTimeString());
                c = EditorSetup(id);

                if (Request.Form["SaveButton"] != null || Request.Form["SaveButton.x"] != null)
                {
                    string destination = Url.Content("~/bvadmin/catalog/categories.aspx");
                    return Redirect(destination);
                }                
            }
            
            BreadCrumbs(c);
            return View(c);
        }

        private void BreadCrumbs(Category cat)
        {
            var renderer = new code.TemplateEngine.TagHandlers.BreadCrumbs();
            ViewBag.BreadCrumbsFinal = renderer.RenderCategory(this.MTApp, new List<Models.BreadCrumbItem>(), cat);
        }
        private bool Save(Category current, Category posted, System.Collections.Specialized.NameValueCollection form)
        {

            bool result = false;

            if (current != null)
            {
                current.Name = posted.Name.Trim();

                /* Areas */
                string mainArea = string.Empty;
                if (form["area-main"] != null)
                {
                    mainArea = form["area-main"];
                }
                current.Description = mainArea;
                current.PreTransformDescription = mainArea;
                current.GetCurrentVersion().Areas.SetAreaContent("main", mainArea);                
                
                /* Other Settings */
                current.MetaDescription = form["metadescription"] ?? current.MetaDescription;
                current.MetaTitle = form["metatitle"] ?? current.MetaTitle;
                current.MetaKeywords = form["metakeywords"] ?? current.MetaKeywords;
                current.ShowInTopMenu = false; // This could be changed to support show in top menu functionality
                current.SourceType = CategorySourceType.CustomPage;

                // hidden checkbox
                string isChecked = form["hidden"] ?? string.Empty;
                current.Hidden = (isChecked == "true");                

                string oldUrl = current.RewriteUrl;

                // no entry, generate one
                if (current.RewriteUrl.Trim().Length < 1)
                {
                    current.RewriteUrl = MerchantTribe.Web.Text.Slugify(current.Name, true, true);
                }
                else
                {
                    current.RewriteUrl = MerchantTribe.Web.Text.Slugify(form["rewriteurl"] ?? current.RewriteUrl, true, true);
                }
                
                if (UrlRewriter.IsCategorySlugInUse(current.RewriteUrl, current.Bvin, MTApp.CurrentRequestContext))
                {
                    FlashWarning("The requested URL is already in use by another item.");
                    return false;
                }

                current.TemplateName = posted.TemplateName;
                
                current.CustomerChangeableSortOrder = true;

                
                result = MTApp.CatalogServices.Categories.Update(current);
                                
                if (result == true)
                {
                    if (oldUrl != string.Empty)
                    {
                        if (oldUrl != current.RewriteUrl)
                        {
                            MTApp.ContentServices.CustomUrls.Register301(oldUrl,
                                                  current.RewriteUrl,
                                                  current.Bvin, CustomUrlType.Category, MTApp.CurrentRequestContext,
                                                  MTApp);
                            
                        }
                    }
                }
            }

            return result;
        }


        private void LoadTemplates()
        {
            List<string> templates = MTApp.ThemeManager().ListTemplatesForCurrentTheme();
            ViewData["templates"] = templates;
        }

        private string LoadUrls(Category cat)
        {
            if (cat == null) return string.Empty;

            List<CustomUrl> all = MTApp.ContentServices.CustomUrls.FindBySystemData(cat.Bvin);
            if (all == null) return string.Empty;
            if (all.Count < 1) return string.Empty;
            
            StringBuilder sb = new StringBuilder();
            sb.Append("<ul class=\"redirects301\">");
            foreach (CustomUrl c in all)
            {
                sb.Append("<li>");
                sb.Append(HttpUtility.HtmlEncode(c.RequestedUrl));
                sb.Append(" <a href=\"#\" class=\"remove301\" id=\"remove" + c.Bvin + "\">Remove");
                sb.Append("</a></li>");
            }
            sb.Append("</ul>");
            return sb.ToString();            
        }
    }
}
