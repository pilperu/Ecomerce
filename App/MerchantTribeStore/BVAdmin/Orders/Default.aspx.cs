using System;
using MerchantTribe.Commerce.Orders;
using System.Collections.Generic;
using System.Text;
using MerchantTribe.Commerce;

namespace MerchantTribeStore
{

    partial class BVAdmin_Orders_Default : BaseAdminPage
    {

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.PageTitle = "Order Manager";
            this.CurrentTab = AdminTabType.Orders;
            ValidateCurrentUserHasPermission(MerchantTribe.Commerce.Membership.SystemPermissions.OrdersView);
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            this.DateRangePicker1.RangeTypeChanged += new BVAdmin_Controls_DateRangePicker.RangeTypeChangedDelegate(DateRangePicker1_RangeTypeChanged);
            if (!Page.IsPostBack)
            {
                LoadSessionKeys();
                // Force Mode on Request
                if (Request.QueryString["mode"] != null)
                {
                    switch (Request.QueryString["mode"].ToLower())
                    {
                        case "0":
                            SetListToValue(this.lstStatus, string.Empty);
                            break;
                        case "1":
                            SetListToValue(this.lstStatus, OrderStatusCode.Received);
                            break;
                        case "2":
                            SetListToValue(this.lstStatus, OrderStatusCode.ReadyForPayment);
                            break;
                        case "3":
                            SetListToValue(this.lstStatus, OrderStatusCode.ReadyForShipping);
                            break;
                        case "4":
                            SetListToValue(this.lstStatus, OrderStatusCode.Completed);
                            break;
                        case "5":
                            SetListToValue(this.lstStatus, OrderStatusCode.OnHold);
                            break;
                        case "6":
                            SetListToValue(this.lstStatus, OrderStatusCode.Cancelled);
                            break;
                    }
                    SetListToValue(this.lstStatus, Request.QueryString["mode"]);
                }

                LoadTemplates();
            }

            
            int pageNumber = 1;
            if (Request.QueryString["p"] != null)
            {
                string tempPage = Request.QueryString["p"];
                int temp = 0;
                if (int.TryParse(tempPage, out temp))
                {
                    if (temp > 0)
                    {
                        pageNumber = temp;
                    }
                }
            }
         
            MerchantTribe.Commerce.SessionManager.SetCookieString("AdminLastManager", "Default.aspx?p=" + pageNumber.ToString(), MTApp.CurrentStore);
            FindOrders(pageNumber);

            // Acumatica Warning
            if (MTApp.CurrentStore.Settings.Acumatica.IntegrationEnabled)
            {
                this.MessageBox1.ShowWarning(MerchantTribe.Commerce.Content.SiteTerms.GetTerm(MerchantTribe.Commerce.Content.SiteTermIds.AcumaticaWarning));
            }
        }

        private void LoadTemplates()
        {
            this.lstPrintTemplate.DataSource = MTApp.ContentServices.GetAllTemplatesForStoreOrDefaults();
            this.lstPrintTemplate.DataTextField = "DisplayName";
            this.lstPrintTemplate.DataValueField = "Id";
            this.lstPrintTemplate.DataBind();
        }

