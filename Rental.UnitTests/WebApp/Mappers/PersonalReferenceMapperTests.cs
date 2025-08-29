using AutoFixture;
using Rental.WebApp.Mappers;
using PersonalReferenceViewModel = Rental.WebApp.Models.Application.PersonalReference;
using DomainPersonalReference = Rental.Domain.Applications.Models.PersonalReference;

namespace Rental.UnitTests.WebApp.Mappers;

public class PersonalReferenceMapperTests
{
    private readonly Fixture _fixture = new();

    [Fact]
    public void ToDomainModel_MapsAllProperties()
    {
        var vm = _fixture.Create<PersonalReferenceViewModel>();

        DomainPersonalReference domain = vm.ToDomainModel();

        Assert.Equal(vm.AllowElectiveRequire, domain.AllowElectiveRequire);
        Assert.Equal(vm.ElectiveRequireDisplay, domain.ElectiveRequireDisplay);
        Assert.Equal((int?)vm.ElectiveRequireValue, (int?)domain.ElectiveRequireValue);
        Assert.Equal(vm.Name, domain.Name);
        Assert.Equal(vm.Relationship, domain.Relationship);
        Assert.Equal(vm.PhoneNum, domain.PhoneNum);
    }

    [Fact]
    public void ToViewModel_MapsAllProperties()
    {
        var domain = _fixture.Create<DomainPersonalReference>();

        PersonalReferenceViewModel vm = domain.ToViewModel();

        Assert.Equal(domain.AllowElectiveRequire, vm.AllowElectiveRequire);
        Assert.Equal(domain.ElectiveRequireDisplay, vm.ElectiveRequireDisplay);
        Assert.Equal((int?)domain.ElectiveRequireValue, (int?)vm.ElectiveRequireValue);
        Assert.Equal(domain.Name, vm.Name);
        Assert.Equal(domain.Relationship, vm.Relationship);
        Assert.Equal(domain.PhoneNum, vm.PhoneNum);
    }
}
