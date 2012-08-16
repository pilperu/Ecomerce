using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MerchantTribe.Commerce.Orders;

namespace MerchantTribeStore.Models
{
    public class CartViewModel
    {
        public Order CurrentOrder { get; set; }
        public string AddCouponButtonUrl { get; set; }
        public string DeleteButtonUrl { get; set; }
        public string KeepShoppingButtonUrl { get; set; }
        public string KeepShoppingUrl { get; set; }
        public string CheckoutButtonUrl { get; set; }
        public string EstimateShippingButtonUrl { get; set; }
        public string DisplayTitle { get; set; }
        public string DisplaySubTitle { get; set; }
        public string ItemListTitle { get; set; }
        public bool CartEmpty { get; set; }
        public List<CartLineItemViewModel> LineItems { get; set; }
        public bool PayPalExpressAvailable { get; set; }

        public CartViewModel()
        {
            CurrentOrder = new Order();
            this.AddCouponButtonUrl = string.Empty;
            this.DeleteButtonUrl = string.Empty;
            this.KeepShoppingButtonUrl = string.Empty;
            this.KeepShoppingUrl = string.Empty;
            this.CheckoutButtonUrl = string.Empty;
            this.EstimateShippingButtonUrl = string.Empty;
            this.DisplayTitle = "Shopping Cart";
            this.DisplaySubTitle = string.Empty;
            this.ItemListTitle = string.Empty;
            this.CartEmpty = false;
            this.LineItems = new List<CartLineItemViewModel>();
            this.PayPalExpressAvailable = false;
        }
    }
}