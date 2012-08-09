using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MerchantTribeStore.code.TemplateEngine.TagHandlers
{
    public class AnalyticsBottom : ITagHandler
    {

        public string TagName
        {
            get { return "sys:analyticsbottom"; }
        }

        public void Process(List<ITemplateAction> actions,
                            MerchantTribe.Commerce.MerchantTribeApplication app,
                            dynamic viewBag,
                            ITagProvider tagProvider,
                            ParsedTag tag,
                            string innerContents)
        {
            actions.Add(new Actions.LiteralText(viewBag.analyticsbottom));
        }
    }
}