using System;
using System.Net.Mail;
using System.Collections.Generic;

namespace MerchantTribe.Commerce.BusinessRules.OrderTasks
{	
	public class EmailOrder : BusinessRules.OrderTask
	{
        private string _ToEmail = "Customer";
        public string CustomEmail { get; set; }

        public EmailOrder(string toEmail)
        {
            CustomEmail = string.Empty;
            _ToEmail = toEmail;
        }

		public override bool Execute(OrderTaskContext context)
		{

            Content.HtmlTemplate t = context.MTApp.ContentServices.GetHtmlTemplateOrDefault(Content.HtmlTemplateType.NewOrder);

            string EmailSelection = _ToEmail;
            string toEmail = context.Order.UserEmail;
		
			switch (EmailSelection) {
				case "Admin":
                    toEmail = context.MTApp.CurrentRequestContext.CurrentStore.Settings.MailServer.EmailForNewOrder;
                    break;			
				case "Custom":
                    toEmail = CustomEmail;
                    break;
			}

			try {
				if (toEmail.Trim().Length > 0) {
                    
                    t = t.ReplaceTagsInTemplate(context.MTApp,context.Order, context.Order.ItemsAsReplaceable());
					
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
			return "B1BAE947-3F33-473f-8AFE-F27A6B9625D3";
		}

		public override string TaskName()
		{
			return "Email Order";
		}

		public override string StepName()
		{
			string result = string.Empty;
            result = "Email Order";
			if (result == string.Empty) {
				result = this.TaskName();
			}
			return result;
		}

		public override Task Clone()
		{
			return new EmailOrder("Customer");
		}

	}
}
