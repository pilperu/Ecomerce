using System.Web.UI;
using System.Web.UI.WebControls;
using MerchantTribe.Commerce.Catalog;
using MerchantTribe.Commerce.Membership;

namespace MerchantTribeStore
{

    partial class BVAdmin_Catalog_FileVaultDetailsView : BaseAdminPage
    {

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);
            if (!Page.IsPostBack)
            {
                if (Request.QueryString["id"] == null)
                {
                    Response.Redirect("~/BVAdmin/Catalog/default.aspx");
                }
                else
                {
                    ViewState["id"] = Request.QueryString["id"];
                }
                BindProductsGrid();
                PopulateFileInfo();
            }

            //If Me.NewSkuField.Text <> String.Empty Then
            //VariantsDisplay1.Initialize(False)
            //End If
        }

        protected override void OnPreInit(System.EventArgs e)
        {
            base.OnPreInit(e);
            this.PageTitle = "File Vault Edit";
            this.CurrentTab = AdminTabType.Catalog;
            ValidateCurrentUserHasPermission(SystemPermissions.CatalogView);
        }

        protected void BindProductsGrid()
        {
            ProductsGridView.DataSource = MTApp.CatalogServices.FindProductsForFile((string)ViewState["id"]);
            ProductsGridView.DataBind();
        }

        protected void PopulateFileInfo()
        {
            ProductFile file = MTApp.CatalogServices.ProductFiles.Find((string)ViewState["id"]);
            NameLabel.Text = file.FileName;
            DescriptionTextBox.Text = file.ShortDescription;
        }

        protected void ProductsGridView_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
        {
            string key = (string)ProductsGridView.DataKeys[e.RowIndex].Value;
            ProductFile file = MTApp.CatalogServices.ProductFiles.Find((string)ViewState["id"]);
            MTApp.CatalogServices.ProductFiles.RemoveAssociatedProduct(file.Bvin, key);
            BindProductsGrid();
        }

        protected void btnBrowseProducts_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            this.pnlProductPicker.Visible = !this.pnlProductPicker.Visible;
            if (this.NewSkuField.Text.Trim().Length > 0)
            {
                if (this.pnlProductPicker.Visible == true)
                {
                    this.ProductPicker1.Keyword = this.NewSkuField.Text;
                    this.ProductPicker1.LoadSearch();
                }
            }
        }

        private void AddProductBySku()
        {
            this.MessageBox1.ClearMessage();
            if (this.NewSkuField.Text.Trim().Length < 1)
            {
                this.MessageBox1.ShowWarning("Please enter a sku first.");
            }
            else
            {
                Product p = MTApp.CatalogServices.Products.FindBySku(this.NewSkuField.Text.Trim());
                if (p != null)
                {
                    if ((p.Sku == string.Empty))
                    {
                        this.MessageBox1.ShowWarning("That product could not be located. Please check SKU.");
                    }
                    else
                    {
                        ProductFile file = MTApp.CatalogServices.ProductFiles.Find((string)ViewState["id"]);
                        if (file != null)
                        {
                            if (MTApp.CatalogServices.ProductFiles.AddAssociatedProduct(file.Bvin, p.Bvin, 0, 0))
                            {
                                this.MessageBox1.ShowOk("Product Added!");
                            }
                            else
                            {
                                this.MessageBox1.ShowError("Product was not added correctly. Unknown Error");
                            }
                        }
                    }
                }
            }
            BindProductsGrid();
        }

        protected void btnAddProductBySku_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            AddProductBySku();
        }

        protected void btnProductPickerCancel_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            this.MessageBox1.ClearMessage();
            this.pnlProductPicker.Visible = false;
        }

        protected void btnProductPickerOkay_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            this.MessageBox1.ClearMessage();
            if (this.ProductPicker1.SelectedProducts.Count > 0)
            {
                Product p = MTApp.CatalogServices.Products.Find(this.ProductPicker1.SelectedProducts[0]);
                if (p != null)
                {
                    this.NewSkuField.Text = p.Sku;
                }
                AddProductBySku();
                this.pnlProductPicker.Visible = false;
            }
            else
            {
                this.MessageBox1.ShowWarning("Please select a product first.");
            }
        }

        protected void ProductsGridView_RowUpdating(object sender, System.Web.UI.WebControls.GridViewUpdateEventArgs e)
        {
            if (Page.IsValid)
            {
                string key = (string)ProductsGridView.DataKeys[e.RowIndex].Value;
                ProductFile file = MTApp.CatalogServices.ProductFiles.FindByBvinAndProductBvin((string)ViewState["id"], key);

                GridViewRow row = ProductsGridView.Rows[e.RowIndex];
                TextBox tb = (TextBox)row.FindControl("MaxDownloadsTextBox");
                BVAdmin_Controls_TimespanPicker tp = (BVAdmin_Controls_TimespanPicker)row.FindControl("TimespanPicker");

                if (tb != null)
                {
                    int val = 0;
                    if (int.TryParse(tb.Text, out val))
                    {
                        file.MaxDownloads = val;
                    }
                    else
                    {
                        file.MaxDownloads = 0;
                    }
                }

                if (tp != null)
                {
                    file.SetMinutes(tp.Months, tp.Days, tp.Hours, tp.Minutes);
                }

                if (MTApp.CatalogServices.ProductFiles.Update(file))
                {
                    MessageBox1.ShowOk("File was successfully updated!");
                }
                else
                {
                    MessageBox1.ShowError("File update failed. Unknown error.");
                }
                BindProductsGrid();
            }
        }

        protected void ProductsGridView_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string key = (string)ProductsGridView.DataKeys[e.Row.RowIndex].Value;
                ProductFile file = MTApp.CatalogServices.ProductFiles.FindByBvinAndProductBvin((string)ViewState["id"], key);

                TextBox tb = (TextBox)e.Row.FindControl("MaxDownloadsTextBox");
                BVAdmin_Controls_TimespanPicker tp = (BVAdmin_Controls_TimespanPicker)e.Row.FindControl("TimespanPicker");

                if (tb != null)
                {
                    tb.Text = file.MaxDownloads.ToString();
                }

                if (tp != null)
                {
                    int minutes = file.AvailableMinutes;
                    tp.Months = minutes / 43200;
                    minutes = minutes % 43200;
                    tp.Days = minutes / 1440;
                    minutes = minutes % 1440;
                    tp.Hours = minutes / 60;
                    minutes = minutes % 60;
                    tp.Minutes = minutes;
                }
            }
        }

        protected void SaveImageButton_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            ProductFile file = MTApp.CatalogServices.ProductFiles.Find((string)ViewState["id"]);
            if (file != null)
            {
                file.ShortDescription = DescriptionTextBox.Text;
                if (MTApp.CatalogServices.ProductFiles.Update(file))
                {
                    MessageBox1.ShowOk("File updated!");
                }
                else
                {
                    MessageBox1.ShowError("An error occurred while trying to update the file");
                }
            }
        }

        protected void CancelImageButton_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            Response.Redirect("~/BVAdmin/Catalog/FileVault.aspx");
        }

        protected void FileReplaceCancelImageButton_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            FilePicker1.Clear();
            ReplacePanel.Visible = false;
        }

        protected void FileReplaceSaveImageButton_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            ProductFile file = MTApp.CatalogServices.ProductFiles.Find((string)ViewState["id"]);
            if (file != null)
            {
                FilePicker1.DownloadOrLinkFile(file, MessageBox1);
                ReplacePanel.Visible = false;
                PopulateFileInfo();
            }
            else
            {
                MessageBox1.ShowError("An error occurred while trying to save the file.");
            }
        }

        protected void ReplaceImageButton_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            ReplacePanel.Visible = true;
        }

        protected void btnCloseVariants_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            this.MessageBox1.ClearMessage();
            this.pnlProductChoices.Visible = false;
        }

        //Protected Sub btnAddVariant_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnAddVariant.Click
        //Me.MessageBox1.ClearMessage()
        //Dim p As Catalog.Product = Me.VariantsDisplay1.GetSelectedProduct(Nothing)
        //If p IsNot Nothing Then
        //If Not VariantsDisplay1.IsValidCombination Then
        //Me.MessageBox1.ShowError("This is not a valid combination")
        //Else
        //If p IsNot Nothing Then
        //Dim file As Catalog.ProductFile = Catalog.ProductFile.FindByBvin(ViewState("id"))
        //If file IsNot Nothing Then
        //If Catalog.ProductFile.AddAssociatedProduct(file.Bvin, p.Bvin, 0, 0) Then
        //Me.MessageBox1.ShowOk("Product Added!")
        //Else
        //Me.MessageBox1.ShowError("Product was not added correctly. Unknown Error")
        //End If
        //End If
        //End If
        //Me.pnlProductChoices.Visible = False
        //BindProductsGrid()
        //'Me.pnlProductPicker.Visible = False
        //End If
        //End If
        //End Sub
    }
}