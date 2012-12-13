using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MerchantTribe.CommerceDTO.v1.Catalog;
using MerchantTribe.CommerceDTO.v1.Contacts;
using MerchantTribe.CommerceDTO.v1.Content;
using MerchantTribe.CommerceDTO.v1.Marketing;
using MerchantTribe.CommerceDTO.v1.Membership;
using MerchantTribe.CommerceDTO.v1.Orders;
using MerchantTribe.CommerceDTO.v1.Shipping;
using MerchantTribe.CommerceDTO.v1.Taxes;

namespace MerchantTribe.CommerceDTO.v1.Client
{
    public class Api
    {
        private string Enc(string input)
        {
            // The web request class actuall does encoding for us
            // so we only need to encode the "/" character
            return input.Replace("/", "%2F");
            //return System.Web.HttpUtility.UrlEncode(input);
        }

        private string baseUri = "http://localhost";
        private string key = string.Empty;
        private string fullApiUri = "http://localhost/api/rest/v1/";

        public Api(string baseWebSiteUri, string apiKey)
        {
            this.baseUri = baseWebSiteUri;
            if (!this.baseUri.EndsWith("/"))
            {
                this.baseUri += "/";
            }
            this.fullApiUri = System.IO.Path.Combine(this.baseUri, "api/rest/v1/");
            this.key = apiKey;
        }

        // Categories
        public ApiResponse<List<Catalog.CategorySnapshotDTO>> CategoriesFindAll()
        {
            ApiResponse<List<Catalog.CategorySnapshotDTO>> result = new ApiResponse<List<Catalog.CategorySnapshotDTO>>();
            result = RestHelper.GetRequest<ApiResponse<List<Catalog.CategorySnapshotDTO>>>(this.fullApiUri + "categories/?key=" + Enc(key));
            return result;
        }
        public ApiResponse<List<Catalog.CategorySnapshotDTO>> CategoriesFindForProduct(string productBvin)
        {
            ApiResponse<List<Catalog.CategorySnapshotDTO>> result = new ApiResponse<List<Catalog.CategorySnapshotDTO>>();
            result = RestHelper.GetRequest<ApiResponse<List<Catalog.CategorySnapshotDTO>>>(this.fullApiUri + "categories/?key=" + Enc(key) + "&byproduct=" + Enc(productBvin));
            return result;
        }
        public ApiResponse<Catalog.CategoryDTO> CategoriesFind(string bvin)
        {
            ApiResponse<Catalog.CategoryDTO> result = new ApiResponse<Catalog.CategoryDTO>();
            result = RestHelper.GetRequest<ApiResponse<Catalog.CategoryDTO>>(this.fullApiUri + "categories/" + Enc(bvin) + "?key=" + Enc(key));
            return result;
        }
        public ApiResponse<Catalog.CategoryDTO> CategoriesFindBySlug(string slug)
        {
            ApiResponse<Catalog.CategoryDTO> result = new ApiResponse<Catalog.CategoryDTO>();
            result = RestHelper.GetRequest<ApiResponse<Catalog.CategoryDTO>>(this.fullApiUri + "categories/ANY" + "?key=" + Enc(key) + "&byslug=" + Enc(slug));
            return result;
        }
        public ApiResponse<Catalog.CategoryDTO> CategoriesCreate(Catalog.CategoryDTO item)
        {
            ApiResponse<Catalog.CategoryDTO> result = new ApiResponse<Catalog.CategoryDTO>();
            result = RestHelper.PostRequest<ApiResponse<Catalog.CategoryDTO>>(this.fullApiUri + "categories/?key=" + Enc(key), MerchantTribe.Web.Json.ObjectToJson(item));
            return result;
        }
        public ApiResponse<Catalog.CategoryDTO> CategoriesUpdate(Catalog.CategoryDTO item)
        {
            ApiResponse<Catalog.CategoryDTO> result = new ApiResponse<Catalog.CategoryDTO>();
            result = RestHelper.PostRequest<ApiResponse<Catalog.CategoryDTO>>( this.fullApiUri + "categories/" + Enc(item.Bvin) + "?key=" + Enc(key), MerchantTribe.Web.Json.ObjectToJson(item));
            return result;
        }
        public ApiResponse<bool> CategoriesDelete(string bvin)
        {
            ApiResponse<bool> result = new ApiResponse<bool>();
            result = RestHelper.DeleteRequest<ApiResponse<bool>>(this.fullApiUri + "categories/" + Enc(bvin) + "?key=" + Enc(key), string.Empty);
            return result;
        }
        public ApiResponse<bool> CategoriesClearAll()
        {
            ApiResponse<bool> result = new ApiResponse<bool>();
            result = RestHelper.DeleteRequest<ApiResponse<bool>>(this.fullApiUri + "categories/?key=" + Enc(key), string.Empty);
            return result;
        }

        // Price Groups
        public ApiResponse<List<PriceGroupDTO>> PriceGroupsFindAll()
        {
            ApiResponse<List<PriceGroupDTO>> result = new ApiResponse<List<PriceGroupDTO>>();
            result = RestHelper.GetRequest <ApiResponse<List<PriceGroupDTO>>>(this.fullApiUri + "pricegroups/?key=" + Enc(key));
            return result;
        }
        public ApiResponse<PriceGroupDTO> PriceGroupsFind(string bvin)
        {
            ApiResponse<PriceGroupDTO> result = new ApiResponse<PriceGroupDTO>();
            result = RestHelper.GetRequest<ApiResponse<PriceGroupDTO>>(this.fullApiUri + "pricegroups/" + Enc(bvin) + "?key=" + Enc(key));
            return result;
        }
        public ApiResponse<PriceGroupDTO> PriceGroupsCreate(PriceGroupDTO item)
        {
            ApiResponse<PriceGroupDTO> result = new ApiResponse<PriceGroupDTO>();
            result = RestHelper.PostRequest<ApiResponse<PriceGroupDTO>>(this.fullApiUri + "pricegroups/?key=" + Enc(key), MerchantTribe.Web.Json.ObjectToJson(item));
            return result;
        }        
        public ApiResponse<PriceGroupDTO> PriceGroupsUpdate(PriceGroupDTO item)
        {
            ApiResponse<PriceGroupDTO> result = new ApiResponse<PriceGroupDTO>();
            result = RestHelper.PostRequest<ApiResponse<PriceGroupDTO>>(this.fullApiUri + "pricegroups/" + Enc(item.Bvin) + "?key=" + Enc(key), MerchantTribe.Web.Json.ObjectToJson(item));
            return result;
        }
        public ApiResponse<bool> PriceGroupsDelete(string bvin)
        {
            ApiResponse<bool> result = new ApiResponse<bool>();
            result = RestHelper.DeleteRequest<ApiResponse<bool>>(this.fullApiUri + "pricegroups/" + Enc(bvin) + "?key=" + Enc(key), string.Empty);
            return result;
        }

        // Customer Accounts
        public ApiResponse<List<CustomerAccountDTO>> CustomerAccountsFindAll()
        {
            ApiResponse<List<CustomerAccountDTO>> result = new ApiResponse<List<CustomerAccountDTO>>();
            result = RestHelper.GetRequest<ApiResponse<List<CustomerAccountDTO>>>(this.fullApiUri + "customeraccounts/?key=" + Enc(key));
            return result;
        }
        public ApiResponse<long> CustomerAccountsCountOfAll()
        {
            ApiResponse<long> result = new ApiResponse<long>();
            result = RestHelper.GetRequest<ApiResponse<long>>(this.fullApiUri + "customeraccounts/?key=" + Enc(key) + "&countonly=1");
            return result;
        }
        public ApiResponse<List<CustomerAccountDTO>> CustomerAccountsFindAllByPage(int pageNumber, int pageSize)
        {
            ApiResponse<List<CustomerAccountDTO>> result = new ApiResponse<List<CustomerAccountDTO>>();
            result = RestHelper.GetRequest<ApiResponse<List<CustomerAccountDTO>>>(this.fullApiUri + "customeraccounts/?key=" + Enc(key) + "&page=" + pageNumber + "&pagesize=" + pageSize);
            return result;
        }

