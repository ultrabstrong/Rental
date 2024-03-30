using ApartmentWeb.Enums;
using ApartmentWeb.Validation;
using System.ComponentModel.DataAnnotations;
using rm = Resources.WebsiteModels.Application;
using vrm = Resources.WebsiteModels.ApplicationValidation;

namespace ApartmentWeb.Models.Application
{
    public class PersonalReference
    {
        public string DisplayName { get; set; }

        public bool AllowElectiveRequire { get; set; }

        public string ElectiveRequireDisplay { get; set; }

        [EnumDataType(typeof(YesNo))]
        [Range(1, 2, ErrorMessageResourceName = nameof(vrm.APP_ADD_PERSONAL_REF), ErrorMessageResourceType = typeof(vrm))]
        public YesNo ElectiveRequireValue { get; set; }

        [Display(Name = nameof(rm.REF_NAME), ResourceType = typeof(rm))]
        [RequireIfEnum(nameof(ElectiveRequireValue), YesNo.Yes, nameof(vrm.REF_NAME), typeof(vrm))]
        public string Name { get; set; }

        [Display(Name = nameof(rm.REF_RELATION), ResourceType = typeof(rm))]
        [RequireIfEnum(nameof(ElectiveRequireValue), YesNo.Yes, nameof(vrm.REF_NAME), typeof(vrm))]
        public string Relationship { get; set; }

        [Display(Name = nameof(rm.REF_PHONE), ResourceType = typeof(rm))]
        [RequireIfEnum(nameof(ElectiveRequireValue), YesNo.Yes, nameof(vrm.REF_NAME), typeof(vrm))]
        public string PhoneNum { get; set; }

    }
}
