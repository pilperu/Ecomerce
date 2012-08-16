using System;
using System.Web;
using System.Web.UI.WebControls;
using MerchantTribe.Commerce.Contacts;
using MerchantTribe.Commerce.Membership;
using MerchantTribe.Commerce;
using System.Collections.Generic;

namespace MerchantTribeStore
{

    partial class BVAdmin_People_users_edit_address : BaseAdminPage
    {

        protected override void OnPreInit(System.EventArgs e)
        {
            base.OnPreInit(e);
            this.PageTitle = "Edit User Address";
            this.CurrentTab = AdminTabType.People;
            ValidateCurrentUserHasPermission(SystemPermissions.PeopleView);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            SetEditorMode();

            if (!Page.IsPostBack)
            {

                if (Request.QueryString["UserID"] != null)
                {
                    this.UserIDField.Value = Request.QueryString["UserID"];
                }

                if (Request.QueryString["id"] != null)
                {
                    LoadAddressForUser(Request.QueryString["id"]);
                }
                else
                {
                    Address a = new Address();
                    a.CountryBvin = WebAppSettings.ApplicationCountryBvin;
                    this.AddressEditor1.LoadFromAddress(a);
                }

            }
        }

        private void SetEditorMode()
        {
            AddressEditor1.RequireAddress = false;
            AddressEditor1.RequireCity = false;
            AddressEditor1.RequireCompany = false;
            AddressEditor1.RequireFirstName = false;
            AddressEditor1.RequireLastName = false;
            AddressEditor1.RequirePhone = false;
            AddressEditor1.RequirePostalCode = false;
            AddressEditor1.RequireRegion = false;
            AddressEditor1.ShowCompanyName = true;
            AddressEditor1.ShowPhoneNumber = true;
            AddressEditor1.ShowCounty = true;
        }

        private void LoadAddressForUser(string addressID)
        {
            CustomerAccount u = MTApp.MembershipServices.Customers.Find(this.UserIDField.Value);
            if (u != null)
            {
                if (u.Addresses != null)
                {
                    for (int i = 0; i <= u.Addresses.Count - 1; i++)
                    {
                        if (u.Addresses[i].Bvin == addressID)
                        {
                            this.AddressEditor1.LoadFromAddress(u.Addresses[i]);
                        }
                    }
                }
            }
            u = null;
        }

        private void LoadBilling()
        {
            CustomerAccount u = MTApp.MembershipServices.Customers.Find(this.UserIDField.Value);
            if (u != null)
            {
                this.AddressEditor1.LoadFromAddress(u.BillingAddress);
            }
            u = null;
        }
        private void LoadShipping()
        {
            CustomerAccount u = MTApp.MembershipServices.Customers.Find(this.UserIDField.Value);
            if (u != null)
            {
                this.AddressEditor1.LoadFromAddress(u.ShippingAddress);
            }
            u = null;
        }

        protected void btnCancel_Click(System.Object sender, System.Web.UI.ImageClickEventArgs e)
        {
            Response.Redirect("users_edit.aspx?id=" + this.UserIDField.Value);
        }

        protected void btnSave_Click(System.Object sender, System.Web.UI.ImageClickEventArgs e)
        {
            if (this.AddressEditor1.Validate() == true)
            {
                if (SaveCurrentAddress() == true)
                {
                    Response.Redirect("users_edit.aspx?id=" + this.UserIDField.Value);
                }
                else
                {
                    // throw error
                }
            }
        }

        private bool SaveCurrentAddress()
        {
            bool result = false;

            CustomerAccount u;
            u = MTApp.MembershipServices.Customers.Find(this.UserIDField.Value);

            if (u != null)
            {
                Address temp = this.AddressEditor1.GetAsAddress();
                
                if (temp.Bvin == null || temp.Bvin == string.Empty)
                {                                                        
                    temp.Bvin = System.Guid.NewGuid().ToString();
                    MTApp.MembershipServices.CheckIfNewAddressAndAddWithUpdate(u,temp);
                }
                else
                {
                    u.UpdateAddress(temp);
                }

                CreateUserStatus s = CreateUserStatus.None;
                result = MTApp.MembershipServices.UpdateCustomer(u, ref s);
            }

            return result;
        }

    }
}