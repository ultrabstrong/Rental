using Rental.Domain.Maintenance.Models;

namespace Rental.Domain.Defang;

public interface IMaintenanceRequestDefanger
{
    MaintenanceRequest Defang(MaintenanceRequest request);
}
