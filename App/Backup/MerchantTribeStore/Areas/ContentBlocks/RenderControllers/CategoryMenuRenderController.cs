using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MerchantTribeStore.Areas.ContentBlocks.Models;
using System.Text;
using MerchantTribe.Commerce;
using MerchantTribe.Commerce.Content;
using MerchantTribe.Commerce.Catalog;
using MerchantTribe.Commerce.Utilities;

namespace MerchantTribeStore.Areas.ContentBlocks.RenderControllers
{
    public class CategoryMenuRenderController: BaseRenderController, IContentBlockRenderController
    {
        private MerchantTribeApplication MTApp;
        private dynamic ViewBag;
        public string Render(MerchantTribe.Commerce.MerchantTribeApplication app, dynamic viewBag, MerchantTribe.Commerce.Content.ContentBlock block)        
        {
            this.MTApp = app;
            this.ViewBag = viewBag;

            CategoryMenuViewModel model = new CategoryMenuViewModel();
            LoadMenu(model, block);
            return Render(model);
        }
        public string Render(CategoryMenuViewModel model)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("<div class=\"categorymenu\">");
            sb.Append("<div class=\"decoratedblock\">");
                if (!string.IsNullOrEmpty(model.Title))
                { 
                    sb.Append(model.Title); 
                }
            sb.Append(model.Contents);
            sb.Append("</div>");
            sb.Append("</div>");

            return sb.ToString();
        }

