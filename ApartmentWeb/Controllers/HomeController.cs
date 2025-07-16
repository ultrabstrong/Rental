using System.Web.Mvc;

namespace ApartmentWeb.Controllers
{
    public class HomeController : Controller
    {
        public static readonly string Name = nameof(HomeController).Replace(nameof(Controller), "");

        public ActionResult Index() => View();

        [HttpGet, Route("TenantInfo")]
        public ActionResult TenantInfo() => View();

        [HttpGet, Route("ContactUs")]
        public ActionResult ContactUs() => View();

        [HttpGet, Route("Privacy")]
        public ActionResult Privacy() => View();

        [HttpGet, Route("Terms")]
        public ActionResult Terms() => View();

    }
}