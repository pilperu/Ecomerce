using System;
using System.Collections.ObjectModel;
using System.Web.UI;
using System.Web.UI.WebControls;
using MerchantTribe.Commerce.Catalog;
using MerchantTribe.Commerce.Content;
using MerchantTribe.Commerce.Membership;
using MerchantTribe.Commerce.Utilities;
using MerchantTribe.Commerce;

namespace MerchantTribeStore.BVAdmin.Catalog
{
    public partial class Categories_FinishedEditing : BaseAdminPage
    {
        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.PageTitle = "Finished Editing";
            this.CurrentTab = AdminTabType.Catalog;
            ValidateCurrentUserHasPermission(SystemPermissions.CatalogView);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            string bvin = Request.QueryString["id"];
            MTApp.IsEditMode = false;
            Response.Redirect("Categories_EditFlexPage.aspx?id=" + bvin);
        }
    }
}