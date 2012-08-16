using System;
using System.Collections.ObjectModel;
using System.Web.UI;
using System.Web.UI.WebControls;
using MerchantTribe.Commerce.Catalog;
using MerchantTribe.Commerce.Content;
using MerchantTribe.Commerce.Membership;
using MerchantTribe.Commerce.Utilities;
using System.Collections.Generic;
using MerchantTribe.Web.Logging;

namespace MerchantTribeStore.BVAdmin.Catalog
{
    public partial class Categories_EditCustomLink : BaseAdminPage
    {

        public string IconImage = "";

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
                    if (category == null)
                    {
                        MerchantTribe.Commerce.EventLog.LogEvent("Edit Category Page",
                                                              "Could not find category with bvin " + this.BvinField.Value,
                                                              EventLogSeverity.Warning);
                        Response.Redirect("categories.aspx");
                    }

                    if (ViewState["type"] == null)
                    {
                        ViewState["type"] = category.SourceType;
                    }

                    if (category != null)
                    {
                        if (category.SourceType == CategorySourceType.CustomPage)
                        {
                            Response.Redirect("Categories/Custom/Edit/" + category.Bvin);
                        }
                    }

                    CategoryBreadCrumbTrail1.LoadTrail(Request.QueryString["id"]);
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
            //Me.TemplatePreviewImage.ImageUrl = Content.ModuleController.FindCategoryTemplatePreviewImage(Me.TemplateList.SelectedValue, Request.PhysicalApplicationPath)
        }

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.PageTitle = "Edit Custom Link Category";
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
                    this.LinkToField.Text = c.CustomPageUrl;
                    this.MetaTitleField.Text = c.MetaTitle;
                    this.chkHidden.Checked = c.Hidden;
                    this.ParentIDField.Value = c.ParentId;
                    UpdateIconImage(c);
                }
            }
            return c;
        }
    
        private void UpdateIconImage(Category c)
        {
            IconImage = MerchantTribe.Commerce.Storage.DiskStorage.CategoryIconUrl(MTApp, c.Bvin, c.ImageUrl, true);
            if (IconImage == string.Empty || c.ImageUrl == string.Empty)
            {
                IconImage = Page.ResolveUrl("~/content/admin/images/MissingImage.png");
            }
        }

        private bool Save()
        {
            Category c = MTApp.CatalogServices.Categories.Find(this.BvinField.Value);
            if (c == null)
            {
                c = new Category();
            }
            return Save(c);
        }
        private bool Save(Category c)
        {
            bool result = false;

            if (c != null)
            {
                c.Name = this.NameField.Text.Trim();
                c.MetaTitle = this.MetaTitleField.Text.Trim();
                c.CustomPageUrl = this.LinkToField.Text.Trim();
                c.ShowInTopMenu = false;                
                c.Hidden = this.chkHidden.Checked;

                // Icon Image Upload
                if ((this.iconupload.HasFile))
                {
                    string fileName = System.IO.Path.GetFileNameWithoutExtension(iconupload.FileName);
                    string ext = System.IO.Path.GetExtension(iconupload.FileName);

                    if (MerchantTribe.Commerce.Storage.DiskStorage.ValidateImageType(ext))
                    {
                        fileName = MerchantTribe.Web.Text.CleanFileName(fileName);
                        if ((MerchantTribe.Commerce.Storage.DiskStorage.UploadCategoryIcon(MTApp.CurrentStore.Id, c.Bvin, this.iconupload.PostedFile)))
                        {
                            c.ImageUrl = fileName + ext;
                        }
                    }
                    else
                    {
                        result = false;
                        this.MessageBox1.ShowError("Only .PNG, .JPG, .GIF file types are allowed for icon images");
                    }
                }

                c.SourceType = CategorySourceType.CustomLink;
                
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
                }

            }

            return result;
        }

        protected void btnCancel_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            Response.Redirect("Categories.aspx?id=" + this.ParentIDField.Value);
        }

        protected void btnSaveChanges_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            this.lblError.Text = string.Empty;

            if (Save() == true)
            {
                Response.Redirect("Categories.aspx?id=" + this.ParentIDField.Value);
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
                    UpdateIconImage(cat);
                }
            }
            else
            {
                MessageBox1.ShowError("Error during update. Please check event log.");
            }
        }

        protected void delIcon_Click(object sender, ImageClickEventArgs e)
        {
            Category c = MTApp.CatalogServices.Categories.Find(this.BvinField.Value);
            if (c != null)
            {
                c.ImageUrl = string.Empty;
                Save(c);
                LoadCategory();
            }
        }

    }
}