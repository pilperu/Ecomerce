using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MerchantTribeStore.Areas.ContentBlocks.Models
{
    public class BannerAdViewModel
    {
        public string ImageUrl { get; set; }
        public string AltText  { get; set; }
        public string CssId    { get; set; }
        public string CssClass { get; set; }
        public string LinkUrl  { get; set; }

        public BannerAdViewModel()
        {
            this.ImageUrl = string.Empty;
            this.AltText = string.Empty;
            this.CssClass = string.Empty;
            this.CssId = string.Empty;
            this.LinkUrl = string.Empty;
        }
    }
}