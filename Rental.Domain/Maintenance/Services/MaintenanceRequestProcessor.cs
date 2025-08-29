using Microsoft.Extensions.Logging;
using Rental.Domain.Email.Services;
using Rental.Domain.Maintenance.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Rental.Domain.Maintenance.Services;
internal class MaintenanceRequestProcessor(
    IMaintenanceRequestPdfService pdfService,
    IEmailService emailService,
    ILogger<MaintenanceRequestProcessor> logger)
    : IMaintenanceRequestProcessor
{
    private readonly IMaintenanceRequestPdfService _pdfService = pdfService;
    private readonly IEmailService _emailService = emailService;
    private readonly ILogger<MaintenanceRequestProcessor> _logger = logger;

    async Task IMaintenanceRequestProcessor.HandleAsync(MaintenanceRequest maintenanceRequest, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        _logger.LogDebug("Creating PDF for maintenance request");
        var pdf = await _pdfService.GenerateAsync(maintenanceRequest, cancellationToken);

        cancellationToken.ThrowIfCancellationRequested();
        _logger.LogDebug("Sending maintenance email");
        using var pdfStream = new MemoryStream(pdf);
        await _emailService.SendEmailAsync(maintenanceRequest, pdfStream, cancellationToken);
    }
}
