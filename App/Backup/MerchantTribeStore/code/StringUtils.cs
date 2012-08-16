
using System;
using System.Web;

namespace MerchantTribeStore
{
    public class StringUtils
    {
        public static string SessionInfo()
        {
            try
            {
                string sessionInfos;

                string goingto = ToStr(HttpContext.Current.Request.Url);
                sessionInfos = "<br>To:" + goingto + "\r\n<br>From:" + ToStr(HttpContext.Current.Request.UrlReferrer);
                sessionInfos = sessionInfos + "<br>User:" + HttpContext.Current.Request.UserHostName + "(" + HttpContext.Current.Request.UserHostAddress + ")";
                sessionInfos = sessionInfos + "<br>Agent:" + HttpContext.Current.Request.UserAgent;
                return sessionInfos;

            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        public static string ToStr(object value)
        {
            if (value == null)
            {
                return string.Empty;
            }
            else
            {
                return value.ToString();
            }
        }

    }
}