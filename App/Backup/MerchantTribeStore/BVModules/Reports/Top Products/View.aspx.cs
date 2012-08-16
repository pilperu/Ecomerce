using System;
using MerchantTribe.Commerce.Catalog;
using MerchantTribe.Commerce.Membership;
using MerchantTribe.Commerce.Utilities;
using System.Collections.Generic;

namespace MerchantTribeStore
{

    partial class BVAdmin_Reports_Products : BaseAdminPage
    {

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.PageTitle = "Sales By Product";
            this.CurrentTab = AdminTabType.Reports;
            ValidateCurrentUserHasPermission(SystemPermissions.ReportsView);

        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.DateRangeField.RangeTypeChanged += this.DateRangeField_RangeTypeChanged;
        }

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);
            if (!Page.IsPostBack)
            {
                LoadProducts();
            }
        }

        private void LoadProducts()
        {
            TimeZoneInfo tz = MTApp.CurrentStore.Settings.TimeZone;
            DateTime localStart = this.DateRangeField.StartDateForZone(tz);
            DateTime localEnd = this.DateRangeField.EndDateForZone(tz);

            DateTime utcStart = TimeZoneInfo.ConvertTimeToUtc(localStart, tz);
            DateTime utcEnd = TimeZoneInfo.ConvertTimeToUtc(localEnd, tz);

            List<Product> t = MTApp.ReportingTopSellersByDate(utcStart, utcEnd, 10);

            if (t.Count == 0)
            {
                this.lblResults.Text = "No Products Found";
            }
            else if (t.Count == 1)
            {
                this.lblResults.Text = "1 product found";
            }
            else if (t.Count > 1)
            {
                this.lblResults.Text = t.Count + " products found";
            }

            this.GridView1.DataSource = t;
            this.GridView1.DataBind();

        }

        protected void GridView1_PageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            LoadProducts();
        }

        protected void GridView1_RowEditing(object sender, System.Web.UI.WebControls.GridViewEditEventArgs e)
        {
            string bvin = (string)GridView1.DataKeys[e.NewEditIndex].Value;
            Response.Redirect("~/BVAdmin/Catalog/Products_Edit.aspx?id=" + bvin);
        }

        protected void DateRangeField_RangeTypeChanged(System.EventArgs e)
        {
            if (DateRangeField.RangeType != DateRangeType.Custom)
            {
                LoadProducts();
            }

            if (DateRangeField.RangeType == DateRangeType.Custom)
            {
                btnShow.Visible = true;
            }
        }

        protected void btnShow_Click(System.Object sender, System.Web.UI.ImageClickEventArgs e)
        {
            GridView1.PageIndex = 0;
            LoadProducts();
        }


    }
}