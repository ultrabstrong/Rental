using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Rental.Domain.Core;
using Rental.Domain.Email.Services;
using Rental.WebApp.Extensions;
using Rental.WebApp.Models;
using Rental.WebApp.Models.Application;
using Rental.WebApp.Models.Site;
using Serilog;

namespace Rental.WebApp.Controllers;

public class ApplicationController : ControllerWithPdfRenderingBase
{
    public static readonly string Name = nameof(ApplicationController).Replace(nameof(Controller), "");
    private readonly IEmailService _emailService;
    private readonly IOptionsSnapshot<SiteOptions> _siteDetails;

    public ApplicationController(
        IEmailService emailService,
        IOptionsSnapshot<SiteOptions> siteDetails)
    {
        _emailService = emailService;
        _siteDetails = siteDetails;
    }

    [HttpGet, Route("DownloadApplication")]
    public ActionResult DownloadApplication() => View();

    [HttpGet, Route("Apply")]
    public ActionResult Apply() => View("ApplyLoading");

    [HttpGet, Route("ApplyForm")]
    public ActionResult ApplyForm()
    {
        this.AddUSStatesToViewBag();
#if DEBUG
        //return PartialView("Apply", new Application());
        return PartialView("Apply", Application.TestApplication);
#else
        return PartialView("Apply", new Application());
#endif
    }

    [HttpPost, Route("SubmitApplication")]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> SubmitApplication(Application application, CancellationToken cancellationToken)
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
                ViewBag.Errors = true;
                Response.StatusCode = StatusCodes.Status400BadRequest;
                return PartialView("Apply", application);
            }

            Log.Logger.Debug("Creating PDF view HTML");
            var html = await RenderRazorViewToStringAsync("ApplicationPdf", application);

            Log.Logger.Debug("Converting HTML to PDF");
            var pdf = HtmlToPdfConverter.GetPdfBytes(html,
                _siteDetails.Value.CompanyName,
                $"{application.PersonalInfo.FirstName} {application.PersonalInfo.LastName} Application",
                $"Application for {application.RentalAddress} from {application.PersonalInfo.FirstName} {application.PersonalInfo.LastName}; Co-Applicants : {application.OtherApplicants}");

            Log.Logger.Debug("Sending email");
            using (MemoryStream pdfStream = new(pdf))
                await _emailService.SendEmailAsync(application, pdfStream, cancellationToken);
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