using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace MerchantTribeStore.Models
{
    public class SingleCategoryViewModel
    {
        public string AltText { get; set; }
        public string IconUrl { get; set; }
        public string LinkUrl { get; set; }
        public string Name { get; set; }
        public bool OpenInNewWindow { get; set; }
        public bool IsFirstItem { get; set; }
        public bool IsLastItem { get; set; }

        public SingleCategoryViewModel()
        {
            this.AltText = string.Empty;
            this.IconUrl = string.Empty;
            this.LinkUrl = string.Empty;
            this.Name = string.Empty;
            this.OpenInNewWindow = false;
            this.IsFirstItem = false;
            this.IsLastItem = false;
        }
    }
}