using MerchantTribe.Commerce.Content;

namespace MerchantTribeStore
{

    partial class BVModules_ContentBlocks_Html_adminview : BVModule
    {
        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);

            ContentBlock b = MyPage.MTApp.ContentServices.Columns.FindBlock(this.BlockId);
            if (b != null)
            {
                this.lblHtml.Text = Server.HtmlEncode(b.BaseSettings.GetSettingOrEmpty("HtmlData"));
            }

            if (this.lblHtml.Text.Trim() == string.Empty)
            {
                this.lblHtml.Text = "No html set for this block.";
            }
        }

    }
}