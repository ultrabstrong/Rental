using ApartmentWeb.Enums;
using ApartmentWeb.Validation;
using System.ComponentModel.DataAnnotations;

namespace ApartmentWeb.Models.Application
{
    public class ParentInfo
    {
        public string DisplayName { get; set; }

        [Display(Name = "Do your parents pay some or all of your rent?")]
        [Range(1, 2, ErrorMessage = "Please indicate if your parents help pay your rent")]
        public YesNo ElectiveRequireValue { get; set; }

        [Display(Name = "First name")]
        [RequireIfEnum(nameof(ElectiveRequireValue), YesNo.Yes, "Please enter your parent's first name", null)]
        public string FirstName { get; set; }

        [Display(Name = "Middle name")]
        public string MiddleName { get; set; }

        [Display(Name = "Last name")]
        [RequireIfEnum(nameof(ElectiveRequireValue), YesNo.Yes, "Please enter your parent's last name", null)]
        public string LastName { get; set; }

        [Display(Name = "Phone #")]
        [RequireIfEnum(nameof(ElectiveRequireValue), YesNo.Yes, "Please enter your parent's phone number", null)]
        public string PhoneNum { get; set; }

        [Display(Name = "Address")]
        public Address Address { get; set; } = new Address();
    }
}
