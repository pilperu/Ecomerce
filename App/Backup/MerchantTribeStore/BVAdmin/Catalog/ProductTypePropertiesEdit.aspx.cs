using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using MerchantTribe.Commerce.Catalog;
using MerchantTribe.Commerce.Membership;
using MerchantTribe.Commerce.Content;
using MerchantTribe.Web.Geography;
using System.Linq;

namespace MerchantTribeStore
{

    partial class Products_ProductProperties_Edit : BaseAdminPage
    {

        private ProductProperty localProperty = null;

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.PageTitle = "Product Properties Edit";
            this.CurrentTab = AdminTabType.Catalog;
            ValidateCurrentUserHasPermission(SystemPermissions.CatalogView);
        }
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            long propertyId = -1;
            if (Request.QueryString["id"] != null)
            {
                propertyId = long.Parse(Request.QueryString["id"]);
            }
            localProperty = MTApp.CatalogServices.ProductProperties.Find(propertyId);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (!Page.IsPostBack)
            {
                this.CurrentTab = AdminTabType.Catalog;

                if (localProperty != null)
                {
                    ViewState["ID"] = localProperty.Id;
                }
                PopulateCultureCodeList();
                LoadProperty(localProperty);
            }

        }

        private void PopulateCultureCodeList()
        {
            lstCultureCode.DataSource = MTApp.CurrentStore.Settings.FindActiveCountries();
            lstCultureCode.DataValueField = "CultureCode";
            lstCultureCode.DataTextField = "SampleNameAndCurrency";
            lstCultureCode.DataBind();
        }

        private void LoadProperty(ProductProperty prop)
        {
            if (prop != null)
            {
                this.PropertyNameField.Text = prop.PropertyName;
                this.DisplayNameField.Text = prop.DisplayName;
                this.chkDisplayOnSite.Checked = prop.DisplayOnSite;
                this.chkDisplayToDropShipper.Checked = prop.DisplayToDropShipper;
                this.DefaultValueField.Text = prop.DefaultValue;

                foreach (System.Web.UI.WebControls.ListItem li in lstCultureCode.Items)
                {
                    if (li.Value == prop.CultureCode)
                    {
                        lstCultureCode.ClearSelection();
                        li.Selected = true;
                    }
                }

                this.DefaultValueField.TextMode = TextBoxMode.MultiLine;
                //Me.DefaultDate.SetYearRange(Date.Now.Year - 50, Date.Now.Year + 50)

                if (prop.TypeCode == ProductPropertyType.DateField)
                {
                    System.DateTime d = new System.DateTime();
                    d = System.DateTime.Now;
                    try
                    {
                        if (prop.DefaultValue.Trim().Length > 0)
                        {
                            d = System.DateTime.Parse(prop.DefaultValue);
                        }
                    }
                    catch (Exception ex)
                    {
                        MerchantTribe.Commerce.EventLog.LogEvent(ex);
                    }
                    this.DefaultDate.SelectedDate = d;
                }

                DisplayProperControls(prop);
            }
            else
            {
                msg.ShowError("Unable to load Product Property ID " + (string)ViewState["ID"]);
            }
        }

        private void DisplayProperControls(ProductProperty prop)
        {
            ProductPropertyType typeCode = prop.TypeCode;

            this.pnlChoiceControls.Visible = false;
            pnlCultureCode.Visible = false;
            this.DefaultDate.Visible = false;

            switch (typeCode)
            {
                case ProductPropertyType.CurrencyField:
                    pnlCultureCode.Visible = true;
                    DefaultValueField.Visible = true;
                    lstDefaultValue.Visible = false;
                    ChoiceNote.InnerText = "Default Value";
                    break;
                case ProductPropertyType.DateField:
                    DefaultValueField.Visible = false;
                    lstDefaultValue.Visible = false;
                    this.DefaultDate.Visible = true;
                    ChoiceNote.InnerText = "Default Value";
                    break;
                case ProductPropertyType.MultipleChoiceField:
                    lstDefaultValue.Visible = true;
                    DefaultValueField.Visible = false;
                    PopulateMultipleChoice(prop);
                    ChoiceNote.InnerText = "To select a default simply select the item in the list before you hit save.";
                    this.pnlChoiceControls.Visible = true;
                    break;
                case ProductPropertyType.TextField:
                    DefaultValueField.Visible = true;
                    lstDefaultValue.Visible = false;
                    ChoiceNote.InnerText = "Default Value";
                    break;
            }
        }

        private void PopulateVendors()
        {

            this.lstDefaultValue.DataSource = MTApp.ContactServices.Vendors.FindAll();
            this.lstDefaultValue.DataTextField = "DisplayName";
            this.lstDefaultValue.DataValueField = "bvin";
            this.lstDefaultValue.DataBind();
            foreach (System.Web.UI.WebControls.ListItem li in lstDefaultValue.Items)
            {
                if (li.Value == DefaultValueField.Text.Trim())
                {
                    lstDefaultValue.ClearSelection();
                    li.Selected = true;
                }
            }

        }

        private void PopulateManufacturers()
        {
            this.lstDefaultValue.DataSource = MTApp.ContactServices.Manufacturers.FindAll();
            this.lstDefaultValue.DataTextField = "DisplayName";
            this.lstDefaultValue.DataValueField = "bvin";
            this.lstDefaultValue.DataBind();
            foreach (System.Web.UI.WebControls.ListItem li in lstDefaultValue.Items)
            {
                if (li.Value == DefaultValueField.Text.Trim())
                {
                    lstDefaultValue.ClearSelection();
                    li.Selected = true;
                }
            }
        }

        private void PopulateMultipleChoice(ProductProperty p)
        {

            this.lstDefaultValue.DataSource = p.Choices;
            this.lstDefaultValue.DataTextField = "ChoiceName";
            this.lstDefaultValue.DataValueField = "id";
            this.lstDefaultValue.DataBind();

            foreach (System.Web.UI.WebControls.ListItem li in lstDefaultValue.Items)
            {
                if (li.Value == DefaultValueField.Text.Trim())
                {
                    lstDefaultValue.ClearSelection();
                    li.Selected = true;
                }
            }
        }

        protected void btnCancel_Click(System.Object sender, System.Web.UI.ImageClickEventArgs e)
        {
            // Delete newly created item if user cancels so we don't leave a bunch of "new property"
            if (Request.QueryString["newmode"] == "1")
            {
                MTApp.CatalogServices.ProductPropertiesDestroy((long)ViewState["ID"]);
            }
            Response.Redirect("~/BVAdmin/Catalog/ProductTypeProperties.aspx");
        }

        protected void btnSave_Click(System.Object sender, System.Web.UI.ImageClickEventArgs e)
        {
            msg.ClearMessage();
            ProductProperty prop = new ProductProperty();
            prop = MTApp.CatalogServices.ProductProperties.Find((long)ViewState["ID"]);
            if (prop != null)
            {
                prop.PropertyName = PropertyNameField.Text;
                prop.DisplayName = DisplayNameField.Text;
                prop.DisplayOnSite = chkDisplayOnSite.Checked;
                prop.DisplayToDropShipper = chkDisplayToDropShipper.Checked;
                switch (prop.TypeCode)
                {
                    case ProductPropertyType.CurrencyField:
                        prop.CultureCode = lstCultureCode.SelectedValue;
                        prop.DefaultValue = DefaultValueField.Text.Trim();
                        break;
                    case ProductPropertyType.MultipleChoiceField:
                        prop.DefaultValue = lstDefaultValue.SelectedValue;
                        break;
                    case ProductPropertyType.DateField:
                        prop.DefaultValue = string.Format("{0:d}", DefaultDate.SelectedDate);
                        break;
                    case ProductPropertyType.TextField:
                        prop.DefaultValue = DefaultValueField.Text.Trim();
                        break;
                }
                if (MTApp.CatalogServices.ProductProperties.Update(prop) == true)
                {
                    prop = null;
                    Response.Redirect("~/BVAdmin/Catalog/ProductTypeProperties.aspx");
                }
                else
                {
                    prop = null;
                    msg.ShowError("Error: Couldn't Save Property!");
                }
            }
            else
            {
                msg.ShowError("Couldn't Load Property to Update!");
            }

        }

        protected void btnDelete_Click(System.Object sender, System.Web.UI.ImageClickEventArgs e)
        {            
            if (this.lstDefaultValue.Items.Count > 0)
            {

                ProductProperty prop = new ProductProperty();
                prop = MTApp.CatalogServices.ProductProperties.Find((long)ViewState["ID"]);
                if (prop != null)
                {
                    long choiceId = long.Parse(lstDefaultValue.SelectedValue);
                    ProductPropertyChoice c = prop.Choices.Where(y => y.Id == choiceId).FirstOrDefault();
                    if (c != null)
                    {
                        prop.Choices.Remove(c);
                        MTApp.CatalogServices.ProductProperties.Update(prop);
                        localProperty = prop;
                    }                    
                    PopulateMultipleChoice(prop);
                }                
            }
        }

        protected void btnNewChoice_Click(System.Object sender, System.Web.UI.ImageClickEventArgs e)
        {
            msg.ClearMessage();

            ProductProperty prop = new ProductProperty();
            prop = MTApp.CatalogServices.ProductProperties.Find((long)ViewState["ID"]);
            if (prop != null)
            {
                ProductPropertyChoice ppc = new ProductPropertyChoice();
                ppc.ChoiceName = this.NewChoiceField.Text.Trim();
                ppc.PropertyId = (long)ViewState["ID"];
                prop.Choices.Add(ppc);
                if (MTApp.CatalogServices.ProductProperties.Update(prop))
                {                
                        PopulateMultipleChoice(prop);
                    }
                    else
                    {
                        msg.ShowError("Couldn't add choice!");
                    }
                    this.NewChoiceField.Text = string.Empty;                
            }            
        }

        protected void btnMoveUp_Click(System.Object sender, System.Web.UI.ImageClickEventArgs e)
        {
            msg.ClearMessage();
            this.DefaultValueField.Text = this.lstDefaultValue.SelectedValue;
            long propId = (long)ViewState["ID"];
            long choiceId = long.Parse(lstDefaultValue.SelectedValue);
            MTApp.CatalogServices.ProductProperties.MoveChoiceUp(propId, choiceId);
            
            ProductProperty prop = MTApp.CatalogServices.ProductProperties.Find((long)ViewState["ID"]);
            if (prop != null) PopulateMultipleChoice(prop);
        }

        protected void btnMoveDown_Click(System.Object sender, System.Web.UI.ImageClickEventArgs e)
        {
            msg.ClearMessage();
            this.DefaultValueField.Text = this.lstDefaultValue.SelectedValue;
            long propId = (long)ViewState["ID"];
            long choiceId = long.Parse(lstDefaultValue.SelectedValue);
            MTApp.CatalogServices.ProductProperties.MoveChoiceDown(propId, choiceId);

            ProductProperty prop = MTApp.CatalogServices.ProductProperties.Find((long)ViewState["ID"]);
            if (prop != null) PopulateMultipleChoice(prop);
        }

    }
}