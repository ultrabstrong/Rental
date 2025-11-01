using Rental.Domain.Applications.Models;

namespace Rental.Domain.Defang;

public interface IRentalApplicationDefanger
{
    RentalApplication Defang(RentalApplication app);
}
