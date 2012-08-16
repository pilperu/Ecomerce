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
    public class ProductsHandler: BaseRestHandler
    {
        public ProductsHandler(MerchantTribe.Commerce.MerchantTribeApplication app)
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
                string categoryBvin = querystring["bycategory"] ?? string.Empty;
                string countonly = querystring["countonly"] ?? string.Empty;

                string page = querystring["page"] ?? "1";
                int pageInt = 1;
                int.TryParse(page, out pageInt);
                string pageSize = querystring["pagesize"] ?? "9";
                int pageSizeInt = 9;
                int.TryParse(pageSize, out pageSizeInt);
                int totalCount = 0;

                if (categoryBvin.Trim().Length > 0)
                {                    
                    // by category
                    ApiResponse<PageOfProducts> responsePage = new ApiResponse<PageOfProducts>();
                    responsePage.Content = new PageOfProducts();

                    List<Product> resultItems = new List<Product>();
                    resultItems = MTApp.CatalogServices.FindProductForCategoryWithSort(categoryBvin, CategorySortOrder.None, false, pageInt, pageSizeInt, ref totalCount);
                    responsePage.Content.TotalProductCount = totalCount;
                    foreach (Product p in resultItems)
                    {
                        responsePage.Content.Products.Add(p.ToDto());
                    }                    
                    data = MerchantTribe.Web.Json.ObjectToJson(responsePage);
                }
                else if (querystring["page"] != null)
                {
                    // all by page
                    ApiResponse<PageOfProducts> responsePage = new ApiResponse<PageOfProducts>();
                    responsePage.Content = new PageOfProducts();

                    List<Product> resultItems = new List<Product>();
                    resultItems = MTApp.CatalogServices.Products.FindAllPaged(pageInt, pageSizeInt);                    
                    foreach (Product p in resultItems)
                    {
                        responsePage.Content.Products.Add(p.ToDto());
                    }
                    data = MerchantTribe.Web.Json.ObjectToJson(responsePage);
                }
                else if (querystring["countonly"] != null)
                {
                    // count only
                    ApiResponse<long> responseCount = new ApiResponse<long>();
                    responseCount.Content = 0;                    
                    int output = MTApp.CatalogServices.Products.FindAllCount();
                    responseCount.Content = output;
                    data = MerchantTribe.Web.Json.ObjectToJson(responseCount);
                }
                else
                {
                    // single product
                    ApiResponse<List<ProductDTO>> response = new ApiResponse<List<ProductDTO>>();

                    List<Product> results = new List<Product>();
                    results = MTApp.CatalogServices.Products.FindAllPaged(1, 1000);
                    List<ProductDTO> dto = new List<ProductDTO>();
                    foreach (Product item in results)
                    {
                        dto.Add(item.ToDto());
                    }
                    response.Content = dto;
                    data = MerchantTribe.Web.Json.ObjectToJson(response);
                }                
            }
            else
            {
                string bysku = querystring["bysku"] ?? string.Empty;
                string byslug = querystring["byslug"] ?? string.Empty;
                string bvin = FirstParameter(parameters);

                // Find One Specific Category
                ApiResponse<ProductDTO> response = new ApiResponse<ProductDTO>();

                Product item = null;
                if (bysku.Trim().Length > 0)
                {
                    item = MTApp.CatalogServices.Products.FindBySku(bysku);
                }
                else if (byslug.Trim().Length > 0)
                {
                    item = MTApp.CatalogServices.Products.FindBySlug(byslug);
                }
                else
                {
                    item = MTApp.CatalogServices.Products.Find(bvin);
                }

                if (item == null)
                {
                    response.Errors.Add(new ApiError("NULL", "Could not locate that product. Check bvin and try again."));
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
            ApiResponse<ProductDTO> response = new ApiResponse<ProductDTO>();

            ProductDTO postedItem = null;
            try
            {
                postedItem = MerchantTribe.Web.Json.ObjectFromJson<ProductDTO>(postdata);
            }
            catch(Exception ex)
            {
                response.Errors.Add(new ApiError("EXCEPTION", ex.Message));
                return MerchantTribe.Web.Json.ObjectToJson(response);                
            }

            Product item = new Product();
            item.FromDto(postedItem);

            Product existing = MTApp.CatalogServices.Products.Find(item.Bvin);
            if (existing == null || existing.Bvin == string.Empty)
            {

                item.StoreId = MTApp.CurrentStore.Id;
                if (item.UrlSlug.Trim().Length < 1)
                {
                    item.UrlSlug = MerchantTribe.Web.Text.Slugify(item.ProductName, true, true);
                }

                // Try ten times to append to URL if in use
                bool rewriteUrlInUse = MerchantTribe.Commerce.Utilities.UrlRewriter.IsProductSlugInUse(item.UrlSlug, string.Empty, MTApp);
                for (int i = 0; i < 10; i++)
                {
                    if (rewriteUrlInUse)
                    {
                        item.UrlSlug = item.UrlSlug + "-2";
                        rewriteUrlInUse = MerchantTribe.Commerce.Utilities.UrlRewriter.IsProductSlugInUse(item.UrlSlug, string.Empty, MTApp);
                        if (rewriteUrlInUse == false) break;
                    }
                }

                if (MTApp.CatalogServices.ProductsCreateWithInventory(item, false))
                {
                    bvin = item.Bvin;
                }
            }
            else
            {
                MTApp.CatalogServices.ProductsUpdateWithSearchRebuild(item);
            }
            Product resultItem = MTApp.CatalogServices.Products.Find(bvin);                    
            if (resultItem != null) response.Content = resultItem.ToDto();
            


            data = MerchantTribe.Web.Json.ObjectToJson(response);            
            return data;
        }

        public override string DeleteAction(string parameters, System.Collections.Specialized.NameValueCollection querystring, string postdata)
        {
            string data = string.Empty;
            string bvin = FirstParameter(parameters);
            

            if (bvin == string.Empty)
            {
                string howManyString = querystring["howmany"];
                int howMany = 0;
                int.TryParse(howManyString, out howMany);

                // Clear All Products Requested
                ApiResponse<ClearProductsData> response = new ApiResponse<ClearProductsData>();
                response.Content = MTApp.ClearProducts(howMany);
                data = MerchantTribe.Web.Json.ObjectToJson(response);
            }
            else
            {
                // Single Item Delete
                ApiResponse<bool> response = new ApiResponse<bool>();
                response.Content = MTApp.DestroyProduct(bvin);
                data = MerchantTribe.Web.Json.ObjectToJson(response);
            }
            
            return data;
        }
    }
}