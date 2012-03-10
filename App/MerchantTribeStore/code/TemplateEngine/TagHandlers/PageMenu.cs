using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using MerchantTribe.Commerce;
using MerchantTribe.Commerce.Catalog;
using MerchantTribe.Commerce.Utilities;

namespace MerchantTribeStore.code.TemplateEngine.TagHandlers
{
    public class PageMenu : ITagHandler
    {

        public string TagName
        {
            get { return "sys:pagemenu"; }
        }

        protected System.Web.Mvc.UrlHelper Url = null;
        protected MerchantTribeApplication App = null;
        protected Category CurrentCategory = new Category();
        protected bool ShowProductCount = false;
        protected bool ShowCategoryCount = false;

        public void Process(List<ITemplateAction> actions, MerchantTribe.Commerce.MerchantTribeApplication app, ITagProvider tagProvider, ParsedTag tag, string innerContents)
        {
            this.App = app;
            this.Url = new System.Web.Mvc.UrlHelper(app.CurrentRequestContext.RoutingContext);
            CurrentCategory = app.CurrentRequestContext.CurrentCategory;
            if (CurrentCategory == null)
            {
                CurrentCategory = new Category();
                CurrentCategory.Bvin = "0";
            }


            StringBuilder sb = new StringBuilder();

            sb.Append("<div class=\"categorymenu\">");
            sb.Append("<div class=\"decoratedblock\">");

            string title = tag.GetSafeAttribute("title");
            if (title.Trim().Length > 0)
            {
                sb.Append("<h4>" + title + "</h4>");
            }

            sb.Append("<ul>");

            int maxDepth = 5;

            string mode = tag.GetSafeAttribute("mode");
            switch (mode.Trim().ToUpperInvariant())
            {
                case "ROOT":
                case "ROOTS":
                    // Root Categories Only
                    LoadRoots(sb);
                    break;
                case "ALL":
                    // All Categories
                    LoadAllCategories(sb, maxDepth);
                    break;
                case "":
                case "PEERS":
                    // Peers, Children and Parents
                    LoadPeersAndChildren(sb);
                    break;
                case "ROOTPLUS":
                    // Show root and expanded children
                    LoadRootPlusExpandedChildren(sb);
                    break;
                default:
                    // All Categories
                    LoadPeersAndChildren(sb);
                    break;
            }

            sb.Append("</ul>");

            sb.Append("</div>");
            sb.Append("</div>");

            actions.Add(new Actions.LiteralText(sb.ToString()));
        }

