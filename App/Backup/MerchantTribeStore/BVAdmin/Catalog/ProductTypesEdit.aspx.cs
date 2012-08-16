using System.Web.UI;
using System.Collections.Generic;
using MerchantTribe.Commerce.Catalog;
using MerchantTribe.Commerce.Membership;
using System.Text;

namespace MerchantTribeStore
{

    partial class Product_ProductTypes_Edit : BaseAdminPage
    {
        public string TypeId = string.Empty;

        protected override void OnPreInit(System.EventArgs e)
        {
            base.OnPreInit(e);
            this.PageTitle = "Product Types Edit";
            this.CurrentTab = AdminTabType.Catalog;
            ValidateCurrentUserHasPermission(SystemPermissions.CatalogView);
        }

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);
            if (!Page.IsPostBack)
            {
                this.CurrentTab = AdminTabType.Catalog;

                string productID = string.Empty;
                if (Request.QueryString["ID"] != null)
                {
                    productID = Request.QueryString["id"];
                    this.BvinField.Value = productID;
                }
                else
                {
                    msg.ShowError("No product type ID was found.");
                }

                ViewState["ID"] = productID;
                LoadType();
            }

            this.TypeId = this.BvinField.Value;
        }

        private void LoadType()
        {
            ProductType prodType = new ProductType();
            prodType = MTApp.CatalogServices.ProductTypes.Find(this.BvinField.Value);
            if (prodType != null)
            {

                this.ProductTypeNameField.Text = prodType.ProductTypeName;

                LoadPropertyLists();

                if (MTApp.CatalogServices.Products.FindCountByProductType(prodType.Bvin) > 0)
                {
                    msg.ShowWarning("Editing this type will affect all existing products of this type. Any default values you set here will NOT overwrite existing products but any properties that you add or remove will affect existing products.");
                }
            }
            else
            {
                msg.ShowError("Unable to load Product Type ID " + ViewState["ID"]);
            }
        }
    
        protected void lnkClose_Click(object sender, System.EventArgs e)
        {
            // Delete newly created item if user cancels so we don't leave a bunch of "new property"
            if (Request["newmode"] == "1")
            {
                MTApp.CatalogServices.ProductTypeDestroy((string)ViewState["ID"]);
            }
            Response.Redirect("ProductTypes.aspx");
        }

        protected void btnSave_Click(System.Object sender, System.Web.UI.ImageClickEventArgs e)
        {
            msg.ClearMessage();
            ProductType prodType = new ProductType();
            prodType = MTApp.CatalogServices.ProductTypes.Find(this.BvinField.Value);
            if (prodType != null)
            {

                prodType.ProductTypeName = this.ProductTypeNameField.Text;

                if (MTApp.CatalogServices.ProductTypes.Update(prodType) == true)
                {
                    prodType = null;
                    Response.Redirect("ProductTypes.aspx");
                }
                else
                {
                    prodType = null;
                    msg.ShowError("Error: Couldn't Save Product Type!");
                }
            }
            else
            {
                msg.ShowError("Couldn't Load Product Type to Update!");
            }

        }

        private void LoadPropertyLists()
        {

            List<ProductProperty> selectedProperties = MTApp.CatalogServices.ProductPropertiesFindForType(this.BvinField.Value);
            this.litProducts.Text = RenderProperties(selectedProperties);

            this.lstAvailableProperties.DataSource = MTApp.CatalogServices.ProductPropertiesFindNotAssignedToType((string)ViewState["ID"]);
            this.lstAvailableProperties.DataTextField = "PropertyName";
            this.lstAvailableProperties.DataValueField = "id";
            this.lstAvailableProperties.DataBind();
        }

        protected void btnAddProperty_Click(System.Object sender, System.Web.UI.ImageClickEventArgs e)
        {            
            string typeBvin = (string)ViewState["ID"];
            long propertyId = long.Parse(lstAvailableProperties.SelectedValue);
            MTApp.CatalogServices.ProductTypeAddProperty(typeBvin, propertyId);
            this.LoadPropertyLists();
        }

        private string RenderProperties(List<ProductProperty> props)
        {
            string result = string.Empty;

            StringBuilder sb = new StringBuilder();

            if ((props != null))
            {
                foreach (ProductProperty p in props)
                {
                    RenderSingleProperty(p, sb);
                }
            }

            result = sb.ToString();

            return result;
        }
        private void RenderSingleProperty(ProductProperty p, StringBuilder sb)
        {

            sb.Append("<div class=\"dragitem\" id=\"" + p.Id.ToString() + "\">");

            sb.Append("<table border=\"0\" cellspacing=\"0\" cellpadding=\"2\" width=\"100%\">");
            sb.Append("<tr>");
            sb.Append("<td width=\"25%\">" + p.FriendlyTypeName + "</td>");
            sb.Append("<td width=\"50%\">" + p.PropertyName + "</td>");
            sb.Append("<td><a href=\"ProductTypes_RemoveProperty.aspx\" title=\"Remove Product Property\" id=\"rem" + p.Id.ToString() + "\" class=\"trash\"><img src=\"../../images/system/trashcan.png\" alt=\"Remove Property\" border=\"0\" /></a></td>");
            sb.Append("<td class=\"handle\" align=\"right\"><img src=\"../../images/system/draghandle.png\" alt=\"sort\" border=\"0\" /></td>");
            sb.Append("</tr>");
            sb.Append("</table>");
            sb.Append("</div>");

        }
      
        //protected void btnMovePropertyUp_Click(System.Object sender, System.Web.UI.ImageClickEventArgs e)
        //{
        //    string typeBvin = (string)ViewState["ID"];
        //    long selected = long.Parse(this.lstProperties.SelectedValue);
        //    MTApp.CatalogServices.ProductTypeMovePropertyUp(typeBvin, selected);            
        //    this.LoadPropertyLists();
        //    foreach (System.Web.UI.WebControls.ListItem li in this.lstProperties.Items)
        //    {
        //        if (li.Value == selected.ToString())
        //        {
        //            lstProperties.ClearSelection();
        //            li.Selected = true;
        //        }
        //    }
        //}

        //protected void btnMovePropertyDown_Click(System.Object sender, System.Web.UI.ImageClickEventArgs e)
        //{
        //    string typeBvin = (string)ViewState["ID"];
        //    long selected = long.Parse(this.lstProperties.SelectedValue);
        //    MTApp.CatalogServices.ProductTypeMovePropertyDown(typeBvin, selected);            
        //    this.LoadPropertyLists();
        //    foreach (System.Web.UI.WebControls.ListItem li in this.lstProperties.Items)
        //    {
        //        if (li.Value == selected.ToString())
        //        {
        //            lstProperties.ClearSelection();
        //            li.Selected = true;
        //        }
        //    }
        //}

      
    }
}