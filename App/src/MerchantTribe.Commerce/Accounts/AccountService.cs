using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MerchantTribe.Web;

namespace MerchantTribe.Commerce.Accounts
{
    public class AccountService
    {        
        private RequestContext context = null;
        public UserAccountRepository AdminUsers { get; private set; }
        public ApiKeyRepository ApiKeys { get; private set; }
        public AuthTokenRepository AuthTokens { get; private set; }
        public StoreUserRelationshipRepository AdminUsersXStores { get; private set; }
        public StoreRepository Stores { get; set; }

        public static AccountService InstantiateForMemory(RequestContext c)
        {
            return new AccountService(c,
                                     UserAccountRepository.InstantiateForMemory(c),
                                     AuthTokenRepository.InstantiateForMemory(c),
                                     ApiKeyRepository.InstantiateForMemory(c),
                                     StoreUserRelationshipRepository.InstantiateForMemory(c),
                                     StoreRepository.InstantiateForMemory(c));            
        }
        public static AccountService InstantiateForDatabase(RequestContext c)
        {
            return new AccountService(c,
                                     UserAccountRepository.InstantiateForDatabase(c),
                                     AuthTokenRepository.InstantiateForDatabase(c),
                                     ApiKeyRepository.InstantiateForDatabase(c),
                                     StoreUserRelationshipRepository.InstantiateForDatabase(c),
                                     StoreRepository.InstantiateForDatabase(c));   
        }
        public AccountService(RequestContext c,
                              UserAccountRepository userAccounts,
                              AuthTokenRepository authTokens,
                              ApiKeyRepository apiKeys,
                              StoreUserRelationshipRepository adminUsersXStores,
                              StoreRepository stores)
        {            
            context = c;
            AdminUsers = userAccounts;
            AuthTokens = authTokens;
            ApiKeys = apiKeys;
            AdminUsersXStores = adminUsersXStores;
            Stores = stores;
        }

        // Admin Users
        public bool AuthenticateAdminUser(string email, string password, ref string errorMessage)
        {
            bool result = false;

            try
            {
                UserAccount u = AdminUsers.FindByEmail(email);
                if (u == null)
                {
                    errorMessage = "Please check your email address and password and try again.";
                    return false;
                }

                if (!u.DoesPasswordMatch(password))
                {
                    errorMessage = "Please check your email address and password and try again.";
                    return false;
                }

                if (u.Status == UserAccountStatus.Disabled)
                {
                    errorMessage = "Your account is not currently active. Please contact an administrator for details.";
                    return false;
                }

                return true;                
            }
            catch (Exception ex)
            {
                result = false;
                EventLog.LogEvent(ex);
                errorMessage = "Unknown login error. Contact administrator for assistance.";
            }
            return result;
        }

        public bool LoginAdminUser(string email, string password, ref string errorMessage, System.Web.HttpContextBase httpContext, MerchantTribeApplication app)
        {
            bool result = false;

            try
            {
                UserAccount u = AdminUsers.FindByEmail(email);
                if (u == null)
                {
                    errorMessage = "Please check your email address and password and try again.";
                    return false;
                }

                if (!u.DoesPasswordMatch(password))
                {
                    errorMessage = "Please check your email address and password and try again.";
                    return false;
                }

                if (u.Status == UserAccountStatus.Disabled)
                {
                    errorMessage = "Your account is not currently active. Please contact an administrator for details.";
                    return false;
                }

                AuthToken token = new AuthToken();
                token.UserId = u.Id;
                token.Expires = DateTime.UtcNow.AddDays(WebAppSettings.AuthenticationTokenValidForDays());

                if (AuthTokens.Create(token))
                {
                    Cookies.SetCookieGuid(WebAppSettings.CookieNameAuthenticationTokenAdmin(app.CurrentStore.Id),
                                                    token.TokenId,
                                                    httpContext, false, new EventLog());
                    result = true;
                }
                else
                {
                    errorMessage = "There was a problem with your authentication token. Please contact an administrator for assistance.";
                    return false;
                }
            }
            catch (Exception ex)
            {
                result = false;
                EventLog.LogEvent(ex);
                errorMessage = "Unknown login error. Contact administrator for assistance.";
            }

            return result;
        }
        public bool LogoutAdminUser(System.Web.HttpContextBase httpContext, MerchantTribeApplication app)
        {
            bool result = true;

            Cookies.SetCookieGuid(WebAppSettings.CookieNameAuthenticationTokenAdmin(app.CurrentStore.Id),
                                                    System.Guid.NewGuid(),
                                                    httpContext, false, new EventLog());

            return result;
        }
        public UserAccount FindAdminUserByAuthTokenId(Guid tokenId)
        {            
            AuthToken t = AuthTokens.FindByTokenId(tokenId);
            if (t == null) return null;

            return AdminUsers.FindById(t.UserId);
        }
        public List<UserAccount> FindAdminUsersByStoreId(long storeId)
        {
            List<UserAccount> result = new List<UserAccount>();

            List<StoreUserRelationship> relationships = AdminUsersXStores.FindByStoreId(storeId);

            foreach (StoreUserRelationship rel in relationships)
            {
                result.Add(AdminUsers.FindById(rel.UserId));
            }

            return result;
        }
        public bool AdminUserResetRequest(string email, Accounts.Store store)
        {
            UserAccount u = AdminUsers.FindByEmail(email);
            if (u == null)
            {
                return false;
            }
            u.ResetKey = System.Guid.NewGuid().ToString();
            AdminUsers.Update(u);
            Utilities.MailServices.SendAdminUserResetLink(u, store);
            return true;
        }