        public ApiResponse<CustomerAccountDTO> CustomerAccountsFind(string bvin)
        {
            ApiResponse<CustomerAccountDTO> result = new ApiResponse<CustomerAccountDTO>();
            result = RestHelper.GetRequest<ApiResponse<CustomerAccountDTO>>(this.fullApiUri + "customeraccounts/" + Enc(bvin) + "?key=" + Enc(key));
            return result;
        }
        public ApiResponse<CustomerAccountDTO> CustomerAccountsFindByEmail(string email)
        {
            ApiResponse<CustomerAccountDTO> result = new ApiResponse<CustomerAccountDTO>();

            ApiResponse<List<CustomerAccountDTO>> fullResult = RestHelper.GetRequest<ApiResponse<List<CustomerAccountDTO>>>(this.fullApiUri + "customeraccounts/?key=" + Enc(key) + "&email=" + Enc(email));
            foreach (ApiError err in fullResult.Errors)
            {
                result.Errors.Add(err);
            }
            if (fullResult.Content != null)
            {
                if (fullResult.Content.Count > 0)
                {
                    result.Content = fullResult.Content[0];
                }
            }
            return result;
        }
        public ApiResponse<CustomerAccountDTO> CustomerAccountsCreate(CustomerAccountDTO item)
        {
            ApiResponse<CustomerAccountDTO> result = new ApiResponse<CustomerAccountDTO>();
            result = RestHelper.PostRequest<ApiResponse<CustomerAccountDTO>>(this.fullApiUri + "customeraccounts/?key=" + Enc(key), MerchantTribe.Web.Json.ObjectToJson(item));
            return result;
        }
        public ApiResponse<CustomerAccountDTO> CustomerAccountsUpdate(CustomerAccountDTO item)
        {
            ApiResponse<CustomerAccountDTO> result = new ApiResponse<CustomerAccountDTO>();
            result = RestHelper.PostRequest<ApiResponse<CustomerAccountDTO>>(this.fullApiUri + "customeraccounts/" + Enc(item.Bvin) + "?key=" + Enc(key), MerchantTribe.Web.Json.ObjectToJson(item));
            return result;
        }
        public ApiResponse<CustomerAccountDTO> CustomerAccountsCreateWithPassword(CustomerAccountDTO item, string clearpassword)
        {
            ApiResponse<CustomerAccountDTO> result = new ApiResponse<CustomerAccountDTO>();
            result = RestHelper.PostRequest<ApiResponse<CustomerAccountDTO>>(this.fullApiUri + "customeraccounts/?key=" + Enc(key) + "&pwd=" + Enc(clearpassword), MerchantTribe.Web.Json.ObjectToJson(item));
            return result;
        }
        public ApiResponse<bool> CustomerAccountsDelete(string bvin)
        {
            ApiResponse<bool> result = new ApiResponse<bool>();
            result = RestHelper.DeleteRequest<ApiResponse<bool>>(this.fullApiUri + "customeraccounts/" + Enc(bvin) + "?key=" + Enc(key), string.Empty);
            return result;
        }
        public ApiResponse<bool> CustomerAccountsClearAll()
        {
            ApiResponse<bool> result = new ApiResponse<bool>();
            result = RestHelper.DeleteRequest<ApiResponse<bool>>(this.fullApiUri + "customeraccounts/?key=" + Enc(key), string.Empty);
            return result;
        }

        // Affiliates
        public ApiResponse<List<AffiliateDTO>> AffiliatesFindAll()
        {
            ApiResponse<List<AffiliateDTO>> result = new ApiResponse<List<AffiliateDTO>>();
            result = RestHelper.GetRequest<ApiResponse<List<AffiliateDTO>>>(this.fullApiUri + "affiliates/?key=" + Enc(key));
            return result;
        }
        public ApiResponse<AffiliateDTO> AffiliatesFind(long id)
        {
            ApiResponse<AffiliateDTO> result = new ApiResponse<AffiliateDTO>();
            result = RestHelper.GetRequest<ApiResponse<AffiliateDTO>>(this.fullApiUri + "affiliates/" + id + "?key=" + Enc(key));
            return result;
        }
        public ApiResponse<AffiliateDTO> AffiliatesCreate(AffiliateDTO item)
        {
            ApiResponse<AffiliateDTO> result = new ApiResponse<AffiliateDTO>();
            result = RestHelper.PostRequest<ApiResponse<AffiliateDTO>>(this.fullApiUri + "affiliates/?key=" + Enc(key), MerchantTribe.Web.Json.ObjectToJson(item));
            return result;
        }
        public ApiResponse<AffiliateDTO> AffiliatesUpdate(AffiliateDTO item)
        {
            ApiResponse<AffiliateDTO> result = new ApiResponse<AffiliateDTO>();
            result = RestHelper.PostRequest<ApiResponse<AffiliateDTO>>(this.fullApiUri + "affiliates/" + item.Id + "?key=" + Enc(key), MerchantTribe.Web.Json.ObjectToJson(item));
            return result;
        }
        public ApiResponse<bool> AffiliatesDelete(long id)
        {
            ApiResponse<bool> result = new ApiResponse<bool>();
            result = RestHelper.DeleteRequest<ApiResponse<bool>>(this.fullApiUri + "affiliates/" + id + "?key=" + Enc(key), string.Empty);
            return result;
        }

        // AffiliateReferrals
        public ApiResponse<List<AffiliateReferralDTO>> AffiliateReferralsFindForAffiliate(long id)
        {
            ApiResponse<List<AffiliateReferralDTO>> result = new ApiResponse<List<AffiliateReferralDTO>>();
            result = RestHelper.GetRequest<ApiResponse<List<AffiliateReferralDTO>>>(this.fullApiUri + "affiliates/" + id + "/referrals?key=" + Enc(key));
            return result;
        }
        public ApiResponse<AffiliateReferralDTO> AffiliateReferralsCreate(AffiliateReferralDTO item)
        {
            ApiResponse<AffiliateReferralDTO> result = new ApiResponse<AffiliateReferralDTO>();
            result = RestHelper.PostRequest<ApiResponse<AffiliateReferralDTO>>(this.fullApiUri + "affiliates/" + item.AffiliateId + "/referrals?key=" + Enc(key), MerchantTribe.Web.Json.ObjectToJson(item));
            return result;
        }
 
        // Tax Schedules
        public ApiResponse<List<TaxScheduleDTO>> TaxSchedulesFindAll()
        {
            ApiResponse<List<TaxScheduleDTO>> result = new ApiResponse<List<TaxScheduleDTO>>();
            result = RestHelper.GetRequest<ApiResponse<List<TaxScheduleDTO>>>(this.fullApiUri + "taxschedules/?key=" + Enc(key));
            return result;
        }
        public ApiResponse<TaxScheduleDTO> TaxSchedulesFind(long id)
        {
            ApiResponse<TaxScheduleDTO> result = new ApiResponse<TaxScheduleDTO>();
            result = RestHelper.GetRequest<ApiResponse<TaxScheduleDTO>>(this.fullApiUri + "taxschedules/" + id + "?key=" + Enc(key));
            return result;
        }
        public ApiResponse<TaxScheduleDTO> TaxSchedulesCreate(TaxScheduleDTO item)
        {
            ApiResponse<TaxScheduleDTO> result = new ApiResponse<TaxScheduleDTO>();
            result = RestHelper.PostRequest<ApiResponse<TaxScheduleDTO>>(this.fullApiUri + "taxschedules/?key=" + Enc(key), MerchantTribe.Web.Json.ObjectToJson(item));
            return result;
        }
        public ApiResponse<TaxScheduleDTO> TaxSchedulesUpdate(TaxScheduleDTO item)
        {
            ApiResponse<TaxScheduleDTO> result = new ApiResponse<TaxScheduleDTO>();
            result = RestHelper.PostRequest<ApiResponse<TaxScheduleDTO>>(this.fullApiUri + "taxschedules/" + item.Id + "?key=" + Enc(key), MerchantTribe.Web.Json.ObjectToJson(item));
            return result;
        }
        public ApiResponse<bool> TaxSchedulesDelete(long id)
        {
            ApiResponse<bool> result = new ApiResponse<bool>();
            result = RestHelper.DeleteRequest<ApiResponse<bool>>(this.fullApiUri + "taxschedules/" + id + "?key=" + Enc(key), string.Empty);
            return result;
        }

