using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using MerchantTribe.CommerceDTO;
using MerchantTribe.CommerceDTO.v1;
using MerchantTribe.CommerceDTO.v1.Catalog;
using MerchantTribe.CommerceDTO.v1.Client;
using MerchantTribe.CommerceDTO.v1.Contacts;
using MerchantTribe.CommerceDTO.v1.Content;
using MerchantTribe.CommerceDTO.v1.Marketing;
using MerchantTribe.CommerceDTO.v1.Membership;
using MerchantTribe.CommerceDTO.v1.Orders;
using MerchantTribe.CommerceDTO.v1.Shipping;
using MerchantTribe.CommerceDTO.v1.Taxes;
using MerchantTribe.Web.Geography;

namespace MerchantTribe.Migration.Migrators.Reindex
{
    public class Migrator : IMigrator
    {
        private MigrationSettings settings = null;
     
        public event MigrationService.ProgressReportDelegate ProgressReport;
        private void wl(string message)
        {
            if (this.ProgressReport != null)
            {
                this.ProgressReport(message);
            }
        }
        private void Header(string title)
        {
            wl("");
            wl("");
            wl("-----------------------------------------------------------");
            wl("");
            wl("    " + title + " at " + DateTime.UtcNow.ToString());
            wl("");
            wl("-----------------------------------------------------------");
            wl("");
        }
        private void DumpErrors(List<ApiError> errors)
        {
            foreach (ApiError e in errors)
            {
                wl("ERROR: " + e.Code + " | " + e.Description);
            }
        }

        private Api GetBV6Proxy()
        {
            Api result = null;
            try
            {
                string serviceUrl = settings.DestinationServiceRootUrl;
                string apiKey = settings.ApiKey;
                result = new Api(serviceUrl, apiKey);
            }
            catch (Exception ex)
            {
                wl("EXCEPTION While attempting to create service proxy for BV 6!");
                wl(ex.Message);
                wl(ex.StackTrace);
            }
            return result;
        }

        public void Migrate(MigrationSettings s)
        {
            settings = s;

            try
            {
                if (s.ClearProducts)
                {
                    ClearProductIndexes();
                }
                ReindexProducts();
            }
            catch (Exception e)
            {
                wl("ERROR: " + e.Message);
                wl(e.StackTrace);
            }
        }

        private void ReindexProducts()
        {
            Header("Getting Products to Index");

            var proxy = GetBV6Proxy();

            int limit = -1;
            if (settings.ImportProductLimit > 0)
            {
                limit = settings.ImportProductLimit;
            }

            int pageSize = 100;
            
            long totalRecords = 0;
            var totalRecordsResult = proxy.ProductsCountOfAll();
            totalRecords = (long)totalRecordsResult.Content;

            int totalPages = (int)(Math.Ceiling((decimal)totalRecords / (decimal)pageSize));

            int startPage = settings.ProductStartPage;
            if (startPage < 1) startPage = 1;
            if (startPage > totalPages) startPage = totalPages;

            for (int i = startPage; i <= totalPages; i++)
            {
                wl("Getting Products page " + i + " of " + totalPages.ToString());
                int startRecord = i * pageSize;

                var products = proxy.ProductsFindPage(i, pageSize);
                if (products == null)
                {
                    wl("ERROR - MISSING PRODUCTS FROM API! " + i);
                    continue;
                }
                var currentPage = (PageOfProducts)products.Content;

                if (currentPage == null)
                {
                    wl("ERROR - NULL PAGE " + i);
                    continue;
                }
                System.Threading.Tasks.Parallel.ForEach(currentPage.Products, IndexSingleProduct);                
            }
        }
        private void IndexSingleProduct(ProductDTO p)
        {
            if (p == null) return;

            wl("Indexing: " + p.ProductName + " [" + p.Sku + "]");
            Api proxy = GetBV6Proxy();
            var result = proxy.SearchManagerIndexProduct(p.Bvin);
            if (result != null)
            {
                if (result.Content == true)
                {
                    wl("SUCCESS - " + p.ProductName + " [" + p.Sku + "]");
                }
                else
                {
                    wl("FAIL - " + p.ProductName + " [" + p.Sku + "]");
                }
            }
        }

        private void ClearProductIndexes()
        {
            //Header("Clearing All Product Indexes");

            //int remaining = int.MaxValue;

            //while (remaining > 0)
            //{
            //    int pageSize = 100;

            //    Api bv6proxy = GetBV6Proxy();
            //    var res = bv6proxy.ProductsClearAll(pageSize);
            //    if (res == null)
            //    {
            //        wl("FAILED TO CLEAR PRODUCTS!");
            //    }
            //    else
            //    {
            //        if (res.Errors.Count > 0)
            //        {
            //            DumpErrors(res.Errors);
            //            wl("FAILED");
            //            return;
            //        }
            //        else
            //        {
            //            if (res.Content != null)
            //            {
            //                remaining = res.Content.ProductsRemaining;
            //                wl("Clearing products: " + res.Content.ProductsRemaining + " remaining at " + DateTime.UtcNow.ToString());
            //            }
            //            else
            //            {
            //                wl("Invalid Response. Skipping Clear..");
            //                remaining = 0;
            //            }
            //        }
            //    }
            //}
            //wl("Finished Clearing Products at : " + DateTime.UtcNow.ToString());
        }
    }
}
