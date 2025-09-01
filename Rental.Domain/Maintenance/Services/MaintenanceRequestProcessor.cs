using Microsoft.Extensions.Logging;
using Rental.Domain.Email.Models;
using Rental.Domain.Email.Services;
using Rental.Domain.Maintenance.Models;
using System.Text;

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
        var emailRequest = BuildEmailRequest(maintenanceRequest);
        await _emailService.SendEmailAsync(emailRequest, pdfStream, cancellationToken);
    }

    private static EmailRequest BuildEmailRequest(MaintenanceRequest src)
    {
        var bodyBuilder = new StringBuilder();
        bodyBuilder.AppendLine($"Attached is the maintenance request from {src.FirstName} {src.LastName} for {src.RentalAddress}");
        bodyBuilder.AppendLine($"Email: {(string.IsNullOrWhiteSpace(src.Email) ? "Not provided" : src.Email)}");
        bodyBuilder.AppendLine($"Phone: {(string.IsNullOrWhiteSpace(src.Phone) ? "Not provided" : src.Phone)}");
        bodyBuilder.AppendLine();
        bodyBuilder.AppendLine(src.Description);
        return new EmailRequest(
            Subject: $"Maintenance request for {src.RentalAddress} from {src.FirstName} {src.LastName}",
            Body: bodyBuilder.ToString(),
            AttachmentName: $"{src.FirstName} {src.LastName} Maintenance Request.pdf",
            PreferredReplyTo: src.Email
        );
    }
}
