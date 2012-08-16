using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;

namespace MerchantTribeStore
{
    public static class HtmlExtensions
    {
        private static bool FileExists(string fullFileName)
        {
            try
            {
                string path = System.Web.Hosting.HostingEnvironment.MapPath("~/" + fullFileName.TrimStart('/'));
                return (System.IO.File.Exists(path));
            }
            catch (Exception ex)
            {
                MerchantTribe.Commerce.EventLog.LogEvent(ex);
            }
            return false;
        }
        public static string FileIconUrl(this HtmlHelper helper, string fileName)
        {
            string baseUrl = "~/bvadmin/images/fileicons/";
            string basePath = "bvadmin/images/fileicons/";

            string extension = "";
            try
            {
                extension = System.IO.Path.GetExtension(fileName);
                extension = extension.TrimStart('.');
            }
            catch(Exception ex)
            {
                MerchantTribe.Commerce.EventLog.LogEvent(ex);
                return baseUrl + "default_32.png";
            }

            if (extension == string.Empty)
            {
                return baseUrl + "default_32.png";
            }
            if (FileExists(basePath + extension + "_32.png"))
            {
                return baseUrl + extension + "_32.png";
            }
            return baseUrl + "default_32.png";
        }
        public static string SafeRegisterScript(this HtmlHelper helper, string scriptName)
        {
            string keyName = "SafeRegisteredScripts";
            object list = helper.ViewData[keyName];
            if (list == null) list = new List<string>();

            if (list != null)
            {
                List<string> registered = (List<string>)list;
                if (registered != null)
                {
                    if (!registered.Contains(scriptName))
                    {
                        registered.Add(scriptName);
                    }
                    helper.ViewData[keyName] = registered;
                }
            }
            return string.Empty;
        }
    }
}