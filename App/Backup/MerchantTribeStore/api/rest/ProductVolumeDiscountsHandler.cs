using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;
using MerchantTribe.Commerce;
using MerchantTribe.CommerceDTO.v1;
using MerchantTribe.CommerceDTO.v1.Catalog;
using MerchantTribe.Commerce.Catalog;

namespace MerchantTribeStore.api.rest
{
    public class ProductVolumeDiscountsHandler: BaseRestHandler
    {
        public ProductVolumeDiscountsHandler(MerchantTribe.Commerce.MerchantTribeApplication app)
            : base(app)
        {

        }

        // List or Find Single
        public override string GetAction(string parameters, System.Collections.Specialized.NameValueCollection querystring)
        {
            string data = string.Empty;

            // Find One Specific Category
            ApiResponse<ProductVolumeDiscountDTO> response = new ApiResponse<ProductVolumeDiscountDTO>();
            string bvin = FirstParameter(parameters);
            ProductVolumeDiscount item = MTApp.CatalogServices.VolumeDiscounts.Find(bvin);
            if (item == null)
            {
                response.Errors.Add(new ApiError("NULL", "Could not locate that item. Check bvin and try again."));
            }
            else
            {
                response.Content = item.ToDto();
            }
            data = MerchantTribe.Web.Json.ObjectToJson(response);

            return data;
        }

        // Create or Update
        public override string PostAction(string parameters, System.Collections.Specialized.NameValueCollection querystring, string postdata)
        {
            string data = string.Empty;
            string bvin = FirstParameter(parameters);
            ApiResponse<ProductVolumeDiscountDTO> response = new ApiResponse<ProductVolumeDiscountDTO>();

            ProductVolumeDiscountDTO postedItem = null;
            try
            {
                postedItem = MerchantTribe.Web.Json.ObjectFromJson<ProductVolumeDiscountDTO>(postdata);
            }
            catch (Exception ex)
            {
                response.Errors.Add(new ApiError("EXCEPTION", ex.Message));
                return MerchantTribe.Web.Json.ObjectToJson(response);
            }

            ProductVolumeDiscount item = new ProductVolumeDiscount();
            item.FromDto(postedItem);

            if (bvin == string.Empty)
            {
                if (MTApp.CatalogServices.VolumeDiscounts.Create(item))
                {
                    bvin = item.Bvin;
                }
            }
            else
            {
                MTApp.CatalogServices.VolumeDiscounts.Update(item);
            }
            ProductVolumeDiscount resultItem = MTApp.CatalogServices.VolumeDiscounts.Find(bvin);
            if (resultItem != null) response.Content = resultItem.ToDto();

            data = MerchantTribe.Web.Json.ObjectToJson(response);
            return data;
        }

        public override string DeleteAction(string parameters, System.Collections.Specialized.NameValueCollection querystring, string postdata)
        {
            string data = string.Empty;
            string bvin = FirstParameter(parameters);
            ApiResponse<bool> response = new ApiResponse<bool>();

            // Single Item Delete
            response.Content = MTApp.CatalogServices.VolumeDiscounts.Delete(bvin);

            data = MerchantTribe.Web.Json.ObjectToJson(response);
            return data;
        }
    }
}