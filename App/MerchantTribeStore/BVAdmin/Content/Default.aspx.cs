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
            this.PageTitle = "Content";
            this.CurrentTab = AdminTabType.Content;
            ValidateCurrentUserHasPermission(SystemPermissions.ContentView);
        }

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);
            if (!Page.IsPostBack)
            {
                PopulatePages();
                this.chkFlexHome.Checked = MTApp.CurrentStore.Settings.HomePageIsFlex;
                string customBvin = MTApp.CurrentStore.Settings.HomePageIsFlexBvin;
                if (this.lstPages.Items.FindByValue(customBvin) != null)
                {
                    this.lstPages.ClearSelection();
                    this.lstPages.Items.FindByValue(customBvin).Selected = true;
                }

                this.pnlColumns.Visible = (!this.chkFlexHome.Checked);
                
            }
        }

        private void PopulatePages()
        {
            this.lstPages.Items.Clear();
            List<CategorySnapshot> pages = MTApp.CatalogServices.Categories.FindAllCustomPages();
            foreach (var snap in pages)
            {
                this.lstPages.Items.Add(new System.Web.UI.WebControls.ListItem(
                                    snap.Name, snap.Bvin));
            }
        }

        protected void btnSave_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            MTApp.CurrentStore.Settings.HomePageIsFlex = this.chkFlexHome.Checked;
            MTApp.CurrentStore.Settings.HomePageIsFlexBvin = this.lstPages.SelectedItem.Value;
            MTApp.AccountServices.Stores.Update(MTApp.CurrentStore);

            this.MessageBox1.ShowOk("Changes Saved at " + System.DateTime.Now.ToString());
            this.pnlColumns.Visible = (!this.chkFlexHome.Checked);
        }
    }
}