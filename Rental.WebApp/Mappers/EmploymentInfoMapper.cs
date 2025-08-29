using Rental.Domain.Applications.Models;
using Rental.Domain.Enums;
using EmploymentInfoViewModel = Rental.WebApp.Models.Application.EmploymentInfo;
using WageTypeViewModel = Rental.WebApp.Enums.WageType;
using YesNoViewModel = Rental.WebApp.Enums.YesNo;

namespace Rental.WebApp.Mappers;

public static class EmploymentInfoMapper
{
    public static EmploymentInfo ToDomainModel(this EmploymentInfoViewModel src) => new()
    {
        AllowElectiveRequire = src.AllowElectiveRequire,
        ElectiveRequireDisplay = src.ElectiveRequireDisplay,
        ElectiveRequireValue = src.ElectiveRequireValue.MapEnum<YesNoViewModel, YesNo>(),
        Company = src.Company,
        ContactName = src.ContactName,
        ContactPhone = src.ContactPhone,
        EmploymentLength = src.EmploymentLength,
        IsPermenant = src.IsPermenant.MapEnum<YesNoViewModel, YesNo>(),
        WageType = src.WageType.MapEnum<WageTypeViewModel, WageType>(),
        Wage = src.Wage,
        HoursPerWeek = src.HoursPerWeek
    };
}
