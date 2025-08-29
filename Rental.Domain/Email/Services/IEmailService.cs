using Rental.Domain.Email.Models;

namespace Rental.Domain.Email.Services;

public interface IEmailService
{
    Task SendEmailAsync(IEmailRequestBuilder emailRequestBuilder, Stream toAttach, CancellationToken cancellationToken = default);
}
