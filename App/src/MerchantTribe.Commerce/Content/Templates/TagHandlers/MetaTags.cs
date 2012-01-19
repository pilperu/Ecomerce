using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Commerce.Content.Templates.TagHandlers
{
    public class MetaTags : ITagHandler
    {
        public string TagName
        {
            get { return "sys:metatags"; }
        }

        public string Process(MerchantTribeApplication app, Dictionary<string, ITagHandler> handlers, ParsedTag tag, string contents)
        {
            string keywords = string.Empty;
            string description = string.Empty;

            // Load from Category or Product
            if (app.CurrentRequestContext.CurrentCategory != null)
            {
                keywords = app.CurrentRequestContext.CurrentCategory.MetaKeywords;
                description = app.CurrentRequestContext.CurrentCategory.MetaDescription;
            }
            else if (app.CurrentRequestContext.CurrentProduct != null)
            {
                keywords = app.CurrentRequestContext.CurrentProduct.MetaKeywords;
                description = app.CurrentRequestContext.CurrentProduct.MetaDescription;
            }

            //Load from Context if Empty
            if (keywords == string.Empty)
            {
                keywords = app.CurrentRequestContext.MetaKeywords;
            }
            if (description == string.Empty)
            {
                description = app.CurrentRequestContext.MetaDescription;
            }

            // Render Tags
            StringBuilder sb = new StringBuilder();

            if (keywords.Trim().Length > 0)
            {
                sb.Append("<meta name=\"keywords\" content=\"" + keywords.Replace('"', ' ') + "\" />");
            }
            if (description.Trim().Length > 0)
            {
                sb.Append("<meta name=\"description\" content=\"" + description.Replace('"', ' ') + "\" />");
            }

            string moreMetaTags = app.CurrentRequestContext.CurrentStore.Settings.Analytics.AdditionalMetaTags ?? string.Empty;                
            if (moreMetaTags.Trim().Length > 0)
            {
                sb.Append(moreMetaTags);
            }

            return sb.ToString();
        }

        public MetaTags()
        {
        }
    }
}
