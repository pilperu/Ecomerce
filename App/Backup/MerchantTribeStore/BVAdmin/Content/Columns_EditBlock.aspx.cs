using System;
using System.Web;
using System.Web.UI.WebControls;
using MerchantTribe.Commerce.Membership;
using MerchantTribe.Commerce.Content;
using MerchantTribe.Commerce;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace MerchantTribeStore
{  
    partial class BVAdmin_Content_Columns_EditBlock : BaseAdminPage
    {

        private ContentBlock b;
        private BVModule editor;

        protected override void OnInit(System.EventArgs e)
        {
            base.OnInit(e);

            if (Request.QueryString["id"] != null)
            {
                this.BlockIDField.Value = Request.QueryString["id"];
            }
            b = MTApp.ContentServices.Columns.FindBlock(this.BlockIDField.Value);
            LoadEditor();
        }

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);

            if (!Page.IsPostBack)
            {
                PopulateAdvancedOptions();
            }
        }

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.PageTitle = "Edit Content Block";
            this.CurrentTab = AdminTabType.Content;
            ValidateCurrentUserHasPermission(SystemPermissions.ContentView);
        }

        private void PopulateAdvancedOptions()
        {
            List<ContentColumn> columns = MTApp.ContentServices.Columns.FindAll();

            this.CopyToList.DataSource = columns;
            this.CopyToList.DataTextField = "DisplayName";
            this.CopyToList.DataValueField = "bvin";
            this.CopyToList.DataBind();

            this.MoveToList.DataSource = columns;
            this.MoveToList.DataTextField = "DisplayName";
            this.MoveToList.DataValueField = "bvin";
            this.MoveToList.DataBind();

        }

        private void LoadEditor()
        {
            System.Web.UI.Control tempControl = ModuleController.LoadContentBlockEditor(b.ControlName.Replace(" ",""), this);

            if (tempControl is BVModule)
            {
                editor = (BVModule)tempControl;
                if (editor != null)
                {
                    editor.BlockId = b.Bvin;
                    this.TitleLabel.Text = "Edit Block - " + b.ControlName;
                    this.phEditor.Controls.Add(editor);
                }
            }
            else
            {
                this.phEditor.Controls.Add(new System.Web.UI.LiteralControl("Error, editor is not based on Content.BVModule class"));
            }

            this.editor.EditingComplete += this.editor_EditingComplete;
        }

        protected void editor_EditingComplete(object sender, MerchantTribe.Commerce.Content.BVModuleEventArgs e)
        {
            Response.Redirect("Columns_Edit.aspx?id=" + b.ColumnId);
        }

        protected void btnGoCopy_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            this.msg.ClearMessage();
            if (MTApp.ContentServices.Columns.CopyBlockToColumn(b.Bvin,CopyToList.SelectedValue) == true)
            {
                this.msg.ShowOk("Block Copied");
            }
            else
            {
                this.msg.ShowError("Copy failed. Unknown Error.");
            }
        }

        protected void btnGoMove_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            this.msg.ClearMessage();
            if (MTApp.ContentServices.Columns.MoveBlockToColumn(b.Bvin,MoveToList.SelectedValue) == true)
            {
                this.msg.ShowOk("Block Moved");
            }
            else
            {
                this.msg.ShowError("Move failed. Unknown Error.");
            }
        }

    }
}