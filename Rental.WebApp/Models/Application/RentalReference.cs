using Rental.WebApp.Enums;
using Rental.WebApp.Validation;
using System.ComponentModel.DataAnnotations;

namespace Rental.WebApp.Models.Application;

public class RentalReference
{
    public string? DisplayName { get; set; }

    public bool AllowElectiveRequire { get; set; }

    public string? ElectiveRequireDisplay { get; set; }

    [Range(1, 2, ErrorMessage = "Please indicate if you want to add a rental reference")]
    public YesNo ElectiveRequireValue { get; set; }

    [Display(Name = "Street")]
    [RequireIfEnum(nameof(ElectiveRequireValue), YesNo.Yes, "Please enter a street")]
    public string Street { get; set; } = string.Empty;

    [Display(Name = "City")]
    [RequireIfEnum(nameof(ElectiveRequireValue), YesNo.Yes, "Please enter a city")]
    public string City { get; set; } = string.Empty;

    [Display(Name = "State")]
    [RequireIfEnum(nameof(ElectiveRequireValue), YesNo.Yes, "Please enter a state")]
    public string State { get; set; } = string.Empty;

    [Display(Name = "Zip")]
    [RequireIfEnum(nameof(ElectiveRequireValue), YesNo.Yes, "Please enter a zip code")]
    public string Zip { get; set; } = string.Empty;

    [Display(Name = "Landlord name")]
    [RequireIfEnumEnabled(nameof(ElectiveRequireValue), YesNo.Yes, nameof(AllowElectiveRequire), "Please enter the landlord's name")]
    public string LandlordName { get; set; } = string.Empty;

    [Display(Name = "Landlord phone #")]
    [RequireIfEnumEnabled(nameof(ElectiveRequireValue), YesNo.Yes, nameof(AllowElectiveRequire), "Please enter the landlord's phone number")]
    public string LandlordPhoneNum { get; set; } = string.Empty;

    [Display(Name = "From")]
    [RequireIfEnum(nameof(ElectiveRequireValue), YesNo.Yes, "Please enter the start date")]
    [DataType(DataType.Date)]
    public DateTime? Start { get; set; }

    [Display(Name = "To")]
    [RequireIfEnum(nameof(ElectiveRequireValue), YesNo.Yes, "Please enter the end date")]
    [DataType(DataType.Date)]
    public DateTime? End { get; set; }

    [Display(Name = "Reason for moving")]
    [RequireIfEnum(nameof(ElectiveRequireValue), YesNo.Yes, "Please enter your reason for moving")]
    public string ReasonForMoving { get; set; } = string.Empty;
}
