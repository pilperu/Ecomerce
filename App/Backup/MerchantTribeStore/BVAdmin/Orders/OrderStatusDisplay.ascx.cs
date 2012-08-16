using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MerchantTribe.Commerce.Orders;
using MerchantTribe.Commerce.Utilities;
using MerchantTribe.Commerce.Content;

namespace MerchantTribeStore
{

    public partial class BVAdmin_Orders_OrderStatusDisplay : BVUserControl
    {
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (!Page.IsPostBack)
            {
                string id = Request.QueryString["id"];
                Order o = MyPage.MTApp.OrderServices.Orders.FindForCurrentStore(id);
                LoadStatusForOrder(o);
            }
        }

        public void LoadStatusForOrder(Order o)
        {

            if (o != null)
            {

                this.litPay.Text = EnumToString.OrderPaymentStatus(o.PaymentStatus);
                this.litShip.Text = EnumToString.OrderShippingStatus(o.ShippingStatus);

                if (lstStatus.Items.FindByValue(o.StatusCode) != null)
                {
                    lstStatus.ClearSelection();
                    lstStatus.Items.FindByValue(o.StatusCode).Selected = true;
                }
            }
        }

        protected void lstStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            string id = Request.QueryString["id"];
            Order o = MyPage.MTApp.OrderServices.Orders.FindForCurrentStore(id);
            if (o != null)
            {
                o.StatusCode = this.lstStatus.SelectedItem.Value;
                o.StatusName = this.lstStatus.SelectedItem.Text;
                MyPage.MTApp.OrderServices.Orders.Update(o);
            }
        }
    }
}