        private void LoadMenu(CategoryMenuViewModel model, ContentBlock b)
        {
            model.CurrentId = LocateCurrentCategory();
            if (b != null)
            {
                // Load Title
                string title = b.BaseSettings.GetSettingOrEmpty("Title");
                if (title.Trim().Length > 0)
                {
                    model.Title = "<h4>" + title + "</h4>";
                }


                model.ShowProductCounts = b.BaseSettings.GetBoolSetting("ShowProductCount");
                model.ShowCategoryCounts = b.BaseSettings.GetBoolSetting("ShowCategoryCount");
                model.sb.Append("<ul>");

                if (b.BaseSettings.GetBoolSetting("HomeLink") == true)
                {
                    AddHomeLink(model);
                }

                string mode = b.BaseSettings.GetSettingOrEmpty("CategoryMenuMode");
                switch (mode)
                {
                    case "0":
                        // Root Categories Only
                        LoadRoots(model);
                        break;
                    case "1":
                        // All Categories
                        LoadAllCategories(model, b.BaseSettings.GetIntegerSetting("MaximumDepth"));
                        break;
                    case "2":
                        // Peers, Children and Parents
                        LoadPeersAndChildren(model);
                        break;
                    case "3":
                        // Show root and expanded children
                        LoadRootPlusExpandedChildren(model);
                        break;
                    default:
                        // All Categories
                        LoadRoots(model);
                        break;
                }

                model.sb.Append("</ul>");
            }
        }
        private string LocateCurrentCategory()
        {
            string result = string.Empty;
            
            if (MTApp.CurrentRequestContext.RoutingContext.HttpContext.Request.QueryString["categoryId"] != null)
            {
                result = MTApp.CurrentRequestContext.RoutingContext.HttpContext.Request.QueryString["categoryId"];
            }
            else
            {
                result = MerchantTribe.Commerce.SessionManager.CategoryLastId;
            }

            // Always reset to zero if we can't find anything
            if (result == string.Empty)
            {
                result = "0";
            }

            return result;
        }
        private void LoadRoots(CategoryMenuViewModel model)
        {
            List<CategorySnapshot> cats = MTApp.CatalogServices.Categories.FindVisibleChildren("0");
            AddCategoryCollection(model, cats, cats, 1, 1);
        }
        private void AddHomeLink(CategoryMenuViewModel model)
        {
            model.sb.Append("<li>");
            model.sb.Append("<a href=\"" + MTApp.StoreUrl(false,false) + "\" title=\"Home\">Home</a>");
            model.sb.Append("</li>");
        }
        private void AddSingleLink(CategoryMenuViewModel model, CategorySnapshot c, List<CategorySnapshot> allCats)
        {
            if (c.Bvin == model.CurrentId)
            {
                model.sb.Append("<li class=\"current\">");
            }
            else
            {
                model.sb.Append("<li>");
            }

            string title = c.MetaTitle;
            string text = c.Name;

            int childCount = 0;
            if (model.ShowProductCounts)
            {
                childCount += (MTApp.CatalogServices.CategoriesXProducts.FindForCategory(c.Bvin, 1, int.MaxValue).Count);
            }

            if (model.ShowCategoryCounts)
            {
                childCount += Category.FindChildrenInList(allCats, c.Bvin, false).Count;
            }

            if (childCount > 0)
            {
                text += " (" + childCount.ToString() + ")";
            }

            string url = UrlRewriter.BuildUrlForCategory(c, MTApp.CurrentRequestContext.RoutingContext);
            bool openNew = false;

            if (c.SourceType == CategorySourceType.CustomLink)
            {
                openNew = c.CustomPageOpenInNewWindow;
            }

            model.sb.Append("<a href=\"" + url + "\" title=\"" + title + "\">" + text + "</a>");
        }
        private void LoadAllCategories(CategoryMenuViewModel model, int maxDepth)
        {
            List<CategorySnapshot> allCats = MTApp.CatalogServices.Categories.FindAll();
            List<CategorySnapshot> children = Category.FindChildrenInList(allCats, "0", false);
            AddCategoryCollection(model, allCats, children, 0, maxDepth);
        }
        private void AddCategoryCollection(CategoryMenuViewModel model, List<CategorySnapshot> allCats, List<CategorySnapshot> cats, int currentDepth, int maxDepth)
        {
            if (cats != null)
            {
                foreach (CategorySnapshot c in cats)
                {

                    if (c.Hidden == false)
                    {

                        AddSingleLink(model, c, allCats);

                        if ((maxDepth == 0) | (currentDepth + 1 < maxDepth))
                        {
                            List<CategorySnapshot> children = MTApp.CatalogServices.Categories.FindVisibleChildren(c.Bvin);
                            if (children != null)
                            {
                                if (children.Count > 0)
                                {
                                    model.sb.Append("<ul>" + System.Environment.NewLine);
                                    AddCategoryCollection(model, allCats, children, currentDepth + 1, maxDepth);
                                    model.sb.Append("</ul>" + System.Environment.NewLine);
                                }
                            }
                        }

                        model.sb.Append("</li>");
                    }
                }
            }
        }
        private void LoadRootPlusExpandedChildren(CategoryMenuViewModel model)
        {
            List<CategorySnapshot> allCats = MTApp.CatalogServices.Categories.FindAll();

            // Get Current Category
            CategorySnapshot currentCategory = Category.FindInList(allCats, model.CurrentId);

            if (currentCategory != null)
            {
                if (currentCategory.Bvin != string.Empty)
                {
                    model.CurrentId = currentCategory.Bvin;
                }

                // Find the trail from this category back to the root of the site
                List<CategorySnapshot> trail = new List<CategorySnapshot>();
                BuildParentTrail(allCats, model.CurrentId, ref trail);
                if (trail == null)
                {
                    trail = new List<CategorySnapshot>();
                }

                if (trail.Count < 1)
                {
                    // Load Roots Only
                    LoadRoots(model);
                }
                else
                {

                    string StartingRootCategoryId = currentCategory.Bvin;
                    StartingRootCategoryId = trail[trail.Count - 1].Bvin;


                    List<CategorySnapshot> roots = Category.FindChildrenInList(allCats, "0", false);
                    if (roots != null)
                    {
                        model.sb.Append("<ul>" + System.Environment.NewLine);

                        foreach (CategorySnapshot c in roots)
                        {
                            if (IsInTrail(c.Bvin, trail))
                            {
                                AddSingleLink(model, c, allCats);
                                List<CategorySnapshot> children = new List<CategorySnapshot>();
                                children = Category.FindChildrenInList(allCats, StartingRootCategoryId, false);
                                if (children != null)
                                {
                                    model.sb.Append("<ul>" + System.Environment.NewLine);
                                    ExpandInTrail(model, allCats, children, trail);
                                    model.sb.Append("</ul>" + System.Environment.NewLine);
                                }
                                model.sb.Append("</li>");

                                break;
                            }
                        }

                        model.sb.Append("</ul>" + System.Environment.NewLine);
                    }

                }
            }
            else
            {
                model.sb.Append("Invalid Category Id. Contact Administrator");
            }

        }
        private bool IsInTrail(string testBvin, List<CategorySnapshot> trail)
        {
            bool result = false;

            if (trail != null)
            {
                foreach (CategorySnapshot c in trail)
                {
                    if (c.Bvin == testBvin)
                    {
                        result = true;
                        break;
                    }
                }
            }

            return result;
        }
        private void ExpandInTrail(CategoryMenuViewModel model, List<CategorySnapshot> allCats, List<CategorySnapshot> cats, List<CategorySnapshot> trail)
        {
            if (cats != null)
            {
                foreach (CategorySnapshot c in cats)
                {

                    if (c.Hidden == false)
                    {


                        AddSingleLink(model, c, allCats);

                        if (IsInTrail(c.Bvin, trail))
                        {
                            List<CategorySnapshot> children = Category.FindChildrenInList(allCats, c.Bvin, false);
                            if (children != null)
                            {
                                if (children.Count > 0)
                                {
                                    model.sb.Append("<ul>" + System.Environment.NewLine);
                                    ExpandInTrail(model, allCats, children, trail);
                                    model.sb.Append("</ul>" + System.Environment.NewLine);
                                }
                            }
                        }

                        model.sb.Append("</li>");
                    }
                }
            }
        }
        private void BuildParentTrail(List<CategorySnapshot> allCats, string currentId, ref List<CategorySnapshot> trail)
        {
            if (currentId == "0" || currentId == string.Empty)
            {
                return;
            }

            CategorySnapshot current = Category.FindInList(allCats, currentId);

            if (current != null)
            {

                trail.Add(current);
                if (current.ParentId == "0")
                {
                    return;
                }
                if (current.ParentId != null)
                {
                    if (current.ParentId != string.Empty)
                    {
                        BuildParentTrail(allCats, current.ParentId, ref trail);
                    }
                }
            }

        }
        private void LoadPeersAndChildren(CategoryMenuViewModel model)
        {
            List<CategorySnapshot> allCats = MTApp.CatalogServices.Categories.FindAll();

            // Get Current Category
            CategorySnapshot currentCategory = Category.FindInList(allCats, model.CurrentId);

            // Trick system into accepting root category of zero which never exists in database
            if (model.CurrentId == "0")
            {
                currentCategory = new CategorySnapshot();
                currentCategory.Bvin = "0";
            }



            if (currentCategory != null)
            {
                if (currentCategory.Bvin != string.Empty)
                {
                    model.CurrentId = currentCategory.Bvin;
                }

                // Find the trail from this category back to the root of the site
                List<CategorySnapshot> trail = new List<CategorySnapshot>();
                BuildParentTrail(allCats, model.CurrentId, ref trail);
                if (trail == null)
                {
                    trail = new List<CategorySnapshot>();
                }

                if (trail.Count < 1)
                {
                    // Load Roots Only
                    LoadRoots(model);
                }
                else
                {

                    CategoryPeerSet neighbors = GetPeerSet(allCats, currentCategory);

                    if (trail.Count == 1)
                    {
                        // special case where we want only peers and children
                        RenderPeersChildren(model, neighbors, currentCategory, allCats);
                    }
                    else
                    {
                        if (trail.Count >= 3)
                        {
                            if (neighbors.Children.Count < 1)
                            {
                                // Special case where we are at the end of the tree and have
                                // no children. Reset neighbors to parent's bvin

                                CategorySnapshot parent = Category.FindInList(allCats, currentCategory.ParentId);
                                if (parent == null)
                                {
                                    parent = new CategorySnapshot();
                                }

                                neighbors = GetPeerSet(allCats, parent);
                                RenderParentsPeersChildren(model, neighbors, trail[1], allCats);
                            }
                            else
                            {
                                RenderParentsPeersChildren(model, neighbors, currentCategory, allCats);
                            }
                        }
                        else
                        {
                            // normal load of peers
                            RenderParentsPeersChildren(model, neighbors, currentCategory, allCats);
                        }
                    }
                }
            }

            else
            {
                model.sb.Append("Invalid Category Id. Contact Administrator");
            }

        }
        private CategoryPeerSet GetPeerSet(List<CategorySnapshot> allCats, CategorySnapshot cat)
        {
            CategoryPeerSet result = new CategoryPeerSet();

            CategorySnapshot parent = Category.FindInList(allCats, cat.ParentId);
            if (parent != null)
            {
                result.Parents = Category.FindChildrenInList(allCats, parent.ParentId, false);
            }
            result.Peers = Category.FindChildrenInList(allCats, cat.ParentId, false);
            result.Children = Category.FindChildrenInList(allCats, cat.Bvin, false);

            return result;
        }
        private void RenderPeersChildren(CategoryMenuViewModel model, CategoryPeerSet neighbors, CategorySnapshot currentCategory, List<CategorySnapshot> allCats)
        {
            // No Parents, start with peers
            foreach (CategorySnapshot peer in neighbors.Peers)
            {
                if (!peer.Hidden)
                {
                    AddSingleLink(model, peer, allCats);
                    if (peer.Bvin == currentCategory.Bvin)
                    {

                        // Load Children
                        if (neighbors.Children.Count > 0)
                        {
                            bool initialized = false;
                            foreach (CategorySnapshot child in neighbors.Children)
                            {
                                if (!child.Hidden)
                                {
                                    if (!initialized)
                                    {
                                        model.sb.Append("<ul>" + System.Environment.NewLine);
                                        initialized = true;
                                    }

                                    AddSingleLink(model, child, allCats);
                                    model.sb.Append("</li>" + System.Environment.NewLine);
                                }
                            }
                            if (initialized)
                            {
                                model.sb.Append("</ul>" + System.Environment.NewLine);
                            }
                        }

                    }
                    model.sb.Append("</li>" + System.Environment.NewLine);
                }
            }
        }
        private void RenderParentsPeersChildren(CategoryMenuViewModel model, CategoryPeerSet neighbors, CategorySnapshot currentCategory, List<CategorySnapshot> allCats)
        {
            if (neighbors.Parents.Count < 1)
            {
                RenderPeersChildren(model, neighbors, currentCategory, allCats);
            }
            else
            {

                // Add Parents
                foreach (CategorySnapshot parent in neighbors.Parents)
                {
                    if (!parent.Hidden)
                    {
                        AddSingleLink(model, parent, allCats);

                        // Add Peers
                        if (parent.Bvin == currentCategory.ParentId)
                        {

                            bool peerInitialized = false;

                            foreach (CategorySnapshot peer in neighbors.Peers)
                            {
                                if (!peer.Hidden)
                                {
                                    if (!peerInitialized)
                                    {
                                        model.sb.Append("<ul>");
                                        peerInitialized = true;
                                    }
                                    AddSingleLink(model, peer, allCats);
                                    if (peer.Bvin == currentCategory.Bvin)
                                    {

                                        // Load Children
                                        if (neighbors.Children.Count > 0)
                                        {
                                            bool childInitialized = false;
                                            foreach (CategorySnapshot child in neighbors.Children)
                                            {
                                                if (!child.Hidden)
                                                {
                                                    if (!childInitialized)
                                                    {
                                                        model.sb.Append("<ul>" + System.Environment.NewLine);
                                                        childInitialized = true;
                                                    }
                                                    AddSingleLink(model, child, allCats);
                                                    model.sb.Append("</li>" + System.Environment.NewLine);
                                                }
                                            }
                                            if (childInitialized)
                                            {
                                                model.sb.Append("</ul>" + System.Environment.NewLine);
                                            }
                                        }


                                    }
                                    model.sb.Append("</li>" + System.Environment.NewLine);
                                }
                            }

                            if (peerInitialized)
                            {
                                model.sb.Append("</ul>" + System.Environment.NewLine);
                            }

                        }

                    }
                    model.sb.Append("</li>" + System.Environment.NewLine);
                }
            }
        }
    }
}