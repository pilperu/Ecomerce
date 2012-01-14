using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Commerce
{
    public class Integration
    {

        public delegate void CustomerAccountEventHandler(object sender, Membership.CustomerAccount account);
        public event CustomerAccountEventHandler OnCustomerAccountUpdated;
        public event CustomerAccountEventHandler OnCustomerAccountCreated;
        public event CustomerAccountEventHandler OnCustomerAccountDeleted;

        public delegate void CustomerAccountEmailChangedHandler(object sender, string oldEmail, string newEmail);
        public event CustomerAccountEmailChangedHandler OnCustomerAccountEmailChanged;

        public delegate void OrderEventHandler(object sender, Orders.Order order, MerchantTribeApplication app);
        public event OrderEventHandler OnOrderReceived;

        public void CustomerAccountUpdated(Membership.CustomerAccount account)
        {
            if (OnCustomerAccountUpdated != null)
            {
                OnCustomerAccountUpdated(this, account);
            }
        }
        public void CustomerAccountCreated(Membership.CustomerAccount account)
        {
            if (OnCustomerAccountCreated != null)
            {
                OnCustomerAccountCreated(this, account);
            }
        }
        public void CustomerAccountDeleted(Membership.CustomerAccount account)
        {
            if (OnCustomerAccountDeleted != null)
            {
                OnCustomerAccountDeleted(this, account);
            }
        }
        public void CustomerAccountEmailChanged(string oldEmail, string newEmail)
        {
            if (OnCustomerAccountEmailChanged != null)
            {
                OnCustomerAccountEmailChanged(this, oldEmail, newEmail);
            }
        }

        public void OrderReceived(Orders.Order order, MerchantTribeApplication app)
        {
            if (OnOrderReceived != null)
            {
                OnOrderReceived(this, order, app);
            }
        }
    }
}
