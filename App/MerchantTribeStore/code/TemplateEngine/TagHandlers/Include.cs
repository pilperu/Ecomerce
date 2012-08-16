using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MerchantTribe.Commerce;
using MerchantTribe.Commerce.Content;
using System.Text;

namespace MerchantTribeStore.code.TemplateEngine.TagHandlers
{
    public class Include : ITagHandler
    {
        public string TagName
        {
            get { return "sys:include"; }
        }

        public void Process(StringBuilder output, 
                                 MerchantTribeApplication app, 
                                 dynamic viewBag,
                                 ITagProvider tagProvider, 
                                 ParsedTag tag, 
                                 string innerContents)
        {
            string partName = tag.GetSafeAttribute("part");

            ThemeManager tm = app.ThemeManager();
            string result = tm.GetTemplatePartFromCurrentTheme(partName);
            Processor proc = new Processor(app, viewBag, result, tagProvider);
            proc.RenderForDisplay(output);            
        }


    }
}