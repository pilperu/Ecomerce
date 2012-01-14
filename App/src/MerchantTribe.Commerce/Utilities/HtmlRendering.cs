using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace MerchantTribe.Commerce.Utilities
{
    public class HtmlRendering
    {

        public static string StandardHeader()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("<div id=\"logo-area\">");
            sb.Append("{{logo}}");
            sb.Append("</div>");

            sb.Append("<div id=\"links-area\">");
            sb.Append("{{headerlinks}}");
            sb.Append("</div>");

            sb.Append("<div id=\"cart-link\">");
            sb.Append("{{cartlink}}");
            sb.Append("</div>");            

            sb.Append("<div id=\"search-form\">");
            sb.Append("{{searchform}}");
            sb.Append("</div>");
            
            sb.Append("<div id=\"header-menu\">");
            sb.Append("{{headermenu}}");
            sb.Append("</div>");
            return sb.ToString();
        }

        public static string StandardFooter(MerchantTribeApplication app)
        {
            return "{{copyright}} | <a href=\""
                   + app.StoreUrl(false, true)
                   + "SiteMap\"><span>SiteMap</span></a> ";                                    
        }

        public static string PromoTag()
        {
            string copyrightTag = "<span class=\"bvtag\"><a href=\"http://www.MerchantTribe.com\" title=\"Free Open Source Shopping Cart Software\" target=\"_blank\">Shopping Cart Software by MerchantTribe</a></span>";
            return copyrightTag;
        }

        public static string Logo(MerchantTribeApplication app, bool isSecureRequest)
        {
            string storeRootUrl = app.StoreUrl(isSecureRequest, false);
            string storeName = app.CurrentStore.Settings.FriendlyName;

            string logoImage = app.CurrentStore.Settings.LogoImageFullUrl(app, isSecureRequest);
            string logoText = app.CurrentStore.Settings.LogoText;

            StringBuilder sb = new StringBuilder();

            sb.Append("<a href=\"" + storeRootUrl + "\" title=\"" + storeName + "\"");

            if (app.CurrentStore.Settings.UseLogoImage)
            {
                sb.Append("><img src=\"" + logoImage + "\" alt=\"" + storeName + "\" />");

            }
            else
            {
                sb.Append(" class=\"logo\">");
                sb.Append(System.Web.HttpUtility.HtmlEncode(logoText));
            }
            sb.Append("</a>");

            return sb.ToString();
        }

        public static string LogoText(MerchantTribeApplication app)
        {

            string storeRootUrl = app.StoreUrl(false, true);
            string storeName = app.CurrentStore.Settings.FriendlyName;
            string logoText = app.CurrentStore.Settings.LogoText;

            StringBuilder sb = new StringBuilder();

            sb.Append("<a href=\"" + storeRootUrl + "\" title=\"" + storeName + "\"");
            sb.Append(" class=\"logo\">");
            sb.Append(System.Web.HttpUtility.HtmlEncode(logoText));
            sb.Append("</a>");

            return sb.ToString();
        }

        public static string CartLink(MerchantTribeApplication app, string itemCount)
        {
            string storeRootUrl = app.StoreUrl(false, true);
            StringBuilder sb = new StringBuilder();
            sb.Append("<a href=\"" + storeRootUrl + "cart/\"><span>View Cart: ");
            sb.Append(itemCount);
            sb.Append(" items</span></a>");
            return sb.ToString();
        }

        public static string HeaderMenu(System.Web.Routing.RequestContext routingContext, RequestContext requestContext)
        {
            int linksPerRow = 9;
            int maxLinks = 9;
            int tabIndex = 0;
            int tempTabIndex = 0;
            
            //Find Categories to Display in Menu
            Catalog.CategoryRepository repo = Catalog.CategoryRepository.InstantiateForDatabase(requestContext);
            List<Catalog.CategorySnapshot> categories = repo.FindForMainMenu();
            
            // Limit number of links
            int stopCount = categories.Count -1;
            if ((maxLinks > 0) && ((maxLinks - 1) < stopCount))
            {
                stopCount = (maxLinks - 1);
            }

            StringBuilder sb = new StringBuilder();
        
            //Open List
            if (categories.Count > 0)
            {
                sb.Append("<ul>");
            }

            tempTabIndex = tabIndex;

            //Build each Row
            for (int i = 0; i <= stopCount;i++)
            {
                sb.Append(BuildLink(categories[i], routingContext,ref tempTabIndex));
                // Move to Next Row if Not Last Item
                int endOfRowCount = (i + 1) % linksPerRow;
                
                if ( ( endOfRowCount == 0) && (i < stopCount) )
                {
                    sb.Append("</ul><ul>");
                }
            }

            // Close List
            if (categories.Count > 0)
            {
                sb.Append("</ul>");
            }

            return sb.ToString();
        }

        private static string BuildLink(Catalog.CategorySnapshot c, System.Web.Routing.RequestContext routingContext,ref int tempTabIndex)
        {
            bool displayActiveTab = true;
            string result = string.Empty;

            StringBuilder sbLink = new StringBuilder();

            sbLink.Append("<li");

            if ((c.Bvin == SessionManager.CategoryLastId) && displayActiveTab)
            {
            sbLink.Append(" class=\"activemainmenuitem\" >");
            }
            else
            {
                sbLink.Append(">");
            }

            sbLink.Append("<a href=\"");
            sbLink.Append(Utilities.UrlRewriter.BuildUrlForCategory(c, routingContext));
            
            if (c.CustomPageOpenInNewWindow)
            {
                sbLink.Append("\" target=\"_blank\"");
            }
            else
            {            
                sbLink.Append("\"");
            }

            if (tempTabIndex != -1)
            {
                sbLink.Append(" TabIndex=\"" + tempTabIndex.ToString() + "\" ");
                tempTabIndex += 1;
            }

            sbLink.Append(" class=\"actuator\" title=\"" + c.MetaTitle + "\">");                                
            sbLink.Append("<span>" + c.Name + "</span>");
            sbLink.Append("</a></li>");

            result = sbLink.ToString();
            return result;
        }


       
        public static void RenderSingleProduct(ref StringBuilder sb, 
                                                Catalog.Product p,
                                                bool isLastInRow, 
                                                bool isFirstInRow,
                                                System.Web.UI.Page page,
                                                Catalog.UserSpecificPrice price)
        {
             
        string destinationLink = Utilities.UrlRewriter.BuildUrlForProduct(p, page);

        string imageUrl 
            = MerchantTribe.Commerce.Storage.DiskStorage.ProductImageUrlSmall(
                    ((IMultiStorePage)page).MTApp, 
                    p.Bvin, 
                    p.ImageFileSmall, 
                    page.Request.IsSecureConnection);

        if (isLastInRow) 
        {
            sb.Append("<div class=\"record lastrecord\">");
        }
        else if (isFirstInRow)
        {
            sb.Append("<div class=\"record firstrecord\">");
        }
        else
        {
            sb.Append("<div class=\"record\">");
        }


        sb.Append("<div class=\"recordimage\">");
        sb.Append("<a href=\"" + destinationLink + "\">");
        sb.Append("<img src=\"" + imageUrl + "\" border=\"0\" alt=\"" + p.ImageFileSmallAlternateText + "\" /></a>");
        sb.Append("</div>");

        sb.Append("<div class=\"recordname\">");
        sb.Append("<a href=\"" + destinationLink + "\">" + p.ProductName + "</a>");
        sb.Append("</div>");
        sb.Append("<div class=\"recordsku\">");
        sb.Append("<a href=\"" + destinationLink + "\">" + p.Sku + "</a>");
        sb.Append("</div>");
        sb.Append("<div class=\"recordprice\">");
        sb.Append("<a href=\"" + destinationLink + "\">" + price.DisplayPrice(true) + "</a>");
        sb.Append("</div>");

        sb.Append("</div>");

        }


        public static string HeaderLinks(MerchantTribeApplication app, string currentUserId)
        {
            StringBuilder sb = new StringBuilder();

            string rootUrl = app.StoreUrl(false, true);
            string rootUrlSecure = app.StoreUrl(true, false);

            sb.Append("<ul>");

            sb.Append("<li><a class=\"myaccountlink\" href=\"" + rootUrlSecure + "account\"><span>");
            sb.Append("My Account");
            sb.Append("</span></a></li>");

            sb.Append("<li><a class=\"signinlink\"");
            
            if (currentUserId == string.Empty)
            {
                sb.Append(" href=\"" + rootUrlSecure + "SignIn\"><span>");
                sb.Append("Sign In");
            }
            else
            {
                string name = string.Empty;
                MerchantTribe.Commerce.Membership.CustomerAccount a = app.MembershipServices.Customers.Find(currentUserId);
                if (a != null)
                {
                    name = a.Email;
                }
                sb.Append(" href=\"" + rootUrlSecure + "SignOut\" title=\"" + System.Web.HttpUtility.HtmlEncode(name) + "\"><span>");
                sb.Append("Sign Out");                
            }
            sb.Append("</span></a></li>");

            //sb.Append("<li><a class=\"orderstatuslink\" href=\"" + rootUrlSecure + "OrderStatus\"><span>");
            //sb.Append("Order Status");
            //sb.Append("</span></a></li>");

            //sb.Append("<li><a class=\"emailsignuplink\" href=\"" + rootUrl + "EmailSignUp\"><span>");
            //sb.Append("Email Sign Up");
            //sb.Append("</span></a></li>");

            //sb.Append("<li><a class=\"giftcardslink\" href=\"" + rootUrl + "GiftCards\"><span>");
            //sb.Append("Gift Cards");
            //sb.Append("</span></a></li>");

            //sb.Append("<li><a class=\"contactlink\" href=\"" + rootUrl + "Contact\"><span>");
            //sb.Append("Contact Us");
            //sb.Append("</span></a></li>");

            sb.Append("<li><a class=\"contactlink\" href=\"" + rootUrl + "Checkout\"><span>");
            sb.Append("Checkout");
            sb.Append("</span></a></li>");

            sb.Append("</ul>");

            return sb.ToString();
        }

        public static string SearchForm(MerchantTribeApplication app)
        {
            StringBuilder sb = new StringBuilder();

            string rootUrl = app.StoreUrl(false, true);
            string buttonUrl = app.ThemeManager().ButtonUrl("Go", app.CurrentRequestContext.RoutingContext.HttpContext.Request.IsSecureConnection);
            
            sb.Append("<form class=\"searchform\" action=\"" + rootUrl + "search\" method=\"get\">");

            sb.Append("<input type=\"text\" name=\"q\" class=\"searchinput\" /> <input class=\"searchgo\" type=\"image\" src=\"" + buttonUrl + "\" alt=\"Search\" />");

            sb.Append("</form>");            

            return sb.ToString();
        }

        public static string ProductOptions(Catalog.OptionList options, Catalog.OptionSelectionList selections)
        {
            StringBuilder sb = new StringBuilder();

            foreach (Catalog.Option opt in options)
            {
                if (!opt.NameIsHidden)
                {
                    sb.Append("<label for=\"" + opt.Bvin.Replace("-", "") + "\">");
                    sb.Append(opt.Name);
                    sb.Append("</label>");
                }
                sb.Append("<span class=\"choice\">");
                sb.Append(opt.RenderWithSelection(selections));
                sb.Append("</span>");
            }

            return sb.ToString();
        }

        public static void ProductOptionsAsControls(Catalog.OptionList options, System.Web.UI.WebControls.PlaceHolder ph)
        {
            foreach (Catalog.Option opt in options)
            {
                if (!opt.NameIsHidden)
                {
                    System.Web.UI.LiteralControl lit = new System.Web.UI.LiteralControl("<label for=\"" 
                                                                    + opt.Bvin.Replace("-", "") 
                                                                    + "\">"
                                                                    + opt.Name 
                                                                    + "</label>");
                    lit.EnableViewState = false;
                    ph.Controls.Add(lit);
                }

                System.Web.UI.LiteralControl lit2 = new System.Web.UI.LiteralControl("<span class=\"choice\">");
                    lit2.EnableViewState = false;
                    ph.Controls.Add(lit2);

                opt.RenderAsControl(ph);
                
                System.Web.UI.LiteralControl lit3 = new System.Web.UI.LiteralControl("</span>");
                    lit3.EnableViewState = false;
                    ph.Controls.Add(lit3);
            }
        }

        public static string UserSpecificPriceForDisplay(Catalog.UserSpecificPrice price)
        {
            StringBuilder sb = new StringBuilder();

            if (price.ListPriceGreaterThanCurrentPrice)
            {
                sb.Append("<label>" + Content.SiteTerms.GetTerm(Content.SiteTermIds.ListPrice) + "</label>");
                sb.Append("<span class=\"choice\"><strike>");
                sb.Append(price.ListPrice.ToString("C"));
                sb.Append("</strike></span>");
            }

            sb.Append("<label>" + Content.SiteTerms.GetTerm(Content.SiteTermIds.SitePrice) + "</label>");
            sb.Append("<span class=\"choice\">");
            sb.Append(price.DisplayPrice());
            sb.Append("</span>");

            if ((price.BasePrice < price.ListPrice) && (price.OverrideText.Trim().Length < 1))
            {
                sb.Append("<label>" + Content.SiteTerms.GetTerm(Content.SiteTermIds.YouSave) + "</label>");
                sb.Append("<span class=\"choice\">");
                sb.Append(price.Savings.ToString("c")
                            + " - "
                            + price.SavingsPercent
                            + System.Threading.Thread.CurrentThread.CurrentUICulture.NumberFormat.PercentSymbol);
                sb.Append("</span>");
            }

            sb.Append("<div class=\"clear\"></div>");

            return sb.ToString();
        }

        //public static string AddressToForm(Contacts.Address a, string prefix, int tabIndexStart)
        //{
        //    StringBuilder sb = new StringBuilder();

        //    sb.Append("<table>");

        //    // Country
        //    sb.Append("<tr><td class=\"formfield\">Country:</td><td class=\"forminput\">");
        //    sb.Append("<select id=\"" + prefix + "countryname\" name=\"" + prefix + "countryname\"");
        //    sb.Append(" tabindex=\"" + (tabIndexStart + 1).ToString() + "\" >");
        //    foreach (MerchantTribe.Web.Geography.Country c in RequestContext.GetCurrentRequestContext().CurrentStore.Settings.FindActiveCountries())
        //    {
        //        sb.Append("<option value=\"" + c.Bvin + "\"");
        //        if (c.Bvin == a.CountyBvin)
        //        {
        //            sb.Append(" selected ");
        //        }
        //        sb.Append(">" + c.DisplayName + "</option>");
        //    }
        //    sb.Append("</select>");
        //    sb.Append("</td></tr>");

        //    // First Name
        //    sb.Append("<tr><td class=\"formfield\">First Name:</td><td class=\"forminput\">");
        //    sb.Append("<input type=\"text\" id=\"" + prefix + "firstname\" name=\"" + prefix + "firstname\"");
        //    sb.Append(" value=\"" + a.FirstName + "\" tabindex=\"" + (tabIndexStart + 2).ToString() + "\" />");            
        //    sb.Append("</td></tr>");

        //    sb.Append("</table>");
        //    return sb.ToString();
        //}

        public static string ShippingRatesToRadioButtons(SortableCollection<Shipping.ShippingRateDisplay> rates, int tabIndex, string selectedMethodUniqueKey)
        {
            StringBuilder sb = new StringBuilder();

            if (rates == null)
            {
                return string.Empty;
            }            

            // Tab Index Settings
            int tabOffSet = 0;
            if (tabIndex > 0)
            {
                tabOffSet = tabIndex;
            }

            foreach (Shipping.ShippingRateDisplay r in rates)
            {
                if (r.Rate >= 0)
                {
                    sb.Append("<input type=\"radio\" name=\"shippingrate\" value=\"" + r.UniqueKey + "\"");
                    sb.Append(" class=\"shippingratequote\" ");
                    if (r.UniqueKey == selectedMethodUniqueKey)
                    {
                        sb.Append(" checked=\"checked\" ");
                    }
                    sb.Append("/>" + r.RateAndNameForDisplay + "<br />");
                }
            }

            return sb.ToString();
        }
               
    }

}
