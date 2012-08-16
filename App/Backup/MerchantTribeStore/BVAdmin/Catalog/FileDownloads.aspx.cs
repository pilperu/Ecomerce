using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MerchantTribe.Commerce;
using MerchantTribe.Commerce.Catalog;
using MerchantTribe.Commerce.Membership;


namespace MerchantTribeStore.BVAdmin.Catalog
{
    public partial class FileDownloads : BaseAdminPage
    {
        private Product localProduct = new Product();
       
        protected override void OnInit(System.EventArgs e)
        {
            base.OnInit(e);

            this.PageTitle = "Edit File Downloads";
            this.CurrentTab = AdminTabType.Catalog;
            ValidateCurrentUserHasPermission(SystemPermissions.CatalogView);

            if (Request.QueryString["id"] != null)
            {
                string productBvin = Request.QueryString["id"];
                localProduct = MTApp.CatalogServices.Products.Find(productBvin);
                FilePicker1.ProductId = localProduct.Bvin;
            }
        }

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);
            
            InitializeFileGrid();

            if (!Page.IsPostBack)
            {
                this.CurrentTab = AdminTabType.Catalog;
                
            }
        }

        protected void InitializeFileGrid()
        {
            FileGrid.DataSource = MTApp.CatalogServices.ProductFiles.FindByProductId(localProduct.Bvin);            
            FileGrid.DataBind();
        }

        protected void FileGrid_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            if (!MTApp.CatalogServices.ProductFiles.RemoveAssociatedProduct((string)FileGrid.DataKeys[e.RowIndex].Value, localProduct.Bvin))
            {
                MessageBox1.ShowError("An error occurred while trying to delete your file from the database.");
            }
            InitializeFileGrid();
        }

        protected void FileGrid_RowEditing(object sender, GridViewEditEventArgs e)
        {            
                string primaryKey = (string)(FileGrid.DataKeys[e.NewEditIndex].Value);
                ProductFile file = MTApp.CatalogServices.ProductFiles.Find(primaryKey);
                if (!ViewUtilities.DownloadFile(file, this.MTApp))
                {
                    MessageBox1.ShowWarning("The file to download could not be found.");
                }            
        }

        protected void AddFileButton_Click(object sender, ImageClickEventArgs e)
        {
            if (Page.IsValid)
            {
                FilePicker1.DownloadOrLinkFile(null, this.MessageBox1);
                InitializeFileGrid();
            }
        }

    }
}