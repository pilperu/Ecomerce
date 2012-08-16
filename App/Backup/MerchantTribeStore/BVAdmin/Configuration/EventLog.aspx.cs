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
using MerchantTribe.Web.Logging;

using System.Collections.Generic;

namespace MerchantTribeStore
{

    partial class BVAdmin_Configuration_EventLog : BaseAdminPage
    {

        protected EventLogRepository _Repository = null;

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.PageTitle = "EventLog";
            this.CurrentTab = AdminTabType.Configuration;
            ValidateCurrentUserHasPermission(SystemPermissions.SettingsView);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            _Repository = new EventLogRepository(MTApp.CurrentRequestContext);

            if (!Page.IsPostBack)
            {
                LoadEventsByCriteria();
            }            
        }

        void LoadEvents(EventLogSeverity severity)
        {
            
            List<EventLogEntry> e = new List<EventLogEntry>();
            int pageNumber = 1;

            e = _Repository.FindBySeverityPaged(severity, pageNumber, 50);

            RenderEvents(e);
            //this.repeater1.DataSource = e;
            //this.repeater1.DataBind();
        }

        private void RenderEvents(List<EventLogEntry> events)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<table class=\"formtable\" width=\"100%\">");
            sb.Append("<tr>");
            sb.Append("<th>&nbsp;</th>");
            sb.Append("<th>Date</th>");
            sb.Append("<th>Message</th>");
            sb.Append("</tr>");

            foreach(EventLogEntry entry in events)
            {
                sb.Append("<tr>");
                
                sb.Append("<td>");
                sb.Append("<img src=\"../images/messageicons/" + entry.Severity.ToString() + ".png\" alt=\"" + entry.Severity.ToString() + "\" />");
                sb.Append("</td>");

                sb.Append("<td>" + entry.EventTimeUtc.ToString() + "</td>");
                sb.Append("<td><div style=\"width:700px;height:75px;overflow:auto;border:solid 1px #ccc;\">" + System.Web.HttpUtility.HtmlEncode(entry.Source) + "<br />");
                sb.Append(System.Web.HttpUtility.HtmlEncode(entry.Message) + "</div></td>");

                sb.Append("</tr>");
            }
                                                                
            sb.Append("</table>");
            this.litOutput.Text = sb.ToString();
        }
        void LoadEventsByCriteria()
        {
            EventLogSeverity s = EventLogSeverity.None;
            try
            {
                int temp = int.Parse(this.ddlFilter.SelectedValue);
                s = (EventLogSeverity)temp;
            }
            catch
            {

            }

            LoadEvents(s);            
        }

        protected void ClearEventsButton_Click(System.Object sender, System.Web.UI.ImageClickEventArgs e)
        {
            _Repository.DeleteAllForStore(MTApp.CurrentStore.Id);            
            LoadEvents(EventLogSeverity.None);
        }

        //protected void gvEvents_PageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        //{
        //    gvEvents.PageIndex = e.NewPageIndex;
        //    LoadEventsByCriteria();
        //}

        //protected void dgEvents_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
        //{
        //    string deleteID = (string)gvEvents.DataKeys[e.RowIndex].Value;
        //    _Repository.Delete(deleteID);
        //    LoadEventsByCriteria();
        //}

        protected void btnNext_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            LoadEventsByCriteria();
        }
    }
}