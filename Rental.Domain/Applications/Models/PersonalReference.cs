using Rental.Domain.Enums;

namespace Rental.Domain.Applications.Models;

public record PersonalReference(
    bool AllowElectiveRequire,
    string? ElectiveRequireDisplay,
    YesNo? ElectiveRequireValue,
    string? Name,
    string? Relationship,
    string? PhoneNum
);
