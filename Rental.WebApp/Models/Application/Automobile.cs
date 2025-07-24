using Rental.WebApp.Enums;
using Rental.WebApp.Validation;
using System.ComponentModel.DataAnnotations;

namespace Rental.WebApp.Models.Application;

public class Automobile
{
    public string? DisplayName { get; set; }

    public bool AllowElectiveRequire { get; set; }

    [Display(Name = "Do you have an automobile?")]
    [Range(1, 2, ErrorMessage = "Please indicate if you have an automobile")]
    public YesNo ElectiveRequireValue { get; set; }

    [Display(Name = "Make")]
    [RequireIfEnum(nameof(ElectiveRequireValue), YesNo.Yes, "Please enter the make of your vehicle")]
    public string Make { get; set; } = string.Empty;

    [Display(Name = "Model")]
    [RequireIfEnum(nameof(ElectiveRequireValue), YesNo.Yes, "Please enter the model of your vehicle")]
    public string Model { get; set; } = string.Empty;

    [Display(Name = "Year")]
    [RequireIfEnum(nameof(ElectiveRequireValue), YesNo.Yes, "Please enter the year of your vehicle")]
    public string Year { get; set; } = string.Empty;

    [Display(Name = "State")]
    [RequireIfEnum(nameof(ElectiveRequireValue), YesNo.Yes, "Please select the registration state of your vehicle")]
    public string State { get; set; } = string.Empty;

    [Display(Name = "License Plate #")]
    [RequireIfEnum(nameof(ElectiveRequireValue), YesNo.Yes, "Please enter the license plate number of your vehicle")]
    public string LicenseNum { get; set; } = string.Empty;

    [Display(Name = "Color")]
    [RequireIfEnum(nameof(ElectiveRequireValue), YesNo.Yes, "Please enter the color of your vehicle")]
    public string Color { get; set; } = string.Empty;
}
