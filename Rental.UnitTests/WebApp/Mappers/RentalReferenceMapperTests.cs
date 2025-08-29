using AutoFixture;
using Rental.WebApp.Mappers;
using RentalReferenceViewModel = Rental.WebApp.Models.Application.RentalReference;
using DomainRentalReference = Rental.Domain.Applications.Models.RentalReference;

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
}
