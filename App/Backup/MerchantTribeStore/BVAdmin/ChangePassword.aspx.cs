using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MerchantTribe.Commerce.Accounts;

namespace MerchantTribeStore.BVAdmin
{
    public partial class ChangePassword : BaseAdminPage
    {
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
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

        protected void btnSave_Click(object sender, ImageClickEventArgs e)
        {
            this.MessageBox1.ClearMessage();

            UserAccount u = GetCorrectUser();
            if (u == null)
            {
                this.MessageBox1.ShowError("Could not locate your account.");
                return;
            }

            if (!u.DoesPasswordMatch(this.CurrentPasswordField.Text.Trim()))
            {
                this.MessageBox1.ShowWarning("Current password was incorrect. Try Again.");
                return;
            }

            u.HashedPassword = this.NewPasswordField.Text.Trim();
            MTApp.AccountServices.AdminUsers.Update(u);
            this.MessageBox1.ShowOk("Password was changed!");            
        }

    }
}