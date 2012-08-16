using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MerchantTribeStore
{

    public partial class BVAdmin_Configuration_ThemesEditButtons_Delete : BaseAdminJsonPage
    {
        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);


            if (!Page.IsPostBack)
            {
                string themeid = Request.Form["themeid"];
                string buttonname = Request.Form["buttonname"];
                Remove(themeid, buttonname);
            }

        }

        private void Remove(string themeid, string buttonname)
        {
            if (MerchantTribe.Commerce.Storage.DiskStorage.RemoveThemeButton(MTApp.CurrentStore.Id, themeid, buttonname))
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