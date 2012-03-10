using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MerchantTribeStore.code.TemplateEngine.TagHandlers
{
    public class PromoTag: ITagHandler
    {

        public string TagName
        {
            get { return "sys:promotag"; }
        }

        public void Process(List<ITemplateAction> actions, MerchantTribe.Commerce.MerchantTribeApplication app, ITagProvider tagProvider, ParsedTag tag, string innerContents)
        {
           actions.Add(new Actions.LiteralText(MerchantTribe.Commerce.Utilities.HtmlRendering.PromoTag()));
        }
    }
}