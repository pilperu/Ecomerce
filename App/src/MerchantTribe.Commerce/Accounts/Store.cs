using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MerchantTribe.Web.Validation;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using MerchantTribe.Payment;

namespace MerchantTribe.Commerce.Accounts
{
    public class Store: IValidatable
    {
        
        public long Id { get; set; }
        public string StoreName { get; set; }
        public StoreStatus Status { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateCancelled {get;set;}
        private long SubscriptionId { get; set; }
        public int PlanId { get; set; }
        private string _CustomUrl = string.Empty;
        public string CustomUrl
        {
            get
            { return _CustomUrl; }
            set
            { _CustomUrl = value.Trim().ToLowerInvariant(); }
        }        
        public long MaxProducts
        {
            get
            {
                HostedPlan thePlan = HostedPlan.FindById(this.PlanId);
                if (thePlan != null)
                {
                    return thePlan.MaxProducts;
                }
                return 10;
            }
        }
        public decimal MaxMonthlySales
        {
            get
            {
                HostedPlan thePlan = HostedPlan.FindById(this.PlanId);
                if (thePlan != null)
                {
                    return thePlan.SalesCap;
                }
                return 1000;
            }
        }      
        public string PlanName { 
        get {
                HostedPlan thePlan = HostedPlan.FindById(this.PlanId);
                if (thePlan != null)
                {
                    return thePlan.Name;
                }
                return "Uknown";          
            }
        }
        public decimal CurrentPlanRate { get; set; }
        public decimal CurrentPlanPercent { get; set; }
        public int CurrentPlanDayOfMonth { get; set; }

        public string StripeCustomerId { get; set; }

        public StoreSettings Settings {get;set;}
   

        public Store()
        {
            Id = 0;
            StoreName = string.Empty;
            Status = StoreStatus.Active;
            DateCreated = DateTime.UtcNow;
            PlanId = 0;
            SubscriptionId = 0;
            CurrentPlanPercent = 0;
            CurrentPlanRate = 0;
            CurrentPlanDayOfMonth = 1;
            Settings = new StoreSettings(this);
            this.StripeCustomerId = string.Empty;
        }

        public string StoreUniqueId(MerchantTribeApplication app)
        {
            string result = Settings.UniqueId;
            if (result == string.Empty)
            {
                result = System.Guid.NewGuid().ToString();
                this.Settings.UniqueId = result;
                app.AccountServices.Stores.Update(this);
            }
            return result;
        }
                     
        public bool HasCustomUrl
        {
            get
            {
                if (this.CustomUrl.Trim().ToLowerInvariant().Length > 0)
                {
                    return true;
                }
                return false;
            }
        }
                       
        public string RootUrl()
        {
            // Individual Mode
            if (WebAppSettings.IsIndividualMode)
            {
                return WebAppSettings.BaseApplicationUrl;
            }

            // Multi Mode
            string result = string.Empty;

            if (this.HasCustomUrl)
            {
                result = "http://" + this.CustomUrl + "/";
            }
            else
            {
                result = StoreName;
                if (!StoreName.StartsWith("http"))
                {
                    result = WebAppSettings.BaseApplicationUrl.Replace("www", StoreName);
                }
            }
            return result;
        }
        public string RootUrlSecure()
        {
            string result = RootUrl();
            result = result.Replace("http://", "https://");
            return result;
        }

        #region IValidatable Members

        public bool IsValid()
        {
            if (GetRuleViolations().Count > 0)
            {
                return false;
            }
            return true;
        }

        public List<RuleViolation> GetRuleViolations()
        {
            List<RuleViolation> violations = new List<RuleViolation>();

            ValidationHelper.LengthCheck(3, 255, "Store Name", StoreName, violations, "storename");

            return violations;
        }

        #endregion
       
    }
}

