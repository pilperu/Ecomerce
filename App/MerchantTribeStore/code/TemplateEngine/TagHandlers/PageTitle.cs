using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MerchantTribeStore.code.TemplateEngine.TagHandlers
{
    public class PageTitle: ITagHandler
    {

        public string TagName
        {
            get { return "sys:pagetitle"; }
        }

        public void Process(List<ITemplateAction> actions, MerchantTribe.Commerce.MerchantTribeApplication app, ITagProvider tagProvider, ParsedTag tag, string innerContents)
        {
            actions.Add(new Actions.PartialView("~/views/shared/_PageTitle.cshtml", null));
        }
    }
}