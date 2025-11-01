using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Rental.WebApp.Services.HumanVerification;

namespace Rental.WebApp.Filters;

/// <summary>
/// Declarative Cloudflare Turnstile verification for POST actions that return partial views on failure.
/// Usage: [ValidateTurnstile("ViewName")] where ViewName is the partial/page to render when CAPTCHA fails.
/// </summary>
public sealed class ValidateTurnstileAttribute : TypeFilterAttribute
{
    public ValidateTurnstileAttribute(string viewName)
        : base(typeof(TurnstileValidationFilter))
    {
        Arguments = [viewName];
    }
}

internal sealed class TurnstileValidationFilter : IAsyncActionFilter
{
    private readonly IHumanVerifier _humanVerifier;
    private readonly ILogger<TurnstileValidationFilter> _logger;
    private readonly string _viewNameOnFailure;

    public TurnstileValidationFilter(
        IHumanVerifier humanVerifier,
        ILogger<TurnstileValidationFilter> logger,
        string viewName
    )
    {
        _humanVerifier = humanVerifier;
        _logger = logger;
        _viewNameOnFailure = viewName;
    }

    public async Task OnActionExecutionAsync(
        ActionExecutingContext context,
        ActionExecutionDelegate next
    )
    {
        var httpContext = context.HttpContext;
        // Only validate for POST requests
        if (!HttpMethods.IsPost(httpContext.Request.Method))
        {
            await next();
            return;
        }

        // Extract token and IP
        var token = httpContext.Request.Form["cf-turnstile-response"].ToString();
        var remoteIp = httpContext.Connection.RemoteIpAddress?.ToString();

        var ct = httpContext.RequestAborted;
        bool captchaOk = false;
        try
        {
            captchaOk = await _humanVerifier.VerifyAsync(token, remoteIp, ct);
        }
        catch (OperationCanceledException)
        {
            // Let action handle cancellation consistently
            throw;
        }

        if (captchaOk)
        {
            await next();
            return;
        }

        // CAPTCHA failed: log and short-circuit with appropriate partial view
        var controller = (Controller)context.Controller;
        var actionName = context.RouteData.Values["action"]?.ToString();
        var controllerName = context.RouteData.Values["controller"]?.ToString();
        _logger.LogInformation(
            "Turnstile CAPTCHA failed for {Controller}.{Action}",
            controllerName,
            actionName
        );

        controller.ModelState.AddModelError(
            string.Empty,
            "CAPTCHA validation failed. Please try again."
        );
        controller.ViewBag.Errors = true;
        controller.Response.StatusCode = StatusCodes.Status400BadRequest;

        // Try to pick the first complex model argument (e.g., view model) to reuse in the view
        object? model = null;
        foreach (var kvp in context.ActionArguments)
        {
            if (kvp.Value is null)
                continue;
            var type = kvp.Value.GetType();
            if (!type.IsPrimitive && type != typeof(string))
            {
                model = kvp.Value;
                break;
            }
        }

        controller.ViewData.Model = model;
        context.Result = new PartialViewResult
        {
            ViewName = _viewNameOnFailure,
            ViewData = controller.ViewData,
            TempData = controller.TempData,
        };
    }
}
