using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MerchantTribeStore.code.TemplateEngine
{
    public class ParsedTag
    {
        public string TagName { get; set; }
        public Dictionary<string, string> Attributes { get; set; }
        public bool IsSelfClosed { get; set; }

        public ParsedTag()
        {
            TagName = string.Empty;
            Attributes = new Dictionary<string, string>();
            IsSelfClosed = false;
        }

        public string GetSafeAttribute(string name)
        {
            string result = string.Empty;
            if (Attributes != null)
            {
                if (Attributes.ContainsKey(name))
                {
                    result = Attributes[name];
                }
            }
            return result;
        }
        public int GetSafeAttributeAsInteger(string name)
        {
            int result;
            string val = GetSafeAttribute(name);
            int.TryParse(val, out result);
            return result;
        }
        public decimal GetSafeAttributeAsDecimal(string name)
        {
            decimal result;
            string val = GetSafeAttribute(name);
            decimal.TryParse(val, out result);
            return result;
        }
        public bool GetSafeAttributeAsBoolean(string name)
        {            
            string val = GetSafeAttribute(name);
            string clean = val.Trim().ToLowerInvariant();
            if (clean == "1" ||
                clean == "true" ||
                clean == "yes" ||
                clean == "y") return true;
            return false;
        }
        public void SetSafeAttribute(string name, string value)
        {
            if (Attributes != null)
            {
                if (Attributes.ContainsKey(name))
                {
                    Attributes[name] = value;
                }
                else
                {
                    Attributes.Add(name, value);
                }
            }
        }
    }
}