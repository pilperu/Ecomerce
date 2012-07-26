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

        public void Process(List<ITemplateAction> actions, MerchantTribe.Commerce.MerchantTribeApplication app, ITagProvider tagProvider, ParsedTag tag, string innerContents)
        {
            StringBuilder sb = new StringBuilder();
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

            actions.Add(new Actions.PartialView("~/views/shared/_Logo.cshtml", model));            
        }
    }
}