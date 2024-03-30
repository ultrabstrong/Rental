using ApartmentWeb.Enums;
using ApartmentWeb.Validation;
using System.ComponentModel.DataAnnotations;
using rm = Resources.WebsiteModels.Application;
using vrm = Resources.WebsiteModels.ApplicationValidation;

namespace ApartmentWeb.Models.Application
{
    public class ParentInfo
    {
        public string DisplayName { get; set; }

        [Display(Name = nameof(rm.APP_PARENTS_PAY), ResourceType = typeof(rm))]
        [Range(1, 2, ErrorMessageResourceName = nameof(vrm.APP_PARENTS_PAY), ErrorMessageResourceType = typeof(vrm))]
        public YesNo ElectiveRequireValue { get; set; }

        [Display(Name = nameof(rm.PARENT_FIRSTNAME), ResourceType = typeof(rm))]
        [RequireIfEnum(nameof(ElectiveRequireValue), YesNo.Yes, nameof(vrm.PARENT_FIRSTNAME), typeof(vrm))]
        public string FirstName { get; set; }

        [Display(Name = nameof(rm.PARENT_MIDDLENAME), ResourceType = typeof(rm))]
        //[RequireIfEnum(nameof(ElectiveRequireValue), YesNo.Yes, nameof(vrm.PARENT_MIDDLENAME), typeof(vrm))]
        public string MiddleName { get; set; }

        [Display(Name = nameof(rm.PARENT_LASTNAME), ResourceType = typeof(rm))]
        [RequireIfEnum(nameof(ElectiveRequireValue), YesNo.Yes, nameof(vrm.PARENT_LASTNAME), typeof(vrm))]
        public string LastName { get; set; }

        [Display(Name = nameof(rm.PARENT_PHONENUM), ResourceType = typeof(rm))]
        [RequireIfEnum(nameof(ElectiveRequireValue), YesNo.Yes, nameof(vrm.PARENT_PHONENUM), typeof(vrm))]
        public string PhoneNum { get; set; }

        [Display(Name = nameof(rm.PARENT_ADDRESS), ResourceType = typeof(rm))]
        public Address Address { get; set; } = new Address();
    }
}
