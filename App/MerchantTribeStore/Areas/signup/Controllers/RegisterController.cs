using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MerchantTribe.Web;
using MerchantTribe.Commerce;
using MerchantTribe.Commerce.Accounts;
using MerchantTribeStore.Areas.signup.Models;

namespace MerchantTribeStore.Areas.signup.Controllers
{
    public class RegisterController : BaseSignupController
    {

        private RegisterViewModel RegisterSetup(string id)
        {
            RegisterViewModel model = new RegisterViewModel();            
            SetPlanFromName(id, model);

            return model;
        }
        private void SetPlanFromName(string id, RegisterViewModel model)
        {
            if (id == null) return;
            if (id == string.Empty) return;

            // Force everything to trial plan
            model.RegistrationData.plan = 0;
            model.PlanName = id.Trim().ToLowerInvariant();

                //switch (id.Trim().ToLower())
                //{
                //    case "starter":
                //        model.RegistrationData.plan = 0;
                //        model.HideCreditCard = true;
                //        model.PlanName = id.Trim().ToLowerInvariant();        
                //        break;
                //    case "basic":
                //        model.RegistrationData.plan = 1;
                //        model.PlanName = id.Trim().ToLowerInvariant();
                //        break;
                //    case "plus":
                //        model.RegistrationData.plan = 2;
                //        model.PlanName = id.Trim().ToLowerInvariant();
                //        break;
                //    case "premium":
                //        model.RegistrationData.plan = 3;
                //        model.PlanName = id.Trim().ToLowerInvariant();
                //        break;
                //    case "max":
                //        model.RegistrationData.plan = 99;
                //        model.PlanName = id.Trim().ToLowerInvariant();
                //        break;
                //    default:
                //        model.RegistrationData.plan = 0;
                //        model.HideCreditCard = true;
                //        model.PlanName = id.Trim().ToLowerInvariant();
                //        break;
                //}            
        }
        private void AddPlanDetails(RegisterViewModel model)
        {
            string PlanDescription = "You are signing up for the <strong>{0}</strong> plan. {2}Your card will be charged <strong>{1}</strong> today and each month after on this day until you cancel.";
            DateTime expires = MerchantTribe.Web.Dates.MaxOutTime(DateTime.Now).AddMonths(1);

            HostedPlan thePlan = HostedPlan.FindById(model.RegistrationData.plan);
            if (thePlan != null)
            {
                string PayPalLead = string.Empty;
                PayPalLead = MerchantTribe.Commerce.SessionManager.GetCookieString("PayPalLead", MTApp.CurrentStore);

                if (thePlan.Id == 0)
                {
                    model.RegistrationData.plandetails = String.Format(PlanDescription, "Free Plan", "$0", "Free plans are never charged. ");
                }
                else
                {
                    if (PayPalLead != string.Empty)
                    {
                        model.RegistrationData.plandetails = String.Format(PlanDescription, thePlan.Name, 49.ToString("c"), "");
                    }
                    else
                    {
                        model.RegistrationData.plandetails = String.Format(PlanDescription, thePlan.Name, thePlan.Rate.ToString("c"), "");
                    }
                }
            }
        }

        // GET: /signup/Register/
        public ActionResult Index(string id)
        {
            RegisterViewModel model = RegisterSetup(id);
            

            return View(model);
        }

