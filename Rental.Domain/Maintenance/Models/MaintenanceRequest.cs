namespace Rental.Domain.Maintenance.Models;

public record MaintenanceRequest(
    string RentalAddress,
    string FirstName,
    string LastName,
    string Email,
    string Phone,
    string Description
);
