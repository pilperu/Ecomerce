using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Web.UI;
using System.Web.UI.WebControls;
using MerchantTribe.Commerce;
using MerchantTribe.Commerce.Catalog;
using MerchantTribe.Commerce.Content;
using MerchantTribe.Commerce.Utilities;

namespace MerchantTribeStore
{

    partial class BVModules_Controls_CategoryBreadCrumbTrail : MerchantTribe.Commerce.Content.BVUserControl
    {

        private string _Spacer = "&nbsp;&raquo;&nbsp;";

        private LiteralControl ExtrasLiteral = new LiteralControl();

        public void AddExtra(string extra)
        {
            this.ExtrasLiteral.Text += _Spacer + "<span class=\"current\">" + extra + "</span>";
        }
        public void ClearExtra()
        {
            this.ExtrasLiteral.Text = string.Empty;
        }

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);
            
            string categoryId = SessionManager.CategoryLastId;
            CategorySnapshot currentCategory = null;
            
            currentCategory = new CategorySnapshot(MyPage.MTApp.CatalogServices.Categories.Find(categoryId));

            LoadTrail(currentCategory);

        }

        public void LoadTrail(CategorySnapshot category)
        {
            this.TrailPlaceholder.Controls.Clear();

            if ((category != null))
            {


                List<CategorySnapshot> allCats = MyPage.MTApp.CatalogServices.Categories.FindAllPaged(1, int.MaxValue);

                if (!category.Hidden)
                {
                    this.TrailPlaceholder.Controls.Add(new LiteralControl("<div class=\"singletrail\">"));
                    List<CategorySnapshot> trail = new List<CategorySnapshot>();
                    if (category != null && category.Bvin != string.Empty)
                    {
                        trail = Category.BuildTrailToRoot(category.Bvin, MyPage.MTApp.CurrentRequestContext);
                    }

                    HyperLink m = new HyperLink();
                    m.CssClass = "home";
                    m.ToolTip = SiteTerms.GetTerm(SiteTermIds.Home);
                    m.Text = SiteTerms.GetTerm(SiteTermIds.Home);
                    m.NavigateUrl = this.Page.ResolveUrl("~/Default.aspx");
                    m.EnableViewState = false;
                    this.TrailPlaceholder.Controls.Add(m);
                    this.TrailPlaceholder.Controls.Add(new LiteralControl("<span class=\"spacer\">" + _Spacer + "</span>"));

                    if (trail != null)
                    {
                        // Walk list backwards
                        for (int j = trail.Count - 1; j >= 0; j += -1)
                        {
                            if (j != trail.Count - 1)
                            {
                                this.TrailPlaceholder.Controls.Add(new LiteralControl("<span class=\"spacer\">" + _Spacer + "</span>"));
                            }
                            if (j != 0)
                            {
                                AddCategoryLink(trail[j]);
                            }
                            else
                            {
                                AddCategoryLink(trail[j]);
                            }
                        }
                    }
                    this.TrailPlaceholder.Controls.Add(ExtrasLiteral);
                    this.TrailPlaceholder.Controls.Add(new LiteralControl("</div>"));
                }

            }
        }

        private void AddCategoryLink(CategorySnapshot c)
        {
            HyperLink m = new HyperLink();
            m.ToolTip = c.MetaTitle;
            m.Text = c.Name;

            m.NavigateUrl = UrlRewriter.BuildUrlForCategory(c, MyPage.MTApp.CurrentRequestContext.RoutingContext);
            if (c.SourceType == CategorySourceType.CustomLink)
            {
                if (c.CustomPageOpenInNewWindow == true)
                {
                    m.Target = "_blank";
                }
            }

            m.EnableViewState = false;
            this.TrailPlaceholder.Controls.Add(m);
        }

        private void AddCategoryName(CategorySnapshot c)
        {
            this.TrailPlaceholder.Controls.Add(new LiteralControl("<span class=\"current\">" + c.Name + "</span>"));
        }
    }

}