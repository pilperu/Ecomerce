
namespace MerchantTribeStore
{

    public partial class BVAdmin_SetupWizard_WizardThemeInstall : BaseAdminJsonPage
    {
        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);
            string themeId = Request.QueryString["id"];
            this.MTApp.ThemeManager().InstallTheme(themeId);
            MTApp.UpdateCurrentStore();
            Response.Redirect("WizardPayment.aspx");
        }
    }
}