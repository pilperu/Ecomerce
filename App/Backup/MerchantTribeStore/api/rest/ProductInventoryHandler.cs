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
    public class ProductInventoryHandler: BaseRestHandler
    {
        public ProductInventoryHandler(MerchantTribe.Commerce.MerchantTribeApplication app)
            : base(app)
        {

        }

        // List or Find Single
        public override string GetAction(string parameters, System.Collections.Specialized.NameValueCollection querystring)
        {            
            string data = string.Empty;

            
                // Find One Specific Category
            ApiResponse<ProductInventoryDTO> response = new ApiResponse<ProductInventoryDTO>();                                
                string bvin = FirstParameter(parameters);
                ProductInventory item = MTApp.CatalogServices.ProductInventories.Find(bvin);
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
            string bvin = FirstParameter(parameters);
            ApiResponse<ProductInventoryDTO> response = new ApiResponse<ProductInventoryDTO>();

            ProductInventoryDTO postedItem = null;
            try
            {
                postedItem = MerchantTribe.Web.Json.ObjectFromJson<ProductInventoryDTO>(postdata);
            }
            catch(Exception ex)
            {
                response.Errors.Add(new ApiError("EXCEPTION", ex.Message));
                return MerchantTribe.Web.Json.ObjectToJson(response);                
            }

            ProductInventory item = new ProductInventory();
            item.FromDto(postedItem);

            if (bvin == string.Empty)
            {
                bool mustCreate = false;

                // see if there is already an inventory object for product
                List<ProductInventory> existing = MTApp.CatalogServices.ProductInventories.FindByProductId(item.ProductBvin);
                if (existing == null || existing.Count < 1)
                {
                    mustCreate = true;                    
                }
                else
                {
                    var pi = existing.Where(y => y.VariantId == string.Empty).FirstOrDefault();
                    if (pi == null)
                    {
                        mustCreate = true;
                    }
                    else
                    {
                        pi.LowStockPoint = item.LowStockPoint;
                        pi.QuantityOnHand = item.QuantityOnHand;
                        pi.QuantityReserved = item.QuantityReserved;
                        MTApp.CatalogServices.ProductInventories.Update(pi);
                        bvin = pi.Bvin;
                    }
                }

                // if inventory object doesn't exist yet, create one.
                if (mustCreate)
                {
                    if (MTApp.CatalogServices.ProductInventories.Create(item))
                    {
                        bvin = item.Bvin;
                    }
                }                
            }
            else
            {
                MTApp.CatalogServices.ProductInventories.Update(item);
            }
            ProductInventory resultItem = MTApp.CatalogServices.ProductInventories.Find(bvin);                    
            if (resultItem != null) response.Content = resultItem.ToDto();
            
            data = MerchantTribe.Web.Json.ObjectToJson(response);            
            return data;
        }

        public override string DeleteAction(string parameters, System.Collections.Specialized.NameValueCollection querystring, string postdata)
        {
            string data = string.Empty;
            string bvin = FirstParameter(parameters);
            ApiResponse<bool> response = new ApiResponse<bool>();

            // Single Item Delete
            response.Content = MTApp.CatalogServices.ProductInventories.Delete(bvin);

            data = MerchantTribe.Web.Json.ObjectToJson(response);
            return data;
        }
    }
}