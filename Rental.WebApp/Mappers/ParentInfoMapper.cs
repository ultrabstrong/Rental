using Rental.Domain.Applications;
using Rental.Domain.Enums;
using ParentInfoViewModel = Rental.WebApp.Models.Application.ParentInfo;
using YesNoViewModel = Rental.WebApp.Enums.YesNo;

namespace Rental.WebApp.Mappers;

public static class ParentInfoMapper
{
    public static ParentInfo ToDomainModel(this ParentInfoViewModel src) => new()
    {
        ElectiveRequireValue = src.ElectiveRequireValue.MapEnum<YesNoViewModel, YesNo>(),
        FirstName = src.FirstName,
        MiddleName = src.MiddleName,
        LastName = src.LastName,
        PhoneNum = src.PhoneNum,
        Street = src.Street,
        City = src.City,
        State = src.State,
        Zip = src.Zip
    };
}
