using System.Web.Mvc;

namespace MerchantTribeStore.Areas.ContentBlocks
{
    public class ContentBlocksAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "ContentBlocks";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "ContentBlocks_default",
                "ContentBlocks/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
