using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Rental.Domain.Core;
using Rental.WebApp.Models;
using Rental.WebApp.Models.Maintenance;
using Rental.WebApp.Models.Site;
using Serilog;

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
    public async Task<ActionResult> SubmitMaintenanceRequest(MaintenanceRequest maintenanceRequest, CancellationToken cancellationToken)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .Where(m => m.Value != null && m.Value.Errors.Any())
                    .Select(m => $"{m.Key}: {string.Join(", ", m.Value!.Errors.Select(e => e.ErrorMessage))}")
                    .ToList();
                Log.Logger.Information("Maintenance request validation errors: {@Errors}", errors);
                ViewBag.Errors = true;
                Response.StatusCode = StatusCodes.Status400BadRequest;
                return PartialView("_MaintenanceRequestForm", maintenanceRequest);
            }

            Log.Logger.Debug("Creating view HTML");
            var html = await RenderRazorViewToStringAsync("MaintenanceRequestPdf", maintenanceRequest);

            Log.Logger.Debug("Converting HTML to PDF");
            var pdf = HtmlToPdfConverter.GetPdfBytes(html,
                _siteDetails.Value.CompanyName,
                $"{maintenanceRequest.FirstName} {maintenanceRequest.LastName} Maintenance Request",
                $"Maintenance request for {maintenanceRequest.RentalAddress} from {maintenanceRequest.FirstName} {maintenanceRequest.LastName}");

            Log.Logger.Debug("Sending maintenance email");
            using (var pdfStream = new MemoryStream(pdf))
            using (var emailService = new EmailService(_siteDetails.Value.MailSettings))
            {
                await emailService.SendEmailAsync(maintenanceRequest, pdfStream, cancellationToken);
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