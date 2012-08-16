using System.Collections.ObjectModel;
using System.Text;
using System.Web.UI;
using MerchantTribe.Commerce.Catalog;
using System.Collections.Generic;

namespace MerchantTribeStore
{

    partial class BVAdmin_Catalog_Categories_ManualSelection : BaseAdminPage
    {

        public string CategoryBvin = string.Empty;

        private bool _displayButtons = true;

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);

            if (!Page.IsPostBack)
            {
                if (Request.QueryString["id"] != null)
                {
                    this.BvinField.Value = Request.QueryString["id"];
                    if (Request.QueryString["type"] != null)
                    {
                        ViewState["type"] = Request.QueryString["type"];
                    }
                    this.lnkBack.NavigateUrl = "~/BVAdmin/Catalog/Categories_Edit.aspx?id=" + this.BvinField.Value;
                }

                else
                {
                    // No bvin so send back to categories page
                    Response.Redirect("Categories.aspx");
                }
            }

            LoadCategory();
            CategoryBvin = this.BvinField.Value;
        }

        protected override void OnPreInit(System.EventArgs e)
        {
            base.OnPreInit(e);
            this.PageTitle = "Edit Products for Category";
            this.CurrentTab = AdminTabType.Catalog;
        }

        private void LoadCategory()
        {
            Category c;
            c = MTApp.CatalogServices.Categories.Find(this.BvinField.Value);
            if (c != null)
            {
                if (c.Bvin != string.Empty)
                {
                    if (c.DisplaySortOrder == CategorySortOrder.ManualOrder || c.DisplaySortOrder == CategorySortOrder.None)
                    {
                        _displayButtons = true;
                    }
                    else
                    {
                        _displayButtons = false;
                    }


                    List<Product> allProducts = MTApp.CatalogServices.FindProductForCategoryWithSort(c.Bvin, c.DisplaySortOrder, true);

                    this.litProducts.Text = RenderProductList(allProducts);

                    // Exclude existing products from search results
                    if (!Page.IsPostBack)
                    {
                        this.ProductPicker1.ExcludeCategoryBvin = c.Bvin;
                    }

                }
            }
        }

        protected void btnAdd_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            this.msg.ClearMessage();
            if (this.ProductPicker1.SelectedProducts.Count == 0)
            {
                msg.ShowInformation("Please select products to add first.");
            }
            foreach (string s in this.ProductPicker1.SelectedProducts)
            {
                MTApp.CatalogServices.CategoriesXProducts.AddProductToCategory(s, this.BvinField.Value);
            }
            LoadCategory();
            this.ProductPicker1.LoadSearch();
        }

        private string RenderProductList(List<Product> products)
        {
            string result = string.Empty;

            StringBuilder sb = new StringBuilder();

            if ((products != null))
            {
                foreach (Product p in products)
                {
                    RenderSingleProduct(p, sb);
                }
            }

            result = sb.ToString();

            return result;
        }
        private void RenderSingleProduct(Product p, StringBuilder sb)
        {

            sb.Append("<div class=\"dragitem\" id=\"" + p.Bvin + "\">");

            sb.Append("<table border=\"0\" cellspacing=\"0\" cellpadding=\"2\" width=\"100%\">");
            sb.Append("<tr>");
            sb.Append("<td width=\"25%\">" + p.Sku + "</td>");
            sb.Append("<td width=\"50%\">" + p.ProductName + "</td>");
            sb.Append("<td><a href=\"Categories_RemoveProducts.aspx\" title=\"Remove Product\" id=\"rem" + p.Bvin + "\" class=\"trash\"><img src=\"../../images/system/trashcan.png\" alt=\"Remove Product\" border=\"0\" /></a></td>");
            sb.Append("<td class=\"handle\" align=\"right\"><img src=\"../../images/system/draghandle.png\" alt=\"sort\" border=\"0\" /></td>");
            sb.Append("</tr>");
            sb.Append("</table>");
            sb.Append("</div>");

        }

    }
}