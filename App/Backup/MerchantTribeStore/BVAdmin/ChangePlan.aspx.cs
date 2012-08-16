using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MerchantTribe.Commerce.Accounts;
using MerchantTribe.Commerce.Accounts.Billing;

namespace MerchantTribeStore
{

    public partial class BVAdmin_ChangePlan : BaseAdminPage
    {
        protected override void OnPreInit(System.EventArgs e)
        {
            base.OnPreInit(e);
            this.PageTitle = "Change Plan";
            this.CurrentTab = AdminTabType.Dashboard;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (!Page.IsPostBack)
            {
                UserAccount u = GetCorrectUser();
                PopulatePage(u);
                if (Request.QueryString["ok"] != null)
                {
                    this.MessageBox1.ShowOk("Plan Changed!");
                }
            }
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
            SetCurrentStorePlan();
            if (MTApp.CurrentStore.StripeCustomerId.Trim().Length < 1)
            {
                this.pnlCreditCard.Visible = true;
            }
            else
            {
                this.pnlCreditCard.Visible = false;
            }
        }

        private void SetCurrentStorePlan()
        {
            switch (MTApp.CurrentStore.PlanId)
            {
                case 0:
                    this.btnFree.Text = "Current Plan";
                    this.btnBasic.Text = "<b>Upgrade</b>";
                    this.btnPlus.Text = "<b>Upgrade</b>";
                    this.btnPremium.Text = "<b>Upgrade</b>";
                    this.btnMax.Text = "<b>Upgrade</b>";
                    break;
                case 1:
                    this.btnFree.Text = "<b>Downgrade</b>";
                    this.btnBasic.Text = "Current Plan";                    
                    this.btnPlus.Text = "<b>Upgrade</b>";
                    this.btnPremium.Text = "<b>Upgrade</b>";
                    this.btnMax.Text = "<b>Upgrade</b>";
                    break;
                case 2:
                    this.btnFree.Text = "<b>Downgrade</b>";
                    this.btnBasic.Text = "<b>Downgrade</b>";
                    this.btnPlus.Text = "Current Plan";                    
                    this.btnPremium.Text = "<b>Upgrade</b>";
                    this.btnMax.Text = "<b>Upgrade</b>";
                    break;
                case 3:
                    this.btnFree.Text = "<b>Downgrade</b>";
                    this.btnBasic.Text = "<b>Downgrade</b>";
                    this.btnPlus.Text = "<b>Downgrade</b>";
                    this.btnPremium.Text = "Current Plan";
                    this.btnMax.Text = "<b>Upgrade</b>";
                    break;
                case 99:
                    this.btnFree.Text = "<b>Downgrade</b>";
                    this.btnBasic.Text = "<b>Downgrade</b>";
                    this.btnPlus.Text = "<b>Downgrade</b>";
                    this.btnPremium.Text = "<b>Downgrade</b>";
                    this.btnMax.Text = "Current Plan";
                    break;
            }
        }
        
        private void GoToPlan(int planId)
        {
            var user = GetCorrectUser();
            var store = MTApp.CurrentStore;
            var billManager = new BillingManager(this.MTApp);

            if (store.Id == (long)planId)
            {
                this.MessageBox1.ShowInformation("You selected the same plan you're currently on. No change required.");
                return;
            }

            // Make sure you don't have too many items to downgrade
            if (!CheckMax(planId)) return;
                        
            if (store.StripeCustomerId.Trim().Length > 0)
            {
                if (planId == 0)
                {
                    billManager.CancelSubscription(store.StripeCustomerId);                    
                    MTApp.AccountServices.ChangePlan(store.Id, user.Id, planId, MTApp);
                    Response.Redirect("ChangePlan.aspx?ok=1");                                    
                }

                var updateRequest = new UpdateCustomerRequest();
                updateRequest.CustomerId = store.StripeCustomerId;
                updateRequest.PlanId = TranslatePlanId(planId);

                var updateResponse = billManager.UpdateCustomer(updateRequest);
                if (!updateResponse.Success)
                {
                    this.MessageBox1.ShowWarning("Unable to update plan: " + updateResponse.Message);
                    return;
                }                                    
                if (!MTApp.AccountServices.ChangePlan(store.Id, user.Id, planId, MTApp))
                {
                    this.MessageBox1.ShowWarning("Unable to change plans! Check with support.");
                    return;                    
                }

                Response.Redirect("ChangePlan.aspx?ok=1");                
            }
            else
            {            
                var createRequest = new CreateCustomerRequest();
                createRequest.CreditCard = this.CreditCardInput1.GetCardData();
                createRequest.PostalCode = this.txtZipCode.Text;
                createRequest.PlanId = TranslatePlanId(planId);
                createRequest.Name = store.Id + " - " + store.StoreName;
                createRequest.Email = user.Email.Replace("@","+store" + store.Id + "@");

                var createResponse = billManager.CreateCustomer(createRequest);
                if (!createResponse.Success)
                {
                    this.MessageBox1.ShowWarning("Unable to change plans: " + createResponse.Message);
                    return;
                }

                // Save customer subscription id
                store.StripeCustomerId = createResponse.NewCustomerId;
                MTApp.UpdateCurrentStore();

                // Change plan in MerchantTribe
                MTApp.AccountServices.ChangePlan(store.Id, user.Id, planId, MTApp);
                Response.Redirect("ChangePlan.aspx?ok=1");                                
                
            }            
        }

        private string TranslatePlanId(int planId)
        {
            switch (planId)
            {
                case 0:
                    return "";
                case 1:
                    return "hosted-basic";
                case 2:
                    return "hosted-plus";
                case 3:
                    return "hosted-premium";
                case 99:
                    return "hosted-max";
                default:
                    return "";
            }   
        }
        protected void btnFree_Click(object sender, EventArgs e)
        {
            GoToPlan(0);            
        }
        protected void btnBasic_Click(object sender, EventArgs e)
        {
            GoToPlan(1);            
        }
        protected void btnPlus_Click(object sender, EventArgs e)
        {
            GoToPlan(2);            
        }
        protected void btnPremium_Click(object sender, EventArgs e)
        {
            GoToPlan(3);
        }
        protected void btnMax_Click(object sender, EventArgs e)
        {
            GoToPlan(99);
        }

        private bool CheckMax(int newPlan)
        {
            int current = MTApp.CatalogServices.Products.FindAllCount();
            HostedPlan p = HostedPlan.FindById(newPlan);
            if (current > p.MaxProducts)
            {
                this.MessageBox1.ShowWarning("You have too many products to downgrade to that plan. Remove some products first.");
                return false;
            }

            return true;
        }
    }
}