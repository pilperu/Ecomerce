using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Dynamic;
using System.Text;
using MerchantTribe.Commerce;
using MerchantTribe.Commerce.Catalog;
using MerchantTribe.Commerce.Utilities;
using MerchantTribeStore.Models;
using StackExchange.Profiling;

namespace MerchantTribeStore.code.TemplateEngine.TagHandlers
{
    public class ProductsGrid : BaseProductsDisplayHandler
    {
        public override string TagName
        {
            get { return "sys:productsgrid"; }
        }

        protected override void Render(StringBuilder sb, 
                            MerchantTribeApplication app, 
                            dynamic viewBag, 
                            ProductListViewModel model, 
                            bool showPagers, int columns)
        {
            var profiler = MiniProfiler.Current;
            using (profiler.Step("Rendering Grid..."))
            {
                var preppedItems = PrepProducts(model.Items, columns, app);
                var pagerRenderer = new code.TemplateEngine.TagHandlers.Pager();
                var productRenderer = new code.TemplateEngine.TagHandlers.SingleProduct();

                if (showPagers == true)
                {
                    pagerRenderer.Render(sb, model.PagerData);                    
                }
                foreach (var item in preppedItems)
                {
                    productRenderer.RenderModel(sb, item, app);
                }
                if (showPagers == true)
                {
                    pagerRenderer.Render(sb, model.PagerData);                    
                }
            }
        }

    }
}