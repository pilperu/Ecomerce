using System.Web.UI;
using MerchantTribe.Commerce.Membership;

namespace MerchantTribeStore
{

    partial class Reviews_Edit : BaseProductAdminPage
    {
        protected override void OnPreInit(System.EventArgs e)
        {
            base.OnPreInit(e);
            this.PageTitle = "Edit Product Review";
            this.CurrentTab = AdminTabType.Catalog;
            ValidateCurrentUserHasPermission(SystemPermissions.CatalogView);
        }

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);
            if (!Page.IsPostBack)
            {
                this.CurrentTab = AdminTabType.Catalog;

                if (Request.QueryString["pid"] != null)
                {
                    ViewState["pid"] = Request.QueryString["pid"];
                }

                if (Request.QueryString["ReviewID"] != null)
                {
                    string rid = string.Empty;
                    rid = Request.QueryString["ReviewID"];
                    ProductReviewEditor1.ReviewID = rid;
                    ProductReviewEditor1.LoadReview();
                }
            }
        }

        protected override bool Save()
        {
            //we do not want them to be able to click "save and continue"
            return false;
        }
    }
}