        // Taxes
        public ApiResponse<List<TaxDTO>> TaxesFindAll()
        {
            ApiResponse<List<TaxDTO>> result = new ApiResponse<List<TaxDTO>>();
            result = RestHelper.GetRequest<ApiResponse<List<TaxDTO>>>(this.fullApiUri + "taxes/?key=" + Enc(key));
            return result;
        }
        public ApiResponse<TaxDTO> TaxesFind(long id)
        {
            ApiResponse<TaxDTO> result = new ApiResponse<TaxDTO>();
            result = RestHelper.GetRequest<ApiResponse<TaxDTO>>(this.fullApiUri + "taxes/" + id + "?key=" + Enc(key));
            return result;
        }
        public ApiResponse<TaxDTO> TaxesCreate(TaxDTO item)
        {
            ApiResponse<TaxDTO> result = new ApiResponse<TaxDTO>();
            result = RestHelper.PostRequest<ApiResponse<TaxDTO>>(this.fullApiUri + "taxes/?key=" + Enc(key), MerchantTribe.Web.Json.ObjectToJson(item));
            return result;
        }
        public ApiResponse<TaxDTO> TaxesUpdate(TaxDTO item)
        {
            ApiResponse<TaxDTO> result = new ApiResponse<TaxDTO>();
            result = RestHelper.PostRequest<ApiResponse<TaxDTO>>(this.fullApiUri + "taxes/" + item.Id + "?key=" + Enc(key), MerchantTribe.Web.Json.ObjectToJson(item));
            return result;
        }
        public ApiResponse<bool> TaxesDelete(long id)
        {
            ApiResponse<bool> result = new ApiResponse<bool>();
            result = RestHelper.DeleteRequest<ApiResponse<bool>>(this.fullApiUri + "taxes/" + id + "?key=" + Enc(key), string.Empty);
            return result;
        }

        // Vendors
        public ApiResponse<List<VendorManufacturerDTO>> VendorFindAll()
        {
            ApiResponse<List<VendorManufacturerDTO>> result = new ApiResponse<List<VendorManufacturerDTO>>();
            result = RestHelper.GetRequest<ApiResponse<List<VendorManufacturerDTO>>>(this.fullApiUri + "vendors/?key=" + Enc(key));
            return result;
        }
        public ApiResponse<VendorManufacturerDTO> VendorFind(string bvin)
        {
            ApiResponse<VendorManufacturerDTO> result = new ApiResponse<VendorManufacturerDTO>();
            result = RestHelper.GetRequest<ApiResponse<VendorManufacturerDTO>>(this.fullApiUri + "vendors/" + Enc(bvin) + "?key=" + Enc(key));
            return result;
        }
        public ApiResponse<VendorManufacturerDTO> VendorCreate(VendorManufacturerDTO item)
        {
            ApiResponse<VendorManufacturerDTO> result = new ApiResponse<VendorManufacturerDTO>();
            result = RestHelper.PostRequest<ApiResponse<VendorManufacturerDTO>>(this.fullApiUri + "vendors/?key=" + Enc(key), MerchantTribe.Web.Json.ObjectToJson(item));
            return result;
        }
        public ApiResponse<VendorManufacturerDTO> VendorUpdate(VendorManufacturerDTO item)
        {
            ApiResponse<VendorManufacturerDTO> result = new ApiResponse<VendorManufacturerDTO>();
            result = RestHelper.PostRequest<ApiResponse<VendorManufacturerDTO>>(this.fullApiUri + "vendors/" + Enc(item.Bvin) + "?key=" + Enc(key), MerchantTribe.Web.Json.ObjectToJson(item));
            return result;
        }
        public ApiResponse<bool> VendorDelete(string bvin)
        {
            ApiResponse<bool> result = new ApiResponse<bool>();
            result = RestHelper.DeleteRequest<ApiResponse<bool>>(this.fullApiUri + "vendors/" + Enc(bvin) + "?key=" + Enc(key), string.Empty);
            return result;
        }

        // Manufacturers
        public ApiResponse<List<VendorManufacturerDTO>> ManufacturerFindAll()
        {
            ApiResponse<List<VendorManufacturerDTO>> result = new ApiResponse<List<VendorManufacturerDTO>>();
            result = RestHelper.GetRequest<ApiResponse<List<VendorManufacturerDTO>>>(this.fullApiUri + "manufacturers/?key=" + Enc(key));
            return result;
        }
        public ApiResponse<VendorManufacturerDTO> ManufacturerFind(string bvin)
        {
            ApiResponse<VendorManufacturerDTO> result = new ApiResponse<VendorManufacturerDTO>();
            result = RestHelper.GetRequest<ApiResponse<VendorManufacturerDTO>>(this.fullApiUri + "manufacturers/" + Enc(bvin) + "?key=" + Enc(key));
            return result;
        }
        public ApiResponse<VendorManufacturerDTO> ManufacturerCreate(VendorManufacturerDTO item)
        {
            ApiResponse<VendorManufacturerDTO> result = new ApiResponse<VendorManufacturerDTO>();
            result = RestHelper.PostRequest<ApiResponse<VendorManufacturerDTO>>(this.fullApiUri + "manufacturers/?key=" + Enc(key), MerchantTribe.Web.Json.ObjectToJson(item));
            return result;
        }
        public ApiResponse<VendorManufacturerDTO> ManufacturerUpdate(VendorManufacturerDTO item)
        {
            ApiResponse<VendorManufacturerDTO> result = new ApiResponse<VendorManufacturerDTO>();
            result = RestHelper.PostRequest<ApiResponse<VendorManufacturerDTO>>(this.fullApiUri + "manufacturers/" + Enc(item.Bvin) + "?key=" + Enc(key), MerchantTribe.Web.Json.ObjectToJson(item));
            return result;
        }
        public ApiResponse<bool> ManufacturerDelete(string bvin)
        {
            ApiResponse<bool> result = new ApiResponse<bool>();
            result = RestHelper.DeleteRequest<ApiResponse<bool>>(this.fullApiUri + "manufacturers/" + Enc(bvin) + "?key=" + Enc(key), string.Empty);
            return result;
        }

