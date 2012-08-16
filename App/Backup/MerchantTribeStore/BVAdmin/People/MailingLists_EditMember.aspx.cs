
using MerchantTribe.Commerce;
using MerchantTribe.Commerce.Contacts;

namespace MerchantTribeStore
{

    partial class BVAdmin_People_MailingLists_EditMember : BaseAdminPage
    {

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);
            if (!Page.IsPostBack)
            {
                this.EmailAddressField.Focus();

                if (Request.QueryString["id"] != null)
                {
                    this.BvinField.Value = Request.QueryString["id"];
                    LoadMember();
                }
                else
                {
                    this.BvinField.Value = string.Empty;
                    if (Request.QueryString["ListID"] != null)
                    {
                        this.ListIDField.Value = Request.QueryString["ListID"];
                    }
                }

            }
        }

        private long CurrentId
        {
            get
            {
                long temp = 0;
                long.TryParse(this.BvinField.Value, out temp);
                return temp;
            }
            set
            {
                this.BvinField.Value = value.ToString();
            }

        }
        private long CurrentListId
        {
            get
            {
                long temp = 0;
                long.TryParse(this.ListIDField.Value, out temp);
                return temp;
            }
            set
            {
                this.ListIDField.Value = value.ToString();
            }

        }
        protected override void OnPreInit(System.EventArgs e)
        {
            base.OnPreInit(e);
            this.PageTitle = "Edit Mailing List Member";
            this.CurrentTab = AdminTabType.People;
            ValidateCurrentUserHasPermission(MerchantTribe.Commerce.Membership.SystemPermissions.PeopleView);
        }

        private void LoadMember()
        {
            MailingListMember m = MTApp.ContactServices.MailingLists.FindMemberOnlyById(CurrentId);
            
            if (m != null)
            {
                if (m.Id > 0)
                {
                    this.EmailAddressField.Text = m.EmailAddress;
                    this.FirstNameField.Text = m.FirstName;
                    this.LastNameField.Text = m.LastName;
                    this.CurrentListId = m.ListId;                                    
                }
            }

        }

        protected void btnCancel_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            Response.Redirect("MailingLists_Edit.aspx?id=" + CurrentListId.ToString());
        }

        protected void btnSaveChanges_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            if (this.Save() == true)
            {
                Response.Redirect("MailingLists_Edit.aspx?id=" + CurrentListId.ToString());
            }
        }

        private bool Save()
        {
            bool result = false;

            MailingListMember m = MTApp.ContactServices.MailingLists.FindMemberOnlyById(CurrentId);

            if (m == null) m = new MailingListMember();
            
            
                string originalEmail = m.EmailAddress;

                m.EmailAddress = this.EmailAddressField.Text.Trim();
                m.FirstName = this.FirstNameField.Text.Trim();
                m.LastName = this.LastNameField.Text.Trim();
                m.ListId = CurrentListId;

                if (m.Id < 1)
                {
                    if (MTApp.ContactServices.MailingLists.CheckMembership(m.ListId, m.EmailAddress))
                    {
                        this.lblError.Text = "That email address already belongs to this mailing list. Select another email address";
                    }
                    else
                    {
                        result = MTApp.ContactServices.MailingLists.CreateMemberOnly(m);
                    }
                }
                else
                {
                    if (m.EmailAddress != originalEmail)
                    {
                        if (MTApp.ContactServices.MailingLists.CheckMembership(m.ListId, m.EmailAddress))
                        {
                            this.lblError.Text = "That email address already belongs to this mailing list. Select another email address";
                        }
                        else
                        {
                            result = MTApp.ContactServices.MailingLists.UpdateMemberOnly(m);
                        }
                    }
                    else
                    {
                        result = MTApp.ContactServices.MailingLists.UpdateMemberOnly(m);
                    }
                }

                if (result)
                {
                    // Update bvin field so that next save will call updated instead of create
                    this.CurrentId = m.Id;
                }
            
            return result;
        }

    }
}