using System.Web.UI;
using MerchantTribe.Commerce.Content;

namespace MerchantTribeStore
{

    partial class BVModules_ContentBlocks_Html_editor : BVModule
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

            string result = string.Empty;

            if (b != null)
            {
                this.HtmlEditor1.Text = b.BaseSettings.GetSettingOrEmpty("HtmlData");
                string pretransformText = b.BaseSettings.GetSettingOrEmpty("PreTransformHtmlData");
                if (this.HtmlEditor1.SupportsTransform == true)
                {
                    if (pretransformText.Length > 0)
                    {
                        this.HtmlEditor1.Text = pretransformText;
                    }
                }
            }

        }

        private void SaveData()
        {
            ContentBlock b = MyPage.MTApp.ContentServices.Columns.FindBlock(this.BlockId);
            if (b != null)
            {
                b.BaseSettings.AddOrUpdate("HtmlData", this.HtmlEditor1.Text.Trim());
                b.BaseSettings.AddOrUpdate("PreTransformHtmlData", this.HtmlEditor1.PreTransformText.Trim());
                MyPage.MTApp.ContentServices.Columns.UpdateBlock(b);
            }
        }

    }
}