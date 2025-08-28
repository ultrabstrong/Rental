using Rental.WebApp.Middleware;
using Serilog;
using Rental.WebApp.Models.Site;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog with the preferred configuration
Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .Enrich.WithProperty("Application", "Rental.WebApp")
    .Enrich.WithProperty("CorrelationId", Guid.NewGuid())
#if DEBUG
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .WriteTo.Seq("http://localhost:5341")
#else
    .WriteTo.File(
        Path.Combine(builder.Environment.ContentRootPath, "logs", "log-.txt"),
        rollingInterval: RollingInterval.Day,
        retainedFileTimeLimit: TimeSpan.FromDays(90))
    .MinimumLevel.Warning()
#endif
    .CreateLogger();

builder.Host.UseSerilog();

// Ensure appsettings.json reload is enabled (CreateBuilder already does this, explicit for clarity)
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

// Bind SiteDetails with options pattern for live reload
builder.Services.Configure<SiteDetails>(builder.Configuration.GetSection("SiteDetails"));

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseMiddleware<SecurityHeadersMiddleware>();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapDefaultControllerRoute();
app.MapControllerRoute("Default", "{controller}/{action}/{id?}", new { controller = Rental.WebApp.Controllers.HomeController.Name });

try
{
    Log.Information("Starting web application");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