        // ProductTypes
        public ApiResponse<List<ProductTypeDTO>> ProductTypesFindAll()
        {
            ApiResponse<List<ProductTypeDTO>> result = new ApiResponse<List<ProductTypeDTO>>();
            result = RestHelper.GetRequest<ApiResponse<List<ProductTypeDTO>>>(this.fullApiUri + "producttypes/?key=" + Enc(key));
            return result;
        }
        public ApiResponse<ProductTypeDTO> ProductTypesFind(string bvin)
        {
            ApiResponse<ProductTypeDTO> result = new ApiResponse<ProductTypeDTO>();
            result = RestHelper.GetRequest<ApiResponse<ProductTypeDTO>>(this.fullApiUri + "producttypes/" + Enc(bvin) + "?key=" + Enc(key));
            return result;
        }
        public ApiResponse<ProductTypeDTO> ProductTypesCreate(ProductTypeDTO item)
        {
            ApiResponse<ProductTypeDTO> result = new ApiResponse<ProductTypeDTO>();
            result = RestHelper.PostRequest<ApiResponse<ProductTypeDTO>>(this.fullApiUri + "producttypes/?key=" + Enc(key), MerchantTribe.Web.Json.ObjectToJson(item));
            return result;
        }
        public ApiResponse<ProductTypeDTO> ProductTypesUpdate(ProductTypeDTO item)
        {
            ApiResponse<ProductTypeDTO> result = new ApiResponse<ProductTypeDTO>();
            result = RestHelper.PostRequest<ApiResponse<ProductTypeDTO>>(this.fullApiUri + "producttypes/" + Enc(item.Bvin) + "?key=" + Enc(key), MerchantTribe.Web.Json.ObjectToJson(item));
            return result;
        }
        public ApiResponse<bool> ProductTypesDelete(string bvin)
        {
            ApiResponse<bool> result = new ApiResponse<bool>();
            result = RestHelper.DeleteRequest<ApiResponse<bool>>(this.fullApiUri + "producttypes/" + Enc(bvin) + "?key=" + Enc(key), string.Empty);
            return result;
        }
        public ApiResponse<bool> ProductTypesAddProperty(string typeBvin, long propertyId, int sortOrder)
        {
            ApiResponse<bool> result = new ApiResponse<bool>();
            result = RestHelper.PostRequest<ApiResponse<bool>>(this.fullApiUri + "producttypes/" + Enc(typeBvin) + "/properties/" + propertyId + "/" + sortOrder + "?key=" + Enc(key), string.Empty);
            return result;
        }
        public ApiResponse<bool> ProductTypesRemoveProperty(string typeBvin, long propertyId)
        {
            ApiResponse<bool> result = new ApiResponse<bool>();
            result = RestHelper.DeleteRequest<ApiResponse<bool>>(this.fullApiUri + "producttypes/" + Enc(typeBvin) + "/properties/" + propertyId + "?key=" + Enc(key), string.Empty);
            return result;
        }

        // Product Properties
        public ApiResponse<List<ProductPropertyDTO>> ProductPropertiesFindAll()
        {
            ApiResponse<List<ProductPropertyDTO>> result = new ApiResponse<List<ProductPropertyDTO>>();
            result = RestHelper.GetRequest<ApiResponse<List<ProductPropertyDTO>>>(this.fullApiUri + "productproperties/?key=" + Enc(key));
            return result;
        }
        public ApiResponse<ProductPropertyDTO> ProductPropertiesFind(long id)
        {
            ApiResponse<ProductPropertyDTO> result = new ApiResponse<ProductPropertyDTO>();
            result = RestHelper.GetRequest<ApiResponse<ProductPropertyDTO>>(this.fullApiUri + "productproperties/" + id + "?key=" + Enc(key));
            return result;
        }
        public ApiResponse<ProductPropertyDTO> ProductPropertiesCreate(ProductPropertyDTO item)
        {
            ApiResponse<ProductPropertyDTO> result = new ApiResponse<ProductPropertyDTO>();
            result = RestHelper.PostRequest<ApiResponse<ProductPropertyDTO>>(this.fullApiUri + "productproperties/?key=" + Enc(key), MerchantTribe.Web.Json.ObjectToJson(item));
            return result;
        }
        public ApiResponse<ProductPropertyDTO> ProductPropertiesUpdate(ProductPropertyDTO item)
        {
            ApiResponse<ProductPropertyDTO> result = new ApiResponse<ProductPropertyDTO>();
            result = RestHelper.PostRequest<ApiResponse<ProductPropertyDTO>>(this.fullApiUri + "productproperties/" + item.Id + "?key=" + Enc(key), MerchantTribe.Web.Json.ObjectToJson(item));
            return result;
        }
        public ApiResponse<bool> ProductPropertiesDelete(long id)
        {
            ApiResponse<bool> result = new ApiResponse<bool>();
            result = RestHelper.DeleteRequest<ApiResponse<bool>>(this.fullApiUri + "productproperties/" + id + "?key=" + Enc(key), string.Empty);
            return result;
        }
        public ApiResponse<bool> ProductPropertiesSetValueForProduct(long id, string productBvin, string propertyValue, long choiceId)
        {
            ApiResponse<bool> result = new ApiResponse<bool>();
            result = RestHelper.PostRequest<ApiResponse<bool>>(this.fullApiUri + "productproperties/" + id + "/valuesforproduct/" + Enc(productBvin) + "/?key=" + Enc(key), propertyValue);
            return result;
        }
            
        // Product Options
        public ApiResponse<List<OptionDTO>> ProductOptionsFindAll()
        {
            ApiResponse<List<OptionDTO>> result = new ApiResponse<List<OptionDTO>>();
            result = RestHelper.GetRequest<ApiResponse<List<OptionDTO>>>(this.fullApiUri + "productoptions/?key=" + Enc(key));
            return result;
        }
        public ApiResponse<List<OptionDTO>> ProductOptionsFindAllByProductId(string productBvin)
        {
            ApiResponse<List<OptionDTO>> result = new ApiResponse<List<OptionDTO>>();
            result = RestHelper.GetRequest<ApiResponse<List<OptionDTO>>>(this.fullApiUri + "productoptions/?key=" + Enc(key) + "&productbvin=" + productBvin);
            return result;
        }
        public ApiResponse<OptionDTO> ProductOptionsFind(string bvin)
        {
            ApiResponse<OptionDTO> result = new ApiResponse<OptionDTO>();
            result = RestHelper.GetRequest<ApiResponse<OptionDTO>>(this.fullApiUri + "productoptions/" + Enc(bvin) + "?key=" + Enc(key));
            return result;
        }
        public ApiResponse<OptionDTO> ProductOptionsCreate(OptionDTO item)
        {
            ApiResponse<OptionDTO> result = new ApiResponse<OptionDTO>();
            result = RestHelper.PostRequest<ApiResponse<OptionDTO>>(this.fullApiUri + "productoptions/?key=" + Enc(key), MerchantTribe.Web.Json.ObjectToJson(item));
            return result;
        }
        public ApiResponse<OptionDTO> ProductOptionsUpdate(OptionDTO item)
        {
            ApiResponse<OptionDTO> result = new ApiResponse<OptionDTO>();
            result = RestHelper.PostRequest<ApiResponse<OptionDTO>>(this.fullApiUri + "productoptions/" + Enc(item.Bvin) + "?key=" + Enc(key), MerchantTribe.Web.Json.ObjectToJson(item));
            return result;
        }
        public ApiResponse<bool> ProductOptionsDelete(string bvin)
        {
            ApiResponse<bool> result = new ApiResponse<bool>();
            result = RestHelper.DeleteRequest<ApiResponse<bool>>(this.fullApiUri + "productoptions/" + Enc(bvin) + "?key=" + Enc(key), string.Empty);
            return result;
        }
        public ApiResponse<bool> ProductOptionsAssignToProduct(string optionBvin, string productBvin, bool generateVariants)
        {
            ApiResponse<bool> result = new ApiResponse<bool>();            
            result = RestHelper.PostRequest<ApiResponse<bool>>(this.fullApiUri + "productoptions/" 
                                                                + Enc(optionBvin) 
                                                                + "/products/" 
                                                                + Enc(productBvin) 
                                                                + "?key=" + Enc(key)
                                                                + "&generatevariants=" + (generateVariants ? "1":"0"), string.Empty);
            return result;
        }
        public ApiResponse<bool> ProductOptionsGenerateAllVariants(string productBvin)
        {
            ApiResponse<bool> result = new ApiResponse<bool>();
            result = RestHelper.PostRequest<ApiResponse<bool>>(this.fullApiUri + "productoptions/0/generateonly/" + Enc(productBvin) + "?key=" + Enc(key), string.Empty);
            return result;
        }

