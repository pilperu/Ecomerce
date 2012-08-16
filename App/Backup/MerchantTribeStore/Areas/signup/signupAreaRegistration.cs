using System.Web.Mvc;

namespace MerchantTribeStore.Areas.signup
{
    public class signupAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "signup";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            // Signup Content Page Routes
            context.MapRoute("signup-paypal", "paypaloffer", new {controller="MarketingPages", action="paypaloffer"});
            context.MapRoute("signup-tour", "signup/tour", new { controller = "MarketingPages", action = "tour" });
            context.MapRoute("signup-design", "signup/design", new { controller = "MarketingPages", action = "design" });
            context.MapRoute("signup-about", "signup/about", new { controller = "MarketingPages", action = "about" });            
            context.MapRoute("signup-home", "signup/home", new { controller = "MarketingPages", action = "Home" });
            context.MapRoute("signup-featured", "signup/features", new { controller = "MarketingPages", action = "features" });
            context.MapRoute("signup-themes", "signup/premiumthemes", new { controller = "MarketingPages", action = "themes" });
            context.MapRoute("signup-promote", "signup/promote", new { controller = "MarketingPages", action = "promote" });            
            context.MapRoute("signup-sell", "signup/sell", new { controller = "MarketingPages", action = "sell" });
            context.MapRoute("signup-secure", "signup/secure", new { controller = "MarketingPages", action = "secure" });

            // Registration Routes            
            context.MapRoute("signup-register", "signup/register/{id}", new { controller = "Register", action = "index" });
            context.MapRoute("signup-checkstorename", "signup/JsonCheckStoreName", new { controller = "Register", action = "JsonCheckStoreName" });
            context.MapRoute("signup-processsignup", "signup/processsignup", new { controller = "Register", action = "ProcessSignup" });

            context.MapRoute("signup-pricing", "signup", new { controller = "MarketingPages", action = "Pricing" });

            // Wildcard
            context.MapRoute(
                "signup_default",
                "signup/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
