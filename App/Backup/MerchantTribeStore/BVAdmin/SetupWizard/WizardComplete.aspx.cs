using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;

namespace MerchantTribeStore
{

    public partial class BVAdmin_SetupWizard_WizardComplete : BaseAdminPage
    {
        public string TweetUrl { get; set; }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            {
                // Mark Wizard as Complete
                MTApp.CurrentStore.Settings.WizardComplete = true;
                MTApp.AccountServices.Stores.Update(MTApp.CurrentStore);

                TweetUrl = BuildTweetUrl();
            }
        }

        private string BuildTweetUrl()
        {
            string defaultText = "I created a new #MerchantTribe store! Get a Free store at http://bit.ly/mrchtrb : ";

            StringBuilder sb = new StringBuilder();
            sb.Append("https://twitter.com/share");
            sb.Append("?url=" + HttpUtility.UrlEncode(MTApp.CurrentStore.RootUrl()));
            sb.Append("&via=" + HttpUtility.UrlEncode("merchanttribe"));
            sb.Append("&text=" + defaultText);
            sb.Append("&related=");
            sb.Append(HttpUtility.UrlEncode("merchanttribe"));
            sb.Append(":");
            sb.Append(HttpUtility.UrlEncode(","));                
            sb.Append("&count=horizontal");
            sb.Append("&lang=en");
            //sb.Append("&counturl=" + HttpUtility.UrlEncode(Model.DisplayUrl));

            return sb.ToString();   
        }
    }
}