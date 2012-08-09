using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MerchantTribeStore.code.TemplateEngine
{
    public class TagProvider: ITagProvider
    {
        Dictionary<string, ITagHandler> Handlers;

        public TagProvider()
        {
            Handlers = new Dictionary<string, ITagHandler>();
            InitializeTagHandlers();
        }

        public Dictionary<string, ITagHandler> GetHandlers()
        {
            return Handlers;
        }

        private void InitializeTagHandlers()
        {                        
            AddHandler(this.Handlers, new TagHandlers.Include());
            AddHandler(this.Handlers, new TagHandlers.ContentColumn());
            AddHandler(this.Handlers, new TagHandlers.PartialViewHandler("sys:adminpanel", "~/views/shared/_adminpanel.cshtml", null));
            AddHandler(this.Handlers, new TagHandlers.PartialViewHandler("sys:pagetitle", "~/views/shared/_PageTitle.cshtml", null));
            AddHandler(this.Handlers, new TagHandlers.PartialViewHandler("sys:metatags", "~/views/shared/_MetaTags.cshtml", null));
            AddHandler(this.Handlers, new TagHandlers.PartialViewHandler("sys:analyticstop", "~/views/shared/_AnalyticsTop.cshtml", null));
            AddHandler(this.Handlers, new TagHandlers.PartialViewHandler("sys:analyticsbottom", "~/views/shared/_AnalyticsBottom.cshtml", null));            
            AddHandler(this.Handlers, new TagHandlers.Css());
            AddHandler(this.Handlers, new TagHandlers.JavaScript());
            AddHandler(this.Handlers, new TagHandlers.Copyright());
            AddHandler(this.Handlers, new TagHandlers.TempMessages());            
            AddHandler(this.Handlers, new TagHandlers.Area());
            AddHandler(this.Handlers, new TagHandlers.PromoTag());            
            AddHandler(this.Handlers, new TagHandlers.Logo());
            AddHandler(this.Handlers, new TagHandlers.Link());
            AddHandler(this.Handlers, new TagHandlers.SearchForm());
            AddHandler(this.Handlers, new TagHandlers.MainMenu());
            AddHandler(this.Handlers, new TagHandlers.PageMenu());
            AddHandler(this.Handlers, new TagHandlers.BreadCrumbs());
            AddHandler(this.Handlers, new TagHandlers.ProfilerRender());

            // Legacy Tags that will eventually be removed
            AddHandler(this.Handlers, new TagHandlers.PartialViewHandler("sys:flexeditor", "~/views/flexpage/editorpanel.cshtml", null));
            AddHandler(this.Handlers, new TagHandlers.PartialViewHandler("sys:flexpopup", "~/views/flexpage/editorpopup.cshtml", null));

            // Fix up relative urls
            AddHandler(this.Handlers, new TagHandlers.UrlFixer("img", "src"));
            AddHandler(this.Handlers, new TagHandlers.UrlFixer("link", "href", "src"));
            AddHandler(this.Handlers, new TagHandlers.UrlFixer("script", "src"));
        }
        private void AddHandler(Dictionary<string, ITagHandler> handlers, ITagHandler handler)
        {
            handlers.Add(handler.TagName, handler);
        }
        
    }
}