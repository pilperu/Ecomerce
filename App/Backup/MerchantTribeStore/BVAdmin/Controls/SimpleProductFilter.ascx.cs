using System;
using System.Web.UI.WebControls;
using MerchantTribe.Commerce.Content;
using MerchantTribe.Commerce;
using MerchantTribe.Commerce.Catalog;
using System.Collections.ObjectModel;

namespace MerchantTribeStore
{

    partial class BVAdmin_Controls_SimpleProductFilter : BVUserControl
    {

        public delegate void FilterChangedDelegate(ProductSearchCriteria criteria, System.EventArgs e);
        public event FilterChangedDelegate FilterChanged;
        public delegate void GoPressedDelegate(ProductSearchCriteria criteria, System.EventArgs e);
        public event GoPressedDelegate GoPressed;


        protected override void OnInit(System.EventArgs e)
        {
            base.OnInit(e);

            if (!Page.IsPostBack)
            {
                PopulateFilterFields();
            }
            InventoryStatusFilter.Enabled = true;
        }

        public override void Focus()
        {
            this.FilterField.Focus();
        }

        public ProductSearchCriteria LoadProductCriteria()
        {

            ProductSearchCriteria c = new ProductSearchCriteria();

            if (this.FilterField.Text.Trim().Length > 0)
            {
                c.Keyword = this.FilterField.Text.Trim();
            }
            if (this.ManufacturerFilter.SelectedValue != "")
            {
                c.ManufacturerId = this.ManufacturerFilter.SelectedValue;
            }
            if (this.VendorFilter.SelectedValue != "")
            {
                c.VendorId = this.VendorFilter.SelectedValue;
            }
            if (this.CategoryFilter.SelectedValue != "")
            {
                c.CategoryId = this.CategoryFilter.SelectedValue;
            }
            if (this.StatusFilter.SelectedValue != "")
            {
                c.Status = (ProductStatus)int.Parse(this.StatusFilter.SelectedValue);
            }
            if (this.InventoryStatusFilter.SelectedValue != "")
            {
                c.InventoryStatus = (ProductInventoryStatus)int.Parse(this.InventoryStatusFilter.SelectedValue);
            }
            if (this.ProductTypeFilter.SelectedValue != "")
            {
                c.ProductTypeId = this.ProductTypeFilter.SelectedValue;
            }

            //If products.Count = 1 Then
            //    Me.lblResults.Text = "1 product found"
            //Else
            //    Me.lblResults.Text = products.Count & " products found"
            //End If

            // Save Setting to Session
            SessionManager.AdminProductCriteriaKeyword = this.FilterField.Text.Trim();
            SessionManager.AdminProductCriteriaCategory = this.CategoryFilter.SelectedValue;
            SessionManager.AdminProductCriteriaManufacturer = this.ManufacturerFilter.SelectedValue;
            SessionManager.AdminProductCriteriaVendor = this.VendorFilter.SelectedValue;
            SessionManager.AdminProductCriteriaStatus = this.StatusFilter.SelectedValue;
            SessionManager.AdminProductCriteriaInventoryStatus = this.InventoryStatusFilter.SelectedValue;
            SessionManager.AdminProductCriteriaProductType = this.ProductTypeFilter.SelectedValue;

            c.DisplayInactiveProducts = true;

            return c;
        }

        private void PopulateFilterFields()
        {
            PopulateCategories();
            PopulateManufacturers();
            PopulateVendors();
            PopulateStatus();
            PopulateInventoryStatus();
            PopulateProductTypes();

            this.FilterField.Text = SessionManager.AdminProductCriteriaKeyword.Trim();
            SetListToValue(this.CategoryFilter, SessionManager.AdminProductCriteriaCategory);
            SetListToValue(this.ManufacturerFilter, SessionManager.AdminProductCriteriaManufacturer);
            SetListToValue(this.VendorFilter, SessionManager.AdminProductCriteriaVendor);
            SetListToValue(this.StatusFilter, SessionManager.AdminProductCriteriaStatus);
            SetListToValue(this.InventoryStatusFilter, SessionManager.AdminProductCriteriaInventoryStatus);
            SetListToValue(this.ProductTypeFilter, SessionManager.AdminProductCriteriaProductType);
        }

