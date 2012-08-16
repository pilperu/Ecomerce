using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MerchantTribe.Commerce.Marketing;
using MerchantTribe.Commerce.Marketing.PromotionActions;
using MerchantTribe.Commerce.Marketing.PromotionQualifications;
using System.IO;
using System.Text;

namespace MerchantTribeStore.BVAdmin.Marketing
{
    public partial class Promotions_Edit : BaseAdminPage
    {
        protected override void OnPreInit(System.EventArgs e)
        {
            base.OnPreInit(e);
            this.PageTitle = "Edit Promotion";
            this.CurrentTab = AdminTabType.Marketing;
            ValidateCurrentUserHasPermission(MerchantTribe.Commerce.Membership.SystemPermissions.MarketingView);
        }

        private string keyword = string.Empty;
        private int currentPage = 1;
        private string showdisabled = string.Empty;
        private string Destination()
        {
            string result = "Promotions.aspx?page=" + this.currentPage
                            + "&showdisabled=" + System.Web.HttpUtility.UrlEncode(this.showdisabled)
                            + "&keyword=" + System.Web.HttpUtility.UrlEncode(this.keyword);
            return result;
        }
   
        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);

            // Load params to pass back to list page
            if ((Request.QueryString["page"] != null))
            {
                int.TryParse(Request.QueryString["page"], out currentPage);
                if ((currentPage < 1))
                {
                    currentPage = 1;
                }
            }
            if ((Request.QueryString["keyword"] != null))
            {
                keyword = Request.QueryString["keyword"];
            }
            if ((Request.QueryString["showdisabled"] != null))
            {
                showdisabled = Request.QueryString["showdisabled"];                
            }
            this.lnkBack.NavigateUrl = Destination();


