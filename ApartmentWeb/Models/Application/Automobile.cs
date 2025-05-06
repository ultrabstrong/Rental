using ApartmentWeb.Enums;
using ApartmentWeb.Validation;
using System.ComponentModel.DataAnnotations;

namespace ApartmentWeb.Models.Application
{
    public class Automobile
    {
        public string DisplayName { get; set; }

        public bool AllowElectiveRequire { get; set; }

        [Display(Name = "Do you have an automobile?")]
        [Range(1, 2, ErrorMessage = "Please indicate whether you have an automobile")]
        public YesNo ElectiveRequireValue { get; set; }

        [Display(Name = "Make")]
        [RequireIfEnum(nameof(ElectiveRequireValue), YesNo.Yes, "Please enter the make of your vehicle")]
        public string Make { get; set; }

        [Display(Name = "Model")]
        [RequireIfEnum(nameof(ElectiveRequireValue), YesNo.Yes, "Please enter the model of your vehicle")]
        public string Model { get; set; }

        [Display(Name = "Year")]
        [RequireIfEnum(nameof(ElectiveRequireValue), YesNo.Yes, "Please enter the year of your vehicle")]
        public string Year { get; set; }

        [Display(Name = "State")]
        [RequireIfEnum(nameof(ElectiveRequireValue), YesNo.Yes, "Please select the registration state of your vehicle")]
        public string State { get; set; }

        [Display(Name = "License Plate #")]
        [RequireIfEnum(nameof(ElectiveRequireValue), YesNo.Yes, "Please enter the license plate number of your vehicle")]
        public string LicenseNum { get; set; }

        [Display(Name = "Color")]
        [RequireIfEnum(nameof(ElectiveRequireValue), YesNo.Yes, "Please enter the color of your vehicle")]
        public string Color { get; set; }
    }
}
