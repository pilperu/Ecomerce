using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MerchantTribe.Commerce.Storage;

namespace MerchantTribeStore
{

    public partial class BVAdmin_Configuration_ThemesEditAssets_Delete : BaseAdminJsonPage
    {
        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);


            if (!Page.IsPostBack)
            {
                string themeid = Request.Form["themeid"];
                string assetname = Request.Form["assetname"];
                Remove(themeid, assetname);
            }

        }

        private void Remove(string themeid, string asset)
        {
            if (DiskStorage.RemoveAsset(MTApp.CurrentStore.Id, themeid, asset))
            {
                this.litOutput.Text = "{\"result\":true}";
            }
            else
            {
                this.litOutput.Text = "{\"result\":false}";
            }
        }
    }
}