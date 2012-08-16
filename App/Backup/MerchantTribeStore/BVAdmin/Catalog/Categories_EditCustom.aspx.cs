using System;
using System.Collections.ObjectModel;
using System.Web.UI;
using System.Web.UI.WebControls;
using MerchantTribe.Commerce.Catalog;
using MerchantTribe.Commerce.Content;
using MerchantTribe.Commerce.Membership;
using MerchantTribe.Commerce.Utilities;

namespace MerchantTribeStore
{

    public partial class BVAdmin_Catalog_Categories_EditCustom : BaseAdminPage
    {

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);

            if (!Page.IsPostBack)
            {
                this.NameField.Focus();

                if (Request.QueryString["id"] != null)
                {
                    this.BvinField.Value = Request.QueryString["id"];
                    if (Request.QueryString["type"] != null)
                    {
                        ViewState["type"] = Request.QueryString["type"];
                    }
                    Category category = LoadCategory();

                    if (category != null)
                    {
                        if (category.SourceType != CategorySourceType.CustomPage)
                        {
                            Response.Redirect("categories_edit.aspx?id=" + category.Bvin);
                        }
                    }
                    if (ViewState["type"] == null)
                    {
                        ViewState["type"] = category.SourceType;
                    }
                    CategoryBreadCrumbTrail1.LoadTrail(Request.QueryString["id"]);
                    PopulateStoreLink(category);
                    this.UrlsAssociated1.ObjectId = category.Bvin;
                    this.UrlsAssociated1.LoadUrls();
                }
                else
                {
                    this.BvinField.Value = string.Empty;
                    if (Request.QueryString["ParentID"] != null)
                    {
                        CategoryBreadCrumbTrail1.LoadTrail(Request.QueryString["ParentID"]);
                        if (Request.QueryString["type"] != null)
                        {
                            ViewState["type"] = Request.QueryString["type"];
                        }
                        this.ParentIDField.Value = (string)Request.QueryString["ParentID"];
                    }
                    else
                    {
                        Response.Redirect("~/BVAdmin/Catalog/Categories.aspx");
                    }
                }
            }
        }

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.PageTitle = "Edit Category";
            this.CurrentTab = AdminTabType.Catalog;
            ValidateCurrentUserHasPermission(SystemPermissions.CatalogView);
        }

        private Category LoadCategory()
        {
            Category c = MTApp.CatalogServices.Categories.Find(this.BvinField.Value);
            if (c != null)
            {

                if (c.Bvin != string.Empty)
                {
                    this.NameField.Text = c.Name;
                    this.DescriptionField.Text = c.Description;
                    if (this.DescriptionField.SupportsTransform == true)
                    {
                        if (c.PreTransformDescription.Trim().Length > 0)
                        {
                            this.DescriptionField.Text = c.PreTransformDescription;
                        }
                    }
                    this.MetaDescriptionField.Text = c.MetaDescription;
                    this.MetaKeywordsField.Text = c.MetaKeywords;
                    this.MetaTitleField.Text = c.MetaTitle;
                    this.RewriteUrlField.Text = c.RewriteUrl;

                    this.ParentIDField.Value = c.ParentId;

                    if (c.CustomPageLayout == CustomPageLayoutType.WithSideBar)
                    {
                        this.chkShowSidebar.Checked = true;
                    }
                }
            }
            return c;
        }

        private void PopulateStoreLink(Category c)
        {

            HyperLink m = new HyperLink();
            m.ImageUrl = "~/BVAdmin/Images/Buttons/ViewInStore.png";
            m.ToolTip = c.MetaTitle;
            m.NavigateUrl = UrlRewriter.BuildUrlForCategory(new CategorySnapshot(c), MTApp.CurrentRequestContext.RoutingContext);
            m.EnableViewState = false;
            this.inStore.Controls.Add(m);

        }

        private bool Save()
        {
            bool result = false;

            Category c = MTApp.CatalogServices.Categories.Find(this.BvinField.Value);
            if (c == null)
            {
                c = new Category();
            }
            if (c != null)
            {
                c.Name = this.NameField.Text.Trim();
                c.Description = this.DescriptionField.Text.Trim();
                c.PreTransformDescription = this.DescriptionField.PreTransformText;
                c.MetaDescription = this.MetaDescriptionField.Text.Trim();
                c.MetaTitle = this.MetaTitleField.Text.Trim();
                c.MetaKeywords = this.MetaKeywordsField.Text.Trim();
                c.ShowInTopMenu = false;
                c.SourceType = CategorySourceType.CustomPage;

                string oldUrl = c.RewriteUrl;

                // no entry, generate one
                if (c.RewriteUrl.Trim().Length < 1)
                {
                    c.RewriteUrl = MerchantTribe.Web.Text.Slugify(c.Name, true, true);
                }
                else
                {
                    c.RewriteUrl = MerchantTribe.Web.Text.Slugify(this.RewriteUrlField.Text, true, true);
                }
                this.RewriteUrlField.Text = c.RewriteUrl;

                if (UrlRewriter.IsCategorySlugInUse(c.RewriteUrl, c.Bvin, MTApp.CurrentRequestContext))
                {
                    this.MessageBox1.ShowWarning("The requested URL is already in use by another item.");
                    return false;
                }

                if (this.chkShowSidebar.Checked)
                {
                    c.CustomPageLayout = CustomPageLayoutType.WithSideBar;
                }
                else
                {
                    c.CustomPageLayout = CustomPageLayoutType.Empty;
                }

                c.CustomerChangeableSortOrder = true;

                if (this.BvinField.Value == string.Empty)
                {
                    c.ParentId = this.ParentIDField.Value;
                    result = MTApp.CatalogServices.Categories.Create(c);
                    if (result)
                    {
                        result = MTApp.CatalogServices.Categories.SubmitChanges();
                    }
                }
                else
                {
                    result = MTApp.CatalogServices.Categories.Update(c);
                    if (result)
                    {
                        result = MTApp.CatalogServices.Categories.SubmitChanges();
                    }
                }

                if (result == false)
                {
                    this.lblError.Text = "Unable to save category. Uknown error.";
                }
                else
                {
                    // Update bvin field so that next save will call updated instead of create
                    this.BvinField.Value = c.Bvin;

                    if (oldUrl != string.Empty)
                    {
                        if (oldUrl != c.RewriteUrl)
                        {
                            MTApp.ContentServices.CustomUrls.Register301(oldUrl,
                                                  c.RewriteUrl,
                                                  c.Bvin, CustomUrlType.Category, MTApp.CurrentRequestContext,
                                                  MTApp);
                            this.UrlsAssociated1.LoadUrls();
                        }
                    }
                }
            }

            return result;
        }

        protected void btnCancel_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            Response.Redirect("Categories.aspx");
        }

        protected void btnSaveChanges_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            this.lblError.Text = string.Empty;

            if (Save() == true)
            {
                Response.Redirect("Categories.aspx");
            }
        }

        protected void btnSelectProducts_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            if (this.Save() == true)
            {
                Response.Redirect("Categories_ManualSelection.aspx?id=" + this.BvinField.Value + "&type=" + ViewState["type"]);
            }
        }

        protected void UpdateButton_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            this.lblError.Text = string.Empty;
            if (this.Save())
            {
                MessageBox1.ShowOk("Category Updated Successfully.");
                Category cat = MTApp.CatalogServices.Categories.Find(this.BvinField.Value);
                if (cat != null && cat.Bvin != string.Empty)
                {
                    PopulateStoreLink(cat);
                }
            }
            else
            {
                MessageBox1.ShowError("Error during update. Please check event log.");
            }
        }
    }
}