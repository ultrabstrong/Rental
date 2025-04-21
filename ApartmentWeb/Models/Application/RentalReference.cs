using ApartmentWeb.Enums;
using ApartmentWeb.Validation;
using System;
using System.ComponentModel.DataAnnotations;

namespace ApartmentWeb.Models.Application
{
    public class RentalReference
    {
        public string DisplayName { get; set; }

        public bool AllowElectiveRequire { get; set; }

        public string ElectiveRequireDisplay { get; set; }

        [Range(1, 2, ErrorMessage = "Please indicate if you want to add a rental reference")]
        public YesNo ElectiveRequireValue { get; set; }

        [Display(Name = "Address")]
        [RequireIfEnum(nameof(ElectiveRequireValue), YesNo.Yes, "Please enter the address", null)]
        public Address Address { get; set; } = new Address();

        [Display(Name = "Landlord name")]
        [RequireIfEnum(nameof(ElectiveRequireValue), YesNo.Yes, "Please enter the landlord's name", null)]
        public string LandlordName { get; set; }

        [Display(Name = "Landlord phone #")]
        [RequireIfEnum(nameof(ElectiveRequireValue), YesNo.Yes, "Please enter the landlord's phone number", null)]
        public string LandlordPhoneNum { get; set; }

        [Display(Name = "From")]
        [RequireIfEnum(nameof(ElectiveRequireValue), YesNo.Yes, "Please enter the start date", null)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime? Start { get; set; }

        [Display(Name = "To")]
        [RequireIfEnum(nameof(ElectiveRequireValue), YesNo.Yes, "Please enter the end date", null)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime? End { get; set; }

        [Display(Name = "Reason for moving")]
        [RequireIfEnum(nameof(ElectiveRequireValue), YesNo.Yes, "Please enter your reason for moving", null)]
        public string ReasonForMoving { get; set; }
    }
}
