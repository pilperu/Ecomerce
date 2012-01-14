using System;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MerchantTribe.Commerce.Utilities
{

	public class MailServices
	{
        private Accounts.Store _store = new Accounts.Store();

        public MailServices(Accounts.Store currentStore)
        {
            _store = currentStore;
        }

        public static bool SendMail(System.Net.Mail.MailMessage m, Accounts.Store fromStore)
        {
            MailServices sender = new MailServices(fromStore);
            return sender.SendMail(m);
        }
		public bool SendMail(System.Net.Mail.MailMessage m)
		{
			return SendMail(m, false);
		}

		public bool SendMail(System.Net.Mail.MailMessage m, bool async)
		{
			bool result = false;

			try {

                string _server = WebAppSettings.ApplicationMailServer;
                string _user = WebAppSettings.ApplicationMailServerUsername;
                string _password = WebAppSettings.ApplicationMailServerPassword;
                bool _useAuth = true;
                string _port = WebAppSettings.ApplicationMailServerPort;
                bool _useSSL = WebAppSettings.ApplicationMailServerSSL;
                
                
                    if (_store.Settings.MailServer.UseCustomMailServer)
                    {
                        _server = _store.Settings.MailServer.HostAddress;
                        _user = _store.Settings.MailServer.Username;
                        _password = _store.Settings.MailServer.Password;
                        _useAuth = _store.Settings.MailServer.UseAuthentication;
                        _port = _store.Settings.MailServer.Port;
                        _useSSL = _store.Settings.MailServer.UseSsl;
                    }
                

                SmtpClient server = new SmtpClient();
                server.Host = _server;

                NetworkCredential auth = new NetworkCredential(_user,_password);

                if (_useAuth)
                {
                    server.UseDefaultCredentials = false;
                    server.Credentials = auth;
                }
                
                if (_port.Trim() != string.Empty)
                {
                    int altPort = 25;
                    if (int.TryParse(_port, out altPort))
                    {
                        server.Port = altPort;
                    }
                }

                server.EnableSsl = _useSSL;

                // Body Replacement
                if (m.IsBodyHtml)
                {
                    m.Body = m.Body.Replace("</body>", "<div style=\"margin:0.25em;border-style:solid;border-width:1px 0 1px 3px;padding:0.5em 1em;font-face:arial;font-size:11px;color:#333;background-color: #fffffd;border-color: #B3B300;\">Get a <a href=\"http://www.MerchantTribe.com\">Free Shopping Cart by joining MerchantTribe</a> today!</div>");
                }
                else
                {
                    m.Body += System.Environment.NewLine + System.Environment.NewLine + "Get a Free Shopping Cart by joining MerchantTribe.com!" + System.Environment.NewLine;
                }

                if ( WebAppSettings.ApplicationMailServerAsync)
                {
                    string userState = "Mail Sent";
                    server.SendAsync(m, userState);
                }
                else
                {
                    server.Send(m);
                }

				result = true;
			}
			catch (Exception Ex) {
				result = false;
				System.Diagnostics.Trace.Write(Ex.Message);
				EventLog.LogEvent(Ex);
			}

			return result;
		}

		public static string MailToLink(string email, string subject, string body)
		{
			return MailToLink(email, subject, body, email);
		}

		public static string MailToLink(string email, string subject, string body, string displayText)
		{
			string result = "<a href=\"mailto:";
			result += email;
			result += "?subject=" + subject.Replace(" ", "%20");
			result += "&body=" + body.Replace(" ", "%20");
			result += "\">" + displayText + "</a>";
			return result;
		}

        public static string RenderEmailHtmlStyles()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<style  type=\"text/css\">");
            sb.Append("body {margin: 0px;padding:0px;font: 9pt \"Lucida Grande\",Arial,Helvetica, sans-serif;color: #000;background-color:#fff;}   ");
            sb.Append("fieldset { border: solid 1px #ccc}");
            sb.Append("th {text-align:left;}");
            sb.Append(".fl, .fleft {float:left;}");
            sb.Append(".fr, .fright {float:right;}");
            sb.Append(".l, .left,#cart-table td.l, #cart-table td.left, #cart-table th.left, #cart-table th.l {text-align:left;}");
            sb.Append(".r, .right {text-align:right;}");
            sb.Append(".c, .center {text-align:center;}");
            sb.Append("h1 {text-align:left;color:#333;	margin:0 0 15px 0;padding:0;font-size:18pt;letter-spacing:-1px;}");
            sb.Append("h2 {text-align:left;font-size:9pt;font-weight:bold;padding:5px 0;text-align:left;line-height:16px;color:#666;margin:0;border-bottom:1px solid #ccc;}");
            sb.Append("h3 {font-size:17px;font-weight:bold;color:#000;padding:0;margin:0;}");
            sb.Append("h4 {text-align:left;font-size:9pt;font-weight:bold;padding:5px 0;text-align:left;line-height:16px;color:#666;margin:0;}");
            sb.Append(".tiny {font-size:9px;color:#666;}");
            sb.Append("a:link, a:visited {color: #06c;text-decoration: none;}");
            sb.Append("a:hover {text-decoration:underline;}");
            sb.Append(".order-details{margin:20px;}");
            sb.Append("#cart-addresses {margin:18px 0;}");
            sb.Append("#cart-addresses #billing-address {width:345px;float:left;}");
            sb.Append("#cart-addresses #shipping-address {width:214px;float:right;}");
            sb.Append("#cart-table {width:100%;margin:9px 0 18px 0;}");
            sb.Append("#cart-table th {text-align:right;font-weight:bold;padding:0 9px;border-bottom:solid 1px #ccc;}");
            sb.Append("#cart-table td {padding:9px 7px;text-align:right;}");
            sb.Append(".cart-totals {border-top:solid 1px #ccc;font-weight:bold;}");
            sb.Append(".cart-totals .total {display:block;width:300px;height:18px;float:right;");
            sb.Append("margin:9px 0 0 0;text-align:left;clear:both;}");
            sb.Append(".cart-totals .total label {font-size:14px; line-height:1.286em;color:#999;text-align:right;");
            sb.Append("display:block;width:178px;float:left;}");
            sb.Append(".cart-totals .total span {font-size:14px;line-height:1.286em;text-align:right;");
            sb.Append("display:block;width:120px;float:right;color:#999;}                                    ");
            sb.Append(".cart-totals .total #cart-grandtotal {font-size:18px;line-height:1em;text-align:right;");
            sb.Append("display:block;width:120px;float:right;color:#000;}");
            sb.Append("</style>");
            return sb.ToString();
        }

        //public static string RenderStoreLogoLink(Accounts.Store s)
        //{
        //    StringBuilder sb = new StringBuilder();

        //    string name = MerchantTribe.Web.HtmlSanitizer.MakeHtmlSafe(s.StoreName);
        //    sb.Append("<a href=\"");
        //    sb.Append(s.RootUrl());
        //    sb.Append("\" title=\"");
        //    sb.Append(name);
        //    sb.Append("\"><img src=\"");
        //    sb.Append(MerchantTribe.Commerce.Storage.ImageStorage.StoreLogoUrl(s.Id, s.LogoRevision, s.LogoImage, false));
        //    sb.Append("\" alt=\"");
        //    sb.Append(name);
        //    sb.Append("\" border=\"0\" /></a>");

        //    return sb.ToString();
        //}

        //public static void SendAdminReceipt(Orders.Order o)
        //{
        //    if (o != null)
        //    {
        //        Accounts.Store s = Accounts.StoreManager.FindById(o.StoreId);
        //        string ToEmail = string.Empty;

        //        List<Accounts.UserAccount> accounts = Accounts.UserAccountManager.FindByStoreId(s.Id);
        //        if (accounts != null)
        //        {
        //            if (accounts.Count > 0)
        //            {
        //                ToEmail = accounts[0].Email;
        //            }
        //        }

        //        if (s != null && ToEmail != string.Empty)
        //        {
        //            MailMessage m = new MailMessage(WebAppSettings.ApplicationEmail, ToEmail);
        //            m.Subject = "Order " + o.OrderNumber + " Admin Receipt for " + s.FriendlyName;

        //            StringBuilder sb = new StringBuilder();

        //            sb.Append("<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">");
        //            sb.Append("<html xmlns=\"http://www.w3.org/1999/xhtml\" >");
        //            sb.Append("<head><title>Email</title>");
        //            sb.Append(RenderEmailHtmlStyles());
        //            sb.Append("</head><body><div class=\"order-details\">");
        //            sb.Append(RenderStoreLogoLink(s));
        //            sb.Append("<h1>Receipt for Order " + o.OrderNumber + "</h1>");
        //            if (o.DateReceivedUtc.HasValue)
        //            {
        //                sb.Append("<span class=\"tiny\">");
        //                sb.Append(MerchantTribe.Web.Dates.FriendlyLocalDateFromUtc(o.DateReceivedUtc.Value));
        //                sb.Append("</span>");
        //            }

        //            sb.Append(UI.HtmlRendering.RenderOrderQuickDetails(o, true));
        //            sb.Append("</div></body></html>");

        //            m.IsBodyHtml = true;
        //            m.Body = sb.ToString();

        //            Utilities.MailServices.SendMail(m);
        //        }
        //    }
        //}
        //public static void SendReceipt(Orders.Order o)
        //{
        //    if (o != null)
        //    {
        //        Accounts.Store s = Accounts.StoreManager.FindById(o.StoreId);
        //        if (s != null)
        //        {
        //            MailMessage m = new MailMessage(EcommrcSettings.ApplicationEmail, o.CustomerEmail);
        //            m.Subject = "Order " + o.OrderNumber + " Receipt for " + s.FriendlyName;

        //            StringBuilder sb = new StringBuilder();

        //            sb.Append("<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">");
        //            sb.Append("<html xmlns=\"http://www.w3.org/1999/xhtml\" >");
        //            sb.Append("<head><title>Email</title>");
        //            sb.Append(RenderEmailHtmlStyles());
        //            sb.Append("</head><body><div class=\"order-details\">");
        //            sb.Append(RenderStoreLogoLink(s));
        //            sb.Append("<h1>Receipt for Order " + o.OrderNumber + "</h1>");
        //            if (o.DateReceivedUtc.HasValue)
        //            {
        //                sb.Append("<span class=\"tiny\">");
        //                sb.Append(MerchantTribe.Web.Dates.FriendlyLocalDateFromUtc(o.DateReceivedUtc.Value));
        //                sb.Append("</span>");
        //            }

        //            sb.Append(UI.HtmlRendering.RenderOrderQuickDetails(o, false));
        //            sb.Append("</div></body></html>");

        //            m.IsBodyHtml = true;
        //            m.Body = sb.ToString();

        //            Utilities.MailServices.SendMail(m);
        //        }
        //    }

        //}
        //public static void SendShippingReceipt(Orders.Order o)
        //{
        //    if (o != null)
        //    {
        //        Accounts.Store s = Accounts.StoreManager.FindById(o.StoreId);
        //        if (s != null)
        //        {
        //            MailMessage m = new MailMessage(EcommrcSettings.ApplicationEmail, o.CustomerEmail);
        //            m.Subject = "Order " + o.OrderNumber + " Has Shipped for " + s.FriendlyName;

        //            StringBuilder sb = new StringBuilder();

        //            sb.Append("<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">");
        //            sb.Append("<html xmlns=\"http://www.w3.org/1999/xhtml\" >");
        //            sb.Append("<head><title>Order Has Shipped</title>");
        //            sb.Append(RenderEmailHtmlStyles());
        //            sb.Append("</head><body><div class=\"order-details\">");
        //            sb.Append(RenderStoreLogoLink(s));
        //            sb.Append("<h1>Order " + o.OrderNumber + " Has Shipped</h1>");
        //            if (o.DateReceivedUtc.HasValue)
        //            {
        //                sb.Append("<span class=\"tiny\">");
        //                sb.Append(MerchantTribe.Web.Dates.FriendlyLocalDateFromUtc(o.DateReceivedUtc.Value));
        //                sb.Append("</span>");
        //            }

        //            sb.Append(UI.HtmlRendering.RenderOrderQuickDetails(o, false));
        //            sb.Append("</div></body></html>");

        //            m.IsBodyHtml = true;
        //            m.Body = sb.ToString();

        //            Utilities.MailServices.SendMail(m);
        //        }
        //    }

        //}

        private static string RenderEmailTableRow(string label, string value)
        {
            return "<tr><td align=\"right\">" + System.Web.HttpUtility.HtmlEncode(label) +
                ":</td><td><strong>" + System.Web.HttpUtility.HtmlEncode(value) + "</strong></td></tr>";
        }

        private static string RenderEmailTableRowLink(string label, string value, string linkValue)
        {
            return "<tr><td align=\"right\">" + System.Web.HttpUtility.HtmlEncode(label) +
                "</td><td><strong><a href=\"" + linkValue + "\">" + System.Web.HttpUtility.HtmlEncode(value) + "</a></strong></td></tr>";
        }

        public static void SendAccountInformation(Accounts.UserAccount u, Accounts.Store s)
        {
            Accounts.Store tempStore = new Accounts.Store();
            MailServices sender = new MailServices(tempStore);

            if (u == null || s == null) return;

            string fromEmail = WebAppSettings.ApplicationEmail;
            if (WebAppSettings.IsIndividualMode)
            {
                fromEmail = s.Settings.MailServer.FromEmail;
            }
            MailMessage m = new MailMessage(fromEmail, u.Email);
            m.Subject = s.StoreName + " Account Reminder Information";

            StringBuilder sb = new StringBuilder();

            sb.Append("<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">");
            sb.Append("<html xmlns=\"http://www.w3.org/1999/xhtml\" >");
            sb.Append("<head><title>BV Commerce Account Information Reminder</title>");
            sb.Append(RenderEmailHtmlStyles());
            sb.Append("</head><body><div style=\"padding:20px;\">");
            sb.Append("<h1>Account Information</h1>");
            sb.Append("<p>Thank you for creating a MerchantTribe store. Your account information appears below:</p>");
            sb.Append("<table border=\"0\" cellspacing=\"0\" cellpadding=\"3\">");
            sb.Append(RenderEmailTableRow("Email", u.Email));
            // Passwords are now hashed so we can't send them.
            //sb.Append(RenderEmailTableRow("Password", u.Password));
            sb.Append(RenderEmailTableRow("Store Name", s.StoreName));
            sb.Append(RenderEmailTableRowLink("Store URL", s.RootUrl(), s.RootUrl()));
            sb.Append(RenderEmailTableRowLink("Store Admin URL", s.RootUrl() + "admin", s.RootUrl() + "admin"));
            sb.Append("</table>");
            sb.Append("&nbsp;<br />");
            sb.Append("&nbsp;<br />");
            sb.Append("</div></body></html>");

            m.IsBodyHtml = true;
            m.Body = sb.ToString();

            sender.SendMail(m);
        }
        public static void SendAdminUserResetLink(Accounts.UserAccount u, Accounts.Store s)
        {
            Accounts.Store tempStore = new Accounts.Store();
            MailServices sender = new MailServices(tempStore);

            if (u == null || s == null) return;


            MailMessage m = new MailMessage(WebAppSettings.ApplicationEmail, u.Email);
            m.Subject = "BV Commerce Password Reset Instructions";

            StringBuilder sb = new StringBuilder();

            sb.Append("<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">");
            sb.Append("<html xmlns=\"http://www.w3.org/1999/xhtml\" >");
            sb.Append("<head><title>BV Commerce Password Reset Instructions</title>");
            sb.Append(RenderEmailHtmlStyles());
            sb.Append("</head><body><div style=\"padding:20px;\">");
            sb.Append("<h1>Password Reset Instructions</h1>");
            sb.Append("<p>Your Reset Key = " + u.ResetKey + "</p>");
            sb.Append("<p>Use the link below to reset your password.</p>");
            sb.Append("<p><a href=\"" + s.RootUrlSecure() + "adminaccount/ResetPassword2?email=" + u.Email + "&resetkey=" + u.ResetKey + "\">");
            sb.Append("Click Here to Reset Your Password</a></p>");            
            sb.Append("&nbsp;<br />");
            sb.Append("&nbsp;<br />");
            sb.Append("</div></body></html>");

            m.IsBodyHtml = true;
            m.Body = sb.ToString();

            sender.SendMail(m);
        }

        public static void SendCustomDomainRequest(string name, string email, string phone, string domain, string ownsDomain, string hasSsl)
        {
            Accounts.Store tempStore = new Accounts.Store();
            MailServices sender = new MailServices(tempStore);

            MailMessage m = new MailMessage(email, WebAppSettings.SuperAdminEmail);
            m.Subject = "BV Hosted Custom Domain Request | Contact Form";

            StringBuilder sb = new StringBuilder();

            sb.Append("<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">");
            sb.Append("<html xmlns=\"http://www.w3.org/1999/xhtml\" >");
            sb.Append("<head><title>New Hosted Signup Lead</title>");
            sb.Append(RenderEmailHtmlStyles());
            sb.Append("</head><body><div style=\"padding:20px;\">");
            sb.Append("<h1>Custom Domain Request</h1>");
            sb.Append("<table border=\"0\" cellspacing=\"0\" cellpadding=\"3\">");
            sb.Append(RenderEmailTableRow("Domain:", domain));
            sb.Append(RenderEmailTableRow("Email:", email));
            sb.Append(RenderEmailTableRow("Phone:", phone));
            sb.Append(RenderEmailTableRow("Owns Domain:",ownsDomain));
            sb.Append(RenderEmailTableRow("Has SSL:", hasSsl));
            sb.Append("</table>");
            sb.Append("&nbsp;<br />");
            sb.Append("&nbsp;<br />");
            sb.Append("</div></body></html>");

            m.IsBodyHtml = true;
            m.Body = sb.ToString();

            sender.SendMail(m);
        }

        public static void SendLeadAlert(Accounts.UserAccount u, Accounts.Store s)
        {
            Accounts.Store tempStore = new Accounts.Store();
            MailServices sender = new MailServices(tempStore);

            if (u == null || s == null) return;


            MailMessage m = new MailMessage(WebAppSettings.ApplicationEmail, WebAppSettings.SuperAdminEmail);
            

            if (s.PlanId == 0)
            {
                m.Subject = "BV Hosted FREE Signup Lead | Contact Form";                
            }
            else
            {
                m.Subject = "BV Hosted PAID PLAN signup | Contact Form";
            }

            if (s.Settings.LeadSource == "PayPalOffer")
            {
                m.Subject += " | from PayPal Offer";
            }

            StringBuilder sb = new StringBuilder();

            sb.Append("<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">");
            sb.Append("<html xmlns=\"http://www.w3.org/1999/xhtml\" >");
            sb.Append("<head><title>New Hosted Signup Lead</title>");
            sb.Append(RenderEmailHtmlStyles());
            sb.Append("</head><body><div style=\"padding:20px;\">");
            sb.Append("<h1>New Hosted Store Signup</h1>");
            sb.Append("<table border=\"0\" cellspacing=\"0\" cellpadding=\"3\">");
            sb.Append(RenderEmailTableRow("Email", u.Email));
            sb.Append(RenderEmailTableRow("Store Name", s.StoreName));
            sb.Append(RenderEmailTableRowLink("Store URL", s.RootUrl(), s.RootUrl()));
            sb.Append(RenderEmailTableRowLink("Store Admin URL", s.RootUrl() + "admin", s.RootUrl() + "admin"));
            sb.Append("</table>");
            sb.Append("&nbsp;<br />");
            sb.Append("&nbsp;<br />");
            sb.Append("</div></body></html>");

            m.IsBodyHtml = true;
            m.Body = sb.ToString();

            sender.SendMail(m);

        }

        public static void SendPlanUpgradeAlert(Accounts.UserAccount u, Accounts.Store s)
        {
            Accounts.Store tempStore = new Accounts.Store();
            MailServices sender = new MailServices(tempStore);

            if (u == null || s == null) return;


            MailMessage m = new MailMessage(WebAppSettings.ApplicationEmail, WebAppSettings.SuperAdminEmail);

            m.Subject = "BV Hosted | Plan Upgrade to " + s.PlanName;

            if (s.Settings.LeadSource == "PayPalOffer")
            {
                m.Subject += " | from PayPal Offer";
            }

            StringBuilder sb = new StringBuilder();

            sb.Append("<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">");
            sb.Append("<html xmlns=\"http://www.w3.org/1999/xhtml\" >");
            sb.Append("<head><title>New Hosted Plan Upgrade</title>");
            sb.Append(RenderEmailHtmlStyles());
            sb.Append("</head><body><div style=\"padding:20px;\">");
            sb.Append("<h1>New Hosted Plan Upgrade</h1>");
            sb.Append("<table border=\"0\" cellspacing=\"0\" cellpadding=\"3\">");
            sb.Append(RenderEmailTableRow("Email", u.Email));
            sb.Append(RenderEmailTableRow("Store Name", s.StoreName));
            sb.Append(RenderEmailTableRowLink("Store URL", s.RootUrl(), s.RootUrl()));
            sb.Append(RenderEmailTableRowLink("Store Admin URL", s.RootUrl() + "admin", s.RootUrl() + "admin"));
            sb.Append(RenderEmailTableRow("Upgraded to Plan", s.PlanName));
            sb.Append(RenderEmailTableRow("Bill Day of Month", s.CurrentPlanDayOfMonth.ToString()));
            sb.Append(RenderEmailTableRow("Bill Rate", s.CurrentPlanRate.ToString("c")));
            sb.Append(RenderEmailTableRow("Percentage", s.CurrentPlanPercent.ToString()));
            sb.Append("</table>");
            sb.Append("&nbsp;<br />");
            sb.Append("&nbsp;<br />");
            sb.Append("</div></body></html>");

            m.IsBodyHtml = true;
            m.Body = sb.ToString();

            sender.SendMail(m);
        }

        public static void SendPlanDowngradeAlert(Accounts.UserAccount u, Accounts.Store s)
        {
            if (u == null || s == null) return;


            MailMessage m = new MailMessage(WebAppSettings.ApplicationEmail, WebAppSettings.SuperAdminEmail);

            m.Subject = "BV Hosted | Plan Downgrade to " + s.PlanName;

            if (s.Settings.LeadSource == "PayPalOffer")
            {
                m.Subject += " | from PayPal Offer";
            }

            StringBuilder sb = new StringBuilder();

            sb.Append("<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">");
            sb.Append("<html xmlns=\"http://www.w3.org/1999/xhtml\" >");
            sb.Append("<head><title>New Hosted Plan Downgrade</title>");
            sb.Append(RenderEmailHtmlStyles());
            sb.Append("</head><body><div style=\"padding:20px;\">");
            sb.Append("<h1>New Hosted Plan Downgrade</h1>");
            sb.Append("<table border=\"0\" cellspacing=\"0\" cellpadding=\"3\">");
            sb.Append(RenderEmailTableRow("Email", u.Email));
            sb.Append(RenderEmailTableRow("Store Name", s.StoreName));
            sb.Append(RenderEmailTableRowLink("Store URL", s.RootUrl(), s.RootUrl()));
            sb.Append(RenderEmailTableRowLink("Store Admin URL", s.RootUrl() + "admin", s.RootUrl() + "admin"));
            sb.Append(RenderEmailTableRow("Downgraded to Plan", s.PlanName));
            sb.Append(RenderEmailTableRow("Bill Day of Month", s.CurrentPlanDayOfMonth.ToString()));
            sb.Append(RenderEmailTableRow("Bill Rate", s.CurrentPlanRate.ToString("c")));
            sb.Append(RenderEmailTableRow("Percentage", s.CurrentPlanPercent.ToString()));
            sb.Append("</table>");
            sb.Append("&nbsp;<br />");
            sb.Append("&nbsp;<br />");
            sb.Append("</div></body></html>");

            m.IsBodyHtml = true;
            m.Body = sb.ToString();

            Accounts.Store tempStore = new Accounts.Store();
            MailServices sender = new MailServices(tempStore);
            sender.SendMail(m);

        }
	}

}
