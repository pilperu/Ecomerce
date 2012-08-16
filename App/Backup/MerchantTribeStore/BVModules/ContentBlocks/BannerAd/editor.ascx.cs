using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MerchantTribe.Commerce.Content;

namespace MerchantTribeStore
{

    public partial class BVModules_ContentBlocks_Banner_Ad_editor : BVModule
    {

        protected void btnCancel_Click(object sender, ImageClickEventArgs e)
        {
            this.NotifyFinishedEditing();
        }
        protected void btnSave_Click(object sender, ImageClickEventArgs e)
        {
            SaveData();
            this.NotifyFinishedEditing();
        }

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);
            if (!Page.IsPostBack)
            {
                LoadData();
            }
        }

        private void LoadData()
        {
            ContentBlock b = MyPage.MTApp.ContentServices.Columns.FindBlock(this.BlockId);
            if (b != null)
            {
                this.ImageUrlField.Text = b.BaseSettings.GetSettingOrEmpty("imageurl");
                this.AlternateTextField.Text = b.BaseSettings.GetSettingOrEmpty("alttext");
                this.CssClassField.Text = b.BaseSettings.GetSettingOrEmpty("cssclass");
                this.CssIdField.Text = b.BaseSettings.GetSettingOrEmpty("cssid");
                this.LinkUrlField.Text = b.BaseSettings.GetSettingOrEmpty("linkurl");
            }
        }

        private void SaveData()
        {
            ContentBlock b = MyPage.MTApp.ContentServices.Columns.FindBlock(this.BlockId);
            if (b != null)
            {
                b.BaseSettings.AddOrUpdate("imageurl", this.ImageUrlField.Text.Trim());
                b.BaseSettings.AddOrUpdate("alttext", this.AlternateTextField.Text.Trim());
                b.BaseSettings.AddOrUpdate("cssclass", this.CssClassField.Text.Trim());
                b.BaseSettings.AddOrUpdate("cssid", this.CssIdField.Text.Trim());
                b.BaseSettings.AddOrUpdate("linkurl", this.LinkUrlField.Text.Trim());
                MyPage.MTApp.ContentServices.Columns.UpdateBlock(b);
            }
        }
    }
}