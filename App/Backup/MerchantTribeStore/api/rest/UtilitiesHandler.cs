using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MerchantTribe.Commerce;
using MerchantTribe.CommerceDTO.v1;
using MerchantTribe.CommerceDTO.v1.Contacts;
using MerchantTribe.Commerce.Utilities;

namespace MerchantTribeStore.api.rest
{
    public class UtilitiesHandler: BaseRestHandler
    {
        public UtilitiesHandler(MerchantTribe.Commerce.MerchantTribeApplication app)
            : base(app)
        {

        }

        // List or Find Single
        public override string GetAction(string parameters, System.Collections.Specialized.NameValueCollection querystring)
        {            
            string data = string.Empty;                        
            ApiResponse<string> response = new ApiResponse<string>();                        
            data = MerchantTribe.Web.Json.ObjectToJson(response);
            return data;
        }

        // Create or Update
        public override string PostAction(string parameters, System.Collections.Specialized.NameValueCollection querystring, string postdata)
        {
            string data = string.Empty;
            
            string action = FirstParameter(parameters);
            switch (action.ToLowerInvariant().Trim())
            {
                case "slugify":
                    ApiResponse<string> response = new ApiResponse<string>();
                    response.Content = MerchantTribe.Web.Text.Slugify(postdata, true, true);
                    data = MerchantTribe.Web.Json.ObjectToJson(response);
                    break;            
            }
            
            return data;
        }

        public override string DeleteAction(string parameters, System.Collections.Specialized.NameValueCollection querystring, string postdata)
        {
            string data = string.Empty;
            ApiResponse<bool> response = new ApiResponse<bool>();
            response.Content = false;            
            data = MerchantTribe.Web.Json.ObjectToJson(response);
            return data;
        }
    }
}