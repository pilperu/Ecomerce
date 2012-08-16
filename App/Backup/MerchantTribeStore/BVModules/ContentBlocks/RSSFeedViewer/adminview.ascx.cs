using MerchantTribe.Commerce.Content;

namespace MerchantTribeStore
{

    partial class BVModules_ContentBlocks_RSS_Feed_Viewer_adminview : BVModule
    {

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);

            LoadData();
        }

        private void LoadData()
        {
            ContentBlock b = MyPage.MTApp.ContentServices.Columns.FindBlock(this.BlockId);
            if (b != null)
            {
                string feed = b.BaseSettings.GetSettingOrEmpty("FeedUrl");
                if (feed.Length > 80)
                {
                    feed = feed.Substring(0, 80) + "...";
                }
                if (feed.Trim().Length < 1)
                {
                    feed = "No Feed Selected";
                }
                this.lblFeed.Text = feed;
            }

        }

    }
}