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
    public class WishListItemsHandler: BaseRestHandler
    {
        public WishListItemsHandler(MerchantTribe.Commerce.MerchantTribeApplication app)
            : base(app)
        {

        }

        // List or Find Single
        public override string GetAction(string parameters, System.Collections.Specialized.NameValueCollection querystring)
        {
            string data = string.Empty;

            // Find One Specific Item
            ApiResponse<WishListItemDTO> response = new ApiResponse<WishListItemDTO>();
            long id = 0;
            long.TryParse(FirstParameter(parameters), out id);
            WishListItem item = MTApp.CatalogServices.WishListItems.Find(id);
            if (item == null)
            {
                response.Errors.Add(new ApiError("NULL", "Could not locate that item. Check id and try again."));
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
            long id = 0;
            long.TryParse(FirstParameter(parameters), out id);
            ApiResponse<WishListItemDTO> response = new ApiResponse<WishListItemDTO>();

            WishListItemDTO postedItem = null;
            try
            {
                postedItem = MerchantTribe.Web.Json.ObjectFromJson<WishListItemDTO>(postdata);
            }
            catch (Exception ex)
            {
                response.Errors.Add(new ApiError("EXCEPTION", ex.Message));
                return MerchantTribe.Web.Json.ObjectToJson(response);
            }

            WishListItem item = new WishListItem();
            item.FromDto(postedItem);

            if (id < 1)
            {
                if (MTApp.CatalogServices.WishListItems.Create(item))
                {
                    id = item.Id;
                }
            }
            else
            {
                MTApp.CatalogServices.WishListItems.Update(item);
            }
            WishListItem resultItem = MTApp.CatalogServices.WishListItems.Find(id);
            if (resultItem != null) response.Content = resultItem.ToDto();

            data = MerchantTribe.Web.Json.ObjectToJson(response);
            return data;
        }

        public override string DeleteAction(string parameters, System.Collections.Specialized.NameValueCollection querystring, string postdata)
        {
            string data = string.Empty;
            long id = 0;
            long.TryParse(FirstParameter(parameters), out id);            
            ApiResponse<bool> response = new ApiResponse<bool>();

            // Single Item Delete
            response.Content = MTApp.CatalogServices.WishListItems.Delete(id);

            data = MerchantTribe.Web.Json.ObjectToJson(response);
            return data;
        }
    }
}