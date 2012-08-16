using System;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using MerchantTribe.Commerce;
using MerchantTribe.Commerce.Controls;
using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace MerchantTribeStore
{

    partial class BVAdmin_Controls_InventoryModifications : System.Web.UI.UserControl
    {

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (!this.Page.ClientScript.IsClientScriptBlockRegistered(this.GetType(), "flipRow"))
            {
                string script = "function flipRow(checkBoxId, rowId){ " + "   if (document.getElementById(checkBoxId).checked){" + "       document.getElementById(rowId).style.backgroundColor = '#ffc';" + "   }else{" + "       document.getElementById(rowId).style.backgroundColor = 'transparent';" + "   }" + "} ";
                this.Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "flipRow", script, true);
            }

            if (!this.Page.ClientScript.IsClientScriptBlockRegistered(this.GetType(), "checkBoxForOption"))
            {
                string script = "function checkBoxForOption(textbox, checkBoxId, rowId){" + "   if (textbox.value != ''){" + "       document.getElementById(checkBoxId).checked = true;" + "   }else{" + "       document.getElementById(checkBoxId).checked = false;" + "   }" + "   flipRow(checkBoxId, rowId);" + "}";
                this.Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "checkBoxForOption", script, true);
            }


            CheckBox currCheckBox = null;
            ITextBoxBasedControl currTextBoxBasedControl = null;
            foreach (System.Web.UI.Control rowControl in this.InventoryModificationsPanel.Controls)
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
                                            ((HtmlTableRow)rowControl).Attributes.Add("style", "background-color: none");
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

        }

        public bool PostChanges(MerchantTribe.Commerce.Catalog.ProductInventory item)
        {
            bool process = false;
            Collection<ModificationControlBase> controls = new Collection<ModificationControlBase>();
            foreach (System.Web.UI.Control rowControl in this.InventoryModificationsPanel.Controls)
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

            bool result = false;
            foreach (ModificationControlBase control in controls)
            {
                if (control is ModificationControl<int>)
                {
                    ModificationControl<int> integerControl = (ModificationControl<int>)control;
                    result = true;
                    MakeChanges(integerControl, item);
                }
                else if (control is ModificationControl<double>)
                {
                    ModificationControl<double> floatControl = (ModificationControl<double>)control;
                    result = true;
                    MakeChanges(floatControl, item);
                }
                else if (control is ModificationControl<decimal>)
                {
                    ModificationControl<decimal> monetaryControl = (ModificationControl<decimal>)control;
                    result = true;
                    MakeChanges(monetaryControl, item);
                }
            }
            return result;
        }

        protected void MakeChanges(ModificationControl<int> control, MerchantTribe.Commerce.Catalog.ProductInventory item)
        {
            if (control.ID == "QuantityAvailableIntegerModifierField")
            {
                item.QuantityOnHand = control.ApplyChanges((int)item.QuantityOnHand);
            }
            else if (control.ID == "QuantityOutOfStockPointIntegerModifierField")
            {
                item.LowStockPoint = control.ApplyChanges((int)item.LowStockPoint);
            }
            else if (control.ID == "QuantityReserveIntegerModifierField")
            {
                item.QuantityReserved = control.ApplyChanges((int)item.QuantityReserved);
            }
            else
            {
                throw new ControlNotFoundException(control.ID);
            }
        }

        protected void MakeChanges(ModificationControl<double> control, MerchantTribe.Commerce.Catalog.ProductInventory item)
        {
            throw new ControlNotFoundException(control.ID);
        }

        protected void MakeChanges(ModificationControl<decimal> control, MerchantTribe.Commerce.Catalog.ProductInventory item)
        {
            throw new ControlNotFoundException(control.ID);
        }

    }
}