        // Auth Tokens
        public bool IsTokenValidForStore(long storeId, Guid tokenId)
        {
            bool result = false;

            AuthToken t = AuthTokens.FindByTokenId(tokenId);
            if (t == null) return false;
            if (t.Expires < DateTime.UtcNow) return false;

            UserAccount u = AdminUsers.FindById(t.UserId);
            if (u == null) return false;

            if (DoesUserHaveAccessToStore(storeId, u.Id))
            {
                // Make sure account isn't disabled
                if (u.Status != UserAccountStatus.Disabled)
                {
                    result = true;
                }
            }
            else
            {
                // check to see if we're a super user
                if (u.Status == UserAccountStatus.SuperUser)
                {
                    result = true;
                }
            }           

            return result;
        }
        public bool IsTokenValidForSuperUser(Guid tokenId)
        {            
            AuthToken t = AuthTokens.FindByTokenId(tokenId);
            if (t == null) return false;
            if (t.Expires < DateTime.UtcNow) return false;

            UserAccount u = AdminUsers.FindById(t.UserId);
            if (u == null) return false;

            if (u.Status == UserAccountStatus.SuperUser) return true;

            return false;            
        }
        public bool IsTokenValid(Guid tokenId)
        {
            AuthToken t = AuthTokens.FindByTokenId(tokenId);
            if (t == null) return false;
            if (t.Expires < DateTime.UtcNow) return false;

            return true;                        
        }

