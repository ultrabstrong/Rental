using ApartmentWeb.Helpers;
using ApartmentWeb.Models;
using ApartmentWeb.Models.Application;
using ApartmentWeb.Models.Maintenance;
using Domain.Core;
using Serilog;
using System;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace ApartmentWeb.Controllers
{
    public class HomeController : Controller
    {
        public static readonly string Name = nameof(HomeController).Replace(nameof(Controller), "");

        public ActionResult Index() => View();

        [HttpGet, Route("DownloadApplication")]
        public ActionResult DownloadApplication() => View();

        [HttpGet, Route("TenantInfo")]
        public ActionResult TenantInfo() => View();

        [HttpGet, Route("ContactUs")]
        public ActionResult ContactUs() => View();

        [HttpGet, Route("Apply")]
        public ActionResult Apply()
        {
            this.AddUSStatesToViewBag();
#if DEBUG
            return View(Shared.TestApplication); // Uncomment to have prefilled form
            //return View(new Application());
#else
            return View(new Application());
#endif
        }

        [HttpGet, Route("MaintenanceRequest")]
        public ActionResult MaintenanceRequest() => View(new MaintenanceRequest());

        [HttpGet, Route("Privacy")]
        public ActionResult Privacy() => View();

        [HttpGet, Route("Terms")]
        public ActionResult Terms() => View();

        [HttpPost, Route("SubmitApplication")]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitApplication(Application application)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState
                        .Where(m => m.Value.Errors.Any())
                        .Select(m => $"{m.Key} {string.Join(",", m.Value.Errors.Select(e => e.ErrorMessage))}");
                    Log.Logger.Information("Application validation errors: {@Errors}", errors);
                    this.AddUSStatesToViewBag();
                    return Json(new SubmitResponse { isSuccess = false, hasValidationErrors = true });
                }

                Log.Logger.Debug("Creating PDF view HTML");
                // Set ViewBag.IsPdf to true for PDF rendering
                ViewBag.IsPdf = true;
                var html = RenderRazorViewToString(nameof(Apply), application);

                Log.Logger.Debug("Converting HTML to PDF");
                var pdf = HtmlConverter.ToPdf(html,
                    Shared.Configuration.CompanyName,
                    $"{application.PersonalInfo.FirstName} {application.PersonalInfo.LastName} Application",
                    $"Application for {application.RentalAddress} from {application.PersonalInfo.FirstName} {application.PersonalInfo.LastName}; Co-Applicants : {application.OtherApplicants}");

                Log.Logger.Debug("Sending email");
                using (MemoryStream pdfStream = new MemoryStream(pdf))
                using (var emailService = new EmailService(Shared.Configuration.MailSettings))
                {
                    emailService.SendEmail(application, pdfStream);
                }
                Log.Logger.Debug("Finished sending email");
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex, "Failed to submit application");
                return Json(new SubmitResponse() { isSuccess = false });
            }

            return Json(new SubmitResponse { isSuccess = true, redirectUrl = Url.Action(nameof(Index), Name) });
        }

        [HttpPost, Route("SubmitMaintenanceRequest")]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitMaintenanceRequest(MaintenanceRequest maintenanceRequest)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState
                        .Where(m => m.Value.Errors.Any())
                        .Select(m => $"{m.Key} {string.Join(",", m.Value.Errors.Select(e => e.ErrorMessage))}");
                    Log.Logger.Information("Maintenance request validation errors: {@Errors}", errors);
                    return Json(new SubmitResponse { isSuccess = false, hasValidationErrors = true });
                }

                Log.Logger.Debug("Creating view HTML");
                var html = RenderRazorViewToString(nameof(this.MaintenanceRequest), maintenanceRequest);

                Log.Logger.Debug("Converting HTML to PDF");
                var pdf = HtmlConverter.ToPdf(html,
                    Shared.Configuration.CompanyName,
                    $"{maintenanceRequest.FirstName} {maintenanceRequest.LastName} Maintenance Request",
                    $"Maintenance request for {maintenanceRequest.RentalAddress} from {maintenanceRequest.FirstName} {maintenanceRequest.LastName}");

                Log.Logger.Debug("Sending maintenance email");
                using (var pdfStream = new MemoryStream(pdf))
                using (var emailService = new EmailService(Shared.Configuration.MailSettings))
                {
                    emailService.SendEmail(maintenanceRequest, pdfStream);
                }
                Log.Logger.Debug("Finished sending maintenance email");
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex, "Failed to submit maintenance request");
                return Json(new SubmitResponse() { isSuccess = false });
            }

            return Json(new SubmitResponse { isSuccess = true, redirectUrl = Url.Action(nameof(Index), Name) });
        }

        private string RenderRazorViewToString(string viewName, object model)
        {
            ViewData.Model = model;
            using (var sw = new StringWriter())
            {
                if (viewName == nameof(Apply))
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

        public ActionResult ReloadConfig()
        {
            Shared.LoadConfiguration();
            return Content("Configuration has been reloaded");
        }
    }
}