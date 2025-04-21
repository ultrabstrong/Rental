using System.ComponentModel.DataAnnotations;

namespace ApartmentWeb.Models.Application
{
    public class Address
    {
        [Display(Name = "Street")]
        //[RequiredIf("ElectiveRequireValue", YesNo.Yes, ErrorMessageResourceName = nameof(vrm.ADDRESS_STREET), ErrorMessageResourceType = typeof(vrm))]
        public string Street { get; set; }

        [Display(Name = "City")]
        //[RequiredIf("ElectiveRequireValue", YesNo.Yes, ErrorMessageResourceName = nameof(vrm.ADDRESS_CITY), ErrorMessageResourceType = typeof(vrm))]
        public string City { get; set; }

        [Display(Name = "State")]
        //[RequiredIf("ElectiveRequireValue", YesNo.Yes, ErrorMessageResourceName = nameof(vrm.ADDRESS_STATE), ErrorMessageResourceType = typeof(vrm))]
        public string State { get; set; }

        [Display(Name = "Zip")]
        //[RequiredIf("ElectiveRequireValue", YesNo.Yes, ErrorMessageResourceName = nameof(vrm.ADDRESS_ZIP), ErrorMessageResourceType = typeof(vrm))]
        public string Zip { get; set; }
    }
}
