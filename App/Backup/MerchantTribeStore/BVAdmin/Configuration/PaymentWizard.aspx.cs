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

namespace MerchantTribeStore
{

    partial class BVAdmin_Configuration_PaymentWizard : BaseAdminPage
    {
        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.PageTitle = "Payment Setup Wizard";
            this.CurrentTab = AdminTabType.Configuration;
            ValidateCurrentUserHasPermission(SystemPermissions.SettingsView);
        }

        protected void lnkWithPayPal_Click(object sender, System.EventArgs e)
        {
            this.multiview1.SetActiveView(this.step2);
        }

        protected void lnkNoPayPal_Click(object sender, System.EventArgs e)
        {
            this.multiview1.SetActiveView(this.step2b);
        }

        protected void lnkBackFrom2a_Click(object sender, System.EventArgs e)
        {
            this.multiview1.SetActiveView(this.step1);
        }

        protected void lnkBackFrom2_Click(object sender, System.EventArgs e)
        {
            this.multiview1.SetActiveView(this.step1);
        }

        protected void lnkBackFrom3b_Click(object sender, System.EventArgs e)
        {
            this.multiview1.SetActiveView(this.step2b);
        }

        protected void lnkBackFrom3a_Click(object sender, System.EventArgs e)
        {
            this.multiview1.SetActiveView(this.step2);
        }

        protected void btnPayPalStandard_Click(object sender, System.EventArgs e)
        {
            this.multiview1.SetActiveView(this.step3a);
        }

        protected void btnPayPayPro_Click(object sender, System.EventArgs e)
        {
            this.multiview1.SetActiveView(this.step3a);
        }

        protected void btnSaveGateway_Click(object sender, System.EventArgs e)
        {
            this.multiview1.SetActiveView(this.step3b);
        }

        protected void btnPayPalConfig_Click(object sender, System.EventArgs e)
        {
            this.multiview1.SetActiveView(step4);
        }

        protected void btnCCConfig_Click(object sender, System.EventArgs e)
        {
            this.multiview1.SetActiveView(step4);
        }
    }
}