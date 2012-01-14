
using System.Net.Mail;
using System;

namespace MerchantTribe.Commerce.BusinessRules.OrderTasks
{	
	public class EmailShippingInfo : BusinessRules.OrderTask
	{

        private string _ToEmail = "Customer";
        public EmailShippingInfo(string toEmail)
        {
            _ToEmail = toEmail;
        }

        public EmailShippingInfo()
        {

        }
		public override bool Execute(OrderTaskContext context)
		{
		    string EmailSelection = string.Empty;
            EmailSelection = _ToEmail;
			string toEmail = string.Empty;
			switch (EmailSelection) {
				case "Admin":
                    toEmail = context.MTApp.CurrentRequestContext.CurrentStore.Settings.MailServer.EmailForNewOrder;
                    break;
				case "Customer":
					toEmail = context.Order.UserEmail;
                    break;
				default:
					toEmail = context.Order.UserEmail;
					EmailSelection = "Customer";
                    break;
			}
		
			try {
				if (toEmail.Trim().Length > 0) {

                    Content.HtmlTemplate t = context.MTApp.ContentServices.GetHtmlTemplateOrDefault(Content.HtmlTemplateType.OrderShipment);
                    t = t.ReplaceTagsInTemplate(context.MTApp, context.Order, context.Order.PackagesAsReplaceable());

					System.Net.Mail.MailMessage m = new System.Net.Mail.MailMessage();
					m = t.ConvertToMailMessage(toEmail);
					if (m != null) {
						Utilities.MailServices.SendMail(m, context.MTApp.CurrentStore);
					}
				}
			}
			catch (Exception ex) {
				EventLog.LogEvent(ex);
			}

			return true;
		}
	
		public override bool Rollback(OrderTaskContext context)
		{
			return true;
		}

		public override string TaskId()
		{
			return "7817bfd9-1075-4bac-9189-3c76c3ec17a6";
		}

		public override string TaskName()
		{
			return "Email Shipping Info";
		}

		public override string StepName()
		{
			string result = string.Empty;
            result = "Send Shipping Info";
			if (result == string.Empty) {
				result = this.TaskName();
			}
			return result;
		}

		public override Task Clone()
		{
			return new EmailShippingInfo();
		}

	}
}
