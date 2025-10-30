using Microsoft.Extensions.Logging;
using Rental.Domain.Applications.Models;
using Rental.Domain.Email.Models;
using Rental.Domain.Email.Services;

namespace Rental.Domain.Applications.Services;

internal class RentalApplicationProcessor(
    IRentalApplicationPdfService pdfService,
    IEmailService emailService,
    ILogger<RentalApplicationProcessor> logger
) : IRentalApplicationProcessor
{
    private readonly IRentalApplicationPdfService _pdfService = pdfService;
    private readonly IEmailService _emailService = emailService;
    private readonly ILogger<RentalApplicationProcessor> _logger = logger;

    public async Task ProcessAsync(
        RentalApplication rentalApplication,
        CancellationToken cancellationToken
    )
    {
        cancellationToken.ThrowIfCancellationRequested();
        _logger.LogDebug("Creating PDF for rental application");
        var pdf = await _pdfService.GenerateAsync(rentalApplication, cancellationToken);

        cancellationToken.ThrowIfCancellationRequested();
        _logger.LogDebug("Sending rental application email");
        using var pdfStream = new MemoryStream(pdf);
        var emailRequest = BuildEmailRequest(rentalApplication);
        await _emailService.SendEmailAsync(emailRequest, pdfStream, cancellationToken);
    }

    private static EmailRequest BuildEmailRequest(RentalApplication app) =>
        new(
            Subject: $"Application for {app.RentalAddress} from {app.PersonalInfo.FirstName} {app.PersonalInfo.LastName}; Co-Applicants: {app.OtherApplicants}",
            Body: $"Attached is the application for {app.RentalAddress} from {app.PersonalInfo.FirstName} {app.PersonalInfo.LastName}",
            AttachmentName: $"{app.PersonalInfo.FirstName} {app.PersonalInfo.LastName} Application.pdf",
            PreferredReplyTo: app.PersonalInfo.Email
        );
}
