using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MerchantTribe.Commerce.Content;
using MerchantTribe.Commerce.Membership;
using MerchantTribe.Commerce.Storage;

namespace MerchantTribeStore
{

    public partial class BVAdmin_Configuration_ThemesEditHeaderFooter : BaseAdminPage
    {
        protected override void OnInit(System.EventArgs e)
        {
            base.OnInit(e);
            this.PageTitle = "Edit Theme | Header and Footer";
            this.CurrentTab = AdminTabType.Configuration;
            ValidateCurrentUserHasPermission(SystemPermissions.SettingsView);
        }

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);

            string themeId = Request.QueryString["id"];
            LoadTheInfo(themeId);
        }

        private void LoadTheInfo(string themeId)
        {
            this.litheader.Text = "<textarea id=\"headerhtml\" name=\"headerhtml\" style=\"width:700px;height:180px;\" wrap=\"false\">";
            this.litheader.Text += MerchantTribe.Commerce.Storage.DiskStorage.ReadCustomHeader(MTApp.CurrentStore.Id, themeId);
            this.litheader.Text += "</textarea>";

            this.litFooter.Text = "<textarea id=\"footerhtml\" name=\"footerhtml\" style=\"width:700px;height:180px;\" wrap=\"false\">";
            this.litFooter.Text += MerchantTribe.Commerce.Storage.DiskStorage.ReadCustomFooter(MTApp.CurrentStore.Id, themeId);
            this.litFooter.Text += "</textarea>";

            this.litIdField.Text = "<input type=\"hidden\" id=\"themeidfield\" name=\"themeidfield\" value=\"" + themeId + "\" />";
        }

        protected void btnSave_Click(object sender, ImageClickEventArgs e)
        {
            string themeId = Request.QueryString["id"];

            string headerhtml = Request.Form["headerhtml"];
            string footerhtml = Request.Form["footerhtml"];

            bool result = MerchantTribe.Commerce.Storage.DiskStorage.WriteCustomFooter(MTApp.CurrentStore.Id, themeId, footerhtml);
            if (result)
            {
                result = MerchantTribe.Commerce.Storage.DiskStorage.WriteCustomHeader(MTApp.CurrentStore.Id, themeId, headerhtml);
            }

            if (result)
            {
                this.MessageBox1.ShowOk("Changes Saved as " + System.DateTime.Now.ToLongTimeString());
            }
            else
            {
                this.MessageBox1.ShowWarning("Unable to save changes");
            }

            LoadTheInfo(themeId);
        }
    }
}