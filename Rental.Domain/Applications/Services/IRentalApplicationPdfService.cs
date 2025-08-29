using Rental.Domain.Applications.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Rental.Domain.Applications.Services;

public interface IRentalApplicationPdfService
{
    Task<byte[]> GenerateAsync(RentalApplication application, CancellationToken cancellationToken = default);
}
