using AutoFixture;
using Rental.WebApp.Mappers;
using DomainRentalReference = Rental.Domain.Applications.Models.RentalReference;
using RentalReferenceViewModel = Rental.WebApp.Models.Application.RentalReference;

namespace Rental.UnitTests.WebApp.Mappers;

public class RentalReferenceMapperTests
{
    private readonly Fixture _fixture = new();

    [Fact]
    public void ToDomainModel_MapsAllProperties()
    {
        var vm = _fixture.Create<RentalReferenceViewModel>();

        DomainRentalReference domain = vm.ToDomainModel();

        Assert.Equal(vm.AllowElectiveRequire, domain.AllowElectiveRequire);
        Assert.Equal(vm.ElectiveRequireDisplay, domain.ElectiveRequireDisplay);
        Assert.Equal((int?)vm.ElectiveRequireValue, (int?)domain.ElectiveRequireValue);
        Assert.Equal(vm.Street, domain.Street);
        Assert.Equal(vm.City, domain.City);
        Assert.Equal(vm.State, domain.State);
        Assert.Equal(vm.Zip, domain.Zip);
        Assert.Equal(vm.LandlordName, domain.LandlordName);
        Assert.Equal(vm.LandlordPhoneNum, domain.LandlordPhoneNum);
        Assert.Equal(vm.Start, domain.Start);
        Assert.Equal(vm.End, domain.End);
        Assert.Equal(vm.ReasonForMoving, domain.ReasonForMoving);
    }

    [Fact]
    public void ToViewModel_MapsAllProperties()
    {
        var domain = _fixture.Create<DomainRentalReference>();

        RentalReferenceViewModel vm = domain.ToViewModel();

        Assert.Equal(domain.AllowElectiveRequire, vm.AllowElectiveRequire);
        Assert.Equal(domain.ElectiveRequireDisplay, vm.ElectiveRequireDisplay);
        Assert.Equal((int?)domain.ElectiveRequireValue, (int?)vm.ElectiveRequireValue);
        Assert.Equal(domain.Street, vm.Street);
        Assert.Equal(domain.City, vm.City);
        Assert.Equal(domain.State, vm.State);
        Assert.Equal(domain.Zip, vm.Zip);
        Assert.Equal(domain.LandlordName, vm.LandlordName);
        Assert.Equal(domain.LandlordPhoneNum, vm.LandlordPhoneNum);
        Assert.Equal(domain.Start, vm.Start);
        Assert.Equal(domain.End, vm.End);
        Assert.Equal(domain.ReasonForMoving, vm.ReasonForMoving);
    }
}
