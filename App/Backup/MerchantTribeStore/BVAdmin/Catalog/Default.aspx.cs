using System.Collections.ObjectModel;
using System.Text;
using System.Web.UI;
using MerchantTribe.Commerce;
using MerchantTribe.Commerce.Catalog;
using MerchantTribe.Commerce.Membership;
using System.Collections.Generic;

namespace MerchantTribeStore
{

    partial class BVAdmin_Catalog_Default : BaseAdminPage
    {

        private ProductSearchCriteria criteria = new ProductSearchCriteria();
        private int pageSize = 50;
        private int rowCount = 0;
        private int currentPage = 1;

        protected override void OnPreInit(System.EventArgs e)
        {
            base.OnPreInit(e);
            this.PageTitle = "Products";
            this.CurrentTab = AdminTabType.Catalog;
            ValidateCurrentUserHasPermission(SystemPermissions.CatalogView);

        }

        void SimpleProductFilter_FilterChanged(ProductSearchCriteria criteria, System.EventArgs e)
        {
            this.criteria = criteria;
            SaveQuerySettings();
            this.currentPage = 1;
            LoadProducts();
        }

        void SimpleProductFilter_GoPressed(ProductSearchCriteria criteria, System.EventArgs e)
        {
            this.criteria = criteria;
            SaveQuerySettings();
            this.currentPage = 1;
            LoadProducts();
        }

        protected override void OnInit(System.EventArgs e)
        {
            base.OnInit(e);
            this.SimpleProductFilter.GoPressed += new BVAdmin_Controls_SimpleProductFilter.GoPressedDelegate(SimpleProductFilter_GoPressed);
            this.SimpleProductFilter.FilterChanged += new BVAdmin_Controls_SimpleProductFilter.FilterChangedDelegate(SimpleProductFilter_FilterChanged);
            this.PageMessageBox = this.MessageBox1;
        }

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);

            if (!Page.IsPostBack)
            {
                LoadQuerySettings();
                this.criteria = SimpleProductFilter.LoadProductCriteria();
                LoadProducts();
                this.SimpleProductFilter.Focus();
            }

            RenderNewButtons();
        }

        private void RenderNewButtons()
        {
            string btn = "<a href=\"Products_Edit.aspx\" title=\"Add New Product\" class=\"btn\"><b>+ Add New Product</b></a>";
            int currentCount = MTApp.CatalogServices.Products.FindAllCount();
            if (currentCount < MTApp.CurrentStore.MaxProducts)
            {
                this.litNewButton.Text = btn;
                this.litNewButton2.Text = btn;
                this.litNewButton3.Text = btn;
            }
            else
            {
                this.litNewButton.Text = "<div class=\"flash-message-info\">You have reached the maximum of "
                                        + MTApp.CurrentStore.MaxProducts.ToString() + " products for the "
                                        + MTApp.CurrentStore.PlanName + " plan. <a href=\"../ChangePlan.aspx\">Upgrade your plan</a> to create more products.</div>";
                this.litNewButton2.Text = this.litNewButton.Text;
                this.litNewButton3.Text = this.litNewButton.Text;
            }
        }

        private void LoadQuerySettings()
        {
            if ((Request.QueryString["page"] != null))
            {
                int.TryParse(Request.QueryString["page"], out currentPage);
                if ((currentPage < 1))
                {
                    currentPage = 1;
                }                
            }

            // Get criteria from cookies
        }

        private void SaveQuerySettings()
        {
            // Save criteria to cookies
        }

        private void LoadProducts()
        {
            criteria.DisplayInactiveProducts = true;
            List<Product> products = MTApp.CatalogServices.Products.FindByCriteria(this.criteria, this.currentPage, pageSize, ref rowCount);
            this.lblResults.Text = rowCount.ToString() + " product found";
            this.litPager1.Text = MerchantTribe.Web.Paging.RenderPagerWithLimits("Default.aspx?page={0}", currentPage, rowCount, pageSize, 20);
            RenderProducts(products);
            this.litPager2.Text = this.litPager1.Text;

            // Show sample panel if no products in store
            int allProducts = MTApp.CatalogServices.Products.FindAllCount();
            this.pnlSamples.Visible = (allProducts < 1);
        }

        private void RenderProducts(List<Product> products)
        {
            StringBuilder sb = new StringBuilder();

            foreach (Product p in products)
            {
                RenderSingleProduct(sb, p);
            }

            this.litResults.Text = sb.ToString();
        }

        private void RenderSingleProduct(StringBuilder sb, Product p)
        {

            string destinationLink = "Products_Edit.aspx?id=" + p.Bvin;

            string imageUrl = MerchantTribe.Commerce.Storage.DiskStorage.ProductImageUrlSmall(((IMultiStorePage)this.Page).MTApp, p.Bvin, p.ImageFileSmall, Page.Request.IsSecureConnection);

            sb.Append("<div class=\"record\"><a href=\"" + destinationLink + "\">");

            sb.Append("<div class=\"recordimage\">");
            sb.Append("<img src=\"" + imageUrl + "\" border=\"0\" alt=\"" + p.ImageFileSmallAlternateText + "\" />");
            sb.Append("</div>");

            sb.Append("<div class=\"recordname\">");
            sb.Append(p.ProductName);
            sb.Append("</div>");
            sb.Append("<div class=\"recordsku\">");
            sb.Append(p.Sku);
            sb.Append("</div>");
            sb.Append("<div class=\"recordprice\">");
            sb.Append(p.SitePrice.ToString("c"));
            sb.Append("</div>");

            sb.Append("</a></div>");
        }

        protected void lnkAddSamples_Click(object sender, System.EventArgs e)
        {
            MTApp.AddSampleProductsToStore();
            this.ShowMessage("Samples Added!", ErrorTypes.Ok);
            LoadProducts();
        }

        protected void btnRemoveSamples_Click(object sender, System.EventArgs e)
        {
            MTApp.RemoveSampleProductsFromStore();
            this.ShowMessage("Samples Removed!", ErrorTypes.Ok);
            LoadProducts();
        }


    }
}