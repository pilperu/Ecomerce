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
using MerchantTribe.Shipping;

namespace MerchantTribeStore
{

    partial class BVAdmin_Configuration_Shipping_EditMethod : BaseAdminPage
    {

        private ShippingMethod m;
        private BVShippingModule editor;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (!Page.IsPostBack)
            {

            }
        }

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.PageTitle = "Edit Shipping Method";
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
                this.BlockIDField.Value = Request.QueryString["id"];
            }
            m = MTApp.OrderServices.ShippingMethods.Find(this.BlockIDField.Value);
            LoadEditor();
        }

        private IShippingService FindProvider(string bvin)
        {
            return MerchantTribe.Commerce.Shipping.AvailableServices.FindById(bvin, MTApp.CurrentStore);
        }

        private void LoadEditor()
        {
            System.Web.UI.Control tempControl = null;

            MerchantTribe.Shipping.IShippingService p = MerchantTribe.Commerce.Shipping.AvailableServices.FindById(m.ShippingProviderId, MTApp.CurrentStore);

            tempControl = ModuleController.LoadShippingEditor(p.Name, this);

            if (tempControl is BVShippingModule)
            {
                editor = (BVShippingModule)tempControl;
                if (editor != null)
                {
                    editor.BlockId = m.Bvin;
                    editor.ShippingMethod = m;
                    this.phEditor.Controls.Add(editor);
                    this.editor.EditingComplete += this.editor_EditingComplete;
                }
            }
            else
            {
                this.phEditor.Controls.Add(new System.Web.UI.LiteralControl("Error, editor is not based on Content.BVShippingModule class"));
            }
        }

        protected void editor_EditingComplete(object sender, MerchantTribe.Commerce.Content.BVModuleEventArgs e)
        {
            if ((e.Info.ToUpper() == "CANCELED"))
            {
                if ((Request.QueryString["doc"] == "1"))
                {
                    MTApp.OrderServices.ShippingMethods.Delete(m.Bvin);
                }
            }
            else
            {
                if (e.Info != string.Empty)
                {
                    m.Name = e.Info;
                    MTApp.OrderServices.ShippingMethods.Update(m);
                }
            }
            Response.Redirect("Shipping.aspx");
        }

    }
}