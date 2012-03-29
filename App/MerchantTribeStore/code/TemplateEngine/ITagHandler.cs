using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MerchantTribe.Commerce;

namespace MerchantTribeStore.code.TemplateEngine
{
    public interface ITagHandler
    {
        string TagName { get; }
        void Process(List<ITemplateAction> actions, 
                     MerchantTribeApplication app, 
                     ITagProvider tagProvider, 
                     ParsedTag tag, 
                     string innerContents);
    }
}