        public ApiResponse<bool> ProductVariantUpdateSku(ProductVariantSkuUpdateDTO data)
        {
            ApiResponse<bool> result = new ApiResponse<bool>();
            result = RestHelper.PostRequest<ApiResponse<bool>>(this.fullApiUri + "productvariantssku/" + Enc(data.ProductBvin) + "?key=" + Enc(key), MerchantTribe.Web.Json.ObjectToJson(data));
            return result;
        }

        // Products
        public ApiResponse<List<ProductDTO>> ProductsFindAll()
        {
            ApiResponse<List<ProductDTO>> result = new ApiResponse<List<ProductDTO>>();
            result = RestHelper.GetRequest<ApiResponse<List<ProductDTO>>>(this.fullApiUri + "products/?key=" + Enc(key));
            return result;
        }
        public ApiResponse<PageOfProducts> ProductsFindForCategory(string categoryBvin, int pageNumber, int pageSize)
        {
            ApiResponse<PageOfProducts> result = new ApiResponse<PageOfProducts>();
            result = RestHelper.GetRequest<ApiResponse<PageOfProducts>>(this.fullApiUri + "products/?key=" + Enc(key) + "&bycategory=" + Enc(categoryBvin) + "&page=" + pageNumber + "&pagesize=" + pageSize);
            return result;
        }
        public ApiResponse<PageOfProducts> ProductsFindPage(int pageNumber, int pageSize)
        {
            ApiResponse<PageOfProducts> result = new ApiResponse<PageOfProducts>();
            result = RestHelper.GetRequest<ApiResponse<PageOfProducts>>(this.fullApiUri + "products/?key=" + Enc(key) + "&page=" + pageNumber + "&pagesize=" + pageSize);
            return result;
        }
        public ApiResponse<long> ProductsCountOfAll()
        {
            ApiResponse<long> result = new ApiResponse<long>();
            result = RestHelper.GetRequest<ApiResponse<long>>(this.fullApiUri + "products/?key=" + Enc(key) + "&countonly=1");
            return result;
        }
        public ApiResponse<ProductDTO> ProductsFind(string bvin)
        {
            ApiResponse<ProductDTO> result = new ApiResponse<ProductDTO>();
            result = RestHelper.GetRequest<ApiResponse<ProductDTO>>(this.fullApiUri + "products/" + Enc(bvin) + "?key=" + Enc(key));
            return result;
        }
        public ApiResponse<ProductDTO> ProductsFindBySku(string sku)
        {
            ApiResponse<ProductDTO> result = new ApiResponse<ProductDTO>();
            result = RestHelper.GetRequest<ApiResponse<ProductDTO>>(this.fullApiUri + "products/ANY?key=" + Enc(key) + "&bysku=" + Enc(sku));
            return result;
        }
        public ApiResponse<ProductDTO> ProductsBySlug(string slug)
        {
            ApiResponse<ProductDTO> result = new ApiResponse<ProductDTO>();
            result = RestHelper.GetRequest<ApiResponse<ProductDTO>>(this.fullApiUri + "products/ANY?key=" + Enc(key) + "&byslug=" + Enc(slug));
            return result;
        }
        public ApiResponse<ProductDTO> ProductsCreate(ProductDTO item, byte[] imageData)
        {
            ApiResponse<ProductDTO> result = new ApiResponse<ProductDTO>();
            result = RestHelper.PostRequest<ApiResponse<ProductDTO>>(this.fullApiUri + "products/?key=" + Enc(key), MerchantTribe.Web.Json.ObjectToJson(item));

            if (result.Content != null)
            {
                ProductsMainImageUpload(result.Content.Bvin, result.Content.ImageFileSmall, imageData);
            }
            return result;
        }
        public ApiResponse<ProductDTO> ProductsUpdate(ProductDTO item)
        {
            ApiResponse<ProductDTO> result = new ApiResponse<ProductDTO>();
            result = RestHelper.PostRequest<ApiResponse<ProductDTO>>(this.fullApiUri + "products/" + Enc(item.Bvin) + "?key=" + Enc(key), MerchantTribe.Web.Json.ObjectToJson(item));
            return result;
        }
        public ApiResponse<bool> ProductsDelete(string bvin)
        {
            ApiResponse<bool> result = new ApiResponse<bool>();
            result = RestHelper.DeleteRequest<ApiResponse<bool>>(this.fullApiUri + "products/" + Enc(bvin) + "?key=" + Enc(key), string.Empty);
            return result;
        }
        public ApiResponse<ClearProductsData> ProductsClearAll(int howMany)
        {
            ApiResponse<ClearProductsData> result = new ApiResponse<ClearProductsData>();
            result = RestHelper.DeleteRequest<ApiResponse<ClearProductsData>>(this.fullApiUri + "products/?key=" + Enc(key) + "&howmany=" + howMany, string.Empty);
            return result;
        }

        // Product Relationships
        public ApiResponse<ProductRelationshipDTO> ProductRelationshipsFind(long id)
        {
            ApiResponse<ProductRelationshipDTO> result = new ApiResponse<ProductRelationshipDTO>();
            result = RestHelper.GetRequest<ApiResponse<ProductRelationshipDTO>>(this.fullApiUri + "productrelationships/" + id + "?key=" + Enc(key));
            return result;
        }
        public ApiResponse<ProductRelationshipDTO> ProductRelationshipsCreate(ProductRelationshipDTO item)
        {
            ApiResponse<ProductRelationshipDTO> result = new ApiResponse<ProductRelationshipDTO>();
            result = RestHelper.PostRequest<ApiResponse<ProductRelationshipDTO>>(this.fullApiUri + "productrelationships/?key=" + Enc(key), MerchantTribe.Web.Json.ObjectToJson(item));
            return result;
        }
        public ApiResponse<bool> ProductRelationshipsQuickCreate(string productBvin, string otherProductBvin, bool isSubstitute)
        {
            ApiResponse<bool> response = new ApiResponse<bool>();

            ProductRelationshipDTO r = new ProductRelationshipDTO();
            r.IsSubstitute = isSubstitute;
            r.ProductId = productBvin;
            r.RelatedProductId = otherProductBvin;

            ApiResponse<ProductRelationshipDTO> result = ProductRelationshipsCreate(r);
            if (result.Content != null)
            {
                if (result.Content.Id > 0)
                {
                    response.Content = true;
                }
            }
            response.Errors = result.Errors;
            return response;
        }
        public ApiResponse<ProductRelationshipDTO> ProductRelationshipsUpdate(ProductRelationshipDTO item)
        {
            ApiResponse<ProductRelationshipDTO> result = new ApiResponse<ProductRelationshipDTO>();
            result = RestHelper.PostRequest<ApiResponse<ProductRelationshipDTO>>(this.fullApiUri + "productrelationships/" + item.Id + "?key=" + Enc(key), MerchantTribe.Web.Json.ObjectToJson(item));
            return result;
        }
        public ApiResponse<bool> ProductRelationshipsUnrelate(string productBvin, string otherProductBvin)
        {
            ApiResponse<bool> result = new ApiResponse<bool>();
            result = RestHelper.DeleteRequest<ApiResponse<bool>>(this.fullApiUri + "productrelationships/" + Enc(productBvin) + "/" + Enc(otherProductBvin) + "?key=" + Enc(key), string.Empty);
            return result;
        }

