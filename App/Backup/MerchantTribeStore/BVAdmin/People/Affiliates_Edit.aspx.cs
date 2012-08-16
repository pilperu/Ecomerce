using System;
using System.Web.UI.WebControls;
using MerchantTribe.Commerce.Contacts;
using MerchantTribe.Commerce.Content;
using MerchantTribe.Commerce;
using System.Collections.Generic;

namespace MerchantTribeStore
{

    partial class BVAdmin_People_Affiliates_Edit : BaseAdminPage
    {
        private long CurrentId
        {
            get
            {
                long temp = 0;
                long.TryParse(this.BvinField.Value, out temp);
                return temp;
            }
            set
            {
                this.BvinField.Value = value.ToString();
            }
        }
        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);
            SetEditorMode();

            if (!Page.IsPostBack)
            {
                SetDefaults();
                this.DisplayNameField.Focus();

                if (Request.QueryString["id"] != null)
                {
                    this.BvinField.Value = Request.QueryString["id"];
                    LoadAffiliate();
                }
                else
                {
                    this.BvinField.Value = string.Empty;
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

        private void SetDefaults()
        {
            this.lstCommissionType.ClearSelection();
            switch (MTApp.CurrentStore.Settings.AffiliateCommissionType)
            {
                case AffiliateCommissionType.PercentageCommission:
                case AffiliateCommissionType.None:
                    this.lstCommissionType.Items.FindByValue("1").Selected = true;
                    break;
                case AffiliateCommissionType.FlatRateCommission:
                    this.lstCommissionType.Items.FindByValue("2").Selected = true;
                    break;
                default:
                    this.lstCommissionType.Items.FindByValue("1").Selected = true;
                    break;
            }
            this.CommissionAmountField.Text = MTApp.CurrentStore.Settings.AffiliateCommissionAmount.ToString("N");
            this.ReferralDaysField.Text = MTApp.CurrentStore.Settings.AffiliateReferralDays.ToString();
        }

        private void LoadAffiliate()
        {
            Affiliate a = MTApp.ContactServices.Affiliates.Find(CurrentId);
            if (a == null) return;
            
                    this.chkEnabled.Checked = a.Enabled;
                    this.DisplayNameField.Text = a.DisplayName;
                    this.ReferralIdField.Text = a.ReferralId;
                    this.lstCommissionType.ClearSelection();
                    switch (a.CommissionType)
                    {
                        case AffiliateCommissionType.PercentageCommission:
                            this.lstCommissionType.Items.FindByValue("1").Selected = true;
                            break;
                        case AffiliateCommissionType.FlatRateCommission:
                        case AffiliateCommissionType.None:
                            this.lstCommissionType.Items.FindByValue("2").Selected = true;
                            break;
                        default:
                            this.lstCommissionType.Items.FindByValue("1").Selected = true;
                            break;
                    }
                    this.CommissionAmountField.Text = a.CommissionAmount.ToString("N");
                    this.ReferralDaysField.Text = a.ReferralDays.ToString();
                    this.TaxIdField.Text = a.TaxId;
                    this.DriversLicenseField.Text = a.DriversLicenseNumber;
                    this.WebsiteUrlField.Text = a.WebSiteUrl;
                    this.NotesTextBox.Text = a.Notes;
                    if (a.ReferralId != string.Empty)
                    {
                        this.SampleUrlLabel.Text = MTApp.CurrentStore.RootUrl() + "?" + WebAppSettings.AffiliateQueryStringName + "=" + a.ReferralId;
                    }
                    else
                    {
                        this.SampleUrlLabel.Text = string.Empty;
                    }

                    //Load Theme Info Here                
                    this.AddressEditor1.LoadFromAddress(a.Address);              
        }

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.PageTitle = "Edit Affiliate";
            this.CurrentTab = AdminTabType.People;
            ValidateCurrentUserHasPermission(MerchantTribe.Commerce.Membership.SystemPermissions.PeopleView);
        }

        protected void btnCancel_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            Response.Redirect("Affiliates.aspx");
        }

        protected void btnSaveChanges_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            this.lblError.Text = string.Empty;

            if (Save() == true)
            {
                Response.Redirect("Affiliates.aspx");
            }
        }

        private bool Save()
        {
            bool result = false;
            
            Affiliate a = null;
            if (CurrentId < 1)
            {
                a = new Affiliate();
            }
            else
            {
                a = MTApp.ContactServices.Affiliates.Find(CurrentId);
            }


                a.Enabled = this.chkEnabled.Checked;
                a.DisplayName = this.DisplayNameField.Text.Trim();
                a.ReferralId = this.ReferralIdField.Text.Trim();
                int typeSelection = int.Parse(this.lstCommissionType.SelectedValue);
                a.CommissionType = (AffiliateCommissionType)typeSelection;
                a.CommissionAmount = decimal.Parse(this.CommissionAmountField.Text, System.Globalization.NumberStyles.Currency);
                a.ReferralDays = int.Parse(this.ReferralDaysField.Text);
                a.TaxId = this.TaxIdField.Text.Trim();
                a.DriversLicenseNumber = this.DriversLicenseField.Text.Trim();
                a.WebSiteUrl = this.WebsiteUrlField.Text.Trim();
                a.Address = this.AddressEditor1.GetAsAddress();
                a.Notes = this.NotesTextBox.Text;
                if (CurrentId < 1)
                {
                    result = MTApp.ContactServices.Affiliates.Create(a);
                    if (result)
                    {
                        CurrentId = a.Id;
                    }
                }
                else
                {
                    result = MTApp.ContactServices.Affiliates.Update(a);
                }

                if (result == false)
                {
                    this.lblError.Text = "Unable to save affiliate. Uknown error.";
                }           

            return result;
        }

    }
}