
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System;
using System.Web;
using System.Web.UI.WebControls;
using System.Text;
using MerchantTribe.Commerce;
using MerchantTribe.Commerce.Accounts;
using MerchantTribe.Commerce.BusinessRules;
using MerchantTribe.Commerce.Catalog;
using MerchantTribe.Commerce.Contacts;
using MerchantTribe.Commerce.Content;
using MerchantTribe.Commerce.Membership;
using MerchantTribe.Commerce.Metrics;
using MerchantTribe.Commerce.Orders;
using MerchantTribe.Commerce.Payment;
using MerchantTribe.Commerce.Shipping;
using MerchantTribe.Commerce.Taxes;
using MerchantTribe.Commerce.Utilities;

namespace MerchantTribeStore
{

    partial class BVAdmin_Configuration_MailServer : BaseAdminPage
    {
        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.PageTitle = "Mail Server Settings";
            this.CurrentTab = AdminTabType.Configuration;
            ValidateCurrentUserHasPermission(SystemPermissions.SettingsView);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (!Page.IsPostBack)
            {
                LoadData();
            }
        }

        private void LoadData()
        {
            string valueToSet = "0";
            this.pnlMain.Visible = false;

            if (MTApp.CurrentStore.Settings.MailServer.UseCustomMailServer)
            { valueToSet = "1"; this.pnlMain.Visible = true; }

            if (WebAppSettings.IsIndividualMode)
            {
                valueToSet = "1";
                this.pnlMain.Visible = true;
                this.lstMailServerChoice.Visible = false;
            }
           

            if (this.lstMailServerChoice.Items.FindByValue(valueToSet) != null)
            {
                this.lstMailServerChoice.ClearSelection();
                this.lstMailServerChoice.Items.FindByValue(valueToSet).Selected = true;
            }

            MailServerField.Text = MTApp.CurrentStore.Settings.MailServer.HostAddress;
            this.chkMailServerAuthentication.Checked = MTApp.CurrentStore.Settings.MailServer.UseAuthentication;
            this.UsernameField.Text = MTApp.CurrentStore.Settings.MailServer.Username;
            this.PasswordField.Text = "****************";

            this.chkSSL.Checked = MTApp.CurrentStore.Settings.MailServer.UseSsl;
            this.SmtpPortField.Text = MTApp.CurrentStore.Settings.MailServer.Port;

        }


        protected void btnSave_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            SaveData();
            this.msg.ShowOk("Settings saved successfully.");
        }

        private void SaveData()
        {
            MTApp.CurrentStore.Settings.MailServer.HostAddress = MailServerField.Text.Trim();
            MTApp.CurrentStore.Settings.MailServer.UseAuthentication = this.chkMailServerAuthentication.Checked;
            MTApp.CurrentStore.Settings.MailServer.Username = this.UsernameField.Text.Trim();
            if (this.PasswordField.Text.Trim().Length > 0)
            {
                if (this.PasswordField.Text != "****************")
                {
                    MTApp.CurrentStore.Settings.MailServer.Password = this.PasswordField.Text.Trim();
                    this.PasswordField.Text = "****************";
                }
            }

            MTApp.CurrentStore.Settings.MailServer.UseSsl = this.chkSSL.Checked;
            MTApp.CurrentStore.Settings.MailServer.Port = this.SmtpPortField.Text.Trim();
            if (this.lstMailServerChoice.SelectedItem.Value == "1")
            {
                MTApp.CurrentStore.Settings.MailServer.UseCustomMailServer = true;
            }
            else
            {
                MTApp.CurrentStore.Settings.MailServer.UseCustomMailServer = false;
            }
            MTApp.UpdateCurrentStore();
        }

        protected void btnCancel_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            Response.Redirect("Default.aspx");
        }

        protected void btnSendTest_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            SaveData();

            msg.ClearMessage();

            System.Net.Mail.MailMessage m = new System.Net.Mail.MailMessage("testemail@merchanttribe.com", this.TestToField.Text.Trim());
            m.Subject = "Mail Server Test Message";
            m.Body = "Your mail server appears to be correctly configured!";
            m.IsBodyHtml = false;

            if (MerchantTribe.Commerce.Utilities.MailServices.SendMail(m, MTApp.CurrentStore) == true)
            {
                msg.ShowOk("Test Message Sent");
            }
            else
            {
                msg.ShowError("Test Failed. Please check your settings and try again.");
            }

            m = null;
        }
        protected void lstMailServerChoice_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.lstMailServerChoice.SelectedItem.Value == "0")
            {
                this.pnlMain.Visible = false;
            }
            else
            {
                this.pnlMain.Visible = true;
            }
        }
    }
}