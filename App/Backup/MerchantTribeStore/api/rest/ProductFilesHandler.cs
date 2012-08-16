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
    public class ProductFilesHandler: BaseRestHandler
    {
        public ProductFilesHandler(MerchantTribe.Commerce.MerchantTribeApplication app)
            : base(app)
        {

        }

        // List or Find Single
        public override string GetAction(string parameters, System.Collections.Specialized.NameValueCollection querystring)
        {            
            string data = string.Empty;

            
                // Find One Specific Category
                ApiResponse<ProductFileDTO> response = new ApiResponse<ProductFileDTO>();                                
                string bvin = FirstParameter(parameters);
                ProductFile item = MTApp.CatalogServices.ProductFiles.Find(bvin);
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
            ApiResponse<ProductFileDTO> response = new ApiResponse<ProductFileDTO>();

            ProductFileDTO postedItem = null;
            try
            {
                postedItem = MerchantTribe.Web.Json.ObjectFromJson<ProductFileDTO>(postdata);
            }
            catch(Exception ex)
            {
                response.Errors.Add(new ApiError("EXCEPTION", ex.Message));
                return MerchantTribe.Web.Json.ObjectToJson(response);                
            }

            ProductFile item = new ProductFile();
            item.FromDto(postedItem);

            if (bvin == string.Empty)
            {
                if (MTApp.CatalogServices.ProductFiles.Create(item))
                {
                    bvin = item.Bvin;
                }
            }
            else
            {
                MTApp.CatalogServices.ProductFiles.Update(item);
            }
            ProductFile resultItem = MTApp.CatalogServices.ProductFiles.Find(bvin);                    
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
            response.Content = MTApp.CatalogServices.ProductFiles.Delete(bvin);

            data = MerchantTribe.Web.Json.ObjectToJson(response);
            return data;
        }
    }
}