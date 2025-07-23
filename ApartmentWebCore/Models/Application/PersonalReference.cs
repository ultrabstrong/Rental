using ApartmentWeb.Enums;
using ApartmentWeb.Validation;
using System.ComponentModel.DataAnnotations;

namespace ApartmentWeb.Models.Application;

public class PersonalReference
{
    public string? DisplayName { get; set; }

    public bool AllowElectiveRequire { get; set; }

    public string? ElectiveRequireDisplay { get; set; }

    [Range(1, 2, ErrorMessage = "Please indicate if you want to add a personal reference")]
    public YesNo ElectiveRequireValue { get; set; }

    [Display(Name = "Name")]
    [RequireIfEnum(nameof(ElectiveRequireValue), YesNo.Yes, "Please enter the reference's name")]
    public string Name { get; set; }

    [Display(Name = "Relationship")]
    [RequireIfEnum(nameof(ElectiveRequireValue), YesNo.Yes, "Please enter your relationship to this reference")]
    public string Relationship { get; set; }

    [Display(Name = "Phone #")]
    [RequireIfEnum(nameof(ElectiveRequireValue), YesNo.Yes, "Please enter the reference's phone number")]
    public string PhoneNum { get; set; }
}
