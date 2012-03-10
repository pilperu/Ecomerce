using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MerchantTribe.Commerce;

namespace MerchantTribeStore.code.TemplateEngine.TagHandlers
{
    public class AdminPanel : ITagHandler
    {
        public string TagName
        {
            get { return "sys:adminpanel"; }
        }

        public void Process(List<ITemplateAction> actions, MerchantTribeApplication app, ITagProvider tagProvider, ParsedTag tag, string contents)
        {
            actions.Add(new Actions.PartialView("~/views/shared/_adminpanel.cshtml", null));            
        }
    }
}
