using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Dynamic;
using System.Text;
using MerchantTribe.Commerce.Utilities;

namespace MerchantTribeStore.code.TemplateEngine.TagHandlers
{
    public class CategoryDescription : ITagHandler
    {

        public string TagName
        {
            get { return "sys:category-description"; }
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
                if (category.Description.Trim().Length > 0)
                {
                    var description = category.Description;
                    //var description = TagReplacer.ReplaceContentTags(category.Description,
                    //                                             app,
                    //                                             "",
                    //                                             app.IsCurrentRequestSecure());                    
                    output.Append(description);                    
                }
            }
        }
    }
}