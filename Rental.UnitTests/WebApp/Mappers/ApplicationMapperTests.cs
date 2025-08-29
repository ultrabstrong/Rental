using Rental.WebApp.Mappers;
using ApplicationViewModel = Rental.WebApp.Models.Application.Application;
using DomainApplication = Rental.Domain.Applications.Models.RentalApplication;

namespace Rental.UnitTests.WebApp.Mappers;

public class ApplicationMapperTests
{
    private readonly AutoFixture.Fixture _fixture = new();

    [Fact]
    public void ToDomainModel_MapsTopLevelAndNested()
    {
        ApplicationViewModel vm = ApplicationViewModel.TestApplication; // use existing test sample for realistic data

        DomainApplication domain = vm.ToDomainModel();

        Assert.Equal(vm.RentalAddress, domain.RentalAddress);
        Assert.Equal(vm.OtherApplicants, domain.OtherApplicants);
        Assert.Equal(vm.PersonalInfo.FirstName, domain.PersonalInfo.FirstName);
        Assert.Equal(vm.PrimaryEmployment.Company, domain.PrimaryEmployment.Company);
        Assert.Equal((int?)vm.PrimaryEmployment.ElectiveRequireValue, (int?)domain.PrimaryEmployment.ElectiveRequireValue);
        Assert.Equal(vm.Automobile.Make, domain.Automobile.Make);
        Assert.Equal(vm.CurrentRental.Street, domain.CurrentRental.Street);
        Assert.Equal(vm.PriorRentRef1.Street, domain.PriorRentRef1.Street);
        Assert.Equal(vm.PersonalReference1.Name, domain.PersonalReference1.Name);
        Assert.Equal(vm.PersonalReference2.Name, domain.PersonalReference2.Name);
        Assert.Equal(vm.AnticipatedDuration, domain.AnticipatedDuration);
        Assert.Equal((int?)vm.HasCriminalRecord, (int?)domain.HasCriminalRecord);
        Assert.Equal(vm.ExplainCriminalRecord, domain.ExplainCriminalRecord);
        Assert.Equal((int?)vm.Smokers, (int?)domain.Smokers);
        Assert.Equal(vm.SmokersCount, domain.SmokersCount);
        Assert.Equal((int?)vm.HowOftenDrink, (int?)domain.HowOftenDrink);
        Assert.Equal((int?)vm.CertificationAndAuthorization, (int?)domain.CertificationAndAuthorization);
    }
}
