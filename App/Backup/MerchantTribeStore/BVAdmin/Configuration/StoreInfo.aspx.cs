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

namespace MerchantTribeStore
{

    partial class BVAdmin_Content_StoreInfo : BaseAdminPage
    {

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);

            this.ShipFromAddressField.RequireFirstName = false;
            this.ShipFromAddressField.RequireLastName = false;
            this.ShipFromAddressField.RequirePhone = false;
            this.ShipFromAddressField.RequirePostalCode = false;
            this.ShipFromAddressField.RequireRegion = false;
            this.ShipFromAddressField.RequireAddress = false;
            this.ShipFromAddressField.RequireCity = false;
            this.ShipFromAddressField.RequireCompany = false;

            if (!Page.IsPostBack)
            {

                LoadData();
            }
        }

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.PageTitle = "Store's Address";
            this.CurrentTab = AdminTabType.Configuration;
            ValidateCurrentUserHasPermission(SystemPermissions.SettingsView);
        }

        protected void btnSave_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            this.MessageBox1.ClearMessage();
            if (this.Save() == true)
            {
                this.MessageBox1.ShowOk("Changes Saved!");
            }
            else
            {
                this.MessageBox1.ShowWarning("Unable to save changes.");
            }
        }

        private void LoadData()
        {
            this.ShipFromAddressField.LoadFromAddress(MTApp.ContactServices.Addresses.FindStoreContactAddress());
        }

        private bool Save()
        {
            bool result = false;

            Address toUpdate = this.ShipFromAddressField.GetAsAddress();
            toUpdate.AddressType = AddressTypes.StoreContact;
            if (toUpdate.Bvin == string.Empty)
            {
                result = MTApp.ContactServices.Addresses.Create(toUpdate);
            }
            else
            {
                result = MTApp.ContactServices.Addresses.Update(toUpdate);                
            }
            MTApp.ContactServices.Addresses.SubmitChanges();

            return result;

        }

    }
}