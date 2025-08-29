using Rental.Domain.Applications.Models;
using Rental.Domain.Enums;
using AutomobileViewModel = Rental.WebApp.Models.Application.Automobile;
using YesNoViewModel = Rental.WebApp.Enums.YesNo;

namespace Rental.WebApp.Mappers;

public static class AutomobileMapper
{
    public static Automobile ToDomainModel(this AutomobileViewModel src) => new()
    {
        AllowElectiveRequire = src.AllowElectiveRequire,
        ElectiveRequireValue = src.ElectiveRequireValue.MapEnum<YesNoViewModel, YesNo>(),
        Make = src.Make,
        Model = src.Model,
        Year = src.Year,
        State = src.State,
        LicenseNum = src.LicenseNum,
        Color = src.Color
    };

    public static AutomobileViewModel ToViewModel(this Automobile src) => new()
    {
        AllowElectiveRequire = src.AllowElectiveRequire,
        ElectiveRequireValue = src.ElectiveRequireValue.MapEnum<YesNo, YesNoViewModel>(),
        Make = src.Make,
        Model = src.Model,
        Year = src.Year,
        State = src.State,
        LicenseNum = src.LicenseNum,
        Color = src.Color
    };
}
