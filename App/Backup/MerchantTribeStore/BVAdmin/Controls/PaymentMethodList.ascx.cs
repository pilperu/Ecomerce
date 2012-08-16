
using MerchantTribe.Commerce;
using MerchantTribe.Commerce.Payment;
using MerchantTribe.Commerce.Content;

namespace MerchantTribeStore
{

    partial class BVAdmin_Controls_PaymentMethodList : MerchantTribe.Commerce.Content.BVUserControl
    {

        protected MerchantTribe.Commerce.Payment.AvailablePayments availablePayments = new MerchantTribe.Commerce.Payment.AvailablePayments();

        public string SelectMethodId
        {
            get
            {
                string result = string.Empty;
                if (this.lstPaymentMethods.SelectedItem != null)
                {
                    result = this.lstPaymentMethods.SelectedValue;
                }
                return result;
            }
            set
            {
                if (this.lstPaymentMethods.Items.FindByValue(value) != null)
                {
                    this.lstPaymentMethods.ClearSelection();
                    this.lstPaymentMethods.Items.FindByValue(value).Selected = true;
                }
            }
        }

        protected override void OnInit(System.EventArgs e)
        {
            base.OnInit(e);
            if (!Page.IsPostBack)
            {
                PopulateMethodList();
            }
            LoadView();
        }

        private void LoadView()
        {
            this.phOptions.Controls.Clear();

            DisplayPaymentMethod m = FindMethod(this.lstPaymentMethods.SelectedValue);
            System.Web.UI.Control tempControl = ModuleController.LoadPaymentMethodView(m.MethodName, this.Page);
            if (tempControl is BVModule)
            {
                BVModule view;
                view = (BVModule)tempControl;
                if (!(view == null))
                {
                    view.BlockId = m.MethodId;
                    this.phOptions.Controls.Add(view);
                }
            }
            else
            {
                //Me.phOptions.Controls.Add(New LiteralControl("Error, editor is not based on Content.BVModule class"))
            }
        }

        private DisplayPaymentMethod FindMethod(string methodId)
        {
            DisplayPaymentMethod result = new MerchantTribe.Commerce.Payment.Method.NullPayment();

            System.Collections.ObjectModel.Collection<DisplayPaymentMethod> enabled = availablePayments.EnabledMethods(MyPage.MTApp.CurrentStore);

            for (int i = 0; i <= enabled.Count - 1; i++)
            {
                if (enabled[i].MethodId == this.lstPaymentMethods.SelectedValue)
                {
                    result = enabled[i];
                    break;
                }
            }

            return result;
        }

        private void PopulateMethodList()
        {
            this.lstPaymentMethods.Items.Clear();

            System.Collections.ObjectModel.Collection<DisplayPaymentMethod> enabled = availablePayments.EnabledMethods(MyPage.MTApp.CurrentStore);
            for (int i = 0; i <= enabled.Count - 1; i++)
            {
                lstPaymentMethods.Items.Add(new System.Web.UI.WebControls.ListItem(enabled[i].MethodId, enabled[i].MethodId));
            }
        }

        protected void lstPaymentMethods_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            LoadView();
        }

    }
}