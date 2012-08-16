using System;
using System.Collections.Generic;
using System.Text;
using MerchantTribe.Commerce;
using MerchantTribe.Commerce.Catalog;
using MerchantTribe.Commerce.Contacts;
using MerchantTribe.Commerce.Content;
using MerchantTribe.Commerce.Membership;
using MerchantTribe.Commerce.Orders;

namespace MerchantTribeStore
{

    partial class BVAdmin_Content_EmailTemplates_Edit : BaseAdminPage
    {

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.PageTitle = "Edit Html Template";
            this.CurrentTab = AdminTabType.Content;
            ValidateCurrentUserHasPermission(SystemPermissions.ContentView);
        }

        private long CurrentTemplateId()
        {
            long result = 0;
            long.TryParse(this.BvinField.Value, out result);
            return result;
        }

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);

            this.RegisterWindowScripts();

            if (!Page.IsPostBack)
            {

                PopulateTags(this.MTApp);

                if (Request.QueryString["id"] != null)
                {
                    this.BvinField.Value = Request.QueryString["id"];
                    LoadEmailTemplate();
                }
                else
                {
                    this.BvinField.Value = string.Empty;
                }
            }
        }

        private void RegisterWindowScripts()
        {

            StringBuilder sb = new StringBuilder();
            sb.Append("function MoveEmailVars()");
            sb.Append("{");
            sb.Append("var textbox = document.getElementById('" + this.BodyField.ClientID + "');");
            sb.Append("var listbox = document.getElementById('" + this.Tags.ClientID + "');");
            sb.Append("textbox.value += listbox.options[listbox.selectedIndex].value;");
            sb.Append("textbox.focus();");
            sb.Append("}");
            sb.Append("function MoveEmailVars2()");
            sb.Append("{");
            sb.Append("var textbox = document.getElementById('" + this.RepeatingSectionField.ClientID + "');");
            sb.Append("var listbox = document.getElementById('" + this.Tags.ClientID + "');");
            sb.Append("textbox.value += listbox.options[listbox.selectedIndex].value;");
            sb.Append("textbox.focus();");
            sb.Append("}");

            Page.ClientScript.RegisterClientScriptBlock(typeof(System.Web.UI.Page), "WindowScripts", sb.ToString(), true);
        }

        private void PopulateTags(MerchantTribeApplication app)
        {
            List<IReplaceable> items = new List<IReplaceable>();

            // Default Tags
            Replaceable defaultTags = new Replaceable();
            defaultTags.Tags.AddRange(new HtmlTemplate().DefaultReplacementTags(app));
            items.Add(defaultTags);

            // Objects with tags
            items.Add(new MailingListMember());
            items.Add(new CustomerAccount());
            items.Add(new VendorManufacturer());
            items.Add(new Order());
            items.Add(new LineItem());
            items.Add(new OrderPackage());
            items.Add(new Product());

            // Get all tags from everything into one big list
            List<HtmlTemplateTag> t = new List<HtmlTemplateTag>();
            foreach (IReplaceable r in items)
            {
                t.AddRange(r.GetReplaceableTags(app));
            }

            this.Tags.DataSource = t;
            this.Tags.DataValueField = "Tag";
            this.Tags.DataTextField = "Tag";
            this.Tags.DataBind();
        }

        private void LoadEmailTemplate()
        {
            HtmlTemplate e = MTApp.ContentServices.HtmlTemplates.Find(CurrentTemplateId());
            if (e == null) return;

            this.FromField.Text = e.From;
            this.DisplayNameField.Text = e.DisplayName;
            this.SubjectField.Text = e.Subject;
            this.BodyField.Text = e.Body;
            this.RepeatingSectionField.Text = e.RepeatingSection;
            if (this.lstTemplateType.Items.FindByValue(((int)e.TemplateType).ToString()) != null)
            {
                this.lstTemplateType.ClearSelection();
                this.lstTemplateType.Items.FindByValue(((int)e.TemplateType).ToString()).Selected = true;
            }
        }

        protected void btnSaveChanges_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            if (this.Save() == true)
            {
                Response.Redirect("EmailTemplates.aspx");
            }
        }

        protected void btnCancel_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            Response.Redirect("EmailTemplates.aspx");
        }

        private bool Save()
        {
            bool result = false;

            HtmlTemplate e = MTApp.ContentServices.HtmlTemplates.Find(CurrentTemplateId());
            if (CurrentTemplateId() == 0) e = new HtmlTemplate();
            if (e == null) return false;


            e.Body = this.BodyField.Text.Trim();
            e.DisplayName = this.DisplayNameField.Text.Trim();
            e.From = this.FromField.Text.Trim();
            e.RepeatingSection = this.RepeatingSectionField.Text.Trim();
            e.Subject = this.SubjectField.Text.Trim();

            long typeId = 0;
            long.TryParse(this.lstTemplateType.SelectedValue, out typeId);
            if (typeId < 0) typeId = 0;
            e.TemplateType = (HtmlTemplateType)typeId;

            if (e.Id == 0)
            {
                result = MTApp.ContentServices.HtmlTemplates.Create(e);
            }
            else
            {
                result = MTApp.ContentServices.HtmlTemplates.Update(e);
            }

            if (result == true)
            {
                // Update bvin field so that next save will call updated instead of create
                this.BvinField.Value = e.Id.ToString();
            }

            return result;
        }


    }
}