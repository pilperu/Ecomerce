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
    public class ProductReviewsHandler: BaseRestHandler
    {
        public ProductReviewsHandler(MerchantTribe.Commerce.MerchantTribeApplication app)
            : base(app)
        {

        }

        // List or Find Single
        public override string GetAction(string parameters, System.Collections.Specialized.NameValueCollection querystring)
        {
            string data = string.Empty;

            if (string.Empty == (parameters ?? string.Empty))
            {
                string byProduct = querystring["productbvin"] ?? string.Empty;
                
                // Find All                
                ApiResponse<List<ProductReviewDTO>> response = new ApiResponse<List<ProductReviewDTO>>();
                List<ProductReview> items = new List<ProductReview>();
                if (byProduct != string.Empty)
                {
                    items = MTApp.CatalogServices.ProductReviews.FindByProductId(byProduct);
                }
                else
                {
                    items = MTApp.CatalogServices.ProductReviews.FindAllPaged(1, int.MaxValue);
                }
                 
                if (items == null)
                {
                    response.Errors.Add(new ApiError("NULL", "Could not locate reviews. Try again."));
                }
                else
                {
                    foreach (ProductReview r in items)
                    {
                        response.Content.Add(r.ToDto());
                    }                    
                }
                data = MerchantTribe.Web.Json.ObjectToJson(response);
            }
            else
            {

                // Find One Specific Category
                ApiResponse<ProductReviewDTO> response = new ApiResponse<ProductReviewDTO>();
                string bvin = FirstParameter(parameters);
                ProductReview item = MTApp.CatalogServices.ProductReviews.Find(bvin);
                if (item == null)
                {
                    response.Errors.Add(new ApiError("NULL", "Could not locate that item. Check bvin and try again."));
                }
                else
                {
                    response.Content = item.ToDto();
                }
                data = MerchantTribe.Web.Json.ObjectToJson(response);
            }

            return data;
        }

        // Create or Update
        public override string PostAction(string parameters, System.Collections.Specialized.NameValueCollection querystring, string postdata)
        {
            string data = string.Empty;
            string bvin = FirstParameter(parameters);
            ApiResponse<ProductReviewDTO> response = new ApiResponse<ProductReviewDTO>();

            ProductReviewDTO postedItem = null;
            try
            {
                postedItem = MerchantTribe.Web.Json.ObjectFromJson<ProductReviewDTO>(postdata);
            }
            catch (Exception ex)
            {
                response.Errors.Add(new ApiError("EXCEPTION", ex.Message));
                return MerchantTribe.Web.Json.ObjectToJson(response);
            }

            ProductReview item = new ProductReview();
            item.FromDto(postedItem);

            if (bvin == string.Empty)
            {
                if (MTApp.CatalogServices.ProductReviews.Create(item))
                {
                    bvin = item.Bvin;
                }
            }
            else
            {
                MTApp.CatalogServices.ProductReviews.Update(item);
            }
            ProductReview resultItem = MTApp.CatalogServices.ProductReviews.Find(bvin);
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
            response.Content = MTApp.CatalogServices.ProductReviews.Delete(bvin);

            data = MerchantTribe.Web.Json.ObjectToJson(response);
            return data;
        }
    }
}