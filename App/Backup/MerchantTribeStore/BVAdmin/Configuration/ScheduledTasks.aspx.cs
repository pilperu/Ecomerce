using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MerchantTribe.Commerce.Scheduling;
using System.Text;

namespace MerchantTribeStore.BVAdmin.Configuration
{
    public partial class ScheduledTasks : BaseAdminPage
    {
        private int pageSize = 50;
        private int rowCount = 0;
        private int currentPage = 1;

        protected override void OnPreInit(System.EventArgs e)
        {
            base.OnPreInit(e);
            this.PageTitle = "Scheduled Tasks";
            this.CurrentTab = AdminTabType.Configuration;
            ValidateCurrentUserHasPermission(MerchantTribe.Commerce.Membership.SystemPermissions.SettingsView);
        }

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);

            if (!Page.IsPostBack)
            {
                if ((Request.QueryString["page"] != null))
                {
                    int.TryParse(Request.QueryString["page"], out currentPage);
                    if ((currentPage < 1))
                    {
                        currentPage = 1;
                    }
                }
                LoadData();
            }
        }

        private void LoadData()
        {
            List<QueuedTask> items = MTApp.ScheduleServices.QueuedTasks.FindAllPaged(currentPage, pageSize);
            //this.lblResults.Text = rowCount.ToString() + " found";
            //this.litPager1.Text = MerchantTribe.Web.Paging.RenderPagerWithLimits("Promotions.aspx?page={0}&showdisabled=" + (this.chkShowDisabled.Checked ? "1" : "0") + "&keyword=" + System.Web.HttpUtility.UrlEncode(keyword), currentPage, rowCount, pageSize, 20);
            RenderItems(items);
            //this.litPager2.Text = this.litPager1.Text;
        }

        private void RenderItems(List<QueuedTask> items)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("<table width=\"100%\"><tr class=\"rowheader\">");
            sb.Append("<th style=\"text-align:left;\">Task</th>");
            sb.Append("<th style=\"text-align:left;\">Start At</th>");
            sb.Append("<th style=\"text-align:left;\">Status</th>");
            sb.Append("<th style=\"text-align:left;\">Notes</th>");
            sb.Append("</tr>");

            foreach (QueuedTask t in items)
            {
                RenderSingleItem(sb, t);
            }

            sb.Append("</table>");
            this.litResults.Text = sb.ToString();
        }

        private void RenderSingleItem(StringBuilder sb, QueuedTask t)
        {
            //string destinationLink = "Promotions_edit.aspx?id=" + p.Id + "&page=" + currentPage + "&showdisabled="
            //                  + (this.chkShowDisabled.Checked ? "1" : "0") +
            //                  "&keyword=" + System.Web.HttpUtility.UrlEncode(keyword);
            //string deleteLink = destinationLink.Replace("_edit", "_delete");
            
            sb.Append("<tr>");
            sb.Append("<td>" + t.FriendlyName + "<br />");
            sb.Append(t.TaskProcessorName + "</td>");

            DateTime startTime = TimeZoneInfo.ConvertTimeFromUtc(t.StartAtUtc, MTApp.CurrentStore.Settings.TimeZone);
            sb.Append("<td>" + startTime.ToString() + " </td>");
            sb.Append("<td>");
            switch (t.Status)
            {
                case QueuedTaskStatus.Running:
                    sb.Append("<span style=\"color:#006\">Running</span>");
                    break;
                case QueuedTaskStatus.Unknown:
                    sb.Append("<span style=\"color:#999\">Unknown</span>");
                    break;
                case QueuedTaskStatus.Failed:
                    sb.Append("<span style=\"color:#600\">Failed</span>");
                    break;
                case QueuedTaskStatus.Completed:
                    sb.Append("<span style=\"color:#060;\">Complete</span>");
                    break;
                case QueuedTaskStatus.Pending:
                    sb.Append("<span style=\"color:#ff0;background-color:#333;padding:3px;display:block;\">Pending</span>");
                    break;
            }
            sb.Append("</td>");
            sb.Append("<td><div style=\"width:250px;height:60px;overflow:auto;\">" + t.StatusNotes + "</div></td>");
            sb.Append("</tr>");

            //sb.Append("<td><a href=\"" + destinationLink + "\">" + (p.IsEnabled ? "<span style=\"color:#060\">Yes</span" : "<span style=\"color:#999\">No</span>") + "</a></td>");
            //sb.Append("<td><a onclick=\"return window.confirm('Delete this item?');\" href=\"" + deleteLink + "\" class=\"btn\"><b>Delete</b></a></td>");
            //sb.Append("<td><a href=\"" + destinationLink + "\" class=\"btn\"><b>Edit</b></a></td></tr>");
        }
    }
}