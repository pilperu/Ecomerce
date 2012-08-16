using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using MerchantTribe.Commerce;
using MerchantTribe.Commerce.Membership;
using MerchantTribe.Commerce.Orders;
using MerchantTribe.Commerce.Utilities;

namespace MerchantTribeStore
{

    partial class BVAdmin_Reports_Default : BaseAdminPage
    {

        private decimal TotalSub = 0;
        private decimal TotalShip = 0;
        private decimal TotalHandling = 0;
        private decimal TotalTax = 0;
        private decimal TotalGrand = 0;
        private int TotalCount = 0;
        private decimal TotalShipDiscounts = 0;
        private decimal TotalDiscounts = 0;

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.PageTitle = "Reports";
            this.CurrentTab = AdminTabType.Reports;
            ValidateCurrentUserHasPermission(SystemPermissions.ReportsView);

        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.DateRangeField.RangeTypeChanged += this.DateRangeField_RangeTypeChanged;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (!Page.IsPostBack)
            {
                FillList();
                
                SetImageURL(System.DateTime.Today.Month.ToString(), System.DateTime.Today.Year.ToString());
            }

        }
      
        protected void btnShow_Click(System.Object sender, System.Web.UI.ImageClickEventArgs e)
        {
            dgList.CurrentPageIndex = 0;
            FillList();
            string sDate = this.DateRangeField.StartDate.ToString();
            string eDate = this.DateRangeField.EndDate.ToString();
            SetImageURL(sDate, eDate);
        }

        void FillList()
        {

            try
            {

                TotalSub = 0;
                TotalShip = 0;
                TotalHandling = 0;
                TotalTax = 0;
                TotalGrand = 0;
                TotalCount = 0;
                TotalDiscounts = 0;
                TotalShipDiscounts = 0;

                OrderSearchCriteria c = new OrderSearchCriteria();

                // Get Local Times
                TimeZoneInfo timezone = MTApp.CurrentStore.Settings.TimeZone;
                DateTime zonedStart = this.DateRangeField.StartDateForZone(timezone);
                DateTime zonedEnd = this.DateRangeField.EndDateForZone(timezone);

                // Convert to UTC
                DateTime utcStart = TimeZoneInfo.ConvertTimeToUtc(zonedStart, timezone);
                DateTime utcEnd = TimeZoneInfo.ConvertTimeToUtc(zonedEnd, timezone);
                
                c.StartDateUtc = utcStart;
                c.EndDateUtc = utcEnd;

                List<OrderSnapshot> found = new List<OrderSnapshot>();
                found = MTApp.OrderServices.Orders.FindByCriteria(c);

                TotalCount = found.Count;

                foreach (OrderSnapshot o in found)
                {
                    TotalSub += o.TotalOrderBeforeDiscounts;
                    TotalDiscounts += o.TotalOrderDiscounts;
                    TotalShip += o.TotalShippingBeforeDiscounts;
                    TotalShipDiscounts += o.TotalShippingDiscounts;
                    TotalHandling += o.TotalHandling;
                    TotalTax += o.TotalTax + o.TotalTax2;
                    TotalGrand += o.TotalGrand;
                }

                int i = 0;
                string month = string.Empty;
                decimal monthTotal = 0m;
                decimal dayTotal = 0m;
                string day = string.Empty;
                if (found.Count > 0)
                {
                    DateTime zonedTimeOfOrder = TimeZoneInfo.ConvertTimeFromUtc(found[0].TimeOfOrderUtc, timezone);
                    month = zonedTimeOfOrder.Month.ToString() + ":" + zonedTimeOfOrder.Year.ToString();
                    day = zonedTimeOfOrder.DayOfYear.ToString() + ":" + zonedTimeOfOrder.Year.ToString();
                    while (i <= found.Count - 1)
                    {
                        DateTime zonedTime = TimeZoneInfo.ConvertTimeFromUtc(found[i].TimeOfOrderUtc, timezone);
                        monthTotal = monthTotal + found[i].TotalGrand;
                        dayTotal = dayTotal + found[i].TotalGrand;
                        if (zonedTime.DayOfYear.ToString() + ":" + zonedTime.Year.ToString() != day)
                        {
                            day = zonedTime.DayOfYear.ToString() + ":" + zonedTime.Year.ToString();
                            // we need to insert a day total
                            OrderSnapshot order = new OrderSnapshot();
                            order.OrderNumber = "DayTotal";
                            order.TotalGrand = (dayTotal - found[i].TotalGrand);
                            dayTotal = found[i].TotalGrand;
                            found.Insert(i, order);
                            i += 1;
                        }

                        if (zonedTime.Month.ToString() + ":" + zonedTime.Year.ToString() != month)
                        {
                            month = zonedTime.Month.ToString() + ":" + zonedTime.Year.ToString();
                            // we need to insert a month total
                            OrderSnapshot order = new OrderSnapshot();
                            order.OrderNumber = "MonthTotal";
                            order.TotalGrand = (monthTotal - found[i].TotalGrand);
                            monthTotal = found[i].TotalGrand;
                            found.Insert(i, order);
                            i += 1;
                        }
                        i += 1;
                    }
                    if (dayTotal > 0)
                    {
                        // we need to insert a day total
                        OrderSnapshot order = new OrderSnapshot();
                        order.OrderNumber = "DayTotal";
                        order.TotalGrand = dayTotal;
                        found.Add(order);
                    }
                    if (monthTotal > 0)
                    {
                        OrderSnapshot order = new OrderSnapshot();
                        order.OrderNumber = "MonthTotal";
                        order.TotalGrand = monthTotal;
                        found.Add(order);
                    }
                }
                lblResponse.Text = "<b>" + TotalCount + "</b>";
                lblResponse.Text += " Orders Totaling <b>";
                lblResponse.Text += string.Format("{0:c}", TotalGrand);
                lblResponse.Text += "</b>";

                dgList.DataSource = found;
                dgList.DataBind();
            }

            catch (Exception Ex)
            {
                msg.ShowException(Ex);
                EventLog.LogEvent(Ex);
            }

        }

