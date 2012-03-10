using System;
using System.Web;
using System.Collections.Specialized;
using MerchantTribe.Web.Cryptography;

namespace MerchantTribe.Commerce.Utilities
{
	public class SSL
	{
		public enum SSLRedirectTo
		{
			SSL = 1,
			NonSSL = 2
		}

		public static string BuildUrlForRedirect(string currentUrl, string standardUrl, string secureUrl, SSLRedirectTo redirectTo, string sessionId, string cartId, string currentSessionId, string currentCartId, bool useClearText
		)
		{
			string url = string.Empty;

			RemoveAllEncoding(ref currentUrl);
			RemoveAllEncoding(ref standardUrl);
			RemoveAllEncoding(ref secureUrl);
			TripleDesEncryption enc = new TripleDesEncryption();

			switch (redirectTo) {
				case SSLRedirectTo.NonSSL:
					url = UrlRewriter.SwitchUrlToStandard(currentUrl);
                    break;				
				case SSLRedirectTo.SSL:				
					url = UrlRewriter.SwitchUrlToSecure(currentUrl);
                    break;				
			}

            //if (differentTld) {
            //    Uri temp = new Uri(url);
            //    NameValueCollection queryString = HttpUtility.ParseQueryString(temp.Query);

            //    object obj = queryString.GetValues(sessionId);
            //    if (!string.IsNullOrEmpty(currentSessionId.Trim())) {
            //        string sesval = currentSessionId;
            //        if ((!useClearText)) {
            //            sesval = enc.Encode(currentSessionId);
            //        }
            //        if (obj != null) {
            //            queryString[sessionId] = sesval;
            //        }
            //        else {
            //            queryString.Add(sessionId, sesval);
            //        }
            //    }

            //    obj = queryString.GetValues(cartId);
            //    if (!string.IsNullOrEmpty(currentCartId.Trim())) {
            //        string cidval = currentCartId;
            //        if ((!useClearText)) {
            //            cidval = enc.Encode(currentCartId);
            //        }
            //        if (obj != null) {
            //            queryString[cartId] = cidval;
            //        }
            //        else {
            //            queryString.Add(cartId, cidval);
            //        }
            //    }

            //    if (!string.IsNullOrEmpty(temp.Query)) {
            //        url = temp.AbsoluteUri.Replace(temp.Query, "");
            //    }
            //    else {
            //        url = temp.AbsoluteUri;
            //    }

            //    if (queryString.HasKeys()) {
            //        url = url + "?";
            //        foreach (string item in queryString.AllKeys) {
            //            url = url + item + "=" + HttpUtility.UrlEncode(queryString[item]) + "&";
            //        }
            //        url = url.TrimEnd('&');
            //    }
            //}

			return url;
		}

		public static void SSLRedirect(MerchantTribeApplication app, Accounts.Store currentStore, SSLRedirectTo RedirectTo)
		{
            string CurrentUrl = app.CurrentRequestContext.RoutingContext.HttpContext.Request.RawUrl.ToLower(); //UrlRewriter.GetRewrittenUrlFromRequest(page.Request).ToLower();
            string StandardURL = app.StoreUrl(false, true).ToLowerInvariant();
            string SecureURL = app.StoreUrl(true, false).ToLowerInvariant();
			string sessionId = WebAppSettings.SessionId;
			string cartId = WebAppSettings.CartId;
			string currentSessionId = SessionManager.GetCurrentUserId(currentStore, app.CurrentRequestContext.RoutingContext.HttpContext.Request.Cookies);
			string currentCartId = SessionManager.GetCurrentCartID(currentStore);
			string url = BuildUrlForRedirect(CurrentUrl, StandardURL, SecureURL, RedirectTo, sessionId, cartId, currentSessionId, currentCartId, false
			);

			//if the urls match, then for some reason we aren't replacing anything
			//so if we redirect then we will go into a loop
			if (url != CurrentUrl) {
                app.CurrentRequestContext.RoutingContext.HttpContext.Response.Redirect(url);				
			}
		}

		public static void RemoveAllEncoding(ref string URL)
		{
			string NewURL = "";
			Int16 count = 0;
			while (URL != NewURL) {
				NewURL = URL;
				URL = HttpUtility.UrlDecode(NewURL);
				count += Convert.ToInt16(1);
			}
		}
	}
}
