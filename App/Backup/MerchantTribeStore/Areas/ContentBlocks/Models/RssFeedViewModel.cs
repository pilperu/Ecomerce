using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MerchantTribe.Web.Rss;

namespace MerchantTribeStore.Areas.ContentBlocks.Models
{
    public class RssFeedViewModel
    {
        public RSSChannel Channel { get; set; }
        public bool ShowTitle { get; set; }
        public bool ShowDescription { get; set; }
        public int MaxItems { get; set; }

        public RssFeedViewModel()
        {
            Channel = null;
            ShowTitle = false;
            ShowDescription = false;
            MaxItems = 5;
        }
    }
}