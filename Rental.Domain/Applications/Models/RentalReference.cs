using Rental.Domain.Enums;

namespace Rental.Domain.Applications.Models;

public record RentalReference(
    bool AllowElectiveRequire,
    string? ElectiveRequireDisplay,
    YesNo? ElectiveRequireValue,
    string? Street,
    string? City,
    string? State,
    string? Zip,
    string? LandlordName,
    string? LandlordPhoneNum,
    DateTime? Start,
    DateTime? End,
    string? ReasonForMoving
);
