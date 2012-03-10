using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MerchantTribeStore.code.TemplateEngine.Actions
{
    public class CallAction: ITemplateAction
    {
        public string ActionName { get; set; }
        public string ControllerName { get; set; }
        public object RouteValues { get; set; }

        public CallAction(string controllerName, string actionName, object routeValues)
        {
            this.ControllerName = controllerName;
            this.ActionName = actionName;
            this.RouteValues = routeValues;
        }

        TemplateActionType ITemplateAction.ActionType()
        {
            return TemplateActionType.CallAction;
        }

        string ITemplateAction.RenderCustom()
        {
            throw new NotImplementedException();
        }
    }
}