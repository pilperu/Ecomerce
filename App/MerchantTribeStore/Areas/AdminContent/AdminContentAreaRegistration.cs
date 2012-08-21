using System.Web.Mvc;

namespace MerchantTribeStore.Areas.AdminContent
{
    public class AdminContentAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "AdminContent";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute("FileBrowser", "bvadmin/filebrowser/{action}/{id}",
                            new { controller = "FileBrowser", action = "Index", id= UrlParameter.Optional }
                            );
            context.MapRoute("ContentFileManager", "bvadmin/content/filemanager/{action}/{id}",
                           new { controller = "FileManager", action = "Index", id = UrlParameter.Optional }
                           );

            //context.MapRoute(
            //    "AdminContent_default",
            //    "AdminContent/{controller}/{action}/{id}",
            //    new { action = "Index", id = UrlParameter.Optional }
            //);
        }
    }
}
