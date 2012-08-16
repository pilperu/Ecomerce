using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MerchantTribe.Commerce.Catalog;

namespace MerchantTribeStore.BVAdmin.Catalog
{
    public partial class ProductTypes_SortProperties : BaseAdminJsonPage
    {

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);

            if (!Page.IsPostBack)
            {
                string ids = Request.Form["ids"];
                string typeId = Request.Form["typeid"];
                Resort(ids, typeId);
            }

        }

        private void Resort(string ids, string typeId)
        {

            string[] sorted = ids.Split(',');

            int counter = 0;
            foreach (string id in sorted)
            {
                counter++;

                int temp = 0;
                int.TryParse(id, out temp);
                this.MTApp.CatalogServices.ProductTypesXProperties.UpdateSortOrder(typeId, temp, counter);                    
            }

            this.litOutput.Text = "true";
        }

    }
}