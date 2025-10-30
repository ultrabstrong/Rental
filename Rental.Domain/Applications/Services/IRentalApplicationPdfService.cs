using System.Threading;
using System.Threading.Tasks;
using Rental.Domain.Applications.Models;

namespace Rental.Domain.Applications.Services;

public interface IRentalApplicationPdfService
{
    Task<byte[]> GenerateAsync(
        RentalApplication application,
        CancellationToken cancellationToken = default
    );
}
