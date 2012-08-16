using MerchantTribe.Web.Geography;
using MerchantTribe.Commerce;

namespace MerchantTribeStore
{

    partial class BVAdmin_Controls_AddressEditor : MerchantTribe.Commerce.Content.BVUserControl
    {

        private bool _ShowCompanyName = true;
        private bool _ShowPhoneNumber = true;
        private bool _ShowCounty = false;

        private bool _RequireFirstName = true;
        private bool _RequireLastName = true;
        private bool _RequireCompany = false;
        private bool _RequirePhone = false;

        private bool _RequireAddress = true;
        private bool _RequireCity = true;
        private bool _RequirePostalCode = true;
        private bool _RequireRegion = true;

        private int _TabOrderOffSet = 100;

        public bool ShowCompanyName
        {
            get { return _ShowCompanyName; }
            set { _ShowCompanyName = value; }
        }
        public bool ShowPhoneNumber
        {
            get { return _ShowPhoneNumber; }
            set { _ShowPhoneNumber = value; }
        }
        public bool ShowCounty
        {
            get { return _ShowCounty; }
            //Me.lstState.AutoCallBack = value
            set { _ShowCounty = value; }
        }
        public bool RequireFirstName
        {
            get { return _RequireFirstName; }
            set { _RequireFirstName = value; }
        }
        public bool RequireLastName
        {
            get { return _RequireLastName; }
            set { _RequireLastName = value; }
        }
        public bool RequireCompany
        {
            get { return _RequireCompany; }
            set { _RequireCompany = value; }
        }
        public bool RequirePhone
        {
            get { return _RequirePhone; }
            set { _RequirePhone = value; }
        }
        public bool RequireAddress
        {
            get { return _RequireAddress; }
            set { _RequireAddress = value; }
        }
        public bool RequireCity
        {
            get { return _RequireCity; }
            set { _RequireCity = value; }
        }
        public bool RequirePostalCode
        {
            get { return _RequirePostalCode; }
            set { _RequirePostalCode = value; }
        }
        public bool RequireRegion
        {
            get { return _RequireRegion; }
            set { _RequireRegion = value; }
        }

        public string CountryName
        {
            get { return this.lstCountry.SelectedItem.Text; }
        }
        public string CountryCode
        {
            get { return this.lstCountry.SelectedValue; }
            set
            {

                if (this.lstCountry.Items.FindByValue(value) != null)
                {
                    this.lstCountry.ClearSelection();
                    this.lstCountry.Items.FindByValue(value).Selected = true;
                }
                this.PopulateRegions(this.lstCountry.SelectedValue);
            }
        }
        public string FirstName
        {
            get { return this.firstNameField.Text.Trim(); }
            set { this.firstNameField.Text = value.Trim(); }
        }
        public string LastName
        {
            get { return lastNameField.Text.Trim(); }
            set { lastNameField.Text = value.Trim(); }
        }
        public string CompanyName
        {
            get { return CompanyField.Text.Trim(); }
            set { CompanyField.Text = value.Trim(); }
        }
        public string StreetLine1
        {
            get { return address1Field.Text.Trim(); }
            set { address1Field.Text = value.Trim(); }
        }
        public string StreetLine2
        {
            get { return address2Field.Text.Trim(); }
            set { address2Field.Text = value.Trim(); }
        }
        public string StreetLine3
        {
            get { return address3Field.Text.Trim(); }
            set { address3Field.Text = value.Trim(); }
        }
        public string City
        {
            get { return cityField.Text.Trim(); }
            set { cityField.Text = value.Trim(); }
        }
        public string StateName
        {
            get
            {
                if (lstState.Items.Count > 0)
                {
                    return lstState.SelectedItem.Text;
                }
                else
                {
                    return stateField.Text.Trim();
                }
            }
            set { stateField.Text = value.Trim(); }
        }
        public string StateCode
        {
            get
            {
                if (this.lstState.Items.Count > 0)
                {
                    return lstState.SelectedValue;
                }
                else
                {
                    return "";
                }
            }
            set
            {
                if (this.lstState.Items.FindByValue(value) != null)
                {
                    this.lstState.ClearSelection();
                    this.lstState.Items.FindByValue(value).Selected = true;
                }
            }
        }
        public string PostalCode
        {
            get { return postalCodeField.Text.Trim(); }
            set { postalCodeField.Text = value.Trim(); }
        }
        public string PhoneNumber
        {
            get { return PhoneNumberField.Text.Trim(); }
            set { PhoneNumberField.Text = value.Trim(); }
        }

