using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using System.Web.Mvc;

namespace MerchantTribeStore
{
    public class RouteEvaluator
    {
        RouteCollection routes;

        public RouteEvaluator(RouteCollection routes)
        {
            this.routes = routes;
        }

        public IList<RouteData> GetMatches(string virtualPath)
        {

            HttpContextBase context = new HttpContextWrapper(HttpContext.Current);
            
            return GetMatches(virtualPath, "GET", context);
        }

        public IList<RouteData> GetMatches(string virtualPath, string httpMethod, System.Web.HttpContextBase context)
        {
            List<RouteData> matchingRouteData = new List<RouteData>();

            foreach (var route in this.routes)
            {                                
                RouteData routeData = this.routes.GetRouteData(context);
                if (routeData != null)
                {
                    matchingRouteData.Add(routeData);
                }
            }
            return matchingRouteData;
        }
    }
}