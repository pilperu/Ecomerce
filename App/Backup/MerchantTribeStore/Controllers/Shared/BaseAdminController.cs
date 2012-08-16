using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MerchantTribe.Commerce;
using MerchantTribe.Commerce.Accounts;

namespace MerchantTribeStore.Controllers.Shared
{
    public class BaseAdminController : BaseAppController
    {
        // Initialize Store Specific Request Data
        private Guid? _AuthTokenGuid = null;
        public Guid? AuthTokenGuid
        {
            get { return _AuthTokenGuid; }
            set { _AuthTokenGuid = value; }
        }

        public AdminTabType SelectedTab = AdminTabType.Dashboard;        

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            // Culture Settings
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(MTApp.CurrentStore.Settings.CultureCode);
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(MTApp.CurrentStore.Settings.CultureCode);
            
            ValidateAdminLogin();            

            // Jquery
            ViewData["JQueryInclude"] = Helpers.Html.JQueryIncludes(Url.Content("~/scripts"), this.Request.IsSecureConnection);

            ViewData["AppVersion"] = WebAppSettings.SystemVersionNumber;
            ViewData["StoreName"] = MTApp.CurrentStore.Settings.FriendlyName;
            ViewData["RenderedMenu"] = Helpers.Html.RenderMenu(this.SelectedTab, MTApp);
            ViewData["HideMenu"] = 0;
        }

        public void ValidateAdminLogin()
        {
            bool validLogin = false;

            Guid? tokenId = MerchantTribe.Web.Cookies.GetCookieGuid(WebAppSettings.CookieNameAuthenticationTokenAdmin(MTApp.CurrentStore.Id),
                    this.HttpContext,
                    new EventLog());

            if (tokenId.HasValue)
            {
                if (this.MTApp.AccountServices.IsTokenValidForStore(this.MTApp.CurrentStore.Id, tokenId.Value))
                {
                    validLogin = true;
                }
            }

            if (validLogin == false)
            {
                Response.Redirect("~/adminaccount/login");
            }
            
            _AuthTokenGuid = tokenId;
        }

    }
}