        // Image Upload Helpers
        public ApiResponse<bool> ProductsMainImageUpload(string productBvin, string fileName, byte[] imageData)
        {
            ApiResponse<bool> response = new ApiResponse<bool>();
            response = RestHelper.PostRequest<ApiResponse<bool>>(this.fullApiUri + "productmainimage/" + Enc(productBvin) + "?key=" + Enc(key) + "&filename=" + Enc(fileName),
                                                        MerchantTribe.Web.Json.ObjectToJson(imageData));
            return response;
        }        
        public ApiResponse<bool> CategoriesImagesIconUpload(string categoryBvin, string fileName, byte[] imageData)
        {
            ApiResponse<bool> response = new ApiResponse<bool>();
            response = RestHelper.PostRequest<ApiResponse<bool>>(this.fullApiUri + "categoriesimagesicon/" + Enc(categoryBvin) + "?key=" + Enc(key) + "&filename=" + Enc(fileName),
                                                        MerchantTribe.Web.Json.ObjectToJson(imageData));
            return response;
        }
        public ApiResponse<bool> CategoriesImagesBannerUpload(string categoryBvin, string fileName, byte[] imageData)
        {
            ApiResponse<bool> response = new ApiResponse<bool>();
            response = RestHelper.PostRequest<ApiResponse<bool>>(this.fullApiUri + "categoriesimagesbanner/" + Enc(categoryBvin) + "?key=" + Enc(key) + "&filename=" + Enc(fileName),
                                                        MerchantTribe.Web.Json.ObjectToJson(imageData));
            return response;
        }

        // Product Files
        public ApiResponse<List<ProductFileDTO>> ProductFilesFindForProduct(string bvin)
        {
            ApiResponse<List<ProductFileDTO>> result = new ApiResponse<List<ProductFileDTO>>();
            result = RestHelper.GetRequest<ApiResponse<List<ProductFileDTO>>>(this.fullApiUri + "productfilesxproducts/" + Enc(bvin) + "?key=" + Enc(key));
            return result;
        }
        public ApiResponse<ProductFileDTO> ProductFilesFind(string bvin)
        {
            ApiResponse<ProductFileDTO> result = new ApiResponse<ProductFileDTO>();
            result = RestHelper.GetRequest<ApiResponse<ProductFileDTO>>(this.fullApiUri + "productfiles/" + Enc(bvin) + "?key=" + Enc(key));
            return result;
        }
        public ApiResponse<ProductFileDTO> ProductFilesCreate(ProductFileDTO item)
        {
            ApiResponse<ProductFileDTO> result = new ApiResponse<ProductFileDTO>();
            result = RestHelper.PostRequest<ApiResponse<ProductFileDTO>>(this.fullApiUri + "productfiles/?key=" + Enc(key), MerchantTribe.Web.Json.ObjectToJson(item));
            return result;
        }                
        public ApiResponse<ProductFileDTO> ProductFilesUpdate(ProductFileDTO item)
        {
            ApiResponse<ProductFileDTO> result = new ApiResponse<ProductFileDTO>();
            result = RestHelper.PostRequest<ApiResponse<ProductFileDTO>>(this.fullApiUri + "productfiles/" + Enc(item.Bvin) + "?key=" + Enc(key), MerchantTribe.Web.Json.ObjectToJson(item));
            return result;
        }
        public ApiResponse<bool> ProductFilesDelete(string bvin)
        {
            ApiResponse<bool> result = new ApiResponse<bool>();
            result = RestHelper.DeleteRequest<ApiResponse<bool>>(this.fullApiUri + "productfiles/" + Enc(bvin) + "?key=" + Enc(key), string.Empty);
            return result;
        }
        public ApiResponse<bool> ProductFilesAddToProduct(string productBvin, string fileBvin, int availableMinutes, int maxDownloads)
        {
            ApiResponse<bool> result = new ApiResponse<bool>();
            result = RestHelper.PostRequest<ApiResponse<bool>>(this.fullApiUri + "productfilesxproducts/" + Enc(fileBvin) + "/" + Enc(productBvin) + "?key=" + Enc(key) + "&minutes=" + availableMinutes + "&downloads=" + maxDownloads, string.Empty);
            return result;
        }
        public ApiResponse<bool> ProductFilesRemoveFromProduct(string productBvin, string fileBvin)
        {
            ApiResponse<bool> result = new ApiResponse<bool>();
            result = RestHelper.DeleteRequest<ApiResponse<bool>>(this.fullApiUri + "productfilesxproducts/" + Enc(fileBvin) + "/" + Enc(productBvin) + "?key=" + Enc(key), string.Empty);
            return result;
        }
        public ApiResponse<bool> ProductFilesDataUploadFirstPart(string fileBvin, string fileName, string description, byte[] data)
        {
            ApiResponse<bool> response = new ApiResponse<bool>();
            response = RestHelper.PostRequest<ApiResponse<bool>>(this.fullApiUri + "productfilesdata/" + Enc(fileBvin) + "?key=" + Enc(key) + "&first=1" + "&filename=" + Enc(fileName) + "&description=" + Enc(description),
                                                        MerchantTribe.Web.Json.ObjectToJson(data));
            return response;
        }
        public ApiResponse<bool> ProductFilesDataUploadAdditionalPart(string fileBvin, string fileName, byte[] moreData)
        {
            ApiResponse<bool> response = new ApiResponse<bool>();
            response = RestHelper.PostRequest<ApiResponse<bool>>(this.fullApiUri + "productfilesdata/" + Enc(fileBvin) + "?key=" + Enc(key) + "&first=0" + "&filename=" + Enc(fileName),
                                                        MerchantTribe.Web.Json.ObjectToJson(moreData));
            return response;
        }

        // Product Inventory
        //public ApiResponse<List<ProductInventoryDTO>> ProductInventoryFindAll()
        //{
        //    ApiResponse<List<ProductInventoryDTO>> result = new ApiResponse<List<ProductInventoryDTO>>();
        //    result = RestHelper.GetRequest<ApiResponse<List<ProductInventoryDTO>>>(this.fullApiUri + "productinventory/?key=" + Enc(key));
        //    return result;
        //}
        public ApiResponse<ProductInventoryDTO> ProductInventoryFind(string bvin)
        {
            ApiResponse<ProductInventoryDTO> result = new ApiResponse<ProductInventoryDTO>();
            result = RestHelper.GetRequest<ApiResponse<ProductInventoryDTO>>(this.fullApiUri + "productinventory/" + Enc(bvin) + "?key=" + Enc(key));
            return result;
        }
        public ApiResponse<ProductInventoryDTO> ProductInventoryCreate(ProductInventoryDTO item)
        {
            ApiResponse<ProductInventoryDTO> result = new ApiResponse<ProductInventoryDTO>();
            result = RestHelper.PostRequest<ApiResponse<ProductInventoryDTO>>(this.fullApiUri + "productinventory/?key=" + Enc(key), MerchantTribe.Web.Json.ObjectToJson(item));
            return result;
        }
        public ApiResponse<ProductInventoryDTO> ProductInventoryUpdate(ProductInventoryDTO item)
        {
            ApiResponse<ProductInventoryDTO> result = new ApiResponse<ProductInventoryDTO>();
            result = RestHelper.PostRequest<ApiResponse<ProductInventoryDTO>>(this.fullApiUri + "productinventory/" + Enc(item.Bvin) + "?key=" + Enc(key), MerchantTribe.Web.Json.ObjectToJson(item));
            return result;
        }
        public ApiResponse<bool> ProductInventoryDelete(string bvin)
        {
            ApiResponse<bool> result = new ApiResponse<bool>();
            result = RestHelper.DeleteRequest<ApiResponse<bool>>(this.fullApiUri + "productinventory/" + Enc(bvin) + "?key=" + Enc(key), string.Empty);
            return result;
        }

