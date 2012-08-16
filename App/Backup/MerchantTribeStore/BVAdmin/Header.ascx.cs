using System;
using System.Web;
using System.Text;
using MerchantTribe.Commerce;
using MerchantTribe.Commerce.Content;
using System.Collections.Specialized;
using MerchantTribe.Commerce.Utilities;

namespace MerchantTribeStore
{

    partial class BVAdmin_Header : BVUserControl
    {
        public bool HideMenu { get; set; }

        protected internal string root = string.Empty;

        public AdminTabType SelectedTab
        {
            get
            {
                if (Session["ActiveAdminTab"] == null)
                {
                    return AdminTabType.Dashboard;
                }
                else
                {
                    return (AdminTabType)Session["ActiveAdminTab"];
                }
            }
            set { Session["ActiveAdminTab"] = value; }
        }

        protected string StoreName { get; set; }
        protected string AppVersion { get; set; }
        protected string BaseUrl { get; set; }
        protected string BaseStoreUrl { get; set; }
        protected string RenderedMenu { get; set; }

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);

            this.BaseUrl = MyPage.MTApp.StoreUrl(true, false);
            this.BaseStoreUrl = MyPage.MTApp.StoreUrl(false, true);
            
            this.AppVersion = WebAppSettings.SystemVersionNumber;
            this.StoreName = MyPage.MTApp.CurrentStore.Settings.FriendlyName;            

            if (!HideMenu)
            {                
                this.RenderedMenu = Helpers.Html.RenderMenu(SelectedTab, MyPage.MTApp);                
            }            
        }      
    }
}