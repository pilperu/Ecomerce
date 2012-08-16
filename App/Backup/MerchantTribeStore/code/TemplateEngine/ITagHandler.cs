using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MerchantTribe.Commerce;
using System.Text;

namespace MerchantTribeStore.code.TemplateEngine
{
    public interface ITagHandler
    {
        string TagName { get; }
        void Process(StringBuilder output, 
                     MerchantTribeApplication app,
                     dynamic viewBag,
                     ITagProvider tagProvider, 
                     ParsedTag tag, 
                     string innerContents);
    }
}