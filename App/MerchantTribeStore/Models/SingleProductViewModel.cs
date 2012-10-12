using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MerchantTribe.Commerce.Catalog;
using MerchantTribe.Commerce.Membership;
using System.ComponentModel.DataAnnotations;
using MerchantTribe.Commerce;
using MerchantTribe.Commerce.Utilities;
using StackExchange.Profiling;

namespace MerchantTribeStore.Models
{
    public class SingleProductViewModel
    {
        public Product Item { get; set; }
        public bool IsFirstItem { get; set; }
        public bool IsLastItem { get; set; }
        public UserSpecificPrice UserPrice { get; set; }
        public string ImageUrl { get; set; }
        public string ProductLink { get; set; }
        public string SwatchDisplay { get; set; }

        public SingleProductViewModel()
        {
            Item = null;
            IsFirstItem = false;
            IsLastItem = false;
            UserPrice = null;
            ImageUrl = string.Empty;
            ProductLink = string.Empty;
            SwatchDisplay = string.Empty;
        }

        public SingleProductViewModel(Product p, MerchantTribeApplication mtapp)
        {
            var profiler = MiniProfiler.Current;

            using (profiler.Step("Price Product " + p.ProductName))
            {
                this.UserPrice = mtapp.PriceProduct(p, mtapp.CurrentCustomer, null, mtapp.CurrentlyActiveSales);
            }
            this.IsFirstItem = false;
            this.IsLastItem = false;
            this.Item = p;
            using (profiler.Step("Image Url Product" + p.ProductName))
            {
                this.ImageUrl = MerchantTribe.Commerce.Storage.DiskStorage.ProductImageUrlSmall(
                    mtapp,
                    p.Bvin,
                    p.ImageFileSmall,
                    mtapp.IsCurrentRequestSecure());
            }
            using (profiler.Step("Product Link " + p.ProductName))
            {
                this.ProductLink = UrlRewriter.BuildUrlForProduct(p,
                                                mtapp.CurrentRequestContext.RoutingContext,
                                                string.Empty);
            }
            this.SwatchDisplay = MerchantTribe.Commerce.Utilities.ImageHelper.GenerateSwatchHtmlForProduct(p, mtapp);
        }
        
    }
}