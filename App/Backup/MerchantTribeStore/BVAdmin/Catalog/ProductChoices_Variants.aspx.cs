using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using MerchantTribe.Commerce.Catalog;
using MerchantTribe.Commerce.Membership;

namespace MerchantTribeStore
{

    partial class BVAdmin_Catalog_ProductChoices_Variants : BaseProductAdminPage
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

            this.PageTitle = "Edit Product Variants";
            this.CurrentTab = AdminTabType.Catalog;
            ValidateCurrentUserHasPermission(SystemPermissions.CatalogView);

            if (Request.QueryString["id"] != null)
            {
                productBvin = Request.QueryString["id"];
                localProduct = MTApp.CatalogServices.Products.Find(productBvin);

                PopulateEditorControls();
            }
        }

        private void PopulateEditorControls()
        {
            this.phLists.Controls.Clear();

            List<Option> variantOptions = localProduct.Options.VariantsOnly();

            if ((variantOptions.Count > 0))
            {
                this.btnNew.Visible = true;
                this.btnGenerateAll.Visible = true;
                foreach (Option opt in variantOptions)
                {
                    DropDownList ddl = new DropDownList();
                    ddl.ID = "new" + opt.Bvin;
                    ddl.ClientIDMode = ClientIDMode.Static;
                    foreach (OptionItem oi in opt.Items)
                    {
                        if ((oi.IsLabel == false))
                        {
                            ddl.Items.Add(new System.Web.UI.WebControls.ListItem(oi.Name, oi.Bvin));
                        }
                    }
                    phLists.Controls.Add(ddl);
                }
            }
            else
            {
                this.btnNew.Visible = false;
                this.btnGenerateAll.Visible = false;
            }
        }

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);
            Page.ClientScript.RegisterClientScriptBlock(typeof(System.Web.UI.Page), "variantscript", RenderJQuery(productBvin), true);
            this.CurrentTab = AdminTabType.Catalog;
        }

        private string RenderJQuery(string productBvin)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("$(document).ready(function() {");

            sb.Append(" LoadVariants(); ");

            sb.Append(" BindClicks(); ");

            sb.Append("$(\"#dialogclose\").click(function() {");
            sb.Append("CloseDialog();");
            sb.Append("return false;");
            sb.Append("});");

            //sb.Append("$('#updateframe').contents().find(""#dialogsave"").click(function() {")
            //sb.Append("SaveDialog();")
            //sb.Append("return false;")
            //sb.Append("});")

            sb.Append("});");
            // End of Document Ready

            sb.Append("function BindClicks() {");
            sb.Append("$('.trash').click(function() {");
            sb.Append(" RemoveItem($(this));");
            sb.Append(" return false;");
            sb.Append("});");
            sb.Append("$(\".edit\").click(function() {");
            sb.Append("OpenDialog($(this));");
            sb.Append("});");
            sb.Append("}");

            sb.Append("function RemoveItem(lnk) {");
            sb.Append("  var id = $(lnk).attr('id');");
            sb.Append("$.post('ProductVariants_Delete.aspx',");
            sb.Append("  { \"id\": id.replace('rem', ''),");
            sb.Append("  \"bvin\": \"" + productBvin + "\"");
            sb.Append("  },");
            sb.Append("  function() {");
            sb.Append("lnk.parent().parent().parent().parent().parent().slideUp('slow', function() {");
            sb.Append("lnk.parent().parent().parent().parent().parent().remove();});");
            sb.Append("  }");
            sb.Append(" );");
            sb.Append("}");

            sb.Append("function CloseDialog() {");
            sb.Append("$('.overlay').remove();");
            sb.Append("$('.modal').hide();");
            sb.Append("}");

            sb.Append("function OpenDialog(lnk) {");
            sb.Append("$('<div />').addClass('overlay').appendTo('body').show();");
            sb.Append("$('.modal').show();");
            sb.Append(" var loadid = $(lnk).attr('id');");
            sb.Append(" LoadDialog(loadid.replace('edit', ''));");
            sb.Append("}");

            sb.Append("function LoadDialog(itemid) {");
            sb.Append(" var dest = 'ProductVariants_Get.aspx?id=' + itemid;");
            sb.Append("$.getJSON(dest,'', function(data) {");
            sb.Append("$('#dialogbvin').val(data.Bvin);");
            sb.Append("$('#dialogsku').val(data.Sku);");
            sb.Append("$('#dialogprice').val(data.Price);");
            sb.Append("$('#dialogimg').attr('src',data.ImageUrl);");
            sb.Append("$('#dialogdescription').html(data.Description);");
            sb.Append("});");
            sb.Append("}");

            sb.Append("function LoadVariants() {");
            sb.Append(" var dest = 'ProductVariants_List.aspx?id=" + productBvin + "';");
            sb.Append("$.get(dest, function(data) {");
            //sb.Append(" alert(data); ")
            sb.Append("$('#litVariants').html(data);");
            sb.Append(" BindClicks(); ");
            sb.Append("});");
            sb.Append("}");

            return sb.ToString();
        }

        protected void btnGenerateAll_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            MTApp.CatalogServices.VariantsGenerateAllPossible(localProduct);

            //// Generate Inventory to Match Variants
            //MTApp.CatalogServices.InventoryGenerateForProduct(localProduct);
            //MTApp.CatalogServices.UpdateProductVisibleStatusAndSave(localProduct);

            Response.Redirect("ProductChoices_Variants.aspx?id=" + productBvin);
        }

        protected void btnNew_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {

            List<OptionSelection> selections = new List<OptionSelection>();
            List<Option> variantOptions = localProduct.Options.VariantsOnly();

            if ((variantOptions.Count > 0))
            {
                foreach (Option opt in variantOptions)
                {
                    DropDownList ddl = new DropDownList();
                    ddl = (DropDownList)phLists.FindControl("new" + opt.Bvin);
                    if ((ddl != null))
                    {
                        if ((ddl.SelectedItem != null))
                        {
                            selections.Add(new OptionSelection(opt.Bvin, ddl.SelectedItem.Value));
                        }
                    }
                }

                if ((selections.Count == variantOptions.Count))
                {
                    Variant v = new Variant();
                    v.ProductId = localProduct.Bvin;
                    v.Selections.AddRange(selections);
                    MTApp.CatalogServices.ProductVariants.Create(v);
                }
            }

            //// Generate Inventory to Match
            //MTApp.CatalogServices.InventoryGenerateForProduct(localProduct);
            //MTApp.CatalogServices.UpdateProductVisibleStatusAndSave(localProduct);

            Response.Redirect("ProductChoices_Variants.aspx?id=" + productBvin);
        }

        protected void btnSave_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {

            Variant item = MTApp.CatalogServices.ProductVariants.Find(this.dialogbvin.Value);
            if ((item != null))
            {

                item.Sku = this.dialogsku.Text.Trim();
                decimal p = item.Price;
                if ((decimal.TryParse(this.dialogprice.Text.Trim(), out p)))
                {
                    item.Price = p;
                }

                if (this.dialognewFile.HasFile)
                {
                    MerchantTribe.Commerce.Storage.DiskStorage.UploadProductVariantImage(this.MTApp.CurrentStore.Id, this.productBvin, item.Bvin, this.dialognewFile.PostedFile);
                }
                MTApp.CatalogServices.ProductVariants.Update(item);

            }


            Response.Redirect("ProductChoices_Variants.aspx?id=" + productBvin);
        }
    }
}