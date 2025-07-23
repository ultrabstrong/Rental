using ApartmentWeb.Extensions;
using ApartmentWeb.Models;
using ApartmentWeb.Models.Application;
using Domain.Core;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace ApartmentWeb.Controllers
{
    public class ApplicationController : ControllerWithPdfRenderingBase
    {
        public static readonly string Name = nameof(ApplicationController).Replace(nameof(Controller), "");

        [HttpGet, Route("DownloadApplication")]
        public ActionResult DownloadApplication() => View();

        [HttpGet, Route("Apply")]
        public ActionResult Apply()
        {
            // This action now routes to the loading page.
            // The actual form will be loaded via AJAX by ApplyLoading.cshtml
            return View("ApplyLoading");
        }

        [HttpGet, Route("ApplyForm")]
        public ActionResult ApplyForm()
        {
            this.AddUSStatesToViewBag();
#if DEBUG
            return PartialView("Apply", Shared.TestApplication);
            //return PartialView("Apply", new Application());
#else
            return PartialView("Apply", new Application()); // Use PartialView for AJAX
#endif
        }

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
                        .Select(m => $"{m.Key}: {string.Join(", ", m.Value.Errors.Select(e => e.ErrorMessage))}")
                        .ToList();

                    Log.Logger.Information("Application validation errors: {@Errors}", errors);
                    this.AddUSStatesToViewBag();
                    return Json(new SubmitResponse { isSuccess = false, hasValidationErrors = true });
                }

                Log.Logger.Debug("Creating PDF view HTML");

                var html = RenderRazorViewToString("ApplicationPdf", application);

                Log.Logger.Debug("Converting HTML to PDF");
                var pdf = HtmlToPdfConverter.GetPdfBytes(html,
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

            return Json(new SubmitResponse { isSuccess = true, redirectUrl = Url.Action(nameof(HomeController.Index), HomeController.Name) });
        }
    }
}