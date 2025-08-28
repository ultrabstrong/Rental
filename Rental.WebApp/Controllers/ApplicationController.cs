using Microsoft.AspNetCore.Mvc;
using Rental.Domain.Core;
using Rental.WebApp.Extensions;
using Rental.WebApp.Models;
using Rental.WebApp.Models.Application;
using Serilog;

namespace Rental.WebApp.Controllers;

public class ApplicationController : ControllerWithPdfRenderingBase
{
    public static readonly string Name = nameof(ApplicationController).Replace(nameof(Controller), "");

    [HttpGet, Route("DownloadApplication")]
    public ActionResult DownloadApplication() => View();

    [HttpGet, Route("Apply")]
    public ActionResult Apply()
    {
        // The actual form will be loaded via AJAX by ApplyLoading.cshtml
        return View("ApplyLoading");
    }

    [HttpGet, Route("ApplyForm")]
    public ActionResult ApplyForm()
    {
        this.AddUSStatesToViewBag();
#if DEBUG
        //return PartialView("Apply", Shared.TestApplication);
        return PartialView("Apply", new Application());
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
                    .Where(m => m.Value != null && m.Value.Errors.Any())
                    .Select(m => $"{m.Key}: {string.Join(", ", m.Value!.Errors.Select(e => e.ErrorMessage))}")
                    .ToList();

                Log.Logger.Information("Application validation errors: {@Errors}", errors);
                this.AddUSStatesToViewBag();
                ViewBag.Errors = true; // trigger validation summary display in partial

                // Return the form partial with validation messages and a 400 status so client JS can swap it in
                Response.StatusCode = StatusCodes.Status400BadRequest;
                return PartialView("Apply", application);
            }

            Log.Logger.Debug("Creating PDF view HTML");

            var html = RenderRazorViewToString("ApplicationPdf", application);

            Log.Logger.Debug("Converting HTML to PDF");
            var pdf = HtmlToPdfConverter.GetPdfBytes(html,
                Shared.Configuration.CompanyName,
                $"{application.PersonalInfo.FirstName} {application.PersonalInfo.LastName} Application",
                $"Application for {application.RentalAddress} from {application.PersonalInfo.FirstName} {application.PersonalInfo.LastName}; Co-Applicants : {application.OtherApplicants}");

            Log.Logger.Debug("Sending email");
            using (MemoryStream pdfStream = new(pdf))
            using (var emailService = new EmailService(Shared.Configuration.MailSettings))
            {
                emailService.SendEmail(application, pdfStream);
            }
            Log.Logger.Debug("Finished sending email");
        }
        catch (Exception ex)
        {
            Log.Logger.Error(ex, "Failed to submit application");
            return Json(new SubmitResponse() { IsSuccess = false });
        }

        return Json(new SubmitResponse
        {
            IsSuccess = true,
            RedirectUrl = Url.Action(nameof(HomeController.Index), HomeController.Name)!
        });
    }
}