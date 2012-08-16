using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;

namespace MerchantTribeStore.Areas.ContentBlocks.Models
{
    public class CategoryMenuViewModel
    {
        public bool ShowProductCounts { get; set; }
        public bool ShowCategoryCounts { get; set; }
        public string CurrentId { get; set; }
        public string Title { get; set; }
        public string Contents { get { return sb.ToString(); } }
        public StringBuilder sb { get; private set; }

        public CategoryMenuViewModel()
        {
            ShowProductCounts = false;
            ShowCategoryCounts = false;
            CurrentId = "0";
            Title = string.Empty;
            sb = new StringBuilder();
        }
    }
}