using System;
using System.Web.UI;
using System.Collections.ObjectModel;
using MerchantTribe.Commerce.Catalog;
using MerchantTribe.Commerce.Membership;
using System.Collections.Generic;

namespace MerchantTribeStore
{

    partial class products_edit_categories : BaseProductAdminPage
    {

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.PageTitle = "Edit Product Categories";
            this.CurrentTab = AdminTabType.Catalog;
            ValidateCurrentUserHasPermission(SystemPermissions.CatalogView);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (!Page.IsPostBack)
            {

                string EditID;
                EditID = Request.QueryString["id"];
                if (EditID.Trim().Length < 1)
                {
                    msg.ShowError("Unable to load the requested product.");
                }
                else
                {
                    Product p = new Product();
                    p = MTApp.CatalogServices.Products.Find(EditID);
                    if (p != null)
                    {
                        ViewState["ID"] = EditID;
                    }
                    else
                    {
                        msg.ShowError("Unable to load the requested product.");
                    }
                    p = null;
                }

                LoadCategories();
            }
        }

        void LoadCategories()
        {

            chkCategories.Items.Clear();

            List<CategorySnapshot> t = MTApp.CatalogServices.FindCategoriesForProduct((string)Request.QueryString["ID"]);
            Collection<System.Web.UI.WebControls.ListItem> tree = Category.ListFullTreeWithIndents(MTApp.CurrentRequestContext);

            foreach (System.Web.UI.WebControls.ListItem li in tree)
            {
                this.chkCategories.Items.Add(li);
            }

            foreach (CategorySnapshot ca in t)
            {
                //Dim ca As Catalog.Category = Catalog.Category.FindByBvin(mydatarow.Item(0).ToString)
                foreach (System.Web.UI.WebControls.ListItem l in chkCategories.Items)
                {
                    if (l.Value == ca.Bvin)
                    {
                        l.Selected = true;
                    }
                }

            }

        }

        private void SaveSettings()
        {

            MTApp.CatalogServices.CategoriesXProducts.DeleteAllForProduct(Request.QueryString["id"].ToString());
            //Category.RemoveProductFromAll(Request.QueryString["id"].ToString());
            foreach (System.Web.UI.WebControls.ListItem li in chkCategories.Items)
            {
                if (li.Selected == true)
                {
                    MTApp.CatalogServices.CategoriesXProducts.AddProductToCategory(Request.QueryString["id"].ToString(), li.Value);
                }
                else
                {
                    MTApp.CatalogServices.CategoriesXProducts.RemoveProductFromCategory(Request.QueryString["id"].ToString(), li.Value);
                }
            }
        }

        protected void CancelButton_Click(System.Object sender, System.Web.UI.ImageClickEventArgs e)
        {
            Cancel();
        }

        protected void SaveButton_Click(System.Object sender, System.Web.UI.ImageClickEventArgs e)
        {
            Save();
        }

        private void Cancel()
        {
            Response.Redirect("Products_edit.aspx?id=" + ViewState["ID"]);
        }

        protected override bool Save()
        {
            this.msg.ClearMessage();
            SaveSettings();
            LoadCategories();
            msg.ShowOk("Changes Saved!");
            return true;
        }
    }
}