        // Product Images
        //public ApiResponse<List<ProductImageDTO>> ProductImagesFindAll()
        //{
        //    ApiResponse<List<ProductImageDTO>> result = new ApiResponse<ProductImageDTO>>();
        //    result = RestHelper.GetRequest<ApiResponse<List<ProductImageDTO>>>(this.fullApiUri + "productimages/?key=" + Enc(key));
        //    return result;
        //}
        public ApiResponse<ProductImageDTO> ProductImagesFind(string bvin)
        {
            ApiResponse<ProductImageDTO> result = new ApiResponse<ProductImageDTO>();
            result = RestHelper.GetRequest<ApiResponse<ProductImageDTO>>(this.fullApiUri + "productimages/" + Enc(bvin) + "?key=" + Enc(key));
            return result;
        }
        public ApiResponse<ProductImageDTO> ProductImagesCreate(ProductImageDTO item, byte[] data)
        {
            ApiResponse<ProductImageDTO> result = new ApiResponse<ProductImageDTO>();
            result = RestHelper.PostRequest<ApiResponse<ProductImageDTO>>(this.fullApiUri + "productimages/?key=" + Enc(key), MerchantTribe.Web.Json.ObjectToJson(item));
            if (data != null)
            {
                if (result.Content != null && result.Errors.Count < 1)
                {
                    ProductImagesUpload(item.ProductId, result.Content.Bvin, result.Content.FileName, data);
                }
            }
            return result;
        }
        public ApiResponse<ProductImageDTO> ProductImagesUpdate(ProductImageDTO item)
        {
            ApiResponse<ProductImageDTO> result = new ApiResponse<ProductImageDTO>();
            result = RestHelper.PostRequest<ApiResponse<ProductImageDTO>>(this.fullApiUri + "productimages/" + Enc(item.Bvin) + "?key=" + Enc(key), MerchantTribe.Web.Json.ObjectToJson(item));
            return result;
        }
        public ApiResponse<bool> ProductImagesDelete(string bvin)
        {
            ApiResponse<bool> result = new ApiResponse<bool>();
            result = RestHelper.DeleteRequest<ApiResponse<bool>>(this.fullApiUri + "productimages/" + Enc(bvin) + "?key=" + Enc(key), string.Empty);
            return result;
        }
        public ApiResponse<bool> ProductImagesUpload(string productBvin, string imageBvin, string fileName, byte[] imageData)
        {
            ApiResponse<bool> response = new ApiResponse<bool>();
            response = RestHelper.PostRequest<ApiResponse<bool>>(this.fullApiUri + "productimagesupload/" + Enc(productBvin) + "/" + Enc(imageBvin) + "?key=" + Enc(key) 
                                                        + "&filename=" + Enc(fileName)
                                                        , MerchantTribe.Web.Json.ObjectToJson(imageData));
            return response;
        }

        // Product Volume Discounts
        public ApiResponse<ProductVolumeDiscountDTO> ProductVolumeDiscountsFind(string bvin)
        {
            ApiResponse<ProductVolumeDiscountDTO> result = new ApiResponse<ProductVolumeDiscountDTO>();
            result = RestHelper.GetRequest<ApiResponse<ProductVolumeDiscountDTO>>(this.fullApiUri + "productvolumediscounts/" + Enc(bvin) + "?key=" + Enc(key));
            return result;
        }
        public ApiResponse<ProductVolumeDiscountDTO> ProductVolumeDiscountsCreate(ProductVolumeDiscountDTO item)
        {
            ApiResponse<ProductVolumeDiscountDTO> result = new ApiResponse<ProductVolumeDiscountDTO>();
            result = RestHelper.PostRequest<ApiResponse<ProductVolumeDiscountDTO>>(this.fullApiUri + "productvolumediscounts/?key=" + Enc(key), MerchantTribe.Web.Json.ObjectToJson(item));
            return result;
        }
        public ApiResponse<ProductVolumeDiscountDTO> ProductVolumeDiscountsUpdate(ProductVolumeDiscountDTO item)
        {
            ApiResponse<ProductVolumeDiscountDTO> result = new ApiResponse<ProductVolumeDiscountDTO>();
            result = RestHelper.PostRequest<ApiResponse<ProductVolumeDiscountDTO>>(this.fullApiUri + "productvolumediscounts/" + Enc(item.Bvin) + "?key=" + Enc(key), MerchantTribe.Web.Json.ObjectToJson(item));
            return result;
        }
        public ApiResponse<bool> ProductVolumeDiscountsDelete(string bvin)
        {
            ApiResponse<bool> result = new ApiResponse<bool>();
            result = RestHelper.DeleteRequest<ApiResponse<bool>>(this.fullApiUri + "productvolumediscounts/" + Enc(bvin) + "?key=" + Enc(key), string.Empty);
            return result;
        }

        // Product Reviews
        public ApiResponse<List<ProductReviewDTO>> ProductReviewsFindAll()
        {
            ApiResponse<List<ProductReviewDTO>> result = new ApiResponse<List<ProductReviewDTO>>();
            result = RestHelper.GetRequest<ApiResponse<List<ProductReviewDTO>>>(this.fullApiUri + "productreviews/?key=" + Enc(key));
            return result;
        }
        public ApiResponse<List<ProductReviewDTO>> ProductReviewsByProduct(string productBvin)
        {
            ApiResponse<List<ProductReviewDTO>> result = new ApiResponse<List<ProductReviewDTO>>();
            result = RestHelper.GetRequest<ApiResponse<List<ProductReviewDTO>>>(this.fullApiUri + "productreviews/?key=" + Enc(key) + "&productbvin=" + Enc(productBvin));
            return result;
        }
        public ApiResponse<ProductReviewDTO> ProductReviewsFind(string bvin)
        {
            ApiResponse<ProductReviewDTO> result = new ApiResponse<ProductReviewDTO>();
            result = RestHelper.GetRequest<ApiResponse<ProductReviewDTO>>(this.fullApiUri + "productreviews/" + Enc(bvin) + "?key=" + Enc(key));
            return result;
        }
        public ApiResponse<ProductReviewDTO> ProductReviewsCreate(ProductReviewDTO item)
        {
            ApiResponse<ProductReviewDTO> result = new ApiResponse<ProductReviewDTO>();
            result = RestHelper.PostRequest<ApiResponse<ProductReviewDTO>>(this.fullApiUri + "productreviews/?key=" + Enc(key), MerchantTribe.Web.Json.ObjectToJson(item));
            return result;
        }
        public ApiResponse<ProductReviewDTO> ProductReviewsUpdate(ProductReviewDTO item)
        {
            ApiResponse<ProductReviewDTO> result = new ApiResponse<ProductReviewDTO>();
            result = RestHelper.PostRequest<ApiResponse<ProductReviewDTO>>(this.fullApiUri + "productreviews/" + Enc(item.Bvin) + "?key=" + Enc(key), MerchantTribe.Web.Json.ObjectToJson(item));
            return result;
        }
        public ApiResponse<bool> ProductReviewsDelete(string bvin)
        {
            ApiResponse<bool> result = new ApiResponse<bool>();
            result = RestHelper.DeleteRequest<ApiResponse<bool>>(this.fullApiUri + "productreviews/" + Enc(bvin) + "?key=" + Enc(key), string.Empty);
            return result;
        }


        // Category Product Association
        public ApiResponse<CategoryProductAssociationDTO> CategoryProductAssociationsFind(long id)
        {
            ApiResponse<CategoryProductAssociationDTO> result = new ApiResponse<CategoryProductAssociationDTO>();
            result = RestHelper.GetRequest<ApiResponse<CategoryProductAssociationDTO>>(this.fullApiUri + "categoryproductassociations/" + id + "?key=" + Enc(key));
            return result;
        }
        public ApiResponse<CategoryProductAssociationDTO> CategoryProductAssociationsCreate(CategoryProductAssociationDTO item)
        {
            ApiResponse<CategoryProductAssociationDTO> result = new ApiResponse<CategoryProductAssociationDTO>();
            result = RestHelper.PostRequest<ApiResponse<CategoryProductAssociationDTO>>(this.fullApiUri + "categoryproductassociations/?key=" + Enc(key), MerchantTribe.Web.Json.ObjectToJson(item));
            return result;
        }
        public ApiResponse<bool> CategoryProductAssociationsQuickCreate(string productBvin, string categoryBvin)
        {
            ApiResponse<bool> response = new ApiResponse<bool>();

            CategoryProductAssociationDTO a = new CategoryProductAssociationDTO();
            a.CategoryId = categoryBvin;
            a.ProductId = productBvin;

            ApiResponse<CategoryProductAssociationDTO> result = CategoryProductAssociationsCreate(a);
            if (result.Content != null)
            {
                if (result.Content.Id > 0)
                {
                    response.Content = true;
                }
            }
            response.Errors = result.Errors;
            return response;
        }
        public ApiResponse<CategoryProductAssociationDTO> CategoryProductAssociationsUpdate(CategoryProductAssociationDTO item)
        {
            ApiResponse<CategoryProductAssociationDTO> result = new ApiResponse<CategoryProductAssociationDTO>();
            result = RestHelper.PostRequest<ApiResponse<CategoryProductAssociationDTO>>(this.fullApiUri + "categoryproductassociations/" + item.Id + "?key=" + Enc(key), MerchantTribe.Web.Json.ObjectToJson(item));
            return result;
        }
        public ApiResponse<bool> CategoryProductAssociationsUnrelate(string productBvin, string categoryBvin)
        {
            ApiResponse<bool> result = new ApiResponse<bool>();
            result = RestHelper.DeleteRequest<ApiResponse<bool>>(this.fullApiUri + "categoryproductassociations/" + Enc(productBvin) + "/" + Enc(categoryBvin) + "?key=" + Enc(key), string.Empty);
            return result;
        }
        
