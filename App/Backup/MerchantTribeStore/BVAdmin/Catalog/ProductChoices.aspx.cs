using System.Text;
using System.Web.UI;
using System.Collections.Generic;
using MerchantTribe.Commerce;
using MerchantTribe.Commerce.Catalog;
using MerchantTribe.Commerce.Membership;
using MerchantTribe.Web.Logging;

namespace MerchantTribeStore
{

    partial class BVAdmin_Catalog_ProductChoices : BaseProductAdminPage
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

            this.PageTitle = "Edit Product Choices";
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
            Page.ClientScript.RegisterClientScriptBlock(typeof(System.Web.UI.Page), "optionsort", RenderJQuery(productBvin), true);

            if (!Page.IsPostBack)
            {
                PopulateSharedChoices();
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
            sb.Append(" var sortedQuoted = sorted + ' ';");
            sb.Append("$.post('ProductChoices_Sort.aspx',");
            sb.Append("  { \"ids\": sortedQuoted,");
            sb.Append("    \"bvin\": \"" + productBvin + "\"");
            sb.Append("  }, function() {return;});");

            sb.Append(" }");
            sb.Append("});");                                                 

            sb.Append("$('#dragitem-list').disableSelection();");

            sb.Append("$('.trash').click(function() {");
            sb.Append(" if ($(this).attr('title') === 'variant') {");
            sb.Append(" if (window.confirm('Deleting this choice will DELETE YOUR VARIANTS. You will lose inventory, price and picture settings for variants. Are you sure you want to continue?'))");
            sb.Append(" {");
            sb.Append("   RemoveItem($(this));");
            sb.Append(" }} else { RemoveItem($(this));}");
            sb.Append(" return false;");
            sb.Append("});");

            sb.Append("});");
            // End of Document Ready

            sb.Append("function RemoveItem(lnk) {");
            sb.Append("  var id = $(lnk).attr('id');");
            sb.Append("$.post('ProductChoices_Delete.aspx',");
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

        private void PopulateSharedChoices()
        {
            List<Option> items = MTApp.CatalogServices.ProductOptions.FindAllShared(1, 1000);
            if ((items.Count > 0))
            {
                this.ChoiceTypes.Items.Add(new System.Web.UI.WebControls.ListItem("", ""));
                this.ChoiceTypes.Items.Add(new System.Web.UI.WebControls.ListItem("", ""));
                this.ChoiceTypes.Items.Add(new System.Web.UI.WebControls.ListItem(" Shared Choices", ""));
                this.ChoiceTypes.Items.Add(new System.Web.UI.WebControls.ListItem("------------------", ""));
                foreach (Option opt in items)
                {
                    this.ChoiceTypes.Items.Add(new System.Web.UI.WebControls.ListItem(opt.Name, opt.Bvin));
                }
            }
        }

        private void LoadItems()
        {
            RenderItems(localProduct.Options);
        }

        private void RenderItems(List<Option> items)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("<div id=\"dragitem-list\">");
            foreach (Option opt in items)
            {
                RenderSingleItem(sb, opt);
            }
            sb.Append("</div>");
            this.litResults.Text = sb.ToString();
        }

        private void RenderSingleItem(StringBuilder sb, Option o)
        {

            string destinationLink = "ProductChoices_Edit.aspx?cid=" + o.Bvin + "&id=" + productBvin;

            sb.Append("<div class=\"dragitem\" id=\"item_" + o.Bvin + "\"><table class=\"formtable\" width=\"100%\"><tr>");
            sb.Append("<td width=\"25%\"><a href=\"" + destinationLink + "\">");
            sb.Append(o.Name);
            sb.Append("</a></td>");
            sb.Append("<td>");
            sb.Append(o.Render());
            sb.Append("</td>");

            sb.Append("<td width=\"75\">");
            if ((o.IsVariant))
            {
                sb.Append("VARIANT");
            }
            sb.Append("</td>");

            if ((o.IsShared))
            {
                sb.Append("<td width=\"75\">SHARED</td>");
            }
            else
            {
                sb.Append("<td width=\"75\"><a href=\"" + destinationLink + "\"><img src=\"../images/buttons/edit.png\" alt=\"edit\" /></a></td>");
            }

            sb.Append("<td width=\"30\"><a href=\"#\" class=\"trash\" id=\"rem" + o.Bvin + "\"");
            if ((o.IsVariant))
            {
                sb.Append("  title=\"variant\" ");
            }
            sb.Append("><img src=\"../../images/system/trashcan.png\" alt=\"Delete\" /></a></td>");
            sb.Append("<td width=\"30\"><a href=\"#\" class=\"handle\"><img src=\"../../images/system/draghandle.png\" alt=\"Move\" /></a></td>");

            sb.Append("</tr></table></div>");
        }

        protected void NewChoiceButton_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            this.MessageBox1.ClearMessage();

            Option opt = new Option();
            opt.SetProcessor(OptionTypes.DropDownList);

            int typeCode = 100;
            if ((int.TryParse(this.ChoiceTypes.SelectedItem.Value, out typeCode)))
            {
                opt.SetProcessor((OptionTypes)typeCode);
            }

            opt.IsShared = false;
            opt.IsVariant = false;
            opt.Name = "New Choice";

            switch (opt.OptionType)
            {
                case OptionTypes.CheckBoxes:
                    opt.Name = "New Checkboxes";
                    break;
                case OptionTypes.DropDownList:
                    opt.Name = "New Drop Down List";
                    break;
                case OptionTypes.FileUpload:
                    opt.Name = "New File Upload";
                    break;
                case OptionTypes.Html:
                    opt.Name = "New Html Description";
                    break;
                case OptionTypes.RadioButtonList:
                    opt.Name = "New Radio Button List";
                    break;
                case OptionTypes.TextInput:
                    opt.Name = "New Text Input";
                    break;
            }
            opt.StoreId = MTApp.CurrentStore.Id;

            // Trick the new option to be an already existing option if shared
            if ((this.ChoiceTypes.SelectedItem.Value.Trim().Length > 30))
            {
                opt.Bvin = this.ChoiceTypes.SelectedItem.Value;
                opt.IsShared = true;
                opt.Name = this.ChoiceTypes.SelectedItem.Text;
            }

            // Create Choice
            bool created = false;

            if ((opt.IsShared))
            {
                created = true;
            }
            else
            {
                localProduct.Options.Add(opt);
                MTApp.CatalogServices.Products.Update(localProduct);
                created = true;
            }

            // Associate Choice with Product
            if ((created))
            {
                if ((localProduct != null))
                {
                    MTApp.CatalogServices.ProductsAddOption(localProduct, opt.Bvin);                    
                    if ((opt.IsShared == false))
                    {
                        Response.Redirect("ProductChoices_Edit.aspx?cid=" + opt.Bvin + "&id=" + localProduct.Bvin);
                    }
                }
                else
                {
                    this.MessageBox1.ShowError("Unable to associate choice with product. An Administrator has been alerted to the issue.");
                    EventLog.LogEvent("ProductChoices.aspx", "Could not associate choice " + opt.Bvin + " with product " + productBvin, EventLogSeverity.Error);
                }
            }

            MTApp.CatalogServices.ProductsReloadOptions(localProduct);            
            LoadItems();
        }


    }
}