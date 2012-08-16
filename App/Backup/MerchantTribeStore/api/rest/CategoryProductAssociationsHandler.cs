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
    public class CategoryProductAssociationsHandler: BaseRestHandler
    {
        public CategoryProductAssociationsHandler(MerchantTribe.Commerce.MerchantTribeApplication app)
            : base(app)
        {

        }

        // List or Find Single
        public override string GetAction(string parameters, System.Collections.Specialized.NameValueCollection querystring)
        {
            string data = string.Empty;


            // Find One Specific Category
            ApiResponse<CategoryProductAssociationDTO> response = new ApiResponse<CategoryProductAssociationDTO>();
            string ids = FirstParameter(parameters);
            long id = 0;
            long.TryParse(ids, out id);

            var item = MTApp.CatalogServices.CategoriesXProducts.Find(id);
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
            string ids = FirstParameter(parameters);
            long id = 0;
            long.TryParse(ids, out id);

            ApiResponse<CategoryProductAssociationDTO> response = new ApiResponse<CategoryProductAssociationDTO>();

            CategoryProductAssociationDTO postedItem = null;
            try
            {
                postedItem = MerchantTribe.Web.Json.ObjectFromJson<CategoryProductAssociationDTO>(postdata);
            }
            catch (Exception ex)
            {
                response.Errors.Add(new ApiError("EXCEPTION", ex.Message));
                return MerchantTribe.Web.Json.ObjectToJson(response);
            }

            CategoryProductAssociation item = new CategoryProductAssociation();
            item.FromDto(postedItem);

            if (id < 1)
            {
                MTApp.CatalogServices.CategoriesXProducts.AddProductToCategory(item.ProductId, item.CategoryId);
                //if (MTApp.CatalogServices.CategoriesXProducts.Create(item))
                //{
                //    id = item.Id;
                //}
            }
            else
            {
                MTApp.CatalogServices.CategoriesXProducts.Update(item);
            }
            CategoryProductAssociation resultItem = MTApp.CatalogServices.CategoriesXProducts.Find(id);
            if (resultItem != null) response.Content = resultItem.ToDto();

            data = MerchantTribe.Web.Json.ObjectToJson(response);
            return data;
        }

        public override string DeleteAction(string parameters, System.Collections.Specialized.NameValueCollection querystring, string postdata)
        {
            string data = string.Empty;
            string bvin = FirstParameter(parameters);
            string categoryBvin = GetParameterByIndex(1, parameters);
            ApiResponse<bool> response = new ApiResponse<bool>();

            // Single Item Delete
            response.Content = MTApp.CatalogServices.CategoriesXProducts.RemoveProductFromCategory(bvin, categoryBvin);

            data = MerchantTribe.Web.Json.ObjectToJson(response);
            return data;
        }
    }
}