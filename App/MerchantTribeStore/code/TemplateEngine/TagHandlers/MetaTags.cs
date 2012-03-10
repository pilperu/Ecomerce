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

        public void Process(List<ITemplateAction> actions, MerchantTribe.Commerce.MerchantTribeApplication app, ITagProvider tagProvider, ParsedTag tag, string innerContents)
        {
            actions.Add(new Actions.PartialView("~/views/shared/_MetaTags.cshtml", null));
        }
    }
}