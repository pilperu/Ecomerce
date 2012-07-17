using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Commerce.Accounts.Billing
{
    public class CreateCustomerResponse
    {
        public string NewCustomerId { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }

        public CreateCustomerResponse()
        {
            NewCustomerId = string.Empty;
            Success = false;
            Message = string.Empty;
        }
    }
}
