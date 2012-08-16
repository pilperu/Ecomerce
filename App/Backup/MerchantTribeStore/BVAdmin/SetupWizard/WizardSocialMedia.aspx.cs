using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MerchantTribe.Commerce;

namespace MerchantTribeStore.BVAdmin.SetupWizard
{
    public partial class WizardSocialMedia : BaseAdminPage
    {
        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.PageTitle = "Setup Wizard | Social Media";
            this.CurrentTab = AdminTabType.Configuration;
        }

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);
            if (!Page.IsPostBack)
            {
                this.twitterhandle.Text = MTApp.CurrentStore.Settings.Twitter.TwitterHandle;                 
            }
        }


        protected void btnSaveMain_Click(object sender, ImageClickEventArgs e)
        {

            MTApp.CurrentStore.Settings.Twitter.TwitterHandle = this.twitterhandle.Text.Trim();
            if (this.twitterhandle.Text.Trim().Length > 0)
            {
                MTApp.CurrentStore.Settings.Twitter.UseTwitter = true;
            }
            MTApp.UpdateCurrentStore();
            Response.Redirect("WizardComplete.aspx");
        }

    }
}