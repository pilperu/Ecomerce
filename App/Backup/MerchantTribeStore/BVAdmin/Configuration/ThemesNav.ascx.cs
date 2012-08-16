
namespace MerchantTribeStore
{

    partial class BVAdmin_Configuration_ThemesNav : System.Web.UI.UserControl
    {

        protected override void OnInit(System.EventArgs e)
        {
            base.OnInit(e);
            if (!Page.IsPostBack)
            {
                string themeId = Request.QueryString["id"];
                this.lnkEdit.NavigateUrl += "?id=" + themeId;
                this.lnkCss.NavigateUrl += "?id=" + themeId;
                this.lnkHeader.NavigateUrl += "?id=" + themeId;
                this.lnkButtons.NavigateUrl += "?id=" + themeId;
                this.lnkAssets.NavigateUrl += "?id=" + themeId;
                this.lnkColumns.NavigateUrl += "?id=" + themeId;
            }
        }

    }

}