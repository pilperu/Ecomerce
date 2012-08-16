using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MerchantTribeStore.Controllers.Shared;
using MerchantTribe.Commerce;
using MerchantTribe.Commerce.Orders;
using System.IO;
using System.Text;

namespace MerchantTribeStore.Controllers
{
    public class PayPalIpnController : BaseStoreController
    {        
        [HttpPost]
        public ActionResult Index()
        {
            string strFormValues = Request.Form.ToString();
            string strNewValue = string.Empty;
            string strResponse = string.Empty;

            System.Net.HttpWebRequest req;
            // Create the request back
            if (string.Compare(MTApp.CurrentStore.Settings.PayPal.Mode, "Live", true) == 0)
            {
                req = (System.Net.HttpWebRequest)System.Net.WebRequest.Create("https://www.paypal.com/cgi-bin/webscr");
            }
            else
            {
                req = (System.Net.HttpWebRequest)System.Net.WebRequest.Create("https://www.sandbox.paypal.com/cgi-bin/webscr");
            }


            // Set values for the request back
            req.Method = "POST";
            req.ContentType = "application/x-www-form-urlencoded";
            strNewValue = strFormValues + "&cmd=_notify-validate";
            req.ContentLength = strNewValue.Length;

            // Write the request back IPN strings
            StreamWriter stOut = new StreamWriter(req.GetRequestStream(), Encoding.ASCII);
            stOut.Write(strNewValue);
            stOut.Close();

            // Do the request to PayPal and get the response
            StreamReader stIn = new StreamReader(req.GetResponse().GetResponseStream());
            strResponse = stIn.ReadToEnd();
            stIn.Close();

            // Confirm whether the IPN was VERIFIED or INVALID. If INVALID, just ignore the IPN
            if ((strResponse == "VERIFIED"))
            {
                //Create the IpnTransaction                
                //if (Request.Form["payment_status"] != null)
                //{
                //    string payment_status = Request.Form["payment_status"];
                //    if (Request.Form["txn_id"] != null)
                //    {
                //        string transId = (string)Request.Form["txn_id"];
                //        if (payment_status == "Completed")
                //        {
                //            List<OrderTransaction> transPayments = MTApp.OrderServices.FindTransactionsByThirdPartyOrderIdFindTransactionsByThirdPartyTransactionId(transId);
                //            if (transPayments.Count > 0)
                //            {
                //                decimal paymentAmount = 0m;
                //                if (decimal.TryParse(Request.Form["mc_gross"], out paymentAmount))
                //                {
                //                    foreach (OrderTransaction payItem in transPayments)
                //                    {
                //                        if ((payItem.AmountCharged + paymentAmount) <= payItem.AmountAuthorized)
                //                        {
                //                            payItem.AmountCharged += paymentAmount;
                //                        }
                //                        payItem.CustomProperties.Remove(payItem.CustomProperties["status"]);
                //                        OrderPayment.Update(payItem);
                //                        Order order = Order.FindByBvin(payItem.OrderID);

                //                        MerchantTribe.Commerce.BusinessRules.OrderTaskContext context = new MerchantTribe.Commerce.BusinessRules.OrderTaskContext();
                //                        context.Order = order;
                //                        context.UserId = SessionManager.GetCurrentUserId();
                //                        OrderPaymentStatus previousPaymentStatus = order.PaymentStatus;
                //                        order.EvaluatePaymentStatus();
                //                        context.Inputs.Add("bvsoftware", "PreviousPaymentStatus", previousPaymentStatus.ToString());
                //                        MerchantTribe.Commerce.BusinessRules.Workflow.RunByName(context, MerchantTribe.Commerce.BusinessRules.WorkflowNames.PaymentChanged);

                //                        Order.Update(order);
                //                    }
                //                }
                //            }
                //            else
                //            {
                //                //check to see if we can find our order based on the auth id
                //                string authId = (string)Request.Form["auth_id"];
                //                transPayments = OrderPayment.FindByThirdPartyTransactionId(authId);
                //                if (transPayments.Count > 0)
                //                {
                //                    decimal paymentAmount = 0m;
                //                    if (decimal.TryParse(Request.Form["mc_gross"], out paymentAmount))
                //                    {
                //                        foreach (OrderPayment payItem in transPayments)
                //                        {
                //                            if ((payItem.AmountCharged + paymentAmount) <= payItem.AmountAuthorized)
                //                            {
                //                                payItem.AmountCharged += paymentAmount;
                //                            }
                //                            payItem.CustomProperties.Remove(payItem.CustomProperties["status"]);
                //                            OrderPayment.Update(payItem);
                //                            Order order = Order.FindByBvin(payItem.OrderID);

                //                            MerchantTribe.Commerce.BusinessRules.OrderTaskContext context = new MerchantTribe.Commerce.BusinessRules.OrderTaskContext();
                //                            context.Order = order;
                //                            context.UserId = SessionManager.GetCurrentUserId();
                //                            OrderPaymentStatus previousPaymentStatus = order.PaymentStatus;
                //                            order.EvaluatePaymentStatus();
                //                            context.Inputs.Add("bvsoftware", "PreviousPaymentStatus", previousPaymentStatus.ToString());
                //                            MerchantTribe.Commerce.BusinessRules.Workflow.RunByName(context, MerchantTribe.Commerce.BusinessRules.WorkflowNames.PaymentChanged);

                //                            Order.Update(order);
                //                        }
                //                    }
                //                }
                //            }
                //        }
                //        else if (payment_status == "Failed")
                //        {
                //            if (!FindAndMarkStatus(transId, "Failed"))
                //            {
                //                //check to see if we can find our order based on the auth id
                //                string authId = (string)Request.Form["auth_id"];
                //                FindAndMarkStatus(authId, "Failed");
                //            }
                //        }
                //        else if (payment_status == "Denied")
                //        {
                //            if (!FindAndMarkStatus(transId, "Denied"))
                //            {
                //                //check to see if we can find our order based on the auth id
                //                string authId = (string)Request.Form["auth_id"];
                //                FindAndMarkStatus(authId, "Denied");
                //            }
                //        }
                //        else if (payment_status == "Reversed")
                //        {
                //            if (!FindAndMarkStatus(transId, "Reversed"))
                //            {
                //                //check to see if we can find our order based on the auth id
                //                string authId = (string)Request.Form["auth_id"];
                //                FindAndMarkStatus(authId, "Reversed");
                //            }
                //        }
                //    }
                //}
            }
            return View();
        }

        private bool FindAndMarkStatus(string transId, string status)
        {
            
            //List<OrderTransaction> transPayments = MTApp.OrderServices.Transactions.FindByThirdPartyTransactionId(transId);
            //if (transPayments.Count > 0)
            //{
            //    foreach (OrderTransaction payItem in transPayments)
            //    {                    
            //        payItem.Success = false;                    
            //        MTApp.OrderServices.Transactions.Update(payItem);                    
            //        return true;
            //    }
            //}
            return false;
        }

    }
}
