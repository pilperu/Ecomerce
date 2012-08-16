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
using MerchantTribe.Web.Geography;

namespace MerchantTribeStore
{

    partial class Taxes_Edit : BaseAdminPage
    {

        private TaxSchedule ts;

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.PageTitle = "Tax Schedule Edit";
            this.CurrentTab = AdminTabType.Configuration;
            ValidateCurrentUserHasPermission(SystemPermissions.SettingsView);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            msg.ClearMessage();

            ts = new TaxSchedule();

            if (!Page.IsPostBack)
            {

                string id = Request.QueryString["id"];

                PopulateCountries();
                string homeCountry = WebAppSettings.ApplicationCountryBvin;
                lstCountry.SelectedValue = homeCountry;
                PopulateRegions(lstCountry.SelectedItem.Value);

                LoadSchedule(long.Parse(id));

            }
        }

        private void LoadSchedule(long id)
        {
            ts = MTApp.OrderServices.TaxSchedules.FindForThisStore(id);            
            this.ScheduleNameField.Text = ts.Name;
            LoadRates(ts);            
        }

        void SetCountry(string Code)
        {
            foreach (System.Web.UI.WebControls.ListItem li in lstCountry.Items)
            {
                if (li.Value == Code)
                {
                    lstCountry.ClearSelection();

                    li.Selected = true;
                }
            }
            PopulateRegions(lstCountry.SelectedValue);
        }

        void PopulateCountries()
        {
            try
            {
                lstCountry.DataSource = MTApp.CurrentStore.Settings.FindActiveCountries();
                lstCountry.DataValueField = "Bvin";
                lstCountry.DataTextField = "DisplayName";
                lstCountry.DataBind();
            }
            catch (Exception Ex)
            {
                throw new ArgumentException("Error Loading Countries: " + Ex.Message);
            }
        }

        void SetRegion(string ID)
        {
            foreach (System.Web.UI.WebControls.ListItem li in lstState.Items)
            {
                if (li.Value == ID)
                {
                    lstState.ClearSelection();
                    li.Selected = true;
                }
            }
        }

        void PopulateRegions(string sID)
        {
            lstState.Items.Clear();
            try
            {
                lstState.DataSource = Country.FindByBvin(sID).Regions;
                lstState.DataTextField = "name";
                lstState.DataValueField = "abbreviation";
                lstState.DataBind();

                System.Web.UI.WebControls.ListItem li = new System.Web.UI.WebControls.ListItem();
                li.Value = "";
                li.Text = "All States/Regions";
                lstState.Items.Insert(0, li);
                li = null;
            }

            catch (Exception Ex)
            {
                throw new ArgumentException(Ex.Message);
            }
        }

        protected void lstCountry_SelectedIndexChanged(object Sender, EventArgs E)
        {
            PopulateRegions(lstCountry.SelectedItem.Value);
        }

        protected void btnSaveChanges_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            if (Save())
            {
                Response.Redirect("TaxClasses.aspx");
            }
        }

        private bool Save()
        {
            string ids = Request.QueryString["id"];
            long id = long.Parse(ids);
            ts = MTApp.OrderServices.TaxSchedules.FindForThisStore(id);
            ts.Name = this.ScheduleNameField.Text;
            return MTApp.OrderServices.TaxSchedules.Update(ts);
        }

        protected void btnNew_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            string sid = Request.QueryString["id"];
            long scheduleId = long.Parse(sid);

            Tax t = new Tax();
            t.CountryName = this.lstCountry.SelectedItem.Text;
            t.ApplyToShipping = this.ApplyToShippingCheckBox.Checked;
            t.PostalCode = this.postalCode.Text.Trim();
            t.Rate = decimal.Parse(this.Rate.Text.Trim());
            t.RegionAbbreviation = this.lstState.SelectedItem.Value;
            t.StoreId = MTApp.CurrentStore.Id;
            t.TaxScheduleId = scheduleId;

            MTApp.OrderServices.Taxes.Create(t);
            LoadSchedule(scheduleId);
        }

        private void LoadRates(TaxSchedule ts)
        {
            this.litRates.Text = string.Empty;
            foreach (Tax t in MTApp.OrderServices.Taxes.GetRates(MTApp.CurrentStore.Id, ts.Id))
            {
                RenderTax(t);
            }
        }

        private void RenderTax(Tax t)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("<tr>");
            sb.Append("<td align=\"middle\">" + t.CountryName + "</td>");
            sb.Append("<td>" + t.RegionAbbreviation + "</td>");
            sb.Append("<td>" + t.PostalCode + "</td>");
            sb.Append("<td>" + t.Rate.ToString("#.00") + "</td>");
            if (t.ApplyToShipping)
            {
                sb.Append("<td>Yes</td>");
            }
            else
            {
                sb.Append("<td>No</td>");
            }
            sb.Append("<td><a href=\"Taxes_Delete.aspx?id=" + t.Id.ToString() + "&sid=" + t.TaxScheduleId.ToString() + "\">Delete</a></td>");
            sb.Append("</tr>");

            this.litRates.Text += sb.ToString();
        }
    }
}