using AutoFixture;
using Rental.WebApp.Mappers;
using ParentInfoViewModel = Rental.WebApp.Models.Application.ParentInfo;
using DomainParentInfo = Rental.Domain.Applications.Models.ParentInfo;

namespace Rental.UnitTests.WebApp.Mappers;

public class ParentInfoMapperTests
{
    private readonly Fixture _fixture = new();

    [Fact]
    public void ToDomainModel_MapsAllProperties()
    {
        var vm = _fixture.Create<ParentInfoViewModel>();

        DomainParentInfo domain = vm.ToDomainModel();

        Assert.Equal((int?)vm.ElectiveRequireValue, (int?)domain.ElectiveRequireValue);
        Assert.Equal(vm.FirstName, domain.FirstName);
        Assert.Equal(vm.MiddleName, domain.MiddleName);
        Assert.Equal(vm.LastName, domain.LastName);
        Assert.Equal(vm.PhoneNum, domain.PhoneNum);
        Assert.Equal(vm.Street, domain.Street);
        Assert.Equal(vm.City, domain.City);
        Assert.Equal(vm.State, domain.State);
        Assert.Equal(vm.Zip, domain.Zip);
    }
}
