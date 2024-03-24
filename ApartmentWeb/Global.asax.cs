using Serilog;
using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace ApartmentWeb
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            GlobalFilters.Filters.Add(new RequireHttpsAttribute());

            Log.Logger = new LoggerConfiguration()
                .WriteTo.File(
                    Server.MapPath("~/logs/log-.txt"),
                    rollingInterval: RollingInterval.Day,
                    retainedFileTimeLimit: TimeSpan.FromDays(90))
                .CreateLogger();
        }
    }
}
