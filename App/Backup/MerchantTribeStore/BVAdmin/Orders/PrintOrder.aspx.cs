using System;
using System.Web;
using System.Web.UI.WebControls;
using MerchantTribe.Commerce;
using MerchantTribe.Commerce.Orders;
using MerchantTribe.Commerce.Membership;
using MerchantTribe.Commerce.Utilities;
using MerchantTribe.Commerce.Shipping;
using MerchantTribe.Commerce.Payment;
using MerchantTribe.Commerce.Content;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace MerchantTribeStore
{

    partial class BVAdmin_Orders_PrintOrder : BaseAdminPage
    {

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (!Page.IsPostBack)
            {
                LoadTemplates();
                LoadMode();

                if (Request.QueryString["templateid"] != null)
                {
                    SetTemplate(Request.QueryString["templateid"]);
                }
                if (Request.QueryString["autoprint"] == "1")
                {
                    AutoPrint();
                }
            }
        }
        private void LoadTemplates()
        {
            this.TemplateField.DataSource = MTApp.ContentServices.GetAllTemplatesForStoreOrDefaults();
            this.TemplateField.DataTextField = "DisplayName";
            this.TemplateField.DataValueField = "Id";
            this.TemplateField.DataBind();
        }

        private void SetTemplate(string bvin)
        {
            if (this.TemplateField.Items.FindByValue(bvin) != null)
            {
                this.TemplateField.ClearSelection();
                this.TemplateField.Items.FindByValue(bvin).Selected = true;
            }
        }

        private void LoadMode()
        {
            if (Request.QueryString["mode"] != null)
            {
                switch (Request.QueryString["mode"])
                {
                    case "pack":
                        SetTemplate(WebAppSettings.PrintTemplatePackingSlip);
                        Generate();
                        break;
                    case "receipt":
                        SetTemplate(WebAppSettings.PrintTemplateAdminReceipt);
                        Generate();
                        break;
                    case "invoice":
                        SetTemplate(WebAppSettings.PrintTemplateCustomerInvoice);
                        Generate();
                        break;
                    default:
                        SetTemplate(WebAppSettings.PrintTemplateCustomerInvoice);
                        Generate();
                        break;
                }
            }
        }

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.PageTitle = "Print Order";
            this.CurrentTab = AdminTabType.Orders;
            ValidateCurrentUserHasPermission(SystemPermissions.OrdersView);
        }

        protected void AutoPrint()
        {
            Generate();
            this.litAutoPrint.Text = "1";            
        }
        protected void btnGenerate_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            Generate();
        }

        protected void btnContinue_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            if (Request.QueryString["id"].Contains(",") == true)
            {
                Response.Redirect("default.aspx");
            }
            else
            {
                Response.Redirect("ViewOrder.aspx?id=" + Request.QueryString["id"]);
            }
        }

        protected void btnContinue2_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            if (Request.QueryString["id"].Contains(",") == true)
            {
                Response.Redirect("default.aspx");
            }
            else
            {
                Response.Redirect("ViewOrder.aspx?id=" + Request.QueryString["id"]);
            }
        }

        private void Generate()
        {
            string id = Request.QueryString["id"];
            id = id.TrimEnd(',');
            string[] os = id.Split(',');
            this.DataList1.DataSource = os;
            this.DataList1.DataBind();
        }

        protected void DataList1_ItemDataBound(object sender, System.Web.UI.WebControls.DataListItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.AlternatingItem | e.Item.ItemType == ListItemType.Item)
            {
                long templateId = 0;
                long.TryParse(this.TemplateField.SelectedValue, out templateId);
                HtmlTemplate t = MTApp.ContentServices.HtmlTemplates.Find(templateId);                
                if (t != null)
                {
                    string orderId = (string)e.Item.DataItem;
                    Order o = MTApp.OrderServices.Orders.FindForCurrentStore(orderId);
                    Literal litTemplate = (Literal)e.Item.FindControl("litTemplate");
                    if (litTemplate != null)
                    {
                        t = t.ReplaceTagsInTemplate(MTApp, o, o.ItemsAsReplaceable());
                        litTemplate.Text = t.Body;
                    }
                }
            }
        }

    }
}