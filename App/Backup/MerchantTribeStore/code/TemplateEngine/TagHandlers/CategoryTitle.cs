using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Dynamic;
using System.Text;

namespace MerchantTribeStore.code.TemplateEngine.TagHandlers
{
    public class CategoryTitle : ITagHandler
    {

        public string TagName
        {
            get { return "sys:category-title"; }
        }

        public void Process(StringBuilder output,
                            MerchantTribe.Commerce.MerchantTribeApplication app,
                            dynamic viewBag,
                            ITagProvider tagProvider,
                            ParsedTag tag,
                            string innerContents)
        {
            var category = app.CurrentRequestContext.CurrentCategory;
            if (category != null)
            {                
                if (category.ShowTitle == true)
                {
                    output.Append("<h1>" + HttpUtility.HtmlEncode(category.Name) + "</h1>");
                }
            }
        }
    }
}