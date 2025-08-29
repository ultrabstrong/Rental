using AutoFixture;
using Rental.WebApp.Mappers;
using EmploymentInfoViewModel = Rental.WebApp.Models.Application.EmploymentInfo;
using DomainEmploymentInfo = Rental.Domain.Applications.Models.EmploymentInfo;

namespace Rental.UnitTests.WebApp.Mappers;

public class EmploymentInfoMapperTests
{
    private readonly Fixture _fixture = new();

    [Fact]
    public void ToDomainModel_MapsAllProperties()
    {
        var vm = _fixture.Create<EmploymentInfoViewModel>();

        DomainEmploymentInfo domain = vm.ToDomainModel();

        Assert.Equal(vm.AllowElectiveRequire, domain.AllowElectiveRequire);
        Assert.Equal((int?)vm.ElectiveRequireValue, (int?)domain.ElectiveRequireValue);
        Assert.Equal(vm.Company, domain.Company);
        Assert.Equal(vm.ContactName, domain.ContactName);
        Assert.Equal(vm.ContactPhone, domain.ContactPhone);
        Assert.Equal(vm.EmploymentLength, domain.EmploymentLength);
        Assert.Equal((int?)vm.IsPermenant, (int?)domain.IsPermenant);
        Assert.Equal((int?)vm.WageType, (int?)domain.WageType);
        Assert.Equal(vm.Wage, domain.Wage);
        Assert.Equal(vm.HoursPerWeek, domain.HoursPerWeek);
    }

    [Fact]
    public void ToViewModel_MapsAllProperties()
    {
        var domain = _fixture.Create<DomainEmploymentInfo>();

        EmploymentInfoViewModel vm = domain.ToViewModel();

        Assert.Equal(domain.AllowElectiveRequire, vm.AllowElectiveRequire);
        Assert.Equal(domain.ElectiveRequireDisplay, vm.ElectiveRequireDisplay);
        Assert.Equal((int?)domain.ElectiveRequireValue, (int?)vm.ElectiveRequireValue);
        Assert.Equal(domain.Company, vm.Company);
        Assert.Equal(domain.ContactName, vm.ContactName);
        Assert.Equal(domain.ContactPhone, vm.ContactPhone);
        Assert.Equal(domain.EmploymentLength, vm.EmploymentLength);
        Assert.Equal((int?)domain.IsPermenant, (int?)vm.IsPermenant);
        Assert.Equal((int?)domain.WageType, (int?)vm.WageType);
        Assert.Equal(domain.Wage, vm.Wage);
        Assert.Equal(domain.HoursPerWeek, vm.HoursPerWeek);
    }
}
