using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.ObjectModel;
using MerchantTribe.Commerce;
using MerchantTribe.Commerce.Catalog;
using MerchantTribe.Commerce.Membership;

namespace MerchantTribeStore
{

    partial class BVAdmin_Catalog_Inventory : BaseAdminPage
    {

        private string criteriaSessionKey = "InventoryCriteria";

        private enum Mode
        {
            FilterView = 0,
            EditView = 1
        }

        protected override void OnPreInit(System.EventArgs e)
        {
            base.OnPreInit(e);
            this.PageTitle = "Inventory Edit";
            this.CurrentTab = AdminTabType.Catalog;
            ValidateCurrentUserHasPermission(SystemPermissions.CatalogView);
        }

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);

            if (!Page.IsPostBack)
            {
                MultiView1.ActiveViewIndex = (int)Mode.FilterView;
            }
        }

        protected void GoPressed(ProductSearchCriteria criteria)
        {
            this.Session[criteriaSessionKey] = criteria;
            //GridView1.DataSource = Catalog.Product.FindByCriteria(criteria)
            GridView1.DataBind();
            GridView1.PageIndex = 0;
            topLabel.Visible = (GridView1.PageCount > 1);
            BottomLabel.Visible = (GridView1.PageCount > 1);
            MultiView1.ActiveViewIndex = (int)Mode.EditView;
        }

        protected void SaveChangesImageButton_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            if (Page.IsValid)
            {
                SaveChanges();
            }
            MultiView1.ActiveViewIndex = (int)Mode.FilterView;
        }

        private void SaveChanges()
        {
            Collection<ProductInventory> inventories = new Collection<ProductInventory>();
            foreach (GridViewRow row in this.GridView1.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    //BVAdmin_Controls_InventoryModifications mods = (BVAdmin_Controls_InventoryModifications)row.FindControl("InventoryModifications");
                    //string key = (string)this.GridView1.DataKeys[row.RowIndex].Value;
                    //Product prod = Product.FindByBvinLight(key);
                    //if (prod.Inventory != null && prod.Inventory.Bvin != string.Empty)
                    //{
                    //    if (mods.PostChanges(prod.Inventory))
                    //    {
                    //        inventories.Add(prod.Inventory);
                    //    }
                    //}
                }
            }
            foreach (ProductInventory item in inventories)
            {
                MTApp.CatalogServices.ProductInventories.Update(item);
            }
        }

        protected void GridView1_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Product product = (Product)e.Row.DataItem;
                //((Label)e.Row.FindControl("QuantityAvailableLabel")).Text = product.QuantityAvailable.ToString("N");
                //((Label)e.Row.FindControl("OutOfStockPointLabel")).Text = product.QuantityOutOfStockPoint.ToString("N");
                //((Label)e.Row.FindControl("QuantityReservedLabel")).Text = product.QuantityReserved.ToString("N");
            }
        }

        protected void ImageButton3_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            MultiView1.ActiveViewIndex = (int)Mode.FilterView;
        }

        protected void ObjectDataSource1_Selecting(object sender, System.Web.UI.WebControls.ObjectDataSourceSelectingEventArgs e)
        {
            if (e.ExecutingSelectCount)
            {
                e.InputParameters["rowCount"] = HttpContext.Current.Items["RowCount"];
                HttpContext.Current.Items["RowCount"] = null;
            }
        }

        protected void ObjectDataSource1_Selected(object sender, System.Web.UI.WebControls.ObjectDataSourceStatusEventArgs e)
        {
            if (e.OutputParameters["RowCount"] != null)
            {
                HttpContext.Current.Items["RowCount"] = e.OutputParameters["RowCount"];
            }
        }

        protected void GridView1_PageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            SaveChanges();
        }
    }
}