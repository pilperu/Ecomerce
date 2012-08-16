using System.Web.UI;
using System.Web.UI.WebControls;
using MerchantTribe.Commerce.Catalog;
using MerchantTribe.Commerce.Content;
using MerchantTribe.Commerce.Utilities;

namespace MerchantTribeStore
{

    partial class BVModules_ContentBlocks_Product_Grid_editor : BVModule
    {

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);
            if (!Page.IsPostBack)
            {
                ContentBlock b = MyPage.MTApp.ContentServices.Columns.FindBlock(this.BlockId);
                if (b != null)
                {
                    LoadItems(b);
                    int gridColumns = b.BaseSettings.GetIntegerSetting("GridColumns");
                    if (gridColumns < 1) gridColumns = 3;

                    this.GridColumnsField.Text = gridColumns.ToString();
                }
            }
        }

        private void LoadItems(ContentBlock b)
        {
            this.GridView1.DataSource = b.Lists.FindList("ProductGrid");
            this.GridView1.DataBind();
        }

        protected void btnOkay_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            ContentBlock b = MyPage.MTApp.ContentServices.Columns.FindBlock(this.BlockId);
            if (b != null)
            {
                b.BaseSettings.SetIntegerSetting("GridColumns", int.Parse(this.GridColumnsField.Text.Trim()));
                MyPage.MTApp.ContentServices.Columns.UpdateBlock(b);
            }
            this.NotifyFinishedEditing();
        }

        protected void btnNew_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            ContentBlock b = MyPage.MTApp.ContentServices.Columns.FindBlock(this.BlockId);
            if (b != null)
            {

                //Inserting
                //SettingsManager.GetSettingList("ProductGrid");
                foreach (string product in ProductPicker1.SelectedProducts)
                {
                    ContentBlockSettingListItem c = new ContentBlockSettingListItem();
                    Product p = MyPage.MTApp.CatalogServices.Products.Find(product);
                    c.Setting1 = product;
                    c.Setting2 = p.Sku;
                    c.Setting3 = p.ProductName;
                    c.Setting4 = Page.ResolveUrl(ImageHelper.GetValidImage(p.ImageFileSmall, true));
                    c.Setting5 = UrlRewriter.BuildUrlForProduct(p, this.Page);
                    c.Setting6 = p.SitePrice.ToString();
                    c.ListName = "ProductGrid";
                    b.Lists.AddItem(c);
                    MyPage.MTApp.ContentServices.Columns.UpdateBlock(b);
                }
                LoadItems(b);
            }
        }

        protected void GridView1_RowCancelingEdit(object sender, System.Web.UI.WebControls.GridViewCancelEditEventArgs e)
        {
            ContentBlock b = MyPage.MTApp.ContentServices.Columns.FindBlock(this.BlockId);
            string bvin = string.Empty;
            bvin = ((GridView)sender).DataKeys[e.RowIndex].Value.ToString();
            b.Lists.MoveItemDown(bvin, "ProductGrid");
            MyPage.MTApp.ContentServices.Columns.UpdateBlock(b);
            LoadItems(b);
        }

        protected void GridView1_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
        {
            ContentBlock b = MyPage.MTApp.ContentServices.Columns.FindBlock(this.BlockId);
            string bvin = (string)this.GridView1.DataKeys[e.RowIndex].Value;
            b.Lists.RemoveItem(bvin);
            MyPage.MTApp.ContentServices.Columns.UpdateBlock(b);
            LoadItems(b);
        }

        protected void GridView1_RowUpdating(object sender, System.Web.UI.WebControls.GridViewUpdateEventArgs e)
        {
            ContentBlock b = MyPage.MTApp.ContentServices.Columns.FindBlock(this.BlockId);
            string bvin = string.Empty;
            bvin = ((GridView)sender).DataKeys[e.RowIndex].Value.ToString();
            b.Lists.MoveItemUp(bvin, "ProductGrid");
            MyPage.MTApp.ContentServices.Columns.UpdateBlock(b);
            LoadItems(b);
        }

        protected void btnCancel_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            Response.Redirect("~/BVAdmin/Content/Columns.aspx");
        }

    }
}