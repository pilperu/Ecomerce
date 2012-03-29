using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;

namespace MerchantTribeStore.code.TemplateEngine.TagHandlers
{
    public class JavaScript: ITagHandler
    {

        public string TagName
        {
            get { return "sys:javascript"; }
        }

        public void Process(List<ITemplateAction> actions, MerchantTribe.Commerce.MerchantTribeApplication app, ITagProvider tagProvider, ParsedTag tag, string innerContents)
        {
            StringBuilder sb = new StringBuilder();
            bool secure = app.CurrentRequestContext.RoutingContext.HttpContext.Request.IsSecureConnection;
            string mode = tag.GetSafeAttribute("mode");

            if (mode == "system")
            {
                string baseScriptFolder = app.CurrentStore.RootUrl();
                if (secure) baseScriptFolder = app.CurrentStore.RootUrlSecure();
                if (baseScriptFolder.EndsWith("/") == false)
                {
                    baseScriptFolder += "/";
                }
                baseScriptFolder += "scripts/";

                bool useCDN = false;
                string cdn = tag.GetSafeAttribute("cdn");
                if (cdn == "1" || cdn == "true" || cdn == "y" || cdn == "Y") useCDN = true;

                if (useCDN)
                {
                    // CDN JQuery
                    if (secure)
                    {
                        sb.Append("<script src='https://ajax.microsoft.com/ajax/jQuery/jquery-1.5.1.min.js' type=\"text/javascript\"></script>");
                    }
                    else
                    {
                        sb.Append("<script src='http://ajax.microsoft.com/ajax/jQuery/jquery-1.5.1.min.js' type=\"text/javascript\"></script>");
                    }
                }
                else
                {
                    // Local JQuery
                    sb.Append("<script src='" + baseScriptFolder + "jquery-1.5.1.min.js' type=\"text/javascript\"></script>");
                }
                sb.Append(System.Environment.NewLine);

                sb.Append("<script src='" + baseScriptFolder + "jquery-ui-1.8.7.custom/js/jquery-ui-1.8.7.custom.min.js' type=\"text/javascript\"></script>");
                sb.Append("<script src='" + baseScriptFolder + "jquery.form.js' type=\"text/javascript\"></script>");
                sb.Append(System.Environment.NewLine);
            }
            else
            {
                string src = tag.GetSafeAttribute("src");
                string fileName = tag.GetSafeAttribute("file");
                if (fileName.Trim().Length > 0)
                {
                    var tm = app.ThemeManager();
                    src = tm.ThemeFileUrl(fileName, app);
                }
                sb.Append("<script src=\"" + src + "\" type=\"text/javascript\"></script>");
            }

            actions.Add(new Actions.LiteralText(sb.ToString()));
        }
    }
}