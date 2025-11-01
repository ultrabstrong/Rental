using Rental.Domain.Applications.Models;

namespace Rental.Domain.Defang;

internal class RentalApplicationDefanger(IDefanger d) : IRentalApplicationDefanger
{
    private readonly IDefanger _d = d;

    public RentalApplication Defang(RentalApplication app)
    {
        return new RentalApplication(
            RentalAddress: _d.Defang(app.RentalAddress),
            OtherApplicants: _d.Defang(app.OtherApplicants),
            PersonalInfo: Defang(app.PersonalInfo),
            CurrentRental: Defang(app.CurrentRental),
            PrimaryEmployment: Defang(app.PrimaryEmployment),
            SecondaryEmployment: Defang(app.SecondaryEmployment),
            ParentInfo: Defang(app.ParentInfo),
            ConsiderOtherIncome: app.ConsiderOtherIncome,
            OtherIncomeExplain: _d.Defang(app.OtherIncomeExplain),
            Automobile: Defang(app.Automobile),
            PriorRentRef1: Defang(app.PriorRentRef1),
            PersonalReference1: Defang(app.PersonalReference1),
            PersonalReference2: Defang(app.PersonalReference2),
            AnticipatedDuration: _d.Defang(app.AnticipatedDuration),
            HasCriminalRecord: app.HasCriminalRecord,
            ExplainCriminalRecord: _d.Defang(app.ExplainCriminalRecord),
            HasBeenEvicted: app.HasBeenEvicted,
            ExplainBeenEvicted: _d.Defang(app.ExplainBeenEvicted),
            MarijuanaCard: app.MarijuanaCard,
            Smokers: app.Smokers,
            SmokersCount: app.SmokersCount,
            Drinkers: app.Drinkers,
            HowOftenDrink: app.HowOftenDrink,
            AnyPets: app.AnyPets,
            DescribePets: _d.Defang(app.DescribePets),
            AnyNonHuman: app.AnyNonHuman,
            DescribeNonHuman: _d.Defang(app.DescribeNonHuman),
            AttendCollege: app.AttendCollege,
            CollegeYearsAttended: app.CollegeYearsAttended,
            PlanToGraduate: _d.Defang(app.PlanToGraduate),
            NeedReasonableAccommodation: app.NeedReasonableAccommodation,
            DescribeReasonableAccommodation: _d.Defang(app.DescribeReasonableAccommodation),
            AcceptedTerms: app.AcceptedTerms,
            AdditionalComments: _d.Defang(app.AdditionalComments)
        );
    }

    private PersonalInfo Defang(PersonalInfo p) =>
        new(
            FirstName: _d.Defang(p.FirstName),
            MiddleName: _d.Defang(p.MiddleName),
            LastName: _d.Defang(p.LastName),
            PhoneNum: _d.Defang(p.PhoneNum),
            SSN: _d.Defang(p.SSN),
            DriverLicense: _d.Defang(p.DriverLicense),
            DriverLicenseStateOfIssue: _d.Defang(p.DriverLicenseStateOfIssue),
            Email: _d.Defang(p.Email)
        );

    private RentalReference Defang(RentalReference r) =>
        new(
            AllowElectiveRequire: r.AllowElectiveRequire,
            ElectiveRequireDisplay: _d.Defang(r.ElectiveRequireDisplay),
            ElectiveRequireValue: r.ElectiveRequireValue,
            Street: _d.Defang(r.Street),
            City: _d.Defang(r.City),
            State: _d.Defang(r.State),
            Zip: _d.Defang(r.Zip),
            LandlordName: _d.Defang(r.LandlordName),
            LandlordPhoneNum: _d.Defang(r.LandlordPhoneNum),
            Start: r.Start,
            End: r.End,
            ReasonForMoving: _d.Defang(r.ReasonForMoving)
        );

    private EmploymentInfo Defang(EmploymentInfo e) =>
        new(
            AllowElectiveRequire: e.AllowElectiveRequire,
            ElectiveRequireDisplay: _d.Defang(e.ElectiveRequireDisplay),
            ElectiveRequireValue: e.ElectiveRequireValue,
            Company: _d.Defang(e.Company),
            ContactName: _d.Defang(e.ContactName),
            ContactPhone: _d.Defang(e.ContactPhone),
            EmploymentLength: _d.Defang(e.EmploymentLength),
            IsPermenant: e.IsPermenant,
            WageType: e.WageType,
            Wage: e.Wage,
            HoursPerWeek: e.HoursPerWeek
        );

    private ParentInfo Defang(ParentInfo p) =>
        new(
            ElectiveRequireValue: p.ElectiveRequireValue,
            FirstName: _d.Defang(p.FirstName),
            MiddleName: _d.Defang(p.MiddleName),
            LastName: _d.Defang(p.LastName),
            PhoneNum: _d.Defang(p.PhoneNum),
            Street: _d.Defang(p.Street),
            City: _d.Defang(p.City),
            State: _d.Defang(p.State),
            Zip: _d.Defang(p.Zip)
        );

    private Automobile Defang(Automobile a) =>
        new(
            AllowElectiveRequire: a.AllowElectiveRequire,
            ElectiveRequireValue: a.ElectiveRequireValue,
            Make: _d.Defang(a.Make),
            Model: _d.Defang(a.Model),
            Year: _d.Defang(a.Year),
            State: _d.Defang(a.State),
            LicenseNum: _d.Defang(a.LicenseNum),
            Color: _d.Defang(a.Color)
        );

    private PersonalReference Defang(PersonalReference p) =>
        new(
            AllowElectiveRequire: p.AllowElectiveRequire,
            ElectiveRequireDisplay: _d.Defang(p.ElectiveRequireDisplay),
            ElectiveRequireValue: p.ElectiveRequireValue,
            Name: _d.Defang(p.Name),
            Relationship: _d.Defang(p.Relationship),
            PhoneNum: _d.Defang(p.PhoneNum)
        );
}
