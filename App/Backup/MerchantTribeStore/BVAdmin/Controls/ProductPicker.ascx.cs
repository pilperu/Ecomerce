using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Text;
using System.Web.UI.WebControls;
using MerchantTribe.Commerce;
using MerchantTribe.Commerce.Catalog;
using MerchantTribe.Commerce.Content;

namespace MerchantTribeStore
{

    partial class BVAdmin_Controls_ProductPicker : BVUserControl
    {
        public int CurrentPage
        {
            get
            {
                int result = 1;
                int temp = 0;
                if (int.TryParse(this.currentpagefield.Value, out temp))
                {
                    return temp;
                }
                return result;
            }
            set
            {
                this.currentpagefield.Value = value.ToString();
            }
        }
        public string Keyword
        {
            get { return this.FilterField.Text.Trim(); }
            set { this.FilterField.Text = value; }
        }
        protected bool IsInitialized
        {
            get
            {
                object obj = ViewState["IsInitialized"];
                if (obj != null)
                {
                    return (bool)obj;
                }
                else
                {
                    return false;
                }
            }
            set { ViewState["IsInitialized"] = value; }
        }
        public bool IsMultiSelect
        {
            get
            {
                object obj = ViewState["IsMultiSelect"];
                if (obj != null)
                {
                    return (bool)obj;
                }
                else
                {
                    return true;
                }
            }
            set { ViewState["IsMultiSelect"] = value; }
        }
        public bool DisplayPrice
        {
            get
            {
                object obj = ViewState["DisplayPrice"];
                if (obj != null)
                {
                    return (bool)obj;
                }
                else
                {
                    return false;
                }
            }
            set { ViewState["DisplayPrice"] = value; }
        }
        public bool DisplayInventory
        {
            get
            {
                object obj = ViewState["DisplayInventory"];
                if (obj != null)
                {
                    return (bool)obj;
                }
                else
                {
                    return false;
                }
            }
            set { ViewState["DisplayInventory"] = value; }
        }
        public bool DisplayKits
        {
            get
            {
                object obj = ViewState["DisplayKits"];
                if (obj != null)
                {
                    return (bool)obj;
                }
                else
                {
                    return false;
                }
            }
            set { ViewState["DisplayKits"] = value; }
        }

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);

            // Register jQuery
            Page.ClientScript.RegisterClientScriptBlock(typeof(System.Web.UI.Page), "productpicker", RenderJQuery(), true);

