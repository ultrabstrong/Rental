using Corely.Common.Http;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Rental.Domain;
using Rental.WebApp.Middleware;
using Rental.WebApp.Models.Site;
using Rental.WebApp.Rendering;
using Rental.WebApp.Services;
using Rental.WebApp.Services.HumanVerification;
using Rental.WebApp.Validation;
using Serilog;
using Serilog.Events;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Verbose()
    .Enrich.WithProperty("Application", "Rental.WebApp")
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .MinimumLevel.Override("System", LogEventLevel.Warning)
    .Enrich.FromLogContext() // include context info from log scopes as properties
#if DEBUG
    .WriteTo.Console(restrictedToMinimumLevel: LogEventLevel.Debug)
    .WriteTo.Seq("http://localhost:5341", LogEventLevel.Verbose)
#else
    .WriteTo.File(
        Path.Combine(AppContext.BaseDirectory, "logs", "log-.txt"),
        rollingInterval: RollingInterval.Day,
        retainedFileTimeLimit: TimeSpan.FromDays(90)
    )
    .MinimumLevel.Warning()
#endif
    .CreateLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog();

    builder.Services.AddSingleton<IValidateOptions<SiteOptions>, SiteOptionsValidator>();
    builder
        .Services.AddOptionsWithValidateOnStart<SiteOptions>()
        .Bind(builder.Configuration.GetSection(SiteOptions.NAME));

    builder.Services.AddControllersWithViews();

    builder.Services.AddScoped<IRazorViewRenderer, RazorViewRenderer>();

    builder.Services.AddDomainServices(
        builder.Configuration,
        rentalApplicationPdfFactory: sp => new RentalApplicationPdfService(
            sp.GetRequiredService<IRazorViewRenderer>(),
            sp.GetRequiredService<IOptionsSnapshot<SiteOptions>>(),
            sp.GetRequiredService<ILogger<RentalApplicationPdfService>>()
        ),
        maintenanceRequestPdfFactory: sp => new MaintenanceRequestPdfService(
            sp.GetRequiredService<IRazorViewRenderer>(),
            sp.GetRequiredService<IOptionsSnapshot<SiteOptions>>(),
            sp.GetRequiredService<ILogger<MaintenanceRequestPdfService>>()
        )
    );

    // Turnstile
    builder.Services.TryAddTransient<HttpErrorLoggingHandler>();
    builder.Services.TryAddTransient<HttpRequestResponseLoggingHandler>();
    builder.Services.Configure<TurnstileOptions>(
        builder.Configuration.GetSection(TurnstileOptions.NAME)
    );
    builder
        .Services.AddHttpClient<TurnstileVerifier>(TurnstileOptions.NAME)
        .AddHttpMessageHandler<HttpErrorLoggingHandler>()
        .AddHttpMessageHandler<HttpRequestResponseLoggingHandler>();
    builder.Services.AddScoped<IHumanVerifier, TurnstileVerifier>();

    var app = builder.Build();

    if (!app.Environment.IsDevelopment())
    {
        app.UseHsts();
    }

    app.UseHttpsRedirection();
    app.UseSerilogRequestLogging();
    app.UseMiddleware<CorrelationIdMiddleware>();
    app.UseMiddleware<SecurityHeadersMiddleware>();
    app.UseStaticFiles();
    app.UseRouting();
    app.UseAuthorization();
    app.MapDefaultControllerRoute();
    app.MapControllerRoute(
        "Default",
        "{controller}/{action}/{id?}",
        new { controller = Rental.WebApp.Controllers.HomeController.Name }
    );

    Log.Information("Starting web application");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application start-up failed");
}
finally
{
    Log.CloseAndFlush();
}
