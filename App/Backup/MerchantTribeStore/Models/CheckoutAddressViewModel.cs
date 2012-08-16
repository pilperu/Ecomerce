using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MerchantTribe.Commerce.Contacts;

namespace MerchantTribeStore.Models
{
    public class CheckoutAddressViewModel
    {
        public Address Address { get; set; }
        public string Prefix { get; set; }
        public int TabIndex { get; set; }
        public List<MerchantTribe.Web.Geography.Country> Countries { get; set; }
        public bool ShowPhone { get; set; }
        public List<MerchantTribe.Web.Validation.RuleViolation> Violations { get; set; }
        public string ErrorCssClass { get; set; }

        public CheckoutAddressViewModel()
        {
            this.Address = new Address();
            this.Prefix = string.Empty;
            this.TabIndex = 0;
            this.Countries = new List<MerchantTribe.Web.Geography.Country>();
            this.ShowPhone = true;
            this.Violations = new List<MerchantTribe.Web.Validation.RuleViolation>();
            this.ErrorCssClass = "input-validation-error";
        }

        public string IsErr(string nameWithoutPrefix)
        {
            string result = string.Empty;

            if (this.Violations != null)
            {
                var v = this.Violations.Where(y => y.ControlName == (this.Prefix + nameWithoutPrefix)).FirstOrDefault();
                if (v != null)
                {
                    return this.ErrorCssClass;
                }
            }
            return result;
        }
    }
}