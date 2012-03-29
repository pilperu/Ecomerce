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