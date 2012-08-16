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

namespace MerchantTribeStore.Areas.ContentBlocks.RenderControllers
{
    public class SideMenuRenderController : BaseRenderController, IContentBlockRenderController
    {
        public string Render(MerchantTribe.Commerce.MerchantTribeApplication app, dynamic viewBag, MerchantTribe.Commerce.Content.ContentBlock block)
        {
            SideMenuViewModel model = new SideMenuViewModel();

            if (block != null)
            {
                model.Title = block.BaseSettings.GetSettingOrEmpty("Title");

                List<ContentBlockSettingListItem> links = block.Lists.FindList("Links");
                if (links != null)
                {
                    foreach (ContentBlockSettingListItem l in links)
                    {
                        model.Items.Add(AddSingleLink(l));
                    }
                }
            }

            return RenderModel(model);
        }
        public string RenderModel(SideMenuViewModel model)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("<div class=\"sidemenu\">");
            sb.Append("<div class=\"decoratedblock\">");
            sb.Append("<h4>" + HttpUtility.HtmlEncode(model.Title) + "</h4>");
            sb.Append("<ul>");
            foreach (var item in model.Items)
            {
                sb.Append("<li><a href=\"" + item.Url + "\" class=\"" + HttpUtility.HtmlEncode(item.CssClass) + "\" title=\"" + HttpUtility.HtmlEncode(item.Title));
                if (item.OpenInNewWindow == true)
                {
                    sb.Append(" target=\"_blank\"");
                }
                sb.Append("\">" + HttpUtility.HtmlEncode(item.Name) + "</a></li>");
            }
            sb.Append("</ul>");
            sb.Append("</div>");
            sb.Append("</div>");
                               
            return sb.ToString();
        }

        private SideMenuItem AddSingleLink(ContentBlockSettingListItem l)
        {
            SideMenuItem result = new SideMenuItem();
            result.Title = l.Setting4;
            result.Name = l.Setting1;
            result.Url = l.Setting2;
            if (l.Setting3 == "1")
            {
                result.OpenInNewWindow = true;
            }
            result.CssClass = l.Setting5;
            return result;
        }
    }
}