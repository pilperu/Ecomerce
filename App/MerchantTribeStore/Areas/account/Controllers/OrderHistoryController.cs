using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MerchantTribeStore.Filters;
using MerchantTribe.Commerce;
using MerchantTribe.Commerce.Orders;
using MerchantTribe.Commerce.Catalog;
using MerchantTribe.Web;
using MerchantTribe.Commerce.Content;

namespace MerchantTribeStore.Areas.account.Controllers
{
    [CustomerSignedInFilter]
    public class OrderHistoryController : MerchantTribeStore.Controllers.Shared.BaseStoreController
    {
        //
        // GET: /account/OrderHistory/
        public ActionResult Index()
        {            
            ViewBag.MetaDescription = "Order History | " + MTApp.CurrentStore.Settings.MetaDescription;
            ViewBag.MetaKeywords = MTApp.CurrentStore.Settings.MetaKeywords;
            ViewBag.BodyClass = "myaccountpage";
            ViewBag.ViewButtonImageUrl = MTApp.ThemeManager().ButtonUrl("view", Request.IsSecureConnection); ;

            List<OrderSnapshot> orders = LoadOrders();
            return View(orders);
        }

        private List<OrderSnapshot> LoadOrders()
        {
            
            int totalCount = 0;
            List<OrderSnapshot> orders = new List<OrderSnapshot>();

            // pull all BV Orders
            orders = MTApp.OrderServices.Orders.FindByUserId(MTApp.CurrentCustomerId, 1, 100, ref totalCount);

            if (orders == null) return new List<OrderSnapshot>();

            return orders;
        }

        //
        // GET: /account/OrderHistory/Details/{id}
        public ActionResult Details(string id)
        {
            ViewBag.Title = "View Order";
            ViewBag.MetaDescription = "View Order | " + MTApp.CurrentStore.Settings.MetaDescription;
            ViewBag.MetaKeywords = MTApp.CurrentStore.Settings.MetaKeywords;
            ViewBag.BodyClass = "myaccountorderdetailspage";
            
            PrepBVOrder(id);
            
            return View();           
        }
        
        private void PrepBVOrder(string bvin)
        {                        
            Order o = MTApp.OrderServices.Orders.FindForCurrentStore(bvin);
            ViewBag.Order = o;
            ViewBag.AcumaticaOrder = null;
            
            OrderPaymentSummary paySummary = MTApp.OrderServices.PaymentSummary(o);
            ViewBag.OrderPaymentSummary = paySummary;

            // File Downloads
            List<ProductFile> fileDownloads = new List<ProductFile>();            
            if ((o.PaymentStatus == OrderPaymentStatus.Paid) && (o.StatusCode != OrderStatusCode.OnHold))
            {                
                foreach (LineItem item in o.Items)
                {
                    if (item.ProductId != string.Empty)
                    {
                        List<ProductFile> productFiles = MTApp.CatalogServices.ProductFiles.FindByProductId(item.ProductId);
                        foreach (ProductFile file in productFiles)
                        {
                            fileDownloads.Add(file);
                        }
                    }
                }            
            }
            ViewBag.FileDownloads = fileDownloads;
            ViewBag.FileDownloadsButtonUrl = MTApp.ThemeManager().ButtonUrl("download", Request.IsSecureConnection);
        }

        public ActionResult DownloadFile(string id)
        {
            string orderBvin = Request.QueryString["oid"] ?? string.Empty;
            string fileId = id;
            string userId = MTApp.CurrentCustomerId;

            var o = MTApp.OrderServices.Orders.FindForCurrentStore(orderBvin);            
            var file = MTApp.CatalogServices.ProductFiles.Find(fileId);

            // Validation checks
            if (file == null)
            {
                FlashWarning("The file could not be located for download.");
                return View("DownloadFileError");
            }
            if (o == null)
            {
                FlashWarning("The order number could not be located for downloads.");
                return View("DownloadFileError");
            }
            if (o.UserID != userId)
            {
                FlashWarning("This order does not belong to the current user. Please try again.");
                return View("DownloadFileError");
            }

            if (file.MaxDownloads <= 0) file.MaxDownloads = 32000;
            int currentCount = o.GetFileDownloadCount(file.Bvin);
            if (currentCount >= file.MaxDownloads)
            {
                FlashWarning("This file has already been downloaded the maximum number of allowed times. Please contact store administrator for assistance.");
                return View("DownloadFileError");
            }

            if (file.AvailableMinutes != 0)
            {
                if (DateTime.UtcNow.AddMinutes(file.AvailableMinutes * -1) > o.TimeOfOrderUtc)
                {
                    FlashWarning("File can no longer be downloaded. Its available time period has elapsed.");
                    return View("DownloadFileError");
                }
            }

            // Load File from Disk
            string extension = System.IO.Path.GetExtension(file.FileName);
            string name = System.IO.Path.GetFileName(file.FileName);            

            long storeId = MTApp.CurrentStore.Id;
            string diskFileName = file.Bvin + "_" + file.FileName + ".config";
            if (!MerchantTribe.Commerce.Storage.DiskStorage.FileVaultFileExists(storeId, diskFileName))
            {
                FlashWarning("The file source code not be located.");
                return View("DownloadFileError");
            }            
            byte[] bytes = MerchantTribe.Commerce.Storage.DiskStorage.FileVaultGetBytes(storeId, diskFileName);
            string type = MerchantTribe.Commerce.Utilities.MimeTypes.FindTypeForExtension(extension);

            // Record download
            o.IncreaseFileDownloadCount(file.Bvin);
            MTApp.OrderServices.Orders.Update(o);

            // Send File
            FileContentResult r = new FileContentResult(bytes, type);
            r.FileDownloadName = file.FileName;            
            return r;            
        }

    }
}
