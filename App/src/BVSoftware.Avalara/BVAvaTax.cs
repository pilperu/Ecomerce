using System;
using System.Collections.Generic;
using System.Text;
using avt = Avalara.AvaTax.Adapter;


namespace BVSoftware.Avalara
{
    public class BVAvaTax
    {
        public static string AvalaraTaxPropertyName { get { return "AvalaraTaxCommitted"; } }
        public static string AvalaraGetTaxCountPropertyName { get { return "AvalaraGetTaxCount"; } }
        public static string AvalaraOrderEditedPropertyName { get { return "AvalaraOrderEdited"; } }

        public static AvaTaxResult GetTax(string URL, DocumentType DocType,
                                            string Account, string License,
                                            string CompanyCode, string UserName, string Password,
                                            string OrderIdentifier, BaseAddress OriginationAddress,
                                            BaseAddress DestinationAddress, List<Line> items,
                                            string CustomerCode)
        {

            BVSoftware.Avalara.AvaTaxResult result = new AvaTaxResult();

            avt.TaxService.GetTaxRequest gtr = new avt.TaxService.GetTaxRequest();
            gtr.OriginAddress = ConvertBaseAddress(OriginationAddress);
            gtr.DestinationAddress = ConvertBaseAddress(DestinationAddress);
            
            
            //' Origin and Destination addresses
            //OriginationAddress.AddressCode = "O"
            //DestinationAddress.AddressCode = "D"
            //gtr.Addresses = New BaseAddress(2) {}
            //gtr.Addresses(0) = OriginationAddress
            //gtr.Addresses(1) = DestinationAddress
            //Document Level Origin and Destination Addresses - see AddressCode field.
            //gtr.OriginCode = gtr.Addresses(0).AddressCode
            //gtr.DestinationCode = gtr.Addresses(1).AddressCode

            gtr.CompanyCode = CompanyCode;
            gtr.CustomerCode = CustomerCode;
            gtr.DetailLevel = avt.TaxService.DetailLevel.Document;
            gtr.DocCode = OrderIdentifier;
            gtr.DocType = ConvertDocType(DocType);
            gtr.DocDate = DateTime.Now;
            
            foreach (Line l in items)
            {
                avt.TaxService.Line nl = ConvertLine(l);
                gtr.Lines.Add(nl);
            }            
            
            avt.TaxService.TaxSvc svc = GetTaxServiceProxy(URL, Account, License, UserName, Password);
            avt.TaxService.GetTaxResult gtres = svc.GetTax(gtr);


            if (gtres.ResultCode != avt.SeverityLevel.Success)
            {
                result.Success = false;
                result.Messages.Add("GetTax Failed");
                ApplyMessagesToResult(result, gtres.Messages);
            }
            else
            {
                result.Success = true;
            }

            result.DocId = gtres.DocId;
            result.TotalAmount = gtres.TotalAmount;
            result.TotalTax = gtres.TotalTax;

            return result;                      
        }
        
        public static AvaTaxResult PostTax(string URL, string Account,
                                             string License,
                                            string CompanyCode, string UserName, string Password,
                                            string OrderIdentifier, string DocId,
                                            DocumentType DocType, decimal TotalAmount,
                                            decimal TotalTax)
        {

            BVSoftware.Avalara.AvaTaxResult result = new AvaTaxResult();

            avt.TaxService.PostTaxRequest ptr = new avt.TaxService.PostTaxRequest();
            ptr.CompanyCode = CompanyCode;
            ptr.DocId = DocId;
            ptr.DocType = ConvertDocType(DocType);
            ptr.DocDate = DateTime.Now;
            ptr.TotalAmount = TotalAmount;
            ptr.TotalTax = TotalTax;

            avt.TaxService.TaxSvc svc = GetTaxServiceProxy(URL, Account, License, UserName, Password);

            avt.TaxService.PostTaxResult ptres = svc.PostTax(ptr);


            if (ptres.ResultCode != avt.SeverityLevel.Success)
            {
                result.Success = false;
                result.Messages.Add("PostTax Failed");
                ApplyMessagesToResult(result, ptres.Messages);                
            }
            else
            {
                result.Success = true;
            }

            result.DocId = DocId;
            result.TotalAmount = TotalAmount;
            result.TotalTax = TotalTax;

            return result;
        }

        public static AvaTaxResult CommitTax(string URL, string Account,
                                            string License,
                                           string CompanyCode, string UserName, string Password,
                                           string OrderIdentifier, string DocId,
                                           DocumentType DocType)
        {                                
            BVSoftware.Avalara.AvaTaxResult result = new AvaTaxResult();

            avt.TaxService.CommitTaxRequest ctr = new avt.TaxService.CommitTaxRequest();
            ctr.CompanyCode = CompanyCode;
            ctr.DocId = DocId;
            ctr.DocType = ConvertDocType(DocType);

            avt.TaxService.TaxSvc svc = GetTaxServiceProxy(URL, Account, License, UserName, Password);

            avt.TaxService.CommitTaxResult ctres = svc.CommitTax(ctr);


            if (ctres.ResultCode != avt.SeverityLevel.Success)
            {
                result.Success = false;
                result.Messages.Add("CommitTax Failed");
                ApplyMessagesToResult(result, ctres.Messages);
            }
            else
            {
                result.Success = true;
            }

            return result;
        }

