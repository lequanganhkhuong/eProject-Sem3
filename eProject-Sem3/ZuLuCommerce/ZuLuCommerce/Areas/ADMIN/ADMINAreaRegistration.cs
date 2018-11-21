using System.Web.Mvc;

namespace ZuLuCommerce.Areas.ADMIN
{
    public class ADMINAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "ADMIN";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "ADMIN_default",
                "ADMIN/{controller}/{action}/{id}",
                new { controller = "Products", action = "Index", id = UrlParameter.Optional },
                new string[] { "ZuluCommerce.Areas.ADMIN.Controllers" }
            );
        }
    }
}