using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MerchantTribe.Commerce.Accounts;
using MerchantTribe.Billing;
using System.Text;
using System.Configuration;

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

            LoadStores(u);
            LoadBilling(u);

        }

        private void LoadStores(UserAccount u)
        {
            List<Store> stores = MTApp.AccountServices.FindStoresForUserId(u.Id);

            StringBuilder sb = new StringBuilder();

            foreach (Store s in stores)
            {
                RenderStore(s, sb);
            }

            this.litStores.Text = sb.ToString();
        }

        private void RenderStore(Store s, StringBuilder sb)
        {
            bool isIndividual = MerchantTribe.Commerce.WebAppSettings.IsIndividualMode;

            if (s != null)
            {
                sb.Append("<tr>");
                sb.Append("<td>" + s.Settings.FriendlyName + "</td>");

                if (!isIndividual)
                {
                    if (s.DateCancelled.HasValue)
                    {
                        sb.Append("<td>Cancelled " + s.DateCancelled.Value.ToShortDateString() + "</td>");
                    }
                    else
                    {
                        sb.Append("<td>" + s.PlanName + " " + s.CurrentPlanRate.ToString("c") + "/month + " + s.CurrentPlanPercent + "%<br />");
                        sb.Append("<a href=\"" + s.RootUrlSecure() + "bvadmin/ChangePlan.aspx\">Change Plans</a>");
                        sb.Append("</td>");
                    }
                    sb.Append("<td><a href=\"CancelStore.aspx?id=" + s.Id + "\">Cancel</a></td>");
                }
                
                sb.Append("<td><a target=\"_blank\" href=\"" + s.RootUrl() + "\">View Store</a></td>");
                sb.Append("<td><a target=\"_blank\" href=\"" + s.RootUrlSecure() + "bvadmin\">Admin</a></td>");
                sb.Append("</tr>");
            }
        }
        private void LoadBilling(UserAccount u)
        {
            MerchantTribe.Billing.Service svc = new MerchantTribe.Billing.Service(MerchantTribe.Commerce.WebAppSettings.ApplicationConnectionString);
            BillingAccount act = svc.Accounts.FindOrCreate(u.Email);
            if (act != null)
            {
                this.CreditCardInput1.LoadFromCardData(act.CreditCard);
                if (act.CreditCard.CardNumber == "4111111111111111")
                {
                    this.CreditCardInput1.CardNumber = "";
                }
                this.txtZipCode.Text = act.BillingZipCode;
            }

        }
        protected void btnUpdateCreditCard_Click(object sender, EventArgs e)
        {
            UserAccount u = GetCorrectUser();
            MerchantTribe.Billing.Service svc = new MerchantTribe.Billing.Service(MerchantTribe.Commerce.WebAppSettings.ApplicationConnectionString);
            BillingAccount act = svc.Accounts.FindOrCreate(u.Email);

            MerchantTribe.Payment.CardData data = this.CreditCardInput1.GetCardData();
            if (data.CardNumber == "") data.CardNumber = act.CreditCard.CardNumber;
            act.CreditCard = data;
            act.BillingZipCode = this.txtZipCode.Text.Trim();
            svc.Accounts.Update(act);
            this.MessageBox1.ShowOk("Billing Details Updated!");
        }
    }
}