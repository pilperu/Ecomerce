using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MerchantTribe.Commerce;

namespace MerchantTribeStore.Filters
{
    public class CustomerSignedInFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // do base action first to ensure we have our context objects like mtapp
            base.OnActionExecuting(filterContext);

            if (filterContext.Controller is Controllers.Shared.BaseStoreController)
            {
                MerchantTribeApplication app = ((Controllers.Shared.BaseStoreController)filterContext.Controller).MTApp;
                if (app != null)
                {
                    if (!SessionManager.IsUserAuthenticated(app))
                    {
                        filterContext.HttpContext.Response.Redirect("~/signin");
                    }
                }
            }
        }            
    }
}