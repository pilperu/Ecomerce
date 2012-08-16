using System.Web.UI;
using System.Collections.Generic;
using MerchantTribe.Commerce.Catalog;

namespace MerchantTribeStore
{

    partial class BVAdmin_Catalog_OptionItems_Sort : BaseAdminJsonPage
    {

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);


            if (!Page.IsPostBack)
            {
                string ids = Request.Form["ids"];
                string optionId = Request.Form["optionid"];
                Resort(ids, optionId);
            }

        }

        private void Resort(string ids, string optionId)
        {

            string[] sorted = ids.Split(',');
            List<string> l = new List<string>();
            foreach (string id in sorted)
            {
                l.Add(id);
            }
                        
            if ((MTApp.CatalogServices.ProductOptions.ResortOptionItems(optionId, l)))
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