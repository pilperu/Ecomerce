using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MerchantTribe.Web;
using MerchantTribe.Web.Logging;

namespace MerchantTribe.Commerce.Membership
{
    public class MembershipServices
    {
        private RequestContext context = null;

        public CustomerAccountRepository Customers { get; private set; }
        public UserQuestionRepository UserQuestions { get; private set; }

        public static MembershipServices InstantiateForMemory(RequestContext c)
        {
            return new MembershipServices(c,
                                      UserQuestionRepository.InstantiateForMemory(c),
                                      CustomerAccountRepository.InstantiateForMemory(c)
                                      );

        }
        public static MembershipServices InstantiateForDatabase(RequestContext c)
        {
            return new MembershipServices(c, 
                                    UserQuestionRepository.InstantiateForDatabase(c),
                                    CustomerAccountRepository.InstantiateForDatabase(c)
                                    );
        }
        public MembershipServices(RequestContext c, 
                            UserQuestionRepository questions,
                            CustomerAccountRepository customers
                            )
        {
            context = c;
            this.UserQuestions = questions;
            this.Customers = customers;
        }

        public bool CreateCustomer(Membership.CustomerAccount u, string clearPassword)
        {
            CreateUserStatus status = CreateUserStatus.None;
            return CreateCustomer(u, ref status, clearPassword);
        }
        public bool CreateCustomer(Membership.CustomerAccount u, ref CreateUserStatus status, string clearPassword)
        {
            bool result = false;

            if (u != null)
            {
                Membership.CustomerAccount testUser = new Membership.CustomerAccount();
                testUser = Customers.FindByEmail(u.Email);
                if (testUser != null)
                {
                    if (testUser.Bvin != string.Empty)
                    {
                        status = CreateUserStatus.DuplicateUsername;
                        return false;
                    }
                }

                if (u.Salt == string.Empty)
                {
                    u.Salt = System.Guid.NewGuid().ToString();
                    u.Password = u.EncryptPassword(clearPassword);
                }

                if (Customers.Create(u) == true)
                {
                    result = true;
                    status = CreateUserStatus.Success;
                }
                else
                {
                    status = CreateUserStatus.UpdateFailed;
                }
            }

            return result;
        }
        public bool UpdateCustomer(Membership.CustomerAccount u)
        {
            CreateUserStatus s = new CreateUserStatus();
            return UpdateCustomer(u, ref s);
        }
        public bool UpdateCustomer(Membership.CustomerAccount u, ref CreateUserStatus status)
        {
            bool result = false;

            if (u != null)
            {
                Membership.CustomerAccount testUser = new Membership.CustomerAccount();
                testUser = Customers.FindByEmail(u.Email);
                if (testUser != null && testUser.Bvin != string.Empty)
                {
                    if (testUser.Bvin != u.Bvin)
                    {
                        status = CreateUserStatus.DuplicateUsername;
                        return false;
                    }
                }

                if (Customers.Update(u) == true)
                {
                    result = true;
                    status = CreateUserStatus.Success;
                }
                else
                {
                    status = CreateUserStatus.UpdateFailed;
                }
            }

            return result;
        }
        public bool ChangePasswordForCustomer(string email, string oldPassword, string newPassword)
        {
            bool result = false;

            CustomerAccount u = Customers.FindByEmail(email);
            if (u != null)
            {
                if (DoPasswordsMatchForCustomer(oldPassword, u) == true)
                {
                    u.Password = u.EncryptPassword(newPassword);
                    Membership.CreateUserStatus s = CreateUserStatus.None;
                    result = UpdateCustomer(u, ref s);
                }
            }

            return result;
        }
        public bool ResetPasswordForCustomer(string email, string newPassword)
        {
            bool result = false;
            CustomerAccount u = Customers.FindByEmail(email);
            if (u != null)
            {
                u.Password = u.EncryptPassword(newPassword);
                Membership.CreateUserStatus s = CreateUserStatus.None;
                result = UpdateCustomer(u, ref s);
            }
            return result;
        }

