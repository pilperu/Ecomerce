using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for RegisterStoreData
/// </summary>
/// 
namespace MerchantTribeStore.app
{
    public class RegisterStoreData
    {        
        public int plan { get; set; }
        public string plandetails { get; set; }        
        public string email { get; set; }
        public string password { get; set; }
        public string storename { get; set; }

        public RegisterStoreData()
        {
            plan = 0;
            plandetails = string.Empty;            
            email = string.Empty;
            password = string.Empty;
            storename = string.Empty;
        }
        
    }
}