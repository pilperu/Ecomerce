using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using MerchantTribe.Commerce;
using MerchantTribe.Commerce.Membership;
using MerchantTribe.Commerce.Orders;

namespace MerchantTribeStore
{

    partial class BVModules_Reports_Shopping_Carts_Default : BaseAdminPage
    {

        private decimal TotalSub = 0;
        private int TotalCount = 0;

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.PageTitle = "Reports - Shopping Carts";
            this.CurrentTab = AdminTabType.Reports;
            ValidateCurrentUserHasPermission(SystemPermissions.ReportsView);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (!Page.IsPostBack)
            {
                LoadData();
            }
        }

        void LoadData()
        {

            try
            {

                TotalSub = 0;
                TotalCount = 0;

                OrderSearchCriteria c = new OrderSearchCriteria();
                c.IsPlaced = false;

                List<OrderSnapshot> found = new List<OrderSnapshot>();
                int totalCarts = 0;
                found = MTApp.OrderServices.Orders.FindByCriteriaPaged(c, 1, 1000, ref totalCarts);

                TotalCount = found.Count;

                foreach (OrderSnapshot o in found)
                {
                    TotalSub += o.TotalOrderBeforeDiscounts;
                }

                dgList.DataSource = found;
                dgList.DataBind();
            }

            catch (Exception Ex)
            {
                msg.ShowException(Ex);
                EventLog.LogEvent(Ex);
            }

        }

        protected void dgList_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Order order = (Order)e.Item.DataItem;
            }

            if (e.Item.ItemType == ListItemType.Footer)
            {
                e.Item.Cells[0].Text = "Totals:";
                e.Item.Cells[2].Text = string.Format("{0:C}", TotalSub);
            }

        }

        protected void dgList_Edit(object sender, DataGridCommandEventArgs e)
        {
            string bvin = Convert.ToString(dgList.DataKeys[e.Item.ItemIndex]);
            Response.Redirect("~/BVAdmin/Orders/ViewOrder.aspx?id=" + bvin);
        }

    }
}