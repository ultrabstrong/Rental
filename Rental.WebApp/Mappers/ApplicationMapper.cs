using Rental.Domain.Applications.Models;
using Rental.Domain.Enums;
using ApplicationViewModel = Rental.WebApp.Models.Application.Application;
using HowOftenViewModel = Rental.WebApp.Enums.HowOften;
using YesNoViewModel = Rental.WebApp.Enums.YesNo;
using YesViewModel = Rental.WebApp.Enums.Yes;

namespace Rental.WebApp.Mappers;

public static class ApplicationMapper
{
    public static RentalApplication ToDomainModel(this ApplicationViewModel src) => new()
    {
        RentalAddress = src.RentalAddress,
        OtherApplicants = src.OtherApplicants,
        PersonalInfo = src.PersonalInfo.ToDomainModel(),
        CurrentRental = src.CurrentRental.ToDomainModel(),
        PrimaryEmployment = src.PrimaryEmployment.ToDomainModel(),
        SecondaryEmployment = src.SecondaryEmployment.ToDomainModel(),
        ParentInfo = src.ParentInfo.ToDomainModel(),
        ConsiderOtherIncome = src.ConsiderOtherIncome.MapEnum<YesNoViewModel, YesNo>(),
        OtherIncomeExplain = src.OtherIncomeExplain,
        Automobile = src.Automobile.ToDomainModel(),
        PriorRentRef1 = src.PriorRentRef1.ToDomainModel(),
        PersonalReference1 = src.PersonalReference1.ToDomainModel(),
        PersonalReference2 = src.PersonalReference2.ToDomainModel(),
        AnticipatedDuration = src.AnticipatedDuration,
        HasCriminalRecord = src.HasCriminalRecord.MapEnum<YesNoViewModel, YesNo>(),
        ExplainCriminalRecord = src.ExplainCriminalRecord,
        HasBeenEvicted = src.HasBeenEvicted.MapEnum<YesNoViewModel, YesNo>(),
        ExplainBeenEvicted = src.ExplainBeenEvicted,
        MarijuanaCard = src.MarijuanaCard.MapEnum<YesNoViewModel, YesNo>(),
        Smokers = src.Smokers.MapEnum<YesNoViewModel, YesNo>(),
        SmokersCount = src.SmokersCount,
        Drinkers = src.Drinkers.MapEnum<YesNoViewModel, YesNo>(),
        HowOftenDrink = src.HowOftenDrink.MapEnum<HowOftenViewModel, HowOften>(),
        AnyPets = src.AnyPets.MapEnum<YesNoViewModel, YesNo>(),
        DescribePets = src.DescribePets,
        AnyNonHuman = src.AnyNonHuman.MapEnum<YesNoViewModel, YesNo>(),
        DescribeNonHuman = src.DescribeNonHuman,
        AttendCollege = src.AttendCollege.MapEnum<YesNoViewModel, YesNo>(),
        CollegeYearsAttended = src.CollegeYearsAttended,
        PlanToGraduate = src.PlanToGraduate,
        NeedReasonableAccommodation = src.NeedReasonableAccommodation.MapEnum<YesNoViewModel, YesNo>(),
        DescribeReasonableAccommodation = src.DescribeReasonableAccommodation,
        CertificationAndAuthorization = src.CertificationAndAuthorization.MapEnum<YesViewModel, Yes>(),
        AdditionalComments = src.AdditionalComments
    };
}
