using System.Web.UI.WebControls;
using MerchantTribe.Commerce;
using MerchantTribe.Commerce.Content;

namespace MerchantTribeStore
{
    public class BlankBlock: System.Web.UI.LiteralControl
    {
        public string bvin { get; set; }
    }

    partial class BVAdmin_Controls_ContentColumnEditor : MerchantTribe.Commerce.Content.BVUserControl
    {
        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);
            if (!Page.IsPostBack)
            {
                LoadBlockList();
                LoadColumn();
            }
        }

        public string ColumnId
        {
            get
            {
                object obj = ViewState["ColumnId"];
                if (obj != null)
                {
                    return (string)obj;
                }
                else
                {
                    return "";
                }
            }
            set { ViewState["ColumnId"] = value; }
        }

        private void LoadBlockList()
        {
            this.lstBlocks.DataSource = ModuleController.FindContentBlocks();
            this.lstBlocks.DataBind();
        }

        public void LoadColumn()
        {
            ContentColumn c = MyPage.MTApp.ContentServices.Columns.Find(this.ColumnId);
            this.lblTitle.Text = c.DisplayName;
            this.GridView1.DataSource = c.Blocks;
            this.GridView1.DataBind();
        }

        protected void btnNew_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {

            ContentColumn col = MyPage.MTApp.ContentServices.Columns.Find(this.ColumnId);
            if (col != null)
            {
                ContentBlock b = new ContentBlock();
                b.ControlName = this.lstBlocks.SelectedValue;                
                b.ColumnId = this.ColumnId;
                col.Blocks.Add(b);
                MyPage.MTApp.ContentServices.Columns.Update(col);
            }
            LoadColumn();
        }

        protected void GridView1_RowCancelingEdit(object sender, System.Web.UI.WebControls.GridViewCancelEditEventArgs e)
        {
            string bvin = string.Empty;
            //this.GridView1.UpdateAfterCallBack = true;
            bvin = ((GridView)sender).DataKeys[e.RowIndex].Value.ToString();
            MyPage.MTApp.ContentServices.Columns.MoveBlockDown(bvin, this.ColumnId, this.MyPage.MTApp.CurrentStore.Id);
            LoadColumn();
        }

        protected void GridView1_RowUpdating(object sender, System.Web.UI.WebControls.GridViewUpdateEventArgs e)
        {
            string bvin = string.Empty;
            //this.GridView1.UpdateAfterCallBack = true;
            bvin = ((GridView)sender).DataKeys[e.RowIndex].Value.ToString();
            MyPage.MTApp.ContentServices.Columns.MoveBlockUp(bvin, this.ColumnId, this.MyPage.MTApp.CurrentStore.Id);
            LoadColumn();
        }

        protected void GridView1_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ContentBlock b = (ContentBlock)e.Row.DataItem;

                bool controlFound = false;

                System.Web.UI.Control viewControl;

                // Control name gets spaces replaced for backwards compatibility
                viewControl = ModuleController.LoadContentBlockAdminView(b.ControlName.Replace(" ",""), this.Page);                
                if (viewControl == null)
                {
                    //No admin view, try standard view
                    
                    // Block views are now MVC partial so we need a way to render them here
                    // There are some tricks but since this page will eventually go MVC itself
                    // we'll just put in placeholders for now.
                    //viewControl = ModuleController.LoadContentBlock(b.ControlName, this.Page);
                    viewControl = new BlankBlock() { bvin= b.Bvin, Text= "<div>Block: " + b.ControlName + "</div>"};                    
                }


                if (viewControl != null)
                {
                    if ((viewControl is BVModule) || (viewControl is BlankBlock))
                    {
                        if (viewControl is BVModule)
                        {
                            ((BVModule)viewControl).BlockId = b.Bvin;
                        }
                        controlFound = true;
                        e.Row.Cells[0].Controls.Add(new System.Web.UI.LiteralControl("<div style=\"width:280px;overflow:auto;\">"));                        
                        e.Row.Cells[0].Controls.Add(viewControl);
                        e.Row.Cells[0].Controls.Add(new System.Web.UI.LiteralControl("</div>"));
                    }
                }

                if (controlFound == true)
                {
                    // Check for Editor
                    HyperLink lnkEdit = (HyperLink)e.Row.FindControl("lnkEdit");
                    if (lnkEdit != null)
                    {
                        lnkEdit.Visible = EditorExists(b.ControlName);
                    }
                }
                else
                {
                    e.Row.Cells[0].Controls.Add(new System.Web.UI.LiteralControl("Control " + b.ControlName + "could not be located"));
                }
            }
        }

        protected void GridView1_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
        {
            string bvin = string.Empty;
            //this.GridView1.UpdateAfterCallBack = true;
            bvin = ((GridView)sender).DataKeys[e.RowIndex].Value.ToString();
            MyPage.MTApp.ContentServices.Columns.DeleteBlock(bvin);
            LoadColumn();
        }

        private bool EditorExists(string controlName)
        {
            bool result = false;
            System.Web.UI.Control editorControl;
            editorControl = ModuleController.LoadContentBlockEditor(controlName.Replace(" ",""), this.Page);
            if (editorControl != null)
            {
                if (editorControl is BVModule)
                {
                    result = true;
                }
            }
            return result;
        }
    }
}