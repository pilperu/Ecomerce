using System;
using System.Web;
using System.Web.UI.WebControls;
using MerchantTribe.Commerce.Contacts;
using MerchantTribe.Commerce.Membership;
using MerchantTribe.Commerce;
using System.Collections.Generic;

namespace MerchantTribeStore
{

    partial class BVAdmin_People_Vendors_Edit : BaseAdminPage
    {

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);
            SetEditorMode();

            if (!Page.IsPostBack)
            {
                this.DisplayNameField.Focus();

                if (Request.QueryString["id"] != null)
                {
                    this.BvinField.Value = Request.QueryString["id"];
                    InitializeForm();

                    if (this.BvinField.Value == "0")
                    {
                        this.BvinField.Value = string.Empty;
                        LoadGroups(new VendorManufacturer());
                        EmailTemplateDropDownList.SelectedValue = WebAppSettings.DefaultDropShipEmailTemplateId;
                    }
                    else
                    {
                        LoadManufacturer();
                    }
                }
            }
            this.UserPicker1.MessageBox = this.MessageBox1;
            this.UserPicker1.UserSelected += this.UserSelected;
        }

        private void InitializeForm()
        {
            EmailTemplateDropDownList.DataSource = MTApp.ContentServices.GetAllTemplatesForStoreOrDefaults();
            EmailTemplateDropDownList.DataTextField = "DisplayName";
            EmailTemplateDropDownList.DataValueField = "Id";
            EmailTemplateDropDownList.DataBind();
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

        private void LoadManufacturer()
        {
            VendorManufacturer m = MTApp.ContactServices.Vendors.Find(this.BvinField.Value);
            if (m != null)
            {
                if (m.Bvin != string.Empty)
                {
                    this.DisplayNameField.Text = m.DisplayName;
                    this.EmailField.Text = m.EmailAddress;
                    this.AddressEditor1.LoadFromAddress(m.Address);
                    this.EmailTemplateDropDownList.SelectedValue = m.DropShipEmailTemplateId;
                }
            }

            LoadGroups(m);
        }

        protected override void OnPreInit(System.EventArgs e)
        {
            base.OnPreInit(e);
            this.PageTitle = "Edit Manufacturer";
            this.CurrentTab = AdminTabType.People;
            ValidateCurrentUserHasPermission(MerchantTribe.Commerce.Membership.SystemPermissions.PeopleView);
        }

        protected void btnCancel_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            Response.Redirect("Vendors.aspx");
        }

        protected void btnSaveChanges_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            this.lblError.Text = string.Empty;

            if (Save() == true)
            {
                Response.Redirect("Vendors.aspx");
            }
        }

        private bool Save()
        {
            bool result = false;

            VendorManufacturer m = MTApp.ContactServices.Vendors.Find(this.BvinField.Value);
            if (m == null) m = new VendorManufacturer();

            m.DisplayName = this.DisplayNameField.Text.Trim();
            m.EmailAddress = this.EmailField.Text.Trim();
            m.Address = this.AddressEditor1.GetAsAddress();
            m.DropShipEmailTemplateId = this.EmailTemplateDropDownList.SelectedValue;
            if (this.BvinField.Value == string.Empty)
            {
                result = MTApp.ContactServices.Vendors.Create(m);
            }
            else
            {
                result = MTApp.ContactServices.Vendors.Update(m);
            }

            if (result == false)
            {
                this.lblError.Text = "Unable to save vendor. Uknown error.";
            }
            else
            {
                // Update bvin field so that next save will call updated instead of create
                this.BvinField.Value = m.Bvin;
            }

            return result;
        }

        protected void RemoveButton_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            VendorManufacturer m = MTApp.ContactServices.Vendors.Find(this.BvinField.Value);
            if (m != null)
            {
                foreach (System.Web.UI.WebControls.ListItem li in MemberList.Items)
                {
                    if (li.Selected == true)
                    {
                        m.RemoveContact(li.Value);
                    }
                }
            }
            MTApp.ContactServices.Vendors.Update(m);
            LoadGroups(m);
        }

        private void LoadGroups(VendorManufacturer m)
        {
            MemberList.DataSource = m.Contacts;
            MemberList.DataValueField = "bvin";
            MemberList.DataTextField = "Username";
            MemberList.DataBind();
        }

        protected void UserSelected(MerchantTribe.Commerce.Controls.UserSelectedEventArgs args)
        {
            if (this.BvinField.Value == string.Empty)
            {
                Save();
            }
            VendorManufacturer m = MTApp.ContactServices.Vendors.Find(this.BvinField.Value);
            if (m != null)
            {
                m.AddContact(args.UserAccount.Bvin);
                MTApp.ContactServices.Vendors.Update(m);
            }
            LoadGroups(m);
        }

    }
}