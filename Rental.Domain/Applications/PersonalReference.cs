using Rental.Domain.Enums;

namespace Rental.Domain.Applications;

public class PersonalReference
{
    public bool AllowElectiveRequire { get; set; }
    public string? ElectiveRequireDisplay { get; set; }
    public YesNo? ElectiveRequireValue { get; set; }
    public string? Name { get; set; }
    public string? Relationship { get; set; }
    public string? PhoneNum { get; set; }
}
