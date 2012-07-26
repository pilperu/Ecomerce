using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribeStore.Tests
{
    public static class ContextHelper
    {
        public static MerchantTribe.Commerce.RequestContext GetFakeRequestContext(string fileName, string url, string querystring)
        {
            var result = new MerchantTribe.Commerce.RequestContext();

            var request = new System.Web.HttpRequest(fileName, url, querystring);
            var response = new System.Web.HttpResponse(new System.IO.StringWriter());
            System.Web.HttpContext httpContext = new System.Web.HttpContext(request, response);
            System.Web.HttpContextWrapper httpWrapper = new System.Web.HttpContextWrapper(httpContext);
            result.RoutingContext = new System.Web.Routing.RequestContext(httpWrapper,
                new System.Web.Routing.RouteData());

            return result;
        }
    }
}
