using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MerchantTribe.Commerce.Membership;
using MerchantTribe.Commerce.Catalog;
using MerchantTribe.Commerce.Marketing;
using MerchantTribe.Commerce.Utilities;
using MerchantTribe.Commerce;

namespace MerchantTribeStore.BVAdmin.Marketing
{
    public partial class RewardsPoints : BaseAdminPage
    {

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.PageTitle = MTApp.CurrentStore.Settings.RewardsPointsName;
            this.CurrentTab = AdminTabType.Marketing;
            ValidateCurrentUserHasPermission(SystemPermissions.MarketingView);
        }        

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (!Page.IsPostBack)
            {
                LoadInfo();
            }
        }

        protected void btnSave_Click(object sender, ImageClickEventArgs e)
        {
            this.MessageBox1.ClearMessage();

            if (SaveInfo())
            {
                this.MessageBox1.ShowOk("Changed Saved");
            }

        }

        private void LoadInfo()
        {
            CustomerPointsManager manager = CustomerPointsManager.InstantiateForDatabase(MTApp.CurrentStore.Settings.RewardsPointsIssuedPerDollarSpent,
                                                                      MTApp.CurrentStore.Settings.RewardsPointsNeededPerDollarCredit,
                                                                      MTApp.CurrentStore.Id);

            int pointsIssued = manager.TotalPointsIssuedForStore(MTApp.CurrentStore.Id);
            this.lblPointsIssued.Text = pointsIssued.ToString();
            this.lblPointsIssuedValue.Text = manager.DollarCreditForPoints(pointsIssued).ToString("c");

            int pointsReserverd = manager.TotalPointsReservedForStore(MTApp.CurrentStore.Id);
            this.lblPointsReserved.Text = pointsReserverd.ToString();
            this.lblPointsReservedValue.Text = manager.DollarCreditForPoints(pointsReserverd).ToString("c");

            this.RewardsNameField.Text = MTApp.CurrentStore.Settings.RewardsPointsName;
            this.chkPointForDollars.Checked = MTApp.CurrentStore.Settings.RewardsPointsOnPurchasesActive;
            this.chkPointsForProducts.Checked = MTApp.CurrentStore.Settings.RewardsPointsOnProductsActive;
            this.PointsCreditField.Text = MTApp.CurrentStore.Settings.RewardsPointsNeededPerDollarCredit.ToString();
            this.PointsPerDollarField.Text = MTApp.CurrentStore.Settings.RewardsPointsIssuedPerDollarSpent.ToString();
        }

        private bool SaveInfo()
        {
            MTApp.CurrentStore.Settings.RewardsPointsName = this.RewardsNameField.Text.Trim();
            MTApp.CurrentStore.Settings.RewardsPointsOnProductsActive = this.chkPointsForProducts.Checked;
            MTApp.CurrentStore.Settings.RewardsPointsOnPurchasesActive = this.chkPointForDollars.Checked;
            int pointPerDollar = 1;
            if (int.TryParse(this.PointsPerDollarField.Text, out pointPerDollar))
            {
                MTApp.CurrentStore.Settings.RewardsPointsIssuedPerDollarSpent = pointPerDollar;
            }
            else
            {
                this.MessageBox1.ShowWarning("Please enter a valid point about issued");
                return false;
            }
            int pointsPerCredit = 100;
            if (int.TryParse(this.PointsCreditField.Text, out pointsPerCredit))
            {
                MTApp.CurrentStore.Settings.RewardsPointsNeededPerDollarCredit = pointsPerCredit;
            }
            else
            {
                this.MessageBox1.ShowWarning("Please enter a valid point amount for redemption");
                return false;
            }

            return MTApp.UpdateCurrentStore();            
        }
    }
}