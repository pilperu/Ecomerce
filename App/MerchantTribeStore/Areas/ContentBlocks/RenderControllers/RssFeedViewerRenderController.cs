using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MerchantTribeStore.Areas.ContentBlocks.Models;
using MerchantTribe.Commerce;
using MerchantTribe.Web.Rss;
using System.Text;

namespace MerchantTribeStore.Areas.ContentBlocks.RenderControllers
{
    public class RssFeedViewerRenderController : IContentBlockRenderController
    {
        public string Render(MerchantTribe.Commerce.MerchantTribeApplication app, dynamic viewBag, MerchantTribe.Commerce.Content.ContentBlock block)
        {
            RssFeedViewModel model = new RssFeedViewModel();
            model.Channel = new RSSChannel(new MerchantTribe.Commerce.EventLog());

            if (block != null)
            {
                string feedUrl = block.BaseSettings.GetSettingOrEmpty("FeedUrl");
                model.Channel.LoadFromFeed(feedUrl);
                model.ShowTitle = block.BaseSettings.GetBoolSetting("ShowTitle");
                model.ShowDescription = block.BaseSettings.GetBoolSetting("ShowDescription");
                int max = block.BaseSettings.GetIntegerSetting("MaxItems");
                if (max <= 0)
                {
                    max = 5;
                }
                model.MaxItems = max;
            }

            return RenderModel(model);
        }

        private string RenderModel(RssFeedViewModel model)
        {            
            StringBuilder sb = new StringBuilder();

            sb.Append("<div class=\"rssfeedviewer\">");
            sb.Append("<div class=\"rssfeedviewerwrapper\">");
            if (model.ShowTitle)
            {
                sb.Append("<h4>" + HttpUtility.HtmlEncode(model.Channel.Title) + "</h4>");
            }
            if (model.ShowDescription)
            {
                sb.Append(model.Channel.Description);
            }
            sb.Append("<ul>");
            foreach (var item in model.Channel.GetChannelItems(model.MaxItems))
            {
                sb.Append("<li><a href=\"" + item.Link + "\">" + item.Title + "</a><br />");
                sb.Append(item.Description + "</li>");
            }   
            sb.Append("</ul>");
            sb.Append("</div>");
            sb.Append("</div>");        

            return sb.ToString();
        }
    }
}