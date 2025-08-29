using AutoFixture;
using Rental.WebApp.Mappers;
using MaintenanceRequestViewModel = Rental.WebApp.Models.Maintenance.MaintenanceRequest;
using DomainMaintenanceRequest = Rental.Domain.MaintenanceRequest.MaintenanceRequest;

namespace Rental.UnitTests.WebApp.Mappers;

public class MaintenanceRequestMapperTests
{
    private readonly Fixture _fixture = new();

    [Fact]
    public void ToDomainModel_MapsAllProperties()
    {
        var vm = _fixture.Create<MaintenanceRequestViewModel>();

        DomainMaintenanceRequest domain = vm.ToDomainModel();

        Assert.Equal(vm.RentalAddress, domain.RentalAddress);
        Assert.Equal(vm.FirstName, domain.FirstName);
        Assert.Equal(vm.LastName, domain.LastName);
        Assert.Equal(vm.Email, domain.Email);
        Assert.Equal(vm.Phone, domain.Phone);
        Assert.Equal(vm.Description, domain.Description);
    }
}
