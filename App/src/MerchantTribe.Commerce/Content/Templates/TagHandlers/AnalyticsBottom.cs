using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Commerce.Content.Templates.TagHandlers
{
    public class AnalyticsBottom : ITagHandler
    {
        public string TagName
        {
            get { return "sys:analyticsbottom"; }
        }

        public string Process(MerchantTribeApplication app, Dictionary<string, ITagHandler> handlers, ParsedTag tag, string contents)
        {
            string result = string.Empty;

            Orders.Order o = new Orders.Order();
            if (app.CurrentRequestContext.CurrentReceiptOrder != null)
            {
                o = app.CurrentRequestContext.CurrentReceiptOrder;

                // Adwords Tracker at bottom if needed
                if (app.CurrentStore.Settings.Analytics.UseGoogleAdWords)
                {
                    result = MerchantTribe.Commerce.Metrics.GoogleAnalytics.RenderGoogleAdwordTracker(
                                                            o.TotalGrand,
                                                            app.CurrentStore.Settings.Analytics.GoogleAdWordsId,
                                                            app.CurrentStore.Settings.Analytics.GoogleAdWordsLabel,
                                                            app.CurrentStore.Settings.Analytics.GoogleAdWordsBgColor,
                                                            app.CurrentRequestContext.RoutingContext.HttpContext.Request.IsSecureConnection);
                }

                // Add Yahoo Tracker to Bottom if Needed
                if (app.CurrentStore.Settings.Analytics.UseYahooTracker)
                {
                    result += MerchantTribe.Commerce.Metrics.YahooAnalytics.RenderYahooTracker(
                        o, app.CurrentStore.Settings.Analytics.YahooAccountId);
                }
            }

            string moreTags = app.CurrentStore.Settings.Analytics.BottomAnalytics ?? string.Empty;
            if (moreTags.Trim().Length > 0)
            {
                result += moreTags;
            }

            return result;
        }
    }
}
