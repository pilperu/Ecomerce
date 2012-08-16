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

namespace MerchantTribeStore
{

    public partial class BVAdmin_Catalog_ProductsEdit_Tabs : BaseProductAdminPage
    {
        private string productBvin = string.Empty;
        private Product localProduct = new Product();

        protected override bool Save()
        {
            return true;
        }

        protected override void OnInit(System.EventArgs e)
        {
            base.OnInit(e);

            this.PageTitle = "Edit Product Tabs";
            this.CurrentTab = AdminTabType.Catalog;
            ValidateCurrentUserHasPermission(SystemPermissions.CatalogView);
        }

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);
            if (Request.QueryString["id"] != null)
            {
                productBvin = Request.QueryString["id"];
                localProduct = MTApp.CatalogServices.Products.Find(productBvin);
            }

            Page.ClientScript.RegisterClientScriptBlock(typeof(System.Web.UI.Page), "tabsort", RenderJQuery(productBvin), true);

            if (!Page.IsPostBack)
            {
                this.CurrentTab = AdminTabType.Catalog;
                LoadItems();
            }
        }

        private string RenderJQuery(string productBvin)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("$(document).ready(function() {");

            sb.Append("$(\"#dragitem-list\").sortable({");
            sb.Append("placeholder:  'ui-state-highlight',");
            sb.Append("axis:   'y',");
            sb.Append("opacity:  '0.75',");
            sb.Append("cursor:  'move',");
            sb.Append("update: function(event, ui) {");
            sb.Append(" var sorted = $(this).sortable('toArray');");
            sb.Append(" sorted += '';");
            sb.Append("$.post('ProductsEdit_TabsSort.aspx',");
            sb.Append("  { \"ids\": sorted,");
            sb.Append("    \"bvin\": \"" + productBvin + "\"");
            sb.Append("  });");
            sb.Append(" }");
            sb.Append("});");

            sb.Append("$('#dragitem-list').disableSelection();");

            sb.Append("$('.trash').click(function() {");
            sb.Append(" RemoveItem($(this));");
            sb.Append(" return false;");
            sb.Append("});");

            sb.Append("});");
            // End of Document Ready

            sb.Append("function RemoveItem(lnk) {");
            sb.Append("  var id = $(lnk).attr('id');");
            sb.Append("$.post('ProductsEdit_TabsDelete.aspx',");
            sb.Append("  { \"id\": id.replace('rem', ''),");
            sb.Append("  \"bvin\": \"" + productBvin + "\"");
            sb.Append("  },");
            sb.Append("  function() {");
            sb.Append("lnk.parent().parent().parent().parent().parent().slideUp('slow', function() {");
            sb.Append("lnk.parent().parent().parent().parent().parent().remove();});");
            sb.Append("  }");
            sb.Append(" );");
            sb.Append("}");

            return sb.ToString();
        }

        private void LoadItems()
        {
            RenderItems(localProduct.Tabs);
        }

        private void RenderItems(List<ProductDescriptionTab> tabs)
        {
            if (tabs == null)
            {
                this.litResults.Text = "This product does not have any tabs yet. Click &quot;New&quot; to create one.";
                return;
            }
            StringBuilder sb = new StringBuilder();

            sb.Append("<div id=\"dragitem-list\">");
            foreach (ProductDescriptionTab tab in tabs.OrderBy(y => y.SortOrder))
            {
                RenderSingleItem(sb, tab);
            }
            sb.Append("</div>");
            this.litResults.Text = sb.ToString();
        }

        private void RenderSingleItem(StringBuilder sb, ProductDescriptionTab t)
        {

            string destinationLink = "ProductsEdit_TabsEdit.aspx?tid=" + t.Bvin + "&id=" + productBvin;

            sb.Append("<div class=\"dragitem\" id=\"" + t.Bvin + "\"><table class=\"formtable\" width=\"100%\"><tr>");
            sb.Append("<td><a href=\"" + destinationLink + "\">");
            sb.Append(t.TabTitle);
            sb.Append("</a></td>");
            sb.Append("<td width=\"75\"><a href=\"" + destinationLink + "\"><img src=\"../images/buttons/edit.png\" alt=\"edit\" /></a></td>");
            sb.Append("<td width=\"30\"><a href=\"#\" class=\"trash\" id=\"rem" + t.Bvin + "\"");
            sb.Append("><img src=\"../../images/system/trashcan.png\" alt=\"Delete\" /></a></td>");
            sb.Append("<td width=\"30\"><a href=\"#\" class=\"handle\"><img src=\"../../images/system/draghandle.png\" alt=\"Move\" /></a></td>");
            sb.Append("</tr></table></div>");
        }

        protected void NewTabButton_Click(object sender, ImageClickEventArgs e)
        {
            this.MessageBox1.ClearMessage();

            ProductDescriptionTab t = new ProductDescriptionTab();
            t.Bvin = System.Guid.NewGuid().ToString().Replace("{", "").Replace("}", "");

            if (localProduct.Tabs.Count > 0)
            {
                var m = (from sort in localProduct.Tabs
                         select sort.SortOrder).Max();
                t.SortOrder = m + 1;
            }
            else
            {
                t.SortOrder = 1;
            }

            localProduct.Tabs.Add(t);
            if (MTApp.CatalogServices.ProductsUpdateWithSearchRebuild(localProduct))
            {
                Response.Redirect("ProductsEdit_TabsEdit.aspx?tid=" + t.Bvin + "&id=" + localProduct.Bvin);
            }
            else
            {
                this.MessageBox1.ShowError("Unable to update product tabs.");
            }

            localProduct = MTApp.CatalogServices.Products.Find(localProduct.Bvin);
            LoadItems();
        }
    }
}