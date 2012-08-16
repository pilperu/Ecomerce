using System.Text;
using System.Web.UI;
using System.Collections.Generic;
using MerchantTribe.Commerce;
using MerchantTribe.Commerce.Catalog;

namespace MerchantTribeStore
{

    partial class BVAdmin_Catalog_ProductVariants_List : BaseAdminJsonPage
    {

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);
            if (!Page.IsPostBack)
            {
                string productId = Request["id"];
                Product p = MTApp.CatalogServices.Products.Find(productId);
                RenderVariants(p);
            }
        }

        private void RenderVariants(Product p)
        {
            StringBuilder sb = new StringBuilder();
            int renderedCount = 0;

            List<OptionSelectionList> possibleVariants = MTApp.CatalogServices.VariantsGenerateAllPossibleSelections(p.Options);

            foreach (OptionSelectionList possible in possibleVariants)
            {
                string possibleKey = OptionSelection.GenerateUniqueKeyForSelections(possible);
                Variant v = p.Variants.FindByKey(possibleKey);
                if (v != null)
                {
                    RenderSingleVariant(sb, p.Options.VariantsOnly(), v, p);
                    renderedCount += 1;
                }
            }

            if ((renderedCount >= MerchantTribe.Commerce.WebAppSettings.MaxVariants))
            {
                this.litOutput.Text = "<div class=\"flash-message-warning\">This product has " + renderedCount + " variants. The maximum allowed is " + WebAppSettings.MaxVariants + ". If you need more variants try creating separate products with fewer choices that affect Pictures, Sku and Price.</div>";
                this.litOutput.Text += sb.ToString();
            }
            else
            {
                this.litOutput.Text = sb.ToString();
            }

        }

        private void RenderSingleVariant(StringBuilder sb, List<Option> options, Variant v, Product p)
        {

            sb.Append("<div id=\"" + v.Bvin + "\"><table class=\"formtable\" width=\"100%\"><tr>");
            sb.Append("<td>");
            foreach (string s in v.SelectionNames(options))
            {
                sb.Append(s + ", ");
            }
            sb.Append("</td>");

            // Image
            sb.Append("<td width=\"75\">");
            sb.Append("<img width=\"50\" src=\"");
            sb.Append(MerchantTribe.Commerce.Storage.DiskStorage.ProductVariantImageUrlMedium(this.MTApp, p.Bvin, p.ImageFileSmall, v.Bvin, true));
            sb.Append("\" border=\"0\" />");
            sb.Append("</td>");

            // Price
            sb.Append("<td width=\"75\">");
            if ((v.Price >= 0))
            {
                sb.Append(v.Price.ToString("C"));
            }
            else
            {
                sb.Append(p.SitePrice.ToString("C"));
            }
            sb.Append("</td>");

            // SKU
            sb.Append("<td width=\"75\">");
            if ((v.Sku.Trim() != string.Empty))
            {
                sb.Append(v.Sku);
            }
            else
            {
                sb.Append(p.Sku);
            }
            sb.Append("</td>");

            // Delete
            sb.Append("<td width=\"30\"><a href=\"#\" class=\"trash\" id=\"rem" + v.Bvin + "\"");
            sb.Append("><img src=\"../../images/system/trashcan.png\" alt=\"Delete\" /></a></td>");

            // Edit
            sb.Append("<td width=\"75\"><a href=\"#\" class=\"edit\" id=\"edit" + v.Bvin + "\"><img src=\"../images/buttons/edit.png\" alt=\"edit\" /></a></td>");

            sb.Append("</tr></table></div>");
        }

    }
}