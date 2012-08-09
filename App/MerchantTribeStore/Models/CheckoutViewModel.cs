using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MerchantTribe.Commerce;
using MerchantTribe.Commerce.Orders;
using MerchantTribe.Commerce.Utilities;
using MerchantTribe.Commerce.Membership;

namespace MerchantTribeStore.Models
{
    public class CheckoutViewModel
    {
        public Order CurrentOrder { get; set; }
        public CustomerAccount CurrentCustomer { get; set; }
        public bool AgreedToTerms { get; set; }
        public string AgreedToTermsDescription { get; set; }
        public bool ShowAgreeToTerms { get; set; }
        public bool IsLoggedIn { get; set; }
        public string ButtonCheckoutUrl { get; set; }
        public string ButtonLoginUrl { get; set; }
        public string ButtonCancelUrl { get; set; }
        public string LabelRewardsUse { get; set; }
        public string LabelRewardPoints { get; set; }
        public string LabelTerms { get; set; }
        public bool BillShipSame { get; set; }
        public bool ShowRewards { get; set; }
        public bool UseRewardsPoints { get; set; }
        public string RewardPointsAvailable { get; set; }
        public List<MerchantTribe.Web.Geography.Country> Countries { get; set; }
        public List<MerchantTribe.Web.Validation.RuleViolation> Violations { get; set; }
        public CheckoutPaymentViewModel PaymentViewModel { get; set; }
        public string ErrorCssClass { get; set; }

        public string PayPalToken { get; set; }
        public string PayPalPayerId { get; set; }

        public string SideColumn { get; set; }

        public CheckoutViewModel()
        {
            this.CurrentOrder = new Order();
            this.CurrentCustomer = new CustomerAccount();
            this.AgreedToTerms = false;
            this.IsLoggedIn = false;
            this.ButtonCheckoutUrl = string.Empty;
            this.ButtonLoginUrl = string.Empty;
            this.ButtonCancelUrl = string.Empty;
            this.LabelRewardPoints = "Reward Points";
            this.BillShipSame = true;
            this.ShowRewards = false;
            this.UseRewardsPoints = false;
            this.RewardPointsAvailable = string.Empty;
            this.ShowAgreeToTerms = false;
            this.LabelRewardsUse = string.Empty;
            this.LabelTerms = string.Empty;
            this.AgreedToTermsDescription = string.Empty;
            this.Countries = new List<MerchantTribe.Web.Geography.Country>();
            this.Violations = new List<MerchantTribe.Web.Validation.RuleViolation>();
            this.PaymentViewModel = new CheckoutPaymentViewModel();
            this.ErrorCssClass = "input-validation-error";
            this.PayPalPayerId = string.Empty;
            this.PayPalToken = string.Empty;
            this.SideColumn = string.Empty;
        }

        public string IsErr(string nameWithoutPrefix)
        {
            string result = string.Empty;

            if (this.Violations != null)
            {
                var v = this.Violations.Where(y => y.ControlName == (nameWithoutPrefix)).FirstOrDefault();
                if (v != null)
                {
                    return this.ErrorCssClass;
                }
            }
            return result;
        }
    }
}