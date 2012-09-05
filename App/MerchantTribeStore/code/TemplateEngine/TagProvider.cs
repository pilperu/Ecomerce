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
            AddHandler(this.Handlers, new TagHandlers.BodyOpen());
            AddHandler(this.Handlers, new TagHandlers.ContentColumn());
            AddHandler(this.Handlers, new TagHandlers.AdminPanel()); 
            AddHandler(this.Handlers, new TagHandlers.PageTitle());
            AddHandler(this.Handlers, new TagHandlers.MetaTags());
            AddHandler(this.Handlers, new TagHandlers.AnalyticsTop());
            AddHandler(this.Handlers, new TagHandlers.AnalyticsBottom());
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
            AddHandler(this.Handlers, new TagHandlers.SingleProduct());
            AddHandler(this.Handlers, new TagHandlers.CategoryBanner());
            AddHandler(this.Handlers, new TagHandlers.CategoryTitle());
            AddHandler(this.Handlers, new TagHandlers.CategoryDescription());
            AddHandler(this.Handlers, new TagHandlers.ProductsGrid());
            AddHandler(this.Handlers, new TagHandlers.ProductsList());
            AddHandler(this.Handlers, new TagHandlers.ProductsDetailedList());
            AddHandler(this.Handlers, new TagHandlers.ProductsQuantityList());
            AddHandler(this.Handlers, new TagHandlers.ProductsOrderList());
            AddHandler(this.Handlers, new TagHandlers.Pager());
            AddHandler(this.Handlers, new TagHandlers.CategoriesGrid());
            AddHandler(this.Handlers, new TagHandlers.MiniPager());

            // Legacy Tags that will eventually be removed
            AddHandler(this.Handlers, new TagHandlers.FlexEditorPanel());
            AddHandler(this.Handlers, new TagHandlers.FlexEditorPopup()); 

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