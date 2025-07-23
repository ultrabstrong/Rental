using System.ComponentModel.DataAnnotations;

namespace ApartmentWeb.Models.Application;

public class PersonalInfo
{
    public string? DisplayName { get; set; }

    public bool AllowElectiveRequire { get; set; }

    public string? ElectiveRequireDisplay { get; set; }

    [Display(Name = "First name")]
    [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter your first name")]
    public string FirstName { get; set; }

    [Display(Name = "Middle name")]
    public string? MiddleName { get; set; }

    [Display(Name = "Last name")]
    [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter your last name")]
    public string LastName { get; set; }

    [Display(Name = "Phone #")]
    [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter your phone number")]
    public string PhoneNum { get; set; }

    [Display(Name = "SSN")]
    [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter your SSN")]
    public string SSN { get; set; }

    [Display(Name = "Driver license #")]
    [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter your driver license number")]
    public string DriverLicense { get; set; }

    [Display(Name = "State of issue")]
    [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter the state that issued your driver license")]
    public string DriverLicenseStateOfIssue { get; set; }

    [Display(Name = "Email")]
    [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter your email address")]
    public string Email { get; set; }
}
