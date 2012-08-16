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
using MerchantTribe.Payment;
using System.Collections.Generic;

namespace MerchantTribeStore
{

    partial class BVAdmin_Configuration_Payment_Edit_Gateway : BaseAdminPage
    {

        private MerchantTribe.Payment.Methods.AvailableMethod g;
        private BVModule editor;

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.PageTitle = "Payment Gateway Settings";
            this.CurrentTab = AdminTabType.Configuration;
            ValidateCurrentUserHasPermission(SystemPermissions.SettingsView);
            if (this.editor != null)
            {
                this.editor.EditingComplete += this.editor_EditingComplete;
            }
        }

        protected override void OnInit(System.EventArgs e)
        {
            base.OnInit(e);

            if (Request.QueryString["id"] != null)
            {
                this.GatewayIdField.Value = Request.QueryString["id"];
            }
            if (Request.QueryString["payid"] != null)
            {
                this.MethodIdField.Value = Request.QueryString["payid"];
            }
            //if (AvailableGateways.CurrentGateway().GatewayId == this.GatewayIdField.Value) {
            //    g = AvailableGateways.CurrentGateway();
            //}
            //else {
            List<MerchantTribe.Payment.Methods.AvailableMethod> AvailableGateways = MerchantTribe.Payment.Methods.AvailableMethod.FindAll();

            for (int i = 0; i <= AvailableGateways.Count - 1; i++)
            {
                if (string.Compare(AvailableGateways[i].Id, this.GatewayIdField.Value, true) == 0)
                {
                    g = AvailableGateways[i];
                }
            }
            //}

            LoadEditor();
        }

        private void LoadEditor()
        {
            System.Web.UI.Control tempControl = ModuleController.LoadCreditCardGatewayEditor(g.Name, this);

            if (tempControl is BVModule)
            {
                editor = (BVModule)tempControl;
                if (editor != null)
                {
                    editor.BlockId = g.Id;
                    this.phEditor.Controls.Add(editor);
                    this.editor.EditingComplete += this.editor_EditingComplete;
                }
            }
            else
            {
                this.phEditor.Controls.Add(new System.Web.UI.LiteralControl("Error, editor is not based on Content.BVModule class"));
            }
        }

        protected void editor_EditingComplete(object sender, MerchantTribe.Commerce.Content.BVModuleEventArgs e)
        {
            Response.Redirect("Payment_Edit.aspx?id=" + this.MethodIdField.Value);
        }


    }

}