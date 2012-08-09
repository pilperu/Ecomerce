using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MerchantTribeStore.Models;
using System.Text;
using MerchantTribe.Commerce;
using MerchantTribe.Commerce.Catalog;
using MerchantTribe.Commerce.Utilities;

namespace MerchantTribeStore.code.TemplateEngine.TagHandlers
{
    public class BreadCrumbs : ITagHandler
    {

        public string TagName
        {
            get { return "sys:breadcrumbs"; }
        }

        public void Process(List<ITemplateAction> actions, 
                            MerchantTribe.Commerce.MerchantTribeApplication app, 
                            dynamic viewBag,
                            ITagProvider tagProvider, 
                            ParsedTag tag, 
                            string innerContents)
        {
            List<BreadCrumbItem> extras = new List<BreadCrumbItem>();

            string[] parts = innerContents.Split(',');
            if (parts.Length > 0)
            {
                foreach (string p in parts)
                {
                    string[] linkParts = p.Split('|');
                    if (linkParts.Length > 0)
                    {
                        string name = linkParts[0].Trim();
                        if (name.Length > 0)
                        {
                            BreadCrumbItem item = new BreadCrumbItem();
                            item.Name = linkParts[0].Trim();
                            item.Link = "";
                            if (linkParts.Length > 1)
                            {
                                item.Link = linkParts[1].Trim();
                            }
                            extras.Add(item);
                        }
                    }
                }
            }

            string mode = tag.GetSafeAttribute("mode");
            if (mode == "manual")
            {
                actions.Add(new Actions.LiteralText(RenderManual(app, extras)));
            }
            else
            {
                if (app.CurrentRequestContext.CurrentProduct != null)
                {
                    actions.Add(new Actions.LiteralText(RenderProduct(app, extras)));
                }
                else if (app.CurrentRequestContext.CurrentCategory != null)
                {
                    actions.Add(new Actions.LiteralText(RenderCategory(app, extras)));                    
                }
                else
                {
                    actions.Add(new Actions.LiteralText(RenderManual(app, extras)));
                }
            }
        }

        public string Render(MerchantTribeApplication app, BreadCrumbViewModel model)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("<div class=\"breadcrumbs\">");
            sb.Append("<div class=\"links\">");
            if (model.HideHomeLink == false)
            {
                sb.Append("<a href=\"" + app.StoreUrl(false, false) + "\">Home</a>" + model.Spacer);
            }           
            while (model.Items.Count > 0)
            {
                var item = model.Items.Dequeue();
                if (item.Link == string.Empty)
                {
                    sb.Append("<span class=\"current\">" + HttpUtility.HtmlEncode(item.Name) + "</span>");
                }
                else
                {
                    sb.Append("<a href=\"" + item.Link + "\" title=\"" + HttpUtility.HtmlEncode(item.Title) + "\">" + HttpUtility.HtmlEncode(item.Name) + "</a>");
                }
                        
                if (model.Items.Count > 0 && model.Items.Peek() != null)
                {
                    sb.Append(model.Spacer);
                }
            }
            sb.Append("</div>");
            sb.Append("</div>");

            return sb.ToString();
        }

        public string RenderManual(MerchantTribeApplication app, List<BreadCrumbItem> extras)
        {
            BreadCrumbViewModel model = new BreadCrumbViewModel();
            model.HomeName = MerchantTribe.Commerce.Content.SiteTerms.GetTerm(MerchantTribe.Commerce.Content.SiteTermIds.Home);
            if (extras != null)
            {
                foreach (BreadCrumbItem item in extras)
                {
                    model.Items.Enqueue(item);
                }
            }
            return Render(app, model);
        }

        public string RenderProduct(MerchantTribeApplication app, List<BreadCrumbItem> extras)
        {
            return RenderProduct(app, extras, app.CurrentRequestContext.CurrentProduct);
        }
        public string RenderProduct(MerchantTribeApplication app, List<BreadCrumbItem> extras, Product product)
        {            
            BreadCrumbViewModel model = new BreadCrumbViewModel();
            model.HomeName = MerchantTribe.Commerce.Content.SiteTerms.GetTerm(MerchantTribe.Commerce.Content.SiteTermIds.Home);

            LoadTrailForProduct(model, product, app);

            if (extras != null)
            {
                foreach (BreadCrumbItem item in extras)
                {
                    model.Items.Enqueue(item);
                }
            }
            return Render(app, model);
        }
        private void LoadTrailForProduct(BreadCrumbViewModel model, Product p, MerchantTribeApplication app)
        {
            if (p == null) return;
            CategorySnapshot currentCategory = null;
            List<CategorySnapshot> cats = app.CatalogServices.FindCategoriesForProduct(p.Bvin);
            if ((cats.Count > 0))
            {
                currentCategory = cats[0];
            }
            LoadTrailForCategory(model, currentCategory, true, app);
            model.Items.Enqueue(new BreadCrumbItem() { Name = p.ProductName });
        }

        public string RenderCategory(MerchantTribeApplication app, List<BreadCrumbItem> extras)
        {
            return RenderCategory(app, extras, app.CurrentRequestContext.CurrentCategory);
        }
        public string RenderCategory(MerchantTribeApplication app, List<BreadCrumbItem> extras, Category cat)
        {            
            CategorySnapshot snap = new CategorySnapshot(cat);

            BreadCrumbViewModel model = new BreadCrumbViewModel();
            model.HomeName = MerchantTribe.Commerce.Content.SiteTerms.GetTerm(MerchantTribe.Commerce.Content.SiteTermIds.Home);

            LoadTrailForCategory(model, snap, false, app);

            if (extras != null && extras.Count > 0)
            {
                foreach (BreadCrumbItem item in extras)
                {
                    model.Items.Enqueue(item);
                }
            }

            return Render(app, model);
        }
        private void LoadTrailForCategory(BreadCrumbViewModel model, CategorySnapshot cat, bool linkAll, MerchantTribeApplication app)
        {
            if (cat == null) return;
            if (cat.Hidden) return;

            List<CategorySnapshot> allCats = app.CatalogServices.Categories.FindAllPaged(1, int.MaxValue);


            List<CategorySnapshot> trail = new List<CategorySnapshot>();
            trail = Category.BuildTrailToRoot(cat.Bvin, app.CurrentRequestContext);

            if (trail == null) return;

            // Walk list backwards
            for (int j = trail.Count - 1; j >= 0; j += -1)
            {
                if (j != 0 || linkAll == true)
                {
                    model.Items.Enqueue(AddCategoryLink(trail[j], app));
                }
                else
                {
                    model.Items.Enqueue(new BreadCrumbItem() { Name = trail[j].Name });
                }
            }
        }
        private BreadCrumbItem AddCategoryLink(CategorySnapshot c, MerchantTribeApplication app)
        {
            BreadCrumbItem result = new BreadCrumbItem();
            result.Name = c.Name;
            result.Title = c.MetaTitle;
            result.Link = UrlRewriter.BuildUrlForCategory(c,
                app.CurrentRequestContext.RoutingContext);
            return result;
        }
        
    }
}