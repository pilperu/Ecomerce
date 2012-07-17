using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Commerce.Accounts.Billing
{
        public class UpdateCustomerResponse
        {
            public bool Success { get; set; }
            public string Message { get; set; }

            public UpdateCustomerResponse()
            {                
                Success = false;
                Message = string.Empty;
            }
        }
    
}
