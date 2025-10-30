using System.Threading;
using System.Threading.Tasks;
using Rental.Domain.Maintenance.Models;

namespace Rental.Domain.Maintenance.Services;

public interface IMaintenanceRequestPdfService
{
    Task<byte[]> GenerateAsync(
        MaintenanceRequest maintenanceRequest,
        CancellationToken cancellationToken = default
    );
}
