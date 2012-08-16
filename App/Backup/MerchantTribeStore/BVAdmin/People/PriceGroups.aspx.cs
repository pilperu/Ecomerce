using System;
using System.Web.UI.WebControls;
using MerchantTribe.Commerce;

namespace MerchantTribeStore
{

    partial class BVAdmin_People_PriceGroups : BaseAdminPage
    {

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (!Page.IsPostBack)
            {
                BindGrids();
            }
        }

        protected void BindGrids()
        {
            PricingGroupsGridView.DataSource = MTApp.ContactServices.PriceGroups.FindAll();
            PricingGroupsGridView.DataBind();
        }

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.PageTitle = "Price Groups";
            this.CurrentTab = AdminTabType.People;
            ValidateCurrentUserHasPermission(MerchantTribe.Commerce.Membership.SystemPermissions.PeopleView);
        }

        protected void SaveImageButton_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            foreach (GridViewRow row in PricingGroupsGridView.Rows)
            {
                string key = (string)PricingGroupsGridView.DataKeys[row.RowIndex].Value;
                MerchantTribe.Commerce.Contacts.PriceGroup pricingGroup = MTApp.ContactServices.PriceGroups.Find(key);

                TextBox NameTextBox = (TextBox)row.FindControl("NameTextBox");
                DropDownList PricingTypeDropDownList = (DropDownList)row.FindControl("PricingTypeDropDownList");
                TextBox AdjustmentAmountTextBox = (TextBox)row.FindControl("AdjustmentAmountTextBox");

                bool needToUpdate = false;
                if (pricingGroup.Name != NameTextBox.Text)
                {
                    pricingGroup.Name = NameTextBox.Text;
                    needToUpdate = true;
                }

                if ((int)pricingGroup.PricingType != int.Parse(PricingTypeDropDownList.SelectedValue))
                {
                    pricingGroup.PricingType = (MerchantTribe.Commerce.Contacts.PricingTypes)int.Parse(PricingTypeDropDownList.SelectedValue);
                    needToUpdate = true;
                }

                if (pricingGroup.AdjustmentAmount != decimal.Parse(AdjustmentAmountTextBox.Text, System.Globalization.NumberStyles.Currency))
                {
                    pricingGroup.AdjustmentAmount = decimal.Parse(AdjustmentAmountTextBox.Text, System.Globalization.NumberStyles.Currency);
                    needToUpdate = true;
                }

                if (needToUpdate)
                {
                    MTApp.ContactServices.PriceGroups.Update(pricingGroup);
                }
            }
            MessageBox1.ShowOk("Price groups updated");
        }

        protected void PricingGroupsGridView_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    MerchantTribe.Commerce.Contacts.PriceGroup pricingGroup = (MerchantTribe.Commerce.Contacts.PriceGroup)e.Row.DataItem;
                    ((TextBox)e.Row.FindControl("NameTextBox")).Text = pricingGroup.Name;
                    ((DropDownList)e.Row.FindControl("PricingTypeDropDownList")).SelectedValue = ((int)pricingGroup.PricingType).ToString();
                    ((TextBox)e.Row.FindControl("AdjustmentAmountTextBox")).Text = pricingGroup.AdjustmentAmount.ToString("N");
                }
            }
        }

        protected void AddNewImageButton_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            MerchantTribe.Commerce.Contacts.PriceGroup pricingGroup = new MerchantTribe.Commerce.Contacts.PriceGroup();
            pricingGroup.Name = "New Pricing Group";
            if (MTApp.ContactServices.PriceGroups.Create(pricingGroup))
            {
                MessageBox1.ShowOk("New price group added");
            }
            else
            {
                MessageBox1.ShowError("An error occurred while price group was being added.");
            }
            BindGrids();
        }

        protected void CancelImageButton_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            BindGrids();
            MessageBox1.ShowOk("Price group values have been reset");
        }

        protected void PricingGroupsGridView_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
        {
            string key = (string)PricingGroupsGridView.DataKeys[e.RowIndex].Value;
            if (MTApp.ContactServices.PriceGroups.Delete(key))
            {
                MessageBox1.ShowOk("Pricing group deleted");
            }
            else
            {
                MessageBox1.ShowError("An error occurred while price group was being deleted");
            }
            BindGrids();
        }
    }
}