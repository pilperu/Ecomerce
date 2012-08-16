using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml.Linq;

namespace MerchantTribeStore
{
    public static class XDocumentExtensions
    {
        public static string ToStringWithDeclaration(this XDocument xdoc)
        {
            if (xdoc == null) return string.Empty;

            StringBuilder sb = new StringBuilder();
            using (TextWriter writer = new StringWriter(sb))
            {
                xdoc.Save(writer);
            }
            return sb.ToString();
        }
    }
}