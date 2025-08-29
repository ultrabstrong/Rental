using Rental.Domain.Enums;

namespace Rental.Domain.Applications;

public class EmploymentInfo
{
    public bool AllowElectiveRequire { get; set; }
    public string? ElectiveRequireDisplay { get; set; }
    public bool HasEmployment => ElectiveRequireValue == YesNo.Yes;
    public YesNo? ElectiveRequireValue { get; set; }
    public string? Company { get; set; }
    public string? ContactName { get; set; }
    public string? ContactPhone { get; set; }
    public string? EmploymentLength { get; set; }
    public YesNo? IsPermenant { get; set; }
    public WageType? WageType { get; set; }
    public decimal? Wage { get; set; }
    public int? HoursPerWeek { get; set; }
}
