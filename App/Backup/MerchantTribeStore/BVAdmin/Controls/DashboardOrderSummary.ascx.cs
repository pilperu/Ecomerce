using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MerchantTribe.Commerce.Reporting;

namespace MerchantTribeStore
{

    public partial class BVAdmin_Controls_DashboardOrderSummary : MerchantTribe.Commerce.Content.BVUserControl
    {
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            OrderSummary summary = new OrderSummary(MyPage.MTApp.CurrentStore.Id);
            this.litNewCount.Text = summary.NewOrderCount.ToString();
            this.litHoldCount.Text = summary.OnHoldOrderCount.ToString();
            this.litPaymentCount.Text = summary.ReadyForPaymentOrderCount.ToString();
            this.litShippingCount.Text = summary.ReadyForShippingOrderCount.ToString();
        }
    }
}