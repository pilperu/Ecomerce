using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;

namespace MerchantTribeStore.code.TemplateEngine.TagHandlers
{
    /// <summary>
    /// This class fixes up relative URLs from templates so that they point to
    /// the theme-{}/templates folder instead of site relative root
    /// </summary>
    public class UrlFixer : ITagHandler
    {
        string _tagName = string.Empty;
        string[] _attributesToFix;        

        public UrlFixer(string tagName, params string[] attributes)
        {
            this._tagName = tagName;
            this._attributesToFix = attributes;            
        }

        public string TagName
        {
            get { return _tagName; }
        }        

        public void Process(List<ITemplateAction> actions, MerchantTribe.Commerce.MerchantTribeApplication app, ITagProvider tagProvider, ParsedTag tag, string innerContents)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<" + _tagName);
            
            string pathToTemplate = app.ThemeManager().ThemeFileUrl("",app) + "templates/";
            if (pathToTemplate.StartsWith("http://"))
            {
                pathToTemplate = pathToTemplate.Replace("http://", "//");
            }

            foreach (var att in tag.Attributes)
            {
                string name = att.Key;
                string val = att.Value;
                if (_attributesToFix.Contains(att.Key.ToLowerInvariant()))
                {
                    val = FixUpValue(val, pathToTemplate);
                }                
                sb.Append(" " + name + "=\"" + val + "\"");                
            }
            
            if (tag.IsSelfClosed)
            {
                sb.Append("/>");
            }
            else
            {
                sb.Append(">" + innerContents + "</" + _tagName + ">");
            }
            
            actions.Add(new Actions.LiteralText(sb.ToString()));
        }

        private string FixUpValue(string original, string basePath)
        {
            string temp = original;            
            if (temp.StartsWith("http://") ||
                temp.StartsWith("https://")) return original;

            if (temp.StartsWith("./") || temp.StartsWith("//"))
            {
                temp = temp.Substring(2, temp.Length - 2);
            }
            if (temp.StartsWith("/")) temp = temp.TrimStart('/');

            return basePath + temp;
        }
    }
}