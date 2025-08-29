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
}
