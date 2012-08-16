using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using MerchantTribe.Commerce;
using MerchantTribe.Commerce.Catalog;
using MerchantTribe.Commerce.Content;
using MerchantTribeStore.Areas.ContentBlocks.Models;
using MerchantTribe.Commerce.Utilities;

namespace MerchantTribeStore.Areas.ContentBlocks.RenderControllers
{
    public class TopWeeklySellersRenderController : BaseRenderController, IContentBlockRenderController
    {
        public string Render(MerchantTribe.Commerce.MerchantTribeApplication app, dynamic viewBag, MerchantTribe.Commerce.Content.ContentBlock block)
        {
            SideMenuViewModel model = new SideMenuViewModel();
            model.Title = "Top Weekly Sellers";

            DateTime _StartDate = DateTime.Now;
            DateTime _EndDate = DateTime.Now;
            System.DateTime c = DateTime.Now;
            CalculateDates(c, _StartDate, _EndDate);
            model.Items = LoadProducts(app, _StartDate, _EndDate);

            model.Title = "Top Sellers";

            return RenderModel(model);
        }
        public string RenderModel(SideMenuViewModel model)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("<div class=\"sidemenu topweeklysellers\">");
            sb.Append("<div class=\"decoratedblock\">");
            sb.Append("<h3>" + HttpUtility.HtmlEncode(model.Title) + "</h3>");
            sb.Append("<ol>");
            foreach (var item in model.Items)
            {
                sb.Append("<li><a href=\"" + item.Url + "\" title=\"" + HttpUtility.HtmlEncode(item.Title) + "\">" + HttpUtility.HtmlEncode(item.Name) + "</a></li>");
            }
            sb.Append("</ol>");
            sb.Append("</div>");
            sb.Append("</div>");

            return sb.ToString();
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

        private List<SideMenuItem> LoadProducts(MerchantTribeApplication app, DateTime start, DateTime end)
        {
            System.DateTime s = start;
            System.DateTime e = end;

            List<Product> t = app.ReportingTopSellersByDate(s, e, 10);

            List<SideMenuItem> result = new List<SideMenuItem>();
            foreach (Product p in t)
            {
                SideMenuItem item = new SideMenuItem();
                item.Title = p.ProductName;
                item.Name = p.ProductName;
                item.Url = UrlRewriter.BuildUrlForProduct(p, app.CurrentRequestContext.RoutingContext, string.Empty);
                result.Add(item);
            }
            return result;
        }
    }
}