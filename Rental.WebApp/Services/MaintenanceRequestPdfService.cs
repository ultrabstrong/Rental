using Microsoft.Extensions.Options;
using Rental.Domain.Maintenance.Models;
using Rental.Domain.Maintenance.Services;
using Rental.WebApp.Converters;
using Rental.WebApp.Mappers;
using Rental.WebApp.Models.Site;
using Rental.WebApp.Rendering;

namespace Rental.WebApp.Services;

public class MaintenanceRequestPdfService : IMaintenanceRequestPdfService
{
    private readonly IRazorViewRenderer _viewRenderer;
    private readonly IOptionsSnapshot<SiteOptions> _siteOptions;
    private readonly ILogger<MaintenanceRequestPdfService> _logger;

    public MaintenanceRequestPdfService(
        IRazorViewRenderer viewRenderer,
        IOptionsSnapshot<SiteOptions> siteOptions,
        ILogger<MaintenanceRequestPdfService> logger)
    {
        _viewRenderer = viewRenderer;
        _siteOptions = siteOptions;
        _logger = logger;
    }

    public async Task<byte[]> GenerateAsync(MaintenanceRequest maintenanceRequest, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        _logger.LogDebug("Rendering maintenance request PDF view");
        var vm = maintenanceRequest.ToViewModel();
        var html = await _viewRenderer.RenderAsync("~/Views/Maintenance/MaintenanceRequestPdf.cshtml", vm);

        cancellationToken.ThrowIfCancellationRequested();
        _logger.LogDebug("Converting maintenance request HTML to PDF");
        var pdf = HtmlToPdfConverter.GetPdfBytes(
            html,
            _siteOptions.Value.CompanyName,
            $"{maintenanceRequest.FirstName} {maintenanceRequest.LastName} Maintenance Request",
            $"Maintenance request for {maintenanceRequest.RentalAddress} from {maintenanceRequest.FirstName} {maintenanceRequest.LastName}");

        return pdf;
    }
}
