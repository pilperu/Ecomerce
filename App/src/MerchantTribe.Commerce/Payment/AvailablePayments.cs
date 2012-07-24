using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace MerchantTribe.Commerce.Payment
{
    [Serializable()]
    public class AvailablePayments
    {
        public AvailablePayments()
        {
            Populate();
        }

        private void Populate()
        {
            _Methods.Add(new Payment.Method.CreditCard());
            _Methods.Add(new Payment.Method.PurchaseOrder());
            _Methods.Add(new Payment.Method.CompanyAccount());
            _Methods.Add(new Payment.Method.Check());
            //_Methods.Add(new Payment.Method.Cash());
            _Methods.Add(new Payment.Method.Telephone());
            _Methods.Add(new Payment.Method.CashOnDelivery());
            //_Methods.Add(new Payment.Method.GiftCertificate());
            _Methods.Add(new Payment.Method.PaypalExpress());            
        }
        private Collection<DisplayPaymentMethod> _Methods = new Collection<DisplayPaymentMethod>();

        public Collection<DisplayPaymentMethod> Methods
        {
            get { return _Methods; }
        }

        public Collection<DisplayPaymentMethod> AvailableMethodsForPlan(int planId)
        {
            return _Methods;                        
        }
        public Collection<DisplayPaymentMethod> EnabledMethods(Accounts.Store currentStore)
        {
            Collection<DisplayPaymentMethod> result = new Collection<DisplayPaymentMethod>();

            Dictionary<string, string> enabledList = currentStore.Settings.PaymentMethodsEnabled;
            if (enabledList != null)
            {
                for (int i = 0; i <= Methods.Count - 1; i++)
                {
                    if (enabledList.ContainsKey(Methods[i].MethodId))
                    {
                        result.Add(Methods[i]);
                    }
                }
            }

            return result;
        }      

    }

}
