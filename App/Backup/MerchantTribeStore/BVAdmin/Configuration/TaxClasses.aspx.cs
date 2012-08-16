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

    partial class BVAdmin_Configuration_TaxClasses : BaseAdminPage
    {

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.PageTitle = "Tax Schedules";
            this.CurrentTab = AdminTabType.Configuration;
            ValidateCurrentUserHasPermission(SystemPermissions.SettingsView);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (!Page.IsPostBack)
            {

                LoadSchedules();

            }

        }

        protected void btnAddNewRegion_Click(System.Object sender, System.Web.UI.ImageClickEventArgs e)
        {
            msg.ClearMessage();
            try
            {
                MerchantTribe.Commerce.Taxes.TaxSchedule t = new TaxSchedule();
                t.Name = this.DisplayNameField.Text.Trim();
                MTApp.OrderServices.TaxSchedules.Create(t);
                msg.ShowOk("Added: " + t.Name);
                LoadSchedules();
                DisplayNameField.Text = "";
            }
            catch (Exception Ex)
            {
                msg.ShowException(Ex);
            }
        }

        void LoadSchedules()
        {
            try
            {
                dgTaxClasses.DataSource = MTApp.OrderServices.TaxSchedules.FindAllAndCreateDefault(MTApp.CurrentStore.Id);
                dgTaxClasses.DataBind();
            }
            catch (Exception Ex)
            {
                msg.ShowException(Ex);
            }
        }

        public void dgTaxClasses_Delete(System.Object sender, System.Web.UI.WebControls.DataGridCommandEventArgs e)
        {
            long editID = (long)dgTaxClasses.DataKeys[e.Item.ItemIndex];
            MTApp.OrderServices.TaxSchedulesDestroy(editID);
            LoadSchedules();
        }


    }
}