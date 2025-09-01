using Rental.Domain.Applications.Models;
using Rental.Domain.Enums;
using ApplicationViewModel = Rental.WebApp.Models.Application.RentalApplication;
using HowOftenViewModel = Rental.WebApp.Enums.HowOften;
using YesNoViewModel = Rental.WebApp.Enums.YesNo;

namespace Rental.WebApp.Mappers;

public static class ApplicationMapper
{
    public static RentalApplication ToDomainModel(this ApplicationViewModel src) => new(
        RentalAddress: src.RentalAddress,
        OtherApplicants: src.OtherApplicants,
        PersonalInfo: src.PersonalInfo.ToDomainModel(),
        CurrentRental: src.CurrentRental.ToDomainModel(),
        PrimaryEmployment: src.PrimaryEmployment.ToDomainModel(),
        SecondaryEmployment: src.SecondaryEmployment.ToDomainModel(),
        ParentInfo: src.ParentInfo.ToDomainModel(),
        ConsiderOtherIncome: src.ConsiderOtherIncome.MapEnum<YesNoViewModel, YesNo>(),
        OtherIncomeExplain: src.OtherIncomeExplain,
        Automobile: src.Automobile.ToDomainModel(),
        PriorRentRef1: src.PriorRentRef1.ToDomainModel(),
        PersonalReference1: src.PersonalReference1.ToDomainModel(),
        PersonalReference2: src.PersonalReference2.ToDomainModel(),
        AnticipatedDuration: src.AnticipatedDuration,
        HasCriminalRecord: src.HasCriminalRecord.MapEnum<YesNoViewModel, YesNo>(),
        ExplainCriminalRecord: src.ExplainCriminalRecord,
        HasBeenEvicted: src.HasBeenEvicted.MapEnum<YesNoViewModel, YesNo>(),
        ExplainBeenEvicted: src.ExplainBeenEvicted,
        MarijuanaCard: src.MarijuanaCard.MapEnum<YesNoViewModel, YesNo>(),
        Smokers: src.Smokers.MapEnum<YesNoViewModel, YesNo>(),
        SmokersCount: src.SmokersCount,
        Drinkers: src.Drinkers.MapEnum<YesNoViewModel, YesNo>(),
        HowOftenDrink: src.HowOftenDrink.MapEnum<HowOftenViewModel, HowOften>(),
        AnyPets: src.AnyPets.MapEnum<YesNoViewModel, YesNo>(),
        DescribePets: src.DescribePets,
        AnyNonHuman: src.AnyNonHuman.MapEnum<YesNoViewModel, YesNo>(),
        DescribeNonHuman: src.DescribeNonHuman,
        AttendCollege: src.AttendCollege.MapEnum<YesNoViewModel, YesNo>(),
        CollegeYearsAttended: src.CollegeYearsAttended,
        PlanToGraduate: src.PlanToGraduate,
        NeedReasonableAccommodation: src.NeedReasonableAccommodation.MapEnum<YesNoViewModel, YesNo>(),
        DescribeReasonableAccommodation: src.DescribeReasonableAccommodation,
        AcceptedTerms: src.AcceptedTerms,
        AdditionalComments: src.AdditionalComments
    );

    public static ApplicationViewModel ToViewModel(this RentalApplication src) => new()
    {
        RentalAddress = src.RentalAddress,
        OtherApplicants = src.OtherApplicants,
        PersonalInfo = src.PersonalInfo.ToViewModel(),
        CurrentRental = src.CurrentRental.ToViewModel(),
        PrimaryEmployment = src.PrimaryEmployment.ToViewModel(),
        SecondaryEmployment = src.SecondaryEmployment.ToViewModel(),
        ParentInfo = src.ParentInfo.ToViewModel(),
        ConsiderOtherIncome = src.ConsiderOtherIncome.MapEnum<YesNo, YesNoViewModel>(),
        OtherIncomeExplain = src.OtherIncomeExplain,
        Automobile = src.Automobile.ToViewModel(),
        PriorRentRef1 = src.PriorRentRef1.ToViewModel(),
        PersonalReference1 = src.PersonalReference1.ToViewModel(),
        PersonalReference2 = src.PersonalReference2.ToViewModel(),
        AnticipatedDuration = src.AnticipatedDuration,
        HasCriminalRecord = src.HasCriminalRecord.MapEnum<YesNo, YesNoViewModel>(),
        ExplainCriminalRecord = src.ExplainCriminalRecord,
        HasBeenEvicted = src.HasBeenEvicted.MapEnum<YesNo, YesNoViewModel>(),
        ExplainBeenEvicted = src.ExplainBeenEvicted,
        MarijuanaCard = src.MarijuanaCard.MapEnum<YesNo, YesNoViewModel>(),
        Smokers = src.Smokers.MapEnum<YesNo, YesNoViewModel>(),
        SmokersCount = src.SmokersCount,
        Drinkers = src.Drinkers.MapEnum<YesNo, YesNoViewModel>(),
        HowOftenDrink = src.HowOftenDrink.MapEnum<HowOften, HowOftenViewModel>(),
        AnyPets = src.AnyPets.MapEnum<YesNo, YesNoViewModel>(),
        DescribePets = src.DescribePets,
        AnyNonHuman = src.AnyNonHuman.MapEnum<YesNo, YesNoViewModel>(),
        DescribeNonHuman = src.DescribeNonHuman,
        AttendCollege = src.AttendCollege.MapEnum<YesNo, YesNoViewModel>(),
        CollegeYearsAttended = src.CollegeYearsAttended,
        PlanToGraduate = src.PlanToGraduate,
        NeedReasonableAccommodation = src.NeedReasonableAccommodation.MapEnum<YesNo, YesNoViewModel>(),
        DescribeReasonableAccommodation = src.DescribeReasonableAccommodation,
        AcceptedTerms = src.AcceptedTerms,
        AdditionalComments = src.AdditionalComments
    };
}
