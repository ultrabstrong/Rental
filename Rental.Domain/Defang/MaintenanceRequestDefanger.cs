using Rental.Domain.Maintenance.Models;

namespace Rental.Domain.Defang;

internal class MaintenanceRequestDefanger(IDefanger defanger) : IMaintenanceRequestDefanger
{
    private readonly IDefanger _defanger = defanger;

    public MaintenanceRequest Defang(MaintenanceRequest request)
    {
        return new MaintenanceRequest(
            RentalAddress: _defanger.Defang(request.RentalAddress),
            FirstName: _defanger.Defang(request.FirstName),
            LastName: _defanger.Defang(request.LastName),
            Email: _defanger.Defang(request.Email),
            Phone: _defanger.Defang(request.Phone),
            Description: _defanger.Defang(request.Description)
        );
    }
}
