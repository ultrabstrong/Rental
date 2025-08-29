using AutoFixture;
using Rental.WebApp.Mappers;
using PersonalInfoViewModel = Rental.WebApp.Models.Application.PersonalInfo;
using DomainPersonalInfo = Rental.Domain.Applications.Models.PersonalInfo;

namespace Rental.UnitTests.WebApp.Mappers;

public class PersonalInfoMapperTests
{
    private readonly Fixture _fixture = new();

    [Fact]
    public void ToDomainModel_MapsAllProperties()
    {
        var vm = _fixture.Create<PersonalInfoViewModel>();

        DomainPersonalInfo domain = vm.ToDomainModel();

        Assert.Equal(vm.FirstName, domain.FirstName);
        Assert.Equal(vm.MiddleName, domain.MiddleName);
        Assert.Equal(vm.LastName, domain.LastName);
        Assert.Equal(vm.PhoneNum, domain.PhoneNum);
        Assert.Equal(vm.SSN, domain.SSN);
        Assert.Equal(vm.DriverLicense, domain.DriverLicense);
        Assert.Equal(vm.DriverLicenseStateOfIssue, domain.DriverLicenseStateOfIssue);
        Assert.Equal(vm.Email, domain.Email);
    }

    [Fact]
    public void ToViewModel_MapsAllProperties()
    {
        var domain = _fixture.Create<DomainPersonalInfo>();

        PersonalInfoViewModel vm = domain.ToViewModel();

        Assert.Equal(domain.FirstName, vm.FirstName);
        Assert.Equal(domain.MiddleName, vm.MiddleName);
        Assert.Equal(domain.LastName, vm.LastName);
        Assert.Equal(domain.PhoneNum, vm.PhoneNum);
        Assert.Equal(domain.SSN, vm.SSN);
        Assert.Equal(domain.DriverLicense, vm.DriverLicense);
        Assert.Equal(domain.DriverLicenseStateOfIssue, vm.DriverLicenseStateOfIssue);
        Assert.Equal(domain.Email, vm.Email);
    }
}
