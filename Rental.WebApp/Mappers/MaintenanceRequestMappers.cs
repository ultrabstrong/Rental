using Rental.Domain.MaintenanceRequest;
using MaintenanceRequestViewModel = Rental.WebApp.Models.Maintenance.MaintenanceRequest;

namespace Rental.WebApp.Mappers;

public static class MaintenanceRequestMappers
{
    public static MaintenanceRequest ToDomainModel(this MaintenanceRequestViewModel src) => new()
    {
        RentalAddress = src.RentalAddress,
        FirstName = src.FirstName,
        LastName = src.LastName,
        Email = src.Email,
        Phone = src.Phone,
        Description = src.Description
    };
}
