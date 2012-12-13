using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BVSoftware.Avalara;

namespace MerchantTribe.Commerce.Utilities
{
    public class AvalaraUtilities
    {
        public static BaseAddress ConvertAddressToAvalara(MerchantTribe.Web.Geography.IAddress address)
        {
            var result = new BaseAddress();
            result.Line1 = address.Street;
            result.Line2 = address.Street2;            
            result.City = address.City;
            result.Region = address.RegionData.Name;
            result.PostalCode = address.PostalCode;
            //result.Country = address.CountryName

            return result;
        }

        public static List<Line> ConvertOrderLines(List<Orders.LineItem> items)
        {
            var result = new List<Line>();

            int count = 1;
            
            foreach(Orders.LineItem item in items)
            {
                var newLine = new Line();
                newLine.No = count.ToString();
                newLine.ItemCode = item.ProductSku;
                newLine.Description = item.ProductShortDescription;                
                newLine.Amount = item.LineTotal;
                newLine.Qty = item.Quantity;
                if (item.LineTotalWithoutDiscounts != item.LineTotal)
                {
                    newLine.Discounted = true;
                }                
                result.Add(newLine);
                count += 1;
            }

            return result;
        }

        public static void CancelAvalaraTaxDocument(Orders.Order order, MerchantTribeApplication app)
        {
            string docId = order.CustomProperties.GetProperty("bvsoftware", BVAvaTax.AvalaraTaxPropertyName);
            
            var result = BVAvaTax.CancelTax(app.CurrentStore.Settings.Avalara.ServiceUrl, 
                app.CurrentStore.Settings.Avalara.Account, 
                app.CurrentStore.Settings.Avalara.LicenseKey, 
                app.CurrentStore.Settings.Avalara.CompanyCode, 
                app.CurrentStore.Settings.Avalara.Username, 
                app.CurrentStore.Settings.Avalara.Password, 
                order.OrderNumber, docId, DocumentType.SalesInvoice);

            if (!result.Success)
            {
                foreach(var m in result.Messages)
                {                
                    EventLog.LogEvent("CancelAvalaraTaxes", m, Web.Logging.EventLogSeverity.Information);
                }

                if (app.CurrentStore.Settings.Avalara.DebugMode)
                {
                    string note = "Avalara - Cancel Tax Request Failed:";
                    foreach(var m in result.Messages)
                         {
                             note += "\n" + m;
                         }                
                    order.Notes.Add(new Orders.OrderNote() {
                         IsPublic = false,
                         Note = note});                         
                    app.OrderServices.Orders.Update(order);
                }
            } else {   
                if (app.CurrentStore.Settings.Avalara.DebugMode)
                {
                    order.Notes.Add(new Orders.OrderNote() {
                         IsPublic = false,
                         Note = "Avalara - Cancel Tax Request Succeeded"}); 
                    app.OrderServices.Orders.Update(order);

                    EventLog.LogEvent("CancelAvalaraTaxes", "Avalara Taxes successfully cancelled. DocId: " + docId, Web.Logging.EventLogSeverity.Information);
                }
            }
        }

        public static string GetOrderIdentifier(Orders.Order order, MerchantTribeApplication app)
        {
            var result = order.OrderNumber;
            if (string.IsNullOrEmpty(result))
            {
                result = order.bvin;
            } else {
            
                if (string.IsNullOrEmpty(order.CustomProperties.GetProperty("bvsoftware", BVAvaTax.AvalaraGetTaxCountPropertyName)))
                {
                    result += "-1";                    
                    order.CustomProperties.SetProperty("bvsoftware", BVAvaTax.AvalaraGetTaxCountPropertyName, "1");
                    app.OrderServices.Orders.Update(order);
                } else {
                    var count = order.CustomProperties.GetPropertyAsInt("bvsoftware", BVAvaTax.AvalaraGetTaxCountPropertyName);
                       count += 1;                                                            
                    order.CustomProperties.SetProperty("bvsoftware", BVAvaTax.AvalaraGetTaxCountPropertyName, count.ToString());
                    app.OrderServices.Orders.Update(order);
                    result += "-" + count.ToString();
                }
            }
            return result;
        }