            if (!Page.IsPostBack)
            {
                LoadItem();
            }
        }

        // Helper 
        private Promotion GetCurrentPromotion()
        {
            string promoid = "0";
            if (Request.QueryString["id"] != null)
            {
                promoid =  Request.QueryString["id"];
            }        
            long pid = 0;
            long.TryParse(promoid, out pid);
            Promotion p = MTApp.MarketingServices.Promotions.Find(pid);
            return p;            
        }

        // Loaders
        private void LoadItem()
        {
            Promotion p = GetCurrentPromotion();
            if (p == null) return;

            this.chkEnabled.Checked = p.IsEnabled;
            this.NameField.Text = p.Name;
            this.CustomerDescriptionField.Text = p.CustomerDescription;
            this.DateStartField.SelectedDate = p.StartDateUtc.ToLocalTime();
            this.DateEndField.SelectedDate = p.EndDateUtc.ToLocalTime();
            this.LoadQualifications(p.Qualifications);
            LoadActions(p.Actions);

            PopulateLists(p.Mode);
        }
        private void LoadQualifications(ReadOnlyCollection<IPromotionQualification> qualifiers)
        {
            string img = Page.ResolveUrl("~/bvadmin/images/buttons/SmallX.png");
            string deleteButton = "<img src=\"" + img + "\" alt=\"Delete This?\" />";

            StringBuilder sb = new StringBuilder();
            foreach (IPromotionQualification q in qualifiers)
            {
                sb.Append("<div class=\"coolitem\">");

                sb.Append("<table width=\"100%\"><tr>");
                sb.Append("<td>");
                sb.Append(q.FriendlyDescription(MTApp));
                if (q.ProcessingCost == RelativeProcessingCost.Higher || q.ProcessingCost == RelativeProcessingCost.Highest)
                {
                    sb.Append("<br /><span class=\"smallwarning\"><img src=\"" + Page.ResolveUrl("~/bvadmin/images/WarningSymbol.png") + "\" alt=\"possible slow qualifier\" /> May run slowly!</span>");
                }
                sb.Append("</td>");

                sb.Append("<td align=\"left\" width=\"50\">");
                sb.Append("<a href=\"#\" class=\"btn editrequester\" id=\"q" + q.Id.ToString() + "\">");
                sb.Append("<b>Edit</b>");
                sb.Append("</a>");
                sb.Append("</td>");

                sb.Append("<td align=\"right\" width=\"50\">");
                sb.Append("<a href=\"#\" class=\"deleterequester\" id=\"x" + q.Id.ToString() + "\">");
                sb.Append(deleteButton);
                sb.Append("</a>");
                sb.Append("</td>");

                
                sb.Append("</tr></table>");
                                
                sb.Append("</div>");
                sb.Append("<div class=\"triangledown\"></div>");
            }
            this.litQualifications.Text = sb.ToString();
        }
        private void LoadActions(ReadOnlyCollection<IPromotionAction> actions)
        {
            string img = Page.ResolveUrl("~/bvadmin/images/buttons/SmallX.png");
            string deleteButton = "<img src=\"" + img + "\" alt=\"Delete This?\" />";

            StringBuilder sb = new StringBuilder();
            foreach (IPromotionAction a in actions)
            {

                sb.Append("<div class=\"coolitem\">");

                sb.Append("<table width=\"100%\"><tr>");
                sb.Append("<td>"); 
                //if (a.ProcessingCost == RelativeProcessingCost.Higher || a.ProcessingCost == RelativeProcessingCost.Highest)
                //{
                //    sb.Append("<div class=\"flash-message-warning\">This action can run slowly. Don't use too many of these at the same time in your store</div>");
                //}
                sb.Append(a.FriendlyDescription + "</td>");

                sb.Append("<td align=\"left\" width=\"50\">");
                sb.Append("<a href=\"#\" class=\"btn editrequester\" id=\"a" + a.Id.ToString() + "\">");
                sb.Append("<b>Edit</b>");
                sb.Append("</a>");
                sb.Append("</td>");

                sb.Append("<td align=\"right\" width=\"50\">");
                sb.Append("<a href=\"#\" class=\"deleterequester\" id=\"d" + a.Id.ToString() + "\">");
                sb.Append(deleteButton);
                sb.Append("</a>");
                sb.Append("</td>");

                sb.Append("</tr></table>");

                sb.Append("</div>");
                sb.Append("<div class=\"triangledown\"></div>");
            }
            this.litActions.Text = sb.ToString();
        }

        // Qualifiers and Action Methods
        private void PopulateLists(PromotionType mode)
        {
            if (mode == PromotionType.Sale)
            {
                // sale
                this.lstNewQualification.Items.Clear();
                this.lstNewQualification.Items.Add(new ListItem("Any Product", PromotionQualificationBase.TypeIdAnyProduct));
                this.lstNewQualification.Items.Add(new ListItem("When Product Is...", PromotionQualificationBase.TypeIdProductBvin));
                this.lstNewQualification.Items.Add(new ListItem("When Product Category Is...", PromotionQualificationBase.TypeIdProductCategory));
                this.lstNewQualification.Items.Add(new ListItem("When Product Type Is...", PromotionQualificationBase.TypeIdProductType));
                this.lstNewQualification.Items.Add(new ListItem("When User Is...", PromotionQualificationBase.TypeIdUserIs));
                this.lstNewQualification.Items.Add(new ListItem("When User Price Group Is...", PromotionQualificationBase.TypeIdUserIsInGroup));
                

                this.lstNewAction.Items.Clear();
                this.lstNewAction.Items.Add(new ListItem("Adjust Product Price", "A07AFF02-BA28-42E0-B334-324DE467B2D7"));
            }
            else
            {
                // offer
                this.lstNewQualification.Items.Clear();
                this.lstNewQualification.Items.Add(new ListItem("Any Order", PromotionQualificationBase.TypeIdAnyOrder));
                this.lstNewQualification.Items.Add(new ListItem("Order Has Coupon Code...", PromotionQualificationBase.TypeIdOrderHasCoupon));
                this.lstNewQualification.Items.Add(new ListItem("When Order Has Products...", PromotionQualificationBase.TypeIdOrderHasProducts));
                this.lstNewQualification.Items.Add(new ListItem("When Order Total >= ", PromotionQualificationBase.TypeIdOrderSubTotalIs));
                this.lstNewQualification.Items.Add(new ListItem("When User Is...", PromotionQualificationBase.TypeIdUserIs));
                this.lstNewQualification.Items.Add(new ListItem("When User Price Group Is...", PromotionQualificationBase.TypeIdUserIsInGroup));
                this.lstNewQualification.Items.Add(new ListItem("Any Shipping Method", PromotionQualificationBase.TypeIdAnyShippingMethod));

                this.lstNewAction.Items.Clear();
                this.lstNewAction.Items.Add(new ListItem("Adjust Order Total", PromotionActionBase.TypeIdOrderTotalAdjustment));
                this.lstNewAction.Items.Add(new ListItem("Adjust Shipping By...", PromotionActionBase.TypeIdOrderShippingAdjustment));
                
            }
        }
        protected void btnNewQualification_Click(object sender, ImageClickEventArgs e)
        {
            Promotion p = GetCurrentPromotion();
            if (p == null) return;

            string newid = this.lstNewQualification.SelectedValue;
            PromotionQualificationBase pq = PromotionQualificationBase.Factory(newid);
            p.AddQualification(pq);

            MTApp.MarketingServices.Promotions.Update(p);
            LoadItem();
        }
        protected void btnNewAction_Click(object sender, ImageClickEventArgs e)
        {
            Promotion p = GetCurrentPromotion();
            if (p == null) return;
            
            string newid = this.lstNewAction.SelectedValue;
            PromotionActionBase pa = PromotionActionBase.Factory(newid);            
            p.AddAction(pa);
            
            MTApp.MarketingServices.Promotions.Update(p);
            LoadItem();
        }

        // Save        
        protected void btnSave_Click(object sender, ImageClickEventArgs e)
        {
            this.MessageBox1.ClearMessage();
            if (Save() == true)
            {
                Response.Redirect(Destination());
            }
        }
        private bool Save()
        {
            bool result = false;

            Promotion p = GetCurrentPromotion();
            if (p == null) return false;

            p.IsEnabled = this.chkEnabled.Checked;
            p.Name = this.NameField.Text.Trim();
            p.CustomerDescription = this.CustomerDescriptionField.Text.Trim();
            p.StartDateUtc = this.DateStartField.SelectedDate.ToUniversalTime();
            p.EndDateUtc = this.DateEndField.SelectedDate.ToUniversalTime();

            result = MTApp.MarketingServices.Promotions.Update(p);
           
            if (result == false)
            {
                this.MessageBox1.ShowWarning("Unable to save promotion. Uknown error.");
            }

            return result;
        }

        // Editor Helpers                         
        protected void btnCloseQualificationEditor_Click(object sender, EventArgs e)
        {
            CloseQualificationEditor();
        }
        protected void btnCloseActionEditor_Click(object sender, EventArgs e)
        {
            CloseActionEditor();
        }
        protected void btnSaveQualifications_Click1(object sender, ImageClickEventArgs e)
        {
            if (SaveQualificationEditor())
            {
                CloseQualificationEditor();
            }
        }
        protected void btnSaveActions_Click1(object sender, ImageClickEventArgs e)
        {
            if (SaveActionEditor())
            {
                CloseActionEditor();
            }
        }

        protected void btnTest_Click(object sender, EventArgs e)
        {
            string editId = this.currenteditid.Value;
            if (editId.StartsWith("a"))
            {
                ShowActionEditor(editId.TrimStart('a'));
            }
            else
            {
                ShowQualificationEditor(editId.TrimStart('q'));
            }
        }
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            string editId = this.currentdeleteid.Value;
            if (editId.StartsWith("d"))
            {
                DeleteAction(editId.TrimStart('d'));
            }
            else
            {
                DeleteQualification(editId.TrimStart('x'));
            }
        }
        private void DeleteAction(string id)
        {
            Promotion p = GetCurrentPromotion();
            if (p != null)
            {
                string trimmed = id.TrimStart('d');
                long temp = -1;
                long.TryParse(trimmed, out temp);
                if (p.RemoveAction(temp))
                {
                    MTApp.MarketingServices.Promotions.Update(p);
                }
            }
            LoadItem();     
        }
        private void DeleteQualification(string id)
        {
            Promotion p = GetCurrentPromotion();
            if (p != null)
            {
                string trimmed = id.TrimStart('x');
                long temp = -1;
                long.TryParse(trimmed, out temp);
                if (p.RemoveQualification(temp))
                {
                    MTApp.MarketingServices.Promotions.Update(p);
                }
            }
            LoadItem();            
        }

        private void ShowQualificationEditor(string id)
        {            
            Promotion p = GetCurrentPromotion();
            this.Promotions_Edit_Qualification1.LoadQualification(p, id);            
            this.pnlEditQualification.Visible = true;
        }
        private void CloseQualificationEditor()
        {
            this.pnlEditQualification.Visible = false;
            this.LoadItem();
        }
        private bool SaveQualificationEditor()
        {         
            return this.Promotions_Edit_Qualification1.SaveQualification();            
        }

        private void ShowActionEditor(string id)
        {
            Promotion p = GetCurrentPromotion();
            this.Promotions_Edit_Actions1.LoadAction(p, id);
            this.pnlEditAction.Visible = true;
        }
        private void CloseActionEditor()
        {
            this.pnlEditAction.Visible = false;
            this.LoadItem();
        }
        private bool SaveActionEditor()
        {            
            return this.Promotions_Edit_Actions1.SaveAction();            
        }

     
        
    }
}