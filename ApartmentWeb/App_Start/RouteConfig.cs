using System.Web.Mvc;
using System.Web.Routing;

namespace ApartmentWeb
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapMvcAttributeRoutes();

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = Controllers.HomeController.Name, action = nameof(Controllers.HomeController.Index), id = UrlParameter.Optional }
            );
        }
    }
}
