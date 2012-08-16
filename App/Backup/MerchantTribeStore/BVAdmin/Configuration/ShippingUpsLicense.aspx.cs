using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System;
using System.Web;
using System.Web.UI.WebControls;
using System.Text;
using MerchantTribe.Commerce;
using MerchantTribe.Commerce.Accounts;
using MerchantTribe.Commerce.BusinessRules;
using MerchantTribe.Commerce.Catalog;
using MerchantTribe.Commerce.Contacts;
using MerchantTribe.Commerce.Content;
using MerchantTribe.Commerce.Membership;
using MerchantTribe.Commerce.Metrics;
using MerchantTribe.Commerce.Orders;
using MerchantTribe.Commerce.Payment;
using MerchantTribe.Commerce.Shipping;
using MerchantTribe.Commerce.Taxes;
using MerchantTribe.Commerce.Utilities;
using System.Collections.ObjectModel;
using MerchantTribe.Web.Geography;

namespace MerchantTribeStore.BVAdmin.Configuration
{
    public partial class ShippingUpsLicense : BaseAdminPage
    {
        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.PageTitle = "UPS Online Tools License";
            this.CurrentTab = AdminTabType.Configuration;
            ValidateCurrentUserHasPermission(SystemPermissions.SettingsView);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (!Page.IsPostBack)
            {

                try
                {
                    LoadStates();

                    Address contactAddress = MTApp.ContactServices.Addresses.FindStoreContactAddress();

                    Country c = Country.FindByBvin(contactAddress.CountryBvin);
                    if (c != null)
                    {
                        inCountry.SelectedValue = c.IsoCode;
                    }
                    inAddress1.Text = contactAddress.Line1;
                    inAddress2.Text = contactAddress.Line2;
                    inCity.Text = contactAddress.City;
                    Region r = c.FindRegion(contactAddress.RegionBvin);
                    if (r != null)
                    {
                        inState.SelectedValue = r.Abbreviation;
                    }
                    inEmail.Text = MTApp.CurrentStore.Settings.MailServer.EmailForGeneral;
                    inPhone.Text = contactAddress.Phone;
                    inZip.Text = contactAddress.PostalCode;

                }

                catch (Exception Ex)
                {
                    msg.ShowException(Ex);
                }

                // Get License
                try
                {
                    BVSoftware.Shipping.Ups.Registration UPSReg = new BVSoftware.Shipping.Ups.Registration();
                    if (UPSReg.GetLicense(WebAppSettings.ShippingUpsServer) == true)
                    {
                        lblLicense.Text = Server.HtmlEncode(UPSReg.License);
                        lblLicense.Text = lblLicense.Text.Replace(((char)10).ToString(), "&nbsp;<br>");
                    }
                    else
                    {
                        msg.ShowError("There was an error getting a license agreement: " + UPSReg.ErrorMessage);
                    }
                    UPSReg = null;
                }
                catch (Exception Exx)
                {
                    msg.ShowWarning(Exx.Message + "There was an error loading a license agreement.");
                }

            }
        }

        protected void btnCancel_Click(System.Object sender, System.Web.UI.ImageClickEventArgs e)
        {
            Response.Redirect("Shipping.aspx", true);
        }

