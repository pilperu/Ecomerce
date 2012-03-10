using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Dynamic;

namespace MerchantTribeStore.code.TemplateEngine.TagHandlers
{
    public class Copyright: ITagHandler
    {

        public string TagName
        {
            get { return "sys:copyright"; }
        }

        public void Process(List<ITemplateAction> actions, MerchantTribe.Commerce.MerchantTribeApplication app, ITagProvider tagProvider, ParsedTag tag, string innerContents)
        {            
            string byParam = tag.GetSafeAttribute("by");
            
            dynamic model = new ExpandoObject();
            model.By = byParam;

            actions.Add(new Actions.PartialView("~/views/shared/_copyright.cshtml", model));
        }
    }
}