using Rental.Domain.Enums;

namespace Rental.Domain.Applications.Models;

public class RentalApplication
{
    public string RentalAddress { get; set; } = string.Empty;
    public string? OtherApplicants { get; set; }
    public PersonalInfo PersonalInfo { get; set; } = new();
    public RentalReference CurrentRental { get; set; } = new();
    public EmploymentInfo PrimaryEmployment { get; set; } = new();
    public EmploymentInfo SecondaryEmployment { get; set; } = new();
    public ParentInfo ParentInfo { get; set; } = new();
    public YesNo? ConsiderOtherIncome { get; set; }
    public string? OtherIncomeExplain { get; set; }
    public Automobile Automobile { get; set; } = new();
    public RentalReference PriorRentRef1 { get; set; } = new();
    public PersonalReference PersonalReference1 { get; set; } = new();
    public PersonalReference PersonalReference2 { get; set; } = new();
    public string AnticipatedDuration { get; set; } = string.Empty;
    public YesNo? HasCriminalRecord { get; set; }
    public string? ExplainCriminalRecord { get; set; }
    public YesNo? HasBeenEvicted { get; set; }
    public string? ExplainBeenEvicted { get; set; }
    public YesNo? MarijuanaCard { get; set; }
    public YesNo? Smokers { get; set; }
    public int? SmokersCount { get; set; }
    public YesNo? Drinkers { get; set; }
    public HowOften? HowOftenDrink { get; set; }
    public YesNo? AnyPets { get; set; }
    public string? DescribePets { get; set; }
    public YesNo? AnyNonHuman { get; set; }
    public string? DescribeNonHuman { get; set; }
    public YesNo? AttendCollege { get; set; }
    public int? CollegeYearsAttended { get; set; }
    public string? PlanToGraduate { get; set; }
    public YesNo? NeedReasonableAccommodation { get; set; }
    public string? DescribeReasonableAccommodation { get; set; }
    public bool AcceptedTerms { get; set; }
    public string? AdditionalComments { get; set; }
}