        // POST: /signup/register
        [ActionName("Index")]
        [HttpPost]
        public ActionResult IndexPost(string id)
        {
            RegisterViewModel model = RegisterSetup(id);
            LoadModelFromForm(model);

            // See if we're skipping the agree checkbox
            if (model.FromHomePage)
            {
                model.Agreed = true;                
            }

            DoSignUp(model);
            
            return View(model);
        }
        private void LoadModelFromForm(RegisterViewModel model)
        {
            if (Request.Form != null)
            {
                model.RegistrationData.email = Request.Form["email"] ?? model.RegistrationData.email;
                model.RegistrationData.password = Request.Form["password"] ?? model.RegistrationData.password;
                model.RegistrationData.storename = Request.Form["storename"] ?? model.RegistrationData.storename;                                                                                
                model.Agreed = Request.Form["chkagree"] != null;
                if (Request.Form["hiddenaction"] != null) model.FromHomePage = true;
            }
        }
        private void DoSignUp(RegisterViewModel model)
        {                                                
            bool storeOkay = false;

            MerchantTribe.Commerce.Accounts.Store testStore = new MerchantTribe.Commerce.Accounts.Store();
            testStore.StoreName = model.RegistrationData.storename;
            if (!testStore.IsValid())
            {
                foreach (MerchantTribe.Web.Validation.RuleViolation v in testStore.GetRuleViolations())
                {
                    RenderError(v, model);
                }
            }
            else
            {
                if (MTApp.AccountServices.StoreNameExists(testStore.StoreName))
                {
                    RenderError("storename", "A store with that name already exists. Choose another name and try again.", model);
                }
                else
                {
                    storeOkay = true;
                }
            }                        

            UserAccount u = MTApp.AccountServices.AdminUsers.FindByEmail(model.RegistrationData.email);

            if (u == null)
            {
                u = new MerchantTribe.Commerce.Accounts.UserAccount();
            }

            bool userOk = false;
            if (u.IsValid() && (u.Email == model.RegistrationData.email))
            {
                if (u.DoesPasswordMatch(model.RegistrationData.password))
                {
                    userOk = true;
                }
                else
                {
                    RenderError("email", "A user account with that email address already exists. If it's your account make sure you enter the exact same password as you usually use.", model);
                }
            }
            else
            {
                u = new MerchantTribe.Commerce.Accounts.UserAccount();
                u.Email = model.RegistrationData.email;
                u.HashedPassword = model.RegistrationData.password;

                if (u.IsValid())
                {
                    u.Status = MerchantTribe.Commerce.Accounts.UserAccountStatus.Active;
                    u.DateCreated = DateTime.UtcNow;
                    userOk = MTApp.AccountServices.AdminUsers.Create(u);
                    u = MTApp.AccountServices.AdminUsers.FindByEmail(u.Email);
                }
                else
                {
                    foreach (MerchantTribe.Web.Validation.RuleViolation v in u.GetRuleViolations())
                    {
                        RenderError(v.ControlName, v.ErrorMessage, model);
                    }
                }
            }

            if (userOk && storeOkay)
            {
                try
                {                    
                    bool isPayPalLead = false;
                    if (MerchantTribe.Commerce.SessionManager.GetCookieString("PayPalLead", MTApp.CurrentStore) != string.Empty)
                    {
                        isPayPalLead = true;
                    }

                    decimal rate = 0;
                    HostedPlan thePlan = HostedPlan.FindById(model.RegistrationData.plan);
                    if (thePlan != null)
                    {
                        rate = thePlan.Rate;
                        if (isPayPalLead)
                        {
                            rate = 49;
                        }
                    }

                    MerchantTribe.Commerce.Accounts.Store s = MTApp.AccountServices.CreateAndSetupStore(model.RegistrationData.storename,
                                                                u.Id,
                                                                model.RegistrationData.storename + " store for " + model.RegistrationData.email,
                                                                model.RegistrationData.plan,
                                                                rate);
                    if (s != null)
                    {                        

                        string e = MerchantTribe.Web.Cryptography.Base64.ConvertStringToBase64(u.Email);
                        string st = MerchantTribe.Web.Cryptography.Base64.ConvertStringToBase64(s.StoreName);

                        Response.Redirect("~/signup/ProcessSignUp?e=" + e + "&s=" + st);                        
                    }
                }
                catch (MerchantTribe.Commerce.Accounts.CreateStoreException cex)
                {
                    RenderError("storename", cex.Message, model);
                }
            }
        }
        private void RenderError(MerchantTribe.Web.Validation.RuleViolation v, RegisterViewModel model)
        {
            RenderError(v.ControlName, v.ErrorMessage, model);
        }
        private void RenderError(string control, string message, RegisterViewModel model)
        {
            this.FlashFailure(message);
            model.InvalidFields.Add(control);            
        }

        // GET: /signup/processsignup
        public ActionResult ProcessSignup()
        {
            string email = Request.QueryString["e"];
            string storename = Request.QueryString["s"];

            // Bail out if we don't have an email
            if (String.IsNullOrEmpty(email)) return Redirect("~/signup");
            if (String.IsNullOrEmpty(storename)) return Redirect("~/signup");

            email = MerchantTribe.Web.Cryptography.Base64.ConvertStringFromBase64(email);
            storename = MerchantTribe.Web.Cryptography.Base64.ConvertStringFromBase64(storename);

            // Encode store name for safety from injections
            storename = System.Web.HttpUtility.HtmlEncode(storename);

            string baseUrl = WebAppSettings.ApplicationBaseUrl;
            string rootUrl = baseUrl.Replace("www", storename);
            string rootUrlSecure = rootUrl.Replace("http://", "https://");
            
            ViewBag.RootUrl = rootUrl;
            ViewBag.RootUrlSecure = rootUrlSecure;
            ViewBag.CompleteEmail = email;
            ViewBag.LoginUrl = rootUrlSecure + "adminaccount/login?wizard=1&username="
                                            + System.Web.HttpUtility.UrlEncode(email);            
            return View();
        }
        
        private class JsonCheckStoreNameRequest
        {
            public string storename { get; set; }
        }
        private class JsonOut
        {
            public string cleanstorename { get; set; }
            public string message { get; set; }
        }

        public ActionResult JsonCheckStoreName()
        {                        
            JsonCheckStoreNameRequest data = MerchantTribe.Web.Json.ObjectFromJson<JsonCheckStoreNameRequest>(Request.InputStream);

            string clean = "";
            if (data != null)
            {
                clean = data.storename;
                clean = Text.ForceAlphaNumericOnly(clean);
            }
            string msg = "";
            if (MTApp.AccountServices.StoreNameExists(clean))
            {
                msg = "<div class=\"flash-message-failure\"><strong>" + clean + ".merchanttribestores.com</strong><br />Store name is already taken.</div>";
            }
            else
            {
                msg = "<div class=\"flash-message-success\"><strong>" + clean + ".merchanttribestores.com</strong><br />Store name is available.</div>";
            }

            if (clean == "")
            {
                msg = "<div class=\"flash-message-watermark\">a store name is required<br />&nbsp;</div>";
            }

            JsonOut result = new JsonOut() { cleanstorename = clean, message = msg };

            string json = MerchantTribe.Web.Json.ObjectToJson(result);

            return new MerchantTribeStore.Controllers.PreJsonResult(json);            
        }
    }
}
