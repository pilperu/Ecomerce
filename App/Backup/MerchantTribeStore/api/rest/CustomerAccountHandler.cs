using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;
using MerchantTribe.Commerce;
using MerchantTribe.CommerceDTO.v1;
using MerchantTribe.CommerceDTO.v1.Membership;
using MerchantTribe.CommerceDTO.v1.Contacts;
using MerchantTribe.Commerce.Membership;
using MerchantTribe.Commerce.Contacts;

namespace MerchantTribeStore.api.rest
{
    public class CustomerAccountHandler: BaseRestHandler
    {
        public CustomerAccountHandler(MerchantTribe.Commerce.MerchantTribeApplication app)
            : base(app)
        {

        }

        // List or Find Single
        public override string GetAction(string parameters, System.Collections.Specialized.NameValueCollection querystring)
        {            
            string data = string.Empty;
            string email = querystring["email"];

            if ((string.Empty == (parameters ?? string.Empty)) && (email.Trim().Length < 1))
            {
                string countonly = querystring["countonly"] ?? string.Empty;
                string page = querystring["page"] ?? "1";
                int pageInt = 1;
                int.TryParse(page, out pageInt);
                string pageSize = querystring["pagesize"] ?? "9";
                int pageSizeInt = 9;
                int.TryParse(pageSize, out pageSizeInt);
                
                if (querystring["countonly"] != null)
                {
                    // List by Page
                    ApiResponse<long> response = new ApiResponse<long>();
                    int results = MTApp.MembershipServices.Customers.CountOfAll();                    
                    response.Content = (long)results;
                    data = MerchantTribe.Web.Json.ObjectToJson(response);
                }
                else if (querystring["page"] != null)
                {
                    // List by Page
                    ApiResponse<List<CustomerAccountDTO>> response = new ApiResponse<List<CustomerAccountDTO>>();
                    List<CustomerAccount> results = MTApp.MembershipServices.Customers.FindAllPaged(pageInt, pageSizeInt);
                    List<CustomerAccountDTO> dto = new List<CustomerAccountDTO>();
                    foreach (CustomerAccount item in results)
                    {
                        dto.Add(item.ToDto());
                    }
                    response.Content = dto;
                    data = MerchantTribe.Web.Json.ObjectToJson(response);
                }
                else
                {
                    // List
                    ApiResponse<List<CustomerAccountDTO>> response = new ApiResponse<List<CustomerAccountDTO>>();
                    List<CustomerAccount> results = MTApp.MembershipServices.Customers.FindAll();
                    List<CustomerAccountDTO> dto = new List<CustomerAccountDTO>();

                    foreach (CustomerAccount item in results)
                    {
                        dto.Add(item.ToDto());
                    }
                    response.Content = dto;
                    data = MerchantTribe.Web.Json.ObjectToJson(response);
                }
            }
            else
            {
                // Find One Specific Category
                ApiResponse<CustomerAccountDTO> response = new ApiResponse<CustomerAccountDTO>();                                
                string bvin = FirstParameter(parameters);

                CustomerAccount item;
                if (email.Trim().Length > 0)
                {
                    item = MTApp.MembershipServices.Customers.FindByEmail(email);
                }
                else
                {
                    item = MTApp.MembershipServices.Customers.Find(bvin);
                }
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
            ApiResponse<CustomerAccountDTO> response = new ApiResponse<CustomerAccountDTO>();

            CustomerAccountDTO postedItem = null;
            try
            {
                postedItem = MerchantTribe.Web.Json.ObjectFromJson<CustomerAccountDTO>(postdata);
            }
            catch(Exception ex)
            {
                response.Errors.Add(new ApiError("EXCEPTION", ex.Message));
                return MerchantTribe.Web.Json.ObjectToJson(response);                
            }

            CustomerAccount item = new CustomerAccount();
            item.FromDto(postedItem);

            if (bvin == string.Empty)
            {
                CustomerAccount existing = MTApp.MembershipServices.Customers.FindByEmail(item.Email);
                if (existing == null || existing.Bvin == string.Empty)
                {
                    string clearPassword = querystring["pwd"];
                    if (clearPassword.Trim().Length < 1)
                    {
                        clearPassword = MerchantTribe.Web.PasswordGenerator.GeneratePassword(10);
                    }
                    // Create
                    bool result = MTApp.MembershipServices.CreateCustomer(item, clearPassword);
                    bvin = item.Bvin;
                }
                else
                {
                    bvin = existing.Bvin;
                }
            }
            else
            {
                MTApp.MembershipServices.UpdateCustomer(item);
            }
            CustomerAccount resultItem = MTApp.MembershipServices.Customers.Find(bvin);
            if (resultItem != null)
            {
                response.Content = resultItem.ToDto();
                // Address Import
                foreach (AddressDTO a in postedItem.Addresses)
                {
                    Address addr = new Address();
                    addr.FromDto(a);
                    MTApp.MembershipServices.CheckIfNewAddressAndAddWithUpdate(resultItem,addr);
                }
            }
            
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
                // Clear All Requested
                response.Content = MTApp.MembershipServices.DestroyAllCustomers(MTApp);
            }
            else
            {
                // Delete Single Customer
                response.Content = MTApp.DestroyCustomerAccount(bvin);
            }

            data = MerchantTribe.Web.Json.ObjectToJson(response);
            return data;
        }
    }
}