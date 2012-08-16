using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.ObjectModel;
using MerchantTribe.Commerce.Content;
using MerchantTribe.Commerce.Membership;
using MerchantTribe.Commerce.Storage;

namespace MerchantTribeStore
{

    public partial class BVAdmin_Content_StoreAssets : BaseAdminPage
    {
        protected override void OnInit(System.EventArgs e)
        {
            base.OnInit(e);
            this.PageTitle = "Page Images";
            this.CurrentTab = AdminTabType.Content;
            ValidateCurrentUserHasPermission(SystemPermissions.SettingsView);
        }

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);

            if (!Page.IsPostBack)
            {
                LoadTheInfo();
            }

        }

        void LoadTheInfo()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("<ul class=\"removablelist\">");

            List<MerchantTribe.Commerce.Storage.StoreAssetSnapshot> assets =
                MerchantTribe.Commerce.Storage.DiskStorage.ListStoreAssets(MTApp);
            foreach (MerchantTribe.Commerce.Storage.StoreAssetSnapshot snapshot in assets)
            {
                sb.Append("<li id=\"" + snapshot.FileName + "\">");
                sb.Append("<a href=\"#\" title=\"" + snapshot.FileName + "\" class=\"deleteitem\"><img src=\"../../images/system/trashcan.png\" alt=\"Delete Item\" /></a>");
                sb.Append("<span class=\"preview\"><img src=\"" + snapshot.Url(true) + "?uid=" + System.Guid.NewGuid().ToString() + "\" alt=\"" + snapshot.FileName + "\" /></span>");
                sb.Append("&nbsp;&nbsp;<span class=\"details\">" + snapshot.FileName + "</span><div class=\"clear smalldetails\">" + snapshot.Url(false) + "</div>");
                sb.Append("</li>");
            }
            sb.Append("</ul>");

            this.litMain.Text = sb.ToString();
        }

        protected void btnUpload_Click(object sender, ImageClickEventArgs e)
        {
            if (this.fileupload1.HasFile)
            {
                MerchantTribe.Commerce.Storage.DiskStorage.UploadStoreAsset(MTApp.CurrentStore.Id, this.fileupload1.PostedFile);
                LoadTheInfo();
            }
        }
    }
}