using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MerchantTribe.Commerce.Catalog;
using MerchantTribe.Web;
using System.ComponentModel.DataAnnotations;

namespace MerchantTribeStore.Models
{
    public class ProductListViewModel
    {
        public string Title { get; set; }
        public PagerViewModel PagerData { get; set; }
        public List<Product> Items { get; set; }

        public ProductListViewModel()
        {
            Title = string.Empty;
            PagerData = new PagerViewModel();
            Items = new List<Product>();
        }
    }
}