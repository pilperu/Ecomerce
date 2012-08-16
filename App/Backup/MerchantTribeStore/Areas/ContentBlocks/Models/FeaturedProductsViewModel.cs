using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MerchantTribe.Commerce.Catalog;
using MerchantTribe.Web;
using MerchantTribeStore.Models;
using System.ComponentModel.DataAnnotations;

namespace MerchantTribeStore.Areas.ContentBlocks.Models
{
    public class FeaturedProductsViewModel
    {
        public string Title { get; set; }
        public PagerViewModel PagerData { get; set; }
        public List<SingleProductViewModel> Items { get; set; }

        public FeaturedProductsViewModel()
        {
            Title = string.Empty;
            PagerData = new PagerViewModel();
            Items = new List<SingleProductViewModel>();
        }
    }
}