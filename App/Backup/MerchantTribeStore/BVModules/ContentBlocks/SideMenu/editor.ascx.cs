using System.Web.UI;
using System.Web.UI.WebControls;
using MerchantTribe.Commerce.Content;

namespace MerchantTribeStore
{

    partial class BVModules_ContentBlocks_Side_Menu_editor : BVModule
    {

        private string SettingListName = "Links";

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);

            if (!Page.IsPostBack)
            {
                ContentBlock b = MyPage.MTApp.ContentServices.Columns.FindBlock(this.BlockId);
                if (b != null)
                {
                    LoadItems(b);
                    this.TitleField.Text = b.BaseSettings.GetSettingOrEmpty("Title");
                }
            }
        }

        private void LoadItems(ContentBlock b)
        {
            this.GridView1.DataSource = b.Lists.FindList(SettingListName);
            this.GridView1.DataKeyNames = new string[] { "Id" };
            this.GridView1.DataBind();
        }

        protected void btnOkay_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            ContentBlock b = MyPage.MTApp.ContentServices.Columns.FindBlock(this.BlockId);
            if (b != null)
            {
                b.BaseSettings.AddOrUpdate("Title", this.TitleField.Text.Trim());
                MyPage.MTApp.ContentServices.Columns.UpdateBlock(b);
            }
            this.NotifyFinishedEditing();
        }

        protected void btnNew_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            ContentBlock b = MyPage.MTApp.ContentServices.Columns.FindBlock(this.BlockId);

            ContentBlockSettingListItem c = new ContentBlockSettingListItem();

            if (this.EditBvinField.Value != string.Empty)
            {
                //Updating
                c = b.Lists.FindSingleItem(EditBvinField.Value);
                c.Setting1 = this.LinkTextField.Text.Trim();
                c.Setting2 = this.LinkField.Text.Trim();
                if (this.OpenInNewWindowField.Checked == true)
                {
                    c.Setting3 = "1";
                }
                else
                {
                    c.Setting3 = "0";
                }
                c.Setting4 = this.AltTextField.Text.Trim();
                c.Setting5 = this.CssClassField.Text.Trim();
                MyPage.MTApp.ContentServices.Columns.UpdateBlock(b);
                ClearEditor();
            }
            else
            {
                //Inserting
                c.Setting1 = this.LinkTextField.Text.Trim();
                c.Setting2 = this.LinkField.Text.Trim();
                if (this.OpenInNewWindowField.Checked == true)
                {
                    c.Setting3 = "1";
                }
                else
                {
                    c.Setting3 = "0";
                }
                c.Setting4 = this.AltTextField.Text.Trim();
                c.Setting5 = this.CssClassField.Text.Trim();
                c.ListName = SettingListName;
                b.Lists.AddItem(c);
                MyPage.MTApp.ContentServices.Columns.UpdateBlock(b);
            }
            LoadItems(b);
        }

        void ClearEditor()
        {
            this.EditBvinField.Value = string.Empty;
            this.LinkTextField.Text = string.Empty;
            this.LinkField.Text = string.Empty;
            this.OpenInNewWindowField.Checked = false;
            this.AltTextField.Text = string.Empty;
            this.CssClassField.Text = string.Empty;
            this.btnNew.ImageUrl = "~/BVAdmin/Images/Buttons/New.png";
        }

        protected void GridView1_RowCancelingEdit(object sender, System.Web.UI.WebControls.GridViewCancelEditEventArgs e)
        {
            ContentBlock b = MyPage.MTApp.ContentServices.Columns.FindBlock(this.BlockId);
            string bvin = string.Empty;
            bvin = ((GridView)sender).DataKeys[e.RowIndex].Value.ToString();
            b.Lists.MoveItemDown(bvin, SettingListName);
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

        protected void GridView1_RowEditing(object sender, System.Web.UI.WebControls.GridViewEditEventArgs e)
        {
            ContentBlock b = MyPage.MTApp.ContentServices.Columns.FindBlock(this.BlockId);
            string bvin = (string)GridView1.DataKeys[e.NewEditIndex].Value;
            ContentBlockSettingListItem c = b.Lists.FindSingleItem(bvin);
            if (c != null)
            {
                this.EditBvinField.Value = c.Id;
                this.LinkField.Text = c.Setting2;
                this.LinkTextField.Text = c.Setting1;
                if (c.Setting3 == "1")
                {
                    this.OpenInNewWindowField.Checked = true;
                }
                else
                {
                    this.OpenInNewWindowField.Checked = false;
                }
                this.AltTextField.Text = c.Setting4;
                this.CssClassField.Text = c.Setting5;
                this.btnNew.ImageUrl = "~/BVAdmin/Images/Buttons/SaveChanges.png";
            }
        }

        protected void GridView1_RowUpdating(object sender, System.Web.UI.WebControls.GridViewUpdateEventArgs e)
        {
            ContentBlock b = MyPage.MTApp.ContentServices.Columns.FindBlock(this.BlockId);
            string bvin = string.Empty;
            bvin = ((GridView)sender).DataKeys[e.RowIndex].Value.ToString();
            b.Lists.MoveItemUp(bvin, SettingListName);
            MyPage.MTApp.ContentServices.Columns.UpdateBlock(b);
            LoadItems(b);
        }

        protected void btnCancel_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            ClearEditor();
        }

    }
}