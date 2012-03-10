using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MerchantTribeStore.code.TemplateEngine
{
    public interface ITemplateAction
    {
        TemplateActionType ActionType();
        string RenderCustom();
    }
}