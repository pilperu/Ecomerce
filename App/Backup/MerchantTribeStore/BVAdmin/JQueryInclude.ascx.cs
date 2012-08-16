using System;
using System.Web;
using System.Web.UI;
using System.Text;
using System.IO;

namespace MerchantTribeStore
{

    partial class JQueryInclude : System.Web.UI.UserControl
    {

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            BuildJQueryIncludes();
        }

        private void BuildJQueryIncludes()
        {
            string baseScriptFolder = Page.ResolveUrl("~/scripts");
            this.litJQuery.Text = Helpers.Html.JQueryIncludes(baseScriptFolder, Request.IsSecureConnection);            
        }

    }
}