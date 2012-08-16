using System;
using MerchantTribe.Commerce;
using MerchantTribe.Commerce.Catalog;
using MerchantTribe.Commerce.Orders;

namespace MerchantTribeStore
{
    public interface IVariantDisplay
    {
        bool IsValidCombination { get; set; }
        Product GetSelectedProduct(Product currentProduct);
        bool IsValid();
        void WriteValuesToLineItem(LineItem lineItem);
        void Initialize(bool clear);
    }
}