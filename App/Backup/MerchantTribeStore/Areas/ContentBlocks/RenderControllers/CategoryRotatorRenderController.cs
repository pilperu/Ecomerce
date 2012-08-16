using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using MerchantTribe.Commerce;
using MerchantTribe.Commerce.Catalog;
using MerchantTribe.Commerce.Content;
using MerchantTribeStore.Areas.ContentBlocks.Models;
using MerchantTribe.Commerce.Utilities;
using MerchantTribeStore.Models;

namespace MerchantTribeStore.Areas.ContentBlocks.RenderControllers
{
    public class CategoryRotatorRenderController : BaseRenderController, IContentBlockRenderController
    {
        public string Render(MerchantTribe.Commerce.MerchantTribeApplication app, dynamic viewBag, MerchantTribe.Commerce.Content.ContentBlock block)
        {
            SingleCategoryViewModel model = new SingleCategoryViewModel();

            bool showInOrder = false;
            if (block != null)
            {
                showInOrder = block.BaseSettings.GetBoolSetting("ShowInOrder");
                
                List<ContentBlockSettingListItem> settings = block.Lists.FindList("Categories");

                int nextIndex = GetProductIndex(settings.Count -1);                

                if (settings.Count != 0)
                {
                    if (settings.Count > nextIndex)
                    {
                        LoadCategory(model, settings[nextIndex].Setting1, app);
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
                        LoadCategory(model, settings[nextIndex].Setting1, app);
                    }

                    if (showInOrder)
                    {
                        nextIndex += 1;
                    }
                    else
                    {
                        nextIndex = MerchantTribe.Web.RandomNumbers.RandomInteger(settings.Count - 1, 0);
                    }                    
                }

            }

            return RenderModel(model);
        }
        
        private int GetProductIndex(int maxIndex)
        {
            int result = 0;

            result = MerchantTribe.Web.RandomNumbers.RandomInteger(maxIndex, 0);

            return result;
        }

        private void LoadCategory(SingleCategoryViewModel model, string categoryId, MerchantTribeApplication app)
        {
            Category c = app.CatalogServices.Categories.Find(categoryId);
            if (c != null)
            {
                if (c.Bvin != string.Empty)
                {
                    string destination = UrlRewriter.BuildUrlForCategory(new CategorySnapshot(c), app.CurrentRequestContext.RoutingContext);

                    if (c.ImageUrl.StartsWith("~") | c.ImageUrl.StartsWith("http://"))
                    {
                        model.IconUrl = ImageHelper.SafeImage(app.CurrentRequestContext.UrlHelper.Content(c.ImageUrl));
                    }
                    else
                    {
                        model.IconUrl = ImageHelper.SafeImage(app.CurrentRequestContext.UrlHelper.Content("~/" + c.ImageUrl));
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

        public string RenderModel(SingleCategoryViewModel model)
        {
            StringBuilder sb = new StringBuilder();
            
            sb.Append("<div class=\"categoryrotator\"><div class=\"decoratedblock\"><div class=\"blockcontent\">");
            sb.Append("<a href=\"" + model.LinkUrl + "\" title=\"" + HttpUtility.HtmlEncode(model.AltText) + "\">");
            sb.Append("<img src=\"" + model.IconUrl + "\" alt=\"" + HttpUtility.HtmlEncode(model.AltText) + "\" /><br />");
            sb.Append("<span>" + HttpUtility.HtmlEncode(model.Name) + "</span></a>");                      
            sb.Append("</div></div></div>");

            return sb.ToString();
        }
    }
}