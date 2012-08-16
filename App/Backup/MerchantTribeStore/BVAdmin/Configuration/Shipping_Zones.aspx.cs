using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MerchantTribe.Commerce;
using MerchantTribe.Commerce.Shipping;
using MerchantTribe.Web.Geography;
using MerchantTribe.Commerce.Membership;
using System.Text;

namespace MerchantTribeStore
{

    public partial class BVAdmin_Configuration_Shipping_Zones : BaseAdminPage
    {

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.PageTitle = "Shipping Zones";
            this.CurrentTab = AdminTabType.Configuration;
            ValidateCurrentUserHasPermission(SystemPermissions.SettingsView);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (!Page.IsPostBack)
            {
                LoadZones();
            }
        }

        private void LoadZones()
        {
            List<Zone> zones = MTApp.OrderServices.ShippingZones.FindForStore(MTApp.CurrentStore.Id);

            StringBuilder sb = new StringBuilder();

            foreach (Zone z in zones)
            {
                sb.Append("<tr><td>" + z.Name + "</td>");
                if (z.IsBuiltInZone)
                {
                    sb.Append("<td colspan=\"2\">&nbsp;</td>");
                }
                else
                {
                    sb.Append("<td><a onclick=\"return window.confirm('Delete This Zone Forever?');\" href=\"Shipping_Zones_Delete.aspx?id=" + z.Id.ToString() + "\"><img src=\"../images/buttons/delete.png\" alt=\"Delete\" /></a></td>");
                    sb.Append("<td><a href=\"Shipping_Zones_Edit.aspx?id=" + z.Id.ToString() + "\"><img src=\"../images/buttons/edit.png\" alt=\"Edit\" /></a></td></tr>");
                }
            }

            this.litMain.Text = sb.ToString();
        }

        protected void btnNew_Click(object sender, ImageClickEventArgs e)
        {
            this.MessageBox1.ClearMessage();

            Zone z = new Zone();
            z.Name = this.NewZoneField.Text.Trim();
            z.StoreId = MTApp.CurrentStore.Id;
            if (z.IsValid())
            {
                if (MTApp.OrderServices.ShippingZones.Create(z))
                {
                    this.MessageBox1.ShowOk("Zone Created");
                }
                else
                {
                    this.MessageBox1.ShowWarning("Unable to create zone");
                }
            }
            else
            {
                this.MessageBox1.RenderViolations(z.GetRuleViolations());
            }

            LoadZones();
        }
    }
}