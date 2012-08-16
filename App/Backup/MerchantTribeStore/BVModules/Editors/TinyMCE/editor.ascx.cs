using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MerchantTribe.Commerce.Content;
using MerchantTribe.Commerce;


namespace MerchantTribeStore
{

    public partial class BVModules_Editors_TinyMCE_editor : TextEditorBase
    {

        public override int EditorHeight
        {
            get { return (int)this.EditorField.Height.Value; }
            set { this.EditorField.Height = Unit.Pixel(value); }
        }

        public override int EditorWidth
        {
            get { return (int)this.EditorField.Width.Value; }
            set { this.EditorField.Width = Unit.Pixel(value); }
        }

        public override bool EditorWrap
        {
            get { return this.EditorField.Wrap; }
            set { this.EditorField.Wrap = value; }
        }

        public override string PreTransformText
        {
            get { return this.EditorField.Text; }
            set { this.EditorField.Text = value; }
        }

        public override bool SupportsTransform
        {
            get { return false; }
        }

        public override int TabIndex
        {
            get { return this.EditorField.TabIndex; }
            set { this.EditorField.TabIndex = (short)value; }
        }

        public override string Text
        {
            get { return this.EditorField.Text; }
            set { this.EditorField.Text = value; }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.EditorField.CausesValidation = false;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (!Page.IsPostBack)
            {
                Page.ClientScript.RegisterClientScriptInclude("tinymcejquery", Page.ResolveUrl("~/scripts/tinymce/jquery.tinymce.js"));
                Page.ClientScript.RegisterClientScriptInclude("tinymceinit", Page.ResolveUrl("~/js/bvinit.js"));
            }

        }
    }
}