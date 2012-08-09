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

        public void Process(List<ITemplateAction> actions, 
                            MerchantTribe.Commerce.MerchantTribeApplication app, 
                            dynamic viewBag,
                            ITagProvider tagProvider, 
                            ParsedTag tag, 
                            string innerContents)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("<meta name=\"keywords\" content=\"" + HttpUtility.HtmlEncode(viewBag.MetaKeywords) + "\" />");
            sb.Append("<meta name=\"description\" content=\"" + HttpUtility.HtmlEncode(viewBag.MetaDescription) + "\" />");
            if (!String.IsNullOrEmpty(viewBag.AdditionalMetaTags))
            {
                sb.Append(viewBag.AdditionalMetaTags);
            }
            actions.Add(new Actions.LiteralText(sb.ToString()));            
        }
    }
}