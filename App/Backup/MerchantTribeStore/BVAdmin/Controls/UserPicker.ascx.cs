using MerchantTribe.Commerce;
using System.Collections.Generic;

namespace MerchantTribeStore
{

    partial class BVAdmin_Controls_UserPicker : MerchantTribe.Commerce.Content.BVUserControl
    {

        private int _tabIndex = -1;

        private int _usernameFieldSize = 15;
        
        public int UserNameFieldSize
        {
            get { return _usernameFieldSize; }
            set { _usernameFieldSize = value; }
        }
        public string UserName
        {
            get { return this.UserNameField.Text; }
            set { this.UserNameField.Text = value; }
        }

        protected IMessageBox _messageBox = null;

        public IMessageBox MessageBox
        {
            get { return _messageBox; }
            set { _messageBox = value; }
        }

        public int TabIndex
        {
            get { return _tabIndex; }
            set { _tabIndex = value; }
        }

        public delegate void UserSelectedDelegate(MerchantTribe.Commerce.Controls.UserSelectedEventArgs e);
        public event UserSelectedDelegate UserSelected;

        protected override void OnLoad(System.EventArgs e)
        {
            this.UserNameField.Columns = _usernameFieldSize;
            base.OnLoad(e);
            if (_tabIndex != -1)
            {
                FilterField.TabIndex = (short)_tabIndex;
                btnGoUserSearch.TabIndex = (short)(_tabIndex + 1);
                btnBrowserUserCancel.TabIndex = (short)(_tabIndex + 2);
                
                NewUserEmailField.TabIndex = (short)(_tabIndex + 3);
                NewUserFirstNameField.TabIndex = (short)(_tabIndex + 4);
                NewUserLastNameField.TabIndex = (short)(_tabIndex + 5);
                NewUserTaxExemptField.TabIndex = (short)(_tabIndex + 6);

                btnNewUserCancel.TabIndex = (short)(_tabIndex + 7);
                btnNewUserSave.TabIndex = (short)(_tabIndex + 8);
            }
        }

        protected void btnBrowseUsers_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            if (MessageBox != null) MessageBox.ClearMessage();

            this.pnlNewUser.Visible = false;
            if (pnlUserBrowser.Visible == true)
            {
                pnlUserBrowser.Visible = false;
            }
            else
            {
                this.pnlUserBrowser.Visible = true;
                LoadUsers();
            }
        }

        protected void btnBrowserUserCancel_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            if (MessageBox != null) MessageBox.ClearMessage();

            this.pnlUserBrowser.Visible = false;
        }

        protected void btnNewUserCancel_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            if (MessageBox != null) MessageBox.ClearMessage();

            this.pnlNewUser.Visible = false;
        }

        protected void btnNewUser_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            if (MessageBox != null) MessageBox.ClearMessage();

            this.pnlUserBrowser.Visible = false;
            if (this.pnlNewUser.Visible == true)
            {
                this.pnlNewUser.Visible = false;
            }
            else
            {
                this.pnlNewUser.Visible = true;
            }
        }

        protected void btnGoUserSearch_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            if (MessageBox != null) MessageBox.ClearMessage();

            LoadUsers();
            this.FilterField.Focus();
        }

        private void LoadUsers()
        {
            List<MerchantTribe.Commerce.Membership.CustomerAccount> users;
            int count = 0;
            users = MyPage.MTApp.MembershipServices.Customers.FindByFilter(this.FilterField.Text.Trim(), 0, 50, ref count);
            this.GridView1.DataSource = users;
            this.GridView1.DataBind();
        }

        protected void btnNewUserSave_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            if (MessageBox != null) MessageBox.ClearMessage();

            MerchantTribe.Commerce.Membership.CustomerAccount u = new MerchantTribe.Commerce.Membership.CustomerAccount();
            u.Email = this.NewUserEmailField.Text.Trim();
            u.FirstName = this.NewUserFirstNameField.Text.Trim();
            u.LastName = this.NewUserLastNameField.Text.Trim();
            u.Password = MyPage.MTApp.MembershipServices.GeneratePasswordForCustomer(12);
            u.TaxExempt = this.NewUserTaxExemptField.Checked;
            MerchantTribe.Commerce.Membership.CreateUserStatus createResult = new MerchantTribe.Commerce.Membership.CreateUserStatus();
            if (MyPage.MTApp.MembershipServices.CreateCustomer(u, ref createResult, u.Password) == true)
            {
                this.UserNameField.Text = u.Email;
                ValidateUser();
                this.pnlNewUser.Visible = false;
            }
            else
            {
                switch (createResult)
                {
                    case MerchantTribe.Commerce.Membership.CreateUserStatus.DuplicateUsername:
                        if (MessageBox != null) MessageBox.ShowWarning("The username " + this.NewUserEmailField.Text.Trim() + " already exists. Please select another username.");
                        break;
                    case MerchantTribe.Commerce.Membership.CreateUserStatus.InvalidPassword:
                        if (MessageBox != null) MessageBox.ShowWarning("Unable to create this account. Invalid Password");
                        break;
                    default:
                        if (MessageBox != null) MessageBox.ShowWarning("Unable to create this account. Unknown Error.");
                        break;
                }
            }

        }

        protected void GridView1_RowEditing(object sender, System.Web.UI.WebControls.GridViewEditEventArgs e)
        {
            if (MessageBox != null) MessageBox.ClearMessage();
            string bvin = (string)GridView1.DataKeys[e.NewEditIndex].Value;
            MerchantTribe.Commerce.Membership.CustomerAccount u = MyPage.MTApp.MembershipServices.Customers.Find(bvin);
            if (u != null)
            {
                this.UserNameField.Text = u.Email;
            }
            ValidateUser();
            this.pnlUserBrowser.Visible = false;
        }

        protected void btnValidateUser_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            if (MessageBox != null) MessageBox.ClearMessage();

            ValidateUser();
        }

        private void ValidateUser()
        {
            MerchantTribe.Commerce.Membership.CustomerAccount u
                = MyPage.MTApp.MembershipServices.Customers.FindByEmail(this.UserNameField.Text.Trim());
            if (u != null)
            {
                if (u.Bvin != string.Empty)
                {
                    MerchantTribe.Commerce.Controls.UserSelectedEventArgs args = new MerchantTribe.Commerce.Controls.UserSelectedEventArgs();
                    args.UserAccount = u;
                    if (UserSelected != null)
                    {
                        UserSelected(args);
                    }
                }
                else
                {
                    if (MessageBox != null) MessageBox.ShowWarning("User account " + this.UserNameField.Text.Trim() + " wasn't found. Please try again or create a new account.");
                }
            }
        }

       
    }
}