        public bool DoPasswordsMatchForCustomer(string trialpassword, Membership.CustomerAccount u)
        {
            return u.Password.Equals(u.EncryptPassword(trialpassword), StringComparison.InvariantCulture);
        }
        public bool UpdateCustomerEmail(CustomerAccount user, string newEmail)
        {
            string oldEmail = user.Email;
            user.Email = newEmail;
            bool result = UpdateCustomer(user);
            if (result)
            {
                context.IntegrationEvents.CustomerAccountEmailChanged(oldEmail, newEmail);
            }
            return result;
        }
        public SystemOperationResult ValidateUser(string email, string password)
        {
            SystemOperationResult result = new SystemOperationResult();

            CustomerAccount u = Customers.FindByEmail(email);
            if (u != null)
            {
                if (DoPasswordsMatchForCustomer(password, u) == true)
                {
                    CustomerCheckLock(u);
                    if (u.Locked == false)
                    {
                        // Reset Failed Login Count
                        if (u.FailedLoginCount > 0)
                        {
                            u.FailedLoginCount = 0;
                            UpdateCustomer(u);
                        }
                        result.Success = true;
                    }
                    else
                    {
                        result.Success = false;
                        result.Message = Content.SiteTerms.GetTerm(Content.SiteTermIds.AccountLocked);
                    }
                }
                else
                {
                    result.Message = Content.SiteTerms.GetTerm(Content.SiteTermIds.LoginIncorrect);
                    u.FailedLoginCount += 1;
                    UpdateCustomer(u);
                    CustomerCheckLock(u);
                }
            }
            else
            {
                result.Message = Content.SiteTerms.GetTerm(Content.SiteTermIds.LoginIncorrect);
            }

            if (result.Success == false)
            {
                EventLog.LogEvent("Membership", "Login Failed for User: " + email, EventLogSeverity.Information);
            }

            return result;
        }
        public bool LoginCustomer(string email, string password, ref string errorMessage, System.Web.HttpContextBase context, ref string userId, MerchantTribeApplication app)
        {
            bool result = false;

            try
            {
                SystemOperationResult op = ValidateUser(email, password);
                if (op.Success == false)
                {
                    errorMessage = op.Message;
                    return false;
                }

                CustomerAccount u = Customers.FindByEmail(email);
                if (u == null)
                {
                    errorMessage = "Please check your email address and password and try again.";
                    return false;
                }

                userId = u.Bvin;

                Cookies.SetCookieString(WebAppSettings.CookieNameAuthenticationTokenCustomer(app.CurrentStore.Id),
                                                u.Bvin,
                                                context, false, new EventLog());
                result = true;

            }
            catch (Exception ex)
            {
                result = false;
                EventLog.LogEvent(ex);
                errorMessage = "Unknown login error. Contact administrator for assistance.";
            }

            return result;
        }
        public bool LogoutCustomer(System.Web.HttpContextBase context, MerchantTribeApplication app)
        {
            bool result = true;
            Cookies.SetCookieString(WebAppSettings.CookieNameAuthenticationTokenCustomer(app.CurrentStore.Id),
                                                    "",
                                                    context, false, new EventLog());
            return result;
        }
        public string GeneratePasswordForCustomer(int length)
        {
            return MerchantTribe.Web.PasswordGenerator.GeneratePassword(length);
        }
        // Use this VERY carefully
        public bool DestroyAllCustomers(MerchantTribeApplication app)
        {            
            DateTime current = DateTime.UtcNow;
            DateTime availableUntil = app.CurrentStore.Settings.AllowApiToClearUntil;
            int compareResult = DateTime.Compare(current, availableUntil);
            if (compareResult >= 0)
            {
                return false;
            }
            List<CustomerAccount> all = Customers.FindAll();
            foreach (CustomerAccount a in all)
            {
                Customers.Delete(a.Bvin);
            }
            return true;
        }
        public bool CheckIfNewAddressAndAddWithUpdate(CustomerAccount a, Contacts.Address address)
        {
            bool addressWasAdded = a.CheckIfNewAddressAndAddNoUpdate(address);
            if (addressWasAdded) UpdateCustomer(a);
            return addressWasAdded;
        }                                        
        public void UnlockCustomer(CustomerAccount c)
        {
            c.Locked = false;
            c.FailedLoginCount = 0;
            c.LockedUntilUtc = DateTime.UtcNow.AddMilliseconds(-1);
            UpdateCustomer(c);
        }
        public void LockCustomer(CustomerAccount c)
        {
            c.Locked = true;
            c.LockedUntilUtc = DateTime.UtcNow.AddMinutes(WebAppSettings.UserLockoutMinutes);
            UpdateCustomer(c);
        }
        public void CustomerCheckLock(CustomerAccount c)
        {
            if (c.Locked == true)
            {
                if (DateTime.Compare(DateTime.UtcNow, c.LockedUntilUtc) > 0)
                {
                    UnlockCustomer(c);
                }
            }
            else
            {
                if (c.FailedLoginCount >= WebAppSettings.UserLockoutAttempts)
                {
                    LockCustomer(c);
                    EventLog.LogEvent("Membership", "User Account " + c.Email + " was locked.", EventLogSeverity.Warning);
                }
            }
        }
    }
}
