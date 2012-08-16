using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;

namespace MerchantTribeStore.code.TemplateEngine.TagHandlers
{
    public class Logo : ITagHandler
    {
        public class LogoViewModel
        {
            public bool UseTextOnly { get; set; }
            public string LogoText { get; set; }
            public string LogoImageUrl { get; set; }
            public string LinkUrl { get; set; }
            public string StoreName { get; set; }
            public string InnerContent { get; set; }
        }

        public string TagName
        {
            get { return "sys:logo"; }
        }

        public void Process(StringBuilder output, 
                            MerchantTribe.Commerce.MerchantTribeApplication app, 
                            dynamic viewBag,
                            ITagProvider tagProvider, 
                            ParsedTag tag, 
                            string innerContents)
        {            
            bool isSecureRequest = app.IsCurrentRequestSecure();            
            bool textOnly = !app.CurrentStore.Settings.UseLogoImage;
            string textOnlyTag = tag.GetSafeAttribute("textonly").Trim().ToLowerInvariant();
            if (textOnlyTag == "1" || textOnlyTag == "y" || textOnlyTag == "yes" || textOnlyTag == "true") textOnly = true;

            string storeRootUrl = app.CurrentStore.RootUrl();
            string storeName = app.CurrentStore.Settings.FriendlyName;
            string logoImage = app.CurrentStore.Settings.LogoImageFullUrl(app, isSecureRequest);
            string logoText = app.CurrentStore.Settings.LogoText;

            LogoViewModel model = new LogoViewModel();
            model.InnerContent = innerContents.Trim();
            model.LinkUrl = storeRootUrl;
            model.LogoImageUrl = logoImage;
            model.LogoText = logoText;
            model.StoreName = storeName;
            model.UseTextOnly = textOnly;

            Render(output, model);            
        }

        private void Render(StringBuilder sb, LogoViewModel model)
        {            
            sb.Append("<a href=\"" + model.LinkUrl + "\" title=\"" + HttpUtility.HtmlEncode(model.StoreName) + "\" class=\"logo\">");
            if (model.InnerContent.Trim().Length > 0)
            {
                sb.Append(model.InnerContent);
            }
            else if (model.UseTextOnly == false && model.LogoImageUrl.Length > 0)
            {
                sb.Append("<img src=\"" + model.LogoImageUrl + "\" alt=\"" + HttpUtility.HtmlEncode(model.StoreName) + "\" />");
            }
            else
            {
                sb.Append(HttpUtility.HtmlEncode(model.LogoText));
            }
            sb.Append("</a>");         
        }
    }
}