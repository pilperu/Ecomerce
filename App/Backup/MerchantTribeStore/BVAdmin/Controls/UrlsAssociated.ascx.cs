using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MerchantTribe.Commerce.Content;
using System.Text;

namespace MerchantTribeStore.BVAdmin.Controls
{
    public partial class UrlsAssociated : MerchantTribe.Commerce.Content.BVUserControl
    {
        public string ObjectId
        {
            get { return this.ObjectBvin.Value; }
            set { this.ObjectBvin.Value = value; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                //RegisterJquery
                Page.ClientScript.RegisterClientScriptInclude("301redirectlist", Page.ResolveUrl("~/bvadmin/controls/urlsassociated.js"));
            }
            LoadUrls();
        }

        public void LoadUrls()
        {
            List<CustomUrl> all = MyPage.MTApp.ContentServices.CustomUrls.FindBySystemData(this.ObjectId);
            if (all.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("<ul class=\"redirects301\">");
                foreach (CustomUrl c in all)
                {
                    sb.Append("<li>");
                    sb.Append(HttpUtility.HtmlEncode(c.RequestedUrl));
                    sb.Append(" <a href=\"#\" class=\"remove301\" id=\"remove" + c.Bvin + "\">Remove");                    
                    sb.Append("</a></li>");
                }
                sb.Append("</ul>");
                this.litMain.Text = sb.ToString();
            }
        }
    }
}