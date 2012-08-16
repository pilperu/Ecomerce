using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MerchantTribe.Commerce.Catalog;
using MerchantTribeStore.Models;

namespace MerchantTribeStore.Areas.account.Models
{
    public class SavedItemViewModel
    {
        public WishListItem SavedItem { get; set; }
        public SingleProductViewModel FullProduct { get; set; }

        public SavedItemViewModel()
        {
            this.SavedItem = new WishListItem();
            this.FullProduct = new SingleProductViewModel();
        }
    }
}