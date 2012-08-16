using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Routing;
using StackExchange.Profiling;

namespace MerchantTribe.Commerce.Utilities
{
    ////TODO: Get rid of this slow pig!!, TagHandlers and templates take care of this now.
    //public class TagReplacer
    //{
    //    public static string ReplaceContentTags(string source, MerchantTribeApplication app, string itemCount, bool isSecureRequest)            
    //    {
    //        var profiler = MiniProfiler.Current;
    //        using (profiler.Step("Tag Replacer"))
    //        {
    //            Accounts.Store currentStore = app.CurrentStore;
    //            string currentUserId = app.CurrentCustomerId;

    //            string output = source;

    //            RouteCollection r = System.Web.Routing.RouteTable.Routes;
    //            //VirtualPathData homeLink = r.GetVirtualPath(requestContext.RoutingContext, "homepage", new RouteValueDictionary());

    //            output = output.Replace("{{homelink}}", app.StoreUrl(isSecureRequest, false));
    //            output = output.Replace("{{logo}}", HtmlRendering.Logo(app, isSecureRequest));
    //            output = output.Replace("{{logotext}}", HtmlRendering.LogoText(app));
    //            output = output.Replace("{{headermenu}}", HtmlRendering.HeaderMenu(app));
    //            output = output.Replace("{{cartlink}}", HtmlRendering.CartLink(app, itemCount));
    //            output = output.Replace("{{copyright}}", "<span class=\"copyright\">Copyright &copy;" + DateTime.Now.Year.ToString() + "</span>");
    //            output = output.Replace("{{headerlinks}}", HtmlRendering.HeaderLinks(app, currentUserId));
    //            output = output.Replace("{{searchform}}", HtmlRendering.SearchForm(app));
    //            output = output.Replace("{{assets}}", MerchantTribe.Commerce.Storage.DiskStorage.BaseUrlForStoreTheme(app, currentStore.Settings.ThemeId, isSecureRequest) + "assets/");
    //            output = output.Replace("{{img}}", MerchantTribe.Commerce.Storage.DiskStorage.StoreAssetUrl(app, string.Empty, isSecureRequest));
    //            output = output.Replace("{{storeassets}}", MerchantTribe.Commerce.Storage.DiskStorage.StoreAssetUrl(app, string.Empty, isSecureRequest));
    //            output = output.Replace("{{sitefiles}}", MerchantTribe.Commerce.Storage.DiskStorage.BaseUrlForSingleStore(app, isSecureRequest));

    //            output = output.Replace("{{storeaddress}}", app.ContactServices.Addresses.FindStoreContactAddress().ToHtmlString());

    //            return output;
    //        }
    //    }

    //}
}
