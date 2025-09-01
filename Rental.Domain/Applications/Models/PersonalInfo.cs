namespace Rental.Domain.Applications.Models;

public record PersonalInfo(
    string FirstName,
    string? MiddleName,
    string LastName,
    string PhoneNum,
    string SSN,
    string DriverLicense,
    string DriverLicenseStateOfIssue,
    string Email
);
