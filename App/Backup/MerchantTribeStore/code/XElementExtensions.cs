using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace MerchantTribeStore
{
    public static class XElementExtensions
    {
        public static string TryGetElementValue(this XElement parent, string name, string defaultValue)
        {
            var child = parent.Element(name);
            if (child != null)
            {
                return child.Value;
            }
            else
            {
                return defaultValue;
            }
        }

        public static int TryGetElementValueAsInt(this XElement parent, string name, int defaultValue)
        {
            var child = parent.Element(name);
            if (child != null)
            {
                string temp = child.Value;
                int tempResult = 0;
                if (int.TryParse(temp, out tempResult)) return tempResult;
            }
            return defaultValue;
        }

        public static long TryGetElementValueAsLong(this XElement parent, string name, long defaultValue)
        {
            var child = parent.Element(name);
            if (child != null)
            {
                string temp = child.Value;
                long tempResult = 0;
                if (long.TryParse(temp, out tempResult)) return tempResult;
            }
            return defaultValue;
        }
    }
}