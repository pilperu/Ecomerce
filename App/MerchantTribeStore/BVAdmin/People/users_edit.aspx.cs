using System;
using System.Web;
using System.Web.UI.WebControls;
using MerchantTribe.Commerce.Membership;
using MerchantTribe.Commerce.Orders;
using MerchantTribe.Commerce.Catalog;
using MerchantTribe.Commerce.Contacts;
using MerchantTribe.Commerce;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MerchantTribeStore
{

    partial class BVAdmin_People_users_edit : BaseAdminPage
    {

        protected override void OnInit(System.EventArgs e)
        {
            base.OnInit(e);

            if (!Page.IsPostBack)
            {
                PasswordRegularExpressionValidator.ErrorMessage = "Password must be at least " + WebAppSettings.PasswordMinimumLength + " characters long.";
                PasswordRegularExpressionValidator.ValidationExpression = ".{" + WebAppSettings.PasswordMinimumLength.ToString() + ",50}";
            }
        }

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);

            if (!Page.IsPostBack)
            {

                BindPricingGroups();
                if (Request.QueryString["id"] != null)
                {
                    this.BvinField.Value = Request.QueryString["id"];
                    if (this.BvinField.Value.Trim().Length > 0)
                    {
                        LoadUser();
                        LoadOrders();
                        LoadSearchResults();
                        LoadWishList();
                    }
                    else
                    {
                        this.BvinField.Value = string.Empty;
                    }
                }
                else
                {
                    this.BvinField.Value = string.Empty;
                }
            }
        }


        private void LoadUser()
        {
            CustomerAccount u;
            u = MTApp.MembershipServices.Customers.Find(this.BvinField.Value);
            if (u != null)
            {
                if (u.Bvin != string.Empty)
                {
                    this.EmailField.Text = u.Email;
                    this.FirstNameField.Text = u.FirstName;
                    this.LastNameField.Text = u.LastName;
                    this.chkTaxExempt.Checked = u.TaxExempt;
                    this.PasswordField.Text = "********";
                    this.CommentField.Text = u.Notes;
                    this.LockedField.Checked = u.Locked;
                    this.LockedField.Text = " until " + TimeZoneInfo.ConvertTimeFromUtc(u.LockedUntilUtc, MTApp.CurrentStore.Settings.TimeZone).ToString() + " (" + u.FailedLoginCount + " failed attempts)";

                    this.PricingGroupDropDownList.SelectedValue = u.PricingGroupId;
                    //this.CustomQuestionAnswerTextBox.Text = u.CustomQuestionAnswers;
                }
            }

            if (!Page.IsPostBack)
            {
                AddressList.DataSource = u.Addresses;
                AddressList.DataBind();
            }
        }

        private void LoadAddresses()
        {
            CustomerAccount u;
            u = MTApp.MembershipServices.Customers.Find(this.BvinField.Value);
            if (u != null)
            {
                AddressList.DataSource = u.Addresses;
                AddressList.DataBind();
            }
            u = null;
        }

        private void BindPricingGroups()
        {
            PricingGroupDropDownList.Items.Clear();
            PricingGroupDropDownList.DataSource = MTApp.ContactServices.PriceGroups.FindAll();
            PricingGroupDropDownList.DataTextField = "Name";
            PricingGroupDropDownList.DataValueField = "bvin";
            PricingGroupDropDownList.DataBind();
            System.Web.UI.WebControls.ListItem item = new System.Web.UI.WebControls.ListItem("None", "");
            PricingGroupDropDownList.Items.Insert(0, item);
        }

        private void LoadOrders()
        {

            List<OrderSnapshot> dtOrders = new List<OrderSnapshot>();
            int totalCount = 0;
            dtOrders = MTApp.OrderServices.Orders.FindByUserId(Request.QueryString["id"], 1, 100, ref totalCount);

            if (dtOrders != null)
            {
                if (dtOrders.Count < 100)
                {
                    lblItems.Text = dtOrders.Count + " Orders Found";
                }
                else
                {
                    lblItems.Text = "First " + dtOrders.Count + " of " + totalCount + " Total Orders";
                }
                dgOrders.DataSource = dtOrders;
                dgOrders.DataBind();
            }
            else
            {
                lblItems.Text = "No Orders Could be Found";
            }
        }
        private void LoadSearchResults()
        {
            List<MerchantTribe.Commerce.Metrics.SearchQuery> sr = new List<MerchantTribe.Commerce.Metrics.SearchQuery>();
            int totalCount = 0;
            sr = MTApp.MetricsSerices.SearchQueries.FindByShopperId(this.BvinField.Value.ToString(), 1, 50, ref totalCount);

            if (sr != null)
            {
                this.dgSearchHistory.DataSource = sr;
                this.dgSearchHistory.DataBind();
            }

        }

        private void LoadWishList()
        {
            List<WishListItem> w = MTApp.CatalogServices.WishListItems.FindByCustomerIdPaged(Request.QueryString["id"], 1, 100);
            List<Product> p = new List<Product>();

            foreach (WishListItem item in w)
            {
                Product n = MTApp.CatalogServices.Products.Find(item.ProductId);
                n.ImageFileSmall = MerchantTribe.Commerce.Storage.DiskStorage.ProductImageUrlSmall(
                        MTApp,
                        n.Bvin,
                        n.ImageFileSmall,
                        Request.IsSecureConnection); 
                p.Add(n);
            }

            this.DataList1.DataSource = p;
            this.DataList1.DataBind();
        }

        protected void dgOrders_EditCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
        {
            string bvin = dgOrders.DataKeys[e.Item.ItemIndex].ToString();
            Response.Redirect("~/BVadmin/Orders/ViewOrder.aspx?id=" + bvin);
        }

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.PageTitle = "Edit User";
            this.CurrentTab = AdminTabType.People;
            ValidateCurrentUserHasPermission(SystemPermissions.PeopleView);
        }

        protected void AddressList_EditCommand(object source, System.Web.UI.WebControls.DataListCommandEventArgs e)
        {
            Save();
            string bvin = (string)this.AddressList.DataKeys[e.Item.ItemIndex];
            Response.Redirect("users_edit_address.aspx?userID=" + this.BvinField.Value + "&id=" + bvin);
        }

        protected void AddressList_DeleteCommand(object source, System.Web.UI.WebControls.DataListCommandEventArgs e)
        {

            Save();

            CustomerAccount u;
            u = MTApp.MembershipServices.Customers.Find(this.BvinField.Value);
            if (u != null)
            {
                string bvin = (string)this.AddressList.DataKeys[e.Item.ItemIndex];
                u.DeleteAddress(bvin);                
                CreateUserStatus s = CreateUserStatus.None;
                MTApp.MembershipServices.UpdateCustomer(u, ref s);
                LoadAddresses();
            }
        }

        protected void AddressList_ItemDataBound(object sender, System.Web.UI.WebControls.DataListItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item | e.Item.ItemType == ListItemType.AlternatingItem)
            {
                System.Web.UI.WebControls.Label AddressDisplay;
                AddressDisplay = (Label)e.Item.FindControl("AddressDisplay");
                if (AddressDisplay != null)
                {
                    AddressDisplay.Text = ((Address)e.Item.DataItem).ToHtmlString();
                }
            }
        }

        protected void btnCancel_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            Response.Redirect("default.aspx");
        }

        protected void btnSaveChanges_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            if (Page.IsValid)
            {
                this.lblError.Text = string.Empty;

                if (Save() == true)
                {
                    Response.Redirect("default.aspx");
                }
            }
        }

        private bool Save()
        {
            bool result = false;

            // Check password length
            if (this.PasswordField.Text.Trim().Length < WebAppSettings.PasswordMinimumLength)
            {
                this.lblError.Text = "Password must be at least " + WebAppSettings.PasswordMinimumLength + " characters long.";
                return false;
            }

            bool emailChanged = false;
            string oldEmailAddress = string.Empty;
            string newEmailAddress = this.EmailField.Text.Trim();

            CustomerAccount u;
            u = MTApp.MembershipServices.Customers.Find(this.BvinField.Value);
            if (u == null) u = new CustomerAccount();

            if (u != null)
            {

                u.Notes = this.CommentField.Text.Trim();
                if (string.Compare(u.Email.Trim(), this.EmailField.Text.Trim(), true) != 0)
                {
                    oldEmailAddress = u.Email.Trim();
                    emailChanged = true;
                }                
                u.FirstName = this.FirstNameField.Text.Trim();
                u.LastName = this.LastNameField.Text.Trim();
                u.TaxExempt = this.chkTaxExempt.Checked;
                //u.CustomQuestionAnswers = this.CustomQuestionAnswerTextBox.Text.Trim();
                
                if (u.Locked != this.LockedField.Checked)
                {
                    // Lock Status Changed                
                    if (this.LockedField.Checked == true)
                    {
                        MTApp.MembershipServices.LockCustomer(u);
                    }
                    else
                    {
                        MTApp.MembershipServices.UnlockCustomer(u);
                    }
                }

                u.PricingGroupId = PricingGroupDropDownList.SelectedValue;
             
                CreateUserStatus s = CreateUserStatus.None;

                if (this.BvinField.Value == string.Empty)
                {
                    // Create new user
                    u.Email = this.EmailField.Text.Trim();
                    result = MTApp.MembershipServices.CreateCustomer(u, ref s, this.PasswordField.Text.Trim());
                }
                else
                {

                    if (this.PasswordField.Text != "********")
                    {
                        u.Password = u.EncryptPassword(this.PasswordField.Text.Trim());
                    }           

                    // Update User
                    result = MTApp.MembershipServices.UpdateCustomer(u, ref s);
                }

                if (result == false)
                {
                    switch (s)
                    {
                        case CreateUserStatus.DuplicateUsername:
                            this.lblError.Text = "That username already exists. Select another username.";
                            break;
                        default:
                            this.lblError.Text = "Unable to save user. Uknown error.";
                            break;
                    }
                }
                else
                {                  
                    // Update bvin field so that next save will call updated instead of create
                    this.BvinField.Value = u.Bvin;

                    if (emailChanged)
                    {
                        if (MTApp.MembershipServices.UpdateCustomerEmail(u, newEmailAddress))
                        {
                            MTApp.CurrentRequestContext.IntegrationEvents.CustomerAccountEmailChanged(oldEmailAddress, u.Email);
                        }
                    }
                }
            }

            return result;
        }

        protected void btnNewAddress_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            if (this.Save() == true)
            {
                Response.Redirect("users_edit_address.aspx?userID=" + this.BvinField.Value);
            }
        }

        protected void btnBillingAddressEdit_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {

            if (this.Save() == true)
            {
                Response.Redirect("users_edit_address.aspx?userID=" + this.BvinField.Value + "&id=b");
            }

        }

        protected void btnShippingAddressEdit_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {

            if (this.Save() == true)
            {
                Response.Redirect("users_edit_address.aspx?userID=" + this.BvinField.Value + "&id=s");
            }

        }
    }
}