        // Stores X Users
        public bool DoesUserHaveAccessToStore(long storeId, long userId)
        {

            List<StoreUserRelationship> relationships = AdminUsersXStores.FindByStoreId(storeId);
            foreach (StoreUserRelationship r in relationships)
            {
                if (r.UserId == userId)
                {
                    return true;
                }
            }
            return false;
        }
        public bool IsUserStoreOwner(long storeId, long userId)
        {
            List<StoreUserRelationship> relationships = AdminUsersXStores.FindByStoreId(storeId);
            foreach (StoreUserRelationship r in relationships)
            {
                if (r.UserId == userId)
                {
                    if (r.AccessMode == StoreAccessMode.Owner)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        
        //Store
        public Store FindStoreByIdForUser(long storeId, long userId)
        {
            Store result = null;

            List<StoreUserRelationship> relationships = AdminUsersXStores.FindByUserId(userId);

            foreach (StoreUserRelationship rel in relationships)
            {
                if (rel.StoreId == storeId)
                {
                    result = Stores.FindById(storeId);
                    break;
                }
            }

            return result;
        }
        public bool RemoveUserFromStore(long storeId, long userId)
        {
            return AdminUsersXStores.Delete(storeId, userId);
        }
        public bool RemoveAllUsersFromStore(long storeId)
        {

            List<StoreUserRelationship> relationships = AdminUsersXStores.FindByStoreId(storeId);

            foreach (StoreUserRelationship rel in relationships)
            {
                bool temp = AdminUsersXStores.Delete(storeId, rel.UserId);
                if (temp == false) return false;
            }

            return true;
        }
        public bool AddUserToStore(long storeId, long userId, StoreAccessMode mode)
        {
            bool result = false;

            StoreUserRelationship existing = AdminUsersXStores.FindByUserIdAndStoreId(userId, storeId);
            if (existing != null) return true;

            StoreUserRelationship rel = new StoreUserRelationship();

            rel.UserId = userId;
            rel.StoreId = storeId;
            rel.AccessMode = mode;

            result = AdminUsersXStores.Create(rel);

            return result;
        }
        public bool AddUserToStoreByEmail(long storeId, string email, StoreAccessMode mode)
        {
            UserAccount u = AdminUsers.FindByEmail(email);
            if (u == null)
            {
                u = new UserAccount();
                u.Email = email;
                string password = Utilities.PasswordGenerator.GeneratePassword();
                u.HashedPassword = password;
                AdminUsers.Create(u);
            }

            if (AddUserToStore(storeId, u.Id, mode))
            {
                Store s = Stores.FindById(storeId);
                Utilities.MailServices.SendAccountInformation(u, s);
                return true;
            }

            return false;
        }
        
        public bool CancelStore(long storeId, long userId)
        {
            Store s = FindStoreByIdForUser(storeId, userId);
            if (s == null) return false;
            if (s.Id < 1) return false;

            s.DateCancelled = DateTime.UtcNow;
            s.Status = StoreStatus.Deactivated;
            return Stores.Update(s);
        }
        public List<Store> FindStoresForUserId(long userId)
        {
            List<Store> result = new List<Store>();

            List<StoreUserRelationship> relationships = AdminUsersXStores.FindByUserId(userId);
            foreach (StoreUserRelationship rel in relationships)
            {
                result.Add(Stores.FindById(rel.StoreId));
            }

            return result;
        }
        public int CountOfActiveStoresByUserId(long userId)
        {
            int result = 0;
            List<Store> stores = FindStoresForUserId(userId);
            if (stores != null)
            {
                foreach (Store s in stores)
                {
                    if (s.Status == StoreStatus.Active)
                    {
                        result += 1;
                    }
                }
            }
            return result;
        }
        public int CountOfStoresByUserId(long userId)
        {
            int result = 0;
            List<Store> stores = FindStoresForUserId(userId);
            if (stores != null)
            {
                result = stores.Count;
            }
            return result;
        }
        public Store CreateAndSetupStore(string storeName,
                                                long userAccountId,
                                                string friendlyName,
                                                int plan,
                                                decimal rate
                                                )
        {
            Store s = null;

            if (StoreNameExists(storeName))
                throw new CreateStoreException("That store name is not available. Please choose another name and try again.");

            s = new Store();
            s.StoreName = Text.ForceAlphaNumericOnly(storeName).ToLower();
            s.Status = StoreStatus.Active;
            s.DateCreated = DateTime.UtcNow;
            s.PlanId = plan;
            s.CustomUrl = string.Empty;

            if (!Stores.Create(s))
            {
                throw new CreateStoreException("Unable to create store. Unknown error. Please contact an administrator for assistance.");
            }

            s = Stores.FindByStoreName(s.StoreName);
            if (s != null)
            {
                AddUserToStore(s.Id, userAccountId, StoreAccessMode.Owner);

                s.Settings.FriendlyName = friendlyName;
                s.Settings.MailServer.FromEmail = "noreply@merchanttribe.com";
                s.Settings.LastOrderNumber = 0;
                s.Settings.LogoImage = "[[default]]";
                s.Settings.LogoRevision = 0;
                s.Settings.UseLogoImage = false;
                s.Settings.LogoText = s.StoreName;
                s.Settings.MinumumOrderAmount = 0;

                // Send Reminder of account information to new user
                Accounts.UserAccount u = AdminUsers.FindById(userAccountId);

                s.Settings.MailServer.EmailForGeneral = u.Email;
                s.Settings.MailServer.EmailForNewOrder = u.Email;
                s.Settings.MailServer.UseCustomMailServer = false;
                s.Settings.ProductReviewCount = 3;
                s.Settings.ProductReviewModerate = true;
                s.Settings.ProductReviewShowRating = true;
                s.Settings.PayPal.FastSignupEmail = u.Email;
                s.Settings.PayPal.Currency = "USD";
                s.Settings.MaxItemsPerOrder = 999;
                s.Settings.MaxWeightPerOrder = 9999;
                s.Settings.MaxOrderMessage = "That's a really big order! Call us instead of ordering online.";

                s.CurrentPlanRate = rate;
                s.CurrentPlanDayOfMonth = DateTime.Now.Day;
                if (s.CurrentPlanDayOfMonth > 28)
                {
                    s.CurrentPlanDayOfMonth = 28;
                }
                HostedPlan thePlan = HostedPlan.FindById(s.PlanId);
                if (thePlan != null)
                {
                    s.CurrentPlanPercent = thePlan.PercentageOfSales;
                }
                else
                {
                    if (plan == 0)
                    {
                        s.CurrentPlanPercent = 0;
                    }
                    else
                    {
                        s.CurrentPlanPercent = 0;
                    }
                }

                // Save data to store 
                Stores.Update(s);
                
                Utilities.MailServices.SendLeadAlert(u, s);
                Utilities.MailServices.SendAccountInformation(u, s);

            }

            return s;
        }

        private Store CreateIndividualStore()
        {
            Store s = null;

            string storeName = "www";

            s = new Store();
            s.StoreName = Text.ForceAlphaNumericOnly(storeName).ToLower();
            s.Status = StoreStatus.Active;
            s.DateCreated = DateTime.UtcNow;
            s.PlanId = 99;
            s.CustomUrl = string.Empty;

            if (!Stores.Create(s))
            {
                throw new CreateStoreException("Unable to create store. Unknown error. Please contact an administrator for assistance.");
            }

            s = Stores.FindByStoreName(s.StoreName);
            if (s != null)
            {
                UserAccount mainAccount = new UserAccount();
                mainAccount.Email = "admin@merchanttribe.com";
                mainAccount.HashedPassword = "password";
                mainAccount.Status = UserAccountStatus.Active;
                AdminUsers.Create(mainAccount);
                mainAccount = AdminUsers.FindByEmail(mainAccount.Email);

                AddUserToStore(s.Id, mainAccount.Id, StoreAccessMode.Owner);

                s.Settings.FriendlyName = "My MerchantTribe Store";
                s.Settings.MailServer.FromEmail = "noreply@merchanttribe.com";
                s.Settings.LastOrderNumber = 0;
                s.Settings.LogoImage = "[[default]]";
                s.Settings.LogoRevision = 0;
                s.Settings.UseLogoImage = false;
                s.Settings.LogoText = s.StoreName;
                s.Settings.MinumumOrderAmount = 0;
                s.Settings.MailServer.EmailForGeneral = mainAccount.Email;
                s.Settings.MailServer.EmailForNewOrder = mainAccount.Email;
                s.Settings.MailServer.UseCustomMailServer = false;
                s.Settings.ProductReviewCount = 3;
                s.Settings.ProductReviewModerate = true;
                s.Settings.ProductReviewShowRating = true;
                s.Settings.PayPal.FastSignupEmail = mainAccount.Email;
                s.Settings.PayPal.Currency = "USD";
                s.Settings.MaxItemsPerOrder = 999;
                s.CurrentPlanRate = 0;
                s.CurrentPlanDayOfMonth = DateTime.Now.Day;
                s.CurrentPlanPercent = 0;

                // Save data to store 
                Stores.Update(s);

                // No longer need to redirect because home controller handles this
                //System.Web.HttpContext.Current.Response.Redirect("~/adminaccount/login?wizard=1");

                // Force this store into the request context so
                // non-repository datalayer will read in the correct
                // store id
                //RequestContext demoContext = new RequestContext();
                //demoContext.CurrentStore = s;
                //RequestContext.ForceCurrentRequestContext(demoContext);

                //// Add Sample Data
                //AddSampleProductsToStore(demoContext);

                //// Set a default theme
                //Content.ThemeManager m = new Content.ThemeManager(demoContext);
                //m.InstallTheme("cf09d318-3792-47b8-a207-a9502f96f0f9");
            }

            return s;
        }
        public Store FindOrCreateIndividualStore()
        {
            Store result = null;

            result = Stores.FindByStoreName("www");

            if (result == null)
            {
                result = CreateIndividualStore();
            }
            else
            {
                if (result.Id < 1)
                {
                    result = CreateIndividualStore();
                }
            }

            return result;           
        }
        public bool IsStoreNameReserved(string storeName)
        {
            string working = Text.ForceAlphaNumericOnly(storeName).ToLower();

            bool result = false;

            // Length Check of 3 or more
            if (working.Trim().Length < 3) return true;

            List<String> reservedWords = new List<string>();
            reservedWords.Add("www");
            reservedWords.Add("bvc");
            reservedWords.Add("marcus");
            reservedWords.Add("bvcommerce");
            reservedWords.Add("merchanttribe");
            reservedWords.Add("help");
            reservedWords.Add("support");
            reservedWords.Add("info");
            reservedWords.Add("greenspring");
            reservedWords.Add("cenacle");
            reservedWords.Add("shit");
            reservedWords.Add("piss");
            reservedWords.Add("cock");
            reservedWords.Add("ass");
            reservedWords.Add("fuck");
            reservedWords.Add("fucker");
            reservedWords.Add("bvsucks");
            reservedWords.Add("sucks");
            reservedWords.Add("test");
            reservedWords.Add("demo");
            reservedWords.Add("sample");
            reservedWords.Add("trial");
            reservedWords.Add("warez");
            reservedWords.Add("directory");
            reservedWords.Add("free");
            reservedWords.Add("bvguy");
            reservedWords.Add("extendbv");
            reservedWords.Add("dildo");
            reservedWords.Add("dildos");
            reservedWords.Add("porn");
            reservedWords.Add("xxx");
            reservedWords.Add("xrated");
            reservedWords.Add("forum");
            reservedWords.Add("forums");
            reservedWords.Add("beta");
            reservedWords.Add("info");
            reservedWords.Add("wwww");
            reservedWords.Add("web");

            if (reservedWords.Contains(working)) return true;

            return result;
        }
        public bool StoreNameExists(string storeName)
        {
            string working = Text.ForceAlphaNumericOnly(storeName).ToLower();

            if (IsStoreNameReserved(working))
            {
                return true;
            }

            Store s = Stores.FindByStoreName(working);
            if (s != null)
            {
                if (s.StoreName == working)
                {
                    return true;
                }
            }
            return false;
        }

        public bool ChangePlan(long storeId, long userId, int newPlanId, MerchantTribeApplication app)
        {

            Store s = Stores.FindById(storeId);
            if (s == null) return false;
            if (s.Id < 1) return false;
            if (s.Status == StoreStatus.Deactivated) return false;

            UserAccount a = AdminUsers.FindById(userId);
            if (a == null) return false;
            if (a.Id < 0) return false;

            HostedPlan newPlan = HostedPlan.FindById(newPlanId);
            if (newPlan == null) return false;

            if (newPlanId > s.PlanId)
            {
                return UpgradePlan(s, a, newPlan);
            }
            if (newPlanId < s.PlanId)
            {
                return DowngradePlan(s, a, newPlan, app);
            }

            // no chanes to plan
            return true;
        }
        private bool UpgradePlan(Store s, UserAccount a, HostedPlan newPlan)
        {
            s.CurrentPlanRate = newPlan.Rate;
            s.CurrentPlanPercent = newPlan.PercentageOfSales;
            s.CurrentPlanDayOfMonth = DateTime.Now.Day;
            if (s.CurrentPlanDayOfMonth > 28) s.CurrentPlanDayOfMonth = 28;

            s.PlanId = newPlan.Id;
            bool result = Stores.Update(s);
            if (result)
            {
                // Charge Card
                // Notify Admin of Change
                Utilities.MailServices.SendPlanUpgradeAlert(a, s);
            }
            return result;
        }
        private bool DowngradePlan(Store s, UserAccount a, HostedPlan newPlan, MerchantTribeApplication app)
        {
            int currentProductCount = app.CatalogServices.Products.FindAllCount(s.Id);
            if (currentProductCount > newPlan.MaxProducts) return false;

            s.CurrentPlanRate = newPlan.Rate;
            s.CurrentPlanPercent = newPlan.PercentageOfSales;
            //s.CurrentPlanDayOfMonth = DateTime.Now.Day;
            //if (s.CurrentPlanDayOfMonth > 28) s.CurrentPlanDayOfMonth = 28;

            s.PlanId = newPlan.Id;
            bool result = Stores.Update(s);
            if (result)
            {
                // Charge Card
                // Notify Admin of Change
                Utilities.MailServices.SendPlanDowngradeAlert(a, s);
            }
            return result;
        }
       


    }
}
