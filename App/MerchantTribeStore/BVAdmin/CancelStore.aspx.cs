using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MerchantTribe.Commerce;
using MerchantTribe.Commerce.Accounts;
using MerchantTribe.Commerce.Accounts.Billing;

namespace MerchantTribeStore
{

    public partial class BVAdmin_CancelStore : BaseAdminPage
    {
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            UserAccount u = GetCorrectUser();
            Store s = this.MTApp.CurrentStore;
            if (s == null) Response.Redirect("/bvadmin/account.aspx");
            this.lblStoreName.Text = s.Settings.FriendlyName;
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

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            UserAccount u = GetCorrectUser();
            long storeId = this.MTApp.CurrentStore.Id;

            var billManager = new BillingManager(this.MTApp);
            var response = billManager.CancelSubscription(this.MTApp.CurrentStore.StripeCustomerId);            
            MTApp.AccountServices.CancelStore(storeId, u.Id);
            Response.Redirect("http://www.merchanttribestores.com");
        }
    }
}