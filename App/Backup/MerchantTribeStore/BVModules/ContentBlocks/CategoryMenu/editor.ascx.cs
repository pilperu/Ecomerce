using System.Web.UI;
using MerchantTribe.Commerce.Content;

namespace MerchantTribeStore
{

    partial class BVModules_ContentBlocks_Category_Menu_editor : BVModule
    {

        protected void btnCancel_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            this.NotifyFinishedEditing();
        }

        protected void btnSave_Click(object sender, System.Web.UI.ImageClickEventArgs e)
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
                this.TitleField.Text = b.BaseSettings.GetSettingOrEmpty("Title");

                string mode = "0";
                mode = b.BaseSettings.GetSettingOrEmpty("CategoryMenuMode");
                if (ModeField.Items.FindByValue(mode) != null)
                {
                    ModeField.Items.FindByValue(mode).Selected = true;
                }
                this.ProductCountCheckBox.Checked = b.BaseSettings.GetBoolSetting("ShowProductCount");
                this.SubCategoryCountCheckBox.Checked = b.BaseSettings.GetBoolSetting("ShowCategoryCount");
                this.HomeLinkField.Checked = b.BaseSettings.GetBoolSetting("HomeLink");

                this.MaximumDepth.Text = b.BaseSettings.GetIntegerSetting("MaximumDepth").ToString();
            }

        }

        private void SaveData()
        {

            ContentBlock b = MyPage.MTApp.ContentServices.Columns.FindBlock(this.BlockId);
            if (b != null)
            {

                b.BaseSettings.AddOrUpdate("Title", this.TitleField.Text.Trim());

                string mode = "0";
                if (ModeField.SelectedValue != null)
                {
                    mode = ModeField.SelectedValue;
                }
                b.BaseSettings.AddOrUpdate("CategoryMenuMode", mode);
                b.BaseSettings.SetBoolSetting("ShowProductCount", this.ProductCountCheckBox.Checked);
                b.BaseSettings.SetBoolSetting("ShowCategoryCount", this.SubCategoryCountCheckBox.Checked);
                b.BaseSettings.SetBoolSetting("HomeLink", this.HomeLinkField.Checked);

                int maxDepth = 0;
                int.TryParse(this.MaximumDepth.Text.Trim(), out maxDepth);
                b.BaseSettings.SetIntegerSetting("MaximumDepth", maxDepth);

                MyPage.MTApp.ContentServices.Columns.UpdateBlock(b);
            }
        }

    }
}