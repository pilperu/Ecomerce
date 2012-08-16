using System.Text;
using System.Web.UI;
using System.Collections.Generic;
using MerchantTribe.Commerce.Catalog;
using MerchantTribe.Commerce.Membership;

namespace MerchantTribeStore
{

    partial class BVAdmin_Catalog_ProductSharedChoices : BaseAdminPage
    {
        protected override void OnPreInit(System.EventArgs e)
        {
            base.OnPreInit(e);
            this.PageTitle = "Shared Product Choices";
            this.CurrentTab = AdminTabType.Catalog;
            ValidateCurrentUserHasPermission(SystemPermissions.CatalogView);
        }

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);

            if (!Page.IsPostBack)
            {
                LoadItems();
            }
        }

        private void LoadItems()
        {
            List<Option> options = MTApp.CatalogServices.ProductOptions.FindAllShared(1, 1000);
            RenderItems(options);
        }

        private void RenderItems(List<Option> items)
        {
            StringBuilder sb = new StringBuilder();

            bool isAlternate = false;
            sb.Append("<table border=\"0\" class=\"formtable\">");
            foreach (Option opt in items)
            {
                RenderSingleItem(sb, opt, isAlternate);
                isAlternate = !(isAlternate);
            }
            sb.Append("</table>");
            this.litResults.Text = sb.ToString();
        }

        private void RenderSingleItem(StringBuilder sb, Option o, bool isAlternate)
        {

            string destinationLink = "ProductSharedChoices_Edit.aspx?id=" + o.Bvin;

            sb.Append("<tr id=\"" + o.Bvin + "\"");
            if ((isAlternate))
            {
                sb.Append(" class=\"alternaterow-padded\"");
            }
            else
            {
                sb.Append(" class=\"row-padded\"");
            }
            sb.Append("><td><a href=\"" + destinationLink + "\">");
            sb.Append(o.Name);
            sb.Append("</a></td>");
            sb.Append("<td>");
            sb.Append(o.Render());
            sb.Append("</td>");
            sb.Append("<td>");
            sb.Append("<a id=\"rem" + o.Bvin + "\" class=\"trash\" href=\"ProductSharedChoices_Delete.aspx?redirect=y&id=");
            sb.Append(o.Bvin);
            sb.Append("\" >");
            sb.Append("<img src=\"../images/buttons/delete.png\" alt=\"delete\" />");
            sb.Append("</a></td>");
            sb.Append("<td><a href=\"" + destinationLink + "\">");
            sb.Append("<img src=\"../images/buttons/edit.png\" alt=\"Edit\" />");
            sb.Append("</a></td>");
            sb.Append("</tr>");
        }

        protected void NewSharedChoiceImageButton_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            this.MessageBox1.ClearMessage();

            Option opt = Option.Factory(OptionTypes.Html);

            int typeCode = 100;
            if ((int.TryParse(this.SharedChoiceTypes.SelectedItem.Value, out typeCode)))
            {
                opt.SetProcessor((OptionTypes)typeCode);
            }

            opt.IsShared = true;
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

            if ((MTApp.CatalogServices.ProductOptions.Create(opt)))
            {
                Response.Redirect("ProductSharedChoices_Edit.aspx?id=" + opt.Bvin);
            }
            else
            {
                this.MessageBox1.ShowError("Unable to create choice. An Administrator has been alerted to the issue.");
            }
        }
    }
}