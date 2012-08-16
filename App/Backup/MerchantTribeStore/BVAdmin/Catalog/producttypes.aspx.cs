using System.Web.UI;
using System.Web.UI.WebControls;
using MerchantTribe.Commerce.Catalog;
using MerchantTribe.Commerce.Membership;

namespace MerchantTribeStore
{

    partial class Products_ProductTypes : BaseAdminPage
    {
        protected override void OnPreInit(System.EventArgs e)
        {
            base.OnPreInit(e);
            this.PageTitle = "Product Types";
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
            this.dgList.DataSource = MTApp.CatalogServices.ProductTypes.FindAll();
            this.dgList.DataBind();
        }

        protected void btnNew_Click(System.Object sender, System.Web.UI.ImageClickEventArgs e)
        {
            ProductType pt = new ProductType();
            if (MTApp.CatalogServices.ProductTypes.CreateAsNew(pt))
            {
                Response.Redirect("~/BVAdmin/Catalog/ProductTypesEdit.aspx?newmode=1&id=" + pt.Bvin);
            }
            else
            {
                msg.ShowError("Error while attempting to create new type.");
            }
        }

        protected void dgList_EditCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
        {
            string editID = (string)dgList.DataKeys[e.Item.ItemIndex];
            Response.Redirect("~/BVAdmin/Catalog/ProductTypesEdit.aspx?id=" + editID);
        }

        protected void dgList_DeleteCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
        {
            string deleteID = (string)dgList.DataKeys[e.Item.ItemIndex];
            MTApp.CatalogServices.ProductTypeDestroy(deleteID);
            FillList();
        }

        protected void dgList_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.AlternatingItem | e.Item.ItemType == ListItemType.Item)
            {
                ImageButton deleteButton;
                deleteButton = (ImageButton)e.Item.FindControl("DeleteButton");
                if (deleteButton != null)
                {
                    deleteButton.Visible = MTApp.CatalogServices.Products.FindCountByProductType((string)dgList.DataKeys[e.Item.ItemIndex]) <= 0;
                }
            }
        }
    }
}