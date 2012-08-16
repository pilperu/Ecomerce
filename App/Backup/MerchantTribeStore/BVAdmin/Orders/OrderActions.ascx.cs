using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MerchantTribe.Commerce.Orders;
using MerchantTribe.Commerce;

namespace MerchantTribeStore
{

    public partial class BVAdmin_Orders_OrderActions : MerchantTribe.Commerce.Content.BVUserControl
    {
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (!Page.IsPostBack)
            {
                string id = Request.QueryString["id"];
                this.lnkDetails.NavigateUrl = "ViewOrder.aspx?id=" + id;
                this.lnkEditOrder.NavigateUrl = "EditOrder.aspx?id=" + id;
                this.lnkPayment.NavigateUrl = "ReceivePayments.aspx?id=" + id;
                this.lnkPrint.NavigateUrl = "PrintOrder.aspx?id=" + id;
                this.lnkShipping.NavigateUrl = "ShipOrder.aspx?id=" + id;

                string lastManagerPage = SessionManager.GetCookieString("AdminLastManager", MyPage.MTApp.CurrentStore);
                if (!String.IsNullOrEmpty(lastManagerPage))
                {
                    this.lnkManager.NavigateUrl = lastManagerPage;
                }
                else
                {
                    int lastPage = SessionManager.AdminOrderSearchLastPage;
                    if (lastPage < 1)
                    {
                        lastPage = 1;
                    }
                    this.lnkManager.NavigateUrl = "Default.aspx?p=" + lastPage.ToString();
                }

            }


        }


    }
}