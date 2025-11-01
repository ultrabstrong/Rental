using Rental.Domain.Applications.Models;
using Rental.Domain.Defang;

namespace Rental.UnitTests.Domain.Defang;

public class RentalApplicationDefangerTests
{
    const string DANGEROUS_INPUT = "<script>alert('xss')</script> http://example.com";
    const string SAFE_OUTPUT = "http[colon]//example[dot]com";

    private readonly IDefanger _d = new WebInputDefanger();

    private RentalApplicationDefanger Create() => new(_d);

    [Fact]
    public void Defang_All_String_Fields_Are_Processed()
    {
        var sut = Create();
        var app = new RentalApplication(
            RentalAddress: DANGEROUS_INPUT,
            OtherApplicants: DANGEROUS_INPUT,
            PersonalInfo: new(
                FirstName: DANGEROUS_INPUT,
                MiddleName: DANGEROUS_INPUT,
                LastName: DANGEROUS_INPUT,
                PhoneNum: DANGEROUS_INPUT,
                SSN: DANGEROUS_INPUT,
                DriverLicense: DANGEROUS_INPUT,
                DriverLicenseStateOfIssue: DANGEROUS_INPUT,
                Email: DANGEROUS_INPUT
            ),
            CurrentRental: new(
                AllowElectiveRequire: true,
                ElectiveRequireDisplay: DANGEROUS_INPUT,
                ElectiveRequireValue: Rental.Domain.Enums.YesNo.Yes,
                Street: DANGEROUS_INPUT,
                City: DANGEROUS_INPUT,
                State: DANGEROUS_INPUT,
                Zip: DANGEROUS_INPUT,
                LandlordName: DANGEROUS_INPUT,
                LandlordPhoneNum: DANGEROUS_INPUT,
                Start: DateTime.UtcNow.AddYears(-1),
                End: DateTime.UtcNow,
                ReasonForMoving: DANGEROUS_INPUT
            ),
            PrimaryEmployment: new(
                AllowElectiveRequire: true,
                ElectiveRequireDisplay: DANGEROUS_INPUT,
                ElectiveRequireValue: Rental.Domain.Enums.YesNo.Yes,
                Company: DANGEROUS_INPUT,
                ContactName: DANGEROUS_INPUT,
                ContactPhone: DANGEROUS_INPUT,
                EmploymentLength: DANGEROUS_INPUT,
                IsPermenant: Rental.Domain.Enums.YesNo.Yes,
                WageType: Rental.Domain.Enums.WageType.Hourly,
                Wage: 20,
                HoursPerWeek: 40
            ),
            SecondaryEmployment: new(
                AllowElectiveRequire: true,
                ElectiveRequireDisplay: DANGEROUS_INPUT,
                ElectiveRequireValue: Rental.Domain.Enums.YesNo.Yes,
                Company: DANGEROUS_INPUT,
                ContactName: DANGEROUS_INPUT,
                ContactPhone: DANGEROUS_INPUT,
                EmploymentLength: DANGEROUS_INPUT,
                IsPermenant: Rental.Domain.Enums.YesNo.Yes,
                WageType: Rental.Domain.Enums.WageType.Hourly,
                Wage: 10,
                HoursPerWeek: 10
            ),
            ParentInfo: new(
                ElectiveRequireValue: Rental.Domain.Enums.YesNo.Yes,
                FirstName: DANGEROUS_INPUT,
                MiddleName: DANGEROUS_INPUT,
                LastName: DANGEROUS_INPUT,
                PhoneNum: DANGEROUS_INPUT,
                Street: DANGEROUS_INPUT,
                City: DANGEROUS_INPUT,
                State: DANGEROUS_INPUT,
                Zip: DANGEROUS_INPUT
            ),
            ConsiderOtherIncome: Rental.Domain.Enums.YesNo.Yes,
            OtherIncomeExplain: DANGEROUS_INPUT,
            Automobile: new(
                true,
                Rental.Domain.Enums.YesNo.Yes,
                DANGEROUS_INPUT,
                DANGEROUS_INPUT,
                DANGEROUS_INPUT,
                DANGEROUS_INPUT,
                DANGEROUS_INPUT,
                DANGEROUS_INPUT
            ),
            PriorRentRef1: new(
                AllowElectiveRequire: true,
                ElectiveRequireDisplay: DANGEROUS_INPUT,
                ElectiveRequireValue: Rental.Domain.Enums.YesNo.Yes,
                Street: DANGEROUS_INPUT,
                City: DANGEROUS_INPUT,
                State: DANGEROUS_INPUT,
                Zip: DANGEROUS_INPUT,
                LandlordName: DANGEROUS_INPUT,
                LandlordPhoneNum: DANGEROUS_INPUT,
                Start: DateTime.UtcNow.AddYears(-2),
                End: DateTime.UtcNow.AddYears(-1),
                ReasonForMoving: DANGEROUS_INPUT
            ),
            PersonalReference1: new(
                true,
                DANGEROUS_INPUT,
                Rental.Domain.Enums.YesNo.Yes,
                DANGEROUS_INPUT,
                DANGEROUS_INPUT,
                DANGEROUS_INPUT
            ),
            PersonalReference2: new(
                true,
                DANGEROUS_INPUT,
                Rental.Domain.Enums.YesNo.Yes,
                DANGEROUS_INPUT,
                DANGEROUS_INPUT,
                DANGEROUS_INPUT
            ),
            AnticipatedDuration: DANGEROUS_INPUT,
            HasCriminalRecord: Rental.Domain.Enums.YesNo.Yes,
            ExplainCriminalRecord: DANGEROUS_INPUT,
            HasBeenEvicted: Rental.Domain.Enums.YesNo.Yes,
            ExplainBeenEvicted: DANGEROUS_INPUT,
            MarijuanaCard: Rental.Domain.Enums.YesNo.No,
            Smokers: Rental.Domain.Enums.YesNo.No,
            SmokersCount: 0,
            Drinkers: Rental.Domain.Enums.YesNo.No,
            HowOftenDrink: null,
            AnyPets: Rental.Domain.Enums.YesNo.Yes,
            DescribePets: DANGEROUS_INPUT,
            AnyNonHuman: Rental.Domain.Enums.YesNo.Yes,
            DescribeNonHuman: DANGEROUS_INPUT,
            AttendCollege: Rental.Domain.Enums.YesNo.No,
            CollegeYearsAttended: null,
            PlanToGraduate: DANGEROUS_INPUT,
            NeedReasonableAccommodation: Rental.Domain.Enums.YesNo.No,
            DescribeReasonableAccommodation: DANGEROUS_INPUT,
            AcceptedTerms: true,
            AdditionalComments: DANGEROUS_INPUT
        );

        var result = sut.Defang(app);

        Assert.Multiple(() =>
        {
            // top-level strings
            Assert.Equal(SAFE_OUTPUT, result.RentalAddress);
            Assert.Equal(SAFE_OUTPUT, result.OtherApplicants);
            Assert.Equal(SAFE_OUTPUT, result.OtherIncomeExplain);
            Assert.Equal(SAFE_OUTPUT, result.AnticipatedDuration);
            Assert.Equal(SAFE_OUTPUT, result.ExplainCriminalRecord);
            Assert.Equal(SAFE_OUTPUT, result.ExplainBeenEvicted);
            Assert.Equal(SAFE_OUTPUT, result.DescribePets);
            Assert.Equal(SAFE_OUTPUT, result.DescribeNonHuman);
            Assert.Equal(SAFE_OUTPUT, result.PlanToGraduate);
            Assert.Equal(SAFE_OUTPUT, result.DescribeReasonableAccommodation);
            Assert.Equal(SAFE_OUTPUT, result.AdditionalComments);

            // PersonalInfo strings
            Assert.Equal(SAFE_OUTPUT, result.PersonalInfo.FirstName);
            Assert.Equal(SAFE_OUTPUT, result.PersonalInfo.MiddleName);
            Assert.Equal(SAFE_OUTPUT, result.PersonalInfo.LastName);
            Assert.Equal(SAFE_OUTPUT, result.PersonalInfo.PhoneNum);
            Assert.Equal(SAFE_OUTPUT, result.PersonalInfo.SSN);
            Assert.Equal(SAFE_OUTPUT, result.PersonalInfo.DriverLicense);
            Assert.Equal(SAFE_OUTPUT, result.PersonalInfo.DriverLicenseStateOfIssue);
            Assert.Equal(SAFE_OUTPUT, result.PersonalInfo.Email);

            // CurrentRental strings
            Assert.Equal(SAFE_OUTPUT, result.CurrentRental.ElectiveRequireDisplay);
            Assert.Equal(SAFE_OUTPUT, result.CurrentRental.Street);
            Assert.Equal(SAFE_OUTPUT, result.CurrentRental.City);
            Assert.Equal(SAFE_OUTPUT, result.CurrentRental.State);
            Assert.Equal(SAFE_OUTPUT, result.CurrentRental.Zip);
            Assert.Equal(SAFE_OUTPUT, result.CurrentRental.LandlordName);
            Assert.Equal(SAFE_OUTPUT, result.CurrentRental.LandlordPhoneNum);
            Assert.Equal(SAFE_OUTPUT, result.CurrentRental.ReasonForMoving);

            // PrimaryEmployment strings
            Assert.Equal(SAFE_OUTPUT, result.PrimaryEmployment.ElectiveRequireDisplay);
            Assert.Equal(SAFE_OUTPUT, result.PrimaryEmployment.Company);
            Assert.Equal(SAFE_OUTPUT, result.PrimaryEmployment.ContactName);
            Assert.Equal(SAFE_OUTPUT, result.PrimaryEmployment.ContactPhone);
            Assert.Equal(SAFE_OUTPUT, result.PrimaryEmployment.EmploymentLength);

            // SecondaryEmployment strings
            Assert.Equal(SAFE_OUTPUT, result.SecondaryEmployment.ElectiveRequireDisplay);
            Assert.Equal(SAFE_OUTPUT, result.SecondaryEmployment.Company);
            Assert.Equal(SAFE_OUTPUT, result.SecondaryEmployment.ContactName);
            Assert.Equal(SAFE_OUTPUT, result.SecondaryEmployment.ContactPhone);
            Assert.Equal(SAFE_OUTPUT, result.SecondaryEmployment.EmploymentLength);

            // ParentInfo strings
            Assert.Equal(SAFE_OUTPUT, result.ParentInfo.FirstName);
            Assert.Equal(SAFE_OUTPUT, result.ParentInfo.MiddleName);
            Assert.Equal(SAFE_OUTPUT, result.ParentInfo.LastName);
            Assert.Equal(SAFE_OUTPUT, result.ParentInfo.PhoneNum);
            Assert.Equal(SAFE_OUTPUT, result.ParentInfo.Street);
            Assert.Equal(SAFE_OUTPUT, result.ParentInfo.City);
            Assert.Equal(SAFE_OUTPUT, result.ParentInfo.State);
            Assert.Equal(SAFE_OUTPUT, result.ParentInfo.Zip);

            // Automobile strings
            Assert.Equal(SAFE_OUTPUT, result.Automobile.Make);
            Assert.Equal(SAFE_OUTPUT, result.Automobile.Model);
            Assert.Equal(SAFE_OUTPUT, result.Automobile.Year);
            Assert.Equal(SAFE_OUTPUT, result.Automobile.State);
            Assert.Equal(SAFE_OUTPUT, result.Automobile.LicenseNum);
            Assert.Equal(SAFE_OUTPUT, result.Automobile.Color);

            // Prior rental reference strings
            Assert.Equal(SAFE_OUTPUT, result.PriorRentRef1.ElectiveRequireDisplay);
            Assert.Equal(SAFE_OUTPUT, result.PriorRentRef1.Street);
            Assert.Equal(SAFE_OUTPUT, result.PriorRentRef1.City);
            Assert.Equal(SAFE_OUTPUT, result.PriorRentRef1.State);
            Assert.Equal(SAFE_OUTPUT, result.PriorRentRef1.Zip);
            Assert.Equal(SAFE_OUTPUT, result.PriorRentRef1.LandlordName);
            Assert.Equal(SAFE_OUTPUT, result.PriorRentRef1.LandlordPhoneNum);
            Assert.Equal(SAFE_OUTPUT, result.PriorRentRef1.ReasonForMoving);

            // Personal references strings
            Assert.Equal(SAFE_OUTPUT, result.PersonalReference1.ElectiveRequireDisplay);
            Assert.Equal(SAFE_OUTPUT, result.PersonalReference1.Name);
            Assert.Equal(SAFE_OUTPUT, result.PersonalReference1.Relationship);
            Assert.Equal(SAFE_OUTPUT, result.PersonalReference1.PhoneNum);

            Assert.Equal(SAFE_OUTPUT, result.PersonalReference2.ElectiveRequireDisplay);
            Assert.Equal(SAFE_OUTPUT, result.PersonalReference2.Name);
            Assert.Equal(SAFE_OUTPUT, result.PersonalReference2.Relationship);
            Assert.Equal(SAFE_OUTPUT, result.PersonalReference2.PhoneNum);
        });
    }
}