        private void LoadRoots(StringBuilder sb)
        {
            List<CategorySnapshot> cats = App.CatalogServices.Categories.FindVisibleChildren("0");
            AddCategoryCollection(sb, cats, cats, 1, 1);
        }
        private void AddHomeLink(StringBuilder sb)
        {
            sb.Append("<li>");
            sb.Append("<a href=\"" + Url.Content("~") + "\" title=\"Home\">Home</a>");
            sb.Append("</li>");
        }
        private void AddSingleLink(StringBuilder sb, CategorySnapshot c, List<CategorySnapshot> allCats)
        {
            if (c.Bvin == CurrentCategory.Bvin)
            {
                sb.Append("<li class=\"current\">");
            }
            else
            {
                sb.Append("<li>");
            }

            string title = c.MetaTitle;
            string text = c.Name;

            int childCount = 0;
            if (this.ShowProductCount)
            {
                childCount += (App.CatalogServices.CategoriesXProducts.FindForCategory(c.Bvin, 1, int.MaxValue).Count);
            }

            if (this.ShowCategoryCount)
            {
                childCount += Category.FindChildrenInList(allCats, c.Bvin, false).Count;
            }

            if (childCount > 0)
            {
                text += " (" + childCount.ToString() + ")";
            }

            string url = UrlRewriter.BuildUrlForCategory(c, App.CurrentRequestContext.RoutingContext);
            bool openNew = false;

            if (c.SourceType == CategorySourceType.CustomLink)
            {
                openNew = c.CustomPageOpenInNewWindow;
            }

            sb.Append("<a href=\"" + url + "\" title=\"" + title + "\">" + text + "</a>");
        }
        private void LoadAllCategories(StringBuilder sb, int maxDepth)
        {
            List<CategorySnapshot> allCats = App.CatalogServices.Categories.FindAll();
            List<CategorySnapshot> children = Category.FindChildrenInList(allCats, "0", false);
            AddCategoryCollection(sb, allCats, children, 0, maxDepth);
        }
        private void AddCategoryCollection(StringBuilder sb, List<CategorySnapshot> allCats, List<CategorySnapshot> cats, int currentDepth, int maxDepth)
        {
            if (cats != null)
            {
                foreach (CategorySnapshot c in cats)
                {

                    if (c.Hidden == false)
                    {

                        AddSingleLink(sb, c, allCats);

                        if ((maxDepth == 0) | (currentDepth + 1 < maxDepth))
                        {
                            List<CategorySnapshot> children = App.CatalogServices.Categories.FindVisibleChildren(c.Bvin);
                            if (children != null)
                            {
                                if (children.Count > 0)
                                {
                                    sb.Append("<ul>" + System.Environment.NewLine);
                                    AddCategoryCollection(sb, allCats, children, currentDepth + 1, maxDepth);
                                    sb.Append("</ul>" + System.Environment.NewLine);
                                }
                            }
                        }

                        sb.Append("</li>");
                    }
                }
            }
        }
        private void LoadRootPlusExpandedChildren(StringBuilder sb)
        {
            List<CategorySnapshot> allCats = App.CatalogServices.Categories.FindAll();

            // Get Current Category
            CategorySnapshot currentCategory = Category.FindInList(allCats, CurrentCategory.Bvin);

            if (currentCategory != null)
            {
                if (currentCategory.Bvin != string.Empty)
                {
                    CurrentCategory.Bvin = currentCategory.Bvin;
                }

                // Find the trail from this category back to the root of the site
                List<CategorySnapshot> trail = new List<CategorySnapshot>();
                BuildParentTrail(allCats, CurrentCategory.Bvin, ref trail);
                if (trail == null)
                {
                    trail = new List<CategorySnapshot>();
                }

                if (trail.Count < 1)
                {
                    // Load Roots Only
                    LoadRoots(sb);
                }
                else
                {

                    string StartingRootCategoryId = currentCategory.Bvin;
                    StartingRootCategoryId = trail[trail.Count - 1].Bvin;


                    List<CategorySnapshot> roots = Category.FindChildrenInList(allCats, "0", false);
                    if (roots != null)
                    {
                        sb.Append("<ul>" + System.Environment.NewLine);

                        foreach (CategorySnapshot c in roots)
                        {
                            if (IsInTrail(c.Bvin, trail))
                            {
                                AddSingleLink(sb, c, allCats);
                                List<CategorySnapshot> children = new List<CategorySnapshot>();
                                children = Category.FindChildrenInList(allCats, StartingRootCategoryId, false);
                                if (children != null)
                                {
                                    sb.Append("<ul>" + System.Environment.NewLine);
                                    ExpandInTrail(sb, allCats, children, trail);
                                    sb.Append("</ul>" + System.Environment.NewLine);
                                }
                                sb.Append("</li>");

                                break;
                            }
                        }

                        sb.Append("</ul>" + System.Environment.NewLine);
                    }

                }
            }
            else
            {
                sb.Append("Invalid Category Id. Contact Administrator");
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
        private void ExpandInTrail(StringBuilder sb, List<CategorySnapshot> allCats, List<CategorySnapshot> cats, List<CategorySnapshot> trail)
        {
            if (cats != null)
            {
                foreach (CategorySnapshot c in cats)
                {

                    if (c.Hidden == false)
                    {


                        AddSingleLink(sb, c, allCats);

                        if (IsInTrail(c.Bvin, trail))
                        {
                            List<CategorySnapshot> children = Category.FindChildrenInList(allCats, c.Bvin, false);
                            if (children != null)
                            {
                                if (children.Count > 0)
                                {
                                    sb.Append("<ul>" + System.Environment.NewLine);
                                    ExpandInTrail(sb, allCats, children, trail);
                                    sb.Append("</ul>" + System.Environment.NewLine);
                                }
                            }
                        }

                        sb.Append("</li>");
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
        private void LoadPeersAndChildren(StringBuilder sb)
        {
            List<CategorySnapshot> allCats = App.CatalogServices.Categories.FindAll();

            // Get Current Category
            CategorySnapshot currentCategory = Category.FindInList(allCats, CurrentCategory.Bvin);

            // Trick system into accepting root category of zero which never exists in database
            if (CurrentCategory.Bvin == "0")
            {
                currentCategory = new CategorySnapshot();
                currentCategory.Bvin = "0";
            }

            if (currentCategory != null)
            {
                if (currentCategory.Bvin != string.Empty)
                {
                    CurrentCategory.Bvin = currentCategory.Bvin;
                }

                // Find the trail from this category back to the root of the site
                List<CategorySnapshot> trail = new List<CategorySnapshot>();
                BuildParentTrail(allCats, CurrentCategory.Bvin, ref trail);
                if (trail == null)
                {
                    trail = new List<CategorySnapshot>();
                }

                if (trail.Count < 1)
                {
                    // Load Roots Only
                    LoadRoots(sb);
                }
                else
                {

                    CategoryPeerSet neighbors = GetPeerSet(allCats, currentCategory);

                    if (trail.Count == 1)
                    {
                        // special case where we want only peers and children
                        RenderPeersChildren(sb, neighbors, currentCategory, allCats);
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
                                RenderParentsPeersChildren(sb, neighbors, trail[1], allCats);
                            }
                            else
                            {
                                RenderParentsPeersChildren(sb, neighbors, currentCategory, allCats);
                            }
                        }
                        else
                        {
                            // normal load of peers
                            RenderParentsPeersChildren(sb, neighbors, currentCategory, allCats);
                        }
                    }
                }
            }

            else
            {
                sb.Append("Invalid Category Id. Contact Administrator");
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
        private void RenderPeersChildren(StringBuilder sb, CategoryPeerSet neighbors, CategorySnapshot currentCategory, List<CategorySnapshot> allCats)
        {
            // No Parents, start with peers
            foreach (CategorySnapshot peer in neighbors.Peers)
            {
                if (!peer.Hidden)
                {
                    AddSingleLink(sb, peer, allCats);
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
                                        sb.Append("<ul>" + System.Environment.NewLine);
                                        initialized = true;
                                    }

                                    AddSingleLink(sb, child, allCats);
                                    sb.Append("</li>" + System.Environment.NewLine);
                                }
                            }
                            if (initialized)
                            {
                                sb.Append("</ul>" + System.Environment.NewLine);
                            }
                        }

                    }
                    sb.Append("</li>" + System.Environment.NewLine);
                }
            }
        }
        private void RenderParentsPeersChildren(StringBuilder sb, CategoryPeerSet neighbors, CategorySnapshot currentCategory, List<CategorySnapshot> allCats)
        {
            if (neighbors.Parents.Count < 1)
            {
                RenderPeersChildren(sb, neighbors, currentCategory, allCats);
            }
            else
            {

                // Add Parents
                foreach (CategorySnapshot parent in neighbors.Parents)
                {
                    if (!parent.Hidden)
                    {
                        AddSingleLink(sb, parent, allCats);

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
                                        sb.Append("<ul>");
                                        peerInitialized = true;
                                    }
                                    AddSingleLink(sb, peer, allCats);
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
                                                        sb.Append("<ul>" + System.Environment.NewLine);
                                                        childInitialized = true;
                                                    }
                                                    AddSingleLink(sb, child, allCats);
                                                    sb.Append("</li>" + System.Environment.NewLine);
                                                }
                                            }
                                            if (childInitialized)
                                            {
                                                sb.Append("</ul>" + System.Environment.NewLine);
                                            }
                                        }


                                    }
                                    sb.Append("</li>" + System.Environment.NewLine);
                                }
                            }

                            if (peerInitialized)
                            {
                                sb.Append("</ul>" + System.Environment.NewLine);
                            }

                        }

                    }
                    sb.Append("</li>" + System.Environment.NewLine);
                }
            }
        }        
    }
}