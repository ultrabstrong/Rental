using System.Threading.Tasks;

namespace Rental.Domain.MaintenanceRequest;
public interface IMaintenanceRequestHandlerService
{
    Task HandleAsync();
}
