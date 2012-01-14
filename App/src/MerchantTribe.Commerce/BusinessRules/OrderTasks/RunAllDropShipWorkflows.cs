using System.Collections.Generic;
using System.Collections.ObjectModel;
using MerchantTribe.Web.Logging;

namespace MerchantTribe.Commerce.BusinessRules.OrderTasks
{	
	public class RunAllDropShipWorkflows : BusinessRules.OrderTask
	{

		public override Task Clone()
		{
			return new RunAllDropShipWorkflows();
		}

		public override bool Execute(OrderTaskContext context)
		{
			Collection<string> manufacturers = new Collection<string>();
			Collection<string> vendors = new Collection<string>();
			foreach (Orders.LineItem item in context.Order.Items) {

					
						if (item.ShipFromMode == Shipping.ShippingMode.ShipFromManufacturer) {
							if (item.ShipFromNotificationId != string.Empty) {
								if (!manufacturers.Contains(item.ShipFromNotificationId)) {
									manufacturers.Add(item.ShipFromNotificationId);
								}
							}
							else {
								EventLog.LogEvent("RunAllDropShipWorkflows.bv", "Item with sku " + item.ProductSku + " is marked as Ship From Manufacturer, but contains no Manufacturer Id", EventLogSeverity.Warning);
							}
						}
						else if (item.ShipFromMode == Shipping.ShippingMode.ShipFromVendor) {
							if (item.ShipFromNotificationId != string.Empty) {
								if (!vendors.Contains(item.ShipFromNotificationId)) {
									vendors.Add(item.ShipFromNotificationId);
								}
							}
							else {
								EventLog.LogEvent("RunAllDropShipWorkflows.bv", "Item with sku " + item.ProductSku + " is marked as Ship From Vendor, but contains no Vendor Id", EventLogSeverity.Warning);
							}
						}
					
								
			}
			
			foreach (string item in manufacturers) {
                Contacts.VendorManufacturer mfg = context.MTApp.ContactServices.Manufacturers.Find(item);
				if (mfg != null) {
					if (mfg.Bvin != string.Empty) {
						bool success = false;
                        //if (WebAppSettings.SendDropShipNotificationsThroughWebService) {
                        //    itemsNeedToBeDropShipped = true;
                        //    success = true;
                        //}
                        //else {
                        if (!SendEmail(context.MTApp, mfg, context.Order))
                        {
								Orders.OrderNote n = new Orders.OrderNote();
                                n.IsPublic = false;
								n.Note = "Drop shipper notices for " + mfg.DisplayName + " were not able to send correctly.";
								context.Order.Notes.Add(n);
                                context.MTApp.OrderServices.Orders.Upsert(context.Order);
							}
							else {
								success = true;
							}
						//}

						if (success) {
							Orders.OrderNote n = new Orders.OrderNote();
                            n.IsPublic = false;
							n.Note = "Drop shipper notices for " + mfg.DisplayName + " were sent successfully.";
							context.Order.Notes.Add(n);
                            context.MTApp.OrderServices.Orders.Upsert(context.Order);
						}
					}
				}
			}

			foreach (string item in vendors) {
                Contacts.VendorManufacturer vendor = context.MTApp.ContactServices.Vendors.Find(item);
				if (vendor != null) {
					if (vendor.Bvin != string.Empty) {
						bool success = false;
                        //if (WebAppSettings.SendDropShipNotificationsThroughWebService) {
                        //    itemsNeedToBeDropShipped = true;
                        //    success = true;
                        //}
                        //else {
                        if (!SendEmail(context.MTApp, vendor, context.Order))
                        {
								Orders.OrderNote n = new Orders.OrderNote();
								n.IsPublic = false;
								n.Note = "Drop shipper notices for " + vendor.DisplayName + " were not able to send correctly.";
								context.Order.Notes.Add(n);
                                context.MTApp.OrderServices.Orders.Upsert(context.Order);
							}
							else {
								success = true;
							}
						//}

						if (success) {
							Orders.OrderNote n = new Orders.OrderNote();
							n.IsPublic = false;
							n.Note = "Drop shipper notices for " + vendor.DisplayName + " were sent successfully.";
							context.Order.Notes.Add(n);
                            context.MTApp.OrderServices.Orders.Upsert(context.Order);
						}
					}
				}
			}

            //if (itemsNeedToBeDropShipped) {
            //    return LogDropShipNotification(context.Order);
            //}
			return true;
		}

		public override bool Rollback(OrderTaskContext context)
		{
			return true;
		}

		public override string TaskId()
		{
			return "995E482F-8BDC-47E2-926F-D7248553035F";
		}

		public override string TaskName()
		{
			return "Run All Dropship Notifications";
		}

		private bool SendEmail(MerchantTribeApplication app, Contacts.VendorManufacturer vendorOrManufacturer, Orders.Order order)
		{
			string toEmail = vendorOrManufacturer.EmailAddress;

            Content.HtmlTemplate t = null;
			string templateBvin = vendorOrManufacturer.DropShipEmailTemplateId;
			if (templateBvin != string.Empty) {
                long templateId = 0;
                long.TryParse(templateBvin, out templateId);
                t = app.ContentServices.HtmlTemplates.Find(templateId);
			}
            if (t == null)
            {
                t = app.ContentServices.GetHtmlTemplateOrDefault(Content.HtmlTemplateType.DropShippingNotice);
            }

			if (toEmail.Trim().Length > 0) {

                List<Content.IReplaceable> replacers = new List<Content.IReplaceable>();
                replacers.Add(order);
                replacers.Add(vendorOrManufacturer);
                t = t.ReplaceTagsInTemplate(app, replacers, order.ItemsAsReplaceable());

				System.Net.Mail.MailMessage m = new System.Net.Mail.MailMessage();
				if (vendorOrManufacturer != null) {
					m = t.ConvertToMailMessage(toEmail);
				}
				if (m != null) {
					return Utilities.MailServices.SendMail(m, app.CurrentStore);
				}
			}
			return false;
		}
      
	}
}
