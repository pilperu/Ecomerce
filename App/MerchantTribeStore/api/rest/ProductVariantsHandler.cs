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
    public class ProductVariantsHandler: BaseRestHandler
    {
          public ProductVariantsHandler(MerchantTribe.Commerce.MerchantTribeApplication app)
            : base(app)
        {

        }

        // List or Find Single
        public override string GetAction(string parameters, System.Collections.Specialized.NameValueCollection querystring)
        {            
            throw new NotImplementedException();
        }

        // Create or Update
        public override string PostAction(string parameters, System.Collections.Specialized.NameValueCollection querystring, string postdata)
        {
            string data = string.Empty;
            string bvin = FirstParameter(parameters);
            ApiResponse<bool> response = new ApiResponse<bool>();

            ProductVariantSkuUpdateDTO postedItem = null;
            try
            {
                postedItem = MerchantTribe.Web.Json.ObjectFromJson<ProductVariantSkuUpdateDTO>(postdata);
            }
            catch(Exception ex)
            {
                response.Errors.Add(new ApiError("EXCEPTION", ex.Message));
                return MerchantTribe.Web.Json.ObjectToJson(response);                
            }


            Product existing = MTApp.CatalogServices.Products.Find(bvin);
            if (existing != null && existing.Bvin != string.Empty)
            {                
                
                foreach (var variant in existing.Variants)
                {
                    if (SelectionDataMatches(variant.Selections, postedItem.MatchingOptions))
                    {
                        variant.Sku = postedItem.Sku;
                        MTApp.CatalogServices.Products.Update(existing);
                        response.Content = true;
                        break;
                    }
                }
            }
          
            data = MerchantTribe.Web.Json.ObjectToJson(response);            
            return data;
        }

        public override string DeleteAction(string parameters, System.Collections.Specialized.NameValueCollection querystring, string postdata)
        {
           throw new NotImplementedException();
        }

        private bool SelectionDataMatches(OptionSelectionList variantSelections, List<VariantOptionDataDTO> options)
        {
            if (variantSelections.Count != options.Count)
            {
                return false;
            }

            int expectedMatched = options.Count;
            int actualMatches = 0;
            foreach (var opt in options)
            {
                var match = variantSelections.Where(y => y.OptionBvin.Replace("-", "").ToLowerInvariant() == opt.ChoiceId.Replace("-", "").ToLowerInvariant()
                    && y.SelectionData.Replace("-", "").ToLowerInvariant() == opt.ChoiceItemId.Replace("-", "").ToLowerInvariant()).FirstOrDefault();
                if (match != null)
                {
                    actualMatches++;
                }
            }

            return actualMatches == expectedMatched;            
        }
    }
}