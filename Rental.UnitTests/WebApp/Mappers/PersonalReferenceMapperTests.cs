using AutoFixture;
using Rental.WebApp.Mappers;
using PersonalReferenceViewModel = Rental.WebApp.Models.Application.PersonalReference;
using DomainPersonalReference = Rental.Domain.Applications.PersonalReference;

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
}
