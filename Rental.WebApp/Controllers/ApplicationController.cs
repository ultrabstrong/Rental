using Microsoft.AspNetCore.Mvc;
using Rental.Domain.Applications.Services;
using Rental.WebApp.Extensions;
using Rental.WebApp.Mappers;
using Rental.WebApp.Models;
using Rental.WebApp.Models.Application;
using Serilog;

namespace Rental.WebApp.Controllers;

public class ApplicationController : Controller
{
    public static readonly string Name = nameof(ApplicationController).Replace(nameof(Controller), "");
    private readonly IRentalApplicationProcessor _applicationProcessor;

    public ApplicationController(IRentalApplicationProcessor applicationProcessor)
    {
        _applicationProcessor = applicationProcessor;
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

            await _applicationProcessor.ProcessAsync(application.ToDomainModel());
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