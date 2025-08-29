using Rental.Domain.Enums;

namespace Rental.Domain.Applications;

public class Automobile
{
    public bool AllowElectiveRequire { get; set; }
    public YesNo? ElectiveRequireValue { get; set; }
    public string? Make { get; set; }
    public string? Model { get; set; }
    public string? Year { get; set; }
    public string? State { get; set; }
    public string? LicenseNum { get; set; }
    public string? Color { get; set; }
}
