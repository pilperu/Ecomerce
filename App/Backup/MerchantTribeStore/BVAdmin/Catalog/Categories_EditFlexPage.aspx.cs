using System;
using System.Collections.ObjectModel;
using System.Web.UI;
using System.Web.UI.WebControls;
using MerchantTribe.Commerce.Catalog;
using MerchantTribe.Commerce.Content;
using MerchantTribe.Commerce.Membership;
using MerchantTribe.Commerce.Utilities;

namespace MerchantTribeStore.BVAdmin.Catalog
{
    public partial class Categories_EditFlexPage : BaseAdminPage
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
                        if (category.SourceType != CategorySourceType.FlexPage)
                        {
                            Response.Redirect("categories_edit.aspx?id=" + category.Bvin);
                        }
                    }
                    if (ViewState["type"] == null)
                    {
                        ViewState["type"] = category.SourceType;
                    }
                    CategoryBreadCrumbTrail1.LoadTrail(Request.QueryString["id"]);
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
                        CreateCategory();                        
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
            this.PageTitle = "Edit Web Site Page";
            this.CurrentTab = AdminTabType.Catalog;
            ValidateCurrentUserHasPermission(SystemPermissions.CatalogView);
        }

        private Category CreateCategory()
        {
            Category c = MTApp.CatalogServices.Categories.Find(this.BvinField.Value);
            if (c == null) c = new Category();
            c.Name = "NEW Web Site Page";
            c.ParentId = this.ParentIDField.Value;
            c.RewriteUrl = "NEW-Web-Site-Page";
            c.SourceType = CategorySourceType.FlexPage;
            c.StoreId = MTApp.CurrentStore.Id;            
            MTApp.CatalogServices.Categories.Create(c);
            this.BvinField.Value = c.Bvin;
            return LoadCategory();
        }

        private Category LoadCategory()
        {
            Category c = MTApp.CatalogServices.Categories.Find(this.BvinField.Value);
            if (c != null)
            {
                if (c.Bvin != string.Empty)
                {
                    this.NameField.Text = c.Name;                    
                    this.MetaDescriptionField.Text = c.MetaDescription;
                    this.MetaKeywordsField.Text = c.MetaKeywords;
                    this.MetaTitleField.Text = c.MetaTitle;
                    this.RewriteUrlField.Text = c.RewriteUrl;
                    this.ParentIDField.Value = c.ParentId;
                    this.chkHidden.Checked = c.Hidden;
                }
            }
            return c;
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
                c.MetaDescription = this.MetaDescriptionField.Text.Trim();
                c.MetaTitle = this.MetaTitleField.Text.Trim();
                c.MetaKeywords = this.MetaKeywordsField.Text.Trim();
                c.ShowInTopMenu = false;
                c.SourceType = CategorySourceType.FlexPage;
                c.Hidden = this.chkHidden.Checked;                
                c.CustomPageLayout = CustomPageLayoutType.Empty;                             

                string oldUrl = c.RewriteUrl;

                // no entry, generate one
                if (c.RewriteUrl.Trim().Length < 1 && this.RewriteUrlField.Text.Trim().Length < 1)
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
                
                c.CustomerChangeableSortOrder = true;

                if (this.BvinField.Value == string.Empty)
                {
                    c.ParentId = this.ParentIDField.Value;
                    result = MTApp.CatalogServices.Categories.Create(c);
                }
                else
                {
                    result = MTApp.CatalogServices.Categories.Update(c);
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
                            MTApp.ContentServices.CustomUrls.Register301(oldUrl, c.RewriteUrl,
                                                  c.Bvin, CustomUrlType.Category, MTApp.CurrentRequestContext, MTApp);
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
       
        protected void UpdateButton_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            this.lblError.Text = string.Empty;
            if (this.Save())
            {
                MessageBox1.ShowOk("Page Updated Successfully.");
                Category cat = MTApp.CatalogServices.Categories.Find(this.BvinField.Value);
            }
            else
            {
                MessageBox1.ShowError("Error during update. Please check event log.");
            }
        }

        protected void btnEdit_Click(object sender, ImageClickEventArgs e)
        {               
            if (Save())
            {
                Category c = MTApp.CatalogServices.Categories.Find(this.BvinField.Value);
                MTApp.IsEditMode = true;
                string destination = UrlRewriter.BuildUrlForCategory(new CategorySnapshot(c), MTApp.CurrentRequestContext.RoutingContext);
                destination = "../../" + destination;
                if (destination.StartsWith("http"))
                {
                    destination = destination.Replace("https://", "http://");
                }                
                Response.Redirect(destination);
            }
        }
    }
}