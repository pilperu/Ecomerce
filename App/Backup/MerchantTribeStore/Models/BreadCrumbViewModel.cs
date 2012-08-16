using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace MerchantTribeStore.Models
{
    public class BreadCrumbItem
    {
        public string Name { get; set; }
        public string Link { get; set; }
        public string Title { get; set; }

        public BreadCrumbItem()
        {
            Name = string.Empty;
            Link = string.Empty;
            Title = string.Empty;
        }
    }
    public class BreadCrumbViewModel
    {
        public bool HideHomeLink { get; set; }
        public string HomeName { get; set; }
        public string Spacer { get; set; }        
        public Queue<BreadCrumbItem> Items { get; private set; }

        public BreadCrumbViewModel()
        {
            this.HomeName = "Home";
            this.Spacer = "<span class=\"spacer\">&nbsp;&raquo;&nbsp;</span>";
            this.HideHomeLink = false;
            this.Items = new Queue<BreadCrumbItem>();
        }
    }
}