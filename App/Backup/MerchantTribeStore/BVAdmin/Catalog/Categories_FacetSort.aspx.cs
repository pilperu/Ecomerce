using System.Collections.Generic;
using System.Web.UI;
using MerchantTribe.Commerce.Catalog;
using System;
using System.Collections.ObjectModel;

namespace MerchantTribeStore.BVAdmin.Catalog
{
    public partial class Categories_FacetSort : BaseAdminJsonPage
    {

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);

            if (!Page.IsPostBack)
            {
                string ids = Request.Form["ids"];
                string parentId = Request.Form["bvin"];
                if (parentId.StartsWith("children"))
                {
                    parentId = parentId.Substring(8);
                }

                Resort(ids, parentId);
            }

        }

        private void Resort(string ids, string parentId)
        {
            long theParentId = 0;
            long.TryParse(parentId, out theParentId);

            CategoryFacetManager manager = CategoryFacetManager.InstantiateForDatabase(MTApp.CurrentRequestContext);

            List<CategoryFacet> children = manager.FindByParent(theParentId);

            string[] sorted = ids.Split(',');
            List<long> l = new List<long>();
            foreach (string id in sorted)
            {
                long temp = 0;
                long.TryParse(id, out temp);
                if (temp > 0)
                {
                    l.Add(temp);
                }
            }

            int counter = 1;
            foreach (long s in l)
            {
                foreach (CategoryFacet child in children)
                {
                    if (child.Id == s)
                    {
                        child.SortOrder = counter;
                        counter += 1;
                        manager.Update(child);
                    }
                }
            }

            this.litOutput.Text = "true";

        }

    }
}