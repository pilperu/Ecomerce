using System;
using System.Text;
using System.Web.Mvc;
using MerchantTribe.Commerce;

namespace MerchantTribeStore.Helpers
{
    public static class Html
    {
        public static string Encode(string input)
        {
            return System.Web.HttpUtility.HtmlEncode(input);
        }

        public static string AddErrorClass(string controlName, System.Web.Mvc.ViewDataDictionary viewdata)
        {
            ModelState modelState;

            if (viewdata.ModelState.TryGetValue(controlName, out modelState))
            {
                if (modelState.Errors.Count > 0)
                {
                    return " input-validation-error";
                }
            }

            return "";
        }

        public static string CreateErrorClass(string controlName, System.Web.Mvc.ViewDataDictionary viewdata)
        {
            ModelState modelState;

            if (viewdata.ModelState.TryGetValue(controlName, out modelState))
            {
                if (modelState.Errors.Count > 0)
                {
                    return "class=\"input-validation-error\"";
                }
            }
            return "class=\"\"";
        }

        public static string RenderSiteMap(MerchantTribe.Web.SiteMapNode nodes)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("<ul>" + System.Environment.NewLine);

            RenderNode(sb, nodes);

            sb.Append("</ul>" + System.Environment.NewLine);

            return sb.ToString();
        }

        private static void RenderNode(StringBuilder sb, MerchantTribe.Web.SiteMapNode node)
        {
            sb.Append("<li>");


            if (node.Url.Trim().Length > 0)
            {
                sb.Append("<a href=\"" + node.Url + "\">" + node.DisplayName + "</a>");
            }
            else
            {
                sb.Append("<strong>" + node.DisplayName + "</strong>");
            }

            if (node.Children.Count > 0)
            {
                sb.Append(System.Environment.NewLine);
                sb.Append("<ul>" + System.Environment.NewLine);

                foreach (MerchantTribe.Web.SiteMapNode child in node.Children)
                {
                    RenderNode(sb, child);
                }

                sb.Append("</ul>" + System.Environment.NewLine);
            }
            sb.Append("</li>" + System.Environment.NewLine);
        }

        public static string JQueryIncludes(string baseScriptFolder, bool IsSecure)
        {
            StringBuilder sb = new StringBuilder();
            if (baseScriptFolder.EndsWith("/") == false)
            {
                baseScriptFolder += "/";
            }


            bool UseCDN = false;

            if (UseCDN)
            {
                // CDN JQuery
                if (IsSecure)
                {
                    sb.Append("<script src='https://ajax.microsoft.com/ajax/jQuery/jquery-1.5.1.min.js' type=\"text/javascript\"></script>");
                }
                else
                {
                    sb.Append("<script src='http://ajax.microsoft.com/ajax/jQuery/jquery-1.5.1.min.js' type=\"text/javascript\"></script>");
                }
            }
            else
            {
                // Local JQuery
                sb.Append("<script src='" + baseScriptFolder + "jquery-1.5.1.min.js' type=\"text/javascript\"></script>");
            }
            sb.Append(System.Environment.NewLine);

            
            
            sb.Append("<script src='" + baseScriptFolder + "jquery-ui-1.8.7.custom/js/jquery-ui-1.8.7.custom.min.js' type=\"text/javascript\"></script>");
            sb.Append("<script src='" + baseScriptFolder + "jquery.form.js' type=\"text/javascript\"></script>");
            sb.Append(System.Environment.NewLine);



            return sb.ToString();
        }

        public static string RenderSuperMenu(MerchantTribeApplication app)
        {
            StringBuilder sb = new StringBuilder();
            AdminTabType tab = AdminTabType.None;

            string root = app.StoreUrl(true, false);
            root += "super/";

            sb.Append("<ul>");
            sb.Append(AddMainLink("Home", "", tab, AdminTabType.Dashboard, root));

            // catalog menu
            sb.Append(OpenMenu("Stores", tab, AdminTabType.Dashboard));
            sb.Append(AddMenuItem("Find Stores", "stores/", root));
            sb.Append(AddMenuItem("New Store Report", "stores/NewStoreReport", root));
            sb.Append(CloseMenu());

            sb.Append(OpenMenu("Search", tab, AdminTabType.Dashboard));
            sb.Append(AddMenuItem("Search", "search", root));
            sb.Append(AddMenuItem("Index Controls", "rebuildsearch", root));
            sb.Append(CloseMenu());

            sb.Append("</ul>");

            return sb.ToString();
        }

