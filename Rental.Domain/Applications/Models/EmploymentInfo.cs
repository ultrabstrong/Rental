using Rental.Domain.Enums;

namespace Rental.Domain.Applications.Models;

public record EmploymentInfo(
    bool AllowElectiveRequire,
    string? ElectiveRequireDisplay,
    YesNo? ElectiveRequireValue,
    string? Company,
    string? ContactName,
    string? ContactPhone,
    string? EmploymentLength,
    YesNo? IsPermenant,
    WageType? WageType,
    decimal? Wage,
    int? HoursPerWeek
)
{
    public bool HasEmployment => ElectiveRequireValue == YesNo.Yes;
}
