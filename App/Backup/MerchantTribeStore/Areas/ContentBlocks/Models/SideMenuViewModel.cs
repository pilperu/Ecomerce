using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MerchantTribeStore.Areas.ContentBlocks.Models
{
    public class SideMenuViewModel
    {
        public List<SideMenuItem> Items { get; set; }
        public string Title { get; set; }

        public SideMenuViewModel()
        {
            Items = new List<SideMenuItem>();
            Title = string.Empty;
        }
    }
}