using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MerchantTribe.Commerce;
using MerchantTribe.CommerceDTO.v1;
using MerchantTribe.CommerceDTO.v1.Contacts;
using MerchantTribe.Commerce.Contacts;

namespace MerchantTribeStore.api.rest
{
    public class PriceGroupsHandler: BaseRestHandler
    {
        public PriceGroupsHandler(MerchantTribe.Commerce.MerchantTribeApplication app)
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
                ApiResponse<List<PriceGroupDTO>> response = new ApiResponse<List<PriceGroupDTO>>();

                List<PriceGroup> results = MTApp.ContactServices.PriceGroups.FindAll();
                List<PriceGroupDTO> dto = new List<PriceGroupDTO>();

                foreach (PriceGroup item in results)
                {
                    dto.Add(item.ToDto());
                }
                response.Content = dto;
                data = MerchantTribe.Web.Json.ObjectToJson(response);
            }
            else
            {
                // Find One Specific Category
                ApiResponse<PriceGroupDTO> response = new ApiResponse<PriceGroupDTO>();                                
                string bvin = FirstParameter(parameters);
                PriceGroup item = MTApp.ContactServices.PriceGroups.Find(bvin);
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
            ApiResponse<PriceGroupDTO> response = new ApiResponse<PriceGroupDTO>();

            PriceGroupDTO postedItem = null;
            try
            {
                postedItem = MerchantTribe.Web.Json.ObjectFromJson<PriceGroupDTO>(postdata);
            }
            catch(Exception ex)
            {
                response.Errors.Add(new ApiError("EXCEPTION", ex.Message));
                return MerchantTribe.Web.Json.ObjectToJson(response);                
            }

            PriceGroup item = new PriceGroup();
            item.FromDto(postedItem);

            if (bvin == string.Empty)
            {
                if (MTApp.ContactServices.PriceGroups.Create(item))
                {
                    bvin = item.Bvin;
                }
            }
            else
            {
                MTApp.ContactServices.PriceGroups.Update(item);
            }
            PriceGroup resultItem = MTApp.ContactServices.PriceGroups.Find(bvin);                    
            if (resultItem != null) response.Content = resultItem.ToDto();
            
            data = MerchantTribe.Web.Json.ObjectToJson(response);            
            return data;
        }

        public override string DeleteAction(string parameters, System.Collections.Specialized.NameValueCollection querystring, string postdata)
        {
            string data = string.Empty;
            string bvin = FirstParameter(parameters);
            ApiResponse<bool> response = new ApiResponse<bool>();
            response.Content = MTApp.ContactServices.PriceGroups.Delete(bvin);

            data = MerchantTribe.Web.Json.ObjectToJson(response);
            return data;
        }
    }
}