using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribeStore.api.rest
{
    public interface IRestHandler
    {
        MerchantTribe.Commerce.MerchantTribeApplication MTApp { get; set; }

        string GetAction(string parameters, System.Collections.Specialized.NameValueCollection querystring);
        string PostAction(string parameters, System.Collections.Specialized.NameValueCollection querystring, string postdata);
        string PutAction(string parameters, System.Collections.Specialized.NameValueCollection querystring, string postdata);
        string DeleteAction(string parameters, System.Collections.Specialized.NameValueCollection querystring, string postdata);
    }
}
