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
    public class CategoriesGrid : ITagHandler
    {
        public string TagName
        {
            get { return "sys:categoriesgrid"; }
        }


        public void Process(StringBuilder output,
                            MerchantTribe.Commerce.MerchantTribeApplication app,
                            dynamic viewBag,
                            ITagProvider tagProvider,
                            ParsedTag tag,
                            string innerContents)
        {
            var profiler = MiniProfiler.Current;
                        
            var model = new List<SingleCategoryViewModel>();

            int columns = tag.GetSafeAttributeAsInteger("columns");
            if (columns < 1) columns = 3;                
            
            string source = tag.GetSafeAttribute("source");
            switch (source.Trim().ToLowerInvariant())
            {
                case "manual":
                    string manualBvins = tag.GetSafeAttribute("categories");
                    List<string> bvins = manualBvins.Split(',').ToList();
                    model = PrepSubCategories(app.CatalogServices.Categories.FindManySnapshots(bvins), app);                    
                    break;
                default:
                    using (profiler.Step("Pull Products for Category"))
                    {
                        var cat = app.CurrentRequestContext.CurrentCategory;
                        

                        string categoryId = tag.GetSafeAttribute("categories");
                        using (profiler.Step("Checking for non-current category on grid"))
                        {
                            if (categoryId.Trim().Length < 1 || categoryId.Trim().ToLowerInvariant() == "current")
                            {
                                if (app.CurrentRequestContext.CurrentCategory != null)
                                {
                                    categoryId = app.CurrentRequestContext.CurrentCategory.Bvin;
                                    cat = app.CatalogServices.Categories.Find(categoryId);
                                }
                            }
                        }

                        model = PrepSubCategories(app.CatalogServices.Categories.FindVisibleChildren(cat.Bvin), app);                                            
                    }
                    break;
            }

            Render(output, app, viewBag, model, columns);
        }

        private void Render(StringBuilder sb,
                            MerchantTribeApplication app,
                            dynamic viewBag,
                            List<SingleCategoryViewModel> model,
                            int columns)
        {
            var profiler = MiniProfiler.Current;
            using (profiler.Step("Rendering Categories Grid..."))
            {                                
                if (model.Count > 0)
                {
                    sb.Append("<div id=\"categorygridsubtemplate\">");
                    foreach (var subcat in model)
                    {
                        sb.Append("<div");
                        if (subcat.IsLastItem == true)
                        {
                            sb.Append(" class=\"record lastrecord\"");
                        }
                        else if (subcat.IsFirstItem == true)
                        {
                            sb.Append(" class=\"record firstrecord\"");
                        }
                        else
                        {
                             sb.Append(" class=\"record\"");
                        }         
                        sb.Append(">");
                            sb.Append("<div class=\"recordimage\">");
                                sb.Append("<a id=\"recordimageanchor\" href=\"" + subcat.LinkUrl + "\">");
                                    sb.Append("<img id=\"recordimageimg\" src=\"" + subcat.IconUrl + "\" border=\"0\" alt=\"" + HttpUtility.HtmlEncode(subcat.AltText) + "\" />");
                                sb.Append("</a>");
                            sb.Append("</div>");
                            sb.Append("<div class=\"recordname\">");
                                sb.Append("<a id=\"recordnameanchor\" href=\"" + subcat.LinkUrl + "\">" + HttpUtility.HtmlEncode(subcat.Name) + "</a>");
                            sb.Append("</div>");
                        sb.Append("</div>");
                    }
                    sb.Append("<div class=\"clear\"></div>");
                    sb.Append("</div>");
                }    
            }
        }

        private List<SingleCategoryViewModel> PrepSubCategories(List<CategorySnapshot> snaps, MerchantTribeApplication app)
        {
            List<SingleCategoryViewModel> result = new List<SingleCategoryViewModel>();

            int columnCount = 1;

            foreach (CategorySnapshot snap in snaps)
            {
                SingleCategoryViewModel model = new SingleCategoryViewModel();

                model.LinkUrl = UrlRewriter.BuildUrlForCategory(snap,
                                                                app.CurrentRequestContext.RoutingContext);
                model.IconUrl = MerchantTribe.Commerce.Storage.DiskStorage.CategoryIconUrl(
                                                                app,
                                                                snap.Bvin,
                                                                snap.ImageUrl,
                                                                app.IsCurrentRequestSecure());
                model.AltText = snap.Name;
                model.Name = snap.Name;


                bool isLastInRow = false;
                bool isFirstInRow = false;
                if ((columnCount == 1))
                {
                    isFirstInRow = true;
                }

                if ((columnCount == 3))
                {
                    isLastInRow = true;
                    columnCount = 1;
                }
                else
                {
                    columnCount += 1;
                }

                model.IsFirstItem = isFirstInRow;
                model.IsLastItem = isLastInRow;

                result.Add(model);
            }

            return result;
        }
        

    }
}