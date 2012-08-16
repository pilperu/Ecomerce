using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
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
using System.Text;

namespace MerchantTribeStore
{

    public partial class BVAdmin_Configuration_Shipping_Zones_Edit : BaseAdminPage
    {

        private Zone zone;

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.PageTitle = "Edit Shipping Zone";
            this.CurrentTab = AdminTabType.Configuration;
            ValidateCurrentUserHasPermission(SystemPermissions.SettingsView);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            msg.ClearMessage();

            zone = new Zone();

            if (!Page.IsPostBack)
            {

                string id = Request.QueryString["id"];

                PopulateCountries();
                string homeCountry = WebAppSettings.ApplicationCountryBvin;
                lstCountry.SelectedValue = homeCountry;
                PopulateRegions(lstCountry.SelectedItem.Value);

                LoadZone(long.Parse(id));

            }
        }

        private void LoadZone(long id)
        {
            zone = MTApp.OrderServices.ShippingZones.Find(id);
            if (zone.StoreId == MTApp.CurrentStore.Id)
            {
                this.ZoneNameField.Text = zone.Name;
                LoadAreas(zone);
            }
            else
            {
                Response.Redirect("Shipping_Zones.aspx");
            }
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
                lstCountry.DataValueField = "IsoAlpha3";
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
                lstState.DataSource = Country.FindByISOCode(sID).Regions;
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
                Response.Redirect("Shipping_Zones.aspx");
            }
        }

        private bool Save()
        {
            string ids = Request.QueryString["id"];
            long id = long.Parse(ids);
            zone = MTApp.OrderServices.ShippingZones.Find(id);
            zone.Name = this.ZoneNameField.Text;
            return MTApp.OrderServices.ShippingZones.Update(zone);
        }

        protected void btnNew_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            string zid = Request.QueryString["id"];
            long zoneId = long.Parse(zid);
            zone = MTApp.OrderServices.ShippingZones.Find(zoneId);

            ZoneArea a = new ZoneArea();
            a.CountryIsoAlpha3 = this.lstCountry.SelectedItem.Value;
            a.RegionAbbreviation = this.lstState.SelectedItem.Value;

            zone.Areas.Add(a);
            MTApp.OrderServices.ShippingZones.Update(zone);
            LoadZone(zone.Id);
        }

        private void LoadAreas(Zone z)
        {
            this.litAreas.Text = string.Empty;
            foreach (ZoneArea a in z.Areas)
            {
                RenderArea(a, z.Id);
            }
        }

        private void RenderArea(ZoneArea a, long zoneId)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("<tr>");
            sb.Append("<td>" + a.CountryName + "</td>");
            sb.Append("<td>" + a.RegionAbbreviation + "</td>");
            sb.Append("<td><a href=\"Shipping_Zones_DeleteArea.aspx?id=" + zoneId.ToString() + "&country=" + a.CountryIsoAlpha3 + "&region=" + a.RegionAbbreviation + "\"><img src=\"../images/buttons/delete.png\" alt=\"Delete\" /></a></td>");
            sb.Append("</tr>");

            this.litAreas.Text += sb.ToString();
        }
    }
}