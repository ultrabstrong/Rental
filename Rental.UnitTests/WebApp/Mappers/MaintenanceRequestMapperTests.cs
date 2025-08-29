using AutoFixture;
using Rental.WebApp.Mappers;
using MaintenanceRequestViewModel = Rental.WebApp.Models.Maintenance.MaintenanceRequest;
using DomainMaintenanceRequest = Rental.Domain.Maintenance.Models.MaintenanceRequest;

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

    [Fact]
    public void ToViewModel_MapsAllProperties()
    {
        var domain = _fixture.Create<DomainMaintenanceRequest>();

        MaintenanceRequestViewModel vm = domain.ToViewModel();

        Assert.Equal(domain.RentalAddress, vm.RentalAddress);
        Assert.Equal(domain.FirstName, vm.FirstName);
        Assert.Equal(domain.LastName, vm.LastName);
        Assert.Equal(domain.Email, vm.Email);
        Assert.Equal(domain.Phone, vm.Phone);
        Assert.Equal(domain.Description, vm.Description);
    }
}
