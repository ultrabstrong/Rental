using ApartmentWeb.Extensions;
using System.IO;
using System.Web.Mvc;

namespace ApartmentWeb.Controllers
{
    public abstract class ControllerWithPdfSubmissionBase : Controller
    {
        protected string RenderRazorViewToString(string viewName, object model)
        {
            ViewData.Model = model;
            using (var sw = new StringWriter())
            {
                if (viewName == nameof(HomeController.Apply))
                {
                    this.AddUSStatesToViewBag();
                }
                ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                ViewContext viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(ControllerContext, viewResult.View);
                return sw.GetStringBuilder().ToString();
            }
        }
    }
}