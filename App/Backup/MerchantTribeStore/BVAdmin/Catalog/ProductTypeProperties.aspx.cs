using System.Web.UI;
using MerchantTribe.Commerce.Catalog;
using MerchantTribe.Commerce.Membership;

namespace MerchantTribeStore
{

    partial class BVAdmin_Catalog_ProductTypeProperties : BaseAdminPage
    {
        protected override void OnPreInit(System.EventArgs e)
        {
            base.OnPreInit(e);
            this.PageTitle = "Product Properties";
            this.CurrentTab = AdminTabType.Catalog;
            ValidateCurrentUserHasPermission(SystemPermissions.CatalogView);
        }

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);
            if (!Page.IsPostBack)
            {
                this.CurrentTab = AdminTabType.Catalog;
                FillList();
            }

        }

        private void FillList()
        {
            this.dgList.DataSource = MTApp.CatalogServices.ProductProperties.FindAll();
            this.dgList.DataBind();
        }

        protected void btnNew_Click(System.Object sender, System.Web.UI.ImageClickEventArgs e)
        {
            msg.ClearMessage();

            ProductProperty p = new ProductProperty();
            p.DisplayName = "New Property";
            p.TypeCode = (ProductPropertyType)int.Parse(lstProductType.SelectedValue);
            if (MTApp.CatalogServices.ProductProperties.Create(p) == true)
            {
                FillList();
            }
            else
            {
                msg.ShowError("Error while attempting to create new property.");
            }
        }

        protected void dgList_EditCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
        {
            long editID = (long)dgList.DataKeys[e.Item.ItemIndex];
            Response.Redirect("~/BVAdmin/Catalog/ProductTypePropertiesEdit.aspx?id=" + editID);
        }

        protected void dgList_DeleteCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
        {
            long deleteID = (long)dgList.DataKeys[e.Item.ItemIndex];
            MTApp.CatalogServices.ProductPropertiesDestroy(deleteID);
            FillList();
        }
    }
}