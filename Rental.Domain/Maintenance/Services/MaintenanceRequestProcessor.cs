using Microsoft.Extensions.Logging;
using Rental.Domain.Email.Services;
using Rental.Domain.Maintenance.Models;

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

    async Task IMaintenanceRequestProcessor.HandleAsync(MaintenanceRequest maintenanceRequest)
    {
        _logger.LogDebug("Creating PDF for maintenance request");
        var pdf = await _pdfService.GenerateAsync(maintenanceRequest);

        _logger.LogDebug("Sending maintenance email");
        using (var pdfStream = new MemoryStream(pdf))
            await _emailService.SendEmailAsync(maintenanceRequest, pdfStream, CancellationToken.None);
    }
}
