using System;
using System.Web;
using System.Web.UI.WebControls;
using MerchantTribe.Commerce;

namespace MerchantTribeStore
{

    partial class BVAdmin_Controls_ProductEditMenu : MerchantTribe.Commerce.Content.NotifyClickControl
    {

        protected void btnContinue_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            if (this.NotifyClicked("Continue"))
            {
                Response.Redirect("~/BVAdmin/Catalog/Default.aspx");
            }
        }

        protected void lnkGeneral_Click(object sender, System.EventArgs e)
        {
            if (this.NotifyClicked("GeneralOptions"))
            {
                RedirectPath("~/BVAdmin/Catalog/products_edit.aspx");
            }
        }

        protected void lnkCustomerChoices_Click(object sender, System.EventArgs e)
        {
            if (this.NotifyClicked("CustomerChoices"))
            {
                RedirectPath("~/BVAdmin/Catalog/ProductChoices.aspx");
            }
        }

        protected void RedirectPath(string path)
        {
            if (string.IsNullOrEmpty(Request.Params["id"]))
            {
                string id = (string)HttpContext.Current.Items["productid"];
                if (!string.IsNullOrEmpty(id))
                {
                    Response.Redirect(path + "?id=" + id);
                }
            }
            else
            {
                Response.Redirect(path + "?id=" + Request.Params["id"]);
            }

        }

        protected void lnkProductReviews_Click(object sender, System.EventArgs e)
        {
            if (this.NotifyClicked("ProductReviews"))
            {
                RedirectPath("~/BVAdmin/Catalog/Products_Edit_Reviews.aspx");
            }
        }

        protected void lnkUpSellCrossSell_Click(object sender, System.EventArgs e)
        {
            if (this.NotifyClicked("ProductUpSellsCrossSells"))
            {
                RedirectPath("~/BVAdmin/Catalog/ProductUpSellCrossSell.aspx");
            }
        }

        protected void lnkAdditionalImages_Click(object sender, System.EventArgs e)
        {
            if (this.NotifyClicked("AdditionalImages"))
            {
                RedirectPath("~/BVAdmin/Catalog/Products_Edit_Images.aspx");
            }
        }

        protected void lnkVolumeDiscounts_Click(object sender, System.EventArgs e)
        {
            if (this.NotifyClicked("VolumeDiscounts"))
            {
                RedirectPath("~/BVAdmin/Catalog/ProductVolumeDiscounts.aspx");
            }
        }
       
        protected void lnkCategories_Click(object sender, System.EventArgs e)
        {
            if (this.NotifyClicked("Categories"))
            {
                RedirectPath("~/BVAdmin/Catalog/Products_Edit_Categories.aspx");
            }
        }

        protected void lnkVariants_Click(object sender, System.EventArgs e)
        {
            if (this.NotifyClicked("Variants"))
            {
                RedirectPath("~/BVAdmin/Catalog/ProductChoices_Variants.aspx");
            }
        }
        protected void lnkInfoTabs_Click(object sender, EventArgs e)
        {
            if (this.NotifyClicked("Tabs"))
            {
                RedirectPath("~/BVAdmin/Catalog/ProductsEdit_Tabs.aspx");
            }
        }

        protected void lnkInventory_Click1(object sender, EventArgs e)
        {
            if (this.NotifyClicked("Inventory"))
            {
                RedirectPath("~/BVAdmin/Catalog/Products_Edit_Inventory.aspx");
            }
        }

        protected void lnkFiles_Click1(object sender, EventArgs e)
        {
            if (this.NotifyClicked("Files"))
            {
                RedirectPath("~/BVAdmin/Catalog/FileDownloads.aspx");
            }
        }

        protected void lnkAdditionalImages_Click1(object sender, EventArgs e)
        {
            if (this.NotifyClicked("Images"))
            {
                RedirectPath("~/BVAdmin/Catalog/Products_Edit_Images.aspx");
            }
        }
    }
}