using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MerchantTribe.Commerce.Catalog;
using System.Collections.ObjectModel;

namespace MerchantTribeStore
{

    public partial class BVAdmin_Controls_DashboardAlerts : MerchantTribe.Commerce.Content.BVUserControl
    {

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            LoadAlerts();
        }

        private void LoadAlerts()
        {
            List<ProductReview> reviews = MyPage.MTApp.CatalogServices.ProductReviews.FindNotApproved(1,100);
            if (reviews == null) return;
            if (reviews.Count() < 1) return;

            RenderAlert(reviews.Count().ToString() + " Reviews to <a href=\"catalog/ReviewsToModerate.aspx\">Moderate</a>");
        }

        private void RenderAlert(string message)
        {
            this.litAlerts.Text += "<div class=\"flash-message-info\">" + message + "</div>";
        }
    }
}