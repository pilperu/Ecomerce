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
using System.Collections.Generic;
using MerchantTribe.Web.Geography;

using System.Collections.ObjectModel;

namespace MerchantTribeStore
{

    partial class BVAdmin_Configuration_Countries : BaseAdminPage
    {
        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.PageTitle = "Countries";
            this.CurrentTab = AdminTabType.Configuration;
            ValidateCurrentUserHasPermission(SystemPermissions.SettingsView);
           
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (!Page.IsPostBack)
            {
                LoadCountries();
            }
        }

        private void LoadCountries()
        {
            List<Country> disabled = Country.FindAllInList(MTApp.CurrentStore.Settings.DisabledCountryIso3Codes);
            List<Country> active = MTApp.CurrentStore.Settings.FindActiveCountries();

            this.lstActive.DataSource = active;
            this.lstActive.DataTextField = "DisplayName";
            this.lstActive.DataValueField = "IsoAlpha3";
            this.lstActive.DataBind();

            this.lstDisabled.DataSource = disabled;
            this.lstDisabled.DataTextField = "DisplayName";
            this.lstDisabled.DataValueField = "IsoAlpha3";
            this.lstDisabled.DataBind();
        }


        protected void btnDisabled_Click(object sender, EventArgs e)
        {
            this.MessageBox1.ClearMessage();

            if (this.lstActive.SelectedItem != null)
            {
                string iso = this.lstActive.SelectedItem.Value;
                MTApp.CurrentStore.Settings.DisableCountry(iso);
                MessageBox1.ShowOk("Country Disabled");
            }

            MTApp.UpdateCurrentStore();

            LoadCountries();
        }

        protected void btnEnable_Click(object sender, EventArgs e)
        {
            this.MessageBox1.ClearMessage();

            if (this.lstDisabled.SelectedItem == null)
            {
                this.MessageBox1.ShowWarning("Please select a country to enable first.");
            }
            else
            {
                string iso = this.lstDisabled.SelectedItem.Value;
                MTApp.CurrentStore.Settings.EnableCountry(iso);
                MessageBox1.ShowOk("Country enabled");
            }

            MTApp.UpdateCurrentStore();

            LoadCountries();
        }


    }
}