        public int TabOrderOffSet
        {
            get { return _TabOrderOffSet; }
            set { _TabOrderOffSet = value; }
        }

        private bool _Initialized = false;

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);

            if (!Page.IsPostBack)
            {
                InitializeAddress();

                this.lblStateError.Visible = false;

                this.lstCountry.TabIndex = (short)(_TabOrderOffSet + 0);
                this.firstNameField.TabIndex = (short)(_TabOrderOffSet + 2);
                this.lastNameField.TabIndex = (short)(_TabOrderOffSet + 4);
                this.CompanyField.TabIndex = (short)(_TabOrderOffSet + 5);
                this.address1Field.TabIndex = (short)(_TabOrderOffSet + 6);
                this.address2Field.TabIndex = (short)(_TabOrderOffSet + 7);
                this.address3Field.TabIndex = (short)(_TabOrderOffSet + 7);
                this.cityField.TabIndex = (short)(_TabOrderOffSet + 8);
                this.lstState.TabIndex = (short)(_TabOrderOffSet + 9);
                this.stateField.TabIndex = (short)(_TabOrderOffSet + 10);
                this.postalCodeField.TabIndex = (short)(_TabOrderOffSet + 11);
                this.PhoneNumberField.TabIndex = (short)(_TabOrderOffSet + 13);

            }
            UpdateVisibleRows();
        }

        public bool Validate()
        {
            bool result = true;
            this.lblStateError.Visible = false;

            if (_RequireAddress == true)
            {
                this.valAddress.Validate();
                if (this.valAddress.IsValid == false)
                {
                    result = false;
                }
            }

            if (_RequireFirstName == true)
            {
                this.valFirstName.Validate();
                if (this.valFirstName.IsValid == false)
                {
                    result = false;
                }
            }

            if (_RequireLastName == true)
            {
                this.valLastName.Validate();
                if (this.valLastName.IsValid == false)
                {
                    result = false;
                }
            }

            if (_RequireCity == true)
            {
                this.valCity.Validate();
                if (this.valCity.IsValid == false)
                {
                    result = false;
                }
            }

            if (_RequirePostalCode == true)
            {
                this.valPostalCode.Validate();
                if (this.valPostalCode.IsValid == false)
                {
                    result = false;
                }
            }


            if (_RequireCompany == true)
            {
                this.valCompany.Validate();
                if (this.valCompany.IsValid == false)
                {
                    result = false;
                }
            }

            if (_RequirePhone == true)
            {
                this.valPhone.Validate();
                if (this.valPhone.IsValid == false)
                {
                    result = false;
                }
                if (this.PhoneNumberField.Text.Trim().Length < 7)
                {
                    result = false;
                    this.valPhone.IsValid = false;
                }

            }

            if (_RequireRegion == true)
            {
                if (this.lstState.Items.Count > 1)
                {
                    if (this.lstState.SelectedIndex == 0)
                    {
                        this.lblStateError.Visible = true;
                        result = false;
                    }
                }
            }

            return result;
        }

        public void LoadFromAddress(MerchantTribe.Commerce.Contacts.Address a)
        {
            InitializeAddress();
            if (!(a == null))
            {
                this.AddressBvin.Value = a.Bvin;
                this.AddressTypeField.Value = ((int)a.AddressType).ToString();
                this.lstCountry.ClearSelection();
                if (this.lstCountry.Items.FindByValue(a.CountryBvin) != null)
                {
                    this.lstCountry.Items.FindByValue(a.CountryBvin).Selected = true;
                }
                this.PopulateRegions(this.lstCountry.SelectedValue);
                if (this.lstState.Items.Count > 0)
                {
                    this.lstState.ClearSelection();
                    if (this.lstState.Items.FindByValue(a.RegionBvin) != null)
                    {
                        this.lstState.Items.FindByValue(a.RegionBvin).Selected = true;
                    }
                }

                this.StateName = a.RegionName;
                this.FirstName = a.FirstName;
                this.LastName = a.LastName;
                this.CompanyName = a.Company;
                this.StreetLine1 = a.Line1;
                this.StreetLine2 = a.Line2;
                this.StreetLine3 = a.Line3;
                this.City = a.City;
                this.PostalCode = a.PostalCode;
                this.PhoneNumber = a.Phone;

                this.StoreId.Value = a.StoreId.ToString();
            }
        }

        public MerchantTribe.Commerce.Contacts.Address GetAsAddress()
        {
            MerchantTribe.Commerce.Contacts.Address a = new MerchantTribe.Commerce.Contacts.Address();
            if (lstCountry.Items.Count > 0)
            {
                a.CountryBvin = lstCountry.SelectedValue;
                a.CountryName = lstCountry.SelectedItem.ToString();
            }
            else
            {
                a.CountryBvin = "";
                a.CountryName = "Unknown";
            }
            if (lstState.Items.Count > 0)
            {
                a.RegionName = lstState.SelectedItem.Text;
                a.RegionBvin = lstState.SelectedItem.Value;
            }
            else
            {
                a.RegionName = stateField.Text.Trim();
                a.RegionBvin = "";
            }

            a.FirstName = this.FirstName;
            a.LastName = this.LastName;
            a.Company = this.CompanyName;
            a.Line1 = this.StreetLine1;
            a.Line2 = this.StreetLine2;
            a.Line3 = this.StreetLine3;
            a.City = this.City;
            a.PostalCode = this.PostalCode;
            a.Phone = this.PhoneNumber;
            if (this.AddressBvin.Value != string.Empty)
            {
                a.Bvin = this.AddressBvin.Value;
            }
            int type = 0;
            if (int.TryParse(this.AddressTypeField.Value, out type))
            {
                a.AddressType = (MerchantTribe.Commerce.Contacts.AddressTypes)type;
            }

            long storeId = 0;
            if (long.TryParse(this.StoreId.Value, out storeId)) a.StoreId = storeId;

            return a;
        }

        private void UpdateVisibleRows()
        {

            this.valFirstName.Enabled = _RequireFirstName;
            this.valLastName.Enabled = _RequireLastName;
            this.valAddress.Enabled = _RequireAddress;
            this.valCity.Enabled = _RequireCity;
            this.valPostalCode.Enabled = _RequirePostalCode;

            if (_ShowCompanyName == true)
            {
                this.CompanyNameRow.Visible = true;
                this.valCompany.Enabled = _RequireCompany;
            }
            else
            {
                this.valCompany.Enabled = false;
                this.CompanyNameRow.Visible = false;
            }

            if (_ShowPhoneNumber == true)
            {
                this.PhoneRow.Visible = true;
                this.valPhone.Enabled = _RequirePhone;
            }
            else
            {
                this.valPhone.Enabled = false;
                this.PhoneRow.Visible = false;
            }

        }

        private void InitializeAddress()
        {
            if (this._Initialized == false)
            {
                this.AddressTypeField.Value = "0";

                PopulateCountries();
                this.lstCountry.ClearSelection();
                if (this.lstCountry.Items.FindByValue(WebAppSettings.ApplicationCountryBvin) != null)
                {
                    this.lstCountry.Items.FindByValue(WebAppSettings.ApplicationCountryBvin).Selected = true;
                    PopulateRegions(WebAppSettings.ApplicationCountryBvin);
                }
                else
                {
                    if (this.lstCountry.Items.Count > 0)
                    {
                        this.lstCountry.Items[0].Selected = true;
                        PopulateRegions(this.lstCountry.Items[0].Value);
                    }
                }

                _Initialized = true;
            }
        }

        private void PopulateCountries()
        {
            lstCountry.DataSource = MyPage.MTApp.CurrentStore.Settings.FindActiveCountries();
            lstCountry.DataValueField = "Bvin";
            lstCountry.DataTextField = "DisplayName";
            lstCountry.DataBind();
        }

        private void PopulateRegions(string countryCode)
        {
            this.lstState.Items.Clear();
            this.lstState.AppendDataBoundItems = true;
            this.lstState.DataSource = Country.FindByBvin(countryCode).Regions;
            this.lstState.DataTextField = "abbreviation";
            this.lstState.DataValueField = "abbreviation";
            this.lstState.DataBind();

            if (lstState.Items.Count < 1)
            {
                this.lstState.Visible = false;
                stateField.Visible = true;
            }
            else
            {
                this.lstState.Visible = true;
                stateField.Visible = false;
            }
        }

        protected void lstCountry_SelectedIndexChanged(object Sender, System.EventArgs e)
        {
            PopulateRegions(lstCountry.SelectedItem.Value);
        }

    }

}