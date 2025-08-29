using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Rental.Domain.Maintenance.Services;
using Rental.WebApp.Mappers;
using Rental.WebApp.Models;
using Rental.WebApp.Models.Maintenance;
using Rental.WebApp.Models.Site;
using Serilog;

namespace Rental.WebApp.Controllers;

public class MaintenanceController : Controller
{
    public static readonly string Name = nameof(MaintenanceController).Replace(nameof(Controller), "");
    private readonly IMaintenanceRequestProcessor _maintenanceRequestProcessor;
    private readonly IOptionsSnapshot<SiteOptions> _siteDetails;

    public MaintenanceController(
        IMaintenanceRequestProcessor maintenanceRequestProcessor,
        IOptionsSnapshot<SiteOptions> siteDetails)
    {
        _maintenanceRequestProcessor = maintenanceRequestProcessor;
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

            await _maintenanceRequestProcessor.HandleAsync(maintenanceRequest.ToDomainModel());
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