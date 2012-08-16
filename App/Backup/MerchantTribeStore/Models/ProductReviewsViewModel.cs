using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MerchantTribe.Commerce.Catalog;
using System.ComponentModel.DataAnnotations;

namespace MerchantTribeStore.Models
{
    public class ProductReviewsViewModel
    {
        public SingleProductViewModel ProductView { get; set; }        
        public List<ProductReview> Reviews { get; set; }

        public ProductReviewsViewModel()
        {
            ProductView = new SingleProductViewModel();            
            Reviews =new List<ProductReview>();
        }
    }
}