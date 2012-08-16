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
    public class ProductImagesHandler : BaseRestHandler
    {
        public ProductImagesHandler(MerchantTribe.Commerce.MerchantTribeApplication app)
            : base(app)
        {

        }

        // List or Find Single
        public override string GetAction(string parameters, System.Collections.Specialized.NameValueCollection querystring)
        {
            string data = string.Empty;

            // Find One Specific Category
            ApiResponse<ProductImageDTO> response = new ApiResponse<ProductImageDTO>();
            string bvin = FirstParameter(parameters);
            ProductImage item = MTApp.CatalogServices.ProductImages.Find(bvin);
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
            ApiResponse<ProductImageDTO> response = new ApiResponse<ProductImageDTO>();

            ProductImageDTO postedItem = null;
            try
            {
                postedItem = MerchantTribe.Web.Json.ObjectFromJson<ProductImageDTO>(postdata);
            }
            catch (Exception ex)
            {
                response.Errors.Add(new ApiError("EXCEPTION", ex.Message));
                return MerchantTribe.Web.Json.ObjectToJson(response);
            }

            ProductImage item = new ProductImage();
            item.FromDto(postedItem);

            if (bvin == string.Empty)
            {
                if (MTApp.CatalogServices.ProductImages.Create(item))
                {
                    bvin = item.Bvin;
                }
            }
            else
            {
                MTApp.CatalogServices.ProductImages.Update(item);
            }
            ProductImage resultItem = MTApp.CatalogServices.ProductImages.Find(bvin);
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
            response.Content = MTApp.CatalogServices.ProductImages.Delete(bvin);

            data = MerchantTribe.Web.Json.ObjectToJson(response);
            return data;
        }
    }
}