using Microsoft.AspNetCore.Mvc;
using Rental.Domain.Applications.Services;
using Rental.WebApp.Extensions;
using Rental.WebApp.Mappers;
using Rental.WebApp.Models;
using Rental.WebApp.Models.Application;
using Rental.WebApp.Services.HumanVerification;
using Serilog;

namespace Rental.WebApp.Controllers;

public class RentalApplicationController : Controller
{
    public static readonly string Name = nameof(RentalApplicationController)
        .Replace(nameof(Controller), "");
    private readonly IRentalApplicationProcessor _applicationProcessor;
    private readonly IHumanVerifier _humanVerifier;

    public RentalApplicationController(
        IRentalApplicationProcessor applicationProcessor,
        IHumanVerifier humanVerifier
    )
    {
        _applicationProcessor = applicationProcessor;
        _humanVerifier = humanVerifier;
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
        //return PartialView("Apply", new RentalApplication());
        return PartialView("Apply", RentalApplication.MinimalTestApplication);
        //return PartialView("Apply", RentalApplication.FullTestApplication);
#else
        return PartialView("Apply", new RentalApplication());
#endif
    }

    [HttpPost, Route("SubmitApplication")]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> SubmitApplication(
        RentalApplication application,
        CancellationToken cancellationToken
    )
    {
        try
        {
            // Verify Turnstile token first when configured
            var token = Request.Form["cf-turnstile-response"].ToString();
            var remoteIp = HttpContext.Connection.RemoteIpAddress?.ToString();
            var captchaOk = await _humanVerifier.VerifyAsync(token, remoteIp, cancellationToken);
            if (!captchaOk)
            {
                Log.Logger.Information("Submit rental application CAPTCHA failed");
                ModelState.AddModelError(
                    string.Empty,
                    "CAPTCHA validation failed. Please try again."
                );
                ViewBag.Errors = true;
                Response.StatusCode = StatusCodes.Status400BadRequest;
                return PartialView("Apply", application);
            }

            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .Where(m => m.Value != null && m.Value.Errors.Any())
                    .Select(m =>
                        $"{m.Key}: {string.Join(", ", m.Value!.Errors.Select(e => e.ErrorMessage))}"
                    )
                    .ToList();

                Log.Logger.Information("Application validation errors: {@Errors}", errors);
                this.AddUSStatesToViewBag();
                ViewBag.Errors = true;
                Response.StatusCode = StatusCodes.Status400BadRequest;
                return PartialView("Apply", application);
            }

            await _applicationProcessor.ProcessAsync(
                application.ToDomainModel(),
                cancellationToken
            );
        }
        catch (OperationCanceledException)
        {
            Log.Logger.Warning("Application submission cancelled");
            return StatusCode(StatusCodes.Status499ClientClosedRequest);
        }
        catch (Exception ex)
        {
            Log.Logger.Error(ex, "Failed to submit application");
            return Json(new SubmitResponse(IsSuccess: false));
        }

        return Json(
            new SubmitResponse(
                IsSuccess: true,
                RedirectUrl: Url.Action(nameof(HomeController.Index), HomeController.Name)!
            )
        );
    }
}