        protected void btnAccept_Click(System.Object sender, System.Web.UI.ImageClickEventArgs e)
        {
            Trace.Write("Starting btnAccept_OnClick");
            msg.ClearMessage();


            if (rbContactYes.Checked == false & rbContactNo.Checked == false)
            {
                msg.ShowWarning("Please select yes or no as your answer to the contact me question at the bottom of this page.");
            }
            else
            {
                try
                {
                    BVSoftware.Shipping.Ups.Registration UPSReg = new BVSoftware.Shipping.Ups.Registration();

                    UPSReg.Address1 = inAddress1.Text.Trim();
                    if (inAddress2.Text.Trim().Length > 0)
                    {
                        UPSReg.Address2 = inAddress2.Text.Trim();
                    }
                    if (inAddress3.Text.Trim().Length > 0)
                    {
                        UPSReg.Address3 = inAddress3.Text.Trim();
                    }
                    UPSReg.City = inCity.Text.Trim();
                    UPSReg.Company = inCompany.Text.Trim();
                    UPSReg.Title = inTitle.Text.Trim();
                    UPSReg.Country = inCountry.SelectedValue;
                    UPSReg.Email = inEmail.Text.Trim();
                    UPSReg.Name = inName.Text.Trim();
                    UPSReg.URL = inURL.Text.Trim();
                    UPSReg.Phone = inPhone.Text.Trim();
                    if (inCountry.SelectedValue == "US" | inCountry.SelectedValue == "CA")
                    {
                        UPSReg.State = inState.SelectedItem.Value;
                    }
                    if (inCountry.SelectedValue == "US" | inCountry.SelectedValue == "CA")
                    {
                        UPSReg.Zip = inZip.Text.Trim();
                    }
                    if (rbContactYes.Checked == true)
                    {
                        UPSReg.ContactMe = "yes";
                    }
                    else
                    {
                        UPSReg.ContactMe = "no";
                    }
                    UPSReg.AccountNumber = inUPSAccountNumber.Text.Trim();

                    string sTempLicense = lblLicense.Text;
                    sTempLicense = sTempLicense.Replace("&nbsp;<br>", ((char)10).ToString());
                    sTempLicense = Server.HtmlDecode(sTempLicense);
                    //UPSReg.License = Server.HtmlDecode(lblLicense.Text)
                    UPSReg.License = sTempLicense;

                    if (UPSReg.AcceptLicense(WebAppSettings.ShippingUpsServer) == true)
                    {

                        MTApp.CurrentStore.Settings.ShippingUpsLicense = UPSReg.LicenseNumber;

                        // Complete Registration process here...

                        string tempUsername = "bvc5";
                        if (this.inPhone.Text.Trim().Length > 3)
                        {
                            tempUsername += this.inPhone.Text.Trim().Substring(inPhone.Text.Trim().Length - 4, 4);
                        }
                        UPSReg.Password = MTApp.MembershipServices.GeneratePasswordForCustomer(10);
                        if (UPSReg.Password.Length > 10)
                        {
                            UPSReg.Password = UPSReg.Password.Substring(0, 10);
                        }

                        bool RegistrationComplete = false;
                        int MaxRegistrationAttempts = 10;
                        int CurrentAttempts = 0;


                        while (true)
                        {
                            CurrentAttempts += 1;
                            if ((RegistrationComplete == true) | (CurrentAttempts > MaxRegistrationAttempts))
                            {
                                break;
                            }
                            else
                            {
                                UPSReg.Username = tempUsername;

                                UPSReg.RequestSuggestedUsername = true;
                                if (UPSReg.Register(WebAppSettings.ShippingUpsServer) == true)
                                {
                                    // Got Suggested Username
                                    UPSReg.Username = UPSReg.SuggestedUsername;

                                    // Now attempt actual registration
                                    UPSReg.RequestSuggestedUsername = false;
                                    if (UPSReg.Register(WebAppSettings.ShippingUpsServer) == true)
                                    {
                                        MTApp.CurrentStore.Settings.ShippingUpsUsername = UPSReg.Username;
                                        MTApp.CurrentStore.Settings.ShippingUpsPassword = UPSReg.Password;
                                        MTApp.AccountServices.Stores.Update(MTApp.CurrentStore);
                                        RegistrationComplete = true;
                                        break;
                                    }
                                }

                                UPSReg.RequestSuggestedUsername = false;
                            }
                        }

                        if (RegistrationComplete == true)
                        {
                            Response.Redirect("ShippingUpsThanks.aspx");
                        }
                        else
                        {
                            this.msg.ShowError("The registration process could not be completed at this time. Please try again later.");
                        }
                    }

                    else
                    {
                        msg.ShowError(UPSReg.ErrorMessage + "<br>ErrorCode:" + UPSReg.ErrorCode);
                    }
                }

                catch (Exception Ex)
                {
                    msg.ShowException(Ex);
                }
            }
            Trace.Write("Ending btnAccept_OnClick");

            MTApp.UpdateCurrentStore();
        }

        void LoadStates()
        {
            Trace.Write("Starting LoadStates");
            try
            {


                List<Region> regions;
                regions = Country.FindByISOCode("US").Regions;

                List<Region> caRegions = Country.FindByISOCode("CA").Regions;
                foreach (Region r in caRegions)
                {
                    regions.Add(r);
                }
                inState.DataSource = regions;
                inState.DataTextField = "Name";
                inState.DataValueField = "abbreviation";
                inState.DataBind();
            }
            catch (Exception Ex)
            {
                msg.ShowException(Ex);
            }
            Trace.Write("Ending LoadStates");
        }

        void LoadCountries()
        {
            inCountry.DataSource = MTApp.CurrentStore.Settings.FindActiveCountries();
            inCountry.DataTextField = "DisplayName";
            inCountry.DataValueField = "Bvin";
            inCountry.DataBind();
        }

        void SetCountry(string sCode)
        {
            this.inCountry.SelectedValue = sCode;
        }

        void SetState(string sCode)
        {
            this.inState.SelectedValue = sCode;
        }
    }
}