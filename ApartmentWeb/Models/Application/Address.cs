using System.ComponentModel.DataAnnotations;

namespace ApartmentWeb.Models.Application
{
    public class Address
    {
        [Display(Name = "Street")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter a street")]
        public string Street { get; set; }

        [Display(Name = "City")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter a city")]
        public string City { get; set; }

        [Display(Name = "State")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please select a state")]
        public string State { get; set; }

        [Display(Name = "Zip")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter a zip code")]
        public string Zip { get; set; }
    }
}
