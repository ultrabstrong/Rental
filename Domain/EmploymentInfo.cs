using Domain.Enums;
using Domain.Validation;
using System.ComponentModel.DataAnnotations;
using rm = Resources.Domain.Application;
using vrm = Resources.Domain.ApplicationValidation;

namespace Domain
{
    public class EmploymentInfo
    {
        public string DisplayName { get; set; }

        public bool AllowElectiveRequire { get; set; }

        public string ElectiveRequireDisplay { get; set; }

        [Range(1, 2, ErrorMessageResourceName = nameof(vrm.APP_HAS_SECOND_JOB), ErrorMessageResourceType = typeof(vrm))]
        public YesNo ElectiveRequireValue { get; set; }

        [Display(Name = nameof(rm.EMPLOY_COMPANY), ResourceType = typeof(rm))]
        [RequireIfEnum(nameof(ElectiveRequireValue), YesNo.Yes, nameof(vrm.EMPLOY_COMPANY), typeof(vrm))]
        public string Company { get; set; }

        [Display(Name = nameof(rm.EMPLOY_CONTACT), ResourceType = typeof(rm))]
        [RequireIfEnum(nameof(ElectiveRequireValue), YesNo.Yes, nameof(vrm.EMPLOY_CONTACT), typeof(vrm))]
        public string ContactName { get; set; }

        [Display(Name = nameof(rm.EMPLOY_PHONE), ResourceType = typeof(rm))]
        [RequireIfEnum(nameof(ElectiveRequireValue), YesNo.Yes, nameof(vrm.EMPLOY_PHONE), typeof(vrm))]
        public string ContactPhone { get; set; }

        [Display(Name = nameof(rm.EMPLOY_LENGTH), ResourceType = typeof(rm))]
        [RequireIfEnum(nameof(ElectiveRequireValue), YesNo.Yes, nameof(vrm.EMPLOY_LENGTH), typeof(vrm))]
        public string EmploymentLength { get; set; }

        [Display(Name = nameof(rm.EMPLOY_PERMENANT), ResourceType = typeof(rm))]
        [RangeIfEnum("1", 0, "2", nameof(ElectiveRequireValue), YesNo.Yes, nameof(vrm.EMPLOY_PERMENANT), typeof(vrm))]
        public YesNo IsPermenant { get; set; }

        [Display(Name = nameof(rm.EMPLOY_WAGE_TYPE), ResourceType = typeof(rm))]
        [RangeIfEnum("1", 0, "2", nameof(ElectiveRequireValue), YesNo.Yes, nameof(vrm.EMPLOY_WAGE_TYPE), typeof(vrm))]
        public WageType WageType { get; set; }

        [Display(Name = nameof(rm.EMPLOY_WAGE), ResourceType = typeof(rm))]
        [RangeIfEnum("0.01", 2, nameof(ElectiveRequireValue), YesNo.Yes, nameof(vrm.EMPLOY_WAGE), typeof(vrm))]
        [DataType(DataType.Currency)]
        public double Wage { get; set; }

        [Display(Name = nameof(rm.EMPLOY_HOURS), ResourceType = typeof(rm))]
        [RequireIfEnum(nameof(ElectiveRequireValue), YesNo.Yes, nameof(vrm.EMPLOY_HOURS), typeof(vrm))]
        [RangeIfEnum("1", 0, nameof(ElectiveRequireValue), YesNo.Yes, nameof(vrm.EMPLOY_HOURS), typeof(vrm))]
        public int HoursPerWeek { get; set; }

    }
}