        public static string RenderMenu(AdminTabType selected, MerchantTribeApplication app)
        {
            StringBuilder sb = new StringBuilder();

            string root = app.StoreUrl(true, false);
            root += "bvadmin/";

            sb.Append("<ul>");
            sb.Append(AddMainLink("Dashboard", "default.aspx", selected, AdminTabType.Dashboard, root));

            // catalog menu
            sb.Append(OpenMenu("Catalog", selected, AdminTabType.Catalog));
            sb.Append(AddMenuItem("Products", "catalog/default.aspx", root));
            sb.Append(AddMenuItem("Categories", "catalog/Categories.aspx", root));
            //sb.Append(AddMenuItem("Kits", "catalog/kits.aspx", root))
            sb.Append(AddMenuItem("Shared Choices", "catalog/ProductSharedChoices.aspx", root));
            sb.Append(AddMenuItem("Product Types", "catalog/ProductTypes.aspx", root));
            sb.Append(AddMenuItem("Type Properties", "catalog/ProductTypeProperties.aspx", root));
            sb.Append(AddMenuItem("Reviews", "catalog/ReviewsToModerate.aspx", root));
            //sb.Append(AddMenuItem("Inventory", "catalog/Inventory.aspx", root))
            //sb.Append(AddMenuItem("Bulk Inventory", "catalog/InventoryBatchEdit.aspx", root))
            //sb.Append(AddMenuItem("Gift Certificates", "catalog/GiftCertificates.aspx", root))
            sb.Append(AddMenuItem("File Vault", "catalog/FileVault.aspx", root));
            sb.Append(CloseMenu());

            // Marketing Menu
            sb.Append(OpenMenu("Marketing", selected, AdminTabType.Marketing));
            sb.Append(AddMenuItem("Promotions", "marketing/promotions.aspx", root));            
            sb.Append(AddMenuItem(app.CurrentStore.Settings.RewardsPointsName, "marketing/rewardspoints.aspx", root));
            sb.Append(CloseMenu());

            // People Menu
            sb.Append(OpenMenu("People", selected, AdminTabType.People));
            sb.Append(AddMenuItem("Customers", "people/default.aspx", root));
            sb.Append(AddMenuItem("Price Groups", "people/PriceGroups.aspx", root));
            //sb.Append(AddMenuItem("User Signup", "people/UserSignupConfig.aspx", root))
            sb.Append(AddMenuItem("Affiliates", "people/affiliates.aspx", root));
            //sb.Append(AddMenuItem("Affiliate Signup", "people/AffiliateSignupConfig.aspx", root))
            sb.Append(AddMenuItem("Manufacturers", "people/manufacturers.aspx", root));
            sb.Append(AddMenuItem("Vendors", "people/vendors.aspx", root));
            sb.Append(AddMenuItem("Mailing Lists", "people/mailinglists.aspx", root));
            //sb.Append(AddMenuItem("Contact Us Setup", "people/ContactUsConfig.aspx", root))
            //sb.Append(AddMenuItem("Admin Permissions", "people/roles.aspx", root))
            sb.Append(AddMenuItem("Administrators", "people/Administrators.aspx", root));
            sb.Append(CloseMenu());

            // Pages Menu
            sb.Append(OpenMenu("Pages", selected, AdminTabType.Content));
            sb.Append(AddMenuItem("Pages", "catalog/categories.aspx", root));
            sb.Append(AddMenuItem("File Manager", "content/filemanager", root));
            sb.Append(AddMenuItem("URL Mapper", "content/customurl.aspx", root));
            sb.Append(AddMenuItem("--------------", "catalog/categories.aspx", root));
            sb.Append(AddMenuItem("Site Meta Tags", "content/metatags.aspx", root));            
            sb.Append(AddMenuItem("Policies", "content/policies.aspx", root));
            sb.Append(AddMenuItem("Email Templates", "content/EmailTemplates.aspx", root));
            sb.Append(AddMenuItem("--------------", "catalog/categories.aspx", root));
            sb.Append(AddMenuItem("Page Images", "content/StoreAssets.aspx", root));
            sb.Append(AddMenuItem("Content Columns", "content/columns.aspx", root));
            sb.Append(AddMenuItem("Home Page", "content/default.aspx", root));

            sb.Append(CloseMenu());

            // Orders Menu

            sb.Append(OpenMenu("Orders", selected, AdminTabType.Orders));

            sb.Append(AddMenuItem("Order Manager", "orders/default.aspx?p=1&mode=0", root));
            sb.Append(AddMenuItem("--------------", "orders/default.aspx?p=1&mode=0", root));
            sb.Append(AddMenuItem(" + Add Order", "orders/createorder.aspx", root));
            sb.Append(AddMenuItem("--------------", "orders/default.aspx?p=1&mode=0", root));
            sb.Append(AddMenuItem("  New", "orders/default.aspx?p=1&mode=1", root));
            sb.Append(AddMenuItem("  On Hold", "orders/default.aspx?p=1&mode=5", root));
            sb.Append(AddMenuItem("  Ready for Payment", "orders/default.aspx?p=1&mode=2", root));
            sb.Append(AddMenuItem("  Ready for Shipping", "orders/default.aspx?p=1&mode=3", root));
            sb.Append(AddMenuItem("  Completed", "orders/default.aspx?p=1&mode=4", root));
            sb.Append(AddMenuItem("  Cancelled", "orders/default.aspx?p=1&mode=6", root));
            sb.Append(AddMenuItem("  All Orders", "orders/default.aspx?p=1&mode=0", root));
            sb.Append(AddMenuItem("--------------", "orders/default.aspx?p=1&mode=0", root));
            
            //sb.Append(AddMenuItem("UPS Labels", "orders/upsonlinetools.aspx", root));
            //sb.Append(AddMenuItem("Returns", "orders/rma.aspx", root));
            sb.Append(CloseMenu());

            // Reports Menu
            sb.Append(OpenMenu("Reports", selected, AdminTabType.Reports));

            sb.Append(AddMenuItemWithoutRoot("Transactions by Day", app.StoreUrl(true, false) + "bvmodules/reports/Daily Sales/view.aspx"));
            sb.Append(AddMenuItemWithoutRoot("Orders | by Date", app.StoreUrl(true, false) + "bvmodules/reports/Sales By Date/view.aspx"));
            sb.Append(AddMenuItemWithoutRoot("Orders | by Affiliate", app.StoreUrl(true, false) + "bvmodules/reports/Affiliate Sales/view.aspx"));
            //sb.Append(AddMenuItem("Sales | by Coupon", Page.ResolveUrl("~/bvmodules/reports/Sales By Coupon/view.aspx", root)));
            //sb.Append(AddMenuItemWithoutRoot("Customer List", Page.ResolveUrl("~/bvmodules/reports/Customer List/view.aspx")));
            //sb.Append(AddMenuItemWithoutRoot("Keyword Searches", Page.ResolveUrl("~/bvmodules/reports/Keyword Searches/view.aspx")));
            //sb.Append(AddMenuItemWithoutRoot("Shopping Carts", Page.ResolveUrl("~/bvmodules/reports/Shopping Carts/view.aspx")));
            //sb.Append(AddMenuItemWithoutRoot("Top Customers", Page.ResolveUrl("~/bvmodules/reports/Top Customers/view.aspx")));
            sb.Append(AddMenuItemWithoutRoot("Top Products", app.StoreUrl(true, false) + "bvmodules/reports/Top Products/view.aspx"));

            //StringCollection sc = ModuleController.FindReports();
            //string reportRoot = this.MyPage.CurrentStore.RootUrlSecure() + "bvmodules/reports/";
            //foreach (string reportName in sc) {
            //    sb.Append(AddMenuItemWithoutRoot(reportName, reportRoot + reportName + "/view.aspx"));
            //}
            sb.Append(CloseMenu());

            // Options Menu
            sb.Append(OpenMenu("Options", selected, AdminTabType.Configuration));
            sb.Append(AddMenuItem("Store Name &amp; Logo", "configuration/General.aspx", root));
            sb.Append(AddMenuItem("Store's Address", "configuration/StoreInfo.aspx", root));
            sb.Append(AddMenuItem("Analytics", "configuration/Analytics.aspx", root));
            sb.Append(AddMenuItem("Api", "configuration/Api.aspx", root));
            sb.Append(AddMenuItem("Countries", "configuration/countries.aspx", root));
            sb.Append(AddMenuItem("Email Addresses", "configuration/Email.aspx", root));
            sb.Append(AddMenuItem("Email Server", "configuration/MailServer.aspx", root));
            sb.Append(AddMenuItem("Fraud Screening", "configuration/Fraud.aspx", root));
            sb.Append(AddMenuItem("Geo-Location", "configuration/GeoLocation.aspx", root));
            sb.Append(AddMenuItem("Orders", "configuration/Orders.aspx", root));
            sb.Append(AddMenuItem("Payment Methods", "configuration/Payment.aspx", root));
            sb.Append(AddMenuItem("Product Reviews", "configuration/ProductReviews.aspx", root));
            sb.Append(AddMenuItem("Scheduled Tasks", "configuration/ScheduledTasks.aspx", root));
            sb.Append(AddMenuItem("Shipping: Methods", "configuration/Shipping.aspx", root));
            sb.Append(AddMenuItem("Shipping: Zones", "configuration/Shipping_Zones.aspx", root));
            sb.Append(AddMenuItem("Shipping: Handling", "configuration/ShippingHandling.aspx", root));
            sb.Append(AddMenuItem("Social Media", "configuration/SocialMedia.aspx", root));
            sb.Append(AddMenuItem("Tax Schedules", "configuration/TaxClasses.aspx", root));
            sb.Append(AddMenuItem("Themes", "configuration/themes.aspx", root));

            //sb.Append(AddMenuItem("About", "configuration/versioninfo.aspx"))
            sb.Append(CloseMenu());

            // Plug-Ins Menu
            //sb.Append(OpenMenu("Plug-Ins", selected, AdminTabType.Plugins))
            //Dim plugins As StringCollection = MerchantTribe.Commerce.Content.ModuleController.FindAdminPlugins()
            //For Each item As String In plugins
            //sb.Append(AddMenuItem(item, "plugins/" & item & "/default.aspx"))            
            //Next
            //sb.Append(CloseMenu())


            sb.Append("</ul>");

            return sb.ToString();
        }

