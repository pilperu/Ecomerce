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
    public class ManufacturersHandler: BaseRestHandler
    {
        public ManufacturersHandler(MerchantTribe.Commerce.MerchantTribeApplication app)
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
                ApiResponse<List<VendorManufacturerDTO>> response = new ApiResponse<List<VendorManufacturerDTO>>();

                List<VendorManufacturer> results = MTApp.ContactServices.Manufacturers.FindAll();
                List<VendorManufacturerDTO> dto = new List<VendorManufacturerDTO>();

                foreach (VendorManufacturer cat in results)
                {
                    dto.Add(cat.ToDto());
                }
                response.Content = dto;
                data = MerchantTribe.Web.Json.ObjectToJson(response);
            }
            else
            {
                // Find One Specific Category
                ApiResponse<VendorManufacturerDTO> response = new ApiResponse<VendorManufacturerDTO>();                                
                string bvin = FirstParameter(parameters);
                VendorManufacturer item = MTApp.ContactServices.Manufacturers.Find(bvin);
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
            ApiResponse<VendorManufacturerDTO> response = new ApiResponse<VendorManufacturerDTO>();

            VendorManufacturerDTO postedItem = null;
            try
            {
                postedItem = MerchantTribe.Web.Json.ObjectFromJson<VendorManufacturerDTO>(postdata);
            }
            catch(Exception ex)
            {
                response.Errors.Add(new ApiError("EXCEPTION", ex.Message));
                return MerchantTribe.Web.Json.ObjectToJson(response);                
            }

            VendorManufacturer item = new VendorManufacturer();
            item.FromDto(postedItem);

            if (bvin == string.Empty)
            {
                if (MTApp.ContactServices.Manufacturers.Create(item))
                {
                    bvin = item.Bvin;
                }
            }
            else
            {
                MTApp.ContactServices.Manufacturers.Update(item);
            }
            VendorManufacturer resultCategory = MTApp.ContactServices.Manufacturers.Find(bvin);                    
            if (resultCategory != null) response.Content = resultCategory.ToDto();
            
            data = MerchantTribe.Web.Json.ObjectToJson(response);            
            return data;
        }

        public override string DeleteAction(string parameters, System.Collections.Specialized.NameValueCollection querystring, string postdata)
        {
            string data = string.Empty;
            string bvin = FirstParameter(parameters);
            ApiResponse<bool> response = new ApiResponse<bool>();

            // Single Item Delete
            response.Content = MTApp.ContactServices.Manufacturers.Delete(bvin);

            data = MerchantTribe.Web.Json.ObjectToJson(response);
            return data;
        }
    }
}