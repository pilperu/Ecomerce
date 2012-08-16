using System;
using System.IO;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Collections.Specialized;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using MerchantTribe.Commerce;
using MerchantTribe.Commerce.Catalog;
using MerchantTribe.Commerce.Membership;
using MerchantTribe.Commerce.Metrics;
using MerchantTribe.Commerce.Utilities;

namespace MerchantTribeStore
{

    partial class BVAdmin_Catalog_Products_Edit_Inventory : BaseProductAdminPage
    {

        Product localProduct = null;

        protected override void OnInit(System.EventArgs e)
        {
            base.OnInit(e);
            this.PageTitle = "Edit Product Inventory";
            this.CurrentTab = AdminTabType.Catalog;
            ValidateCurrentUserHasPermission(SystemPermissions.CatalogView);

            if (Request.QueryString["id"] != null)
            {
                this.bvinfield.Value = Request.QueryString["id"];
                localProduct = MTApp.CatalogServices.Products.Find(this.bvinfield.Value);
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (!Page.IsPostBack)
            {
                this.CurrentTab = AdminTabType.Catalog;
                LoadInventory();
                SetOutOfStockMode();
            }
        }

        private void SetOutOfStockMode()
        {
            if (localProduct != null)
            {
                if (this.OutOfStockModeField.Items.FindByValue(((int)localProduct.InventoryMode).ToString()) != null)
                {
                    this.OutOfStockModeField.ClearSelection();
                    this.OutOfStockModeField.Items.FindByValue(((int)localProduct.InventoryMode).ToString()).Selected = true;
                }
            }
        }

        private void LoadInventory()
        {
            List<ProductInventory> inventory = new List<ProductInventory>();                        
            MTApp.CatalogServices.InventoryGenerateForProduct(localProduct);
            MTApp.CatalogServices.CleanUpInventory(localProduct);
            MTApp.CatalogServices.UpdateProductVisibleStatusAndSave(localProduct);
            inventory = MTApp.CatalogServices.ProductInventories.FindByProductId(this.bvinfield.Value);
            
            if (localProduct.IsAvailableForSale)
            {
                this.lblIsAvailable.Text = "<div class=\"flash-message-success\">This product is displayed on the store based on inventory.</div>";
            }
            else
            {
                this.lblIsAvailable.Text = "<div class=\"flash-message-warning\">This product is NOT displayed on the store based on inventory.</div>";
            }

            this.EditsGridView.DataSource = inventory;
            this.EditsGridView.DataBind();
        }

        protected void btnCancel_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            Response.Redirect("~/BVAdmin/Catalog/Products_Edit.aspx?id=" + Request.QueryString["id"]);
        }

        protected void btnSaveChanges_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            this.MessageBox1.ClearMessage();
            if (this.Save())
            {
                this.MessageBox1.ShowOk("Changes Saved!");
            }
            LoadInventory();
        }

        protected override bool Save()
        {
            bool result = false;

            localProduct.InventoryMode = (ProductInventoryMode)int.Parse(this.OutOfStockModeField.SelectedValue);
            MTApp.CatalogServices.ProductsUpdateWithSearchRebuild(localProduct);

            // Process each variant/product row
            foreach (GridViewRow row in this.EditsGridView.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    ProcessRow(row);
                }
            }

            MTApp.CatalogServices.UpdateProductVisibleStatusAndSave(localProduct);            
            result = true;
            return result;
        }

        private void ProcessRow(GridViewRow row)
        {
            string inventoryBvin = (string)this.EditsGridView.DataKeys[row.RowIndex].Value;
            ProductInventory localInventory = MTApp.CatalogServices.ProductInventories.Find(inventoryBvin);

            TextBox AdjustmentField = (TextBox)row.FindControl("AdjustmentField");
            DropDownList AdjustmentModeField = (DropDownList)row.FindControl("AdjustmentModeField");
            TextBox LowStockPointField = (TextBox)row.FindControl("LowStockPointField");

            if (LowStockPointField != null)
            {
                int temp;
                if (int.TryParse(LowStockPointField.Text, out temp))
                {
                    localInventory.LowStockPoint = temp;
                }
            }
                   
            if (AdjustmentModeField != null)
            {
                if (AdjustmentField != null)
                {
                    int qty = 0;
                    int.TryParse(AdjustmentField.Text, out qty);
                    switch (AdjustmentModeField.SelectedValue)
                    {
                        case "1":
                            // Add
                            localInventory.QuantityOnHand += qty;
                            break;
                        case "2":
                            // Subtract
                            localInventory.QuantityOnHand -= qty;
                            break;
                        case "3":
                            // Set To
                            localInventory.QuantityOnHand = qty;
                            break;
                        default:
                            // Add
                            localInventory.QuantityOnHand += qty;
                            break;
                    }
                }
            }

            MTApp.CatalogServices.ProductInventories.Update(localInventory);            
        }

        protected void EditsGridView_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ProductInventory rowP = (ProductInventory)e.Row.DataItem;
                if (rowP != null)
                {

                    string name = localProduct.ProductName;
                    Label Label1 = (Label)e.Row.FindControl("Label1");
                    if (Label1 != null) Label1.Text = name;

                    Label Label2 = (Label)e.Row.FindControl("Label2");
                    if (Label2 != null)
                    {
                        if (rowP.VariantId != string.Empty)
                        {
                            Variant v = localProduct.Variants.FindByBvin(rowP.VariantId);
                            if (v != null)
                            {
                                string variantName = "";
                                foreach (string s in v.SelectionNames(localProduct.Options.VariantsOnly()))
                                {
                                    variantName += s + ", ";
                                }
                                Label2.Text = variantName;
                            }
                        }                        
                    }

                    Label lblQuantityOnHand = (Label)e.Row.FindControl("lblQuantityOnHand");
                    Label lblQuantityReserved = (Label)e.Row.FindControl("lblQuantityReserved");
                    Label lblQuantityAvailableForSale = (Label)e.Row.FindControl("lblQuantityAvailableForSale");                    
                    TextBox LowStockPointField = (TextBox)e.Row.FindControl("LowStockPointField");

                    if (lblQuantityOnHand != null)
                    {
                        lblQuantityOnHand.Text = rowP.QuantityOnHand.ToString();                        
                    }
                    if (lblQuantityAvailableForSale != null)
                    {
                        lblQuantityAvailableForSale.Text = rowP.QuantityAvailableForSale.ToString();
                    }
                    if (lblQuantityReserved != null)
                    {
                        lblQuantityReserved.Text = rowP.QuantityReserved.ToString();
                    }
                    if (LowStockPointField != null)
                    {
                        LowStockPointField.Text = rowP.LowStockPoint.ToString();
                    }
                }
            }
        }

    }
}