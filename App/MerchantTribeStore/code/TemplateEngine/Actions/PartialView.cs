using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MerchantTribeStore.code.TemplateEngine.Actions
{
    public class PartialView: ITemplateAction
    {
        public TemplateActionType ActionType() { return TemplateActionType.PartialView; }
        public string ViewName { get; set; }
        public object Model { get; set; }

        public PartialView(string viewName, object model)
        {
            this.ViewName = viewName;
            this.Model = model;
        }

        public string RenderCustom()
        {
            throw new NotImplementedException();
        }
    }
}