using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MerchantTribe.Commerce.Accounts;
using System.ComponentModel.DataAnnotations;

namespace MerchantTribeStore.Models
{
    public class SuperStoreViewModel: MerchantTribe.Commerce.Accounts.Store
    {
        public List<UserAccount> Users { get; set; }

        public SuperStoreViewModel()
            : base()
        {
            this.Users = new List<UserAccount>();
        }

        public SuperStoreViewModel(Store s)
            : base()
        {
            this.Users = new List<UserAccount>();

            this.DateCreated = s.DateCreated;
            this.DateCancelled = s.DateCancelled;
            this.CurrentPlanDayOfMonth = s.CurrentPlanDayOfMonth;
            this.CurrentPlanPercent = s.CurrentPlanPercent;
            this.CurrentPlanRate = s.CurrentPlanRate;
            this.CustomUrl = s.CustomUrl;
            this.Id = s.Id;            
            this.PlanId = s.PlanId;            
            this.Settings = s.Settings;
            this.Status = s.Status;
            this.StoreName = s.StoreName;                        
        }
    }
}