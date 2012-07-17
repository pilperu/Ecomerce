using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MerchantTribe.Payment;

namespace MerchantTribe.Commerce.Accounts.Billing
{
    public class CreateCustomerRequest
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public string PostalCode { get; set; }
        public string PlanId { get; set; }
        public CardData CreditCard { get; set; }

        public CreateCustomerRequest()
        {
            Email = string.Empty;
            Name = string.Empty;
            PostalCode = string.Empty;
            PlanId = string.Empty;
            CreditCard = new CardData();
        }
    }
}
