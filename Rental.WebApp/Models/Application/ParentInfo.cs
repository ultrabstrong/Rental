using System.ComponentModel.DataAnnotations;
using Rental.WebApp.Enums;
using Rental.WebApp.Validation;

namespace Rental.WebApp.Models.Application;

public class ParentInfo
{
    public string? DisplayName { get; set; }

    [Display(Name = "Do your parents pay some or all of your rent?")]
    [Required(ErrorMessage = "Please indicate if your parents help pay your rent")]
    public YesNo? ElectiveRequireValue { get; set; }

    [Display(Name = "First name")]
    [RequireIfEnum(
        nameof(ElectiveRequireValue),
        YesNo.Yes,
        "Please enter your parent's first name"
    )]
    public string? FirstName { get; set; }

    [Display(Name = "Middle name")]
    public string? MiddleName { get; set; }

    [Display(Name = "Last name")]
    [RequireIfEnum(nameof(ElectiveRequireValue), YesNo.Yes, "Please enter your parent's last name")]
    public string? LastName { get; set; }

    [Display(Name = "Phone #")]
    [RequireIfEnum(
        nameof(ElectiveRequireValue),
        YesNo.Yes,
        "Please enter your parent's phone number"
    )]
    public string? PhoneNum { get; set; }

    [Display(Name = "Street")]
    [RequireIfEnum(nameof(ElectiveRequireValue), YesNo.Yes, "Please enter a street")]
    public string? Street { get; set; }

    [Display(Name = "City")]
    [RequireIfEnum(nameof(ElectiveRequireValue), YesNo.Yes, "Please enter a city")]
    public string? City { get; set; }

    [Display(Name = "State")]
    [RequireIfEnum(nameof(ElectiveRequireValue), YesNo.Yes, "Please enter a state")]
    public string? State { get; set; }

    [Display(Name = "Zip")]
    [RequireIfEnum(nameof(ElectiveRequireValue), YesNo.Yes, "Please enter a zip code")]
    public string? Zip { get; set; }
}
