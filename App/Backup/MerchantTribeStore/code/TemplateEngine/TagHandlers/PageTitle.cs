using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;

namespace MerchantTribeStore.code.TemplateEngine.TagHandlers
{
    public class PageTitle: ITagHandler
    {

        public string TagName
        {
            get { return "sys:pagetitle"; }
        }

        public void Process(StringBuilder output, 
                            MerchantTribe.Commerce.MerchantTribeApplication app, 
                            dynamic viewBag,
                            ITagProvider tagProvider, 
                            ParsedTag tag, 
                            string innerContents)
        {
            string title = viewBag.PageTitle;
            if (string.IsNullOrEmpty(title)) title = viewBag.Title;
            output.Append(HttpUtility.HtmlEncode(title));
        }
    }
}