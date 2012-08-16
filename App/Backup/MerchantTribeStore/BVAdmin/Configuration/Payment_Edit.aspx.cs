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

    partial class BVAdmin_Configuration_Payment_Edit : BaseAdminPage
    {

        private DisplayPaymentMethod m;
        private BVModule editor;
        private MerchantTribe.Commerce.Payment.AvailablePayments availablePayments = new MerchantTribe.Commerce.Payment.AvailablePayments();

        public override bool RequiresSSL
        {
            get { return true; }
        }

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.PageTitle = "Payment Settings";
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
                this.MethodIdField.Value = Request.QueryString["id"];
            }
            for (int i = 0; i <= availablePayments.Methods.Count - 1; i++)
            {
                if (string.Compare(availablePayments.Methods[i].MethodId, this.MethodIdField.Value, true) == 0)
                {
                    m = availablePayments.Methods[i];
                }
            }
            LoadEditor();
        }

        private void LoadEditor()
        {
            System.Web.UI.Control tempControl = ModuleController.LoadPaymentMethodEditor(m.MethodName, this);

            if (tempControl is BVModule)
            {
                editor = (BVModule)tempControl;
                if (editor != null)
                {
                    editor.BlockId = m.MethodId;
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
            Response.Redirect("Payment.aspx");
        }

    }
}