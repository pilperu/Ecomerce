using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;

namespace MerchantTribeStore.code.TemplateEngine.TagHandlers
{
    public class TempMessages : ITagHandler
    {

        public string TagName
        {
            get { return "sys:tempmessages"; }
        }

        public void Process(List<ITemplateAction> actions, MerchantTribe.Commerce.MerchantTribeApplication app, ITagProvider tagProvider, ParsedTag tag, string innerContents)
        {
            StringBuilder sb = new StringBuilder();
            foreach (string s in app.CurrentRequestContext.TempMessages)
            {
                sb.Append(s);
            }            
            actions.Add(new Actions.LiteralText(sb.ToString()));
        }
    }
}