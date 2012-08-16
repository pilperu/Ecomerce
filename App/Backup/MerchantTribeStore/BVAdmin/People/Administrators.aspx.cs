using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MerchantTribe.Commerce.Accounts;
using System.Text;

namespace MerchantTribeStore.BVAdmin.People
{
    public partial class Administrators : BaseAdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {            
            LoadAdmins();
        }

        private void LoadAdmins()
        {
            bool isOwner = MTApp.AccountServices.IsUserStoreOwner(MTApp.CurrentStore.Id, CurrentUser.Id);

            StringBuilder sb = new StringBuilder();
            sb.Append("<table width=\"100%\">");

            foreach (StoreUserRelationship rel in MTApp.AccountServices.AdminUsersXStores.FindByStoreId(MTApp.CurrentStore.Id))
            {
                UserAccount u = MTApp.AccountServices.AdminUsers.FindById(rel.UserId);


                string destinationLink = "AdministratorsRemove.aspx?id=" + u.Id;

                sb.Append("<tr>");
                sb.Append("<td>" + u.Email + "</td>");
                if (rel.AccessMode == StoreAccessMode.Manager)
                {
                    if (isOwner)
                    {
                        sb.Append("<td><a onclick=\"return window.confirm('Remove this Administrator?');\" class=\"btn\" href=\"" + destinationLink + "\">");
                        sb.Append("<b>Remove</b>");
                        sb.Append("</a></td>");
                    }
                    else
                    {
                        sb.Append("<td>&nbsp;</td>");
                    }
                }
                else
                {
                    sb.Append("<td>Store Owner</td>");
                }
                sb.Append("</tr>");                
            }
            sb.Append("</table>");

            this.litAdministrators.Text = sb.ToString();

            if (isOwner)
            {
                this.pnlFilter.Visible = true;
            }
            else
            {
                this.pnlFilter.Visible = false;
            }
        }

        protected void btnGo_Click(object sender, ImageClickEventArgs e)
        {
            this.MessageBox1.ClearMessage();
            bool isOwner = MTApp.AccountServices.IsUserStoreOwner(MTApp.CurrentStore.Id, CurrentUser.Id);
            if (isOwner)
            {
                if (!ValidateEmail(NewAdminField.Text))
                {
                    this.MessageBox1.ShowInformation("Please enter an email address for the administrator instead of a name.");
                }
                else
                {
                    if (MTApp.AccountServices.AddUserToStoreByEmail(MTApp.CurrentStore.Id, NewAdminField.Text.Trim(), StoreAccessMode.Manager))
                    {
                        this.MessageBox1.ShowOk("Added New Admin");
                    }
                    else
                    {
                        this.MessageBox1.ShowWarning("Unable to add new admin at this time.");
                    }
                }
            }

            LoadAdmins();
        }

        private bool ValidateEmail(string input)
        {
            return MerchantTribe.Web.Validation.EmailValidation.MeetsEmailFormatRequirements(input);
        }
    }
}