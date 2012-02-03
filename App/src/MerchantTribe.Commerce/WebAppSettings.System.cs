
namespace MerchantTribe.Commerce
{
    public partial class WebAppSettings
    {

        const string SUPERADMINEMAIL = "noreply@merchanttribe.com";
        const string APPLICATION_NAME = "MerchantTribe";
const string APPLICATION_VERSION = "1.3.2.237";

        public static string SuperAdminEmail
        {
            get { return SUPERADMINEMAIL; }
        }
        public static string SystemProductName
        {
            get { return APPLICATION_NAME; }
        }
        public static string SystemVersionNumber
        {
            get { return APPLICATION_VERSION; }
        }
        public static int MaxProducts
        {
            get { return 32000000; }
        }
        public static int MaxVariants
        {
            get { return 150; }
        }
        public static bool CustomVersion
        {
            get { return false; }
        }

        public static string ApplicationEmail
        {
            get { return "noreply@merchanttribe.com"; }
        }
        public static string BaseApplicationUrl
        {
            get
            {
                return ApplicationBaseUrl;                
            }
        }
        public static string BaseApplicationUrlSecure()
        {
            string result = BaseApplicationUrl;
            result = result.Replace("http://", "https://");
            return result;
        }

        //public static string BaseImageUrl
        //{
        //    get
        //    {
        //        return ApplicationBaseImageUrl;
        //    }
        //}
        //public static string BaseImageUrlSecure()
        //{
        //    string result = BaseImageUrl;
        //    result = result.Replace("http://", "https://");
        //    return result;
        //}

        public static string BaseImagePhysicalPath
        {
            get
            {
                return ApplicationBaseImagePhysicalPath;
            }
        }


        public static string CookieNameCartIdPaymentPending(long storeId)
        {
            return "merchanttribe-cartid-pendingpayment-" + storeId.ToString();
        }
        public static string CookieNameCartId(long storeId)
        {
            return "merchanttribe-cartid-" + storeId.ToString();
        }
        public static string CookieNameCartItemCount(long storeId)
        {
            return "merchanttribe-cartitemcount-" + storeId.ToString();
        }
        public static string CookieNameCartSubTotal(long storeId)
        {
            return "merchanttribe-cartsubtotal-" + storeId.ToString();
        }
        public static string CookieNameAuthenticationTokenAdmin(long storeId)
        {
            return "merchanttribe-authtoken-" + storeId.ToString();
        }
        public static string CookieNameAuthenticationTokenCustomer(long storeId)
        {
            return "merchanttribe-authtokencustomer-" + storeId.ToString();
        }
        public static string CookieNameLastCategory(long storeId)
        {
            return "merchanttribe-lastcategory-" + storeId.ToString();
        }

        public static int AuthenticationTokenValidForDays()
        {
            return 90;
        }
               
        public static int UserLockoutAttempts
        {
            get { return 5; }
        }
        public static int UserLockoutMinutes
        {
            get { return 15; }
        }

    }
}
