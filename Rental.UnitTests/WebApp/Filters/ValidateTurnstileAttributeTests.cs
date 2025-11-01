using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Primitives;
using Moq;
using Rental.WebApp.Filters;
using Rental.WebApp.Services.HumanVerification;

namespace Rental.UnitTests.WebApp.Filters;

public class ValidateTurnstileAttributeTests
{
    private static (
        ActionExecutingContext ctx,
        Controller controller,
        ActionContext actionContext
    ) BuildContext(
        string method,
        Dictionary<string, StringValues>? form = null,
        object? modelArg = null
    )
    {
        var httpContext = new DefaultHttpContext();
        httpContext.Request.Method = method;
        httpContext.Connection.RemoteIpAddress = IPAddress.Loopback;
        // Provide a basic service provider so Request.Form can resolve features safely
        httpContext.RequestServices = new ServiceCollection().BuildServiceProvider();

        if (form != null)
        {
            // Bypass default FormFeature (which may NRE due to antiforgery checks) with a simple test feature
            httpContext.Features.Set<IFormFeature>(new TestFormFeature(new FormCollection(form)));
            // Match typical form posts
            httpContext.Request.ContentType = "application/x-www-form-urlencoded";
        }

        var routeData = new RouteData();
        routeData.Values["controller"] = "Test";
        routeData.Values["action"] = "Post";

        // ControllerContext in ASP.NET Core expects a ControllerActionDescriptor
        var controllerDescriptor = new ControllerActionDescriptor
        {
            ControllerName = "Test",
            ActionName = "Post",
        };
        var actionContext = new ActionContext(httpContext, routeData, controllerDescriptor);

        var controller = new TestController
        {
            ControllerContext = new ControllerContext(actionContext),
            TempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>()),
        };

        var args = new Dictionary<string, object?>();
        if (modelArg != null)
        {
            args["model"] = modelArg;
        }
        var executingContext = new ActionExecutingContext(actionContext, [], args, controller);

        return (executingContext, controller, actionContext);
    }

    [Fact]
    public async Task Post_WithValidToken_InvokesNextAndDoesNotShortCircuit()
    {
        // Arrange
        var form = new Dictionary<string, StringValues>
        {
            ["cf-turnstile-response"] = "valid-token",
        };
        var (ctx, controller, actionContext) = BuildContext("POST", form, new object());

        var verifier = new Mock<IHumanVerifier>();
        verifier
            .Setup(v =>
                v.VerifyAsync("valid-token", It.IsAny<string?>(), It.IsAny<CancellationToken>())
            )
            .ReturnsAsync(true);

        ILogger<TurnstileValidationFilter> logger = NullLogger<TurnstileValidationFilter>.Instance;
        var filter = new TurnstileValidationFilter(verifier.Object, logger, "TestView");

        var nextCalled = false;
        Task<ActionExecutedContext> next()
        {
            nextCalled = true;
            return Task.FromResult<ActionExecutedContext>(
                new ActionExecutedContext(actionContext, [], controller)
            );
        }

        // Act
        await filter.OnActionExecutionAsync(ctx, next);

        // Assert
        Assert.True(nextCalled);
        Assert.Null(ctx.Result);
        Assert.True(controller.ModelState.IsValid);
    }

    [Fact]
    public async Task Post_WithInvalidToken_ShortCircuitsWithPartialViewAnd400()
    {
        // Arrange
        var form = new Dictionary<string, StringValues>
        {
            ["cf-turnstile-response"] = "invalid-token",
        };
        var model = new { Name = "Test" };
        var (ctx, controller, _) = BuildContext("POST", form, model);

        var verifier = new Mock<IHumanVerifier>();
        verifier
            .Setup(v =>
                v.VerifyAsync("invalid-token", It.IsAny<string?>(), It.IsAny<CancellationToken>())
            )
            .ReturnsAsync(false);

        ILogger<TurnstileValidationFilter> logger = NullLogger<TurnstileValidationFilter>.Instance;
        var filter = new TurnstileValidationFilter(verifier.Object, logger, "Apply");

        var next = new ActionExecutionDelegate(() =>
            throw new InvalidOperationException("Should not be called")
        );

        // Act
        await filter.OnActionExecutionAsync(ctx, next);

        // Assert
        var result = Assert.IsType<PartialViewResult>(ctx.Result);
        Assert.Equal("Apply", result.ViewName);
        Assert.Equal(StatusCodes.Status400BadRequest, controller.Response.StatusCode);
        Assert.Equal(true, controller.ViewBag.Errors);
        Assert.False(controller.ModelState.IsValid);
        var error = controller.ModelState[string.Empty]!.Errors.First().ErrorMessage;
        Assert.Contains("CAPTCHA validation failed", error);
        Assert.Same(model, result.ViewData!.Model);
    }

    [Fact]
    public async Task Get_Request_DoesNotValidate_CallsNext()
    {
        // Arrange
        var (ctx, controller, actionContext) = BuildContext("GET");
        var verifier = new Mock<IHumanVerifier>(MockBehavior.Strict);
        ILogger<TurnstileValidationFilter> logger = NullLogger<TurnstileValidationFilter>.Instance;
        var filter = new TurnstileValidationFilter(verifier.Object, logger, "Any");

        var nextCalled = false;
        Task<ActionExecutedContext> next()
        {
            nextCalled = true;
            return Task.FromResult<ActionExecutedContext>(
                new ActionExecutedContext(actionContext, [], controller)
            );
        }

        // Act
        await filter.OnActionExecutionAsync(ctx, next);

        // Assert
        Assert.True(nextCalled);
        Assert.Null(ctx.Result);
        verifier.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task Post_WhenVerifierCanceled_RethrowsCancellation()
    {
        // Arrange
        var form = new Dictionary<string, StringValues> { ["cf-turnstile-response"] = "token" };
        var (ctx, controller, _) = BuildContext("POST", form, new object());

        var verifier = new Mock<IHumanVerifier>();
        verifier
            .Setup(v =>
                v.VerifyAsync(
                    It.IsAny<string>(),
                    It.IsAny<string?>(),
                    It.IsAny<CancellationToken>()
                )
            )
            .ThrowsAsync(new OperationCanceledException());

        ILogger<TurnstileValidationFilter> logger = NullLogger<TurnstileValidationFilter>.Instance;
        var filter = new TurnstileValidationFilter(verifier.Object, logger, "Any");

        // Act + Assert
        await Assert.ThrowsAsync<OperationCanceledException>(() =>
            filter.OnActionExecutionAsync(ctx, () => Task.FromResult<ActionExecutedContext>(null!))
        );
    }

    private sealed class TestController : Controller { }

    private sealed class TestFormFeature : IFormFeature
    {
        public TestFormFeature(IFormCollection form)
        {
            Form = form;
        }

        public bool HasFormContentType => true;
        public IFormCollection? Form { get; set; }

        public IFormCollection ReadForm() => Form!;

        public Task<IFormCollection> ReadFormAsync(CancellationToken cancellationToken) =>
            Task.FromResult(Form!);
    }
}
