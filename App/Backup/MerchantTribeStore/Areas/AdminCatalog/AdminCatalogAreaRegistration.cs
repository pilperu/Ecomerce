using System.Web.Mvc;

namespace MerchantTribeStore.Areas.AdminCatalog
{
    public class AdminCatalogAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "AdminCatalog";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute("CategoryCustomEdit", "bvadmin/catalog/categories/custom/{action}/{id}",
                            new { controller = "CategoriesCustom", action = "Edit", id = UrlParameter.Optional }
                            );
            
            //context.MapRoute(
            //    "AdminCatalog_default",
            //    "bvadmin/catalog/{controller}/{action}/{id}",
            //    new { action = "Index", id = UrlParameter.Optional }
            //);
        }
    }
}
