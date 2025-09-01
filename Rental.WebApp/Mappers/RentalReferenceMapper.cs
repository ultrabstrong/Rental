using Rental.Domain.Applications.Models;
using Rental.Domain.Enums;
using RentalReferenceViewModel = Rental.WebApp.Models.Application.RentalReference;
using YesNoViewModel = Rental.WebApp.Enums.YesNo;

namespace Rental.WebApp.Mappers;

public static class RentalReferenceMapper
{
    public static RentalReference ToDomainModel(this RentalReferenceViewModel src) => new(
        AllowElectiveRequire: src.AllowElectiveRequire,
        ElectiveRequireDisplay: src.ElectiveRequireDisplay,
        ElectiveRequireValue: src.ElectiveRequireValue.MapEnum<YesNoViewModel, YesNo>(),
        Street: src.Street,
        City: src.City,
        State: src.State,
        Zip: src.Zip,
        LandlordName: src.LandlordName,
        LandlordPhoneNum: src.LandlordPhoneNum,
        Start: src.Start,
        End: src.End,
        ReasonForMoving: src.ReasonForMoving
    );

    public static RentalReferenceViewModel ToViewModel(this RentalReference src) => new()
    {
        AllowElectiveRequire = src.AllowElectiveRequire,
        ElectiveRequireDisplay = src.ElectiveRequireDisplay,
        ElectiveRequireValue = src.ElectiveRequireValue.MapEnum<YesNo, YesNoViewModel>(),
        Street = src.Street,
        City = src.City,
        State = src.State,
        Zip = src.Zip,
        LandlordName = src.LandlordName,
        LandlordPhoneNum = src.LandlordPhoneNum,
        Start = src.Start,
        End = src.End,
        ReasonForMoving = src.ReasonForMoving
    };
}
