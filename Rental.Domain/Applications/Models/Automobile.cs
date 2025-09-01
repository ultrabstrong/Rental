using Rental.Domain.Enums;

namespace Rental.Domain.Applications.Models;

public record Automobile(
    bool AllowElectiveRequire,
    YesNo? ElectiveRequireValue,
    string? Make,
    string? Model,
    string? Year,
    string? State,
    string? LicenseNum,
    string? Color
);
