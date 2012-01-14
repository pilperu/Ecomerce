using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MerchantTribe.Commerce.Membership;
using MerchantTribe.Commerce.Content;
using MerchantTribe.Commerce.Utilities;
using MerchantTribe.Web.Logging;

namespace MerchantTribe.Commerce.BusinessRules.OrderTasks
{
    public class CreateUserAccountForNewCustomer : BusinessRules.OrderTask
    {

        public override bool Execute(OrderTaskContext context)
        {
            CustomerAccount u = context.MTApp.MembershipServices.Customers.FindByEmail(context.Order.UserEmail);
            if (u != null)
            {
                if (u.Bvin != string.Empty)
                {
                    return true;
                }
            }

            CustomerAccount n = new CustomerAccount();
            n.Email = context.Order.UserEmail;
            int length = WebAppSettings.PasswordMinimumLength;
            if (length < 8) length = 8;
            string newPassword = MerchantTribe.Web.PasswordGenerator.GeneratePassword(length);
            n.Password = newPassword;
            n.FirstName = context.Order.ShippingAddress.FirstName;
            n.LastName = context.Order.ShippingAddress.LastName;
                        
            if (context.MTApp.MembershipServices.CreateCustomer(n, n.Password))
            {
                // Update Addresses for Customer
                context.Order.BillingAddress.CopyTo(n.BillingAddress);
                context.Order.ShippingAddress.CopyTo(n.ShippingAddress);                
                context.MTApp.MembershipServices.UpdateCustomer(n);
                context.Order.CustomProperties.Add("bvsoftware", "allowpasswordreset", "1");

                // Email Password to Customer
                HtmlTemplate t = context.MTApp.ContentServices.GetHtmlTemplateOrDefault(HtmlTemplateType.ForgotPassword);                
                if (t != null)
                {
                    System.Net.Mail.MailMessage m;

                    List<IReplaceable> replacers = new List<IReplaceable>();
                    replacers.Add(n);
                    replacers.Add(new Replaceable("[[NewPassword]]", newPassword));
                    t = t.ReplaceTagsInTemplate(context.MTApp, replacers);

                    m = t.ConvertToMailMessage(n.Email);
                    
                    if (MailServices.SendMail(m, context.MTApp.CurrentStore) == false)
                    {
                        EventLog.LogEvent("Create Account During Checkout", "Failed to send email to new customer " + n.Email, EventLogSeverity.Warning);
                    }
                }
            }
            context.UserId = n.Bvin;
                                    
            return true;
        }

        public override bool Rollback(OrderTaskContext context)
        {            
            return true;
        }

        public override string TaskId()
        {
            return "1755C649-4C16-41A6-B5AE-5259067FFF0E";
        }

        public override string TaskName()
        {
            return "Create User Account for New Customer";
        }

        public override string StepName()
        {
            string result = string.Empty;
            result = "Create User Account for New Customer";
            if (result == string.Empty)
            {
                result = this.TaskName();
            }
            return result;
        }

        public override Task Clone()
        {
            return new CreateUserAccountForNewCustomer();
        }

    }
}

