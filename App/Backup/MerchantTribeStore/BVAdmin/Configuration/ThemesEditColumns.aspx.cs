using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MerchantTribe.Commerce;
using MerchantTribe.Commerce.Membership;
using MerchantTribe.Commerce.Content;
using MerchantTribe.Commerce.Storage;

namespace MerchantTribeStore
{

    public partial class BVAdmin_Configuration_ThemesEditColumns : BaseAdminPage
    {
        protected override void OnInit(System.EventArgs e)
        {
            base.OnInit(e);
            this.PageTitle = "Edit Theme | Content Columns";
            this.CurrentTab = AdminTabType.Configuration;
            ValidateCurrentUserHasPermission(SystemPermissions.SettingsView);
        }

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);

            if (!Page.IsPostBack)
            {
                string themeId = Request.QueryString["id"];
            }

        }

        protected void btnCopyFromStore_Click(object sender, EventArgs e)
        {
            this.MessageBox1.ClearMessage();
            string themeId = Request.QueryString["id"];
            if (MTApp.ThemeManager().CopyCurrentContentColumnsToTheme(themeId))
            {
                this.MessageBox1.ShowOk("Columns Copied to this Theme");
            }
            else
            {
                this.MessageBox1.ShowWarning("Unable to copy columns to this theme at this time.");
            }
        }
        protected void btnClear_Click(object sender, EventArgs e)
        {
            this.MessageBox1.ClearMessage();
            string themeId = Request.QueryString["id"];
            if (MTApp.ThemeManager().ClearContentColumnDataFromTheme(themeId))
            {
                this.MessageBox1.ShowOk("Columns Removed from this Theme");
            }
            else
            {
                this.MessageBox1.ShowWarning("Unable to remove columns at this time.");
            }
        }

        protected void btnCopyToStore_Click(object sender, EventArgs e)
        {
            this.MessageBox1.ClearMessage();
            string themeId = Request.QueryString["id"];
            if (MTApp.ThemeManager().CopyColumnsFromThemeToStore(themeId))
            {
                this.MessageBox1.ShowOk("Columns Copied to Store");
            }
            else
            {
                this.MessageBox1.ShowWarning("Unable to copy columns at this time.");
            }
        }
    }
}