
using MerchantTribe.Commerce;
using MerchantTribe.Commerce.Content;
using System.IO;

namespace MerchantTribeStore
{

    partial class BVAdmin_Controls_HtmlEditor : System.Web.UI.UserControl
    {
        private TextEditorBase _Editor = null;

        public string Text
        {
            get { return _Editor.Text; }
            set { _Editor.Text = value; }
        }
        public string PreTransformText
        {
            get { return _Editor.PreTransformText; }
            set { _Editor.PreTransformText = value; }
        }
        public bool SupportsTransform
        {
            get { return _Editor.SupportsTransform; }
        }
        public int EditorWidth
        {
            get { return int.Parse(this.WidthField.Value); }
            set { this.WidthField.Value = value.ToString(); }
        }
        public int EditorHeight
        {
            get { return int.Parse(this.HeightField.Value); }
            set { this.HeightField.Value = value.ToString(); }
        }
        public bool EditorWrap
        {
            get
            {
                if (this.WrapField.Value == "1")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            set
            {
                if (value == true)
                {
                    this.WrapField.Value = "1";
                }
                else
                {
                    this.WrapField.Value = "0";
                }
            }
        }

        public int TabIndex
        {
            get
            {
                object obj = ViewState["TabIndex"];
                if (obj != null)
                {
                    return (int)obj;
                }
                else
                {
                    return -1;
                }
            }
            set { ViewState["TabIndex"] = value; }
        }

        protected override void OnInit(System.EventArgs e)
        {
            base.OnInit(e);
            _Editor = (TextEditorBase)ModuleController.LoadDefaultEditor(this.Page);
            this.phEditor.Controls.Add(_Editor);
        }

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);
            if (!Page.IsPostBack)
            {
                _Editor.EditorHeight = this.EditorHeight;
                _Editor.EditorWidth = this.EditorWidth;
                _Editor.EditorWrap = this.EditorWrap;
                if (this.TabIndex != -1)
                {
                    _Editor.TabIndex = this.TabIndex;
                }
            }

        }

    }
}