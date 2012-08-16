using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MerchantTribe.Commerce;
using MerchantTribe.Commerce.Orders;
using MerchantTribe.Commerce.Utilities;

namespace MerchantTribeStore.Models
{
    public class CartLineItemViewModel
    {
        public LineItem Item { get; set; }
        public bool ShowImage { get; set; }
        public string ImageUrl { get; set; }
        public string LinkUrl { get; set; }
        public bool HasDiscounts { get; set; }

        public CartLineItemViewModel()
        {
            this.Item = new LineItem();
            this.ShowImage = false;
            this.ImageUrl = string.Empty;
            this.LinkUrl = "#";
            this.HasDiscounts = false;
        }
    }
}