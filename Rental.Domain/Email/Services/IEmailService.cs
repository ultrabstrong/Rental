using Rental.Domain.Email.Models;

namespace Rental.Domain.Email.Services;

public interface IEmailService
{
    Task SendEmailAsync(EmailRequest emailRequest, Stream attachmentStream, CancellationToken cancellationToken = default);
}
