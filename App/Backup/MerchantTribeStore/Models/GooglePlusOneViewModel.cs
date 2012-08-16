using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MerchantTribeStore.Models
{
    public enum GooglePlusOneSize
    {
        Small = 0,
        Medium = 1,
        Standard = 2,
        Large = 3
    }
    public enum GooglePlusOneCountPosition
    {
        None = 0,
        Bubble = 1,
        Inline = 2
    }

    public class GooglePlusOneViewModel
    {
        public string PageUrl { get; set; }
        public GooglePlusOneSize Size { get; set; }
        public GooglePlusOneCountPosition CountPosition { get; set; }

        public GooglePlusOneViewModel()
        {
            this.PageUrl = string.Empty;
            this.Size = GooglePlusOneSize.Standard;
            this.CountPosition = GooglePlusOneCountPosition.Inline;
        }
    }
}