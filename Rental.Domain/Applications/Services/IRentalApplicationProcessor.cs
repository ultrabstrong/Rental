using Rental.Domain.Applications.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Rental.Domain.Applications.Services;
public interface IRentalApplicationProcessor
{
    Task ProcessAsync(RentalApplication rentalApplication, CancellationToken cancellationToken = default);
}
