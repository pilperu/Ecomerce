using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MerchantTribe.Commerce.Catalog;
using MerchantTribe.Commerce.Membership;

namespace MerchantTribeStore
{

    public partial class BVAdmin_Catalog_ProductsEdit_TabsEdit : BaseProductAdminPage
    {

        protected void btnSave_Click(object sender, ImageClickEventArgs e)
        {
            SaveItem();
        }

        private string tabid = string.Empty;
        protected internal string productBvin = string.Empty;

        protected override void OnInit(System.EventArgs e)
        {

            base.OnInit(e);
            tabid = Request.QueryString["tid"];
            productBvin = Request.QueryString["id"];
            this.PageTitle = "Edit Product Tab";
            this.CurrentTab = AdminTabType.Catalog;
            ValidateCurrentUserHasPermission(SystemPermissions.CatalogView);
        }

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);
            if (!Page.IsPostBack)
            {
                LoadItem();
            }
        }

        private void LoadItem()
        {
            Product p = MTApp.CatalogServices.Products.Find(productBvin);
            if (p == null) return;
            if (p.Tabs.Count < 1) return;

            foreach (ProductDescriptionTab t in p.Tabs)
            {
                if (t.Bvin == tabid)
                {
                    this.TabTitleField.Text = t.TabTitle;
                    this.HtmlDataField.Text = t.HtmlData;
                }
            }
        }


        private bool SaveItem()
        {
            this.MessageBox1.ClearMessage();
            bool success = true;


            Product p = MTApp.CatalogServices.Products.Find(productBvin);
            if (p == null) return false;
            if (p.Tabs.Count < 1) return false;

            foreach (ProductDescriptionTab t in p.Tabs)
            {
                if (t.Bvin == tabid)
                {
                    t.TabTitle = this.TabTitleField.Text.Trim();
                    t.HtmlData = this.HtmlDataField.Text.Trim();
                    success = MTApp.CatalogServices.ProductsUpdateWithSearchRebuild(p);
                }
            }



            if ((success))
            {
                this.MessageBox1.ShowOk("Changes Saved!");
            }
            else
            {
                this.MessageBox1.ShowWarning("Unable to save changes. An administrator has been alerted.");
            }

            return success;
        }

        protected override bool Save()
        {
            return SaveItem();
        }

        protected void btnSaveAndClose_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            if ((SaveItem()))
            {
                Response.Redirect("ProductsEdit_Tabs.aspx?id=" + productBvin);
            }
        }
    }
}