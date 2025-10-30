using AutoFixture;
using Rental.WebApp.Mappers;
using DomainParentInfo = Rental.Domain.Applications.Models.ParentInfo;
using ParentInfoViewModel = Rental.WebApp.Models.Application.ParentInfo;

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

    [Fact]
    public void ToViewModel_MapsAllProperties()
    {
        var domain = _fixture.Create<DomainParentInfo>();

        ParentInfoViewModel vm = domain.ToViewModel();

        Assert.Equal((int?)domain.ElectiveRequireValue, (int?)vm.ElectiveRequireValue);
        Assert.Equal(domain.FirstName, vm.FirstName);
        Assert.Equal(domain.MiddleName, vm.MiddleName);
        Assert.Equal(domain.LastName, vm.LastName);
        Assert.Equal(domain.PhoneNum, vm.PhoneNum);
        Assert.Equal(domain.Street, vm.Street);
        Assert.Equal(domain.City, vm.City);
        Assert.Equal(domain.State, vm.State);
        Assert.Equal(domain.Zip, vm.Zip);
    }
}
