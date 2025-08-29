using Rental.Domain.Maintenance.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Rental.Domain.Maintenance.Services;
public interface IMaintenanceRequestPdfService
{
    Task<byte[]> GenerateAsync(MaintenanceRequest maintenanceRequest, CancellationToken cancellationToken = default);
}
