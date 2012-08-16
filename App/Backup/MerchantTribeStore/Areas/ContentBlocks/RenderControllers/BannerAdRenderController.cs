using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MerchantTribeStore.Areas.ContentBlocks.Models;
using MerchantTribe.Commerce.Utilities;
using System.Text;

namespace MerchantTribeStore.Areas.ContentBlocks.RenderControllers
{
    public class BannerAdRenderController: BaseRenderController,IContentBlockRenderController
    {
        public string Render(MerchantTribe.Commerce.MerchantTribeApplication app, dynamic viewBag, MerchantTribe.Commerce.Content.ContentBlock block)
        {

            BannerAdViewModel model = new BannerAdViewModel();

            if (block != null)
            {
                model.ImageUrl = block.BaseSettings.GetSettingOrEmpty("imageurl");
                model.AltText = block.BaseSettings.GetSettingOrEmpty("alttext");
                model.CssId = block.BaseSettings.GetSettingOrEmpty("cssid");
                model.CssClass = block.BaseSettings.GetSettingOrEmpty("cssclass");
                model.LinkUrl = block.BaseSettings.GetSettingOrEmpty("linkurl");
                model.ImageUrl = model.ImageUrl; // TagReplacer.ReplaceContentTags(model.ImageUrl,
                                                 //               app,
                                                 //               "",
                                                 //               app.IsCurrentRequestSecure());
            }
            return RenderModel(model);            
        }

        private string RenderModel(BannerAdViewModel model)
        {
            StringBuilder sb = new StringBuilder();
            
            if (model.LinkUrl.Trim().Length > 0)
            {    
                sb.Append("<a href=\"" + model.LinkUrl + "\" title=\"" + SafeHtml(model.AltText) + "\" ");
                sb.Append("id=\"" + SafeHtml(model.CssId) + "\" class=\"" + SafeHtml(model.CssClass) + "\">");
                sb.Append("<img src=\"" + model.ImageUrl + "\" alt=\"" + SafeHtml(model.AltText) + "\" />");
                sb.Append("</a>");
            }
            else
            {
                sb.Append("<span id=\"" + model.CssId + "\" class=\"" + SafeHtml(model.CssClass) + "\">");
                sb.Append("<img src=\"" + model.ImageUrl + "\" alt=\"" + SafeHtml(model.AltText) + "\" />");
                sb.Append("</span>");
            }

            return sb.ToString();
        }
    }
}