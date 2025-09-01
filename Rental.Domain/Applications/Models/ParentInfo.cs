using Rental.Domain.Enums;

namespace Rental.Domain.Applications.Models;

public record ParentInfo(
    YesNo? ElectiveRequireValue,
    string? FirstName,
    string? MiddleName,
    string? LastName,
    string? PhoneNum,
    string? Street,
    string? City,
    string? State,
    string? Zip
);
