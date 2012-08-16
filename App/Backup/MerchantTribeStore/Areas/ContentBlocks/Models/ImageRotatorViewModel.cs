using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MerchantTribeStore.Areas.ContentBlocks.Models
{
    public class ImageRotatorImageViewModel
    {
        
        public string ImageUrl { get; set; }
        public string Caption { get; set; }
        public string Url { get; set; }
        public bool NewWindow { get; set; }

        public ImageRotatorImageViewModel()
        {            
            ImageUrl = string.Empty; // 1            
            Url = string.Empty; // 2
            NewWindow = false; // 3
            Caption = string.Empty; // 4
        }
    }
    public class ImageRotatorViewModel
    {
        public List<ImageRotatorImageViewModel> Images { get; set; }            
        public string CssId { get; set; }
        public string CssClass { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
        public int Pause { get; set; }

            public ImageRotatorViewModel()
            {
                this.Pause = 3;
                this.Images = new List<ImageRotatorImageViewModel>();
                this.CssClass = string.Empty;
                this.CssId = string.Empty;
                this.Height = 0;
                this.Width = 0;
            }
     }
}