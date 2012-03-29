using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MerchantTribeStore.Models
{
    public class StoreListViewModel
    {
        public PagerViewModel PagerData { get; set; }
        public List<SuperStoreViewModel> Stores { get; set; }

        public StoreListViewModel()
        {
            PagerData = new PagerViewModel();
            Stores = new List<SuperStoreViewModel>();
        }
    }
}