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
    public class ProductPropertiesHandler: BaseRestHandler
    {
        public ProductPropertiesHandler(MerchantTribe.Commerce.MerchantTribeApplication app)
            : base(app)
        {

        }

        // List or Find Single
        public override string GetAction(string parameters, System.Collections.Specialized.NameValueCollection querystring)
        {            
            string data = string.Empty;

            if (string.Empty == (parameters ?? string.Empty))
            {
                // List
                ApiResponse<List<ProductPropertyDTO>> response = new ApiResponse<List<ProductPropertyDTO>>();

                List<ProductProperty> results = MTApp.CatalogServices.ProductProperties.FindAll();
                List<ProductPropertyDTO> dto = new List<ProductPropertyDTO>();

                foreach (ProductProperty item in results)
                {
                    dto.Add(item.ToDto());
                }
                response.Content = dto;
                data = MerchantTribe.Web.Json.ObjectToJson(response);
            }
            else
            {
                // Find One Specific Category
                ApiResponse<ProductPropertyDTO> response = new ApiResponse<ProductPropertyDTO>();
                string ids = FirstParameter(parameters);
                long id = 0;
                long.TryParse(ids, out id);
                ProductProperty item = MTApp.CatalogServices.ProductProperties.Find(id);
                if (item == null)
                {
                    response.Errors.Add(new ApiError("NULL", "Could not locate that item. Check id and try again."));
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
            string ids = FirstParameter(parameters);
            long id = 0;
            long.TryParse(ids, out id);

            string isValueSetRequest = GetParameterByIndex(1, parameters);

            
            if (isValueSetRequest.Trim().ToLowerInvariant() == "valuesforproduct")
            {
                // Set Property value Request
                ApiResponse<bool> response = new ApiResponse<bool>();

                string productBvin = GetParameterByIndex(2, parameters);
                string propertyValue = postdata;
                response.Content = MTApp.CatalogServices.ProductPropertyValues.SetPropertyValue(productBvin, id, propertyValue);

                data = MerchantTribe.Web.Json.ObjectToJson(response);
            }
            else
            {
                // Regular Create or Update Request

                ApiResponse<ProductPropertyDTO> response = new ApiResponse<ProductPropertyDTO>();
                ProductPropertyDTO postedItem = null;
                try
                {
                    postedItem = MerchantTribe.Web.Json.ObjectFromJson<ProductPropertyDTO>(postdata);
                }
                catch (Exception ex)
                {
                    response.Errors.Add(new ApiError("EXCEPTION", ex.Message));
                    return MerchantTribe.Web.Json.ObjectToJson(response);
                }

                ProductProperty item = new ProductProperty();
                item.FromDto(postedItem);


                // Check for existing and create base property
                ProductProperty existing = MTApp.CatalogServices.ProductProperties.FindByName(item.DisplayName);
                if (existing == null)
                {
                    // Create
                    MTApp.CatalogServices.ProductProperties.Create(item);
                    id = item.Id;

                    //// Merge Properties
                    //foreach (ProductPropertyChoice ppc in item.Choices)
                    //{
                    //    ppc.PropertyId = item.Id;
                    //    ProductPropertyChoice.Insert(ppc);
                    //}
                }
                else
                {
                    // Update
                    MTApp.CatalogServices.ProductProperties.Update(item);
                    id = existing.Id;

                    //// Merge Properties
                    //if (item.Choices.Count > 0)
                    //{
                    //    List<ProductPropertyChoice> existingChoices = ProductPropertyChoice.FindByPropertyID(existing.Id);
                    //    foreach (ProductPropertyChoice newChoice in item.Choices)
                    //    {
                    //        var existCount = existingChoices.Where(y => y.Id == newChoice.Id).Count();
                    //        if (existCount < 1)
                    //        {
                    //            newChoice.PropertyId = existing.Id;
                    //            ProductPropertyChoice.Insert(newChoice);
                    //        }
                    //        else
                    //        {
                    //            ProductPropertyChoice.Update(newChoice);
                    //        }
                    //    }
                    //}
                }

                ProductProperty resultItem = MTApp.CatalogServices.ProductProperties.Find(id);
                if (resultItem != null) response.Content = resultItem.ToDto();

                data = MerchantTribe.Web.Json.ObjectToJson(response);            
            }

            
            return data;
        }

        public override string DeleteAction(string parameters, System.Collections.Specialized.NameValueCollection querystring, string postdata)
        {
            string data = string.Empty;
            string ids = FirstParameter(parameters);
            long id = 0;
            long.TryParse(ids, out id);
            ApiResponse<bool> response = new ApiResponse<bool>();

                // Single Item Delete
                response.Content = MTApp.CatalogServices.ProductPropertiesDestroy(id);

            data = MerchantTribe.Web.Json.ObjectToJson(response);
            return data;
        }
    }
}