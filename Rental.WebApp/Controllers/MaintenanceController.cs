using Microsoft.AspNetCore.Mvc;
using Rental.Domain.Core;
using Rental.WebApp.Models;
using Rental.WebApp.Models.Maintenance;
using Serilog;
using Microsoft.Extensions.Options;
using Rental.WebApp.Models.Site;

namespace Rental.WebApp.Controllers;

public class MaintenanceController : ControllerWithPdfRenderingBase
{
    public static readonly string Name = nameof(MaintenanceController).Replace(nameof(Controller), "");
    private readonly IOptionsSnapshot<SiteDetails> _siteDetails;

    public MaintenanceController(IOptionsSnapshot<SiteDetails> siteDetails)
    {
        _siteDetails = siteDetails;
    }

    [HttpGet, Route("MaintenanceRequest")]
    public ActionResult MaintenanceRequest() => View(new MaintenanceRequest());


    [HttpPost, Route("SubmitMaintenanceRequest")]
    [ValidateAntiForgeryToken]
    public ActionResult SubmitMaintenanceRequest(MaintenanceRequest maintenanceRequest)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .Where(m => m.Value != null && m.Value.Errors.Any())
                    .Select(m => $"{m.Key} {string.Join(",", m.Value!.Errors.Select(e => e.ErrorMessage))}");
                Log.Logger.Information("Maintenance request validation errors: {@Errors}", errors);
                return Json(new SubmitResponse { IsSuccess = false, HasValidationErrors = true });
            }

            Log.Logger.Debug("Creating view HTML");
            var html = RenderRazorViewToString("MaintenanceRequestPdf", maintenanceRequest);

            Log.Logger.Debug("Converting HTML to PDF");
            var pdf = HtmlToPdfConverter.GetPdfBytes(html,
                _siteDetails.Value.CompanyName,
                $"{maintenanceRequest.FirstName} {maintenanceRequest.LastName} Maintenance Request",
                $"Maintenance request for {maintenanceRequest.RentalAddress} from {maintenanceRequest.FirstName} {maintenanceRequest.LastName}");

            Log.Logger.Debug("Sending maintenance email");
            using (var pdfStream = new MemoryStream(pdf))
            using (var emailService = new EmailService(_siteDetails.Value.MailSettings))
            {
                emailService.SendEmail(maintenanceRequest, pdfStream);
            }
            Log.Logger.Debug("Finished sending maintenance email");
        }
        catch (Exception ex)
        {
            Log.Logger.Error(ex, "Failed to submit maintenance request");
            return Json(new SubmitResponse() { IsSuccess = false });
        }

        return Json(new SubmitResponse
        {
            IsSuccess = true,
            RedirectUrl = Url.Action(nameof(HomeController.Index), HomeController.Name)!
        });
    }
}