
using MerchantTribe.Commerce;
using MerchantTribe.Commerce.Catalog;
using MerchantTribe.Commerce.Content;
using System.Collections.Specialized;
using System.Web.UI.WebControls;

namespace MerchantTribeStore
{

    partial class BVAdmin_Controls_CategoryPicker : BVUserControl
    {
        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);
            if (!Page.IsPostBack)
            {
                BindCategoriesGridView();
            }
        }

        protected void BindCategoriesGridView()
        {
            CategoriesGridView.DataSource = Category.ListFullTreeWithIndents(MyPage.MTApp.CurrentRequestContext);
            CategoriesGridView.DataKeyNames = new string[] { "value" };
            CategoriesGridView.DataBind();
        }

        public StringCollection SelectedCategories
        {
            get
            {
                StringCollection result = new StringCollection();
                foreach (GridViewRow row in CategoriesGridView.Rows)
                {
                    if (((CheckBox)row.Cells[0].FindControl("chkSelected")).Checked)
                    {
                        result.Add((string)CategoriesGridView.DataKeys[row.RowIndex].Value);
                    }
                }
                return result;
            }
        }
    }
}