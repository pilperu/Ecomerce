using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using StackExchange.Profiling;

namespace MerchantTribeStore.code.TemplateEngine.TagHandlers
{
    public class ProfilerRender : ITagHandler
    {
        public string TagName
        {
            get { return "sys:profilerrender"; }
        }

        public void Process(StringBuilder output, 
                            MerchantTribe.Commerce.MerchantTribeApplication app, 
                            dynamic viewBag, 
                            ITagProvider tagProvider, 
                            ParsedTag tag, 
                            string innerContents)
        {
            var rendered = MiniProfiler.RenderIncludes();            
            output.Append(rendered.ToHtmlString());            
        }
    }
}