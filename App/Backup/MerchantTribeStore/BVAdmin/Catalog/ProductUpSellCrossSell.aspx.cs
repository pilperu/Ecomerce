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

    partial class BVAdmin_Catalog_ProductUpSellCrossSell : BaseProductAdminPage
    {

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.PageTitle = "Product Cross Sells/Up Sells";
            this.CurrentTab = AdminTabType.Catalog;
            ValidateCurrentUserHasPermission(SystemPermissions.CatalogView);
        }


        protected override void OnLoad(EventArgs e)
        {
            
            base.OnLoad(e);
            
            MessageBox1.ClearMessage();

            // Register jQuery
            Page.ClientScript.RegisterClientScriptBlock(typeof(System.Web.UI.Page), "productrelationshipsort", RenderJQuery(), true);
            //Page.ClientScript.RegisterClientScriptBlock(typeof(System.Web.UI.Page), "optionitemsdialog", RenderDialogJquery(), true);

            RenderItems();
            
        }

        private void RenderItems()
        {

            string productBvin = Request.QueryString["id"];

            List<ProductRelationship> related = MTApp.CatalogServices.ProductRelationships.FindForProduct(productBvin);

            StringBuilder sb = new StringBuilder();
            sb.Append("<div class=\"related-items-list\">");

            foreach (ProductRelationship pr in related)
            {
                sb.Append(RenderSingleItem(pr));
            }

            sb.Append("</div>");
            this.litItems.Text = sb.ToString();
        }

        private string RenderSingleItem(ProductRelationship r)
        {
            StringBuilder sb = new StringBuilder();

            string name = r.RelatedProductId;
            Product p = MTApp.CatalogServices.Products.Find(r.RelatedProductId);
            if (p != null)
            {
                name = p.Sku + "<br />" + p.ProductName;

                string imageUrl = MerchantTribe.Commerce.Storage.DiskStorage.ProductImageUrlSmall(MTApp, p.Bvin, p.ImageFileSmall, true);

                sb.Append("<div class=\"dragitem\" id=\"item" + r.RelatedProductId.ToString() + "\"><table class=\"formtable\" width=\"100%\"><tr>");
                sb.Append("<td width=\"60\" class=\"imgfield\">");
                sb.Append("<img width=\"50\" src=\"" + imageUrl + "\" border=\"0\" alt=\"" + p.ImageFileSmallAlternateText + "\" /></td>");

                sb.Append("<td class=\"namefield\" style=\"line-height:1.5em;\">");
                sb.Append(name);
                sb.Append("</td>");

                sb.Append("<td width=\"75\" class=\"substitutefield\">");
                if (r.IsSubstitute)
                {
                    sb.Append("SUB");
                }
                else
                {
                    sb.Append("&nbsp;");
                }
                sb.Append("</td>");

                // Disable Editing for Now
                //sb.Append("<td width=\"75\"><a href=\"#\" class=\"dragitemedit\" id=\"edit" + r.RelatedProductId.ToString() + "\"><img src=\"../images/buttons/edit.png\" alt=\"edit\" /></a></td>");


                sb.Append("<td width=\"30\"><a href=\"#\" class=\"trash\" id=\"rem" + r.RelatedProductId.ToString() + "\"><img src=\"../../images/system/trashcan.png\" alt=\"Delete\" /></a></td>");
                sb.Append("<td width=\"30\"><a href=\"#\" class=\"handle\"><img src=\"../../images/system/draghandle.png\" alt=\"Move\" /></a></td>");
                sb.Append("</tr></table></div>");

            }
            return sb.ToString();
        }

        private string RenderJQuery()
        {
            string productBvin = Request.QueryString["id"];

            StringBuilder sb = new StringBuilder();

            sb.Append("$(document).ready(function() {");

            sb.Append("$(\".related-items-list\").sortable({");
            sb.Append("placeholder:  'ui-state-highlight',");
            sb.Append("axis:   'y',");
            sb.Append("opacity:  '0.75',");
            sb.Append("cursor:  'move',");
            sb.Append("update: function(event, ui) {");
            sb.Append(" var sorted = $(this).sortable('toArray');");
            sb.Append(" sorted += '';");
            sb.Append("$.post('RelatedItems_Sort.aspx',");           
            sb.Append("  { \"ids\": sorted,");
            sb.Append("    productid: '" + productBvin + "'");
            sb.Append("  });");
            sb.Append(" }");
            sb.Append("});");


            // Dialog Functions
            sb.Append("$(\".dragitemedit\").click(function() {");
            sb.Append("OpenDialog($(this));");
            sb.Append("});");

            sb.Append("$(\"#dialog-close\").click(function() {");
            sb.Append("CloseDialog();");
            sb.Append("return false;");
            sb.Append("});");

            sb.Append("$(\"#dialog-save\").click(function() {");
            sb.Append("SaveDialog();");
            sb.Append("return false;");
            sb.Append("});");


            sb.Append("$('.related-items-list').disableSelection();");

            sb.Append("$('.trash').click(function() {");
            sb.Append("RemoveItem($(this)); return false;");
            sb.Append("});");

            sb.Append("});");
            // End of Document Ready

            sb.Append("function RemoveItem(lnk) {");
            sb.Append("  var id = $(lnk).attr('id');");
            sb.Append("$.post('RelatedItems_Delete.aspx',");
            sb.Append("  { \"id\": id.replace('rem', ''),");
            sb.Append("  \"productid\": \"" + productBvin + "\"");
            sb.Append("  },");
            sb.Append("  function() {");
            sb.Append("lnk.parent().parent().parent().parent().parent().slideUp('slow', function() {");
            sb.Append("lnk.parent().parent().parent().parent().parent().remove();});");
            sb.Append("  }");
            sb.Append(" );");
            sb.Append("}");

            return sb.ToString();
        }

        protected override bool Save()
        {
            return true;
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            string productBvin = Request.QueryString["id"];

            StringCollection selectedBvins = this.UpSellsProductPicker.SelectedProducts;
            foreach (string s in selectedBvins)
            {
                MTApp.CatalogServices.ProductRelationships.RelateProducts(productBvin, s, false);
            }

            RenderItems();
        }
    }
}