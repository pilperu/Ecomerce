using System;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using MerchantTribe.Commerce;
using MerchantTribe.Commerce.Controls;
using System.Collections.ObjectModel;

namespace MerchantTribeStore
{

    partial class BVAdmin_Controls_ProductModifications : MerchantTribe.Commerce.Content.BVUserControl
    {

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);
            if (!this.Page.ClientScript.IsClientScriptBlockRegistered(this.GetType(), "flipRow"))
            {
                string script = "function flipRow(checkBoxId, rowId){ " + "   if (document.getElementById(checkBoxId).checked){" + "       document.getElementById(rowId).style.backgroundColor = '#ffc';" + "   }else{" + "       document.getElementById(rowId).style.backgroundColor = 'white';" + "   }" + "} ";
                this.Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "flipRow", script, true);
            }

            if (!this.Page.ClientScript.IsClientScriptBlockRegistered(this.GetType(), "checkBoxForOption"))
            {
                string script = "function checkBoxForOption(textbox, checkBoxId, rowId){" + "   if (textbox.value != ''){" + "       document.getElementById(checkBoxId).checked = true;" + "   }else{" + "       document.getElementById(checkBoxId).checked = false;" + "   }" + "   flipRow(checkBoxId, rowId);" + "}";
                this.Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "checkBoxForOption", script, true);
            }


            CheckBox currCheckBox = null;
            ITextBoxBasedControl currTextBoxBasedControl = null;
            foreach (System.Web.UI.Control rowControl in this.ProductModificationPanel.Controls)
            {
                if (rowControl is HtmlTableRow)
                {
                    foreach (System.Web.UI.Control cellControl in rowControl.Controls)
                    {
                        if (cellControl is HtmlTableCell)
                        {
                            foreach (System.Web.UI.Control control in cellControl.Controls)
                            {
                                if (control is ITextBoxBasedControl)
                                {
                                    currTextBoxBasedControl = (ITextBoxBasedControl)control;
                                    if ((currTextBoxBasedControl != null))
                                    {
                                        currTextBoxBasedControl.AddTextBoxAttribute("onkeyup", "checkBoxForOption(this, '" + currCheckBox.ClientID + "', '" + rowControl.ClientID + "')");
                                    }
                                }
                                if (control is CheckBox)
                                {
                                    currCheckBox = (CheckBox)control;
                                    if (currCheckBox.CssClass == "modificationSelected")
                                    {
                                        if (currCheckBox.Checked)
                                        {
                                            ((HtmlTableRow)rowControl).Attributes.Add("style", "background-color: #ffc");
                                        }
                                        else
                                        {
                                            ((HtmlTableRow)rowControl).Attributes.Add("style", "background-color: white");
                                        }
                                        currCheckBox.Attributes.Add("onclick", "flipRow(this.id, '" + rowControl.ClientID + "');");
                                    }
                                }
                            }
                        }
                    }
                }
            }

            if (!Page.IsPostBack)
            {
                BindEnumeratedFields();
            }
        }

        protected void BindEnumeratedFields()
        {
            ManufacturerEnumeratedValueModifierField.Datasource = MyPage.MTApp.ContactServices.Manufacturers.FindAll();
            ManufacturerEnumeratedValueModifierField.DataTextField = "DisplayName";
            ManufacturerEnumeratedValueModifierField.DataValueField = "bvin";
            ManufacturerEnumeratedValueModifierField.DataBind();


            ProductTemplateEnumeratedValueModifierField.Datasource = MerchantTribe.Commerce.Content.ModuleController.FindProductTemplates();
            ProductTemplateEnumeratedValueModifierField.DataBind();

            PreContentColumnEnumeratedValueModifierField.Datasource = MyPage.MTApp.ContentServices.Columns.FindAll();
            PreContentColumnEnumeratedValueModifierField.DataTextField = "DisplayName";
            PreContentColumnEnumeratedValueModifierField.DataValueField = "bvin";
            PreContentColumnEnumeratedValueModifierField.DataBind();

            PostContentColumnEnumeratedValueModifierField.Datasource = MyPage.MTApp.ContentServices.Columns.FindAll();
            PostContentColumnEnumeratedValueModifierField.DataTextField = "DisplayName";
            PostContentColumnEnumeratedValueModifierField.DataValueField = "bvin";
            PostContentColumnEnumeratedValueModifierField.DataBind();

            TaxClassEnumeratedValueModifierField.Datasource = MyPage.MTApp.OrderServices.TaxSchedules.FindAllAndCreateDefault(MyPage.MTApp.CurrentStore.Id);
            TaxClassEnumeratedValueModifierField.DataTextField = "Name";
            TaxClassEnumeratedValueModifierField.DataValueField = "Id";
            TaxClassEnumeratedValueModifierField.DataBind();

            VendorEnumeratedValueModifierField.Datasource = MyPage.MTApp.ContactServices.Vendors.FindAll();
            VendorEnumeratedValueModifierField.DataTextField = "DisplayName";
            VendorEnumeratedValueModifierField.DataValueField = "bvin";
            VendorEnumeratedValueModifierField.DataBind();
        }

        public void PostChangesToProduct(MerchantTribe.Commerce.Catalog.Product item)
        {
            bool process = false;
            Collection<ModificationControlBase> controls = new Collection<ModificationControlBase>();
            foreach (System.Web.UI.Control rowControl in this.ProductModificationPanel.Controls)
            {
                if (rowControl is HtmlTableRow)
                {
                    foreach (System.Web.UI.Control cellControl in rowControl.Controls)
                    {
                        if (cellControl is HtmlTableCell)
                        {
                            foreach (System.Web.UI.Control control in cellControl.Controls)
                            {
                                if (control is CheckBox)
                                {
                                    process = ((CheckBox)control).Checked;
                                }
                                if (process)
                                {
                                    if (control is ModificationControlBase)
                                    {
                                        controls.Add((ModificationControlBase)control);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            foreach (ModificationControlBase control in controls)
            {
                if (control is ModificationControl<string>)
                {
                    ModificationControl<string> stringControl = (ModificationControl<string>)control;
                    MakeChanges(stringControl, item);
                }
                else if (control is ModificationControl<bool>)
                {
                    ModificationControl<bool> booleanControl = (ModificationControl<bool>)control;
                    MakeChanges(booleanControl, item);
                }
                else if (control is ModificationControl<int>)
                {
                    ModificationControl<int> integerControl = (ModificationControl<int>)control;
                    MakeChanges(integerControl, item);
                }
                else if (control is ModificationControl<double>)
                {
                    ModificationControl<double> floatControl = (ModificationControl<double>)control;
                    MakeChanges(floatControl, item);
                }
                else if (control is ModificationControl<decimal>)
                {
                    ModificationControl<decimal> monetaryControl = (ModificationControl<decimal>)control;
                    MakeChanges(monetaryControl, item);
                }
            }

        }

        public void MakeChanges(ModificationControl<string> control, MerchantTribe.Commerce.Catalog.Product item)
        {
            if (control.ID == "ProductNameStringModifierField")
            {
                item.ProductName = control.ApplyChanges(item.ProductName);
            }
            else if (control.ID == "ProductSkuStringModifierField")
            {
                item.Sku = control.ApplyChanges(item.Sku);
            }
            else if (control.ID == "MetaKeywordsStringModifierField")
            {
                item.MetaKeywords = control.ApplyChanges(item.MetaKeywords);
            }
            else if (control.ID == "MetaDescriptionStringModifierField")
            {
                item.MetaDescription = control.ApplyChanges(item.MetaDescription);
            }
            else if (control.ID == "MetaTitleStringModifierField")
            {
                item.MetaTitle = control.ApplyChanges(item.MetaTitle);
            }
            else if (control.ID == "ImageFileSmallStringModifierField")
            {
                item.ImageFileSmall = control.ApplyChanges(item.ImageFileSmall);
            }
            else if (control.ID == "ImageFileMediumStringModifierField")
            {
                item.ImageFileMedium = control.ApplyChanges(item.ImageFileMedium);
            }
            else if (control.ID == "ShortDescriptionStringModifierField")
            {
                item.ShortDescription = control.ApplyChanges(item.ShortDescription);
            }
            else if (control.ID == "LongDescriptionHtmlModifierField")
            {
                item.LongDescription = control.ApplyChanges(item.LongDescription);
            }
            else if (control.ID == "KeyWordsStringModifierField")
            {
                item.Keywords = control.ApplyChanges(item.Keywords);
            }
            else if (control.ID == "UrlToRewriteStringModifierField")
            {
                item.UrlSlug = control.ApplyChanges(item.UrlSlug);
            }
            else if (control.ID == "SitePriceOverrideStringModifierField")
            {
                item.SitePriceOverrideText = control.ApplyChanges(item.SitePriceOverrideText);
            }
            else if (control.ID == "ManufacturerEnumeratedValueModifierField")
            {
                item.ManufacturerId = control.ApplyChanges(item.ManufacturerId);
            }
            else if (control.ID == "PreContentColumnEnumeratedValueModifierField")
            {
                item.PreContentColumnId = control.ApplyChanges(item.PreContentColumnId);
            }
            else if (control.ID == "PostContentColumnEnumeratedValueModifierField")
            {
                item.PostContentColumnId = control.ApplyChanges(item.PostContentColumnId);
            }
            //else if (control.ID == "TaxClassEnumeratedValueModifierField") {
            //    item.TaxSchedule = long.Parse(control.ApplyChanges(item.TaxSchedule));
            //}
            else if (control.ID == "VendorEnumeratedValueModifierField")
            {
                item.VendorId = control.ApplyChanges(item.VendorId);
            }
            else
            {
                throw new ControlNotFoundException(control.ID);
            }
        }

        public void MakeChanges(ModificationControl<bool> control, MerchantTribe.Commerce.Catalog.Product item)
        {
            if (control.ID == "TaxExemptBooleanModifierField")
            {
                item.TaxExempt = control.ApplyChanges(item.TaxExempt);
            }
            else if (control.ID == "NonShippingBooleanModifierField")
            {
                item.ShippingDetails.IsNonShipping = control.ApplyChanges(item.ShippingDetails.IsNonShipping);
            }
            else if (control.ID == "ShipSeperatelyBooleanModifierField")
            {
                item.ShippingDetails.ShipSeparately = control.ApplyChanges(item.ShippingDetails.ShipSeparately);
            }
            else if (control.ID == "ProductStateBooleanModifierField")
            {
                if (control.ApplyChanges(item.Status == MerchantTribe.Commerce.Catalog.ProductStatus.Active))
                {
                    item.Status = MerchantTribe.Commerce.Catalog.ProductStatus.Active;
                }
                else
                {
                    item.Status = MerchantTribe.Commerce.Catalog.ProductStatus.Disabled;
                }
            }
            else if (control.ID == "GiftWrapAllowedBooleanModifierField")
            {
                item.GiftWrapAllowed = control.ApplyChanges(item.GiftWrapAllowed);
            }
            else
            {
                throw new ControlNotFoundException(control.ID);
            }
        }

        public void MakeChanges(ModificationControl<int> control, MerchantTribe.Commerce.Catalog.Product item)
        {
            if (control.ID == "MinimumQuantityIntegerModifierField")
            {
                item.MinimumQty = control.ApplyChanges(item.MinimumQty);
            }
            else
            {
                throw new ControlNotFoundException(control.ID);
            }
        }

        public void MakeChanges(ModificationControl<double> control, MerchantTribe.Commerce.Catalog.Product item)
        {
            if (control.ID == "ShippingWeightFloatModifierField")
            {
                item.ShippingDetails.Weight = (decimal)control.ApplyChanges((double)item.ShippingDetails.Weight);
            }
            else if (control.ID == "ShippingLengthFloatModifierField")
            {
                item.ShippingDetails.Length = (decimal)control.ApplyChanges((double)item.ShippingDetails.Length);
            }
            else if (control.ID == "ShippingWidthFloatModifierField")
            {
                item.ShippingDetails.Width = (decimal)control.ApplyChanges((double)item.ShippingDetails.Width);
            }
            else if (control.ID == "ShippingHeightFloatModifierField")
            {
                item.ShippingDetails.Height = (decimal)control.ApplyChanges((double)item.ShippingDetails.Height);
            }
            else
            {
                throw new ControlNotFoundException(control.ID);
            }
        }

        public void MakeChanges(ModificationControl<decimal> control, MerchantTribe.Commerce.Catalog.Product item)
        {
            if (control.ID == "ListPriceMonetaryModifierField")
            {
                item.ListPrice = control.ApplyChanges(item.ListPrice);
            }
            else if (control.ID == "SitePriceMonetaryModifierField")
            {
                item.SitePrice = control.ApplyChanges(item.SitePrice);
            }
            else if (control.ID == "SiteCostMonetaryModifierField")
            {
                item.SiteCost = control.ApplyChanges(item.SiteCost);
            }
            else if (control.ID == "ExtraShipFeeMonetaryModifierField")
            {
                item.ShippingDetails.ExtraShipFee = control.ApplyChanges(item.ShippingDetails.ExtraShipFee);
            }
            else
            {
                throw new ControlNotFoundException(control.ID);
            }
        }
    }
}