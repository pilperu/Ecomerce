using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Commerce.Accounts.Billing
{
    public class GetCustomerResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }

        public string Id { get; set; }
        public string Email { get; set; }
        public string Description { get; set; }
        
        public bool HasCard { get; set; }
        public string PlanId { get; set; }
        public string PlanName { get; set; }
        public decimal PlanAmount { get; set; }
        public string PlanStatus { get; set; }
        public string NextCharge { get; set; }

        public GetCustomerResponse()
        {
            this.Success = false;
            this.Message = string.Empty;
            this.Id = string.Empty;
            this.Email = string.Empty;            
            this.HasCard = false;
            this.PlanId = string.Empty;
            this.PlanName = string.Empty;
            this.PlanAmount = 0;
            this.PlanStatus = string.Empty;
            this.Description = string.Empty;
            this.NextCharge = string.Empty;
        }
    }
}
