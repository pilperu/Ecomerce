using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MerchantTribe.Commerce.Catalog;
using MerchantTribe.Commerce.Membership;

namespace MerchantTribeStore
{

    partial class BVAdmin_Catalog_Categories : BaseAdminPage
    {
        
        private string IconImage(CategorySourceType type)
        {
            switch (type)
            {
                case CategorySourceType.DrillDown:
                    return "IconDrillDown.png";
                case CategorySourceType.CustomPage:
                    return "IconCustomPage.png";
                case CategorySourceType.FlexPage:
                    return "IconCustomPage.png";
                default:
                    return "IconCategory.png";
            }
        }

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);

            if (!Page.IsPostBack)
            {
                RenderCategoryTree();
            }
        }

        protected override void OnPreInit(System.EventArgs e)
        {
            base.OnPreInit(e);
            this.PageTitle = "Categories";
            this.CurrentTab = AdminTabType.Catalog;
            ValidateCurrentUserHasPermission(SystemPermissions.CatalogView);
        }


        private void RenderCategoryTree()
        {
            StringBuilder sb = new StringBuilder();

            List<CategorySnapshot> allCats = MTApp.CatalogServices.Categories.FindAllPaged(1, 5000);

            this.lstParents.Items.Clear();
            this.lstParents.Items.Add(new System.Web.UI.WebControls.ListItem("( Site Root )", "0"));
            this.lstParents.Items.Add(new System.Web.UI.WebControls.ListItem("", "0"));
            Collection<System.Web.UI.WebControls.ListItem> parents = Category.ListFullTreeWithIndents(allCats, true);
            foreach (System.Web.UI.WebControls.ListItem li in parents)
            {
                this.lstParents.Items.Add(li);
            }

            RenderChildren("0", allCats, sb);

            this.litMain.Text = sb.ToString();
        }

        private void RenderChildren(string bvin, List<CategorySnapshot> allCats, StringBuilder sb)
        {

            List<CategorySnapshot> children = Category.FindChildrenInList(allCats, bvin, true);
            if (children == null) return;
            if (children.Count > 0)
            {
                sb.Append("<div id=\"children" + bvin + "\" class=\"ui-sortable nested\" unselectable=\"on\" style=\"-moz-user-select: none;\">");
                foreach (CategorySnapshot child in children)
                {

                    string editUrl = "~" + MTApp.CatalogServices.EditorRouteForCategory(child.SourceType, child.Bvin);
                    editUrl = Page.ResolveUrl(editUrl);
                    string icon = Page.ResolveUrl("~/bvadmin/images/" + IconImage(child.SourceType));
                    

                    sb.Append("<div id=\"" + child.Bvin + "\" class=\"dragitem2\">");
                    sb.Append("<table width=\"100%\" class=\"formtable\">");
                    sb.Append("<tbody><tr><td>");
                    sb.Append("<img src=\"" + icon + "\" border=\"0\" /> ");
                    sb.Append("<a href=\"" + editUrl + "\">" + child.Name + "</a>");
                    //sb.Append("<br /><div class=\"nested\">+ new</div>");
                    sb.Append("</td>");
                    sb.Append("<td width=\"75\"><a href=\"" + editUrl + "\">");
                    sb.Append("<img alt=\"edit\" src=\"../images/buttons/edit.png\"></a></td>");


                    if (Category.FindChildrenInList(allCats, child.Bvin).Count > 0)
                    {
                        sb.Append("<td width=\"30\">&nbsp;</td>");
                    }
                    else
                    {
                        sb.Append("<td width=\"30\"><a id=\"rem" + child.Bvin + "\" class=\"trash\" href=\"#\">");
                        sb.Append("<img alt=\"Delete\" src=\"../../images/system/trashcan.png\"></a></td>");
                    }


                    sb.Append("<td width=\"30\"><a class=\"handle\" href=\"#\">");
                    sb.Append("<img alt=\"Move\" src=\"../../images/system/draghandle.png\"></a></td></tr></tbody></table>");
                    RenderChildren(child.Bvin, allCats, sb);
                    sb.Append("</div>");
                }
                sb.Append("</div>");
            }
        }

        protected void btnNew_Click(object sender, ImageClickEventArgs e)
        {
            Category c = new Category();
            c.ParentId = this.lstParents.SelectedItem.Value;
            CategorySourceType sourceType = CategorySourceType.Manual;
            System.Enum.TryParse<CategorySourceType>(this.lstType.SelectedItem.Value, out sourceType);
            c.SourceType = sourceType;
            c.Name = "NEW Page";            
            c.RewriteUrl = "NEW-Page";
            c.StoreId = MTApp.CurrentStore.Id;

            if (c.SourceType == CategorySourceType.CustomPage)
            {
                c.GetCurrentVersion().Areas.SetAreaContent("Main", string.Empty);
                c.TemplateName = "default.html";                
            }
            MTApp.CatalogServices.Categories.Create(c);

            string editUrl = "~" + MTApp.CatalogServices.EditorRouteForCategory(c.SourceType, c.Bvin);
            editUrl = Page.ResolveUrl(editUrl);
            Response.Redirect(editUrl);            
        }
    }
    
}