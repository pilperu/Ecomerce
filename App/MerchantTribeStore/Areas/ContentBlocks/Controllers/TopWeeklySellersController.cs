using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MerchantTribe.Commerce.Catalog;
using MerchantTribe.Commerce.Content;
using MerchantTribe.Commerce.Utilities;
using MerchantTribeStore.Controllers.Shared;
using MerchantTribeStore.Models;
using MerchantTribeStore.Areas.ContentBlocks.Models;

namespace MerchantTribeStore.Areas.ContentBlocks.Controllers
{
    public class TopWeeklySellersController : BaseAppController
    {
        //
        // GET: /ContentBlocks/TopWeeklySellers/

        public ActionResult Index(ContentBlock b)
        {
            SideMenuViewModel model = new SideMenuViewModel();
            model.Title = "Top Weekly Sellers";

            DateTime _StartDate = DateTime.Now;
            DateTime _EndDate = DateTime.Now;
            System.DateTime c = DateTime.Now;
            CalculateDates(c, _StartDate, _EndDate);
            model.Items = LoadProducts(_StartDate, _EndDate);

            return View(model);
        }

        public void CalculateDates(DateTime currentTime, DateTime start, DateTime end)
        {
            start = FindStartOfWeek(currentTime);
            end = start.AddDays(7);
            end = end.AddMilliseconds(-1);
        }

        private DateTime FindStartOfWeek(DateTime currentDate)
        {
            DateTime result = currentDate;
            switch (currentDate.DayOfWeek)
            {
                case DayOfWeek.Sunday:
                    result = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, 0, 0, 0, 0);
                    break;
                case DayOfWeek.Monday:
                    result = currentDate.AddDays(-1);
                    break;
                case DayOfWeek.Tuesday:
                    result = currentDate.AddDays(-2);
                    break;
                case DayOfWeek.Wednesday:
                    result = currentDate.AddDays(-3);
                    break;
                case DayOfWeek.Thursday:
                    result = currentDate.AddDays(-4);
                    break;
                case DayOfWeek.Friday:
                    result = currentDate.AddDays(-5);
                    break;
                case DayOfWeek.Saturday:
                    result = currentDate.AddDays(-6);
                    break;
            }
            result = new DateTime(result.Year, result.Month, result.Day, 0, 0, 0, 0);
            return result;
        }

        private List<SideMenuItem> LoadProducts(DateTime start, DateTime end)
        {
            System.DateTime s = start;
            System.DateTime e = end;

            List<Product> t = MTApp.ReportingTopSellersByDate(s, e, 10);

            List<SideMenuItem> result = new List<SideMenuItem>();
            foreach (Product p in t)
            {
                SideMenuItem item = new SideMenuItem();
                item.Title = p.ProductName;
                item.Name = p.ProductName;
                item.Url = UrlRewriter.BuildUrlForProduct(p, MTApp.CurrentRequestContext.RoutingContext, string.Empty);
                result.Add(item);
            }
            return result;
        }
    }
}
