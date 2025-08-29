using Rental.Domain.Applications.Models;

namespace Rental.Domain.Applications.Services;

public interface IRentalApplicationPdfService
{
    Task<byte[]> GenerateAsync(RentalApplication application);
}