        // Searching
        private void FindOrders(int pageNumber)
        {
            OrderSearchCriteria criteria = new OrderSearchCriteria();
            criteria.IsPlaced = true;
            criteria.StatusCode = this.lstStatus.SelectedValue;
            if (this.lstPaymentStatus.SelectedValue != string.Empty)
            {
                criteria.PaymentStatus = (OrderPaymentStatus)(int.Parse(this.lstPaymentStatus.SelectedValue));
            }
            if (this.lstShippingStatus.SelectedValue != string.Empty)
            {
                criteria.ShippingStatus = (OrderShippingStatus)(int.Parse(this.lstShippingStatus.SelectedValue));
            }
            criteria.StartDateUtc = this.DateRangePicker1.StartDate.ToUniversalTime();
            criteria.EndDateUtc = this.DateRangePicker1.EndDate.ToUniversalTime();
            criteria.Keyword = this.FilterField.Text.Trim();
            criteria.SortDescending = this.chkNewestFirst.Checked;

            int pageSize = 20;
            int totalCount = 0;

            List<OrderSnapshot> orders = MTApp.OrderServices.Orders.FindByCriteriaPaged(criteria, pageNumber, pageSize, ref totalCount);

            RenderOrders(orders);

            this.litPager.Text = MerchantTribe.Web.Paging.RenderPager(Page.ResolveUrl("~/bvadmin/orders/default.aspx?p={0}"), pageNumber, totalCount, pageSize);
            this.litPager2.Text = this.litPager.Text;

            SaveSessionKeys(pageNumber);
            ShowCorrectBatchButtons(criteria.StatusCode);
        }
        private void SaveSessionKeys(int pageNumber)
        {
            SessionManager.AdminOrderSearchDateRange = this.DateRangePicker1.RangeType;
            SessionManager.AdminOrderSearchEndDate = this.DateRangePicker1.EndDate;
            SessionManager.AdminOrderSearchStartDate = this.DateRangePicker1.StartDate;
            SessionManager.AdminOrderSearchKeyword = this.FilterField.Text.Trim();
            SessionManager.AdminOrderSearchPaymentFilter = this.lstPaymentStatus.SelectedValue;
            SessionManager.AdminOrderSearchShippingFilter = this.lstShippingStatus.SelectedValue;
            SessionManager.AdminOrderSearchStatusFilter = this.lstStatus.SelectedValue;
            SessionManager.AdminOrderSearchLastPage = pageNumber;
            SessionManager.AdminOrderSearchNewestFirst = this.chkNewestFirst.Checked;
        }
        private void SetListToValue(System.Web.UI.WebControls.DropDownList l, string value)
        {
            if (l == null) return;
            if (l.Items.Count < 1) return;
            if (l.Items.FindByValue(value) != null)
            {
                l.ClearSelection();
                l.Items.FindByValue(value).Selected = true;
            }
        }
        private void LoadSessionKeys()
        {
            this.FilterField.Text = SessionManager.AdminOrderSearchKeyword;

            SetListToValue(this.lstPaymentStatus, SessionManager.AdminOrderSearchPaymentFilter);
            SetListToValue(this.lstShippingStatus, SessionManager.AdminOrderSearchShippingFilter);
            SetListToValue(this.lstStatus, SessionManager.AdminOrderSearchStatusFilter);
            this.DateRangePicker1.RangeType = SessionManager.AdminOrderSearchDateRange;
            if (this.DateRangePicker1.RangeType == MerchantTribe.Commerce.Utilities.DateRangeType.Custom)
            {
                this.DateRangePicker1.StartDate = SessionManager.AdminOrderSearchStartDate;
                this.DateRangePicker1.EndDate = SessionManager.AdminOrderSearchEndDate;
            }
            this.chkNewestFirst.Checked = SessionManager.AdminOrderSearchNewestFirst;
        }

        // Rendering
        private void RenderOrders(List<OrderSnapshot> orders)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("<table class=\"formtable\" width=\"100%\"><thead><tr>");
            sb.Append("<th><a href=\"#\" class=\"pickerallbutton\">All</a></th>");
            sb.Append("<th>Order #</th>");
            sb.Append("<th>Date</th>");
            sb.Append("<th>Amount</th>");
            sb.Append("<th>Customer</th>");
            sb.Append("<th>&nbsp;</th>");
            sb.Append("<th>&nbsp;</th>");
            sb.Append("<th>Status</th>");
            sb.Append("<th>&nbsp;</th>");
            sb.Append("</tr></thead>");

            bool altRow = false;
            TimeZoneInfo tz = MTApp.CurrentStore.Settings.TimeZone;
            foreach (OrderSnapshot o in orders)
            {
                RenderSingleOrder(o, sb, altRow, tz);
                altRow = !altRow;
            }
            sb.Append("</table>");

