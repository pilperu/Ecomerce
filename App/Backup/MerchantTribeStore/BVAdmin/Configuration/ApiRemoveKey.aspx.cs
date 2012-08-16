using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MerchantTribe.Commerce.Accounts;

namespace MerchantTribeStore.BVAdmin.Configuration
{
    public partial class ApiRemoveKey : BaseAdminJsonPage
    {
        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);

            if (!Page.IsPostBack)
            {
                string key = Request.Form["id"]; 
                Delete(key);
            }
        }

        private void Delete(string id)
        {
            bool result = false;
           
            long lid = 0;
            long.TryParse(id, out lid);
            result = MTApp.AccountServices.ApiKeys.Delete(lid);

            if ((result))
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