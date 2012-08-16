using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;

namespace MerchantTribeStore.code.TemplateEngine.TagHandlers
{
    public class AnalyticsTop : ITagHandler
    {

        public string TagName
        {
            get { return "sys:analyticstop"; }
        }

        public void Process(StringBuilder output,
                            MerchantTribe.Commerce.MerchantTribeApplication app,
                            dynamic viewBag,
                            ITagProvider tagProvider,
                            ParsedTag tag,
                            string innerContents)
        {
            output.Append((string)viewBag.analyticstop);
        }
    }
}