using Microsoft.Extensions.Options;
using Rental.Domain.Applications.Models;
using Rental.Domain.Applications.Services;
using Rental.WebApp.Controllers;
using Rental.WebApp.Converters;
using Rental.WebApp.Mappers;
using Rental.WebApp.Models.Site;
using Rental.WebApp.Rendering;

namespace Rental.WebApp.Services;

internal class RentalApplicationPdfService : IRentalApplicationPdfService
{
    private readonly IRazorViewRenderer _viewRenderer;
    private readonly IOptionsSnapshot<SiteOptions> _siteOptions;
    private readonly ILogger<RentalApplicationPdfService> _logger;

    public RentalApplicationPdfService(
        IRazorViewRenderer viewRenderer,
        IOptionsSnapshot<SiteOptions> siteOptions,
        ILogger<RentalApplicationPdfService> logger
    )
    {
        _viewRenderer = viewRenderer;
        _siteOptions = siteOptions;
        _logger = logger;
    }

    public async Task<byte[]> GenerateAsync(
        RentalApplication application,
        CancellationToken cancellationToken = default
    )
    {
        cancellationToken.ThrowIfCancellationRequested();
        _logger.LogDebug("Rendering application PDF view");
        var vm = application.ToViewModel();
        var html = await _viewRenderer.RenderAsync(
            $"~/Views/{RentalApplicationController.Name}/ApplicationPdf.cshtml",
            vm
        );

        cancellationToken.ThrowIfCancellationRequested();
        _logger.LogDebug("Converting application HTML to PDF");
        var pdf = HtmlToPdfConverter.GetPdfBytes(
            html,
            _siteOptions.Value.CompanyName,
            $"{application.PersonalInfo.FirstName} {application.PersonalInfo.LastName} Application",
            $"Application for {application.RentalAddress} from {application.PersonalInfo.FirstName} {application.PersonalInfo.LastName}; Co-Applicants : {application.OtherApplicants}"
        );

        return pdf;
    }
}
