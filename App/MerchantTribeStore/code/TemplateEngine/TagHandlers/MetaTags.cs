using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;

namespace MerchantTribeStore.code.TemplateEngine.TagHandlers
{
    public class MetaTags: ITagHandler
    {
        public string TagName
        {
            get { return "sys:metatags"; }
        }

        public void Process(StringBuilder output, 
                            MerchantTribe.Commerce.MerchantTribeApplication app, 
                            dynamic viewBag,
                            ITagProvider tagProvider, 
                            ParsedTag tag, 
                            string innerContents)
        {
            output.Append("<meta name=\"keywords\" content=\"" + HttpUtility.HtmlEncode((string)viewBag.MetaKeywords) + "\" />");
            output.Append("<meta name=\"description\" content=\"" + HttpUtility.HtmlEncode((string)viewBag.MetaDescription) + "\" />");
            if (!String.IsNullOrEmpty(viewBag.AdditionalMetaTags))
            {
                output.Append((string)viewBag.AdditionalMetaTags);
            }            
        }
    }
}