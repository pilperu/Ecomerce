using System;
using System.Text;

namespace MerchantTribeStore
{

    partial class BVAdmin_HelpWithThisPage : System.Web.UI.UserControl
    {


        private void RegisterWindowScripts()
        {

            string nameOfPage = Request.AppRelativeCurrentExecutionFilePath;

            StringBuilder sb = new StringBuilder();

            sb.Append("var w;");
            sb.Append("function OpenHelpWindow() {");
            sb.Append("w = window.open('http://help.merchanttribestores.com?page=");
            sb.Append(Server.UrlEncode(nameOfPage));
            sb.Append("', 'onlineHelp', 'height=700, width=200');");
            sb.Append("}");

            Page.ClientScript.RegisterClientScriptBlock(typeof(System.Web.UI.Page), "OnlineHelpScripts", sb.ToString(), true);

        }

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);
            this.RegisterWindowScripts();
        }

    }
}