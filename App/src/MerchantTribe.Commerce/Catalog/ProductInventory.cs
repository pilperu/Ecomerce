using System;
using System.Data;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using MerchantTribe.CommerceDTO.v1.Catalog;
using MerchantTribe.Web.Logging;

namespace MerchantTribe.Commerce.Catalog
{
    [Serializable]
    public class InventoryCheckData
    {
        public string InventoryMessage = string.Empty;
        public int Qty = 999;
        public bool IsInStock = true;
        public bool IsAvailableForSale = true;
    }

    [Serializable]
    public class ProductInventory
    {

        public string Bvin { get; set; }
        public long StoreId { get; set; }
        public DateTime LastUpdated { get; set; }
        public string ProductBvin{get;set;}
        public string VariantId{get;set;}        
        public int QuantityOnHand {get;set;} // The total physical count of items on hand
        public int QuantityReserved {get;set;} // Count of items in stock but reserved for carts or orders		
        public int LowStockPoint {get;set;} // When does the Low Stock warning go out to merchant?
        
        public ProductInventory()
        {
            this.Bvin = string.Empty;
            this.StoreId = 0;
            this.LastUpdated = DateTime.UtcNow;
            this.ProductBvin = string.Empty;
            this.VariantId = string.Empty;
            this.QuantityOnHand = 0;
            this.QuantityReserved = 0;
            this.LowStockPoint = 0;
        }
                
        public int QuantityAvailableForSale
        {
            get
            {
                int result = QuantityOnHand - QuantityReserved;
                return result;
            }
        }        

        public static void EmailLowStockReport(object State, MerchantTribeApplication app)
        {
            RequestContext context = app.CurrentRequestContext;
            if (context == null) return;

            if (!EmailLowStockReport(context.CurrentStore.Settings.MailServer.EmailForGeneral, context.CurrentStore.StoreName, app))
            {
                EventLog.LogEvent("Low Stock Report", "Low Stock Report Failed", EventLogSeverity.Error);
            }
        }
        public static bool EmailLowStockReport(string recipientEmail, string storeName, MerchantTribeApplication app)
        {
            bool result = false;

            try
            {
                string fromAddress = string.Empty;
                fromAddress = recipientEmail;

                System.Net.Mail.MailMessage m = new System.Net.Mail.MailMessage(fromAddress, recipientEmail);
                m.IsBodyHtml = false;
                m.Subject = "Low Stock Report From " + storeName;

                System.Text.StringBuilder sb = new System.Text.StringBuilder();

                sb.AppendLine("The following are low in stock or out of stock:");
                sb.Append(Environment.NewLine);

                List<Catalog.ProductInventory> inventories = app.CatalogServices.ProductInventories.FindAllLowStock();

                if (inventories.Count < 1)
                {
                    sb.Append("No out of stock items found.");
                }
                else
                {
                    foreach (Catalog.ProductInventory item in inventories)
                    {
                        Catalog.Product product = app.CatalogServices.Products.Find(item.ProductBvin);
                        if (product != null)
                        {
                            sb.Append(WebAppSettings.InventoryLowReportLinePrefix);
                            sb.Append(product.Sku);
                            sb.Append(", ");
                            sb.Append(product.ProductName);
                            sb.Append(", ");
                            sb.Append(item.QuantityOnHand);
                            sb.AppendLine(" ");
                        }
                    }
                }
                m.Body = sb.ToString();

                result = Utilities.MailServices.SendMail(m, app.CurrentStore);
            }

            catch (Exception ex)
            {
                EventLog.LogEvent(ex);
                result = false;
            }

            return result;
        }

        public ProductInventoryStatus InventoryEvaluateStatus(Catalog.Product p)
        {
            ProductInventoryStatus result = ProductInventoryStatus.Available;
            if (p != null)
            {
                if (p.Status == ProductStatus.Disabled)
                {
                    return ProductInventoryStatus.NotAvailable;
                }
            }

            if (QuantityAvailableForSale <= 0)
            {
                if (p != null)
                {
                    switch (p.InventoryMode)
                    {
                        case ProductInventoryMode.AlwayInStock:
                            result = ProductInventoryStatus.Available;
                            break;
                        case ProductInventoryMode.WhenOutOfStockAllowBackorders:
                            result = ProductInventoryStatus.Available;
                            break;
                        case ProductInventoryMode.WhenOutOfStockShow:
                            result = ProductInventoryStatus.NotAvailable;
                            break;
                        case ProductInventoryMode.WhenOutOfStockHide:
                            result = ProductInventoryStatus.NotAvailable;
                            break;
                    }
                }
            }
            return result;
        }

        
        //DTO
        public ProductInventoryDTO ToDto()
        {
            ProductInventoryDTO dto = new ProductInventoryDTO();

            dto.Bvin = this.Bvin;
            dto.LastUpdated = this.LastUpdated;
            dto.LowStockPoint = this.LowStockPoint;
            dto.ProductBvin = this.ProductBvin;
            dto.QuantityOnHand = this.QuantityOnHand;
            dto.QuantityReserved = this.QuantityReserved;
            dto.VariantId = this.VariantId;

            return dto;
        }
        public void FromDto(ProductInventoryDTO dto)
        {
            if (dto == null) return;

            this.Bvin = dto.Bvin;
            this.LastUpdated = dto.LastUpdated;
            this.LowStockPoint = dto.LowStockPoint;
            this.ProductBvin = dto.ProductBvin;
            this.QuantityOnHand = dto.QuantityOnHand;
            this.QuantityReserved = dto.QuantityReserved;
            this.VariantId = dto.VariantId;

        }
        
    }
}
