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
    public class AffiliatesHandler: BaseRestHandler
    {
        public AffiliatesHandler(MerchantTribe.Commerce.MerchantTribeApplication app)
            : base(app)
        {

        }

        // List or Find Single
        public override string GetAction(string parameters, System.Collections.Specialized.NameValueCollection querystring)
        {            
            string data = string.Empty;

            if (string.Empty == (parameters ?? string.Empty))
            {
                // List all
                ApiResponse<List<AffiliateDTO>> response = new ApiResponse<List<AffiliateDTO>>();

                List<Affiliate> results = MTApp.ContactServices.Affiliates.FindAllPaged(1, int.MaxValue);
                List<AffiliateDTO> dto = new List<AffiliateDTO>();

                foreach (Affiliate item in results)
                {
                    dto.Add(item.ToDto());
                }
                response.Content = dto;
                data = MerchantTribe.Web.Json.ObjectToJson(response);
            }
            else
            {
                // Find One Specific Category OR List of Referrals
                                              
                string ids = FirstParameter(parameters);
                long id = 0;
                long.TryParse(ids, out id);
                string isReferrals = GetParameterByIndex(1, parameters);

                if (isReferrals.Trim().ToLowerInvariant() == "referrals")
                {
                    // Referrals handler
                    int totalCount = 0;
                    List<AffiliateReferral> refs = MTApp.ContactServices.AffiliateReferrals.FindByCriteria(new AffiliateReferralSearchCriteria(){ AffiliateId=id}, 1, int.MaxValue, ref totalCount);
                    List<AffiliateReferralDTO> resultRefs = new List<AffiliateReferralDTO>();
                    foreach (AffiliateReferral r in refs)
                    {
                        resultRefs.Add(r.ToDto());
                    }
                    ApiResponse<List<AffiliateReferralDTO>> responseA = new ApiResponse<List<AffiliateReferralDTO>>();
                    responseA.Content = resultRefs;
                    data = MerchantTribe.Web.Json.ObjectToJson(responseA);
                }
                else
                {
                    // Affiliate Handler
                    ApiResponse<AffiliateDTO> response = new ApiResponse<AffiliateDTO>();
                    Affiliate item;
                    item = MTApp.ContactServices.Affiliates.Find(id);
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
            string isReferrals = GetParameterByIndex(1, parameters);

            if (isReferrals.Trim().ToLowerInvariant() == "referrals")
            {
                // Create Referral
                ApiResponse<AffiliateReferralDTO> responseA = new ApiResponse<AffiliateReferralDTO>();
                AffiliateReferralDTO postedItemA = null;
                try
                {
                    postedItemA = MerchantTribe.Web.Json.ObjectFromJson<AffiliateReferralDTO>(postdata);
                }
                catch (Exception ex)
                {
                    responseA.Errors.Add(new ApiError("EXCEPTION", ex.Message));
                    return MerchantTribe.Web.Json.ObjectToJson(responseA);
                }
                AffiliateReferral itemA = new AffiliateReferral();
                itemA.FromDto(postedItemA);
                MTApp.ContactServices.AffiliateReferrals.Create2(itemA, true);
                responseA.Content = itemA.ToDto();
                data = MerchantTribe.Web.Json.ObjectToJson(responseA);
            }
            else
            {
                // Create/Update Affiliate
                ApiResponse<AffiliateDTO> responseB = new ApiResponse<AffiliateDTO>();
                AffiliateDTO postedItem = null;
                try
                {
                    postedItem = MerchantTribe.Web.Json.ObjectFromJson<AffiliateDTO>(postdata);
                }
                catch (Exception ex)
                {
                    responseB.Errors.Add(new ApiError("EXCEPTION", ex.Message));
                    return MerchantTribe.Web.Json.ObjectToJson(responseB);
                }

                Affiliate item = new Affiliate();
                item.FromDto(postedItem);

                if (id < 1)
                {
                    Affiliate existing = MTApp.ContactServices.Affiliates.FindByReferralId(item.ReferralId);
                    if (existing == null || existing.Id < 1)
                    {

                        // Create
                        bool result = MTApp.ContactServices.Affiliates.Create(item);
                        responseB.Content = item.ToDto();
                        id = item.Id;                       
                    }
                    else
                    {
                        responseB.Content = existing.ToDto();
                        id = existing.Id;
                    }
                }
                else
                {
                    MTApp.ContactServices.Affiliates.Update(item);
                    id = item.Id;
                    responseB.Content = item.ToDto();                    
                }
                data = MerchantTribe.Web.Json.ObjectToJson(responseB);  
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
            response.Content = MTApp.ContactServices.Affiliates.Delete(id);

            data = MerchantTribe.Web.Json.ObjectToJson(response);
            return data;
        }
    }
}