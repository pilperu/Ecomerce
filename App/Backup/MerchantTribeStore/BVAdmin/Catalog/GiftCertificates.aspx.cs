using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MerchantTribe.Commerce;
using MerchantTribe.Commerce.Catalog;
using MerchantTribe.Commerce.Membership;

namespace MerchantTribeStore
{

    partial class BVAdmin_Catalog_GiftCertificates : BaseAdminPage
    {

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);
            if (!Page.IsPostBack)
            {
                BindGiftCertificateGridView();
                BindIssuedGiftCertificateGridView();
                IssuedGiftCertificatesGridView.PageSize = WebAppSettings.RowsPerPage;
            }
        }

        protected void BindGiftCertificateGridView()
        {
            ProductSearchCriteria criteria = new ProductSearchCriteria();
            //criteria.SpecialProductTypeOne = SpecialProductTypes.GiftCertificate;
            //criteria.SpecialProductTypeTwo = SpecialProductTypes.ArbitrarilyPricedGiftCertificate;
            criteria.DisplayInactiveProducts = true;
            GiftCertificatesGridView.DataSource = MTApp.CatalogServices.Products.FindByCriteria(criteria);
            GiftCertificatesGridView.DataKeyNames = new string[] { "bvin" };
            GiftCertificatesGridView.DataBind();
        }

        protected void BindIssuedGiftCertificateGridView()
        {
            IssuedGiftCertificatesGridView.DataBind();
        }

        protected override void OnPreLoad(System.EventArgs e)
        {
            base.OnPreLoad(e);
            base.OnPreInit(e);
            this.PageTitle = "Gift Certificates";
            this.CurrentTab = AdminTabType.Catalog;
            ValidateCurrentUserHasPermission(SystemPermissions.CatalogView);
        }

        protected void btnNew_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            Response.Redirect("~/BVAdmin/Catalog/GiftCertificateEdit.aspx");
        }

        protected void GiftCertificatesGridView_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Product p = (Product)e.Row.DataItem;
                if (p != null)
                {
                    CheckBox chkActive = (CheckBox)e.Row.FindControl("chkActive");
                    if (p.Status == ProductStatus.Active)
                    {
                        chkActive.Checked = true;
                    }
                    else
                    {
                        chkActive.Checked = false;
                    }
                }
            }
        }

        protected void GiftCertificatesGridView_RowEditing(object sender, System.Web.UI.WebControls.GridViewEditEventArgs e)
        {
            string key = (string)GiftCertificatesGridView.DataKeys[e.NewEditIndex].Value;
            if (key != string.Empty)
            {
                Response.Redirect("~\\BVAdmin\\Catalog\\GiftCertificateEdit.aspx?id=" + HttpUtility.UrlEncode(key));
            }
        }

        protected void GiftCertificatesGridView_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
        {
            string key = (string)GiftCertificatesGridView.DataKeys[e.RowIndex].Value;
            if (key != string.Empty)
            {
                if (MTApp.DestroyProduct(key))
                {
                    MessageBox1.ShowOk("Gift certificate was deleted successfully");
                    MessageBox2.ShowOk("Gift certificate was deleted successfully");
                }
                else
                {
                    MessageBox1.ShowError("An error occurred while trying to delete the gift certificate");
                    MessageBox2.ShowError("An error occurred while trying to delete the gift certificate");
                }
            }
            BindGiftCertificateGridView();
            e.Cancel = true;
        }

        protected void IssuedGiftCertificatesGridView_RowEditing(object sender, System.Web.UI.WebControls.GridViewEditEventArgs e)
        {
            string key = IssuedGiftCertificatesGridView.DataKeys[e.NewEditIndex].Value.ToString();
            Response.Redirect("~\\BVAdmin\\Catalog\\GiftCertificateInstanceEdit.aspx?id=" + HttpUtility.UrlEncode(key));
        }

        protected void ObjectDataSource1_Selecting(object sender, System.Web.UI.WebControls.ObjectDataSourceSelectingEventArgs e)
        {
            if (e.ExecutingSelectCount)
            {
                e.InputParameters["rowCount"] = HttpContext.Current.Items["RowCount"];
                HttpContext.Current.Items["RowCount"] = null;
            }
        }

        protected void ObjectDataSource1_Selected(object sender, System.Web.UI.WebControls.ObjectDataSourceStatusEventArgs e)
        {
            if (e.OutputParameters["RowCount"] != null)
            {
                HttpContext.Current.Items["RowCount"] = e.OutputParameters["RowCount"];
            }
        }
    }
}