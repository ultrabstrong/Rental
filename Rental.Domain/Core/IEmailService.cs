using Rental.Domain.Models;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Rental.Domain.Core;

public interface IEmailService
{
    Task SendEmailAsync(IEmailRequestBuilder emailRequestBuilder, Stream toAttach, CancellationToken cancellationToken = default);
}