        // Search Manager
        public ApiResponse<bool> SearchManagerIndexProduct(string productBvin)
        {
            ApiResponse<bool> response = new ApiResponse<bool>();
            response = RestHelper.PostRequest<ApiResponse<bool>>(this.fullApiUri + "searchmanager/products/" + Enc(productBvin) + "?key=" + Enc(key), string.Empty);
            return response;
        }

        // Orders
        public ApiResponse<List<Orders.OrderSnapshotDTO>> OrdersFindAll()
        {
            ApiResponse<List<Orders.OrderSnapshotDTO>> result = new ApiResponse<List<Orders.OrderSnapshotDTO>>();
            result = RestHelper.GetRequest<ApiResponse<List<Orders.OrderSnapshotDTO>>>(this.fullApiUri + "orders/?key=" + Enc(key));
            return result;
        }
        public ApiResponse<Orders.OrderDTO> OrdersFind(string bvin)
        {
            ApiResponse<Orders.OrderDTO> result = new ApiResponse<Orders.OrderDTO>();
            result = RestHelper.GetRequest<ApiResponse<Orders.OrderDTO>>(this.fullApiUri + "orders/" + Enc(bvin) + "?key=" + Enc(key));
            return result;
        }
        public ApiResponse<Orders.OrderDTO> OrdersCreate(Orders.OrderDTO item)
        {
            ApiResponse<Orders.OrderDTO> result = new ApiResponse<Orders.OrderDTO>();
            result = RestHelper.PostRequest<ApiResponse<Orders.OrderDTO>>(this.fullApiUri + "orders/?key=" + Enc(key), MerchantTribe.Web.Json.ObjectToJson(item));
            return result;
        }
        public ApiResponse<Orders.OrderDTO> OrdersUpdate(Orders.OrderDTO item)
        {
            ApiResponse<Orders.OrderDTO> result = new ApiResponse<Orders.OrderDTO>();
            result = RestHelper.PostRequest<ApiResponse<Orders.OrderDTO>>(this.fullApiUri + "orders/" + Enc(item.Bvin) + "?key=" + Enc(key), MerchantTribe.Web.Json.ObjectToJson(item));
            return result;
        }
        public ApiResponse<bool> OrdersDelete(string bvin)
        {
            ApiResponse<bool> result = new ApiResponse<bool>();
            result = RestHelper.DeleteRequest<ApiResponse<bool>>(this.fullApiUri + "orders/" + Enc(bvin) + "?key=" + Enc(key), string.Empty);
            return result;
        }

        // Order Transactions
        public ApiResponse<List<OrderTransactionDTO>> OrderTransactionsFindForOrder(string bvin)
        {
            ApiResponse<List<OrderTransactionDTO>> result = new ApiResponse<List<OrderTransactionDTO>>();
            result = RestHelper.GetRequest<ApiResponse<List<OrderTransactionDTO>>>(this.fullApiUri + "ordertransactions/?key=" + Enc(key) + "&orderbvin=" + Enc(bvin));
            return result;
        }
        public ApiResponse<OrderTransactionDTO> OrderTransactionsFind(Guid id)
        {
            ApiResponse<OrderTransactionDTO> result = new ApiResponse<OrderTransactionDTO>();
            result = RestHelper.GetRequest<ApiResponse<OrderTransactionDTO>>(this.fullApiUri + "ordertransactions/" + Enc(id.ToString()) + "?key=" + Enc(key));
            return result;
        }
        public ApiResponse<OrderTransactionDTO> OrderTransactionsCreate(OrderTransactionDTO item)
        {
            ApiResponse<OrderTransactionDTO> result = new ApiResponse<OrderTransactionDTO>();
            result = RestHelper.PostRequest<ApiResponse<OrderTransactionDTO>>(this.fullApiUri + "ordertransactions/?key=" + Enc(key), MerchantTribe.Web.Json.ObjectToJson(item));
            return result;
        }
        public ApiResponse<OrderTransactionDTO> OrderTransactionsUpdate(OrderTransactionDTO item)
        {
            ApiResponse<OrderTransactionDTO> result = new ApiResponse<OrderTransactionDTO>();
            result = RestHelper.PostRequest<ApiResponse<OrderTransactionDTO>>(this.fullApiUri + "ordertransactions/" + Enc(item.Id.ToString()) + "?key=" + Enc(key), MerchantTribe.Web.Json.ObjectToJson(item));
            return result;
        }
        public ApiResponse<bool> OrderTransactionsDelete(Guid id)
        {
            ApiResponse<bool> result = new ApiResponse<bool>();
            result = RestHelper.DeleteRequest<ApiResponse<bool>>(this.fullApiUri + "ordertransactions/" + Enc(id.ToString()) + "?key=" + Enc(key), string.Empty);
            return result;
        }

        //Utilities
        public ApiResponse<string> UtilitiesSlugify(string input)
        {
            ApiResponse<string> result = new ApiResponse<string>();
            result = RestHelper.PostRequest<ApiResponse<string>>(this.fullApiUri + "utilities/slugify?key=" + Enc(key), input);
            return result;
        }

        // Wish List Items
        public ApiResponse<WishListItemDTO> WishListItemsFind(long id)
        {
            ApiResponse<WishListItemDTO> result = new ApiResponse<WishListItemDTO>();
            result = RestHelper.GetRequest<ApiResponse<WishListItemDTO>>(this.fullApiUri + "wishlistitems/" + id + "?key=" + Enc(key));
            return result;
        }
        public ApiResponse<WishListItemDTO> WishListItemsCreate(WishListItemDTO item)
        {
            ApiResponse<WishListItemDTO> result = new ApiResponse<WishListItemDTO>();
            result = RestHelper.PostRequest<ApiResponse<WishListItemDTO>>(this.fullApiUri + "wishlistitems/?key=" + Enc(key), MerchantTribe.Web.Json.ObjectToJson(item));
            return result;
        }
        public ApiResponse<WishListItemDTO> WishListItemsUpdate(WishListItemDTO item)
        {
            ApiResponse<WishListItemDTO> result = new ApiResponse<WishListItemDTO>();
            result = RestHelper.PostRequest<ApiResponse<WishListItemDTO>>(this.fullApiUri + "wishlistitems/" + item.Id + "?key=" + Enc(key), MerchantTribe.Web.Json.ObjectToJson(item));
            return result;
        }
        public ApiResponse<bool> WishListItemsDelete(long id)
        {
            ApiResponse<bool> result = new ApiResponse<bool>();
            result = RestHelper.DeleteRequest<ApiResponse<bool>>(this.fullApiUri + "wishlistitems/" + id + "?key=" + Enc(key), string.Empty);
            return result;
        }
       
    }

}
