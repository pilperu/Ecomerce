using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MerchantTribeStore.code.TemplateEngine.TagHandlers
{
    public class Css: ITagHandler
    {

        public string TagName
        {
            get { return "sys:css"; }
        }

        public void Process(List<ITemplateAction> actions, MerchantTribe.Commerce.MerchantTribeApplication app, ITagProvider tagProvider, ParsedTag tag, string innerContents)
        {
            string fileUrl = string.Empty;
            bool secure = app.IsCurrentRequestSecure();
            var tm = app.ThemeManager();

            string mode = tag.GetSafeAttribute("mode");
            if (mode == "legacy")
            {
                fileUrl = tm.CurrentStyleSheet(app, secure);
            }
            else if (mode == "system")
            {
                string cssFile = tag.GetSafeAttribute("file");
                fileUrl = app.StoreUrl(secure, false) + cssFile.TrimStart('/');
            }
            else
            {
                string fileName = tag.GetSafeAttribute("file");
                fileUrl = tm.ThemeFileUrl(fileName, app);
            }

            if (fileUrl.StartsWith("http://"))
            {
                fileUrl = fileUrl.Replace("http://", "//");
            }

            string result = string.Empty;
            result = "<link href=\"" + fileUrl + "\" rel=\"stylesheet\" type=\"text/css\" />";
            actions.Add(new Actions.LiteralText(result));            
        }
    }
}