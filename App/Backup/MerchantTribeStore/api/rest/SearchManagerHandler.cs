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
    public class SearchManagerHandler: BaseRestHandler
    {
        public SearchManagerHandler(MerchantTribe.Commerce.MerchantTribeApplication app)
            : base(app)
        {

        }

        // List or Find Single
        public override string GetAction(string parameters, System.Collections.Specialized.NameValueCollection querystring)
        {            
            ApiResponse<bool> response = new ApiResponse<bool>();
            response.Errors.Add(new ApiError("NOTSUPPORTED", "GET method is not supported for this object."));
            response.Content = false;            
            string data = string.Empty;
            data = MerchantTribe.Web.Json.ObjectToJson(response);            
            return data;
        }

        // Create or Update
        public override string PostAction(string parameters, System.Collections.Specialized.NameValueCollection querystring, string postdata)
        {
            string data = string.Empty;
            string objecttype = FirstParameter(parameters);
            string objectid = GetParameterByIndex(1, parameters);
            ApiResponse<bool> response = new ApiResponse<bool>();
            
            try
            {
                if (objecttype.Trim().ToLowerInvariant() == "products")
                {
                    SearchManager m = new SearchManager();
                    Product p = MTApp.CatalogServices.Products.Find(objectid);
                    if (p != null)
                    {
                        if (p.Bvin.Length > 0)
                        {
                            m.IndexSingleProduct(p);
                            response.Content = true;
                        }
                    }                    
                }                                
            }
            catch(Exception ex)
            {
                response.Errors.Add(new ApiError("EXCEPTION", ex.Message));
                return MerchantTribe.Web.Json.ObjectToJson(response);                
            }
                        
            data = MerchantTribe.Web.Json.ObjectToJson(response);            
            return data;
        }

        public override string DeleteAction(string parameters, System.Collections.Specialized.NameValueCollection querystring, string postdata)
        {
            ApiResponse<bool> response = new ApiResponse<bool>();
            response.Errors.Add(new ApiError("NOTSUPPORTED", "Delete method is not supported for this object."));
            response.Content = false;            
            string data = string.Empty;
            data = MerchantTribe.Web.Json.ObjectToJson(response);            
            return data;
        }
    }
}