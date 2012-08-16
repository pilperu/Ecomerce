using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using MerchantTribe.Commerce.Content;
using MerchantTribe.Commerce.Membership;
using MerchantTribe.Commerce.Storage;

namespace MerchantTribeStore
{

    partial class BVAdmin_Configuration_ThemesEdit : BaseAdminPage
    {

        protected override void OnInit(System.EventArgs e)
        {
            base.OnInit(e);

            this.PageTitle = "Edit Theme";
            this.CurrentTab = AdminTabType.Configuration;
            ValidateCurrentUserHasPermission(SystemPermissions.SettingsView);
        }

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);

            if (!Page.IsPostBack)
            {
                string themeId = Request.QueryString["id"];
                LoadTheInfo(themeId);
            }

        }

        private void LoadTheInfo(string themeId)
        {
            if ((themeId == null))
            {
                return;
            }

            ThemeInfo info = MTApp.ThemeManager().GetThemeInfo(themeId);
            if ((info != null))
            {
                this.ThemeNameField.Text = info.Title;
                this.DescriptionField.Text = info.Description;
                this.AuthorField.Text = info.Author;
                this.AuthorUrlField.Text = info.AuthorUrl;
                this.VersionField.Text = info.Version;
                this.VersionUrlField.Text = info.VersionUrl;

                ThemeView tv = new ThemeView();
                tv.LoadInstalledTheme(MTApp, themeId);
                this.imgPreview.ImageUrl = tv.PreviewImageUrl + "?uid=" + System.Guid.NewGuid().ToString();
            }

        }

        private void SaveInfo()
        {
            this.MessageBox1.ClearMessage();

            ThemeInfo info = MTApp.ThemeManager().GetThemeInfo(Request.QueryString["id"]);
            if ((info != null))
            {
                info.Title = this.ThemeNameField.Text.Trim();
                info.Description = this.DescriptionField.Text.Trim();
                info.Author = this.AuthorField.Text.Trim();
                info.AuthorUrl = this.AuthorUrlField.Text.Trim();
                info.Version = this.VersionField.Text.Trim();
                info.VersionUrl = this.VersionUrlField.Text.Trim();
                MTApp.ThemeManager().SaveThemeInfo(info.UniqueIdAsString, info);
                this.MessageBox1.ShowOk("Changes Saved!");
            }
            else
            {
                this.MessageBox1.ShowWarning("Unable to save theme info. Please contact an administrator for assistance.");
            }

        }

        protected void ImageButton1_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            SaveInfo();
        }

        protected void btnUpload_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            if (this.fileupload1.HasFile)
            {
                string themeId = Request.QueryString["id"];
                MerchantTribe.Commerce.Storage.DiskStorage.UploadThemePreview(MTApp.CurrentStore.Id, themeId, this.fileupload1.PostedFile, false);
                LoadTheInfo(themeId);
            }
        }
    }
}