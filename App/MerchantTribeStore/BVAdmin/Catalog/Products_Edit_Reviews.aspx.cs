using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.ObjectModel;
using MerchantTribe.Commerce;
using MerchantTribe.Commerce.Catalog;
using MerchantTribe.Commerce.Membership;
using System.Collections.Generic;

namespace MerchantTribeStore
{

    partial class products_products_edit_reviews : BaseProductAdminPage
    {

        protected override void OnInit(System.EventArgs e)
        {
            base.OnInit(e);

            this.PageTitle = "Edit Product Reviews";
            this.CurrentTab = AdminTabType.Catalog;
            ValidateCurrentUserHasPermission(SystemPermissions.CatalogView);
        }

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);
            if (!Page.IsPostBack)
            {
                this.CurrentTab = AdminTabType.Catalog;

                LoadReviews();
            }
        }

        private void LoadReviews()
        {
            List<ProductReview> reviews = new List<ProductReview>();
            reviews = MTApp.CatalogServices.ProductReviews.FindByProductId(Request.QueryString["ID"]);

            if (reviews != null)
            {
                this.dlReviews.DataSource = reviews;
                this.dlReviews.DataBind();
            }

            reviews = null;
        }

        protected void dlReviews_DeleteCommand(object source, System.Web.UI.WebControls.DataListCommandEventArgs e)
        {
            string reviewID = (string)dlReviews.DataKeys[e.Item.ItemIndex];
            MTApp.CatalogServices.ProductReviews.Delete(reviewID);
            LoadReviews();
        }

        protected void dlReviews_EditCommand(object source, System.Web.UI.WebControls.DataListCommandEventArgs e)
        {
            string reviewID = (string)dlReviews.DataKeys[e.Item.ItemIndex];
            Response.Redirect("Reviews_Edit.aspx?reviewID="
                                + reviewID + "&pid=" + Server.UrlEncode(Request.QueryString["id"]));
        }

        protected void dlReviews_ItemDataBound(object sender, System.Web.UI.WebControls.DataListItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.AlternatingItem | e.Item.ItemType == ListItemType.Item)
            {
                System.Web.UI.WebControls.Image imgRating;
                imgRating = (Image)e.Item.FindControl("imgRating");
                if (imgRating != null)
                {
                    ProductReviewRating rating = (ProductReviewRating)DataBinder.Eval(e.Item.DataItem, "Rating");
                    switch (rating)
                    {
                        case ProductReviewRating.ZeroStars:
                            imgRating.ImageUrl = this.MTApp.ThemeManager().ButtonUrl("stars0", Request.IsSecureConnection);
                            break;
                        case ProductReviewRating.OneStar:
                            imgRating.ImageUrl = this.MTApp.ThemeManager().ButtonUrl("stars1", Request.IsSecureConnection);
                            break;
                        case ProductReviewRating.TwoStars:
                            imgRating.ImageUrl = this.MTApp.ThemeManager().ButtonUrl("stars2", Request.IsSecureConnection);
                            break;
                        case ProductReviewRating.ThreeStars:
                            imgRating.ImageUrl = this.MTApp.ThemeManager().ButtonUrl("stars3", Request.IsSecureConnection);
                            break;
                        case ProductReviewRating.FourStars:
                            imgRating.ImageUrl = this.MTApp.ThemeManager().ButtonUrl("stars4", Request.IsSecureConnection);
                            break;
                        case ProductReviewRating.FiveStars:
                            imgRating.ImageUrl = this.MTApp.ThemeManager().ButtonUrl("stars5", Request.IsSecureConnection);
                            break;
                    }
                }
            }
        }

        protected void btnNew_Click(System.Object sender, System.Web.UI.ImageClickEventArgs e)
        {
            ProductReview pr = new ProductReview();
            pr.Approved = true;
            pr.Description = "New Review";
            pr.UserID = MTApp.CurrentCustomerId;
            pr.ReviewDateUtc = System.DateTime.UtcNow;
            pr.ProductBvin = Request.QueryString["ID"];
            //If Datalayer.ProductReviewMapper.SaveAsNew(pr) = True Then
            if (MTApp.CatalogServices.ProductReviews.Create(pr) == true)
            {
                Response.Redirect("Reviews_Edit.aspx?reviewID=" + pr.Bvin + "&DOC=1" + "&pid=" + Request.QueryString["id"]);
            }

        }

        protected override bool Save()
        {
            return true;
        }

    }
}