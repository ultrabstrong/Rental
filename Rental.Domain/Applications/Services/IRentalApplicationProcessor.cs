using Rental.Domain.Applications.Models;

namespace Rental.Domain.Applications.Services;
public interface IRentalApplicationProcessor
{
    Task ProcessAsync(RentalApplication rentalApplication);
}
