using Rental.Domain.Maintenance.Models;
using MaintenanceRequestViewModel = Rental.WebApp.Models.Maintenance.MaintenanceRequest;

namespace Rental.WebApp.Mappers;

internal static class MaintenanceRequestMappers
{
    public static MaintenanceRequest ToDomainModel(this MaintenanceRequestViewModel src) =>
        new(src.RentalAddress, src.FirstName, src.LastName, src.Email, src.Phone, src.Description);

    public static MaintenanceRequestViewModel ToViewModel(this MaintenanceRequest src) =>
        new()
        {
            RentalAddress = src.RentalAddress,
            FirstName = src.FirstName,
            LastName = src.LastName,
            Email = src.Email,
            Phone = src.Phone,
            Description = src.Description,
        };
}
