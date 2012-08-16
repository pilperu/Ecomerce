
namespace MerchantTribeStore
{

    partial class BVModules_Controls_MasterPageJavascript : System.Web.UI.UserControl
    {

        private string _Src = string.Empty;
        public string Src
        {
            get { return _Src; }
            set { _Src = value; }
        }

        protected override void OnInit(System.EventArgs e)
        {
            base.OnInit(e);
            RenderTag();
        }

        private void RenderTag()
        {
            string tag = "<script type=\"text/javascript\" src=\"" + Page.ResolveUrl(_Src) + "\" language=\"javascript\"></script>";
            this.litMain.Text = tag;
        }

    }
}