        private void SetListToValue(DropDownList l, string value)
        {
            if (l != null)
            {
                if (l.Items.FindByValue(value) != null)
                {
                    l.ClearSelection();
                    l.Items.FindByValue(value).Selected = true;
                }
            }
        }

        private void PopulateCategories()
        {
            Collection<System.Web.UI.WebControls.ListItem> tree = Category.ListFullTreeWithIndents(MyPage.MTApp.CurrentRequestContext);
            this.CategoryFilter.Items.Clear();
            foreach (System.Web.UI.WebControls.ListItem li in tree)
            {
                this.CategoryFilter.Items.Add(li);
            }
            this.CategoryFilter.Items.Insert(0, new System.Web.UI.WebControls.ListItem("- Any Category -", ""));
        }

        private void PopulateManufacturers()
        {
            this.ManufacturerFilter.DataSource = MyPage.MTApp.ContactServices.Manufacturers.FindAll();
            this.ManufacturerFilter.DataTextField = "DisplayName";
            this.ManufacturerFilter.DataValueField = "Bvin";
            this.ManufacturerFilter.DataBind();
            this.ManufacturerFilter.Items.Insert(0, new System.Web.UI.WebControls.ListItem("- Any Manufacturer -", ""));
        }

        private void PopulateVendors()
        {
            this.VendorFilter.DataSource = MyPage.MTApp.ContactServices.Vendors.FindAll();
            this.VendorFilter.DataTextField = "DisplayName";
            this.VendorFilter.DataValueField = "Bvin";
            this.VendorFilter.DataBind();
            this.VendorFilter.Items.Insert(0, new System.Web.UI.WebControls.ListItem("- Any Vendor -", ""));
        }

        private void PopulateStatus()
        {
            this.StatusFilter.Items.Clear();
            this.StatusFilter.Items.Add(new System.Web.UI.WebControls.ListItem("- Any Status -", ""));
            this.StatusFilter.Items.Add(new System.Web.UI.WebControls.ListItem("Active", "1"));
            this.StatusFilter.Items.Add(new System.Web.UI.WebControls.ListItem("Disabled", "0"));
        }

        private void PopulateInventoryStatus()
        {
            this.InventoryStatusFilter.Items.Clear();
            this.InventoryStatusFilter.Items.Add(new System.Web.UI.WebControls.ListItem("- Any Inventory Status -", ""));
            this.InventoryStatusFilter.Items.Add(new System.Web.UI.WebControls.ListItem("Not Available", "0"));
            this.InventoryStatusFilter.Items.Add(new System.Web.UI.WebControls.ListItem("Available", "1"));
        }

        private void PopulateProductTypes()
        {
            this.ProductTypeFilter.Items.Clear();
            this.ProductTypeFilter.DataSource = MyPage.MTApp.CatalogServices.ProductTypes.FindAll();
            this.ProductTypeFilter.DataTextField = "ProductTypeName";
            this.ProductTypeFilter.DataValueField = "bvin";
            this.ProductTypeFilter.DataBind();
            this.ProductTypeFilter.Items.Insert(0, new System.Web.UI.WebControls.ListItem("- Any Type -", ""));
        }

        protected void ProductTypeFilter_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (FilterChanged != null)
            {
                FilterChanged(this.LoadProductCriteria(), new System.EventArgs());
            }
        }

        protected void CategoryFilter_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (FilterChanged != null)
            {
                FilterChanged(this.LoadProductCriteria(), new System.EventArgs());
            }
        }

        protected void ManufacturerFilter_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (FilterChanged != null)
            {
                FilterChanged(this.LoadProductCriteria(), new System.EventArgs());
            }
        }

        protected void VendorFilter_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (FilterChanged != null)
            {
                FilterChanged(this.LoadProductCriteria(), new System.EventArgs());
            }
        }

        protected void StatusFilter_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (FilterChanged != null)
            {
                FilterChanged(this.LoadProductCriteria(), new System.EventArgs());
            }
        }

        protected void InventoryStatusFilter_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (FilterChanged != null)
            {
                FilterChanged(this.LoadProductCriteria(), new System.EventArgs());
            }
        }

        protected void btnGo_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            if (GoPressed != null)
            {
                GoPressed(this.LoadProductCriteria(), new System.EventArgs());
            }
            this.FilterField.Focus();
        }
    }
}