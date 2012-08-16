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

        public void Process(StringBuilder output, 
                            MerchantTribe.Commerce.MerchantTribeApplication app, 
                            dynamic viewBag,
                            ITagProvider tagProvider, 
                            ParsedTag tag, 
                            string innerContents)
        {            
            foreach (string s in app.CurrentRequestContext.TempMessages)
            {
                output.Append(s);
            }                        
        }
    }
}