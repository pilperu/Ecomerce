using System.Collections.ObjectModel;
using MerchantTribe.Commerce.Content;
using System.Collections.Generic;

namespace MerchantTribeStore
{

    partial class BVModules_ContentBlocks_Product_Grid_adminview : BVModule
    {

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);
            LoadProductGrid();
        }

        private void LoadProductGrid()
        {
            ContentBlock b = MyPage.MTApp.ContentServices.Columns.FindBlock(this.BlockId);
            if (b != null)
            {
                List<ContentBlockSettingListItem> myProducts = b.Lists.FindList("ProductGrid");
                if (myProducts != null)
                {
                    int gridColumns = b.BaseSettings.GetIntegerSetting("GridColumns");
                    if (gridColumns < 1) gridColumns = 3;

                    this.DataList1.DataSource = myProducts;
                    this.DataList1.DataBind();
                    this.DataList1.RepeatColumns = gridColumns;
                }
            }
        }

    }
}