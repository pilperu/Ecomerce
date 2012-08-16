
using MerchantTribe.Commerce;
using MerchantTribe.Commerce.Catalog;
using System.Collections.ObjectModel;

namespace MerchantTribeStore
{

    partial class BVAdmin_Controls_ProductReviewEditor : MerchantTribe.Commerce.Content.BVUserControl
    {

        public string ReviewID
        {
            get { return Request.QueryString["ReviewID"].ToString(); }
            set { ViewState["ReviewID"] = value; }
        }

        public void LoadReview()
        {
            if (Request.QueryString["ReviewID"] != null)
            {
                ProductReview r = new ProductReview();
                r = MyPage.MTApp.CatalogServices.ProductReviews.Find(ReviewID);
                if (r != null)
                {
                    Product p = MyPage.MTApp.CatalogServices.Products.Find(r.ProductBvin);
                    if (p != null)
                    {
                        this.lblProductName.Text = p.ProductName;
                    }
                    else
                    {
                        this.lblProductName.Text = "Unknown";
                    }
                    if (r.UserID != string.Empty)
                    {
                        MerchantTribe.Commerce.Membership.CustomerAccount u = MyPage.MTApp.MembershipServices.Customers.Find(r.UserID);
                        if (u == null)
                        {
                            this.lblUserName.Text = "Anonymous";
                        }
                        else
                        {
                            this.lblUserName.Text = u.LastName + ", " + u.FirstName + " " + u.Email;
                        }
                    }
                    this.lblReviewDate.Text = r.ReviewDateForTimeZone(MyPage.MTApp.CurrentStore.Settings.TimeZone).ToString();
                    this.chkApproved.Checked = r.Approved;
                    this.KarmaField.Text = r.Karma.ToString();
                    switch (r.Rating)
                    {
                        case ProductReviewRating.ZeroStars:
                            lstRating.SelectedValue = "0";
                            break;
                        case ProductReviewRating.OneStar:
                            lstRating.SelectedValue = "1";
                            break;
                        case ProductReviewRating.TwoStars:
                            lstRating.SelectedValue = "2";
                            break;
                        case ProductReviewRating.ThreeStars:
                            lstRating.SelectedValue = "3";
                            break;
                        case ProductReviewRating.FourStars:
                            lstRating.SelectedValue = "4";
                            break;
                        case ProductReviewRating.FiveStars:
                            lstRating.SelectedValue = "5";
                            break;
                    }
                    this.DescriptionField.Text = r.Description;
                }
                r = null;
            }
        }

        protected void btnCancel_Click(System.Object sender, System.Web.UI.ImageClickEventArgs e)
        {
            if (Request.QueryString["pid"] == null)
            {
                Response.Redirect("ReviewsToModerate.aspx");
            }
            else
            {
                Response.Redirect("Products_Edit_Reviews.aspx?id=" + Request.QueryString["pid"]);
            }
        }

        protected void btnOK_Click(System.Object sender, System.Web.UI.ImageClickEventArgs e)
        {
            try
            {
                if (Request.QueryString["ReviewID"] != null)
                {
                    ProductReview r = new ProductReview();
                    r = MyPage.MTApp.CatalogServices.ProductReviews.Find(ReviewID);
                    if (r != null)
                    {
                        r.Approved = this.chkApproved.Checked;
                        r.Karma = int.Parse(this.KarmaField.Text.Trim());
                        r.Rating = (ProductReviewRating)int.Parse(this.lstRating.SelectedValue);
                        r.Description = this.DescriptionField.Text.Trim();
                    }
                    MyPage.MTApp.CatalogServices.ProductReviews.Update(r);
                    r = null;

                    if (Request.QueryString["pid"] == null)
                    {
                        Response.Redirect("ReviewsToModerate.aspx");
                    }
                    else
                    {
                        Response.Redirect("Products_Edit_Reviews.aspx?id=" + Request.QueryString["pid"]);
                    }
                }
            }
            catch (System.Exception ex)
            {
                EventLog.LogEvent(ex);
            }
        }
    }
}