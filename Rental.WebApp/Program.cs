using Rental.Domain;
using Rental.Domain.Applications.Services;
using Rental.Domain.Maintenance.Services;
using Rental.WebApp.Middleware;
using Rental.WebApp.Models.Site;
using Rental.WebApp.Rendering;
using Rental.WebApp.Services;
using Serilog;
using Rental.WebApp.Validation;
using Microsoft.Extensions.Options;

Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .Enrich.WithProperty("Application", "Rental.WebApp")
#if DEBUG
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .WriteTo.Seq("http://localhost:5341")
#else
    .WriteTo.File(
        Path.Combine(AppContext.BaseDirectory, "logs", "log-.txt"),
        rollingInterval: RollingInterval.Day,
        retainedFileTimeLimit: TimeSpan.FromDays(90))
    .MinimumLevel.Warning()
#endif
    .CreateLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog();

    builder.Services.AddSingleton<IValidateOptions<SiteOptions>, SiteOptionsValidator>();
    builder.Services.AddOptionsWithValidateOnStart<SiteOptions>()
        .Bind(builder.Configuration.GetSection(SiteOptions.NAME));

    builder.Services.AddControllersWithViews();

    builder.Services.AddScoped<IRazorViewRenderer, RazorViewRenderer>();
    builder.Services.AddScoped<IMaintenanceRequestPdfService, MaintenanceRequestPdfService>();
    builder.Services.AddScoped<IRentalApplicationPdfService, RentalApplicationPdfService>();
    builder.Services.AddDomainServices(builder.Configuration);

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
    app.MapControllerRoute("Default", "{controller}/{action}/{id?}", new { controller = Rental.WebApp.Controllers.HomeController.Name });

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
