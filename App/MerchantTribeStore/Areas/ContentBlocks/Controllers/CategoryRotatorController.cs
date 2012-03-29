using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MerchantTribe.Commerce.Catalog;
using MerchantTribe.Commerce.Content;
using MerchantTribe.Commerce.Utilities;
using MerchantTribeStore.Controllers.Shared;
using MerchantTribeStore.Models;
using MerchantTribeStore.Areas.ContentBlocks.Models;
using MvcMiniProfiler;

namespace MerchantTribeStore.Areas.ContentBlocks.Controllers
{
    public class CategoryRotatorController : BaseAppController
    {
        //
        // GET: /ContentBlocks/CategoryRotator/
        public ActionResult Index(ContentBlock block)
        {
            var profiler = MvcMiniProfiler.MiniProfiler.Current;
            using (profiler.Step("Block:CategoryRotator"))
            {
                SingleCategoryViewModel model = new SingleCategoryViewModel();

                bool showInOrder = false;
                if (block != null)
                {
                    showInOrder = block.BaseSettings.GetBoolSetting("ShowInOrder");

                    int nextIndex;
                    if (Session[block.Bvin + "NextImageIndex"] != null)
                    {
                        nextIndex = (int)Session[block.Bvin + "NextImageIndex"];
                    }
                    else
                    {
                        nextIndex = 0;
                    }

                    List<ContentBlockSettingListItem> settings = block.Lists.FindList("Categories");

                    if (settings.Count != 0)
                    {
                        if (settings.Count > nextIndex)
                        {
                            LoadCategory(model, settings[nextIndex].Setting1);
                        }
                        else if (nextIndex >= settings.Count)
                        {
                            if (showInOrder)
                            {
                                nextIndex = 0;
                            }
                            else
                            {
                                nextIndex = MerchantTribe.Web.RandomNumbers.RandomInteger(settings.Count - 1, 0);
                            }
                            LoadCategory(model, settings[nextIndex].Setting1);
                        }

                        if (showInOrder)
                        {
                            nextIndex += 1;
                        }
                        else
                        {
                            nextIndex = MerchantTribe.Web.RandomNumbers.RandomInteger(settings.Count - 1, 0);
                        }
                        Session[block.Bvin + "NextImageIndex"] = nextIndex;
                    }

                }

                return View(model);
            }
        }
  
        private void LoadCategory(SingleCategoryViewModel model, string categoryId)
        {
            Category c = MTApp.CatalogServices.Categories.Find(categoryId);
            if (c != null)
            {
                if (c.Bvin != string.Empty)
                {
                    string destination = UrlRewriter.BuildUrlForCategory(new CategorySnapshot(c), MTApp.CurrentRequestContext.RoutingContext);

                    if (c.ImageUrl.StartsWith("~") | c.ImageUrl.StartsWith("http://"))
                    {
                        model.IconUrl = ImageHelper.SafeImage(Url.Content(c.ImageUrl));
                    }
                    else
                    {
                        model.IconUrl = ImageHelper.SafeImage(Url.Content("~/" + c.ImageUrl));
                    }

                    model.LinkUrl = destination;
                    model.AltText = c.MetaTitle;
                    model.Name = c.Name;
                    
                    if (c.SourceType == CategorySourceType.CustomLink)
                    {
                        model.OpenInNewWindow = c.CustomPageOpenInNewWindow;                        
                    }
                }
            }
        }

    }
}