            GridView1.PageSize = int.Parse(DropDownList1.SelectedValue);
            if (!this.IsInitialized)
            {
                PopulateCategories();
                PopulateManufacturers();
                PopulateVendors();
                this.IsInitialized = true;
                if (!Page.IsPostBack)
                {
                    CurrentPage = 1;
                    PopulateFilterFields();
                    RunSearch();
                }
            }
        }

        public void PopulateFilterFields()
        {
            this.FilterField.Text = SessionManager.AdminProductCriteriaKeyword.Trim();
            SetListToValue(this.CategoryFilter, SessionManager.AdminProductCriteriaCategory);
            SetListToValue(this.ManufacturerFilter, SessionManager.AdminProductCriteriaManufacturer);
            SetListToValue(this.VendorFilter, SessionManager.AdminProductCriteriaVendor);
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

        public string ExcludeCategoryBvin
        {
            get { return this.ExcludeCategoryBvinField.Value; }
            set { this.ExcludeCategoryBvinField.Value = value; }
        }

        private void PopulateCategories()
        {
            Collection<System.Web.UI.WebControls.ListItem> tree = MerchantTribe.Commerce.Catalog.Category.ListFullTreeWithIndents(MyPage.MTApp.CurrentRequestContext);
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

        protected void btnGo_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            RunSearch();
        }

        private ProductSearchCriteria GetCurrentCriteria()
        {
            MerchantTribe.Commerce.Catalog.ProductSearchCriteria c = new MerchantTribe.Commerce.Catalog.ProductSearchCriteria();

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
            if (this.ExcludeCategoryBvin.Trim().Length > 0)
            {
                c.NotCategoryId = this.ExcludeCategoryBvin.Trim();
            }
            c.DisplayInactiveProducts = true;
            return c;
        }

        public void RunSearch()
        {
            CurrentPage = 1;
            LoadSearch();
        }

        public void LoadSearch()
        {
            int totalCount = 0;
            this.GridView1.PageIndex = 0;
            this.GridView1.DataSource = MyPage.MTApp.CatalogServices.Products.FindByCriteria(GetCurrentCriteria(),
                                                                                        CurrentPage,
                                                                                        this.GridView1.PageSize,
                                                                                        ref totalCount);
            this.GridView1.DataBind();
            this.lstPage.Items.Clear();
            int totalPages = MerchantTribe.Web.Paging.TotalPages(totalCount, this.GridView1.PageSize);
            for (int i = 1; i <= totalPages; i++)
            {
                System.Web.UI.WebControls.ListItem li = new System.Web.UI.WebControls.ListItem(i.ToString(), i.ToString());
                if (i == CurrentPage)
                {
                    li.Selected = true;
                }
                this.lstPage.Items.Add(li);
            }
            SessionManager.AdminProductCriteriaKeyword = this.FilterField.Text.Trim();
            SessionManager.AdminProductCriteriaCategory = this.CategoryFilter.SelectedValue;
            SessionManager.AdminProductCriteriaManufacturer = this.ManufacturerFilter.SelectedValue;
            SessionManager.AdminProductCriteriaVendor = this.VendorFilter.SelectedValue;
        }

        public StringCollection SelectedProducts
        {
            get
            {
                StringCollection result = new StringCollection();

                if (this.IsMultiSelect)
                {
                    for (int i = 0; i <= this.GridView1.Rows.Count - 1; i++)
                    {
                        if (GridView1.Rows[i].RowType == DataControlRowType.DataRow)
                        {
                            CheckBox chkSelected = (CheckBox)this.GridView1.Rows[i].Cells[0].FindControl("chkSelected");
                            if (chkSelected != null)
                            {
                                if (chkSelected.Checked == true)
                                {
                                    result.Add((string)GridView1.DataKeys[GridView1.Rows[i].RowIndex].Value);
                                }
                            }
                        }
                    }
                }
                else
                {
                    string val = (string)Request.Form[this.GridView1.ClientID + "CheckBoxSelected"];
                    if (val != null)
                    {
                        if (val != string.Empty)
                        {
                            result.Add(val);
                        }
                    }
                }
                return result;
            }
        }


        protected void ManufacturerFilter_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            CurrentPage = 1;
            LoadSearch();
        }

        protected void VendorFilter_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            LoadSearch();
        }

        protected void CategoryFilter_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            CurrentPage = 1;
            LoadSearch();
        }

        protected void DropDownList1_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            CurrentPage = 1;
            GridView1.PageSize = int.Parse(DropDownList1.SelectedValue);
            LoadSearch();
        }

        protected void GridView1_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (!this.IsMultiSelect)
                {
                    ((CheckBox)e.Row.FindControl("chkSelected")).Visible = false;
                    ((Literal)e.Row.FindControl("radioButtonLiteral")).Text = "<input name='" + this.GridView1.ClientID + "CheckBoxSelected' type='radio' value='" + GridView1.DataKeys[e.Row.RowIndex].Value + "' />";
                }

                Label PriceLabel = (Label)e.Row.FindControl("PriceLabel");
                Label InventoryLabel = (Label)e.Row.FindControl("InventoryLabel");
                if (this.DisplayPrice)
                {
                    PriceLabel.Text = ((MerchantTribe.Commerce.Catalog.Product)e.Row.DataItem).SitePrice.ToString("c");
                }

                if (this.DisplayInventory)
                {
                    Product prod = (Product)e.Row.DataItem;
                    if (prod.IsAvailableForSale)
                    {
                        InventoryLabel.Text = "Available for Sale";
                    }
                }
            }
            else if (e.Row.RowType == DataControlRowType.Header)
            {
                if (this.DisplayPrice)
                {
                    e.Row.Cells[3].Text = "Site Price";
                }

                if (this.DisplayInventory)
                {
                    e.Row.Cells[4].Text = "Available Qty";
                }

            }
        }

        protected void lstPage_SelectedIndexChanged(object sender, EventArgs e)
        {
            int temp = 1;
            if (int.TryParse(this.lstPage.SelectedItem.Value, out temp))
            {
                CurrentPage = temp;
            }
            else
            {
                CurrentPage = 1;
            }
            LoadSearch();
        }

        private string RenderJQuery()
        {
            string productBvin = Request.QueryString["id"];

            StringBuilder sb = new StringBuilder();
            
            sb.Append("$(document).ready(function() {");

            sb.Append("$(\".pickerallbutton\").click(function() {");

            sb.Append(" if ($(\".pickerallbutton\").html() == 'All') {");

            sb.Append("$(\".pickercheck > input\").attr('checked', true);");
            sb.Append("$(\".pickerallbutton\").html('None');");

            sb.Append(" } else { ");

            sb.Append("$(\".pickercheck > input\").attr('checked', false);");
            sb.Append("$(\".pickerallbutton\").html('All');");

            sb.Append(" } ");
            
            sb.Append("return false;");
            sb.Append("});");

            sb.Append("});");
            // End of Document Ready

            return sb.ToString();
        }
    }
}