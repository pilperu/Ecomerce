using System;
using System.Web;
using System.Web.UI.WebControls;
using MerchantTribe.Commerce.Membership;
using MerchantTribe.Commerce.Content;
using MerchantTribe.Commerce;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Collections.Generic;
using MerchantTribe.Web.Logging;

namespace MerchantTribeStore
{

    partial class BVAdmin_Content_Policies : BaseAdminPage
    {
        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.PageTitle = "Policies";
            this.CurrentTab = AdminTabType.Content;
            ValidateCurrentUserHasPermission(SystemPermissions.ContentView);
        }

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);

            if (!Page.IsPostBack)
            {
                LoadPolicies();
            }
        }

        private void LoadPolicies()
        {
            List<Policy> p;
            p = MTApp.ContentServices.Policies.FindAll();
            this.GridView1.DataSource = p;
            this.GridView1.DataBind();
            if (p.Count == 1)
            {
                this.lblResults.Text = "1 policy found";
            }
            else
            {
                this.lblResults.Text = p.Count + " policies found";
            }
        }

        protected void GridView1_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
        {
            string bvin = (string)GridView1.DataKeys[e.RowIndex].Value;
            if (MTApp.ContentServices.Policies.Delete(bvin) == false)
            {
                this.msg.ShowWarning("Unable to delete this policy. System policies can not be deleted.");
            }

            LoadPolicies();
        }

        protected void btnNew_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            msg.ClearMessage();

            if (this.NewNameField.Text.Trim().Length < 1)
            {
                msg.ShowWarning("Please enter a name for the new policy.");
            }
            else
            {
                Policy p = new Policy();
                p.Title = this.NewNameField.Text.Trim();
                p.SystemPolicy = false;
                if (MTApp.ContentServices.Policies.Create(p) == true)
                {
                    Response.Redirect("Policies_Edit.aspx?id=" + p.Bvin);
                }
                else
                {
                    msg.ShowError("Unable to create policy. Please see event log for details");
                    EventLog.LogEvent("Create New Policy Button", "Unable to create policy", EventLogSeverity.Error);
                }
            }
        }

        protected void GridView1_RowEditing(object sender, System.Web.UI.WebControls.GridViewEditEventArgs e)
        {
            string bvin = (string)GridView1.DataKeys[e.NewEditIndex].Value;
            Response.Redirect("Policies_edit.aspx?id=" + bvin);
        }

    }
}