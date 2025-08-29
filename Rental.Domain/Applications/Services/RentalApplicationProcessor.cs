using Microsoft.Extensions.Logging;
using Rental.Domain.Applications.Models;
using Rental.Domain.Email.Services;
using System.Threading;
using System.Threading.Tasks;

namespace Rental.Domain.Applications.Services;

internal class RentalApplicationProcessor(
    IRentalApplicationPdfService pdfService,
    IEmailService emailService,
    ILogger<RentalApplicationProcessor> logger) : IRentalApplicationProcessor
{
    private readonly IRentalApplicationPdfService _pdfService = pdfService;
    private readonly IEmailService _emailService = emailService;
    private readonly ILogger<RentalApplicationProcessor> _logger = logger;

    public async Task ProcessAsync(RentalApplication rentalApplication, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        _logger.LogDebug("Creating PDF for rental application");
        var pdf = await _pdfService.GenerateAsync(rentalApplication, cancellationToken);

        cancellationToken.ThrowIfCancellationRequested();
        _logger.LogDebug("Sending rental application email");
        using var pdfStream = new MemoryStream(pdf);
        await _emailService.SendEmailAsync(rentalApplication, pdfStream, cancellationToken);
    }
}
