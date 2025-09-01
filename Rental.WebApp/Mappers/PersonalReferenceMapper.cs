using Rental.Domain.Applications.Models;
using Rental.Domain.Enums;
using PersonalReferenceViewModel = Rental.WebApp.Models.Application.PersonalReference;
using YesNoViewModel = Rental.WebApp.Enums.YesNo;

namespace Rental.WebApp.Mappers;

public static class PersonalReferenceMapper
{
    public static PersonalReference ToDomainModel(this PersonalReferenceViewModel src) => new(
        AllowElectiveRequire: src.AllowElectiveRequire,
        ElectiveRequireDisplay: src.ElectiveRequireDisplay,
        ElectiveRequireValue: src.ElectiveRequireValue.MapEnum<YesNoViewModel, YesNo>(),
        Name: src.Name,
        Relationship: src.Relationship,
        PhoneNum: src.PhoneNum
    );

    public static PersonalReferenceViewModel ToViewModel(this PersonalReference src) => new()
    {
        AllowElectiveRequire = src.AllowElectiveRequire,
        ElectiveRequireDisplay = src.ElectiveRequireDisplay,
        ElectiveRequireValue = src.ElectiveRequireValue.MapEnum<YesNo, YesNoViewModel>(),
        Name = src.Name,
        Relationship = src.Relationship,
        PhoneNum = src.PhoneNum
    };
}
