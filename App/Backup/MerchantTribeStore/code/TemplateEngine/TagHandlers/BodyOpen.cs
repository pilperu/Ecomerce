using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;

namespace MerchantTribeStore.code.TemplateEngine.TagHandlers
{
    public class BodyOpen : ITagHandler
    {

        public string TagName
        {
            get { return "sys:bodyopen"; }
        }

        public void Process(StringBuilder output,
                            MerchantTribe.Commerce.MerchantTribeApplication app,
                            dynamic viewBag,
                            ITagProvider tagProvider,
                            ParsedTag tag,
                            string innerContents)
        {
            string id = tag.GetSafeAttribute("id");
            if (!string.IsNullOrEmpty((string)viewBag.BodyClass))
            {
                id = viewBag.BodyClass;
            }
            output.Append("<body id=\"" + id + "\">");
        }
    }
}