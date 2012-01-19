using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MerchantTribe.Commerce;
using MerchantTribe.Commerce.Content;
using MerchantTribeStore.Filters;

namespace MerchantTribeStore.Controllers.Shared
{
    [StoreClosedFilter]
    public class BaseStoreController : BaseAppController
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            //Cart Count
            string itemCount = "0";
            string subTotal = "$0.00";
            if (SessionManager.CurrentUserHasCart(MTApp.CurrentStore))
            {
                itemCount = SessionManager.GetCookieString(WebAppSettings.CookieNameCartItemCount(MTApp.CurrentStore.Id), MTApp.CurrentStore);
                subTotal = SessionManager.GetCookieString(WebAppSettings.CookieNameCartSubTotal(MTApp.CurrentStore.Id), MTApp.CurrentStore);
                if (itemCount.Trim().Length < 1) itemCount = "0";
                if (subTotal.Trim().Length < 1) subTotal = "$0.00";
            }
            ViewData["CurrentCartItemCount"] = itemCount;
            this.MTApp.CurrentRequestContext.CartItemCount = itemCount;

            ViewData["CurrentCartSubTotal"] = subTotal;
            this.MTApp.CurrentRequestContext.CartSubtotal = subTotal;
            
            // style sheet
            ThemeManager themes = MTApp.ThemeManager();
            ViewBag.Css = themes.CurrentStyleSheet(MTApp,System.Web.HttpContext.Current.Request.IsSecureConnection);

            // Add Google Tracker to Page
            if (MTApp.CurrentStore.Settings.Analytics.UseGoogleTracker)
            {
                ViewData["analyticstop"] = MerchantTribe.Commerce.Metrics.GoogleAnalytics.RenderLatestTracker(MTApp.CurrentStore.Settings.Analytics.GoogleTrackerId);
            }

            // Additional Meta Tags
            ViewData["AdditionalMetaTags"] = MTApp.CurrentStore.Settings.Analytics.AdditionalMetaTags ?? string.Empty;
            
            // Bottom Analytics Tags
            this.ViewData["analyticsbottom"] = MTApp.CurrentStore.Settings.Analytics.BottomAnalytics ?? string.Empty;

            // JQuery
            ViewBag.JqueryInclude = Helpers.Html.JQueryIncludes(Url.Content("~/scripts"), this.Request.IsSecureConnection);

            // header and footer            
            string header = MerchantTribe.Commerce.Storage.DiskStorage.ReadCustomHeader(MTApp.CurrentStore.Id, MTApp.CurrentStore.Settings.ThemeId);
            string footer = MerchantTribe.Commerce.Storage.DiskStorage.ReadCustomFooter(MTApp.CurrentStore.Id, MTApp.CurrentStore.Settings.ThemeId);
            if ((header.Trim().Length < 1))
            {
                header = MerchantTribe.Commerce.Utilities.HtmlRendering.StandardHeader();
            }
            ViewData["siteheader"] = MerchantTribe.Commerce.Utilities.TagReplacer.ReplaceContentTags(header, MTApp, itemCount, Request.IsSecureConnection);
            if ((footer.Trim().Length < 1))
            {
                footer = MerchantTribe.Commerce.Utilities.HtmlRendering.StandardFooter(MTApp);
            }
            footer = footer + MerchantTribe.Commerce.Utilities.HtmlRendering.PromoTag();
            ViewData["sitefooter"] = MerchantTribe.Commerce.Utilities.TagReplacer.ReplaceContentTags(footer, MTApp, itemCount, Request.IsSecureConnection);

            //log affiliate request
            if (!((string)Request.Params[WebAppSettings.AffiliateQueryStringName] == null))
            {
                string affid = string.Empty;
                try
                {
                    affid = Request.Params[WebAppSettings.AffiliateQueryStringName];
                    string referrerURL = Request.UrlReferrer.AbsoluteUri;
                    if (referrerURL == null) referrerURL = string.Empty;
                    MTApp.ContactServices.RecordAffiliateReferral(affid, referrerURL, MTApp);

                }
                catch (System.Exception ex)
                {
                    EventLog.LogEvent("BaseStorePage - Page_Init", "Error loading affiliate " + ex.Message, MerchantTribe.Web.Logging.EventLogSeverity.Warning);
                }
            }

            //If this is a private store, force login before showing anything.
            if (MTApp.CurrentStore.Settings.IsPrivateStore == true)
            {
                if (SessionManager.IsUserAuthenticated(this.MTApp) == false)
                {
                    string nameOfPage = Request.AppRelativeCurrentExecutionFilePath;
                    // Check to make sure we're not going to end up in an endless loop of redirects
                    if ((!nameOfPage.ToLower().StartsWith("~/signin"))
                        && (!nameOfPage.ToLower().StartsWith("~/forgotpassword.aspx"))
                        && (!nameOfPage.ToLower().StartsWith("~/contactus.aspx")))
                    {
                        Response.Redirect("~/signin?ReturnUrl=" + HttpUtility.UrlEncode(this.Request.RawUrl));
                    }
                }
            }
            
            ViewBag.MetaKeywords = MTApp.CurrentStore.Settings.MetaKeywords;            
            ViewBag.MetaDescription = MTApp.CurrentStore.Settings.MetaDescription;    
            
            
            // Save current URL for facebook like, etc.
            ViewBag.RawUrl = Request.Url.ToString();
            ViewBag.CurrentUrl = MTApp.CurrentStore.RootUrl() + Request.Path.TrimStart('/');

            // Social Media Globals
            ViewBag.UseFaceBook = MTApp.CurrentStore.Settings.FaceBook.UseFaceBook;
            ViewBag.FaceBookAdmins = MTApp.CurrentStore.Settings.FaceBook.Admins;
            ViewBag.FaceBookAppId = MTApp.CurrentStore.Settings.FaceBook.AppId;
            
            ViewBag.UseTwitter = MTApp.CurrentStore.Settings.Twitter.UseTwitter;            
            ViewBag.TwitterHandle = MTApp.CurrentStore.Settings.Twitter.TwitterHandle;
            ViewBag.TwitterDefaultTweetText = MTApp.CurrentStore.Settings.Twitter.DefaultTweetText;

            ViewBag.UseGooglePlus = MTApp.CurrentStore.Settings.GooglePlus.UseGooglePlus;

            // Culture Settings
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(MTApp.CurrentStore.Settings.CultureCode);
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(MTApp.CurrentStore.Settings.CultureCode);

        }

    }
}
