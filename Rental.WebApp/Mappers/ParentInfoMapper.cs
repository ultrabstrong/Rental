using Rental.Domain.Applications.Models;
using Rental.Domain.Enums;
using ParentInfoViewModel = Rental.WebApp.Models.Application.ParentInfo;
using YesNoViewModel = Rental.WebApp.Enums.YesNo;

namespace Rental.WebApp.Mappers;

internal static class ParentInfoMapper
{
    public static ParentInfo ToDomainModel(this ParentInfoViewModel src) =>
        new(
            ElectiveRequireValue: src.ElectiveRequireValue.MapEnum<YesNoViewModel, YesNo>(),
            FirstName: src.FirstName,
            MiddleName: src.MiddleName,
            LastName: src.LastName,
            PhoneNum: src.PhoneNum,
            Street: src.Street,
            City: src.City,
            State: src.State,
            Zip: src.Zip
        );

    public static ParentInfoViewModel ToViewModel(this ParentInfo src) =>
        new()
        {
            ElectiveRequireValue = src.ElectiveRequireValue.MapEnum<YesNo, YesNoViewModel>(),
            FirstName = src.FirstName,
            MiddleName = src.MiddleName,
            LastName = src.LastName,
            PhoneNum = src.PhoneNum,
            Street = src.Street,
            City = src.City,
            State = src.State,
            Zip = src.Zip,
        };
}
