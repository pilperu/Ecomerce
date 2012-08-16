using System.Web.UI;
using MerchantTribe.Commerce.Catalog;
using MerchantTribe.Commerce.Membership;
using System.Linq;

namespace MerchantTribeStore
{

    partial class BVAdmin_Catalog_ProductChoices_Edit : BaseProductAdminPage
    {

        private string choiceid = string.Empty;
        protected internal string productBvin = string.Empty;
        private Product localProduct = null;
        private MerchantTribe.Commerce.Catalog.Option choice;

        protected override void OnInit(System.EventArgs e)
        {

            base.OnInit(e);

            choiceid = Request.QueryString["cid"];
            productBvin = Request.QueryString["id"];
            localProduct = MTApp.CatalogServices.Products.Find(productBvin);
            choice = (Option)localProduct.Options.Where(y => y.Bvin == choiceid).FirstOrDefault();
            //choice = MTApp.CatalogServices.ProductOptions.Find(choiceid);
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

            this.PageTitle = "Edit Product Choice";
            this.CurrentTab = AdminTabType.Catalog;
            ValidateCurrentUserHasPermission(SystemPermissions.CatalogView);
        }

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);
            this.CurrentTab = AdminTabType.Catalog;
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
            SaveItem();
        }

        private bool SaveItem()
        {
            this.MessageBox1.ClearMessage();            
            choice.Name = this.NameField.Text;
            choice.NameIsHidden = this.chkHideName.Checked;
            choice.IsVariant = this.chkVariant.Checked;
            switch (choice.OptionType)
            {
                case OptionTypes.Html:
                    choice.Settings.AddOrUpdate("html", this.HtmlEditor1.Text);
                    break;
                case OptionTypes.TextInput:
                    MerchantTribe.Commerce.Catalog.Options.TextInput ti = (MerchantTribe.Commerce.Catalog.Options.TextInput)choice.Processor;
                    ti.SetColumns(choice,this.ColumnsField.Text);
                    ti.SetRows(choice,this.RowsField.Text);
                    ti.SetMaxLength(choice,this.MaxLengthField.Text);
                    break;
            }
            bool success = MTApp.CatalogServices.Products.Update(localProduct);
            if ((success))
            {
                MTApp.CatalogServices.VariantsValidate(localProduct);                    
                this.MessageBox1.ShowOk("Changes Saved!");
            }
            else
            {
                this.MessageBox1.ShowWarning("Unable to save changes. An administrator has been alerted.");
            }

            return success;
        }

        protected override bool Save()
        {
            return SaveItem();
        }

        protected void btnSaveAndClose_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            if ((SaveItem()))
            {
                Response.Redirect("ProductChoices.aspx?id=" + productBvin);
            }
        }
    }
}