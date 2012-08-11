using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MerchantTribe.Commerce;
using MerchantTribe.Commerce.Content;
using MerchantTribeStore.Filters;
using MerchantTribe.Commerce.Utilities;

namespace MerchantTribeStore.Controllers.Shared
{
    [StoreClosedFilter]
    public class BaseAppController : Controller, IMultiStorePage
    {
        public MerchantTribeApplication MTApp { get; set; }
        public string UniqueStoreId { get; set; }
        public string CustomerId { get; set; }
        public string CustomerIp { get; set; }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
                base.OnActionExecuting(filterContext);

                MTApp = MerchantTribeApplication.InstantiateForDataBase(new RequestContext());
                // Check for non-www url and redirect if needed
                //RedirectBVCommerceCom(System.Web.HttpContext.Current);
                MTApp.CurrentRequestContext.RoutingContext = this.Request.RequestContext;
                MTApp.CurrentRequestContext.UrlHelper = this.Url;

                // Determine store id        
                MTApp.CurrentStore = MerchantTribe.Commerce.Utilities.UrlHelper.ParseStoreFromUrl(System.Web.HttpContext.Current.Request.Url, MTApp);
                if (MTApp.CurrentStore == null)
                {
                    Response.Redirect("~/storenotfound");
                }


                if (MTApp.CurrentStore.Status == MerchantTribe.Commerce.Accounts.StoreStatus.Deactivated)
                {
                    //if ((AvailableWhenInactive == false))
                    //{
                    Response.Redirect("~/storenotavailable");
                    //}
                }


                if (ViewBag.IsAdmin == null)
                {
                    // Store data for admin panel                            
                    ViewBag.IsAdmin = IsCurrentUserAdmin(this.MTApp, this.Request.RequestContext.HttpContext);
                }


                ViewBag.RootUrlSecure = this.MTApp.StoreUrl(true, false);
                ViewBag.RootUrl = this.MTApp.StoreUrl(false, true);

                ViewBag.StoreClosed = MTApp.CurrentStore.Settings.StoreClosed;
                ViewBag.StoreName = MTApp.CurrentStore.Settings.FriendlyName;

                this.UniqueStoreId = MTApp.CurrentStore.StoreUniqueId(MTApp);
                ViewBag.StoreUniqueId = this.UniqueStoreId;
                this.CustomerIp = Request.UserHostAddress ?? "0.0.0.0";
                ViewBag.CustomerIp = this.CustomerIp;
                this.CustomerId = MTApp.CurrentCustomerId ?? string.Empty;
                ViewBag.CustomerId = this.CustomerId;
                ViewBag.HideAnalytics = MTApp.CurrentStore.Settings.Analytics.DisableMerchantTribeAnalytics;            
        }

        public bool IsCurrentUserAdmin(MerchantTribeApplication app, HttpContextBase httpContext)
        {

            Guid? tokenId = MerchantTribe.Web.Cookies.GetCookieGuid(
                                WebAppSettings.CookieNameAuthenticationTokenAdmin(app.CurrentStore.Id),
                                httpContext, new EventLog());

            // no token, return
            if (!tokenId.HasValue) return false;

            if (app.AccountServices.IsTokenValidForStore(app.CurrentStore.Id, tokenId.Value))
            {
                return true;
            }

            return false;
        }


        protected void FlashInfo(string message)
        {
            FlashMessage(message, "flash-message-info");
        }
        protected void FlashSuccess(string message)
        {
            FlashMessage(message, "flash-message-success");
        }
        protected void FlashFailure(string message)
        {
            FlashMessage(message, "flash-message-failure");
        }
        protected void FlashWarning(string message)
        {
            FlashMessage(message, "flash-message-warning");
        }
        private void FlashMessage(string message, string typeClass)
        {
            string format = "<div class=\"{0}\"><p>{1}</p></div>";
            this.TempData["messages"] += string.Format(format, typeClass, message);
        }
    }
}
