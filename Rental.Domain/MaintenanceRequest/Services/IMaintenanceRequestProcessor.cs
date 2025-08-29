namespace Rental.Domain.MaintenanceRequest.Services;
public interface IMaintenanceRequestProcessor
{
    Task HandleAsync(IMaintenanceRequestProcessor maintenanceRequest);
}
