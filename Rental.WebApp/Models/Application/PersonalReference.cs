using System.ComponentModel.DataAnnotations;
using Rental.WebApp.Enums;
using Rental.WebApp.Validation;

namespace Rental.WebApp.Models.Application;

public class PersonalReference
{
    public string? DisplayName { get; set; }

    public bool AllowElectiveRequire { get; set; }

    public string? ElectiveRequireDisplay { get; set; }

    [Required(ErrorMessage = "Please indicate if you want to add a personal reference")]
    public YesNo? ElectiveRequireValue { get; set; }

    [Display(Name = "Name")]
    [RequireIfEnum(nameof(ElectiveRequireValue), YesNo.Yes, "Please enter the reference's name")]
    public string? Name { get; set; }

    [Display(Name = "Relationship")]
    [RequireIfEnum(
        nameof(ElectiveRequireValue),
        YesNo.Yes,
        "Please enter your relationship to this reference"
    )]
    public string? Relationship { get; set; }

    [Display(Name = "Phone #")]
    [RequireIfEnum(
        nameof(ElectiveRequireValue),
        YesNo.Yes,
        "Please enter the reference's phone number"
    )]
    public string? PhoneNum { get; set; }
}
