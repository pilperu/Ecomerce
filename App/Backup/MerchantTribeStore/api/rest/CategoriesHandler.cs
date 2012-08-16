using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MerchantTribe.Commerce;
using MerchantTribe.CommerceDTO.v1;
using MerchantTribe.CommerceDTO.v1.Catalog;
using MerchantTribe.Commerce.Catalog;

namespace MerchantTribeStore.api.rest
{
    public class CategoriesHandler : BaseRestHandler
    {
        public CategoriesHandler(MerchantTribe.Commerce.MerchantTribeApplication app)
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
                string byproduct = querystring["byproduct"] ?? string.Empty;
                ApiResponse<List<CategorySnapshotDTO>> response = new ApiResponse<List<CategorySnapshotDTO>>();

                List<CategorySnapshot> results = new List<CategorySnapshot>();

                if (byproduct.Trim().Length > 0)
                {
                    results = MTApp.CatalogServices.FindCategoriesForProduct(byproduct);
                }
                else
                {
                    results = MTApp.CatalogServices.Categories.FindAll();
                }

                List<CategorySnapshotDTO> dto = new List<CategorySnapshotDTO>();

                foreach (CategorySnapshot cat in results)
                {
                    dto.Add(cat.ToDto());
                }
                response.Content = dto;
                data = MerchantTribe.Web.Json.ObjectToJson(response);
            }
            else
            {
                // Find One Specific Category
                ApiResponse<CategoryDTO> response = new ApiResponse<CategoryDTO>();                                
                string bvin = FirstParameter(parameters);
                string byslug = querystring["byslug"] ?? string.Empty;

                Category c = null;

                if (byslug.Trim().Length > 0)
                {
                    c = MTApp.CatalogServices.Categories.FindBySlug(byslug);
                }
                else
                {
                    c = MTApp.CatalogServices.Categories.Find(bvin);
                }

                if (c == null)
                {
                    response.Errors.Add(new ApiError("NULL", "Could not locate that category. Check bvin and try again."));
                }
                else
                {
                    response.Content = c.ToDto();
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
            ApiResponse<CategoryDTO> response = new ApiResponse<CategoryDTO>();

            CategoryDTO postedCategory = null;
            try
            {
                postedCategory = MerchantTribe.Web.Json.ObjectFromJson<CategoryDTO>(postdata);
            }
            catch(Exception ex)
            {
                response.Errors.Add(new ApiError("EXCEPTION", ex.Message));
                return MerchantTribe.Web.Json.ObjectToJson(response);                
            }

            Category c = new Category();
            c.FromDto(postedCategory);

            if (bvin == string.Empty)
            {
                if (MTApp.CatalogServices.Categories.Create(c))
                {
                    bvin = c.Bvin;
                }
            }
            else
            {
                MTApp.CatalogServices.Categories.Update(c);
            }
            Category resultCategory = MTApp.CatalogServices.Categories.Find(bvin);                    
            if (resultCategory != null) response.Content = resultCategory.ToDto();
            
            data = MerchantTribe.Web.Json.ObjectToJson(response);            
            return data;
        }

        public override string DeleteAction(string parameters, System.Collections.Specialized.NameValueCollection querystring, string postdata)
        {
            string data = string.Empty;
            string bvin = FirstParameter(parameters);
            ApiResponse<bool> response = new ApiResponse<bool>();

            if (bvin == string.Empty)
            {
                // Clear Requested
                response.Content = MTApp.DestroyAllCategories();
            }
            else
            {
                // Single Item Delete
                response.Content = MTApp.CatalogServices.Categories.Delete(bvin);
            }

            data = MerchantTribe.Web.Json.ObjectToJson(response);
            return data;
        }
    }
}