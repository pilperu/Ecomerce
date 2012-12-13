using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MerchantTribe.Commerce;
using MerchantTribe.Commerce.Security;
using MerchantTribe.Commerce.Membership;

namespace MerchantTribeStore.BVAdmin.Configuration
{
    public partial class Caching : BaseAdminPage
    {

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.PageTitle = "Caching";
            this.CurrentTab = AdminTabType.Configuration;
            ValidateCurrentUserHasPermission(SystemPermissions.SettingsView);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (!Page.IsPostBack)
            {                
            }
        }

        protected void btnClearStoreCache_Click(object sender, EventArgs e)
        {
            CacheManager.ClearStoreById(MTApp.CurrentStore.Id);
            this.msg.ShowOk("Store cache cleared at " + DateTime.Now);
        }
    }
}