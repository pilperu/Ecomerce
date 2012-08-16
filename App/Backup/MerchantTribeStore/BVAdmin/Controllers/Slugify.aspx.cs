
namespace MerchantTribeStore
{

    partial class BVAdmin_Controllers_Slugify : System.Web.UI.Page
    {

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);
            string input = Request.Form[0];
            if ((input != null))
            {
                this.litMain.Text = MerchantTribe.Web.Text.Slugify(input);
            }
        }
    }
}