        public static AvaTaxResult CancelTax(string URL, string Account,
                                            string License,
                                           string CompanyCode, string UserName, string Password,
                                           string OrderIdentifier, string DocId,
                                           DocumentType DocType)
        {
            BVSoftware.Avalara.AvaTaxResult result = new AvaTaxResult();

            avt.TaxService.CancelTaxRequest ctr = new avt.TaxService.CancelTaxRequest();
            ctr.CompanyCode = CompanyCode;
            ctr.DocId = DocId;
            ctr.DocType = ConvertDocType(DocType);
            ctr.CancelCode = avt.TaxService.CancelCode.DocDeleted;

            avt.TaxService.TaxSvc svc = GetTaxServiceProxy(URL, Account, License, UserName, Password);

            avt.TaxService.CancelTaxResult ctres = svc.CancelTax(ctr);


            if (ctres.ResultCode != avt.SeverityLevel.Success)
            {
                result.Success = false;
                result.Messages.Add("CancelTax Failed");
                ApplyMessagesToResult(result, ctres.Messages);
            }
            else
            {
                result.Success = true;
            }

            return result;                                
        }

        internal static avt.TaxService.TaxSvc GetTaxServiceProxy(string Url, string Account, string License, string UserName, string Password)
        {
            avt.TaxService.TaxSvc svc = new global::Avalara.AvaTax.Adapter.TaxService.TaxSvc();

            svc.Profile.Client = "BVSoftwareAvalaraToolkit,6.0";
            if (Url != null)
            {
                svc.Configuration.Url = Url;
            }
            if (Account != null && Account.Length > 0)
            {
                svc.Configuration.Security.Account = Account;
            }
            if (License != null && License.Length > 0)
            {
                svc.Configuration.Security.License = License;
            }
            svc.Configuration.Security.UserName = UserName;
            //write as plain text
            svc.Configuration.Security.Password = Password;

            return svc;
        }

        internal static void ApplyMessagesToResult(AvaTaxResult result, avt.Messages messages)
        {
            if (messages != null)
            {                
                foreach (avt.Message item in messages)
                {
                    result.Messages.Add(item.Name + Environment.NewLine + item.Summary + Environment.NewLine + item.Details);
                }
            }
        }

        internal static avt.TaxService.DocumentType ConvertDocType(DocumentType localDoc)
        {
            switch (localDoc)
            {
                case DocumentType.PurchaseInvoice:
                    return avt.TaxService.DocumentType.PurchaseInvoice;
                case DocumentType.PurchaseOrder:
                    return avt.TaxService.DocumentType.PurchaseOrder;
                case DocumentType.ReturnInvoice:
                    return avt.TaxService.DocumentType.ReturnInvoice;
                case DocumentType.ReturnOrder:
                    return avt.TaxService.DocumentType.ReturnOrder;
                case DocumentType.SalesInvoice:
                    return avt.TaxService.DocumentType.SalesInvoice;
                case DocumentType.SalesOrder:
                    return avt.TaxService.DocumentType.SalesOrder;
            }
            return avt.TaxService.DocumentType.PurchaseOrder;
        }

        internal static avt.AddressService.Address ConvertBaseAddress(BaseAddress local)
        {
            avt.AddressService.Address result = new global::Avalara.AvaTax.Adapter.AddressService.Address();

            result.City = local.City;
            result.Country = local.Country;
            result.Line1 = local.Line1;
            result.Line2 = local.Line2;
            result.Line3 = local.Line3;
            result.PostalCode = local.PostalCode;
            result.Region = local.Region;            

            return result;
        }

        internal static avt.TaxService.Line ConvertLine(Line l)
        {
            avt.TaxService.Line result = new global::Avalara.AvaTax.Adapter.TaxService.Line();

            result.Amount = l.Amount;
            result.CustomerUsageType = l.CustomerUsageType;
            result.Description = l.Description;
            result.Discounted = l.Discounted;
            result.ExemptionNo = l.ExemptionNo;
            result.ItemCode = l.ItemCode;
            result.No = l.No;
            result.Qty = (double)l.Qty;
            result.Ref1 = l.Ref1;
            result.Ref2 = l.Ref2;
            result.RevAcct = l.RevAcct;
            result.TaxCode = l.TaxCode;
            result.TaxIncluded = false;            

            return result;
        }
    }
}