            this.litMain.Text = sb.ToString();
        }
        public enum ManagerPage
        {
            Any = 0,
            Payment = 1,
            Shipping = 2
        }
        public void RenderSingleOrder(OrderSnapshot o, StringBuilder sb, bool altRow, TimeZoneInfo timezone)
        {
            string url = ResolveUrl("~/bvadmin/orders/ViewOrder.aspx?id=" + o.bvin);
                        
            if (altRow)
            {
                sb.Append("<tr class=\"alternaterow\" ");
            }
            else
            {
                sb.Append("<tr class=\"row\" ");
            }
            string highlight = this.MTApp.OrderServices.FindHightlightColorForOrder(o);
            if (highlight.Length > 0)
            {
                sb.Append(" style=\"background:url('../images/highlights/" + highlight + ".png')\"");
            }
            sb.Append(">");

            sb.Append("<td><input class=\"pickercheck\" type=\"checkbox\" id=\"check" + o.bvin + "\" /></td>");

            sb.Append("<td><a href=\"" + url + "\">" + o.OrderNumber + "</a></td>");

            DateTime timeOfOrder = TimeZoneInfo.ConvertTimeFromUtc(o.TimeOfOrderUtc, timezone);
            DateTime currentTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timezone);
            sb.Append("<td><a href=\"" + url + "\">" + MerchantTribe.Web.Dates.FriendlyShortDate(timeOfOrder, currentTime.Year) + "</a></td>");
            sb.Append("<td><a href=\"" + url + "\">" + string.Format("{0:c}", o.TotalGrand) + "</a></td>");

            sb.Append("<td>");
            sb.Append(MerchantTribe.Commerce.Utilities.MailServices.MailToLink(o.UserEmail, "Order "
                                                                  + o.OrderNumber,
                                                                  o.BillingAddress.FirstName + ",",
                                                                  o.BillingAddress.FirstName + " " + o.BillingAddress.LastName));
            sb.Append("</td>");



            string payText = MerchantTribe.Commerce.Utilities.EnumToString.OrderPaymentStatus(o.PaymentStatus);
            string payImage = "";
            string payLink = ResolveUrl("~/bvadmin/orders/ReceivePayments.aspx?id=" + o.bvin);
            switch (o.PaymentStatus)
            {
                case OrderPaymentStatus.Overpaid:
                    payImage = ResolveUrl("~/BVAdmin/Images/Lights/PaymentError.gif");
                    break;
                case OrderPaymentStatus.PartiallyPaid:
                    payImage = ResolveUrl("~/BVAdmin/Images/Lights/PaymentAuthorized.gif");
                    break;
                case OrderPaymentStatus.Paid:
                    payImage = ResolveUrl("~/BVAdmin/Images/Lights/PaymentComplete.gif");
                    break;
                case OrderPaymentStatus.Unknown:
                    payImage = ResolveUrl("~/BVAdmin/Images/Lights/PaymentNone.gif");
                    break;
                case OrderPaymentStatus.Unpaid:
                    payImage = ResolveUrl("~/BVAdmin/Images/Lights/PaymentNone.gif");
                    break;
            }
            sb.Append("<td><a href=\"" + payLink + "\" title=\"" + payText + "\"><img src=\"" + payImage + "\" alt=\"" + payText + "\" /></a></td>");


            string shipText = MerchantTribe.Commerce.Utilities.EnumToString.OrderShippingStatus(o.ShippingStatus);
            string shipImage = "";
            string shipLink = ResolveUrl("~/bvadmin/orders/ShipOrder.aspx?id=" + o.bvin);
            switch (o.ShippingStatus)
            {
                case OrderShippingStatus.FullyShipped:
                    shipImage = ResolveUrl("~/BVAdmin/Images/Lights/ShippingShipped.gif");
                    break;
                case OrderShippingStatus.NonShipping:
                    shipImage = ResolveUrl("~/BVAdmin/Images/Lights/ShippingNone.gif");
                    break;
                case OrderShippingStatus.PartiallyShipped:
                    shipImage = ResolveUrl("~/BVAdmin/Images/Lights/ShippingPartially.gif");
                    break;
                case OrderShippingStatus.Unknown:
                    shipImage = ResolveUrl("~/BVAdmin/Images/Lights/ShippingNone.gif");
                    break;
                case OrderShippingStatus.Unshipped:
                    shipImage = ResolveUrl("~/BVAdmin/Images/Lights/ShippingNone.gif");
                    break;
            }
            sb.Append("<td><a href=\"" + shipLink + "\" title=\"" + shipText + "\"><img src=\"" + shipImage + "\" alt=\"" + shipText + "\" /></a></td>");

            ManagerPage pageType = ManagerPage.Any;
            string statImage = "";
            switch (o.StatusCode)
            {
                case OrderStatusCode.Completed:
                    statImage = ResolveUrl("~/bvadmin/images/lights/OrderComplete.gif");
                    break;
                case OrderStatusCode.Received:
                    statImage = ResolveUrl("~/bvadmin/images/lights/OrderInProcess.gif");
                    break;
                case OrderStatusCode.OnHold:
                    statImage = ResolveUrl("~/bvadmin/images/lights/OrderOnHold.gif");
                    break;
                case OrderStatusCode.ReadyForPayment:
                    pageType = ManagerPage.Payment;
                    statImage = ResolveUrl("~/bvadmin/images/lights/OrderInProcess.gif");
                    break;
                case OrderStatusCode.ReadyForShipping:
                    pageType = ManagerPage.Shipping;
                    statImage = ResolveUrl("~/bvadmin/images/lights/OrderInProcess.gif");
                    break;
                default:
                    statImage = ResolveUrl("~/bvadmin/images/lights/OrderInProcess.gif");
                    break;
            }
            sb.Append("<td><a href=\"" + url + "\"><img src=\"" + statImage + "\" alt=\"" + o.StatusName + "\" /> " + o.StatusName + "</a></td>");

            switch (pageType)
            {
                case ManagerPage.Payment:
                    sb.Append("<td><a href=\"" + payLink + "\"><img src=\"" + ResolveUrl("~/BVAdmin/Images/Buttons/Payment.png") + "\"/></a></td>");
                    break;
                case ManagerPage.Shipping:
                    sb.Append("<td><a href=\"" + shipLink + "\"><img src=\"" + ResolveUrl("~/BVAdmin/Images/Buttons/Shipping.png") + "\"/></a></td>");
                    break;
                default:
                    sb.Append("<td><a href=\"" + url + "\"><img src=\"" + ResolveUrl("~/BVAdmin/Images/Buttons/Details.png") + "\"/></a></td>");
                    break;
            }

            sb.Append("</tr>");
        }

                
        // Search & Auto Post Back Handlers
        protected void btnGo_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            FindOrders(1);
        }
        protected void lstStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            FindOrders(1);
        }
        protected void lstPaymentStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            FindOrders(1);
        }
        protected void lstShippingStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            FindOrders(1);
        }
        void DateRangePicker1_RangeTypeChanged(object sender, EventArgs e)
        {
            FindOrders(1);
        }
        protected void chkNewestFirst_CheckedChanged(object sender, EventArgs e)
        {
            FindOrders(1);
        }

        // Batch Actions
        private void ShowCorrectBatchButtons(string statusCode)
        {
            this.OrderManagerActions.Visible = false;
            this.lnkAcceptAll.Visible = false;
            this.lnkPrintPacking.Visible = false;
            this.lnkShipAll.Visible = false;
            this.lnkChargeAll.Visible = false;

            switch (statusCode)
            {
                case OrderStatusCode.Received:
                    this.lnkAcceptAll.Visible = true;
                    this.OrderManagerActions.Visible = true;
                    this.litH1.Text = "New Orders";
                    break;
                case OrderStatusCode.Cancelled:
                    this.litH1.Text = "Cancelled Orders";
                    break;
                case OrderStatusCode.Completed:
                    this.litH1.Text = "Completed Orders";
                    break;
                case OrderStatusCode.OnHold:
                    this.litH1.Text = "Orders On Hold";
                    break;
                case OrderStatusCode.ReadyForPayment:
                    this.litH1.Text = "Orders Ready for Payment";
                    this.lnkChargeAll.Visible = true;
                    this.OrderManagerActions.Visible = true;
                    break;
                case OrderStatusCode.ReadyForShipping:
                    this.litH1.Text = "Orders Ready for Shipping";
                    //this.lnkShipAll.Visible = true;
                    //this.lnkPrintPacking.Visible = true;
                    break;
                default:
                    this.litH1.Text = "Order Manager";
                    break;
            }

        }
        protected void lnkAcceptAll_Click(object sender, EventArgs e)
        {
            OrderBatchProcessor.AcceptAllNewOrders(MTApp.OrderServices);
            FindOrders(1);
        }
        protected void lnkShipAll_Click(object sender, EventArgs e)
        {
            FindOrders(1);
        }
        protected void lnkPrintPacking_Click(object sender, EventArgs e)
        {
            FindOrders(1);
        }
        protected void lnkChargeAll_Click(object sender, EventArgs e)
        {
            OrderBatchProcessor.CollectPaymentAndShipPendingOrders(MTApp);
            FindOrders(1);
        }

        
        
    }

}