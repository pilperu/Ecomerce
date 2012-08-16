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

        public void Process(StringBuilder output, 
                            MerchantTribe.Commerce.MerchantTribeApplication app, 
                            dynamic viewBag,
                            ITagProvider tagProvider, 
                            ParsedTag tag, 
                            string innerContents)
        {
            output.Append("<" + _tagName);
            
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
                output.Append(" " + name + "=\"" + val + "\"");                
            }
            
            if (tag.IsSelfClosed)
            {
                output.Append("/>");
            }
            else
            {
                output.Append(">" + innerContents + "</" + _tagName + ">");
            }                        
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