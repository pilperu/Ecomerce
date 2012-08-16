using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web;
using System.Web.Mvc;

namespace MerchantTribeStore.Filters
{
    public class NonCacheableResponseFilterAttribute: ActionFilterAttribute
    {                
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {            
            base.OnActionExecuted(filterContext);            
            filterContext.HttpContext.Response.Cache.SetCacheability(System.Web.HttpCacheability.NoCache);
            filterContext.HttpContext.Response.Cache.SetNoStore();
            filterContext.HttpContext.Response.Cache.SetExpires(DateTime.Now.AddDays(-5000));
            filterContext.HttpContext.Response.Cache.SetMaxAge(new TimeSpan(0));
            filterContext.HttpContext.Response.Cache.AppendCacheExtension("must-revalidate, proxy-revalidate");
        }
    }
}