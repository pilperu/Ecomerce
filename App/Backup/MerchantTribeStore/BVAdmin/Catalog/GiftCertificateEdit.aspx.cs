using System.Text;
using System.Web.UI;
using System.Collections.ObjectModel;
using MerchantTribe.Commerce.Catalog;
using MerchantTribe.Commerce.Membership;
using MerchantTribe.Commerce.Utilities;
using MerchantTribe.Commerce.Content;
using System.Collections.Generic;

namespace MerchantTribeStore
{

    partial class BVAdmin_Catalog_GiftCertificateEdit : BaseAdminPage
    {

        protected NotifyClickControl _ProductNavigator = new NotifyClickControl();

        protected override void OnInit(System.EventArgs e)
        {
            base.OnInit(e);
            decimal val = 0m;
            SitePriceField.Text = val.ToString("c");
        }

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);

            this.RegisterWindowScripts();

            if (!Page.IsPostBack)
            {
                PopulateColumns();
                this.SkuField.Focus();

                if (Request.QueryString["id"] != null)
                {
                    this.BvinField.Value = Request.QueryString["id"];
                    LoadProduct();
                }
                else
                {
                    this.BvinField.Value = string.Empty;
                }
            }

            this.ResolveSelectedImages();

            PopulateMenuControl();
        }

        private void PopulateMenuControl()
        {
            System.Web.UI.Control c = Page.Master.FindControl("ProductNavigator");
            if (c != null)
            {
                this._ProductNavigator = (MerchantTribe.Commerce.Content.NotifyClickControl)c;
            }
        }

        protected override void OnPreInit(System.EventArgs e)
        {
            base.OnPreInit(e);
            this.PageTitle = "Edit Product";
            this.CurrentTab = AdminTabType.Catalog;
            ValidateCurrentUserHasPermission(SystemPermissions.CatalogView);
            this._ProductNavigator.Clicked += this._ProductNavigator_Clicked;
        }

        private void PopulateColumns()
        {
            List<ContentColumn> columns = MTApp.ContentServices.Columns.FindAll();
            foreach (ContentColumn col in columns)
            {
                this.PreContentColumnIdField.Items.Add(new System.Web.UI.WebControls.ListItem(col.DisplayName, col.Bvin));
                this.PostContentColumnIdField.Items.Add(new System.Web.UI.WebControls.ListItem(col.DisplayName, col.Bvin));
            }
        }

        private void LoadProduct()
        {
            Product p;
            p = MTApp.CatalogServices.Products.Find(this.BvinField.Value);
            if (p != null)
            {
                if (p.Bvin != string.Empty)
                {
                    if (p.Status == ProductStatus.Active)
                    {
                        this.chkActive.Checked = true;
                    }
                    else
                    {
                        this.chkActive.Checked = false;
                    }
                    this.GiftCertificateTypeRadioButtonList.SelectedValue = "0"; // ((int)p.SpecialProductType).ToString();
                    this.SkuField.Text = p.Sku;
                    this.ProductNameField.Text = p.ProductName;

                    this.SitePriceField.Text = p.SitePrice.ToString("c");

                    this.LongDescriptionField.Text = p.LongDescription;
                    this.ShortDescriptionField.Text = p.ShortDescription;

                    if (p.ShortDescription.Length > 255)
                    {
                        this.CountField.Text = "0";
                    }
                    else
                    {
                        this.CountField.Text = (255 - p.ShortDescription.Length).ToString();
                    }

                    this.ImageFileMediumField.Text = p.ImageFileMedium;
                    this.ImageFileSmallField.Text = p.ImageFileSmall;

                    if (p.PreContentColumnId.Trim() != string.Empty)
                    {
                        if (this.PreContentColumnIdField.Items.FindByValue(p.PreContentColumnId) != null)
                        {
                            this.PreContentColumnIdField.Items.FindByValue(p.PreContentColumnId).Selected = true;
                        }
                    }
                    if (p.PostContentColumnId.Trim() != string.Empty)
                    {
                        if (this.PostContentColumnIdField.Items.FindByValue(p.PostContentColumnId) != null)
                        {
                            this.PostContentColumnIdField.Items.FindByValue(p.PostContentColumnId).Selected = true;
                        }
                    }

                    this.CertificateIdTextBox.Text = string.Empty; // p.GiftCertificateCodePattern;

                    this.RewriteUrlField.Text = p.UrlSlug;
                    LoadCustomUrl(p.Bvin);
                    CheckForValidFieldsBasedOnGiftCertificateType();
                }
            }

        }

        private void LoadCustomUrl(string productId)
        {
            //if (this.RewriteUrlField.Text.Trim() == string.Empty)
            //{
            //    // Attempt to find existing rewrite records
            //    CustomUrl c = CustomUrl.FindBySystemData(productId);
            //    if (c != null)
            //    {
            //        this.RewriteUrlField.Text = c.RequestedUrl;
            //    }
            //}
        }

        private void ResolveSelectedImages()
        {
            this.imgPreviewMedium.ImageUrl = Page.ResolveUrl(ImageHelper.GetValidImage(this.ImageFileMediumField.Text.Trim(), true));
            this.imgPreviewSmall.ImageUrl = Page.ResolveUrl(ImageHelper.GetValidImage(this.ImageFileSmallField.Text.Trim(), true));
        }

        private void RegisterWindowScripts()
        {

            StringBuilder sb = new StringBuilder();

            sb.Append("var w;");
            sb.Append("function popUpWindow(parameters) {");
            sb.Append("w = window.open('../ImageBrowser.aspx' + parameters, null, 'height=480, width=640');");
            sb.Append("}");

            sb.Append("function closePopup() {");
            sb.Append("w.close();");
            sb.Append("}");

            sb.Append("function SetSmallImage(fileName) {");
            sb.Append("document.getElementById('");
            sb.Append(this.ImageFileSmallField.ClientID);
            sb.Append("').value = fileName;");
            sb.Append("document.getElementById('");
            sb.Append(this.imgPreviewSmall.ClientID);
            sb.Append("').src = '../../'+fileName;");
            sb.Append("w.close();");
            sb.Append("}");

            sb.Append("function SetMediumImage(fileName) {");
            sb.Append("document.getElementById('");
            sb.Append(this.ImageFileMediumField.ClientID);
            sb.Append("').value = fileName;");
            sb.Append("document.getElementById('");
            sb.Append(this.imgPreviewMedium.ClientID);
            sb.Append("').src = '../../'+fileName;");
            sb.Append("w.close();");
            sb.Append("}");

            //Script for description counter
            sb.Append("function textCounter() {");
            sb.Append("var maxlimit;");
            sb.Append("maxlimit=255;");
            sb.Append("var field;");
            sb.Append("field = document.getElementById('");
            sb.Append(this.ShortDescriptionField.ClientID);
            sb.Append("'); ");
            sb.Append("if (field.value.length > maxlimit) {");
            sb.Append(" field.value = field.value.substring(0, maxlimit); }");
            sb.Append("else");
            sb.Append("{ document.getElementById('");
            sb.Append(this.CountField.ClientID);
            sb.Append("').value = maxlimit - field.value.length; }");
            sb.Append("}");

            this.ShortDescriptionField.Attributes.Add("onkeyup", "textCounter();");

            Page.ClientScript.RegisterClientScriptBlock(typeof(System.Web.UI.Page), "WindowScripts", sb.ToString(), true);

        }

        private void CancelClick()
        {
            Response.Redirect("~/BVAdmin/Catalog/GiftCertificates.aspx");
        }

        private void SaveClick()
        {
            this.lblError.Text = string.Empty;

            if (this.Save() == true)
            {
                Response.Redirect("~/BVAdmin/Catalog/GiftCertificates.aspx");
            }
        }

        protected void btnCancel_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            CancelClick();
        }

        protected void btnSaveChanges_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            if (Page.IsValid)
            {
                SaveClick();
            }
        }

        protected void btnCancel2_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            CancelClick();
        }

        protected void btnSave2_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            SaveClick();
        }

        private bool Save()
        {
            bool result = false;

            Product p;
            p = MTApp.CatalogServices.Products.Find(this.BvinField.Value);
            if (p == null)
            {
                //if it is nothing then create a new product
                p = new Product();
            }

            if (p != null)
            {

                //p.SpecialProductType = (SpecialProductTypes)int.Parse(this.GiftCertificateTypeRadioButtonList.SelectedValue);
                if (this.chkActive.Checked == true)
                {
                    p.Status = ProductStatus.Active;
                }
                else
                {
                    p.Status = ProductStatus.Disabled;
                }

                p.Sku = this.SkuField.Text.Trim();
                p.ProductName = this.ProductNameField.Text.Trim();

                p.SitePrice = decimal.Parse(this.SitePriceField.Text, System.Globalization.NumberStyles.Currency);
                p.LongDescription = this.LongDescriptionField.Text.Trim();
                p.ShortDescription = this.ShortDescriptionField.Text.Trim();

                if (this.GiftCertificateTypeRadioButtonList.SelectedValue == ((int)SpecialProductTypes.GiftCertificate).ToString())
                {
                }
                //p.TemplateName = "FixedPriceGiftCertificate"
                else if (this.GiftCertificateTypeRadioButtonList.SelectedValue == ((int)SpecialProductTypes.ArbitrarilyPricedGiftCertificate).ToString())
                {
                    //p.TemplateName = "ArbitraryPriceGiftCertificate"
                }

                p.ImageFileSmall = this.ImageFileSmallField.Text.Trim();
                p.ImageFileMedium = this.ImageFileMediumField.Text.Trim();
                p.PreContentColumnId = this.PreContentColumnIdField.SelectedValue;
                p.PostContentColumnId = this.PostContentColumnIdField.SelectedValue;
                p.UrlSlug = this.RewriteUrlField.Text.Trim();

                //p.GiftCertificateCodePattern = this.CertificateIdTextBox.Text;
                p.ShippingDetails.IsNonShipping = true;
                if ((p.Bvin == string.Empty))
                {
                    result = MTApp.CatalogServices.ProductsCreateWithInventory(p, true);
                }
                else
                {
                    result = MTApp.CatalogServices.ProductsUpdateWithSearchRebuild(p);
                }

                if (result == false)
                {
                    this.lblError.Text = "Unable to save product. Uknown error.";
                }
                else
                {
                    // Update bvin field so that next save will call updated instead of create
                    this.BvinField.Value = p.Bvin;
                }


                //// Save Customized Url
                //if (this.RewriteUrlField.Text.Trim().Length > 0)
                //{
                //    // Create or update Custom Url
                //    CustomUrl curl = CustomUrl.FindBySystemData(p.Bvin);
                //    if (curl != null)
                //    {
                //        curl.SystemUrl = true;
                //        curl.SystemData = p.Bvin;
                //        curl.RequestedUrl = this.RewriteUrlField.Text.Trim();
                //        curl.RedirectToUrl = UrlRewriter.BuildUrlForProduct(p, this.Page);
                //        if (curl.Bvin != string.Empty)
                //        {
                //            CustomUrl.Update(curl);
                //        }
                //        else
                //        {
                //            CustomUrl.Insert(curl);
                //        }
                //    }
                //}
                //else
                //{
                //    // Delete any system custom Urls
                //    CustomUrl target = CustomUrl.FindBySystemData(p.Bvin);
                //    if (target != null)
                //    {
                //        if (target.Bvin != string.Empty)
                //        {
                //            CustomUrl.Delete(target.Bvin);
                //        }
                //    }
                //}
            }

            return result;
        }

        protected void _ProductNavigator_Clicked(object sender, MerchantTribe.Commerce.Content.NotifyClickControl.ClickedEventArgs e)
        {
            this.Save();
        }

        protected void GiftCertificateTypeRadioButtonList_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            CheckForValidFieldsBasedOnGiftCertificateType();
        }

        protected void CheckForValidFieldsBasedOnGiftCertificateType()
        {
            if (int.Parse(GiftCertificateTypeRadioButtonList.SelectedValue) == (int)SpecialProductTypes.GiftCertificate)
            {
                PriceRow.Visible = true;
            }
            else if (int.Parse(GiftCertificateTypeRadioButtonList.SelectedValue) == (int)SpecialProductTypes.ArbitrarilyPricedGiftCertificate)
            {
                PriceRow.Visible = false;
            }
        }

        protected void CustomValidator1_ServerValidate(object source, System.Web.UI.WebControls.ServerValidateEventArgs args)
        {
            if (CertificateIdTextBox.Text != string.Empty)
            {
                int count = 0;
                foreach (char letter in CertificateIdTextBox.Text)
                {
                    if (letter == '!' | letter == '#' | letter == '?')
                    {
                        count += 1;
                    }
                }

                if (count < 6)
                {
                    args.IsValid = false;
                }
                else
                {
                    args.IsValid = true;
                }
            }
        }

        protected void CustomValidator2_ServerValidate(object source, System.Web.UI.WebControls.ServerValidateEventArgs args)
        {
            decimal temp;
            if (decimal.TryParse(args.Value, System.Globalization.NumberStyles.Currency, System.Threading.Thread.CurrentThread.CurrentUICulture, out temp))
            {
                args.IsValid = true;
            }
            else
            {
                args.IsValid = false;
            }
        }
    }
}