using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MerchantTribe.Commerce;
using MerchantTribe.Commerce.Catalog;
using System.ComponentModel.DataAnnotations;

namespace MerchantTribeStore.Models
{
    public class ProductPageViewModel
    {
        public Product LocalProduct { get; set; }
        public string PreRenderedOptions { get; set; }
        public string JavaScripts { get; set; }
        public bool IsAvailableForSale { get; set; }
        public string StockMessage { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public int Quantity { get; set; }
        public string InitialQuantity 
        {
            get { return Quantity.ToString(); } 
        }
        public OptionSelectionList Selections { get; set; }
        public string MainImageUrl { get; set; }
        public string MainImageAltText { get; set; }
        public string PreRenderedPrices { get; set; }
        public string PreRenderedImages { get; set; }
        public string PreRenderedTypeValues { get; set; }
        public List<SingleProductViewModel> RelatedItems { get; set; }
        public string ValidationMessage { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string LineItemId { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string OrderId { get; set; }
        public bool IsAvailableForWishList { get; set; }
        public string PostColumn { get; set; }
        public string PreColumn { get; set; }
        public string BreadCrumbsFinal { get; set; }

        public ProductPageViewModel()
        {
            this.LocalProduct = new Product();
            this.PreRenderedOptions = string.Empty;
            this.JavaScripts = string.Empty;
            this.IsAvailableForSale = true;
            this.StockMessage = string.Empty;        
            this.Quantity = 1;
            this.Selections = new OptionSelectionList();
            this.MainImageAltText = string.Empty;
            this.MainImageUrl = string.Empty;
            this.PreRenderedPrices = string.Empty;
            this.PreRenderedImages = string.Empty;
            this.PreRenderedTypeValues = string.Empty;
            this.RelatedItems = new List<SingleProductViewModel>();
            this.ValidationMessage = string.Empty;
            this.LineItemId = string.Empty;
            this.OrderId = string.Empty;
            this.IsAvailableForWishList = false;
            this.PreColumn = string.Empty;
            this.PostColumn = string.Empty;
            this.BreadCrumbsFinal = string.Empty;
        }
    }
}