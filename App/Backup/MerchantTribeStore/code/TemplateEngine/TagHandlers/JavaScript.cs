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

        public void Process(StringBuilder output, 
                            MerchantTribe.Commerce.MerchantTribeApplication app, 
                            dynamic viewBag,
                            ITagProvider tagProvider, 
                            ParsedTag tag, 
                            string innerContents)
        {            
            bool secure = app.IsCurrentRequestSecure();            
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
                        output.Append("<script src='https://ajax.microsoft.com/ajax/jQuery/jquery-1.5.1.min.js' type=\"text/javascript\"></script>");
                    }
                    else
                    {
                        output.Append("<script src='http://ajax.microsoft.com/ajax/jQuery/jquery-1.5.1.min.js' type=\"text/javascript\"></script>");
                    }
                }
                else
                {
                    // Local JQuery
                    output.Append("<script src='" + baseScriptFolder + "jquery-1.5.1.min.js' type=\"text/javascript\"></script>");
                }
                output.Append(System.Environment.NewLine);

                output.Append("<script src='" + baseScriptFolder + "jquery-ui-1.8.7.custom/js/jquery-ui-1.8.7.custom.min.js' type=\"text/javascript\"></script>");
                output.Append("<script src='" + baseScriptFolder + "jquery.form.js' type=\"text/javascript\"></script>");
                output.Append(System.Environment.NewLine);
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
                output.Append("<script src=\"" + src + "\" type=\"text/javascript\"></script>");
            }            
        }
    }
}