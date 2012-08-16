using System.Web.UI;
using MerchantTribe.Commerce.Catalog;

namespace MerchantTribeStore
{

    partial class BVAdmin_Catalog_ProductSharedChoices_Edit : BaseAdminPage
    {

        private string choiceid = string.Empty;
        private Option choice;

        protected override void OnPreLoad(System.EventArgs e)
        {
            base.OnPreLoad(e);
            choiceid = Request.QueryString["id"];
            choice = MTApp.CatalogServices.ProductOptions.Find(choiceid);
            if (choice.OptionType == OptionTypes.DropDownList)
            {
                this.ItemsEditor.AllowLabels = true;
            }
            else
            {
                this.ItemsEditor.AllowLabels = false;
            }
            this.ItemsEditor.OptionId = choice.Bvin;

            if (choice.OptionType == OptionTypes.DropDownList | choice.OptionType == OptionTypes.RadioButtonList)
            {
                trVariant.Visible = true;
            }
        }

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);
            if (!Page.IsPostBack)
            {
                LoadItem();
            }
        }

        private void LoadItem()
        {
            this.NameField.Text = choice.Name;
            this.chkHideName.Checked = choice.NameIsHidden;
            this.chkVariant.Checked = choice.IsVariant;

            switch (choice.OptionType)
            {
                case OptionTypes.DropDownList:
                case OptionTypes.RadioButtonList:
                case OptionTypes.CheckBoxes:
                    this.viewMain.SetActiveView(this.viewItems);
                    break;
                case OptionTypes.Html:
                    this.viewMain.SetActiveView(this.viewHtml);
                    this.HtmlEditor1.Text = choice.Settings.GetSettingOrEmpty("html");
                    break;
                case OptionTypes.TextInput:
                    this.viewMain.SetActiveView(this.viewTextInput);
                    MerchantTribe.Commerce.Catalog.Options.TextInput ti = (MerchantTribe.Commerce.Catalog.Options.TextInput)choice.Processor;
                    this.ColumnsField.Text = ti.GetColumns(choice);
                    this.RowsField.Text = ti.GetRows(choice);
                    this.MaxLengthField.Text = ti.GetMaxLength(choice);
                    break;
            }

        }

        protected void btnSaveOption_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            this.MessageBox1.ClearMessage();
            bool success = true;
            choice.Name = this.NameField.Text;
            choice.NameIsHidden = this.chkHideName.Checked;
            choice.IsVariant = this.chkVariant.Checked;

            switch (choice.OptionType)
            {
                case OptionTypes.CheckBoxes:
                    success = MTApp.CatalogServices.ProductOptions.Update(choice);
                    break;
                case OptionTypes.DropDownList:
                    success = MTApp.CatalogServices.ProductOptions.Update(choice);
                    break;
                case OptionTypes.FileUpload:
                    success = MTApp.CatalogServices.ProductOptions.Update(choice);
                    break;
                case OptionTypes.Html:
                    choice.Settings.AddOrUpdate("html", this.HtmlEditor1.Text);
                    success = MTApp.CatalogServices.ProductOptions.Update(choice);
                    break;
                case OptionTypes.RadioButtonList:
                    success = MTApp.CatalogServices.ProductOptions.Update(choice);
                    break;
                case OptionTypes.TextInput:
                    MerchantTribe.Commerce.Catalog.Options.TextInput ti = (MerchantTribe.Commerce.Catalog.Options.TextInput)choice.Processor;
                    ti.SetColumns(choice, this.ColumnsField.Text);
                    ti.SetRows(choice, this.RowsField.Text);
                    ti.SetMaxLength(choice, this.MaxLengthField.Text);
                    success = MTApp.CatalogServices.ProductOptions.Update(choice);
                    break;
            }

            if ((success))
            {
                MTApp.CatalogServices.ValidateVariantsForSharedOption(choice);
                this.MessageBox1.ShowOk("Changes Saved!");
            }

        }
    }

}