        void SetImageURL(string sMonth, string sYear)
        {
            string sURL;
            sURL = "reports_sales_graph.aspx?";
            sURL += "DateCode=";
            if (sMonth.Length < 2)
            {
                sURL += "0";
            }
            sURL += sMonth + sYear;
            //imgGraph.ImageUrl = sURL
            sURL = null;
        }

        protected void dgList_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                OrderSnapshot order = (OrderSnapshot)e.Item.DataItem;

                Label lblDate = (Label)e.Item.FindControl("lblDate");
                if (lblDate != null)
                {
                    lblDate.Text = TimeZoneInfo.ConvertTimeFromUtc(order.TimeOfOrderUtc, MTApp.CurrentStore.Settings.TimeZone).ToShortDateString();
                }

                HyperLink lnkViewOrder = (HyperLink)e.Item.FindControl("lnkViewOrder");
                if (lnkViewOrder != null)
                {
                    lnkViewOrder.NavigateUrl = "~/BVAdmin/Orders/ViewOrder.aspx?id=" + order.bvin;
                }

                if (order.OrderNumber == "DayTotal")
                {
                    TableCell cell = new TableCell();
                    cell.ColumnSpan = e.Item.Cells.Count;
                    cell.Text = "Day Total: " + string.Format("{0:c}", order.TotalGrand);
                    e.Item.Cells.Clear();
                    e.Item.Cells.Add(cell);
                    e.Item.ControlStyle.CssClass = "separator";
                }
                else if (order.OrderNumber == "MonthTotal")
                {
                    TableCell cell = new TableCell();
                    cell.ColumnSpan = e.Item.Cells.Count;
                    cell.Text = "Month Total: " + string.Format("{0:c}", order.TotalGrand);
                    e.Item.Cells.Clear();
                    e.Item.Cells.Add(cell);
                    e.Item.ControlStyle.CssClass = "separator";
                }
            }

            if (e.Item.ItemType == ListItemType.Footer)
            {
                e.Item.Cells[0].Text = "Totals:";
                e.Item.Cells[2].Text = string.Format("{0:C}", TotalSub);
                e.Item.Cells[3].Text = string.Format("{0:C}", TotalDiscounts);
                e.Item.Cells[4].Text = string.Format("{0:C}", TotalShip);
                e.Item.Cells[5].Text = string.Format("{0:C}", TotalShipDiscounts);                
                e.Item.Cells[6].Text = string.Format("{0:C}", TotalTax);
                e.Item.Cells[7].Text = string.Format("{0:C}", TotalGrand);
            }

        }

        protected void DateRangeField_RangeTypeChanged(System.EventArgs e)
        {
            if (DateRangeField.RangeType != DateRangeType.Custom)
            {
                FillList();
            }
        }

    }
}