using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MerchantTribe.Commerce.Accounts;
using System.Text;
using System.Configuration;
using MerchantTribe.Commerce.Accounts.Billing;

namespace MerchantTribeStore
{

    public partial class BVAdmin_Account : BaseAdminPage
    {
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (MerchantTribe.Commerce.WebAppSettings.IsIndividualMode)
            {
                this.pnlBilling.Visible = false;
            }
            
            Store s = MTApp.CurrentStore;
            UserAccount u = GetCorrectUser();
            PopulatePage(u);            
        }


        private UserAccount GetCorrectUser()
        {
            UserAccount u = CurrentUser;

            if (u != null)
            {
                if (u.Status == UserAccountStatus.SuperUser)
                {
                    // don't use current user, get the owner of the store instead
                    List<UserAccount> users = MTApp.AccountServices.FindAdminUsersByStoreId(MTApp.CurrentStore.Id);
                    if (users != null)
                    {
                        if (users.Count > 0)
                        {
                            return users[0];
                        }
                    }
                }
            }

            return u;
        }

        private void PopulatePage(UserAccount u)
        {
            this.lblEmail.Text = u.Email;
            this.lblMemberSince.Text = u.DateCreated.ToLocalTime().ToShortDateString();

            LoadSubscriptionInformation();            
        }

        private void LoadSubscriptionInformation()
        {
            var billingManager = new MerchantTribe.Commerce.Accounts.Billing.BillingManager(this.MTApp);

            string customerId = this.MTApp.CurrentStore.StripeCustomerId;
            if (customerId.Trim().Length < 1)
            {
                this.pnlBilling.Visible = false;
                this.litStores.Text = "Your store is on the free trial plan. No paid subscription is currently active for this store.<br />";
                this.litStores.Text += "&nbsp;<br />";
                this.litStores.Text += "If you would like to cancel your store completely: <a href=\"CancelStore.aspx\">View Cancel Options<br />";
                this.litStores.Text += "&nbsp;<br />";
                this.litStores.Text += "<a href=\"ChangePlan.aspx\">Click Here to Upgrade to a Paid Plan</a> to get support, more products and more features.";
            }
            else
            {
                this.pnlBilling.Visible = true;
                var customer = billingManager.GetCustomerInformation(customerId);
                if (customer.Success)
                {
                    this.litStores.Text = customer.Description + "<br />";
                    this.litStores.Text += "<b>Email:</b> " + customer.Email + "<br />";                    
                    if (customer.HasCard)
                    {
                        this.litStores.Text += "<b>Card:</b> Credit Card is On File<br />";
                    }
                    else
                    {
                        this.litStores.Text += "<b>Card:</b> ERROR - No active card on file<br />";
                    }
                    this.litStores.Text += "<b>Plan:</b> " + customer.PlanName + " (" + customer.PlanAmount.ToString("C") + " per month)<br />";
                    //this.litStores.Text += "<b>Next Charge:</b> " + customer.NextCharge + "<br />";
                    this.litStores.Text += "<b>Plan Status:</b> " + customer.PlanStatus + "<br />";

                    this.litStores.Text += "&nbsp;<br />";
                    this.litStores.Text += "If you would like to cancel your store completely: <a href=\"CancelStore.aspx\">View Cancel Options<br />";
                    this.litStores.Text += "&nbsp;<br />";
                    this.litStores.Text += "<a href=\"ChangePlan.aspx\">Click Here to Upgrade or Downgrade your Paid Plan</a> to change, product limits and features.<br />";
                }
                else
                {
                    this.litStores.Text = "Unable to locate your subscription. Check with support for more information.";
                }
            }            
        }
        
        protected void btnUpdateCreditCard_Click(object sender, EventArgs e)
        {
            if (!this.CreditCardInput1.IsValid())
            {
                this.MessageBox1.ShowWarning("Please check your credit card information is valid before attempting to update.");
                return;
            }

            UserAccount u = GetCorrectUser();
            
            var billManager = new BillingManager(this.MTApp);

            var updateRequest = new UpdateCustomerRequest();
            updateRequest.CreditCard = this.CreditCardInput1.GetCardData();
            updateRequest.CustomerId = this.MTApp.CurrentStore.StripeCustomerId;
            updateRequest.PostalCode = this.txtZipCode.Text.Trim();

            var res = billManager.UpdateCustomer(updateRequest);
            if (!res.Success)
            {
                this.MessageBox1.ShowWarning("Unable to update card: " + res.Message);
                return;
            }

            this.MessageBox1.ShowOk("Credit card information updated!");                        
        }
    }
}