        private static string AddMainLink(string name, string destination, AdminTabType currentTab, AdminTabType tab, string root)
        {
            bool isSelected = false;
            if ((currentTab == tab))
            {
                isSelected = true;
            }

            string result = "<li";
            if (isSelected)
            {
                result += " class=\"current\"";
            }
            result += "><a href=\"" + root + destination + "\" title=\"" + name + "\">" + name + "</a></li>";
            return result;
        }

        private static string OpenMenu(string name, AdminTabType currentTab, AdminTabType tab)
        {
            bool isSelected = false;
            if ((currentTab == tab))
            {
                isSelected = true;
            }

            string result = "<li";
            if (isSelected)
            {
                result += " class=\"current\"";
            }
            result += "><a href=\"#\" onclick=\"return false;\" title=\"" + name + "\">" + name + "</a><ul>";
            return result;
        }

        private static string CloseMenu()
        {
            return "</ul></li>";
        }

        private static string AddMenuItem(string name, string destination, string root)
        {
            string result = "<li><a href=\"" + root + destination + "\" title=\"" + name + "\">" + name + "</a></li>";
            return result;
        }

        private static string AddMenuItemWithoutRoot(string name, string destination)
        {
            string result = "<li><a href=\"" + destination + "\" title=\"" + name + "\">" + name + "</a></li>";
            return result;
        }

        public static string AdminFooter()
        {
            return "<div id=\"footer\"><div id=\"copyright\">&copy; Copyright 2002-" + DateTime.Now.Year.ToString() + " BV Software LLC, All Rights Reserved</div></div>";
        }
               
    }
}