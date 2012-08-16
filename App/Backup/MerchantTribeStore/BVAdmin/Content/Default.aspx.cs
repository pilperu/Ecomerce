using MerchantTribe.Commerce.Membership;
using MerchantTribe.Commerce.Catalog;
using System.Collections.Generic;

namespace MerchantTribeStore
{

    partial class BVAdmin_Content_Default : BaseAdminPage
    {
        protected override void OnPreInit(System.EventArgs e)
        {
            base.OnPreInit(e);
            this.PageTitle = "Homepage Content";
            this.CurrentTab = AdminTabType.Content;
            ValidateCurrentUserHasPermission(SystemPermissions.ContentView);
        }

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);
        }

    }
}