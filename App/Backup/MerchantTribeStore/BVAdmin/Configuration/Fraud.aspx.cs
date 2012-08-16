
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
using MerchantTribe.Commerce.Security;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MerchantTribeStore
{

    partial class BVAdmin_Configuration_Fraud : BaseAdminPage
    {
        FraudRuleRepository repository = null;

        protected override void OnLoad(System.EventArgs e)        
        {                        
            base.OnLoad(e);
            
            repository = new FraudRuleRepository(MTApp.CurrentRequestContext);

            if (!Page.IsPostBack)
            {
                LoadLists();
            }

        }

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.PageTitle = "Fraud Screen Settings";
            this.CurrentTab = AdminTabType.Configuration;
            ValidateCurrentUserHasPermission(SystemPermissions.SettingsView);
        }

        private void LoadLists()
        {

            List<FraudRule> rules = repository.FindForStore(MTApp.CurrentStore.Id);
            SortableCollection<FraudRule> emailRules = new SortableCollection<FraudRule>();
            SortableCollection<FraudRule> domainRules = new SortableCollection<FraudRule>();
            SortableCollection<FraudRule> ipRules = new SortableCollection<FraudRule>();
            SortableCollection<FraudRule> PHrules = new SortableCollection<FraudRule>();
            SortableCollection<FraudRule> CCrules = new SortableCollection<FraudRule>();

            for (int i = 0; i <= rules.Count - 1; i++)
            {
                switch (rules[i].RuleType)
                {
                    case FraudRuleType.DomainName:
                        domainRules.Add(rules[i]);
                        break;
                    case FraudRuleType.EmailAddress:
                        emailRules.Add(rules[i]);
                        break;
                    case FraudRuleType.IPAddress:
                        ipRules.Add(rules[i]);
                        break;
                    case FraudRuleType.PhoneNumber:
                        PHrules.Add(rules[i]);
                        break;
                    case FraudRuleType.CreditCardNumber:
                        CCrules.Add(rules[i]);
                        break;
                }
            }

            emailRules.Sort("RuleValue");
            domainRules.Sort("RuleValue");
            ipRules.Sort("RuleValue");
            PHrules.Sort("RuleValue");
            CCrules.Sort("RuleValue");

            this.lstEmail.DataSource = emailRules;
            this.lstEmail.DataTextField = "RuleValue";
            this.lstEmail.DataValueField = "Bvin";
            this.lstEmail.DataBind();

            this.lstDomain.DataTextField = "RuleValue";
            this.lstDomain.DataValueField = "Bvin";
            this.lstDomain.DataSource = domainRules;
            this.lstDomain.DataBind();

            this.lstIP.DataTextField = "RuleValue";
            this.lstIP.DataValueField = "Bvin";
            this.lstIP.DataSource = ipRules;
            this.lstIP.DataBind();

            this.lstPhoneNumber.DataTextField = "RuleValue";
            this.lstPhoneNumber.DataValueField = "Bvin";
            this.lstPhoneNumber.DataSource = PHrules;
            this.lstPhoneNumber.DataBind();

            this.lstCreditCard.DataTextField = "RuleValue";
            this.lstCreditCard.DataValueField = "Bvin";
            this.lstCreditCard.DataSource = CCrules;
            this.lstCreditCard.DataBind();

            this.EmailField.Text = "";
            this.DomainField.Text = "";
            this.IPField.Text = "";
            this.PhoneNumberField.Text = "";
            this.CreditCardField.Text = "";

        }

        //Email Address
        protected void btnNewEmail_Click(System.Object sender, System.Web.UI.ImageClickEventArgs e)
        {
            if (this.EmailField.Text.Trim().Length > 0)
            {
                FraudRule r = new FraudRule();
                r.RuleType = FraudRuleType.EmailAddress;
                r.RuleValue = this.EmailField.Text.Trim().ToLower();
                repository.Create(r);
            }
            LoadLists();
        }

        protected void btnDeleteEmail_Click(System.Object sender, System.Web.UI.ImageClickEventArgs e)
        {
            for (int i = 0; i <= this.lstEmail.Items.Count - 1; i++)
            {
                if (this.lstEmail.Items[i].Selected == true)
                {
                    DeleteRule(this.lstEmail.Items[i].Value);
                }
            }
            LoadLists();
        }

        //IP Address
        protected void btnNewIP_Click(System.Object sender, System.Web.UI.ImageClickEventArgs e)
        {
            if (this.IPField.Text.Trim().Length > 0)
            {
                FraudRule r = new FraudRule();
                r.RuleType = FraudRuleType.IPAddress;
                r.RuleValue = this.IPField.Text.Trim().ToLower();
                repository.Create(r);
            }
            LoadLists();
        }

        protected void btnDeleteIP_Click(System.Object sender, System.Web.UI.ImageClickEventArgs e)
        {
            for (int i = 0; i <= this.lstIP.Items.Count - 1; i++)
            {
                if (this.lstIP.Items[i].Selected == true)
                {
                    DeleteRule(this.lstIP.Items[i].Value);
                }
            }
            LoadLists();
        }

        //Domain Name
        protected void btnNewDomain_Click(System.Object sender, System.Web.UI.ImageClickEventArgs e)
        {
            if (this.DomainField.Text.Trim().Length > 0)
            {
                FraudRule r = new FraudRule();
                r.RuleType = FraudRuleType.DomainName;
                r.RuleValue = this.DomainField.Text.Trim().ToLower();
                repository.Create(r);
            }
            LoadLists();
        }

        protected void btnDeleteDomain_Click(System.Object sender, System.Web.UI.ImageClickEventArgs e)
        {
            for (int i = 0; i <= this.lstDomain.Items.Count - 1; i++)
            {
                if (this.lstDomain.Items[i].Selected == true)
                {
                    DeleteRule(this.lstDomain.Items[i].Value);
                }
            }
            LoadLists();
        }

        //Phone Number

        protected void btnNewPhoneNumber_Click(System.Object sender, System.Web.UI.ImageClickEventArgs e)
        {
            if (this.PhoneNumberField.Text.Trim().Length > 0)
            {
                FraudRule r = new FraudRule();
                r.RuleType = FraudRuleType.PhoneNumber;
                r.RuleValue = this.PhoneNumberField.Text.Trim().ToLower();
                repository.Create(r);
            }
            LoadLists();
        }

        protected void btnDeletePhoneNumber_Click(System.Object sender, System.Web.UI.ImageClickEventArgs e)
        {
            for (int i = 0; i <= this.lstPhoneNumber.Items.Count - 1; i++)
            {
                if (this.lstPhoneNumber.Items[i].Selected == true)
                {
                    DeleteRule(this.lstPhoneNumber.Items[i].Value);
                }
            }
            LoadLists();
        }


        //CreditCard Number

        protected void btnNewCCNumber_Click(System.Object sender, System.Web.UI.ImageClickEventArgs e)
        {
            if (this.CreditCardField.Text.Trim().Length > 0)
            {
                FraudRule r = new FraudRule();
                r.RuleType = FraudRuleType.CreditCardNumber;
                r.RuleValue = this.CreditCardField.Text.Trim().ToLower();
                repository.Create(r);
            }
            LoadLists();
        }

        protected void btnDeleteCCNumber_Click(System.Object sender, System.Web.UI.ImageClickEventArgs e)
        {
            for (int i = 0; i <= this.lstCreditCard.Items.Count - 1; i++)
            {
                if (this.lstCreditCard.Items[i].Selected == true)
                {
                    DeleteRule(this.lstCreditCard.Items[i].Value);
                }
            }
            LoadLists();
        }

        private void DeleteRule(string bvin)
        {
            repository.Delete(bvin);
        }

    }
}