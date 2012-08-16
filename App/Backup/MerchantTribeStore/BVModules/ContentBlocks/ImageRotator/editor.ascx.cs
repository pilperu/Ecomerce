using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using MerchantTribe.Commerce;
using MerchantTribe.Commerce.Content;
using System.Text;

namespace MerchantTribeStore.BVModules.ContentBlocks.ImageRotator
{
    public partial class editor : BVModule
    {
        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);

            ContentBlock b = MyPage.MTApp.ContentServices.Columns.FindBlock(this.BlockId);

            this.RegisterWindowScripts();

            if (!Page.IsPostBack)
            {
                LoadItems(b);
                this.chkShowInOrder.Checked = b.BaseSettings.GetBoolSetting("ShowInOrder");
                this.cssclass.Text = b.BaseSettings.GetSettingOrEmpty("cssclass");


                this.WidthField.Text = b.BaseSettings.GetIntegerSetting("Width").ToString();
                if ((this.WidthField.Text.Trim() == String.Empty) || (this.WidthField.Text == "0"))
                {
                    this.WidthField.Text = "220";
                }

                this.HeighField.Text = b.BaseSettings.GetIntegerSetting("Height").ToString();
                if ((this.HeighField.Text.Trim() == String.Empty) || (this.HeighField.Text == "0"))
                {
                    this.HeighField.Text = "220";
                }

                int seconds = b.BaseSettings.GetIntegerSetting("Pause");
                if (seconds < 0) seconds = 3;
                this.PauseField.Text = seconds.ToString();


            }

        }

        private void LoadItems(ContentBlock b)
        {
            this.GridView1.DataSource = b.Lists.FindList("Images");
            this.GridView1.DataBind();
        }

        protected void btnCancel_Click(object sender, ImageClickEventArgs e)
        {
            ClearEditor();
        }

        private void ClearEditor()
        {
            this.EditBvinField.Value = String.Empty;
            this.ImageUrlField.Text = String.Empty;
            this.ImageLinkField.Text = String.Empty;
            this.chkOpenInNewWindow.Checked = false;
            this.AltTextField.Text = String.Empty;
            this.btnNew.ImageUrl = "~/BVAdmin/Images/Buttons/New.png";
        }

        private void RegisterWindowScripts()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("var w;");
            sb.Append("function popUpWindow(parameters) {");
            sb.Append("w = window.open('../ImageBrowser.aspx' + parameters, null, 'height=480, width=640');");
            sb.Append("}");

            sb.Append("function closePopup() {");
            sb.Append("w.close();");
            sb.Append("}");

            sb.Append("function SetImage(fileName) {");
            sb.Append("document.getElementById('");
            sb.Append(this.ImageUrlField.ClientID);
            sb.Append("').value = '~/' + fileName;");
            sb.Append("w.close();");
            sb.Append("}");

            this.Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "WindowScripts", sb.ToString(), true);

        }

        protected void btnOkay_Click(object sender, ImageClickEventArgs e)
        {
            ContentBlock b = MyPage.MTApp.ContentServices.Columns.FindBlock(this.BlockId);

            b.BaseSettings["cssclass"] = this.cssclass.Text.Trim();
            b.BaseSettings.SetBoolSetting("ShowInOrder", this.chkShowInOrder.Checked);

            int width = 0;
            int.TryParse(this.WidthField.Text.Trim(),out width);
            b.BaseSettings.SetIntegerSetting("Width", width);

            int height = 0;
            int.TryParse(this.HeighField.Text.Trim(), out height);
            b.BaseSettings.SetIntegerSetting("Height", height);

            int pause = 0;
            int.TryParse(this.PauseField.Text.Trim(), out pause);
            b.BaseSettings.SetIntegerSetting("Pause", pause);

            this.MyPage.MTApp.ContentServices.Columns.UpdateBlock(b);

            this.NotifyFinishedEditing();            
        }

        protected void btnNew_Click(object sender, ImageClickEventArgs e)
        {
            ContentBlock b = MyPage.MTApp.ContentServices.Columns.FindBlock(this.BlockId);

            ContentBlockSettingListItem c = new ContentBlockSettingListItem();

                    if (this.EditBvinField.Value != string.Empty)
                    {
                        //Updating
                        c = b.Lists.FindSingleItem(this.EditBvinField.Value);                        
                        c.Setting1 = this.ImageUrlField.Text.Trim();
                        c.Setting2 = this.ImageLinkField.Text.Trim();
                        if (this.chkOpenInNewWindow.Checked == true)
                        {
                            c.Setting3 = "1";
                        } else {
                            c.Setting3 = "0";
                        }
                        c.Setting4 = this.AltTextField.Text.Trim();                        
                        this.MyPage.MTApp.ContentServices.Columns.UpdateBlock(b);                        
                        ClearEditor();
                    } else {
                        //inserting
                        c.Setting1 = this.ImageUrlField.Text.Trim();
                        c.Setting2 = this.ImageLinkField.Text.Trim();
                        if (this.chkOpenInNewWindow.Checked == true)
                        {
                            c.Setting3 = "1";
                        } else {
                            c.Setting3 = "0";
                        }
                        c.Setting4 = this.AltTextField.Text.Trim();
                        c.ListName = "Images";
                        b.Lists.AddItem(c);
                        this.MyPage.MTApp.ContentServices.Columns.UpdateBlock(b);
                    }
                    LoadItems(b);
        }

        protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
        {
            ContentBlock b = MyPage.MTApp.ContentServices.Columns.FindBlock(this.BlockId);

            string bvin = string.Empty;
            bvin = ((GridView)sender).DataKeys[e.NewEditIndex].Value.ToString();

            ContentBlockSettingListItem c = new ContentBlockSettingListItem();
            c = b.Lists.FindSingleItem(bvin);

            if (c.Id != string.Empty)
            {
                            this.EditBvinField.Value = c.Id;
                            this.ImageLinkField.Text = c.Setting2;
                            this.ImageUrlField.Text = c.Setting1;
                            if (c.Setting3 == "1")
                            {
                                this.chkOpenInNewWindow.Checked = true;
                            } else {                            
                                this.chkOpenInNewWindow.Checked = false;
                            }
                            this.AltTextField.Text = c.Setting4;
                            this.btnNew.ImageUrl = "~/BVAdmin/Images/Buttons/SaveChanges.png";
            }

        }

        protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            ContentBlock b = MyPage.MTApp.ContentServices.Columns.FindBlock(this.BlockId);

            string bvin = string.Empty;
            bvin = ((GridView)sender).DataKeys[e.RowIndex].Value.ToString();
            b.Lists.RemoveItem(bvin);
            this.MyPage.MTApp.ContentServices.Columns.UpdateBlock(b);
            LoadItems(b);            
        }

        protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            ContentBlock b = MyPage.MTApp.ContentServices.Columns.FindBlock(this.BlockId);

            string bvin = string.Empty;
            bvin = ((GridView)sender).DataKeys[e.RowIndex].Value.ToString();
            b.Lists.MoveItemDown(bvin, "Images");
            this.MyPage.MTApp.ContentServices.Columns.UpdateBlock(b);
            LoadItems(b);            
        }

        protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            ContentBlock b = MyPage.MTApp.ContentServices.Columns.FindBlock(this.BlockId);

            string bvin = string.Empty;
            bvin = ((GridView)sender).DataKeys[e.RowIndex].Value.ToString();
            b.Lists.MoveItemUp(bvin, "Images");
            this.MyPage.MTApp.ContentServices.Columns.UpdateBlock(b);
            LoadItems(b);    
        }
    }


}
