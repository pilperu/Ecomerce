using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MerchantTribe.Commerce;

namespace MerchantTribeStore.Filters
{
    public class StoreClosedFilterAttribute: ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // do base action first to ensure we have our context objects like mtapp
            base.OnActionExecuting(filterContext);

            // skip everything if we're a store controller with closed actions, etc.
            if (filterContext.Controller is Controllers.StoreController) return;

            // otherwise check for a store closed page
            if (filterContext.Controller is Controllers.Shared.BaseStoreController)
            {
                MerchantTribeApplication app = ((Controllers.Shared.BaseStoreController)filterContext.Controller).MTApp;
                if (app != null)
                {
                    if (app.CurrentRequestContext.CurrentStore.Settings.StoreClosed)
                    {
                        bool hasPass = false;
                        string guestPass = SessionManager.StoreClosedGuestPasswordForCurrentUser;
                        if (guestPass.Trim().Length > 0)
                        {
                            if (guestPass == app.CurrentStore.Settings.StoreClosedGuestPassword)
                            {
                                hasPass = true;
                            }
                        }
                        if (app.CurrentRequestContext.IsAdmin(app) == false && hasPass == false)
                        {
                            filterContext.Result = new RedirectResult("~/storeclosed");
                        }
                    }
                }
            }            
        }
    }
}