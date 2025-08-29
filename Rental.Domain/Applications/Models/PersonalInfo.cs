namespace Rental.Domain.Applications.Models;

public class PersonalInfo
{
    public string FirstName { get; set; } = string.Empty;
    public string? MiddleName { get; set; }
    public string LastName { get; set; } = string.Empty;
    public string PhoneNum { get; set; } = string.Empty;
    public string SSN { get; set; } = string.Empty; // consider value object later
    public string DriverLicense { get; set; } = string.Empty;
    public string DriverLicenseStateOfIssue { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}
