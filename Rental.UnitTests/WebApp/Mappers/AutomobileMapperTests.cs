using AutoFixture;
using Rental.WebApp.Mappers;
using AutomobileViewModel = Rental.WebApp.Models.Application.Automobile;
using DomainAutomobile = Rental.Domain.Applications.Models.Automobile;

namespace Rental.UnitTests.WebApp.Mappers;

public class AutomobileMapperTests
{
    private readonly Fixture _fixture = new();

    [Fact]
    public void ToDomainModel_MapsAllProperties()
    {
        var vm = _fixture.Create<AutomobileViewModel>();

        DomainAutomobile domain = vm.ToDomainModel();

        Assert.Equal(vm.AllowElectiveRequire, domain.AllowElectiveRequire);
        Assert.Equal((int?)vm.ElectiveRequireValue, (int?)domain.ElectiveRequireValue);
        Assert.Equal(vm.Make, domain.Make);
        Assert.Equal(vm.Model, domain.Model);
        Assert.Equal(vm.Year, domain.Year);
        Assert.Equal(vm.State, domain.State);
        Assert.Equal(vm.LicenseNum, domain.LicenseNum);
        Assert.Equal(vm.Color, domain.Color);
    }

    [Fact]
    public void ToViewModel_MapsAllProperties()
    {
        var domain = _fixture.Create<DomainAutomobile>();

        AutomobileViewModel vm = domain.ToViewModel();

        Assert.Equal(domain.AllowElectiveRequire, vm.AllowElectiveRequire);
        Assert.Equal((int?)domain.ElectiveRequireValue, (int?)vm.ElectiveRequireValue);
        Assert.Equal(domain.Make, vm.Make);
        Assert.Equal(domain.Model, vm.Model);
        Assert.Equal(domain.Year, vm.Year);
        Assert.Equal(domain.State, vm.State);
        Assert.Equal(domain.LicenseNum, vm.LicenseNum);
        Assert.Equal(domain.Color, vm.Color);
    }
}
