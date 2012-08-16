using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MerchantTribe.Commerce.Catalog;

namespace MerchantTribeStore.BVAdmin.Catalog
{
    public partial class ProductTypes_RemoveProperty : BaseAdminJsonPage
    {

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);

            if (!Page.IsPostBack)
            {
                string propertyIdString = Request.Form["id"];
                int propertyId = 0;
                int.TryParse(propertyIdString, out propertyId);
                string typeId = Request.Form["typeid"];

                Delete(propertyId, typeId);
            }
        }

        private void Delete(int propertyId, string typeId)
        {
            bool result = false;

            MTApp.CatalogServices.ProductTypeRemoveProperty(typeId, propertyId);
            result = true;

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