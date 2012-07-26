using MerchantTribe.Commerce.Content;

namespace MerchantTribeStore
{
    public abstract class BaseProductAdminPage : BaseAdminPage
    {

        protected NotifyClickControl _ProductNavigator = new NotifyClickControl();

        protected abstract bool Save();

        protected override void OnInit(System.EventArgs e)
        {
            base.OnInit(e);
            PopulateMenuControl();
            // Added after conversion to wireup
            _ProductNavigator.Clicked += ProductNavigator_Clicked;
        }

        protected override void OnUnload(System.EventArgs e)
        {
            base.OnUnload(e);
            // Remove the handler to prevent a memory leak
            _ProductNavigator.Clicked -= ProductNavigator_Clicked;
        }

        private void PopulateMenuControl()
        {
            System.Web.UI.Control nav = Page.Master.FindControl("NavContent");
            if (nav != null)
            {
                System.Web.UI.Control c = nav.FindControl("ProductNavigator");
                if (c != null)
                {
                    this._ProductNavigator = (NotifyClickControl)c;
                }
            }
        }

        protected void ProductNavigator_Clicked(object sender, MerchantTribe.Commerce.Content.NotifyClickControl.ClickedEventArgs e)
        {
            if (!this.Save())
            {
                e.ErrorOccurred = true;
            }
        }
    }
}