        public static void CommitAvalaraTaxes(Orders.Order order, string identifier, BaseAddress origin, BaseAddress destination, List<Line> lines, MerchantTribeApplication app)
        {                            
                var docId = "";                
                var customerCode = Utilities.AvalaraUtilities.GetCustomerCode(order);

                //The only difference here is that we are using a SalesInvoice instead of SalesOrder
            
                var result = BVAvaTax.GetTax(app.CurrentStore.Settings.Avalara.ServiceUrl, 
                    DocumentType.SalesInvoice, 
                    app.CurrentStore.Settings.Avalara.Account, 
                    app.CurrentStore.Settings.Avalara.LicenseKey, 
                    app.CurrentStore.Settings.Avalara.CompanyCode,
                    app.CurrentStore.Settings.Avalara.Username, 
                    app.CurrentStore.Settings.Avalara.Password,
                    identifier, origin, destination, lines, customerCode);
                                      
                if (result.Success)
                {                 
                    docId = result.DocId;
                } 
                else
                {
                    if (app.CurrentStore.Settings.Avalara.DebugMode)
                    {
                        string note = "Avalara - Commit Tax Failed:";
                        foreach(var m in result.Messages)
                         {
                             note += "\n" + m;
                         }                
                        order.Notes.Add(new Orders.OrderNote() {
                         IsPublic = false,
                         Note = note});                         
                        app.OrderServices.Orders.Update(order);
                        
                        EventLog.LogEvent("Avalara", note, Web.Logging.EventLogSeverity.Error);
                    }
                    return;
                }

            
                var totalTax = result.TotalTax;

                result = BVAvaTax.PostTax(app.CurrentStore.Settings.Avalara.ServiceUrl,
                    app.CurrentStore.Settings.Avalara.Account,
                    app.CurrentStore.Settings.Avalara.LicenseKey,
                    app.CurrentStore.Settings.Avalara.CompanyCode,
                    app.CurrentStore.Settings.Avalara.Username,
                    app.CurrentStore.Settings.Avalara.Password,
                    identifier, docId, DocumentType.SalesInvoice, result.TotalAmount, result.TotalTax);

                    if (result != null)
                    {
                        if (result.Success)
                        {                                                
                            order.CustomProperties.SetProperty("bvsoftware", BVAvaTax.AvalaraTaxPropertyName, result.DocId);                            
                            app.OrderServices.Orders.Update(order);
                        }
                        else
                        {
                            if (app.CurrentStore.Settings.Avalara.DebugMode)
                            {
                                string note = "Avalara - Commit Tax Failed (POST):";
                                foreach (var m in result.Messages)
                                {
                                    note += "\n" + m;
                                }
                                order.Notes.Add(new Orders.OrderNote()
                                {
                                    IsPublic = false,
                                    Note = note
                                });
                                app.OrderServices.Orders.Update(order);

                                EventLog.LogEvent("Avalara", note, Web.Logging.EventLogSeverity.Error);
                            }
                            return;
                        }
                    }


                    result = BVAvaTax.CommitTax(app.CurrentStore.Settings.Avalara.ServiceUrl,
                        app.CurrentStore.Settings.Avalara.Account,
                        app.CurrentStore.Settings.Avalara.LicenseKey,
                        app.CurrentStore.Settings.Avalara.CompanyCode,
                        app.CurrentStore.Settings.Avalara.Username,
                        app.CurrentStore.Settings.Avalara.Password,
                        identifier, docId, DocumentType.SalesInvoice);

                    if (result.Success)
                    {
                        if (app.CurrentStore.Settings.Avalara.DebugMode)
                        {
                            string note = "Avalara - Committed " + totalTax.ToString("C") + ":";
                            foreach (var m in result.Messages)
                            {
                                note += "\n" + m;
                            }
                            order.Notes.Add(new Orders.OrderNote()
                            {
                                IsPublic = false,
                                Note = note
                            });
                            app.OrderServices.Orders.Update(order);

                            EventLog.LogEvent("Avalara", note, Web.Logging.EventLogSeverity.Error);
                        }                        
                    } 
                    else
                    {
                        if (app.CurrentStore.Settings.Avalara.DebugMode)
                        {
                            string note = "Avalara - Commit Tax Failed (Commit Call):";
                            foreach (var m in result.Messages)
                            {
                                note += "\n" + m;
                            }
                            order.Notes.Add(new Orders.OrderNote()
                            {
                                IsPublic = false,
                                Note = note
                            });
                            app.OrderServices.Orders.Update(order);

                            EventLog.LogEvent("Avalara", note, Web.Logging.EventLogSeverity.Error);
                        }                        
                    }
        }

        public static void GetAvalaraTaxes(Orders.Order order, 
                                        string identifier, 
                                        BaseAddress origin, 
                                        BaseAddress destination,List<Line> lines, 
                                        string customerCode, MerchantTribeApplication app)
        {
            var result = new AvaTaxResult();            
                result = BVAvaTax.GetTax(app.CurrentStore.Settings.Avalara.ServiceUrl, 
                    DocumentType.SalesOrder, 
                    app.CurrentStore.Settings.Avalara.Account, 
                    app.CurrentStore.Settings.Avalara.LicenseKey, 
                    app.CurrentStore.Settings.Avalara.CompanyCode,
                    app.CurrentStore.Settings.Avalara.Username, 
                    app.CurrentStore.Settings.Avalara.Password,
                    identifier, origin, destination, lines, customerCode);
                        
            if (result != null)
            {
                if (result.Success == true)
                {                    
                    order.TotalTax = result.TotalTax;
                }
                else
                {
                    if (app.CurrentStore.Settings.Avalara.DebugMode)
                    {
                    string note = "Avalara - Get Tax Request Failed:";
                    foreach(var m in result.Messages)
                         {
                             note += "\n" + m;
                         }                
                    order.Notes.Add(new Orders.OrderNote() {
                         IsPublic = false,
                         Note = note});                         
                    app.OrderServices.Orders.Update(order);
                                                
                       EventLog.LogEvent("Avalara -  ApplyTaxes", note, Web.Logging.EventLogSeverity.Error);
                    }
                }
            }
        }

        public static string GetCustomerCode(Orders.Order order)
        {
            var customerId = "";
            if (order.UserEmail.Trim().Length > 0)
            {            
                customerId = order.UserEmail.Trim();
            }
            else
            {
                customerId = order.UserID;
            }
            if (customerId.Trim().Length < 1)
            {
                customerId = "1";
            }
            
            return customerId;
        }
    }
}
