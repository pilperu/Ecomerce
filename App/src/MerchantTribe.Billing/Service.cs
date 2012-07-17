using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Billing
{
    public class Service
    {        

        public Data.IBillingAccountRepository Accounts { get; set; }
      
        public Service(string connectionString)
        {
            Accounts = new Data.BillingAccountRepository(connectionString);
        }

        public Service(Data.IBillingAccountRepository accountRepository)
        {
            Accounts = accountRepository;
        }
         
    }
}
